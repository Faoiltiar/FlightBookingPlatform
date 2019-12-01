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
    /// Interaction logic for Pomoc.xaml
    /// </summary>
    public partial class Pomoc : Window
    {
        public Pomoc()
        {
            InitializeComponent();

            switch (PojemnikSamolot.newIndex)
            {
                case 0:
                    label1.Content = "1.Wybór Daty oraz Miejsca Wylotu/Przylotu";
                    textBlock1.Text = "W tym oknie należy wybrać miejsce wylotu/przylotu a także datę,w zależności od wybrania lotu w jedną/dwie strony należy wybrać odpowiednie daty";
                    break;
                case 1:
                    label1.Content = "2.Wybór rodzaju biletów";
                    textBlock1.Text = "W tym oknie należy wybrać odpowiednią ilość różnego rodzaju biletów, odpowiednio tych dla dorosłych, dzieci oraz niemowląt, maksymalna ilość niemowląt nie może przekroczyć ilości dorosłych";
                    break;
                case 2:
                    label1.Content = "3.Dane Szczegółowe";
                    textBlock1.Text = "W tym oknie należy wpisać swoje dane, odpowiednio imie i nazwisko dla kolejnych biletów";
                    break;
                case 3:
                    label1.Content = "4.Rezerwacja miejsca";
                    textBlock1.Text = "W tym oknie w zależności od rodzaju biletów należy wybrać miejsca w samolocie, miejsce free oznacza wolne, miejsce busy to zarezerwowane przez Nas, miejsce rez oznacza zarezerwowane przez kogoś innego";
                    break;
                case 4:
                    label1.Content = "5.Podsumowanie/Płatność";
                    textBlock1.Text = "W tym oknie znajduję się podsumowanie całego procesu zakupu biletu, po opłaceniu biletu należy wcisnąć opcje ZAPISZ, bilet zostanie wpisany w baze danych, następnie można go wydrukować";
                    break;
            }
        }
    }
}
