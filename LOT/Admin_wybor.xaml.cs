using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LOT
{
    /// <summary>
    /// Interaction logic for Admin_wybor.xaml
    /// </summary>
    public partial class Admin_wybor : Window
    {
        public Admin_wybor()
        {
            InitializeComponent();
        }

        private void Button_Click_CreatePlane(object sender, RoutedEventArgs e)
        {
            DefinicjaSamolotu def_sam = new DefinicjaSamolotu();
            def_sam.Show();
            this.Close();
        }

        private void Button_Click_ChooseFly(object sender, RoutedEventArgs e)
        {
            DopasowanieLotySamoloty dop_sam = new DopasowanieLotySamoloty();
            dop_sam.Show();
            this.Close();
        }

        private void Logout(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            PojemnikSamolot.uprawnienia_uzytkownika = 0;
            this.Close();
        }
    }
}
