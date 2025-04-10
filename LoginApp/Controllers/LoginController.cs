using LoginApp.Code;
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
        #endregion

        #region Private Methods
        #endregion

    }
}
