using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizador_Lexico
{
    public class Identificadores
    {
        public string Cadena { get; set; }
        public List<char> Lineas { get; set; }
        public int Codigo { get; set; }

        public Identificadores(string cadena, List<char> lineas, int codigo)
        {
            Cadena = cadena;
            Lineas = lineas;
            Codigo = codigo;
        }
    }
}
