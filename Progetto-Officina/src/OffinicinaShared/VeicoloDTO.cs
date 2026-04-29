using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffinicinaShared
{
    public class VeicoloDTO
    {
        public string CodicePub { get; set; } = null!;

        [Required]
        [StringLength(7, MinimumLength = 7)]
        [RegularExpression(@"^[A-Z0-9]*$", ErrorMessage = "Targa non valida")] // protezione specifica
        public string Targa { get; set; } = null!;

        [Required]
        public string Marca { get; set; } = null!;

        [Required]
        public string Modello { get; set; } = null!;

        public int? Anno { get; set; }

        // essenziale il riferimento al proprietario tramite il SUO CodicePub
        [Required(ErrorMessage = "Il proprietario è obbligatorio")]
        public string ClienteCodicePub { get; set; } = null!;
    }
}
