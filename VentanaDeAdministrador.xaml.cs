using OpenKey.BIZ;
using OpenKey.COMMON.Entidades;
using OpenKey.COMMON.Interfaces;
using OpenKey.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OpenKey
{
    /// <summary>
    /// Lógica de interacción para VentanaDeAdministrador.xaml
    /// </summary>
    public partial class VentanaDeAdministrador : Window
    {
        IManejadorDeUsuarioJefe manejadorDeUsuarioJefe;
        public VentanaDeAdministrador()
        {
            InitializeComponent();
            manejadorDeUsuarioJefe = new ManejadorDeUsuariosJefe(new RepositorioGenerico<UsuarioJefe>());
            CargarDatos();
            Limpiar();
            EstadoDeBotones(true);
        }

        private void Limpiar()
        {
            txtContrasenia.Clear();
            txtCorreo.Clear();
            txtDireccion.Clear();
            txtNombre.Clear();
            txtTelefono.Clear();
            txtCargo.Clear();
        }

        private void CargarDatos()
        {
            listAdministardores.ItemsSource = null;
            listAdministardores.ItemsSource = manejadorDeUsuarioJefe.Read;
        }

        private void EstadoDeBotones(bool v)
        {
            btnCancelar.IsEnabled = !v;
            btnCrear.IsEnabled = v;
            btnEliminar.IsEnabled = v;
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            StackCrear.Visibility = Visibility.Visible;
            listAdministardores.Visibility = Visibility.Collapsed;
            Limpiar();
            EstadoDeBotones(false);
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtContrasenia.Text) || string.IsNullOrWhiteSpace(txtCorreo.Text) || string.IsNullOrWhiteSpace(txtDireccion.Text) || string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text) || string.IsNullOrWhiteSpace(txtCargo.Text))
            {
                MessageBox.Show("Faltan datos por llenar", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;

            }

            UsuarioJefe usuarioJefe = new UsuarioJefe()
            {
                Contrasenia = txtContrasenia.Text,
                Correo = txtCorreo.Text,
                Direccion = txtDireccion.Text,
                Nombre = txtNombre.Text,
                NumTelefono = txtTelefono.Text,
                 Cargo=txtCargo.Text
            };

            if (manejadorDeUsuarioJefe.Create(usuarioJefe))
            {
                StackCrear.Visibility = Visibility.Collapsed;
                listAdministardores.Visibility = Visibility.Visible;
                CargarDatos();
            }

            EstadoDeBotones(false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VentanaMenu ventanaMenu = new VentanaMenu();
            this.Close();
            ventanaMenu.Show();

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (listAdministardores.SelectedItem == null)
                return;

            UsuarioEmpleado usuarioEmpleado = (UsuarioEmpleado)listAdministardores.SelectedItem;

            if (MessageBox.Show("Desea eliminar a: \n" + usuarioEmpleado.Nombre, "Precaución", MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (manejadorDeUsuarioJefe.Delete(usuarioEmpleado.id))
                {
                    CargarDatos();
                    MessageBox.Show("Correcto", "Hecho", MessageBoxButton.OK,MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ha ocurrido un error", "Fallo", MessageBoxButton.OK,MessageBoxImage.Error);

                }
            }

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            EstadoDeBotones(true);
            Limpiar();

            StackCrear.Visibility = Visibility.Collapsed;
            listAdministardores.Visibility = Visibility.Visible;

        }
    }
}
