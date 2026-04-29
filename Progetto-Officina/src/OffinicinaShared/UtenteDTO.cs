using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffinicinaShared
{
    public class UtenteDTO
    {
        public string? CodicePub { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Ruolo { get; set; } = null!;

        // non sono presenti in Utente, ma sono necessari per la registrazione e il login
        public string? Email { get; set; } = string.Empty; 
        public string? Password { get; set; }
        public string? Telefono { get; set; }
    }
}
