using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace credito
{
    public partial class Form1 : Form
    {
        private double saldoInicialAntesDeExtraordinario;
        public Form1()
        {
            InitializeComponent();

            // Agregar columnas al DataGridView
            dataGridViewTabla.ColumnCount = 5;
            dataGridViewTabla.Columns[0].Name = "Numero de Cuota";
            dataGridViewTabla.Columns[1].Name = "saldo Inicial";
            dataGridViewTabla.Columns[2].Name = "Interes";
            dataGridViewTabla.Columns[3].Name = "Abono Capital";
            dataGridViewTabla.Columns[4].Name = "Saldo Final";


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtPrestamo.Text, out double prestamo) &&
               double.TryParse(txtTasaInteres.Text, out double tasa) &&
               int.TryParse(txtPeriodos.Text, out int periodos))
            {
                // Convertir la tasa de interés a porcentaje (si es ingresada como un decimal)
                if (tasa > 1)
                {
                    tasa /= 100; // Convertir a porcentaje (0.035 a 3.5)
                }

                saldoInicialAntesDeExtraordinario = prestamo;

                double cuota = CalcularCuota(prestamo, tasa, periodos);

                txtCuota.Text = Math.Round(cuota, 0).ToString(); // Mostrar la cuota redondeada al entero más cercano
                //txtCuota.Text = cuota.ToString("F2"); // Mostrar la cuota en el TextBox


                GenerarTablaAmortizacion(prestamo, tasa, periodos, cuota);
                
            }
            else
            {
                MessageBox.Show("Por favor ingrese números válidos en los campos.");

            }
        }
        

        private double CalcularCuota(double prestamo, double tasa, int periodos)
        {

            double tasaDecimal = tasa; // Convertir la tasa a decimal si es necesario
            double cuota = prestamo * (tasaDecimal / (1 - Math.Pow(1 + tasaDecimal, -periodos)));
            return cuota;
        }

        private void GenerarTablaAmortizacion(double prestamo, double tasa, int periodos, double cuota)
        {
            dataGridViewTabla.Rows.Clear();

            double saldoInicial = prestamo;
            double tasaDecimal = tasa;

            double totalCapital = 0;
            int totalInteres = 0;

            for (int i = 1; i <= periodos; i++)
            {
                double interes = saldoInicial * tasaDecimal;
                double abonoCapital = cuota - interes;
               
                double saldoFinal = saldoInicial - abonoCapital;

                dataGridViewTabla.Rows.Add(i, (int)saldoInicial, (int)interes, (int)abonoCapital, (int)saldoFinal);
                totalCapital += abonoCapital;
                totalInteres += (int)interes;

                saldoInicial = saldoFinal;
            }

            // Agregar la fila con la suma de los totales
            dataGridViewTabla.Rows.Add("Total", "", totalInteres, totalCapital, "");

            // Establecer estilos visuales para resaltar los totales
            dataGridViewTabla.Rows[dataGridViewTabla.RowCount - 1].DefaultCellStyle.Font = new System.Drawing.Font(dataGridViewTabla.Font, System.Drawing.FontStyle.Bold);
            dataGridViewTabla.Rows[dataGridViewTabla.RowCount - 1].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
  
        }

        private void btnAbonar_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtAbono.Text, out double abono) &&
        int.TryParse(txtCuotaAbono.Text, out int cuotaAplicar))
            {
                if (cuotaAplicar >= 1 && cuotaAplicar <= dataGridViewTabla.RowCount)
                {
                    AplicarAbonoExtraordinario(abono, cuotaAplicar);
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un número de cuota válido.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un valor válido para el abono.");
            }
        }

        private void AplicarAbonoExtraordinario(double abono, int cuotaAplicar)
        {
            // Obtener el saldo inicial antes del abono extraordinario
            double saldoInicial = saldoInicialAntesDeExtraordinario;

            // Calcular la cuota original
            double tasa = Convert.ToDouble(txtTasaInteres.Text) / 100;
            double cuotaOriginal = Convert.ToDouble(txtCuota.Text);
            int periodosRestantes = Convert.ToInt32(txtPeriodos.Text) - cuotaAplicar + 1;

            // Recalcular el saldo después del abono extraordinario
            for (int i = 1; i <= periodosRestantes; i++)
            {
                double interes = saldoInicial * tasa;
                double abonoCapital = cuotaOriginal - interes + abono;
                double saldoFinal = saldoInicial - abonoCapital;

                dataGridViewTabla.Rows[cuotaAplicar - 1].Cells[2].Value = (int)interes;
                dataGridViewTabla.Rows[cuotaAplicar - 1].Cells[3].Value = (int)abonoCapital;
                dataGridViewTabla.Rows[cuotaAplicar - 1].Cells[4].Value = (int)saldoFinal;

                saldoInicial = saldoFinal;
                cuotaAplicar++;
            }
        }



    }
}







