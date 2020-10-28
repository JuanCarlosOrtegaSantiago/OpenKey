using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace OpenKey
{
    /// <summary>
    /// Lógica de interacción para VentanaReoleccionDeDatosArduino.xaml
    /// </summary>
    public partial class VentanaReoleccionDeDatosArduino : Window
    {
        System.IO.Ports.SerialPort Arduino;
        bool IsClosed = false;
        int NumPRobos = 0;
        int NumMovimientos = 0;
        int NumEntradas = 0;
        public VentanaReoleccionDeDatosArduino()
        {
            InitializeComponent();
            Arduino = new System.IO.Ports.SerialPort();
            Arduino.PortName = "COM5";
            Arduino.BaudRate = 9600;
            Arduino.ReadTimeout = 500;
            try
            {
                Arduino.Open();
                

            }
            catch (Exception)
            {

                throw;
            }


        }

        private void AlCerrar(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Arduino.IsOpen)
                Arduino.Close();
        }

        private void EscucharSerial()
        {
            while (!IsClosed)
            {
                try
                {
                    string cadena = Arduino.ReadLine();

                    if (cadena == "PRobo\r")
                        NumPRobos += 1;
                    if (cadena == "MSospechoso\r")
                        NumMovimientos += 1;
                    if (cadena == "PAbierta\r")
                        NumEntradas += 1;

                    TxtEntradas.Dispatcher.Invoke(delegate {
                        TxtEntradas.Text = NumEntradas.ToString();

                    });

                    TxtMovimientos.Dispatcher.Invoke(delegate {
                        TxtMovimientos.Text = NumMovimientos.ToString();

                    });

                    TxtRobos.Dispatcher.Invoke(delegate {
                        TxtRobos.Text = NumPRobos.ToString();

                    });

                }
                catch (Exception)
                {


                }
            }
        }

        private void AlCerrarFormulario(object sender, EventArgs e)
        {
            IsClosed = true;
            if (Arduino.IsOpen)
                Arduino.Close();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            Thread hILO = new Thread(EscucharSerial);
            hILO.Start();
        }

    }
}
