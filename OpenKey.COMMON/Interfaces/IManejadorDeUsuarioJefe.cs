using OpenKey.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenKey.COMMON.Interfaces
{
    public interface IManejadorDeUsuarioJefe:IManejadorGenerico<UsuarioJefe>
    {
        bool BuscarUsuario(string Correo, string Contrasenia);
    }
}
