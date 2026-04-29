using System.ComponentModel.DataAnnotations;

namespace OffinicinaShared
{
    public class ClienteDTO
    {
        public string? Codice { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Il nome può contenere solo lettere")] // blocco caratteri speciali SQL
        public string Nome { get; set; } = null!;

        [Required]
        public string Cognome { get; set; } = null!;

        [Phone]
        public string? Telefono { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Formato email non valido")]
        public string? Email { get; set; }

        public string? Indirizzo { get; set; }
        public List<string> Veicoli { get; set; } = new();

        // campo nuovo usato per la creazione rapida di un cliente con un veicolo già associato, così da non dover prima creare il cliente e poi modificare i dati per aggiungere la targa
        [RegularExpression(@"^[A-Z0-9]*$", ErrorMessage = "Targa non valida")]
        public string? NuovaTarga { get; set; }
    }
}
