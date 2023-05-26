using Microsoft.AspNetCore.Mvc; 
using System.Net.Http.Headers;
using System.Net;
using System.Text; 
using Q1.Models;
using System;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Numerics;

namespace Q1.Controllers
{

    [ApiController]
    [Route("api/[controller]")]


    public class Planos : Controller
    {

        SqlConnection Conexao = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Signos;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");




        [HttpGet]
        public async Task<ActionResult<List<Plano>>> GetPlanos()
        {



            List<Plano> planos = new List<Plano>();
            string stmt = "SELECT * FROM dbo.planos";
            using (Conexao)
            {
                Conexao.Open();
                using (SqlCommand comando = new SqlCommand(stmt, Conexao))
                {
                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            Plano plano = new Plano();
                            plano.Id = leitor.GetInt32(0);
                            plano.Tipo = leitor.GetString(1);
                            planos.Add(plano);
                        }
                    }
                }
            }
            Conexao.Close();
            return Ok(planos);
        }

        //public async Task<ActionResult<Plano>> AcionarPlano(Plano plano)
        //{
        //    using (var connection = new SqlConnection("ConnectionString"))
        //    {
        //        var query = "INSERT INTO planos (tipo) VALUES (@Tipo);";

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@Tipo", plano.Tipo);

        //            connection.Open();
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //    return Ok(plano);
        //}

        //public async void CreatePlano(HttpListenerResponse response, HttpListenerRequest request)
        //{

        //    using (var reader = new StreamReader(request.InputStream))
        //    {
        //        var requestBody = reader.ReadToEnd();


        //        JsonSerializerOptions options = new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        };

        //        Plano plano = JsonSerializer.Deserialize<Plano>(requestBody, options);
        //        if (plano == null) { return; }
        //        await AcionarPlano(plano);

        //        response.ContentType = "application/json";
        //        response.StatusCode = (int)HttpStatusCode.Created;
        //        using (var writer = new StreamWriter(response.OutputStream))
        //        {
        //            var json = AcionarPlano(plano);
        //            writer.Write(json);
        //        }
        //    }
        //}



        //[HttpPost]
        //public void PostPlanos(Plano plano)
        //{
        //    List<Plano> planos = new List<Plano>();
        //    string stmt = "INse * FROM dbo.Planos";
        //    Conexao.Open();
        //    using (SqlCommand comando = new SqlCommand(stmt, Conexao))
        //    {
        //        using (SqlDataReader leitor = comando.ExecuteReader())
        //        {
        //            while (leitor.Read())
        //            {
        //                Plano plano = new Plano();
        //                plano.Id = leitor.GetInt32(0);
        //                plano.Tipo = leitor.GetString(1);
        //                planos.Add(plano);
        //            }
        //        }
        //    }
        //    Conexao.Close();

        //    return;
        //}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Plano plano)
        {
            using (Conexao)
            {
               await Conexao.OpenAsync();

                string insertQuery = "UPDATE planos SET (tipo = @plano)WHERE id = @id";


                using (SqlCommand command = new SqlCommand(insertQuery, Conexao))
                {
                    command.Parameters.AddWithValue("@frase", plano.Tipo); 
                    command.ExecuteNonQuery();
                }


            }
            await Conexao.CloseAsync();
            Conexao.Close(); 
            return Ok();

        }

        [Route("&{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {



            await Conexao.OpenAsync();

            var Cmd = Conexao.CreateCommand();
            Cmd.CommandText = "DELETE FROM planos WHERE id=@id";
            Cmd.Parameters.Add(new SqlParameter("id", id));
            await Cmd.ExecuteNonQueryAsync();
            await Conexao.CloseAsync();

            return Ok();
        }





    }
}
