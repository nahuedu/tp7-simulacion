using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tp7_simulacion.domain
{
    public class simulacion_catalogacion
    {
        public double TIME { get; set; }
        public double TFINAL { get; set; }

        public double TPLL { get; set; }
        public List<double> TPSJ { get; set; }
        public List<double> TPSS { get; set; }

        public double HV = 999999999999999; // Son 15 nueves.

        public List<int> SolicitudesJuniorNormales { get; set; }
        public List<int> SolicitudesSeniorNormales { get; set; }

        public List<int> JuniorsSolicitudesRespondidas { get; set; }
        public List<int> SeniorsSolicitudesConsultadas { get; set; }
 
        public List<string> IndicadoresOperacionJunior { get; set; }
        public List<string> IndicadoresOperacionSenior { get; set; }

        // Resultados
        int numeroTotalDeSolicitudes;
        double sumatoriaLlegadas;
        double sumatoriaSalidas;
        List<double> inicioTiempoOciosoJuniors;
        List<double> inicioTiempoOciosoSeniors;
        List<double> sumatoriaTiempoOciosoJuniors;
        List<double> sumatoriaTiempoOciosoSeniors;

        public void simular()
        {
            do
            {
                this.setInitialConditions();
                var menorTPSJ = this.TPSJ.Min();
                var menorTPSS = this.TPSS.Min();

                if (menorTPSJ <= menorTPSS)
                {
                    if (menorTPSJ <= this.TPLL)
                        this.simularSalidaJunior();
                    else
                        this.simularLlegada();
                }
                else
                {
                    if (menorTPSS <= this.TPLL)
                        this.simularSalidaSenior();
                    else
                        this.simularLlegada();
                }
            } while (this.TIME <= this.TFINAL);

            this.calcularResultados();

        }

        public void setInitialConditions()
        {
            // TODO: Implementar
            this.TPSJ[10] = 100;
        }

        public void simularLlegada()
        {
            this.TIME = this.TPLL;
            var intervalosEntreArribos = this.generarIntervaloEntreArribos();
            this.TPLL = this.TIME + intervalosEntreArribos;
            int juniorMenosOcupadoConSolicitudesNormales = obtenerJuniorConMenorCantidadDeSolicitudesNormales();
            int seniorMenosOcupadoConSolicitudesNormales = obtenerSeniorConMenorCantidadDeSolicitudesNormales();

            numeroTotalDeSolicitudes = numeroTotalDeSolicitudes + 1;
            sumatoriaLlegadas = sumatoriaLlegadas + this.TIME;

            int cantSolicitudesNormalesJuniorMenosOcupado = this.SolicitudesJuniorNormales[juniorMenosOcupadoConSolicitudesNormales];
            int cantSolicitudesNormalesSeniorMenosOcupado = this.SolicitudesSeniorNormales[seniorMenosOcupadoConSolicitudesNormales];

            if (cantSolicitudesNormalesJuniorMenosOcupado <= cantSolicitudesNormalesSeniorMenosOcupado)
                this.enviarNuevaSolicitudAJunior(juniorMenosOcupadoConSolicitudesNormales);
            else
                this.enviarnuevaSolicitudASenior(seniorMenosOcupadoConSolicitudesNormales);

        }

        public void simularSalidaJunior()
        {
            // TODO: Implementar
        }

        public void simularSalidaSenior()
        {
            // TODO: Implementar
        }

        public int obtenerJuniorConMenorCantidadDeSolicitudesNormales()
        {
            var min = this.SolicitudesJuniorNormales.Min();

            return this.SolicitudesJuniorNormales.FindIndex(x => x == min);
        }

        public int obtenerSeniorConMenorCantidadDeSolicitudesNormales()
        {
            var min = this.SolicitudesSeniorNormales.Min();

            return this.SolicitudesSeniorNormales.FindIndex(x => x == min);
        }

        public void enviarNuevaSolicitudAJunior(int _junior)
        {
            this.SolicitudesJuniorNormales[_junior] = this.SolicitudesJuniorNormales[_junior] - 1;

            if (this.SolicitudesJuniorNormales[_junior] == 1 && this.TPSJ[_junior] == HV) // Generar atención del Junior.
            {
                this.IndicadoresOperacionJunior[_junior] = "Normal";
                var tiempoAtencion = this.generarTiempoAtencionJunior();
                this.TPSJ[_junior] = this.TIME + tiempoAtencion;
                this.sumatoriaTiempoOciosoJuniors[_junior] = this.sumatoriaTiempoOciosoJuniors[_junior] + (this.TIME - this.inicioTiempoOciosoJuniors[_junior]);
            }
        }

        public void enviarnuevaSolicitudASenior(int _senior)
        {
            this.SolicitudesSeniorNormales[_senior] = this.SolicitudesSeniorNormales[_senior] - 1;

            if (this.SolicitudesSeniorNormales[_senior] == 1 && this.TPSS[_senior] == HV) // Generar atención del Senior
            {
                this.IndicadoresOperacionSenior[_senior] = "Normal";
                var tiempoAtencion = this.generarTiempoAtencionSenior();
                this.TPSS[_senior] = this.TIME + tiempoAtencion;
                this.sumatoriaTiempoOciosoSeniors[_senior] = this.sumatoriaTiempoOciosoSeniors[_senior] + (this.TIME - this.inicioTiempoOciosoSeniors[_senior]);
            }
        }

        // ----------------------------- FDPs ------------------------------------------
        public double generarIntervaloEntreArribos()
        {
            return 100; // TODO: Implementar FDP
        }

        public double generarTiempoAtencionJunior()
        {
            return 100; // TODO: Hacer
        }

        public double generarTiempoAtencionSenior()
        {
            return 100; // TODO: Hacer
        }


        // -------------------- Calculo de Resultados --------------------

        public void calcularResultados()
        {

        }
    }
}
