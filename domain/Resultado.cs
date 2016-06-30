using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tp7_simulacion.domain
{
    public class Resultado
    {
        public double promedioPermanenciaSistema { get; set; }
        public double porcentajeGeneralTOJ { get; set; }
        public double porcentajeGeneralTOS { get; set; }

        public List<double> porcentajeTOJuniors { get; set; }
        public List<double> porcentajeTOSeniors { get; set; }
    }
}
