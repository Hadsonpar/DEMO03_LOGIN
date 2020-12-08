using System;
using System.Collections.Generic;//Lista de colecciones genericas
using System.Data;//Identificar el tipo de objeto a manipular en base de datos
using Microsoft.Data.SqlClient;//Controlador de acceso a datos
using System.Linq;//Para hacer macth entre lista del dataReader y los antributos del modelo (usuario y contrasena)
//using System.Security.Claims;//Extrae las peticiones asociadas al usuario autenticado
using DEMO03_LOGIN.Models;//Para instanciar los atributos de la entidad usuario
using Microsoft.Extensions.Configuration;//Para acceder al archivo de configuración appsettings.json
using Microsoft.AspNetCore.Http;//Para el manejo de solicitudes y respuestas HTTP
using Microsoft.AspNetCore.Mvc;

namespace DEMO03_LOGIN.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// Constante para Inicializar la Sesión _User
        /// </summary>
        const string SessionUser = "_User";
        public IConfiguration Configuration { get; }
        /// <summary>
        /// Interfaz para acceder a los valores del archivo de configuración
        /// </summary>
        /// <param name="configuration"></param>
        public LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public ActionResult Login(string returnUrl)
        /// <summary>
        /// Action para inicializar la carga de la vista del Login en base a los atributos de modelo usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {   
            //ViewBag.ReturnUrl = returnUrl;
            return View(new ClsUsuario());
        }

        /// <summary>
        /// Action de tipo POST para  para inicializar el proceso de validación e iniciar sessión en  base a los datos del modelo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(ClsUsuario model)
        {   
            //Conexión a la base de datos
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            //Estoy usando uso de ADO.Net para interactuar con la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var list_users = new List<ClsUsuario>();
                //@1Inicio: Validar los controladores
                if (model.usuario == null || model.usuario.Equals("") ||
                    model.contrasena == null || model.contrasena.Equals(""))
                {
                    ModelState.AddModelError("", "Ingresar los datos solictiados");
                }//@1Final
                else
                { 
                    connection.Open();//Abrir la conexión a la base de datos
                    SqlCommand com = new SqlCommand("GET_SEG_USUARIO", connection);//Referencia al procedimiento almacenado
                    com.CommandType = CommandType.StoredProcedure;//Se define el tipo de comando a utilizar
                    //Paso los parámetros de acuerdo a los datos cargados segun el modelo usario
                    com.Parameters.AddWithValue("@usuario", model.usuario);
                    com.Parameters.AddWithValue("@contrasena", model.contrasena);
                    SqlDataReader dr = com.ExecuteReader();//Ejecuto el comando a través de un DataReader
                    //@2Inicio: Recorro los datos y adiciono en la lista list_users los valores usuario y contrasena
                    while (dr.Read())
                    {
                        ClsUsuario clsUsuario = new ClsUsuario();
                        clsUsuario.usuario = Convert.ToString(dr["Usuario"]);
                        clsUsuario.contrasena = Convert.ToString(dr["Contrasena"]);

                        list_users.Add(clsUsuario);//Adicionar en la lista
                    }
                    //@2Final 

                    //@3Inicio: Match entre los valores ingresados y la lista
                    if (list_users.Any(p => p.usuario == model.usuario && p.contrasena == model.contrasena))
                    {
                        //var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.usuario), });
                        HttpContext.Session.SetString(SessionUser, model.usuario);//Iniciamos la sesión pasando el valor (nombre del usuario)
                        
                        return RedirectToAction("Index", "Home");//Redireccionar a la vista usario (Lista de Usuarios)
                    }
                    else
                    {
                        ModelState.AddModelError("", "Datos ingresado no válido.");//Error personalizado
                    }
                }
                return View(model);
            }
        }

        /// <summary>
        /// Action para limpiar y cerrar la sesión de la aplicación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.Session.Clear();//Limpiar la sesión
            return RedirectToAction("Login", "Login");//Redireccionar a la vista login
        }
    }
}