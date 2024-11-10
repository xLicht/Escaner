using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Analizador_Lexico
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Errores Error = new Errores();
        int[,] tablaT =
        {
//            (  )  ;  +  -  *  /  L  d  E  .  n  ?
            { 9, 9, 9, 9, 9, 9, 9, 2, 1, 2, 5, 9, 10}, // q0
            { 9, 9, 9, 9, 9, 9, 9, 8, 1, 3, 5, 9, 10}, // q1
            { 9, 9, 9, 9, 9, 9, 9, 2, 7, 2, 8, 9, 10}, // q2
            { 8, 8, 8, 8, 8, 8, 8, 8, 4, 8, 8, 9, 10}, // q3
            { 9, 9, 9, 9, 9, 9, 9, 8, 4, 8, 8, 9, 10}, // q4
            { 8, 8, 8, 8, 8, 8, 8, 8, 6, 8, 8, 9, 10}, // q5
            { 9, 9, 9, 9, 9, 9, 9, 8, 6, 3, 8, 9, 10}, // q6
            { 9, 9, 9, 9, 9, 9, 9, 8, 7, 8, 8, 9, 10}, // q7
            { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 10}, // q8
            { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 10}, // q9
            { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 10}, // q10


        };
        int EDO;
        int count;
        int tamCad;
        char[] let = {
            'A', 'B', 'C', 'D', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Ñ', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'ñ', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };
        char[] nums = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        List<Operadores> Operad;
        List<Delimitadores> Delim;
        List<Identificadores> Idents;
        List<Constantes> Constan;
        int delimVal;
        int Oper;
        int iden;
        int cons;
        int lineCount;
        bool bandera = false;
        bool yafue = false;
        bool banderaDesconocido = false;
        List<int> historialEstados = new List<int>();
        private void BtnStart_Click(object sender, EventArgs e)
        {
            bandera = false;
            EDO = 0;
            count = 0;
            tamCad = 0;
            lineCount = 1;
            historialEstados.Clear();
            dgvTLexica.Rows.Clear();
            dgvTCons.Rows.Clear();
            dgvTIden.Rows.Clear();
            delimVal = 50;
            Oper = 70;
            iden = 101;
            cons = 201;
            yafue = false;
            banderaDesconocido = false;
            Constan = new List<Constantes>();
            Idents = new List<Identificadores>();
            Operad = new List<Operadores>
            {new Operadores('+', new List<char>()), new Operadores('-', new List<char>()), new Operadores('*', new List<char>()), new Operadores('/', new List<char>())};
            Delim = new List<Delimitadores>
            {new Delimitadores('(', new List<char>()), new Delimitadores(')', new List<char>()), new Delimitadores(';', new List<char>())};

            string textoTotal = TxtText.Text + '\n';
            tamCad = textoTotal.Length;
            Automata(tablaT, ref EDO, count, tamCad, textoTotal);

            int filaNum = 1;
            foreach(DataGridViewRow row in dgvTLexica.Rows)
            {
                row.Cells[0].Value = filaNum;
                filaNum++;
            }
            agregarFilaIdent(Idents);
            agregarFilaCons(Constan);
            
        }
        public void agregarFilaIdent (List<Identificadores> identif)
        {
            foreach(var item in identif)
            {
                string lineasTxt = "";
                foreach(var c in item.Lineas)
                {
                    lineasTxt += c.ToString() + ", ";
                }
                dgvTIden.Rows.Add(item.Cadena, item.Codigo, lineasTxt.Remove(lineasTxt.Length -2));
            }
        }
        public void agregarFilaCons(List<Constantes> constants)
        {
            foreach(var item in constants)
            {
                dgvTCons.Rows.Add(item.Cadena, item.Codigo, item.Lineas);
            }
        }
        public void agregarFila(int lineaCoun, string cadeF, string tipo, int codigo)
        {
            dgvTLexica.Rows.Add(null, lineaCoun, cadeF, tipo, codigo);
        }

        public void Automata(int[,] TT, ref int edo, int cont, int tamCad, string cad)
        {
            List<char> cadena = new List<char>();
            while (cont < tamCad)
            {
                char car = cad[cont];
                int carPointer = CaracterCheck(car);
                edo = TT[edo, carPointer];
                ManejarCadena(ref cadena, edo, car, cont);
                cont++;
            }
            if (bandera == false && banderaDesconocido == false && yafue == false)
                Error.SinError(txtError);
        }
        
        public void ManejarCadena (ref List<char> cadena, int estado, char car, int conta) 
        {
            if (estado != 9 && estado != 8 && estado != 10)
            {
                cadena.Add(car);
            }
            if (estado == 9)
            {
                string cadenaF = "";
                foreach (char caract in cadena)
                {
                    cadenaF += caract;
                }
                if (bandera && yafue == false)
                {
                    Error.ElementoInvalido(TxtText, txtError, lineCount, conta);
                    yafue = true;
                    return;
                }
                else if (banderaDesconocido && yafue == false)
                {
                    Error.SimboloDesconocido(TxtText, txtError, lineCount, conta);
                    yafue = true;
                    return;
                }
                else if (yafue == false)
                {
                    if (cadenaF != "")
                    {
                        Aceptados(historialEstados.Last(), cadenaF, lineCount);
                    }
                    if (car == '+' || car == '-' || car == '*' || car == '/') CheckOperadores(car, lineCount);
                    if (car == '(' || car == ')' || car == ';') CheckDelimitadores(car, lineCount);
                    cadena.Clear();
                    EDO = 0;
                }
            }
            if (estado == 8)
            {
                bandera = true;
                cadena.Add(car);
            }
            if (estado == 10)
            {
                banderaDesconocido = true;
                cadena.Add(car);
                EDO = 0;
            }
            if (car == '\n')
                lineCount++;
            historialEstados.Add(EDO);
        }
        public void CheckOperadores(char cara, int lineaCoun)
        {
            if (cara == '+')
            {
                Operad[0].Lineas.Add((char)(lineCount + '0'));
                agregarFila(lineaCoun, cara.ToString(), "7", Oper);
            }
            if (cara == '-')
            {
                Operad[1].Lineas.Add((char)(lineCount + '0'));
                agregarFila(lineaCoun, cara.ToString(), "7", Oper + 1);
            }
            if (cara == '*')
            {
                Operad[2].Lineas.Add((char)(lineCount + '0'));
                agregarFila(lineaCoun, cara.ToString(), "7", Oper + 2);
            }
            if (cara == '/')
            {
                Operad[3].Lineas.Add((char)(lineCount + '0'));
                agregarFila(lineaCoun, cara.ToString(), "7", Oper + 3);
            }

        }
        public void CheckDelimitadores(char cara, int lineaCoun)
        {
            if (cara == '(')
            {
                Delim[0].Lineas.Add((char)(lineCount + '0'));
                agregarFila(lineaCoun, cara.ToString(), "5", delimVal);
            }
            if (cara == ')')
            {
                Delim[1].Lineas.Add((char)(lineCount + '0'));
                agregarFila(lineaCoun, cara.ToString(), "5", delimVal + 1);
            }
            if (cara == ';')
            {
                Delim[2].Lineas.Add((char)(lineCount + '0'));
                agregarFila(lineaCoun, cara.ToString(), "5", delimVal + 2);
            }
        }
        public void Aceptados(int estado, string cadenaF, int lineaCount)
        {
            switch (estado)
            {
                case 1:
                    agregarFila(lineaCount, cadenaF, "2", cons);
                    Constan.Add(new Constantes(cadenaF, (char)(lineaCount + '0'), cons));
                    cons++;
                    break;
                case 6:
                    agregarFila(lineaCount, cadenaF, "2", cons);
                    Constan.Add(new Constantes(cadenaF, (char)(lineaCount + '0'), cons));
                    cons++;
                    break;
                case 4:
                    agregarFila(lineaCount, cadenaF, "2", cons);
                    Constan.Add(new Constantes(cadenaF, (char)(lineaCount + '0'), cons));
                    cons++;
                    break;
                case 2:
                    if (IsFirstTime(cadenaF, Idents) == -1)
                    {
                        Idents.Add(new Identificadores(cadenaF, new List<char> { (char)(lineaCount + '0') }, iden));
                        agregarFila(lineaCount, cadenaF, "1", iden);
                    }
                    else
                    {
                        Idents[IsFirstTime(cadenaF, Idents)].Lineas.Add((char)(lineaCount + '0'));
                        agregarFila(lineaCount, cadenaF, "1", Idents[IsFirstTime(cadenaF, Idents)].Codigo);
                    }
                    iden++;
                    break;
                case 7:
                    if (IsFirstTime(cadenaF, Idents) == -1)
                    {
                        Idents.Add(new Identificadores(cadenaF, new List<char> { (char)(lineaCount + '0') }, iden));
                        agregarFila(lineaCount, cadenaF, "1", iden);
                    }
                    else
                    {
                        Idents[IsFirstTime(cadenaF, Idents)].Lineas.Add((char)(lineaCount + '0'));
                        agregarFila(lineaCount, cadenaF, "1", Idents[IsFirstTime(cadenaF, Idents)].Codigo);
                    }
                    iden++;
                    break;
                default:
                    agregarFila(lineaCount, cadenaF, "Invalida", 0);
                    break;
            }
        }
        public int IsFirstTime(string cadena, List<Identificadores> lista)
        {
            for (int i = 0; i<lista.Count; i++)
            {
                if (lista[i].Cadena == cadena)
                    return i;
            }
            return -1;
        }
        public int CaracterCheck (char cara)
        {
            if (cara == '(') return 0;
            if (cara == ')') return 1;
            if (cara == ';') return 2;
            if (cara == '+') return 3;
            if (cara == '-') return 4;
            if (cara == '*') return 5;
            if (cara == '/') return 6;
            if (let.Contains(cara)) return 7;
            if (nums.Contains(cara)) return 8;
            if (cara == 'E'|| cara == 'e') return 9;
            if (cara == '.') return 10;
            if (cara == '\n') return 11;
            return 12; // algo que no va
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

            dgvTLexica.Rows.Clear();

            TxtText.Clear();

        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cmbDemos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDemos.SelectedIndex == 0)
            {
                TxtText.Text = "(X1+B2);\r\n(Y1+B3*C4)+D4;\r\n(((VAR2+X1)));\r\n(PESO+(CARGO*DIF2));\r\n((X2+45-78*(12.34*3.56E45)+B2;";
            }
            if (cmbDemos.SelectedIndex == 1)
            {
                TxtText.Text = "(X1+B2);\r\n(Y1+B3*C4)+D4;\r\n(((VAR2+X1)));\r\n(PESO+(CARGO*DIF2));&\r\n((X2+45-78*(12.34*3.56E45)+B2;";
            }
            if(cmbDemos.SelectedIndex == 2)
            {
                TxtText.Text = "(X1+B2);\r\n(Y1+B3*C4)+D4;\r\n(((VAR2+X1)));\r\n(PESO+(CARGO*DIF2));\r\n((X2+45-78*(12.34*3.56EA45)+B2;";
            }
        }
    }
}
