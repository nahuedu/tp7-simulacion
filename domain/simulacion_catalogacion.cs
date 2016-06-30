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

        public double HV = 9999999999999999999; // Son 19 nueves.

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
        private Random generadorRandomsIA = new Random();

        public Resultado simular(int _cantJuniors, int _cantSeniors)
        {
            this.setInitialConditions(_cantJuniors, _cantSeniors);

            do
            {
                this.procesarEventos();
            } while (this.TIME <= this.TFINAL);

            // Vaciamiento
            if (this.calcularSolicitudesPendientes() > 0)
                this.TPLL = this.HV;

            while (calcularSolicitudesPendientes() > 0)
                this.procesarEventos();
            // Fin del vaciamiento

            return this.calcularResultados();
        }

        public void procesarEventos()
        {
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
        }

        public int calcularSolicitudesPendientes()
        {
            var cantSJNP = this.SolicitudesJuniorNormales.Sum();
            var cantSJRP = this.JuniorsSolicitudesRespondidas.Sum();
            var cantSSNP = this.SolicitudesSeniorNormales.Sum();
            var cantSSCP = this.SeniorsSolicitudesConsultadas.Sum(x => x.Cantidad);

            var cantSolPendientes = cantSJNP + cantSJRP + cantSSNP + cantSSCP;

            return cantSolPendientes;
        }

        public void setInitialConditions(int _cantJuniors, int _cantSeniors)
        {
            this.TIME = 1;
            this.TFINAL = 525600; // 1 año.

            this.TPLL = 1;

            this.TPSJ = new List<double>(_cantJuniors);
            for (int i = 0; i < _cantJuniors; i++)
                this.TPSJ.Add(this.HV);

            this.TPSS = new List<double>(_cantSeniors);
            for (int i = 0; i < _cantSeniors; i++)
                this.TPSS.Add(this.HV);

            this.SolicitudesJuniorNormales = new List<int>(_cantJuniors);
            for (int i = 0; i < _cantJuniors; i++)
                this.SolicitudesJuniorNormales.Add(0);

            this.SolicitudesSeniorNormales = new List<int>(_cantSeniors);
            for (int i = 0; i < _cantSeniors; i++)
                this.SolicitudesSeniorNormales.Add(0);

            this.JuniorsSolicitudesRespondidas = new List<int>(_cantJuniors);
            for (int i = 0; i < _cantJuniors; i++)
                this.JuniorsSolicitudesRespondidas.Add(0);

            this.SeniorsSolicitudesConsultadas = new List<Consulta>(_cantSeniors);
            for (int i = 0; i < _cantSeniors; i++)
                this.SeniorsSolicitudesConsultadas.Add(new Consulta(0));

            this.IndicadoresOperacionJunior = new List<string>(_cantJuniors);
            for (int i = 0; i < _cantJuniors; i++)
                this.IndicadoresOperacionJunior.Add("");

            this.IndicadoresOperacionSenior = new List<string>(_cantSeniors);
            for (int i = 0; i < _cantSeniors; i++)
                this.IndicadoresOperacionSenior.Add("");

            this.numeroTotalDeSolicitudes = 0;
            this.sumatoriaLlegadas = 0;
            this.sumatoriaSalidas = 0;

            this.inicioTiempoOciosoJuniors = new List<double>(_cantJuniors);
            for (int i = 0; i < _cantJuniors; i++)
                this.inicioTiempoOciosoJuniors.Add(1); // Se settea igual al Tiempo Inicial

            this.inicioTiempoOciosoSeniors = new List<double>(_cantSeniors);
            for (int i = 0; i < _cantSeniors; i++)
                this.inicioTiempoOciosoSeniors.Add(1); // Se settea igual al Tiempo Inicial

            this.sumatoriaTiempoOciosoJuniors = new List<double>(_cantJuniors);
            for (int i = 0; i < _cantJuniors; i++)
                this.sumatoriaTiempoOciosoJuniors.Add(0);

            this.sumatoriaTiempoOciosoSeniors = new List<double>(_cantSeniors);
            for (int i = 0; i < _cantSeniors; i++)
                this.sumatoriaTiempoOciosoSeniors.Add(0);
        }

        public void simularLlegada()
        {
            this.TIME = this.TPLL;
            var intervalosEntreArribos = this.generarIntervaloEntreArribos();
            this.TPLL = this.TIME + intervalosEntreArribos;
            int juniorMenosOcupadoConSolNormales = obtenerJuniorConMenorCantidadDeSolicitudesNormales();
            int seniorMenosOcupadoConSolNormales = obtenerSeniorConMenorCantidadDeSolicitudesNormales();

            numeroTotalDeSolicitudes = numeroTotalDeSolicitudes + 1;
            sumatoriaLlegadas = sumatoriaLlegadas + this.TIME;

            int cantSolNormalesJuniorMenosOcupado = this.SolicitudesJuniorNormales[juniorMenosOcupadoConSolNormales];
            int cantSolNormalesSeniorMenosOcupado = this.SolicitudesSeniorNormales[seniorMenosOcupadoConSolNormales];

            if (cantSolNormalesJuniorMenosOcupado <= cantSolNormalesSeniorMenosOcupado)
                this.enviarNuevaSolicitudAJunior(juniorMenosOcupadoConSolNormales);
            else
                this.enviarNuevaSolicitudASenior(seniorMenosOcupadoConSolNormales);
        }

        public void simularSalidaJunior(int _junior)
        {
            this.TIME = this.TPSJ[_junior];

            if(this.IndicadoresOperacionJunior[_junior] == "Normal")
            {
                this.SolicitudesJuniorNormales[_junior] = this.SolicitudesJuniorNormales[_junior] - 1;
                var rand = this.generarRandom();

                if (rand <= 0.2) // Se manda a consultar
                {
                    var seniorASerConsultado = this.getSeniorConMenosConsultas();
                    this.SeniorsSolicitudesConsultadas[seniorASerConsultado].Cantidad = this.SeniorsSolicitudesConsultadas[seniorASerConsultado].Cantidad + 1;
                    this.SeniorsSolicitudesConsultadas[seniorASerConsultado].JuniorsConsultantes.Add(_junior);

                    if((this.SeniorsSolicitudesConsultadas[seniorASerConsultado].Cantidad == 1) && (this.TPSS[seniorASerConsultado] == this.HV))
                    {
                        this.IndicadoresOperacionSenior[seniorASerConsultado] = "Consulta";
                        var tiempoDeAtencion = this.generarTiempoAtencionSenior();
                        this.TPSS[seniorASerConsultado] = this.TIME + tiempoDeAtencion;

                        sumatoriaTiempoOciosoSeniors[seniorASerConsultado] = sumatoriaTiempoOciosoSeniors[seniorASerConsultado] + (this.TIME - this.inicioTiempoOciosoSeniors[seniorASerConsultado]);
                    }
                }
                else
                {
                    sumatoriaSalidas = sumatoriaSalidas + this.TIME;
                }

            }
            else
            {
                this.JuniorsSolicitudesRespondidas[_junior] = this.JuniorsSolicitudesRespondidas[_junior] - 1;
                sumatoriaSalidas = sumatoriaSalidas + this.TIME;
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
                sumatoriaSalidas = sumatoriaSalidas + this.TIME;
            }
            else
            {
                this.SeniorsSolicitudesConsultadas[_senior].Cantidad = this.SeniorsSolicitudesConsultadas[_senior].Cantidad - 1;

                var juniorQueConsulto = this.SeniorsSolicitudesConsultadas[_senior].JuniorsConsultantes.First();
                this.JuniorsSolicitudesRespondidas[juniorQueConsulto] = this.JuniorsSolicitudesRespondidas[juniorQueConsulto] + 1;
                this.SeniorsSolicitudesConsultadas[_senior].JuniorsConsultantes.RemoveAt(0);

                if((this.JuniorsSolicitudesRespondidas[juniorQueConsulto] == 1) && this.TPSJ[juniorQueConsulto] == this.HV)
                {
                    var tiempoDeAtencion = this.generarTiempoAtencionJunior();
                    this.TPSJ[juniorQueConsulto] = this.TIME + tiempoDeAtencion;
                    this.IndicadoresOperacionJunior[juniorQueConsulto] = "Respuesta";

                    sumatoriaTiempoOciosoJuniors[juniorQueConsulto] = sumatoriaTiempoOciosoJuniors[juniorQueConsulto] + (this.TIME - this.inicioTiempoOciosoJuniors[juniorQueConsulto]);
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

            return this.SolicitudesJuniorNormales.FindIndex(j => j == min);
        }

        public int obtenerSeniorConMenorCantidadDeSolicitudesNormales()
        {
            var min = this.SolicitudesSeniorNormales.Min();

            return this.SolicitudesSeniorNormales.FindIndex(s => s == min);
        }

        public void enviarNuevaSolicitudAJunior(int _junior)
        {
            this.SolicitudesJuniorNormales[_junior] = this.SolicitudesJuniorNormales[_junior] + 1;

            if (this.SolicitudesJuniorNormales[_junior] == 1 && this.TPSJ[_junior] == this.HV) // Generar atención del Junior.
            {
                this.IndicadoresOperacionJunior[_junior] = "Normal";
                var tiempoAtencion = this.generarTiempoAtencionJunior();
                this.TPSJ[_junior] = this.TIME + tiempoAtencion;

                sumatoriaTiempoOciosoJuniors[_junior] = sumatoriaTiempoOciosoJuniors[_junior] + (this.TIME - this.inicioTiempoOciosoJuniors[_junior]);
            }
        }

        public void enviarNuevaSolicitudASenior(int _senior)
        {
            this.SolicitudesSeniorNormales[_senior] = this.SolicitudesSeniorNormales[_senior] + 1;

            if (this.SolicitudesSeniorNormales[_senior] == 1 && this.TPSS[_senior] == this.HV) // Generar atención del Senior
            {
                this.IndicadoresOperacionSenior[_senior] = "Normal";
                var tiempoAtencion = this.generarTiempoAtencionSenior();
                this.TPSS[_senior] = this.TIME + tiempoAtencion;

                sumatoriaTiempoOciosoSeniors[_senior] = sumatoriaTiempoOciosoSeniors[_senior] + (this.TIME - this.inicioTiempoOciosoSeniors[_senior]);
            }
        }

        public int getSeniorConMenosConsultas()
        {
            var menorCantConsultas = this.SeniorsSolicitudesConsultadas.Min(x => x.Cantidad);
            return this.SeniorsSolicitudesConsultadas.FindIndex(s => s.Cantidad == menorCantConsultas);
        }

        public double generarRandom()
        {
            return this.generadorRandoms.NextDouble();
        }

        // ----------------------------- FDPs ------------------------------------------
        public double generarIntervaloEntreArribos()
        {
            //double n1 = 3.2011;
            //double n2 = 0.99389;
            //var r = this.generadorRandomsIA.NextDouble();

            //var IA = Math.Pow(Math.E, (Math.Log(n1) - ((Math.Log((1 / r) - 1) / n2))));

            //return IA;

            var ta = this.generadorRandoms.Next(1, 15);
            return ta;

        }

        public double generarTiempoAtencionJunior()
        {
            var ta = this.generadorRandoms.Next(10, 40);
            return ta;
        }

        public double generarTiempoAtencionSenior()
        {
            var ta = this.generadorRandoms.Next(5, 25);
            return ta;
        }

        // -------------------- Calculo de Resultados --------------------

        public Resultado calcularResultados()
        {
            var promedioPermanencia = (this.sumatoriaSalidas - this.sumatoriaLlegadas) / this.numeroTotalDeSolicitudes;
            var porcentajeTOJ = (this.sumatoriaTiempoOciosoJuniors.Average() / this.TIME) * 100;
            var porcentajeTOS = (this.sumatoriaTiempoOciosoSeniors.Average() / this.TIME) * 100;

            var resultado = new Resultado();
            resultado.promedioPermanenciaSistema = promedioPermanencia;
            resultado.porcentajeGeneralTOJ = porcentajeTOJ;
            resultado.porcentajeGeneralTOS = porcentajeTOS;

            return resultado;
        }
    }
}
