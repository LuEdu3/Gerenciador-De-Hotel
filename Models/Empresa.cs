using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorHotel.Models
{
    public class Empresa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
    public string? Nome { get; set; }

        [MaxLength(255)]
    public string? LogoUrl { get; set; }

        [MaxLength(100)]
    public string? Contato { get; set; }

        [MaxLength(100)]
    public string? Email { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

    public ICollection<EmpresaFoto>? Fotos { get; set; }
    }

    public class EmpresaFoto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
    public Empresa? Empresa { get; set; }

        [Required]
        [MaxLength(255)]
    public string? FotoUrl { get; set; }

        [MaxLength(255)]
    public string? Descricao { get; set; }
    }
}
