using OpenKey.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenKey.COMMON.Interfaces
{
    public interface IManejadorDeUsuarioEmpleado:IManejadorGenerico<UsuarioEmpleado>
    {
        bool BuscarUsuario(string Correo, string Contrasenia);
    }
}
