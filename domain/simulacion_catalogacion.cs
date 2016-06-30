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
        public List<Consulta> SeniorsSolicitudesConsultadas { get; set; }
 
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

        private Random generadorRandoms = new Random();

        public void simular()
        {
            do
            {
                this.setInitialConditions();
                var menorTPSJ = this.TPSJ.Min();
                var menorTPSS = this.TPSS.Min();

                var juniorConMenorTPS = this.TPSJ.FindIndex(j => j == menorTPSJ);
                var seniorConMenorTPS = this.TPSS.FindIndex(s => s == menorTPSS);
       
                if (menorTPSJ <= menorTPSS)
                {
                    if (menorTPSJ <= this.TPLL)
                        this.simularSalidaJunior(juniorConMenorTPS);
                    else
                        this.simularLlegada();
                }
                else
                {
                    if (menorTPSS <= this.TPLL)
                        this.simularSalidaSenior(seniorConMenorTPS);
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

        public void simularSalidaJunior(int _junior)
        {
            this.TIME = this.TPSJ[_junior];
            if(this.IndicadoresOperacionJunior[_junior] == "Normal")
            {
                this.SolicitudesJuniorNormales[_junior] = this.SolicitudesJuniorNormales[_junior] - 1;
                var rand = this.generarRandom();

                if (rand <= 0.2) // Se mandar a consultar
                {
                    var seniorASerConsultado = this.getSeniorConMenosConsultas();
                    this.SeniorsSolicitudesConsultadas[seniorASerConsultado].Cantidad = this.SeniorsSolicitudesConsultadas[seniorASerConsultado].Cantidad + 1;
                    this.SeniorsSolicitudesConsultadas[seniorASerConsultado].JuniorsConsultantes.Add(_junior);

                    if(this.SeniorsSolicitudesConsultadas[seniorASerConsultado].Cantidad == 1 && (this.TPSS[seniorASerConsultado] == this.HV))
                    {
                        this.IndicadoresOperacionSenior[seniorASerConsultado] = "Consulta";
                        var tiempoDeAtencion = this.generarTiempoAtencionSenior();
                        this.TPSS[seniorASerConsultado] = this.TIME + tiempoDeAtencion;

                        this.sumatoriaTiempoOciosoSeniors[seniorASerConsultado] = this.sumatoriaTiempoOciosoSeniors[seniorASerConsultado] + (this.TIME - this.inicioTiempoOciosoSeniors[seniorASerConsultado]);
                    }
                }
                else
                {
                    this.sumatoriaSalidas = this.sumatoriaSalidas + this.TIME;
                }
            }
            else
            {
                this.JuniorsSolicitudesRespondidas[_junior] = this.JuniorsSolicitudesRespondidas[_junior] - 1;
                this.sumatoriaSalidas = this.sumatoriaSalidas + this.TIME;
            }

            if(this.JuniorsSolicitudesRespondidas[_junior] >= 1)
            {
                this.IndicadoresOperacionJunior[_junior] = "Respuesta";
                var tiempoDeATencion = this.generarTiempoAtencionJunior();
                this.TPSJ[_junior] = this.TIME + tiempoDeATencion;
            }
            else
            {
                if (this.SolicitudesJuniorNormales[_junior] >= 1 && this.JuniorsSolicitudesRespondidas[_junior] == 0)
                {
                    this.IndicadoresOperacionJunior[_junior] = "Normal";
                    var tiempoDeATencion = this.generarTiempoAtencionJunior();
                    this.TPSJ[_junior] = this.TIME + tiempoDeATencion;
                }
                else
                {
                    this.TPSJ[_junior] = this.HV;
                    this.inicioTiempoOciosoJuniors[_junior] = this.TIME;
                }
            }
        }

        public void simularSalidaSenior(int _senior)
        {
            this.TIME = this.TPSS[_senior];

            if(this.IndicadoresOperacionSenior[_senior] == "Normal")
            {
                this.SolicitudesSeniorNormales[_senior] = this.SolicitudesSeniorNormales[_senior] - 1;
                this.sumatoriaSalidas = this.sumatoriaSalidas + this.TIME;
            }
            else
            {
                this.SeniorsSolicitudesConsultadas[_senior].Cantidad = this.SeniorsSolicitudesConsultadas[_senior].Cantidad - 1;
                var juniorQueConsulto = this.SeniorsSolicitudesConsultadas[_senior].JuniorsConsultantes.First();
                this.JuniorsSolicitudesRespondidas[juniorQueConsulto] = this.JuniorsSolicitudesRespondidas[juniorQueConsulto] + 1;
                this.SeniorsSolicitudesConsultadas[_senior].JuniorsConsultantes.RemoveAt(0);

                if(this.JuniorsSolicitudesRespondidas[juniorQueConsulto] == 1 && this.TPSJ[juniorQueConsulto] == this.HV)
                {
                    var tiempoDeAtencion = this.generarTiempoAtencionJunior();
                    this.TPSJ[juniorQueConsulto] = this.TIME + tiempoDeAtencion;
                    this.IndicadoresOperacionJunior[juniorQueConsulto] = "Respuesta";

                    this.sumatoriaTiempoOciosoJuniors[juniorQueConsulto] = this.sumatoriaTiempoOciosoJuniors[juniorQueConsulto] + (this.TIME - this.inicioTiempoOciosoJuniors[juniorQueConsulto]);
                }

            }

            if(this.SeniorsSolicitudesConsultadas[_senior].Cantidad >= 1)
            {
                this.IndicadoresOperacionSenior[_senior] = "Consulta";
                var tiempoDeAtencion = this.generarTiempoAtencionSenior();
                this.TPSS[_senior] = this.TIME + tiempoDeAtencion;
            }
            else
            {
                if(this.SolicitudesSeniorNormales[_senior] >= 1 && this.SeniorsSolicitudesConsultadas[_senior].Cantidad == 0)
                {
                    this.IndicadoresOperacionSenior[_senior] = "Normal";
                    var tiempoDeAtencion = this.generarTiempoAtencionSenior();
                    this.TPSS[_senior] = this.TIME + tiempoDeAtencion;
                }
                else
                {
                    this.TPSS[_senior] = this.HV;
                    this.inicioTiempoOciosoSeniors[_senior] = this.TIME;
                }
            }

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

        public int getSeniorConMenosConsultas()
        {
            var menorCantConsultas = this.SeniorsSolicitudesConsultadas.Min();
            return this.SeniorsSolicitudesConsultadas.FindIndex(s => s == menorCantConsultas);
        }

        public double generarRandom()
        {
            return this.generadorRandoms.NextDouble();
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
