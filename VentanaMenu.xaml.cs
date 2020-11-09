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
    /// Lógica de interacción para VentanaMenu.xaml
    /// </summary>
    public partial class VentanaMenu : Window
    {
        public VentanaMenu()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void btnEmpleados_Click(object sender, RoutedEventArgs e)
        {
            VentanaEmpleados ventanaEmpleados = new VentanaEmpleados();
            this.Close();
            ventanaEmpleados.Show();
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            VentanaDeAdministrador ventanaDeAdministrador = new VentanaDeAdministrador();
            this.Close();
            ventanaDeAdministrador.Show();
        }

        private void btnDCircuito_Click(object sender, RoutedEventArgs e)
        {
            VentanaReoleccionDeDatosArduino ventanaReoleccionDeDatosArduino = new VentanaReoleccionDeDatosArduino();
            this.Close();
            ventanaReoleccionDeDatosArduino.Show();
        }

        private void btnCActuadores_Click(object sender, RoutedEventArgs e)
        {
            VentanaControlandoActuadores ventanaControlandoActuadores = new VentanaControlandoActuadores();
            this.Close();
            ventanaControlandoActuadores = new VentanaControlandoActuadores();
        }
    }
}
