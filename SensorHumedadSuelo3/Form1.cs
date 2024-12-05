using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace SensorHumedadSuelo3
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort;
        public Form1()
        {
            InitializeComponent();

            // Inicializar el objeto SerialPort
            serialPort = new SerialPort
            {
                PortName = "COM3", // Cambia esto al puerto de tu Arduino
                BaudRate = 9600,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                ReadTimeout = 5000,
                WriteTimeout = 5000
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Abrir el puerto serie si no está abierto
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }

                // Leer una línea del puerto serie
                string data = serialPort.ReadLine();

                // Identificar y procesar el valor de humedad
                if (data.Contains("Lectura:"))
                {
                    int index = data.IndexOf(':') + 1;
                    string valorStr = data.Substring(index).Trim();

                    if (int.TryParse(valorStr, out int humedad))
                    {
                        ActualizarListBox(humedad);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void ActualizarListBox(int humedad)
        {
            string estado;

            // Determinar el estado según la lectura
            if (humedad >= 0 && humedad <= 300)
            {
                estado = "Suelo seco";
            }
            else if (humedad > 301 && humedad <= 700)
            {
                estado = "Suelo húmedo";
            }
            else
            {
                estado = "Sensor en agua";
            }

            // Mostrar los valores en la ListBox
            listBox1.Items.Add($"Humedad: {humedad} - Estado: {estado}");
        }

        // Cerrar el puerto al cerrar el formulario
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}
