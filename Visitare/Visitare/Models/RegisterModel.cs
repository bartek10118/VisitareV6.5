using System;
using System.Collections.Generic;
using System.Text;

namespace Visitare.Models
{
    class RegisterModel
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
