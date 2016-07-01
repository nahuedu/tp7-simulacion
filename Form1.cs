using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tp7_simulacion.domain;

namespace tp7_simulacion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_simular_Click(object sender, EventArgs e)
        {
            int cantJuniors = Convert.ToInt32(this.txtbox_cant_juniors.Text);
            int cantSeniors = Convert.ToInt32(this.txtbox_cant_seniors.Text);

            var resultado = new simulacion_catalogacion().simular(cantJuniors, cantSeniors);
            resultado.promedioPermanenciaSistema = Math.Round(resultado.promedioPermanenciaSistema);
            resultado.porcentajeGeneralTOJ = Math.Round(resultado.porcentajeGeneralTOJ);
            resultado.porcentajeGeneralTOS = Math.Round(resultado.porcentajeGeneralTOS);

            this.lbl_promedio_permanencia.Text = resultado.promedioPermanenciaSistema.ToString();
            this.lbl_ptoj.Text = resultado.porcentajeGeneralTOJ.ToString();
            this.lbl_ptos.Text = resultado.porcentajeGeneralTOS.ToString();
        }
    }
}
