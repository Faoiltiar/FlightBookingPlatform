using System;
using System.Windows;
using System.IO;

namespace LOT
{
    /// <summary>
    /// Interaction logic for Ticket.xaml
    /// </summary>
    public partial class Ticket : Window
    {
        public Ticket()
        {
            InitializeComponent();
        }

        private void imgButton_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = Owner as Window1;
            // zabezpieczenie przed pozostawieniem pustej nazwy pliku
            if (imageName.Text == "" || imageName.Text == " ")
                MessageBox.Show("Podaj nazwę zapisywanego pliku");
            // zapisanie biletu do folderu "Aplikacja PRL" w Dokumentach
            else
            {
                PojemnikSamolot.nazwaPliku = imageName.Text + ".bmp";
                ToImage.SaveImage(window1.scrollGrid, 300, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), @"Aplikacja PRL\", PojemnikSamolot.nazwaPliku), 1281 + (Int16.Parse(window1.tbAdults.Text) + Int16.Parse(window1.tbChildren.Text)) * 400, 2584);
                this.Close();
            }
        }
    }
}
