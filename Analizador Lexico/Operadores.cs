using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizador_Lexico
{
    public class Operadores
    {
        public char Caracter {  get; set; }
        public List<char> Lineas { get; set; }

        public Operadores(char caracter, List<char> lineas)
        {
            Caracter = caracter;
            Lineas = lineas;
        }
    }
}
