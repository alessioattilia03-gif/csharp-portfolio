using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Officina.API.Models;

public partial class Utente
{
    [Column("UtenteID")]
    public int UtenteId { get; set; }

    public string CodicePub { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Ruolo { get; set; } = null!;
    public string? Email { get; set; } 
    public string? Telefono { get; set; }
    
}