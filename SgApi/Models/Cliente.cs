using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SgApi.Models
{
    public class Cliente
    {
        [Key]       
        public int IdCliente { get; set; }
        public string RazonSocial { get; set; }
        public int? Celular {  get; set; }
    }
}
