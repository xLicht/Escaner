using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analizador_Lexico
{
    public class Errores
    {
        public void ElementoInvalido(RichTextBox txtBox, TextBox txtError, int linea, int contadorPosicion)
        {
            int posicionCursor = contadorPosicion;

            if (posicionCursor > 0)
            {
                char[] delims = { '(', ')', '\n', '*', '/', '+', '-' };

                int inicioIndice = txtBox.Text.LastIndexOfAny(delims, posicionCursor - 1);

                if (inicioIndice == -1)
                    inicioIndice = 0;
                else
                    inicioIndice += 1;

                int finIndice = txtBox.Text.IndexOfAny(delims, posicionCursor);

                if (finIndice == -1)
                    finIndice = txtBox.Text.Length;

                txtBox.SelectionStart = inicioIndice;
                txtBox.SelectionLength = finIndice - inicioIndice;

                txtBox.Focus();
            }
            txtError.BackColor = Color.FromArgb(255, 137, 137);
            txtError.Text = "1:102 Error en Linea " + linea + " Elemento Invalido";
        }
        public void SimboloDesconocido (RichTextBox txtBox, TextBox txtError, int linea, int contadorPosicion)
        {
            int posicionCursor = contadorPosicion;
            if (posicionCursor > 0)
            {
                txtBox.SelectionStart = posicionCursor - 1;
                txtBox.SelectionLength = 1;
                txtBox.Focus();
            }
            txtError.BackColor = Color.FromArgb(255, 137, 137);
            txtError.Text = "1:101 Error en Linea " + linea + " Simbolo Desconocido";
        }
        public void SinError (TextBox txtError)
        {
            txtError.BackColor = Color.FromArgb(232, 255, 223);
            txtError.Text = "1:100 Sin Error";
        }
    }
}
