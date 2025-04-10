using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginData
{
    public class LoginResult
    {
        #region Properties

        public LoginUser? UserInfo { get; set; } = null;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; } = DateTime.Now;
        public string TFToken { get; set; } = string.Empty;

        #endregion

        #region Constructors

        public LoginResult()
        {
            this.UserInfo = null;
            this.Token = string.Empty;
            this.ExpiresAt = DateTime.Now;
            this.TFToken = string.Empty;
        }

        public LoginResult(IdentityUser iu, int vectorsuid, string token, DateTime expiresAt, string tftoken) : base()
        {
            if (iu != null)
                this.UserInfo = new LoginUser(iu, vectorsuid);
            this.Token = token;
            this.ExpiresAt = expiresAt;

            if (!string.IsNullOrWhiteSpace(tftoken))
                this.TFToken = tftoken;

        }

        #endregion

        #region Functions
        #endregion  
    }
}
