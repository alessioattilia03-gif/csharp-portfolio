using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffinicinaShared
{
    public class InterventoDTO
    {
        public string? CodicePub { get; set; }

        public string Descrizione { get; set; } = null!;

        public DateTime DataIngresso { get; set; }

        public DateTime? DataFine { get; set; }

        public decimal? Prezzo { get; set; }

        public string? Stato { get; set; }

        // il backend recupererà il veicolo tramite la Targa
        public string? VeicoloCodicePub { get; set; }

        public string VeicoloInfo { get; set; } = string.Empty; // Es: "Fiat Panda"

        public string VeicoloTarga { get; set; } = string.Empty; // Es: "AB123CD"
    }
}