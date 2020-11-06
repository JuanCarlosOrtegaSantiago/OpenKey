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
    /// Lógica de interacción para VentanaEmpleados.xaml
    /// </summary>
    public partial class VentanaEmpleados : Window
    {
        IManejadorDeUsuarioEmpleado manejadorDeUsuarioEmpleado;
        public VentanaEmpleados()
        {
            InitializeComponent();
            manejadorDeUsuarioEmpleado = new ManejadorDeUsuarioEmpleado(new RepositorioGenerico<UsuarioEmpleado>());
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
            txtPuesto.Clear();
        }

        private void CargarDatos()
        {
            listEmpleados.ItemsSource = null;
            listEmpleados.ItemsSource = manejadorDeUsuarioEmpleado.Read;
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
            listEmpleados.Visibility = Visibility.Collapsed;
            Limpiar();
            EstadoDeBotones(false);
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtContrasenia.Text) || string.IsNullOrWhiteSpace(txtCorreo.Text) || string.IsNullOrWhiteSpace(txtDireccion.Text) || string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text) || string.IsNullOrWhiteSpace(txtPuesto.Text))
            {
                MessageBox.Show("Faltan datos por llenar", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;

            }

            UsuarioEmpleado usuarioEmpleado = new UsuarioEmpleado()
            {
                Contrasenia = txtContrasenia.Text,
                Correo = txtCorreo.Text,
                Direccion = txtDireccion.Text,
                Nombre = txtNombre.Text,
                NumTelefono = txtTelefono.Text,
                Puesto = txtPuesto.Text
            };

            int idEmpleado = manejadorDeUsuarioEmpleado.Read.Count();
            usuarioEmpleado.IdEmpleado = idEmpleado += 1;

            if (manejadorDeUsuarioEmpleado.Create(usuarioEmpleado))
            {
                StackCrear.Visibility = Visibility.Collapsed;
                listEmpleados.Visibility = Visibility.Visible;
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
            if (listEmpleados.SelectedItem == null)
                return;

            UsuarioEmpleado usuarioEmpleado = (UsuarioEmpleado)listEmpleados.SelectedItem;

            if (MessageBox.Show("Desea eliminar a: \n" + usuarioEmpleado.Nombre, "Precaución", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (manejadorDeUsuarioEmpleado.Delete(usuarioEmpleado.id))
                {
                    CargarDatos();
                    MessageBox.Show("Correcto", "Hecho", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ha ocurrido un error", "Fallo", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            EstadoDeBotones(true);
            Limpiar();

            StackCrear.Visibility = Visibility.Collapsed;
            listEmpleados.Visibility = Visibility.Visible;

        }
    }
}
