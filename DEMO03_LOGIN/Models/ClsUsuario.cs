using DEMO03_LOGIN.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DEMO03_LOGIN.Models
{
    public class ClsUsuario
    {
        public int id { get; set; }
   
        [Required]
        public string usuario { get; set; }

        [Required]
        [ContrasenaValidate(ErrorMessage = "Contraseña no valida")]
        public string contrasena { get; set; }

        public int intentos { get; set; }

        public decimal nivelSeg { get; set; }

        public DateTime? fechaReg { get; set; }
    }
}