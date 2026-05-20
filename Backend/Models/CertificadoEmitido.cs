using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class CertificadoEmitido
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid InscripcionId { get; set; }

        [Required]
        public DateTime FechaEmision { get; set; }

        // Un código único alfanumérico para validar la autenticidad del certificado
        [Required]
        [StringLength(50)]
        public string CodigoVerificacion { get; set; } 
    }
}