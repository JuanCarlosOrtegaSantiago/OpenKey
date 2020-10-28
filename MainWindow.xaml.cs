using OpenKey.BIZ;
using OpenKey.COMMON.Entidades;
using OpenKey.COMMON.Interfaces;
using OpenKey.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace OpenKey
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {

        
            IManejadorDeUsuarioJefe manejadorDeUsuarioJefe;

        public MainWindow()
        {
            InitializeComponent();
            manejadorDeUsuarioJefe = new ManejadorDeUsuariosJefe(new RepositorioGenerico<UsuarioJefe>());
            DatosAIniciar();

        }

        private void DatosAIniciar()
        {
            if (manejadorDeUsuarioJefe.Read.Count < 1)
            {
                UsuarioJefe usuarioJefe = new UsuarioJefe
                {
                    Contrasenia = "1234",
                    Correo = "admin@admin",
                    Nombre = "admin"
                };
                if (manejadorDeUsuarioJefe.Create(usuarioJefe))
                {
                    MessageBox.Show("User creado exitasamente");
                }
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(txtCorreo.Text) && !string.IsNullOrWhiteSpace(_Password.Password))
            {
                if (manejadorDeUsuarioJefe.BuscarUsuario(txtCorreo.Text, _Password.Password))
                {
                    MessageBox.Show("Usuario encontrado");
                }
                else
                {

                    MessageBox.Show("Error de datos","Error",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
            }
        }
    }
}
