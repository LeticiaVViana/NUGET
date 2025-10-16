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
