using System;
using System.Windows.Controls;

namespace LOT
{
    class AddingAndSubstracting
    {
        // dodawanie w elemencie textBox z zabezpieczeniem przed przekroczeniem maksymalnej dozwolonej liczby
        public void NumericUp(TextBox textBox, int max)
        {
            int tmp;
            tmp = Int16.Parse(textBox.Text);
            if (tmp < max)
                tmp++;
            textBox.Text = tmp.ToString();
        }
        // odejmowanie w elemencie textBox z zabezpieczeniem przed przekroczeniem minimalnej dozwolonej liczby
        public void NumericDown(TextBox textBox)
        {
            int tmp;
            tmp = Int16.Parse(textBox.Text);
            if (tmp > 0)
                tmp--;
            textBox.Text = tmp.ToString();
        }
    }
}
