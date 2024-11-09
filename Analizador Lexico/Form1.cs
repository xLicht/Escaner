﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Analizador_Lexico
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int[,] tablaT =
        {
//            (  )  ;  +  -  *  /  L  d  E  .
            { 9, 9, 9, 9, 9, 9, 9, 2, 1, 2, 5}, // q0
            { 9, 9, 9, 9, 9, 9, 9, 8, 1, 3, 5}, // q1
            { 9, 9, 9, 9, 9, 9, 9, 2, 7, 2, 8}, // q2
            { 8, 8, 8, 8, 8, 8, 8, 8, 4, 8, 8}, // q3
            { 9, 9, 9, 9, 9, 9, 9, 8, 4, 8, 8}, // q4
            { 8, 8, 8, 8, 8, 8, 8, 8, 6, 8, 8}, // q5
            { 9, 9, 9, 9, 9, 9, 9, 8, 6, 3, 8}, // q6
            { 9, 9, 9, 9, 9, 9, 9, 8, 7, 8, 8}, // q7
            { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9}, // q8
            { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9}, // q9


        };
        int EDO = 0;
        int count = 0;
        int tamCad = 0;
        char[] let = {
            'A', 'B', 'C', 'D', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Ñ', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'ñ', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };
        char[] nums = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        char parAb = '(';
        char E = 'E';
        char puntoComa = ';';
        char punto = '.';
        char mas = '+';
        char menos = '-';
        char por = '*';
        char div = '/';
        char enter = '\n';
        char espacio = ' ';
        int lineCount = 1;
        //
        int contador = 0;
        string cadenaF = "";

        List<int> historialEstados = new List<int>();
        private void BtnStart_Click(object sender, EventArgs e)
        {
            contador = 0;
            bandera = false;
            EDO = 0;
            count = 0;
            tamCad = 0;
            lineCount = 1;
            historialEstados.Clear();
            dgvTLexica.Rows.Clear();
            anteriorCarac = '\0';

            string textoTotal = TxtText.Text + " ";
            tamCad = textoTotal.Length;
            Automata(tablaT, ref EDO, count, tamCad, textoTotal);

            int filaNum = 1;
            foreach(DataGridViewRow row in dgvTLexica.Rows)
            {
                row.Cells[0].Value = filaNum;
                filaNum++;
            }
        }
        public void agregarFila(int lineaCoun, string cadeF, string tipo)
        {
            dgvTLexica.Rows.Add(null, lineaCoun, cadeF, tipo);
        }

        public void Automata(int[,] TT, ref int edo, int cont, int tamCad, string cad)
        {
            List<char> cadena = new List<char>();
            while (cont < tamCad)
            {
                char car = cad[cont];
                int carPointer = CaracterCheck(car);
                edo = TT[edo, carPointer];
                contador++;
                ManejarCadena(ref cadena, edo, car);
                cont++;
            }
        }
        char anteriorCarac;
        bool bandera = false;
        public void ManejarCadena (ref List<char> cadena, int estado, char car) 
        {
            //FALTA IMPLEMENTAR MAS CAMPO DE ESTADO
            
            if (car == '\n')
                lineCount++;
            if (estado != 9 && estado != 8)
            {
                cadena.Add(car);
            }
            if (estado == 9)
            {
                if (bandera == true) 
                {
                    cadena.Add(car);
                    if (tamCad == contador)
                    {
                        foreach (char caract in cadena)
                        {
                            cadenaF += caract;
                        }
                        if (cadenaF != "")
                        {
                            Aceptados(historialEstados.Last(), cadenaF, lineCount);
                        }
                        cadena.Clear();
                        EDO = 0;
                        anteriorCarac = car;
                        historialEstados.Add(EDO);
                    }
                }
                else
                {

                    //CheckPuntosFinales(ref cadena, car);
                    foreach (char caract in cadena)
                    {
                        cadenaF += caract;
                    }
                    if (cadenaF != "")
                    Aceptados(historialEstados.Last(), cadenaF, lineCount);
                    cadena.Clear();
                    EDO = 0;
                }
            }
            
            if (estado == 8)
            {
                bandera = true;
                cadena.Add(car);
            }
            if (bandera != true)
            {
               anteriorCarac = car;
              historialEstados.Add(EDO);
            }
        }
        //Metodo Error
        private bool Error(bool bandera)
        {
            if (bandera)
            {              
                return true; // Indica que hay un error y debe salir del bloque if
            }
            return false; // Indica que no hay error y puede continuar
        }



        // No necesario
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
        // Talvez no necesario
        public void Aceptados(int estado, string cadenaF, int lineaCount)
        {
            switch (estado)
            {
                case 1:
                    agregarFila(lineaCount, cadenaF, "Natural");
                    break;
                case 6:
                    agregarFila(lineaCount, cadenaF, "Real");
                    break;
                case 4:
                    agregarFila(lineaCount, cadenaF, "Exponencial");
                    break;
                case 2:
                    agregarFila(lineaCount, cadenaF, "Identificador");
                    break;
                case 7:
                    agregarFila(lineaCount, cadenaF, "Identificador");
                    break;
                case 8:
                    agregarFila(lineaCount, cadenaF, "Rechazo");
                    break;
                case 9:
                    agregarFila(lineaCount, cadenaF, "Guardado");
                    break;
                default:
                    agregarFila(lineaCount, cadenaF, "Invalida");
                    break;
            }
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
            return 0;
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

            anteriorCarac = '\0';
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
