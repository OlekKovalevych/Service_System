using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_System.Entity
{
    public class Admin : BaseEntity
    {
        public Admin() { }
        public Admin(int id, string login, string password)
        {
            SetId(id);
            this.Login = login;
            this.Password = password;
        }
        private string Login;

        private string Password;

        public string GetPassword() => Password;

        public void SetPassword(string password) => this.Password = password;

        public string GetLogin() => Login;

        public void SetLogin(string login) => this.Login = login;
    }
}
