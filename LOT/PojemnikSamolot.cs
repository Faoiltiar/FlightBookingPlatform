using System.Collections.Generic;
using System.Windows.Controls;

namespace LOT
{
    static class PojemnikSamolot
    {
        //definicja elementów samolotu (nazwy oznaczają opis danego elementu)
        public static int il_rzedow=6;
        public static int il_siedzen_w_rzedzie=30;
        public static int szerokosc_buforowa_rzedow=100;
        public static int szerokosc_buforowa_siedzen=100;
        public static Button[,] Button = new Button[20, 50]; //[max_il_rzedow,max_il_miejsc_w_rzędzie]
        public static int ilosc_miejsc; //przechowuje ilosc miejsc do rezerwacji
        public static int max_il_miejsc=5;
        public static List<Informacja_o_miejscach> informacja;
        public static int uprawnienia_uzytkownika=2; //kiedy 1 to admin //kiedy 2 to zwykly
        public static string[] classes = { "Zwykła", "Premium" };
        public static int klasa_samolot;
        public static TextBox[,] textBox = new TextBox[20, 2];
        public static string nazwaPliku = "";
        public static string skad; //przechowuje miejsce wylotu
        public static string dokad; //przechowuje miejsce docelowe
        public static int newIndex; //do zakładek
    }
}
