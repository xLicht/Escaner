using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analizador_Lexico
{
    public class Errores
    {
        public void ElementoInvalido(RichTextBox txtBox, TextBox txtError, int linea)
        {
            int posicionCursor = txtBox.SelectionStart;
            if (posicionCursor > 0)
            {
                char[] delims = { '(', ')', '\n', '*', '/', '+', '-' };
                int inicioIndice = txtBox.Text.LastIndexOfAny(delims, posicionCursor - 1);

                if (inicioIndice != -1)
                {
                    txtBox.SelectionStart = inicioIndice + 1;
                    txtBox.SelectionLength = posicionCursor - inicioIndice - 1;
                }
                else
                {
                    txtBox.SelectionStart = 0;
                    txtBox.SelectionLength = posicionCursor;
                }
                txtBox.Focus();
            }
            txtError.Text = "1:102 Error en Linea " + linea + " Elemento Invalido";
        }
        public void SimboloDesconocido (RichTextBox txtBox, TextBox txtError, int linea)
        {
            int posicionCursor = txtBox.SelectionStart;
            if (posicionCursor > 0)
            {
                txtBox.SelectionStart = posicionCursor - 2;
                txtBox.SelectionLength = 1;
                txtBox.Focus();
            }
            txtError.Text = "1:101 Error en Linea " + linea + " Simbolo Desconocido";
        }
    }
}
