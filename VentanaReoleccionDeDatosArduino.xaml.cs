using DnsClient.Protocol;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using OpenKey.BIZ;
using OpenKey.COMMON.Entidades;
using OpenKey.COMMON.Interfaces;
using OpenKey.DAL;
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
using System.Windows.Threading;

namespace OpenKey
{
    /// <summary>
    /// Lógica de interacción para VentanaReoleccionDeDatosArduino.xaml
    /// </summary>
    public partial class VentanaReoleccionDeDatosArduino : Window
    {

        public Func<ChartPoint, string> PointLabel { get; set; }
        public ChartValues<ObservableValue> ValueSensorDeMovimiento { get; set; }
        public ChartValues<ObservableValue> ValuePosibleRobo { get; set; }
        IManejadorDedatosArduino manejadorDedatosArduino;
        IManejadorDeUsuarioEmpleado manejadorDeUsuarioEmpleado;

        System.IO.Ports.SerialPort Arduino;
        bool IsClosed = false;
        int NumPRobos = 0;
        int NumMovimientos = 0;
        int NumEntradas = 0;

        int TotalDeEntardas = 0;
        UsuarioEmpleado _UsuarioEmpleado;

        DatosArduino _DatosArduino;
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

            manejadorDedatosArduino = new ManejadorDeDatosArduino(new RepositorioGenerico<DatosArduino>());

            manejadorDeUsuarioEmpleado = new ManejadorDeUsuarioEmpleado(new RepositorioGenerico<UsuarioEmpleado>());

            DatosAIniciar();

            PointLabel = chartPoint =>
                    string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = this;

            ValueSensorDeMovimiento = new ChartValues<ObservableValue>();
            ValuePosibleRobo = new ChartValues<ObservableValue>();

            


            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            
            timer.Tick += Timer_Tick;
            timer.Start();

            DispatcherTimer timer2 = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(60)
            };
            
            timer2.Tick += Timer2_Tick;
            timer2.Start();

            DispatcherTimer timer3 = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };

            timer3.Tick += Timer3_Tick;
            timer3.Start();
        }

        private void Timer3_Tick(object sender, EventArgs e)
        {

            GrafEmpleados.To = TotalDeEntardas;
            GrafEmpleados.Value = _UsuarioEmpleado.NumEntradas;
            GrafEmpleados.From = 0;
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            if (manejadorDedatosArduino.Update(_DatosArduino))
            {
                NumPRobos = 0;
                NumMovimientos = 0;
            }
        }

        private void DatosAIniciar()
        {
            if (manejadorDedatosArduino.Read.Count < 1)
            {
                _DatosArduino = new DatosArduino
                {
                    ActividadSospechosa = NumMovimientos,
                    IntentosDeRobo = NumPRobos
                };
                manejadorDedatosArduino.Create(_DatosArduino);
            }
            else
            {
                _DatosArduino = manejadorDedatosArduino.Read.SingleOrDefault();
            }

            ListEmpleados.ItemsSource = null;
            ListEmpleados.ItemsSource = manejadorDeUsuarioEmpleado.Read;
            _UsuarioEmpleado = manejadorDeUsuarioEmpleado.Read.First();
            foreach (var item in manejadorDeUsuarioEmpleado.Read)
            {
                TotalDeEntardas += item.NumEntradas;
                if (item.NumEntradas > _UsuarioEmpleado.NumEntradas)
                    _UsuarioEmpleado = item;
            }

            GrafEmpleados.To = TotalDeEntardas;
            GrafEmpleados.Value = _UsuarioEmpleado.NumEntradas;
            GrafEmpleados.From = 0;
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                ValuePosibleRobo.Clear();
                ValueSensorDeMovimiento.Clear();

                ValueSensorDeMovimiento.Add(new ObservableValue(_DatosArduino.ActividadSospechosa));
                ValuePosibleRobo.Add(new ObservableValue(_DatosArduino.IntentosDeRobo));

            }
            catch (Exception)
            {

            }


        }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
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
                    string cadena = null;
                    cadena = Arduino.ReadExisting();

                   if (cadena.Equals("1"))
                    {
                        NumPRobos += 1;
                        _DatosArduino.IntentosDeRobo += NumPRobos;
                    }
                    if (cadena.Equals("2"))
                    {
                        NumMovimientos += 1;
                        _DatosArduino.ActividadSospechosa += NumMovimientos;
                    }
                    if (cadena == "PAbierta\r")
                        NumEntradas += 1;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (Arduino.IsOpen)
                Arduino.Close();

            VentanaMenu ventanaMenu = new VentanaMenu();
            this.Close();
            ventanaMenu.Show();
        }

        private void BtnDatosArduino_Click(object sender, RoutedEventArgs e)
        {
            _ContGrafDatosArduino.Visibility = Visibility.Visible;
            _ContGrafEmpleados.Visibility = Visibility.Collapsed;
        }

        private void btnEntardas_Click(object sender, RoutedEventArgs e)
        {
            _ContGrafEmpleados.Visibility = Visibility.Visible;
            _ContGrafDatosArduino.Visibility = Visibility.Collapsed;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (ListEmpleados.SelectedItem == null)
                return;

            UsuarioEmpleado usuarioEmpleado = (UsuarioEmpleado)ListEmpleados.SelectedItem;

            GrafEmpleados.To = TotalDeEntardas;
            GrafEmpleados.Value = usuarioEmpleado.NumEntradas;
            GrafEmpleados.From = 0;

        }
    }
}
