using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Acreditacion
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid InscripcionId { get; set; }
        
        [Required]
        public DateTime FechaHoraIngreso { get; set; }
        
        [Required]
        public Guid AcreditadoPorUsuarioId { get; set; }
    }
}