using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Q1.Models;
using System.Net;
using System.Numerics;
using System.Text.Json;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Q1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Usuarios : ControllerBase
    {
        SqlConnection Conexao = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Signos;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>>   GetUsuarios()
        {
           

            List<Usuario> usuarios = new List<Usuario>(); 
            using (Conexao)
            {
                await Conexao.OpenAsync();

               

                string stmt = "SELECT * FROM dbo.Usuarios";
                using (SqlCommand comando = new SqlCommand(stmt, Conexao))
                {
                    using (SqlDataReader leitor = await comando.ExecuteReaderAsync())
                    {
                        while (leitor.Read())
                        {
                            Usuario usuario = new Usuario();
                            usuario.Id = leitor.GetInt32(0);
                            usuario.Nome = leitor.GetString(1);
                            usuario.Senha = leitor.GetString(2);
                            usuario.DataBirth = leitor.GetDateTime(3);
                            usuario.SignoId = leitor.GetInt32(4);
                            usuario.PlanoId = leitor.GetInt32(5);    
                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            await Conexao.CloseAsync();
            return Ok( usuarios);
        }
        // GET api/<Usuario>/
        [HttpGet("{usuario}&&{senha}")]
        public async Task<ActionResult<Usuario>>  GetSenha(string name, string senha)
        {


            Usuario usuario = new Usuario(); 
            string stmt = "SELECT * FROM usuarios WHERE nome=@nome AND  senha=@senha";
            using (Conexao)
            {
                await Conexao.OpenAsync();


                using (SqlCommand comando = new SqlCommand(stmt, Conexao))
                {
                    comando.Parameters.Add(new SqlParameter("name", name));
                    comando.Parameters.Add(new SqlParameter("senha", senha));
                    using (SqlDataReader leitor = await comando.ExecuteReaderAsync())
                    {
                        while (leitor.Read())
                        {
                            usuario.Id = leitor.GetInt32(0);
                            usuario.Nome = leitor.GetString(1);
                            usuario.Senha = leitor.GetString(2);
                            usuario.DataBirth = leitor.GetDateTime(3);
                            usuario.SignoId = leitor.GetInt32(4);
                            usuario.PlanoId = leitor.GetInt32(5);
                        }
                    }
                }
            }
            await Conexao.CloseAsync();
            return usuario;
        }

        [HttpGet("UserFrase")]
        public async Task<ActionResult<List<Frase>>> GetFraseUser([FromBody] Usuario usuario)
        { 
            Frases frases = new Frases(); 
             
            return Ok(frases.GetByPS(usuario.PlanoId,usuario.SignoId));
        }
        // POST api/<Usuario> 
        [HttpPost]
        public void Post([FromBody] Usuario usuario)
        {
            string insertQuery;
            bool check = false;
            if (usuario.Id == null || usuario.Id <= 0)
            {
                insertQuery = "INSERT INTO usuarios (nome,senha,data_birth,signo_id,plano_id) VALUES (@nome,@senha,@data_birth,@signo_id,@plano_id)";
                check = false;
            }
            else
            {
                insertQuery = "INSERT INTO usuarios (id,nome,senha,data_birth,signo_id,plano_id) VALUES (@Id,@nome,@senha,@data_birth,@signo_id,@plano_id)";

                check = true;
            }
            string selectQuery = "SELECT id FROM signos WHERE data_inicial <= @dataEspecifica AND data_final >= @dataEspecifica";
            using (Conexao)
            {
                Conexao.Open();
                using (SqlCommand command = new SqlCommand(selectQuery, Conexao))
                {
                command.Parameters.AddWithValue("@dataEspecifica", usuario.DataBirth);

                    using (SqlDataReader leitor = command.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            usuario.SignoId = leitor.GetInt32(0); 
                        }
                    }
                }
            
                 
                using (SqlCommand command = new SqlCommand(insertQuery, Conexao))
                {
                    if (check)
                    {
                        command.Parameters.AddWithValue("@Id", usuario.Id);
                    } 
                    command.Parameters.AddWithValue("@nome", usuario.Nome);
                    command.Parameters.AddWithValue("@senha", usuario.Senha);
                    command.Parameters.AddWithValue("@data_birth", usuario.DataBirth);
                    command.Parameters.AddWithValue("@signo_id", usuario.SignoId);
                    command.Parameters.AddWithValue("@plano_id", usuario.PlanoId); 
                    command.ExecuteNonQuery();
                }  
            }
            Conexao.Close();
        }
        //public async void CreateUsuarios(HttpListenerResponse response, HttpListenerRequest request)
        //{

        //    using (var reader = new StreamReader(request.InputStream))
        //    {
        //        var requestBody = reader.ReadToEnd();


        //        JsonSerializerOptions options = new JsonSerializerOptions
        //        {
        //            PropertyNameCaseInsensitive = true
        //        };

        //        Usuario plano = JsonSerializer.Deserialize<Plano>(requestBody, options);
        //        if (plano == null) { return; }
        //        var json = await Post(plano);

        //        response.ContentType = "application/json";
        //        response.StatusCode = (int)HttpStatusCode.Created;
        //        using (var writer = new StreamWriter(response.OutputStream))
        //        { 
        //            writer.Write(json);
        //        }
        //    }
        //}

        // DELETE api/<Usuario>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {



            await Conexao.OpenAsync();

            var Cmd = Conexao.CreateCommand();
            Cmd.CommandText = "DELETE FROM usuarios WHERE id=@id";
            Cmd.Parameters.Add(new SqlParameter("id", id));
            await Cmd.ExecuteNonQueryAsync();
            await Conexao.CloseAsync();

            return Ok();
        }
    }
}
