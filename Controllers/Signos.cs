using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Q1.Models;
using System.Diagnostics;
using System.Numerics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Q1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Signos : ControllerBase
    {
        SqlConnection Conexao = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Signos;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        // GET: api/<Signos>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        [HttpGet]
        public List<Signo> GetFrase()
        {


            List<Signo> signos = new List<Signo>();
            using (Conexao)
            {
                Conexao.Open();

                //string insertQuery = "INSERT INTO planos (tipo) VALUES (@Tipo)";

                //string[] tipos = { "bronze", "prata", "ouro" };

                //foreach (string tipo in tipos)
                //{
                //    using (SqlCommand command = new SqlCommand(insertQuery, Conexao))
                //    {
                //        command.Parameters.AddWithValue("@Tipo", tipo);
                //        command.ExecuteNonQuery();
                //    }
                //}


                string stmt = "SELECT * FROM dbo.signos";
                using (SqlCommand comando = new SqlCommand(stmt, Conexao))
                {
                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            Signo signo = new Signo();
                            signo.Id = leitor.GetInt32(0);
                            signo.SignoT = leitor.GetString(1);
                            signo.DataInicial = leitor.GetDateTime(2);
                            signo.DataFinal = leitor.GetDateTime(3);
                            signo.Planeta = leitor.GetString(4);
                            signos.Add(signo);
                        }
                    }
                }
            }
            Conexao.Close();
            return signos;
        }
        // GET api/<Signos>/5
        [HttpGet("{id}")]
        public Signo Get(int id)
        {
            Signo signo = new Signo();
            using (Conexao)
            {
                Conexao.Open(); 

                string stmt = "SELECT * FROM frases WHERE id=@id";
                using (SqlCommand comando = new SqlCommand(stmt, Conexao))
                {
                    comando.Parameters.Add(new SqlParameter("@id", id));

                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            signo.Id = leitor.GetInt32(0);
                            signo.SignoT = leitor.GetString(1);
                            signo.DataInicial = leitor.GetDateTime(2);
                            signo.DataFinal = leitor.GetDateTime(3);
                            signo.Planeta = leitor.GetString(4);
                        }
                    }
                }
                Conexao.Close();

            }
            return signo;
        }

        // POST api/<Signos>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Signos>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Usuario usuario)
        {
            using (Conexao)
            {
                Conexao.Open();

                string insertQuery = "UPDATE usuarios SET (Nome = @Nome)WHERE id = @id";


                using (SqlCommand command = new SqlCommand(insertQuery, Conexao))
                {
                    command.Parameters.AddWithValue("@Nome", usuario.Nome); 
                    command.ExecuteNonQuery();
                }


            }
            Conexao.Close(); return;
        }

        // DELETE api/<Signos>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {



            await Conexao.OpenAsync();

            var Cmd = Conexao.CreateCommand();
            Cmd.CommandText = "DELETE FROM signos WHERE id=@id";
            Cmd.Parameters.Add(new SqlParameter("id", id));
            await Cmd.ExecuteNonQueryAsync();
            await Conexao.CloseAsync();

            return Ok();
        }
    }
}
