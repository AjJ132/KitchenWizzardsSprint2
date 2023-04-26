using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProServ.models
{
    public class LoginCredentials
    {
        [PrimaryKey, AutoIncrement]
        public int index { get; set; }

        public int userId { get; set; }

       
        [DisallowNull]
        public string password { get; set; }


        public LoginCredentials() { }

        public LoginCredentials(int userID, string ps)
        {
            this.userId = userID;
            this.password = ps;
        }

        public LoginCredentials(string ps)
        {
            this.password = ps;
        }

        public LoginCredentials(bool createGeneric)
        {
            if (createGeneric)
            {
                this.userId = 0;
                this.password = "Default";
            }
        }
    }
}
