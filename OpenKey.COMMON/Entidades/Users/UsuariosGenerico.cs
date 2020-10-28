using System;
using System.Collections.Generic;
using System.Text;

namespace OpenKey.COMMON.Entidades
{
    public abstract class UsuariosGenerico:BaseDTO
    {
        public string Nombre { get; set; }
        public string NumTelefono { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
        public string Direccion { get; set; }

    }
}
