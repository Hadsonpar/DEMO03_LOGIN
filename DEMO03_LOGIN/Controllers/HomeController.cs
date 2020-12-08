using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using DEMO03_LOGIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DEMO03_LOGIN.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }
        public HomeController(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IActionResult Usuario() {
            return View();
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<ClsUsuario> UsuariosList = new List<ClsUsuario>();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString)) {

                connection.Open();

                string sql = "select * from USUARIO";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ClsUsuario clsUsuario = new ClsUsuario();
                        clsUsuario.id = Convert.ToInt32(dataReader["Id"]);
                        clsUsuario.usuario = Convert.ToString(dataReader["Usuario"]);
                        clsUsuario.contrasena = Convert.ToString(dataReader["Contrasena"]);
                        clsUsuario.intentos = Convert.ToInt32(dataReader["Intentos"]);
                        clsUsuario.nivelSeg = Convert.ToDecimal(dataReader["NivelSeg"]);
                        clsUsuario.fechaReg = Convert.ToDateTime(dataReader["FechaReg"]);

                        UsuariosList.Add(clsUsuario);
                    }
                }

                connection.Close();
            }
            return View(UsuariosList);
        }
    }
}