using OpenKey.BIZ;
using OpenKey.COMMON.Entidades;
using OpenKey.COMMON.Interfaces;
using OpenKey.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OpenKey.MOVIL
{
    public partial class MainPage : ContentPage
    {
        IManejadorDeUsuarioJefe manejadorDeUsuarioJefe;
        public MainPage()
        {
            InitializeComponent();
            manejadorDeUsuarioJefe = new ManejadorDeUsuariosJefe(new RepositorioGenerico<UsuarioJefe>());
        }
    }
}
