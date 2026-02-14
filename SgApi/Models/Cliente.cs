using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SgApi.Models
{
    public enum CondicionIva
    {
        ConsumidorFinal = 1,
        ResponsableInscripto = 2,
        Monotributista = 3,
        Exento = 4,
        NoResponsable = 5
    }

    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        [Required]
        [MaxLength(150)]
        public string RazonSocial { get; set; } = null!;

        [MaxLength(30)]
        public string? Celular { get; set; }

        [MaxLength(20)]
        public string? DniCuit { get; set; }

        [MaxLength(150)]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? Direccion { get; set; }

        [MaxLength(120)]
        public string? Localidad { get; set; }

        [MaxLength(200)]
        public string? Observaciones { get; set; }

        public CondicionIva CondicionIva { get; set; } = CondicionIva.ConsumidorFinal;

        // % de descuento (0 a 100)
        public decimal DescuentoPorc { get; set; } = 0m;

        // Saldo de cuenta corriente:
        // positivo = te deben / negativo = a favor del cliente (si querés manejarlo así)
        public decimal Saldo { get; set; } = 0m;

        // Límite de crédito (cuenta corriente). 0 = sin crédito (o podés usar null para "sin límite")
        public decimal CreditoLimite { get; set; } = 0m;

        public bool Activo { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
