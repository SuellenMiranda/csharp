using System.Numerics;

namespace Q1.Models
{

    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public DateTime DataBirth { get; set; }
        public int SignoId { get; set; }
        public int PlanoId { get; set; }
    }

}
