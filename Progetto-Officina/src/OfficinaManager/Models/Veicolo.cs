using System;
using System.Collections.Generic;

namespace Officina.API.Models;

public partial class Veicolo
{
    public int VeicoloId { get; set; }

    public string CodicePub { get; set; } = null!;

    public string Targa { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public string Modello { get; set; } = null!;

    public int? Anno { get; set; }

    public int ClienteId { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<Intervento> Interventos { get; set; } = new List<Intervento>();
}
