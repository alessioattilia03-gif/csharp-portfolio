using System;
using System.Collections.Generic;

namespace Officina.API.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string CodicePub { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Cognome { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Indirizzo { get; set; }

    public virtual ICollection<Veicolo> Veicolos { get; set; } = new List<Veicolo>();
}
