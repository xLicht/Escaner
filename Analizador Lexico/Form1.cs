using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analizador_Lexico
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int[,] tablaTrans =
        {
            {12 ,2, 12, 12, 12, 12, 12 , 12}, //q0
            {1 ,1, 1, 1, 1, 1, 1 , 1}, //q1
            {1 ,2, 5, 3, 3, 7, 12, 12 }, //q2
            {1 ,4, 1, 12, 12, 1, 12, 12 }, //q3
            {1 ,4, 10, 13, 13, 7, 12, 12 }, //q4
            {1 ,6, 1, 12, 12, 1, 12 , 12}, //q5
            {1 ,6, 1, 8, 8, 1, 12, 12 }, //q6
            {1 ,1, 1, 12, 12, 1 , 12, 12}, //q7
            {1 ,9, 1, 12, 12, 1, 12, 12 }, //q8
            {1 ,9, 1, 1, 1, 1, 12, 12 }, //q9
            {1 ,11, 1, 1, 1, 1, 1, 1 }, //q10
            {1 ,11, 1, 12, 12, 1, 12, 12 }, //q11
            {12 ,12, 12, 12, 12, 12, 12, 12 }, //q12
            {13 ,13, 13, 13, 13, 13, 12, 12 }, //q13
        };
        int EDO = 0;
        int count = 0;
        int tamCad = 0;
        char[] let = {
            'A', 'B', 'C', 'D', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Ñ', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'ñ', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };
        char[] nums = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        char porcentaje = '%';
        char E = 'E';
        char coma = ',';
        char punto = '.';
        char enter = '\n';
        char espacio = ' ';
        int lineCount = 1;
        List<int> historialEstados = new List<int>();
        private void BtnStart_Click(object sender, EventArgs e)
        {
            EDO = 0;
            count = 0;
            tamCad = 0;
            lineCount = 1;
            historialEstados.Clear();
            dgvRes.Rows.Clear();
            anteriorCarac = '\0';

            string textoTotal = TxtText.Text + " ";
            tamCad = textoTotal.Length;
            Automata(tablaTrans, ref EDO, count, tamCad, textoTotal);

            int filaNum = 1;
            foreach(DataGridViewRow row in dgvRes.Rows)
            {
                row.Cells[0].Value = filaNum;
                filaNum++;
            }
        }
        public void agregarFila(int lineaCoun, string cadeF, string tipo)
        {
            dgvRes.Rows.Add(null, lineaCoun, cadeF, tipo);
        }

        public void Automata(int[,] TT, ref int edo, int cont, int tamCad, string cad)
        {
            List<char> cadena = new List<char>();
            while (cont < tamCad)
            {
                char car = cad[cont];
                int carPointer = CaracterCheck(car);
                edo = TT[edo, carPointer];
                ManejarCadena(ref cadena, edo, car);
                cont++;
            }
        }
        char anteriorCarac;
        public void ManejarCadena (ref List<char> cadena, int estado, char car) 
        {
            if (car == '\n')
                lineCount++;
            if (estado != 1 && estado != 12)
            {
                cadena.Add(car);
            }
            if (estado == 12)
            {
                string cadenaF = "";
                CheckPuntosFinales(ref cadena, car);
                foreach (char caract in cadena)
                {
                    cadenaF += caract;
                }
                if (cadenaF != "")
                    Aceptados(historialEstados.Last(), cadenaF, lineCount);
                cadena.Clear();
                EDO = 0;
            }
            if (estado == 1)
            {
                string cadenaF = "";
                CheckPuntosFinales(ref cadena, car);
                cadena.Add(car);
                foreach (char caract in cadena)
                {
                    cadenaF += caract;
                }
                if (cadenaF != "")
                    Aceptados(1, cadenaF, lineCount);
                cadena.Clear();
                EDO = 0;
            }
            anteriorCarac = car;
            historialEstados.Add(EDO);
        }
        public void CheckPuntosFinales(ref List<char> cadena, char car)
        {
            if (anteriorCarac == '.' && car == ' ' && cadena.Count > 0)
            {
                historialEstados.RemoveAt(historialEstados.Count - 1);
                cadena.RemoveAt(cadena.Count-1);
            }
            if (anteriorCarac == ',' && car == ' ' && cadena.Count > 0)
            {
                historialEstados.RemoveAt(historialEstados.Count - 1);
                cadena.RemoveAt(cadena.Count-1);
            }
        }
        public void Aceptados(int estado, string cadenaF, int lineaCount)
        {
            switch (estado)
            {
                case 2:
                    agregarFila(lineaCount, cadenaF, "Natural");
                    break;
                case 4:
                    agregarFila(lineaCount, cadenaF, "Real");
                    break;
                case 6:
                    agregarFila(lineaCount, cadenaF, "Exponencial");
                    break;
                case 9:
                    agregarFila(lineaCount, cadenaF, "Exponencial");
                    break;
                case 11:
                    agregarFila(lineaCount, cadenaF, "Exponencial");
                    break;
                case 7:
                    agregarFila(lineaCount, cadenaF, "Porcentual");
                    break;
                default:
                    agregarFila(lineaCount, cadenaF, "Invalida");
                    break;
            }
        }
        public int CaracterCheck (char cara)
        {
            if (let.Contains(cara)) return 0;
            if (nums.Contains(cara)) return 1;
            if (E == cara) return 2;
            if (coma == cara) return 3;
            if (punto == cara) return 4;
            if (porcentaje == cara) return 5;
            if (espacio == cara) return 6;
            if (enter == cara) return 7;
            return 6;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetearSistema();
        }
        private void ResetearSistema()
        {
            EDO = 0;
            count = 0;
            tamCad = 0;
            lineCount = 1;

            historialEstados.Clear();

            dgvRes.Rows.Clear();

            TxtText.Clear();

            anteriorCarac = '\0';
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
