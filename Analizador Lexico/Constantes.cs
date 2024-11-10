using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizador_Lexico
{
    public class Constantes
    {
        public string Cadena { get; set; }
        public char Lineas { get; set; }
        public int Codigo { get; set; }

        public Constantes(string cadena, char lineas, int codigo)
        {
            Cadena = cadena;
            Lineas = lineas;
            Codigo = codigo;
        }
    }
}
