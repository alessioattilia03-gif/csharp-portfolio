using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffinicinaShared
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Ruolo { get; set; } = null!;
    }
}
