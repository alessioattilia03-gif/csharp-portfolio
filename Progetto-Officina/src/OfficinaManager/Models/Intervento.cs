using System;
using System.Collections.Generic;

namespace Officina.API.Models;

public partial class Intervento
{
    public int InterventoId { get; set; }

    public string CodicePub { get; set; } = null!;

    public string Descrizione { get; set; } = null!;

    public DateTime DataIngresso { get; set; }

    public DateTime? DataFine { get; set; }

    public decimal? Prezzo { get; set; }

    public string? Stato { get; set; }

    public int VeicoloId { get; set; }

    public virtual Veicolo Veicolo { get; set; } = null!;
}
