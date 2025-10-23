using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NUGET.Models;

using MySql.Data.MySqlClient;

namespace NUGET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cadastrar()
        {
            return View();
        }
        public IActionResult Editar()
        {
            return View();
        }
        public IActionResult Deletar()
        {
            return View();
        }
        public IActionResult Listar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Listar(int idlogin)
        {
            string connectionString = "Server=localhost;Database=cad_login;Uid=root;Pwd=123456;";
            string query = "SELECT * FROM login WHERE idlogin = @idlogin";

            try
            {
                using var connection = new MySqlConnection(connectionString);
                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@idlogin", idlogin);
                connection.Open();

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ViewData["Message"] = "Usuario encontrado!";
                    ViewData["UsuarioEncontrado"] = reader["usuario"].ToString();
                    ViewData["SenhaEncontrada"] = reader["senha"].ToString();
                }

                else
                {
                    ViewData["Message"] = "Usuario não encontrado!";
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = $"ERRO de banco de dados: {ex.Message}";
            }
            return View();
        }
        //********************************************************************************************
        [HttpPost]
        public IActionResult Deletar(int idlogin)
        {
            string connectionString = "Server=localhost; Database=cad_login;Uid=root;Pwd=123456;";
            string query = "DELETE FROM login WHERE idlogin = @idlogin";

            try
            {
                using var connection = new MySqlConnection(connectionString);
                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@idlogin", idlogin);
                

                connection.Open();
                int linhasAfetadas = command.ExecuteNonQuery();

                ViewData["Message"] = linhasAfetadas > 0
                    ? "Usuário deletado com sucesso!"
                    : "Ocorreu um erro e o usuário não foi deletado.";
            }
            catch (MySqlException ex)
            {
                ViewData["Message"] = $"ERRO de banco de dados: {ex.Message}";
            }
            return View();
        }
        //**********************************************************************************************
        [HttpPost]
        public IActionResult Editar(string usuario,string senha, int idlogin )
        {
            if(string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                ViewData["Message"] = "O nome de usuario e a senha não podem estar vazios.";
                return View();
            }
            string connectionString = "Server=localhost; Database=cad_login;Uid=root;Pwd=123456;";
            string query = "UPDATE login SET usuario = @usuario, senha = @senha WHERE idlogin = @idlogin";

            try
            {
                using var connection = new MySqlConnection(connectionString);
                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@usuario", usuario);
                command.Parameters.AddWithValue("@senha", senha);
                command.Parameters.AddWithValue("@idlogin", idlogin);

                connection.Open();
                int linhasAfetadas = command.ExecuteNonQuery();

                ViewData["Message"] = linhasAfetadas > 0
                    ? "Usuário alterado com sucesso!"
                    : "Ocorreu um erro e o usuário não foi cadastrado.";
            }
            catch (MySqlException ex)
            {
                ViewData["Message"] = $"ERRO de banco de dados: {ex.Message}";
            }
            return View();
        }
        //******************************************************************************************
        [HttpPost]
        public IActionResult Cadastrar(string usuario, string senha)
        {
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                ViewData["Message"] = "O nome de usuario e a senha não podem estar vazios.";
                return View();
            }
            string connectionString = "Server=localhost;Database=cad_login;Uid=root;Pwd=123456;";
            string query = "INSERT INTO login (usuario, senha) VALUES (@usuario, @senha)";

            try
            {
                using var connection = new MySqlConnection(connectionString);
                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@usuario", usuario);
                command.Parameters.AddWithValue("@senha", senha);

                connection.Open();
                int linhasAfetadas = command.ExecuteNonQuery();

                ViewData["Message"] = linhasAfetadas > 0
                ? "Usuario cadastrado com sucesso!"
                : "Ocorreu um erro e o usuario não foi cadastrado.";
            }
            catch (MySqlException ex)
            {
                ViewData["Message"] = $"ERRO de banco de dados: {ex.Message}";
            }
            return View(); 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
