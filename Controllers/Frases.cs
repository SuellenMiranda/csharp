using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Q1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Q1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Frases : ControllerBase
    {
        SqlConnection Conexao = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Signos;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        // GET: api/<Frases>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        [HttpGet]
        public async Task<ActionResult<List<Frase>>>  GetFrase()
        {


            List<Frase> frases = new List<Frase>();
            using (Conexao)
            {
                await Conexao.OpenAsync();

                string stmt = "SELECT * FROM dbo.frases";
                using (SqlCommand comando = new SqlCommand(stmt, Conexao))
                {
                    using (SqlDataReader leitor = await comando.ExecuteReaderAsync())
                    {
                        while (leitor.Read())
                        {
                            Frase frase = new Frase();
                            frase.Id = leitor.GetInt32(0);
                            frase.FraseT = leitor.GetString(1);
                            frase.SignoId = leitor.GetInt32(2);
                            frase.PlanoId = leitor.GetInt32(3);
                            frases.Add(frase);
                        }
                    }
                }
            }
            await Conexao.CloseAsync();

            return Ok(frases);
        }
        // GET api/<Frases>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Frase>> Get(int id)
        {
            Frase frase = new Frase();
            using (Conexao)
            {
                await Conexao.OpenAsync();




                string stmt = "SELECT * FROM frases WHERE id=@id";
                using (SqlCommand comando = new SqlCommand(stmt, Conexao))
                { 
                    comando.Parameters.Add(new SqlParameter("@id", id));

                    using (SqlDataReader leitor =  await comando.ExecuteReaderAsync())
                    {
                        while (leitor.Read())
                        { 
                            frase.Id = leitor.GetInt32(0);
                            frase.FraseT = leitor.GetString(1);
                            frase.SignoId = leitor.GetInt32(2);
                            frase.PlanoId = leitor.GetInt32(3);
                        }
                    }
                }
            } 
            await Conexao.CloseAsync();
            return Ok(frase);
        }
        [HttpGet("FrasesByplano")]
        public async Task<ActionResult<List<Frase>>> GetByP(int id)
        { 
            List<Frase> frases = new List<Frase>();
            using (Conexao)
            {
                await Conexao.OpenAsync();
                string stmt = "SELECT * FROM dbo.frases WHERE plano_id=@planoid";
                using (SqlCommand comando = new SqlCommand(stmt, Conexao))
                {
                    comando.Parameters.Add(new SqlParameter("@planoid", id));
                    using (SqlDataReader leitor = await comando.ExecuteReaderAsync())
                    {
                        while (leitor.Read())
                        {
                            Frase frase = new Frase();
                            frase.Id = leitor.GetInt32(0);
                            frase.FraseT = leitor.GetString(1);
                            frase.SignoId = leitor.GetInt32(2);
                            frase.PlanoId = leitor.GetInt32(3);
                            frases.Add(frase);
                        }
                    }
                } 
            }

            await Conexao.CloseAsync();
            return Ok(frases);
        }
        [HttpGet("FrasesByP{idp}&S{ids}")]
        public async Task<ActionResult<List<Frase>>> GetByPS(int idp, int ids)
        {
            List<Frase> frases = new List<Frase>();
            using (Conexao)
            {
                await Conexao.OpenAsync();
                string stmt = "SELECT * FROM dbo.frases WHERE plano_id=@planoid AND signo_id=@signo_id";
                using (SqlCommand comando = new SqlCommand(stmt, Conexao))
                {
                    comando.Parameters.Add(new SqlParameter("@planoid", idp));
                    comando.Parameters.Add(new SqlParameter("@signo_id", ids));
                    using (SqlDataReader leitor = await comando.ExecuteReaderAsync())
                    {
                        while (leitor.Read())
                        {
                            Frase frase = new Frase();
                            frase.Id = leitor.GetInt32(0);
                            frase.FraseT = leitor.GetString(1);
                            frase.SignoId = leitor.GetInt32(2);
                            frase.PlanoId = leitor.GetInt32(3);
                            frases.Add(frase);
                        }
                    }
                }
            }

            await Conexao.CloseAsync();
            return Ok(frases);
        }
        // POST api/<Frases>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Frases>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Frase frase)
        { 
                using (Conexao)
                {
                    Conexao.Open();

                    string insertQuery = "UPDATE frases SET (frase = @frase, signo_id = @signo_id )WHERE id = @id";


                    using (SqlCommand command = new SqlCommand(insertQuery, Conexao))
                    {
                        command.Parameters.AddWithValue("@frase", frase.FraseT);
                        command.Parameters.AddWithValue("@signo_id", frase.SignoId);
                    command.ExecuteNonQuery();
                    }


                }
                Conexao.Close(); return;
          
        }

        // DELETE api/<Frases>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        { 
             


            await Conexao.OpenAsync();

            var Cmd = Conexao.CreateCommand();
            Cmd.CommandText = "DELETE FROM frases WHERE id=@id";
            Cmd.Parameters.Add(new SqlParameter("id", id));
            await Cmd.ExecuteNonQueryAsync();
            await Conexao.CloseAsync();

            return Ok();
        }
    }
}
