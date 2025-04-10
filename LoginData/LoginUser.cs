using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginData
{
    public class LoginUser
    {
        #region Properties

        public string ID { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public int VectorsUID { get; set; }

        #endregion

        #region Constructors

        public LoginUser()
        {
            this.ID = string.Empty;
            this.UserName = string.Empty;
            this.Email = string.Empty;
            this.EmailConfirmed = false;
            this.VectorsUID = 0;
        }

        public LoginUser(IdentityUser iu, int u1) : this()
        {
            if (iu != null)
            {
                this.ID = iu.Id;
                this.UserName = iu.UserName;
                this.Email = iu.Email;
                this.EmailConfirmed = iu.EmailConfirmed;

            }

            if (u1 > 0)
            {
                this.VectorsUID = u1;
            }
        }

        #endregion

        #region Functions
        #endregion 
    }
}
