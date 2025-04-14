using LoginApp.Code;
using ServiceData;
using LoginData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoginApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region Properties

        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signinManager;
        private IConfiguration _configuration;
        private IHttpContextAccessor _ctx;
        private ApplicationDbContext _dbc;
        private DateTime MinDate;
        private DateTime MaxDate;

        #endregion

        #region Constructors

        public LoginController(ApplicationDbContext dbc, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager, IConfiguration configuration, IHttpContextAccessor ctx) 
        {
            this._dbc = dbc;
            this._userManager = userManager;
            this._signinManager = signinManager;
            this._configuration = configuration;
            this.MinDate = DateTime.Parse("01/01/1900 00:00:00");
            this.MaxDate = DateTime.Now.AddYears(10);
            this._ctx = ctx;
        }

        #endregion

        #region Public Methods

        [HttpPost("userlogin")]
        public async Task<ServiceResponse<LoginResult>> Login([FromBody] Login li) 
        {
            Console.WriteLine("Login - Login");

            return await LoginTheUser(li);
                        
        }

        [HttpPost("twostep")]
        public async Task<ServiceResponse<LoginResult>> LoginTwoStep([FromBody] Login li)
        {
            Console.WriteLine("Login - Login");

            return await LoginTheTwoStep(li);

        }

        #endregion

        #region Private Methods

        private async Task<ServiceResponse<LoginResult>> LoginTheUser([FromBody] Login li) 
        {
            Console.WriteLine("LoginTheUser - LoginTheUser");

            try
            {
                if (li == null)
                    return new ServiceResponse<LoginResult>() { Data = null, Message = "No login information provided.", Success = false };
                if (string.IsNullOrWhiteSpace(li.UserName))
                    return new ServiceResponse<LoginResult>() { Data = null, Message = "No user name, email address provided.", Success = false };
                if (string.IsNullOrWhiteSpace(li.Password))
                    return new ServiceResponse<LoginResult>() { Data = null, Message = "No password provided.", Success = false }; ;

                var user = await _userManager.FindByEmailAsync(li.UserName);
                if (user == null)
                    return new ServiceResponse<LoginResult>() { Data = null, Message = "Invalid user name, email address provided.", Success = false }; ;

                var passwordValid = await _userManager.CheckPasswordAsync(user, li.Password);
                if (!passwordValid)
                    return new ServiceResponse<LoginResult>() { Data = null, Message = "No password provided.", Success = false };


                await _signinManager.SignOutAsync();
                // Need to ensure set RequiresTwoFactor to true!!!!!
                // NotFiniteNumberException really, just need to set TwoFactorEnabled to true
                var result = await _signinManager.PasswordSignInAsync(user, li.Password, false, false);

                var expiresAt = DateTime.Now.AddMinutes(this._configuration.GetValue<int>("AppSettings:TokenExpires"));

                if (result.RequiresTwoFactor)
                {
                    var tftoken = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    // send code
                    if (!await SendCode(user, tftoken))
                        return new ServiceResponse<LoginResult>() { Data = null, Message = "Unable to send code for login.", Success = false };

                    return new ServiceResponse<LoginResult>() { Data = new LoginResult(user, await GetVectorsUID(user.Email), string.Empty, expiresAt, tftoken), Message = "", Success = true };
                }

                return new ServiceResponse<LoginResult>() { Data = null, Message = "Login Invalid sign in information provided.", Success = false };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<LoginResult>() { Data = null, Message = "Login Error.<br />" + ex.Message, Success = false };
            }
        }

        private async Task<bool> SendCode(IdentityUser user, string code)
        {
            var smtpServer = await GetVectorSettingString("OutgoingSmtpServer");
            var smtpUser = await GetVectorSettingString("Info_AICEmail");
            var smtpPassword = await GetSmtpEmailPwd("Info_AICEmailPwd");

            if (string.IsNullOrWhiteSpace(smtpServer) || string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPassword))
                return false;

            return Globals.SendEmail("Two Factor Code", code, user.Email, true, smtpServer, smtpUser, smtpPassword);
        }

        private async Task<string> GetVectorSettingString(string lookup)
        {
            var setting = string.Empty;

            var query = await _dbc.tblVectorsSettings.FirstOrDefaultAsync(X => X.Item == lookup);
            if (query != null)
                setting = query.ItemValue;

            return setting;
        }

        private async Task<string> GetSmtpEmailPwd(string lookup)
        {
            var pwd = string.Empty;

            var spdb = new AIC_DBContextProcedures(_dbc);
            var q = await spdb.aic_sp_GetEncrValue_tblVectorsSettingsAsync(lookup);
            if (q != null && q.Count > 0 && q[0] != null && q[0].ItemValue != string.Empty)
                pwd = q[0].ItemValue;
            spdb = null;

            return pwd;
        }

        private async Task<int> GetVectorsUID(string uid)
        {
            var vuid = 0;

            var query = await _dbc.tblAIC_Users_erasemes.FirstOrDefaultAsync(X => X.email == uid);
            if (query != null)
                vuid = query.VectorsUid;

            return vuid;
        }



        private async Task<ServiceResponse<LoginResult>> LoginTheTwoStep(Login li)
        {
            Console.WriteLine("LoginTheTwoStep - LoginTheTwoStep");

            try
            {
                return new ServiceResponse<LoginResult>() { Data = null, Message = "Login Two Step Invalid sign in information provided.", Success = false };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<LoginResult>() { Data = null, Message = "Login Two Step Error.<br />" + ex.Message, Success = false };
            }
        }

        #endregion

    }
}
