using System;
using System.Collections.Generic;
using System.Text;

namespace OpenKey.COMMON.Entidades
{
    public class DatosArduino:BaseDTO
    {
        public int IntentosDeRobo { get; set; }
        public int ActividadSospechosa { get; set; }
        public List<UsuarioEmpleado> UsuariosQueEntraron { get; set; }
    }
}
