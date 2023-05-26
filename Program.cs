using Microsoft.Data.SqlClient;

namespace Q1
{
    public class Program
    {
        public static void Main(string[] args)
        {

            SqlConnection Conexao = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Signos;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
             
            int[] signosids = { 1,2,3,4,5,6,7,8,9,10,11,12};
            string[] signos = { "Áries", "Touro", "Gêmeos", "Câncer", "Leão", "Virgem", "Libra", "Escorpião", "Sagitário", "Capricórnio", "Aquário", "Peixes" };
            DateTime[] datasIniciais = { new DateTime(2022, 3, 21), new DateTime(2022, 4, 20), new DateTime(2022, 5, 21), new DateTime(2022, 6, 21), new DateTime(2022, 7, 23), new DateTime(2022, 8, 23), new DateTime(2022, 9, 23), new DateTime(2022, 10, 23), new DateTime(2022, 11, 22), new DateTime(2022, 12, 22), new DateTime(2022, 1, 20), new DateTime(2022, 2, 19) };
            DateTime[] datasFinais = { new DateTime(2022, 4, 19), new DateTime(2022, 5, 20), new DateTime(2022, 6, 20), new DateTime(2022, 7, 22), new DateTime(2022, 8, 22), new DateTime(2022, 9, 22), new DateTime(2022, 10, 22), new DateTime(2022, 11, 21), new DateTime(2022, 12, 21), new DateTime(2023, 1, 19), new DateTime(2023, 2, 18), new DateTime(2023, 3, 20) };
            string[] planetas = { "Marte", "Vênus", "Mercúrio", "Lua", "Sol", "Uranio", "Neturno", "Saturno", "Plutão", "Ceres", "Jupiter", "Terra" };

            int[] Uids = { 1, 2, 3, 4};
            string[] usuarios = { "João", "Maria", "Pedro", "Ana" };
            string[] senhas = { "123", "123", "123", "123" };
            DateTime[] datasNascimento = { new DateTime(1990, 1, 1), new DateTime(1995, 2, 2), new DateTime(1985, 3, 3), new DateTime(1992, 4, 4) };
            int[] signoIds = { 1, 2, 3, 4 };
            int[] planoIds = { 1, 2, 3, 1 };

            int[] Fids = { 1, 2, 3, 4 };
            string[] frases = { "Frase 1", "Frase 2", "Frase 3", "Frase 4" };
            int[] signoIdsFrase = { 1, 2, 3, 4 }; 
            int[] planoIdsFrase = { 1, 2, 1, 3 };


            int[] Tids = { 1, 2, 3, 4 };
            string[] tiposPlanos = { "Bronze", "Prata", "Ouro" };


            int[] Bids = { 1, 2, 3, 4 };
            string[] bichos = { "Cachorro", "Gato", "Coelho", "Peixe" };
            using (Conexao)
            {
                Conexao.Open();

                string deleteFrasesQuery = "DELETE FROM frases";
                using (SqlCommand command = new SqlCommand(deleteFrasesQuery, Conexao))
                {
                    command.ExecuteNonQuery();
                }

                string deleteUsuariosQuery = "DELETE FROM usuarios";
                using (SqlCommand command = new SqlCommand(deleteUsuariosQuery, Conexao))
                {
                    command.ExecuteNonQuery();
                }

                string deleteSignosQuery = "DELETE FROM signos";
                using (SqlCommand command = new SqlCommand(deleteSignosQuery, Conexao))
                {
                    command.ExecuteNonQuery();
                }

                string deletePlanosQuery = "DELETE FROM planos";
                using (SqlCommand command = new SqlCommand(deletePlanosQuery, Conexao))
                {
                    command.ExecuteNonQuery();
                }

                string deleteBichosQuery = "DELETE FROM bichos";
                using (SqlCommand command = new SqlCommand(deleteBichosQuery, Conexao))
                {
                    command.ExecuteNonQuery();
                }


                for (int i = 0; i < signoIds.Length; i++)
                {
                    string query = $"INSERT INTO signos (id,signo, data_inicial, data_final, planeta) " +
                                   $"VALUES ('{signoIds[i]}','{signos[i]}', '{datasIniciais[i].ToString("yyyy-MM-dd")}', '{datasFinais[i].ToString("yyyy-MM-dd")}', '{planetas[i]}')";

                    using (SqlCommand command = new SqlCommand(query, Conexao))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                for (int i = 0; i < tiposPlanos.Length; i++)
                {
                    string query = $"INSERT INTO planos (id,tipo) " +
                                   $"VALUES ('{Tids[i]}','{tiposPlanos[i]}')";

                    using (SqlCommand command = new SqlCommand(query, Conexao))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                for (int i = 0; i < usuarios.Length; i++)
                {
                    string query = $"INSERT INTO usuarios (id,nome,senha, data_birth, signo_id, plano_id) " +
                                   $"VALUES ('{Uids[i]}','{usuarios[i]}','{senhas[i]}', '{datasNascimento[i].ToString("yyyy-MM-dd")}','{signoIds[i]} ', '{planoIds[i]}')";

                    using (SqlCommand command = new SqlCommand(query, Conexao))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                for (int i = 0; i < frases.Length; i++)
                {
                    string query = $"INSERT INTO frases (id,frase, signo_id, plano_id) " +
                                   //$"VALUES ('{frases[i]}',  null, null)";
                                   $"VALUES ('{Fids[i]}','{frases[i]}', '{signoIdsFrase[i]}','{planoIds[i]}')";

                    using (SqlCommand command = new SqlCommand(query, Conexao))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                for (int i = 0; i < bichos.Length; i++)
                {
                    string query = $"INSERT INTO bichos (id,bicho) " +
                                   $"VALUES ('{Bids[i]}','{bichos[i]}')";

                    using (SqlCommand command = new SqlCommand(query, Conexao))
                    {
                        command.ExecuteNonQuery();
                    }
                }


            }
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}