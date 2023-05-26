using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using Q1.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Q1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Bichos : ControllerBase
    {
        SqlConnection Conexao = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Signos;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        // GET: api/<Bichos>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        [HttpGet]
        public List<Bicho> GetBichoS()
        {


            List<Bicho> bichos = new List<Bicho>();
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


                string stmt = "SELECT * FROM dbo.bichos";
                using (SqlCommand comando = new SqlCommand(stmt, Conexao))
                {
                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            Bicho bicho = new Bicho();
                            bicho.Id = leitor.GetInt32(0);
                            bicho.BichoT = leitor.GetString(1);
                            bichos.Add(bicho);
                        }
                    }
                }
            }
            Conexao.Close();
            return bichos;
        }
        // GET api/<Bichos>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            Bicho bicho = new Bicho();
            using (Conexao)
            {
                await Conexao.OpenAsync();




                string stmt = "SELECT * FROM bichos WHERE id=@id";
                using (SqlCommand comando = new SqlCommand(stmt, Conexao))
                {
                    comando.Parameters.Add(new SqlParameter("@id", id));

                    using (SqlDataReader leitor = await comando.ExecuteReaderAsync())
                    {
                        while (leitor.Read())
                        {
                            bicho.Id = leitor.GetInt32(0);
                            bicho.BichoT = leitor.GetString(1); 
                        }
                    }
                }
            }
            await Conexao.CloseAsync();
            return Ok(bicho);
        }

        // POST api/<Bichos>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string value)
        {
            return Ok();
        }

        // PUT api/<Bichos>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Bicho bicho)
        { 
            using (Conexao)
            {
                Conexao.Open();

                string insertQuery =  "UPDATE bichos SET bicho = @bicho WHERE id = @id";
                 
                  
                using (SqlCommand command = new SqlCommand(insertQuery, Conexao))
                {
                        command.Parameters.AddWithValue("@bicho", bicho.BichoT);
                        command.ExecuteNonQuery();
                } 

 
            }
            Conexao.Close(); 
            return Ok();
        }

        // DELETE api/<Bichos>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Conexao.OpenAsync();
            var Cmd = Conexao.CreateCommand();
            Cmd.CommandText = "DELETE FROM bichos WHERE id=@id";

            Cmd.Parameters.Add(new SqlParameter("id", id));
            await Cmd.ExecuteNonQueryAsync();
            await Conexao.CloseAsync();
             
            return Ok();
        }
    }
}
