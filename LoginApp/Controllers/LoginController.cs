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

        private async Task<ServiceResponse<LoginResult>> LoginTheUser(Login li) 
        {
            Console.WriteLine("LoginTheUser - LoginTheUser");

            try
            {
                var vUser = await ValidUser(li);
                if (vUser.Equals(true))
                {
                    return new ServiceResponse<LoginResult>() { Data = null, Message = "Login Invalid vUser.", Success = false };
                }
                return new ServiceResponse<LoginResult>() { Data = null, Message = "Login Invalid sign in information provided.", Success = false };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<LoginResult>() { Data = null, Message = "Login Error.<br />" + ex.Message, Success = false };
            }
        }

        private async Task<bool> ValidUser(Login li) 
        {
            return false;
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
