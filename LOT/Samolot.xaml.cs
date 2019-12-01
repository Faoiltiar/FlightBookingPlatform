using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace LOT
{
    public class Informacja_o_miejscach : IEquatable<Informacja_o_miejscach> //klasa przechowująca informacje o zajętych miejscach
    {
        public Informacja_o_miejscach(int r,int m)
        {
            this.rzad = r;
            this.miejsce = m;
        }

        public int rzad { get; set; }
        public int miejsce { get; set; }

        public bool Equals(Informacja_o_miejscach other) //przeciążanie porównania obiektów
        {
            if(this.rzad == other.rzad && this.miejsce == other.miejsce)
            {
                return true;
            }else
            {
                return false;
            }
        }

        public override string ToString() //przeciążanie metody to string 
        {
            return "Rząd : " + rzad + "   Miejsce: " + miejsce;
        }
    }
    /// <summary>
    /// Interaction logic for Samolot.xaml
    /// </summary>
    public partial class Samolot : Window
    {
        MySqlConnection con = new MySqlConnection(); //odpowiedzialna za polaczenie
        public static int ilosc_pobran = 0;

        public Samolot()
        {
            InitializeComponent();
            con.ConnectionString = "server=localhost;user id=root;password=toor;persistsecurityinfo=True;database=plane"; //tu wklejamy connection stringa 
            con.Open();

            this.Topmost = true; //okno zawsze na wierzchu

            if(PojemnikSamolot.uprawnienia_uzytkownika == 1)
            {
                Powrot.Visibility = Visibility.Visible;
            }else
            {
                Powrot.Visibility = Visibility.Hidden;
            }

            //inicjalizacja listy miejsc
            PojemnikSamolot.informacja = new List<Informacja_o_miejscach>();

            grid_definition(1366, HorizontalAlignment.Left, VerticalAlignment.Top, true, Colors.White); //definicja siatki

            for (int i = 0; i < PojemnikSamolot.il_siedzen_w_rzedzie + 2; i++) //tworzenie kolumn
            {
                if (i == 0)
                {
                    ColumnDefinition gridCol = new_column_ustalona(PojemnikSamolot.szerokosc_buforowa_siedzen);
                }
                if (i != 0 && i != (PojemnikSamolot.il_siedzen_w_rzedzie + 1))
                {
                    ColumnDefinition gridCol = new_column();
                }
                if (i == (PojemnikSamolot.il_siedzen_w_rzedzie + 1))
                {
                    ColumnDefinition gridCol = new_column_ustalona(PojemnikSamolot.szerokosc_buforowa_siedzen);
                }
            }

            //parzysta ilosc rzędów
            if (PojemnikSamolot.il_rzedow % 2 == 0) //tworzenie rzędów
            {
                for (int i = 0; i < PojemnikSamolot.il_rzedow + 3; i++)
                {
                    if (i == 0)
                    {
                        //strefa buforowa
                        RowDefinition gridRow = new_row(PojemnikSamolot.szerokosc_buforowa_rzedow);
                    }

                    if (i >= 1 && i <= PojemnikSamolot.il_rzedow / 2)
                    {
                        //normalne rzędy
                        RowDefinition gridRow = new_row((768 - (2 * PojemnikSamolot.szerokosc_buforowa_rzedow)) / (PojemnikSamolot.il_rzedow + 3));
                    }

                    if (i == ((PojemnikSamolot.il_rzedow / 2) + 1))
                    {
                        //strefa buforowa środkowa
                        RowDefinition gridRow = new_row(PojemnikSamolot.szerokosc_buforowa_rzedow / 5);
                    }

                    if (i >= ((PojemnikSamolot.il_rzedow / 2) + 2) && i <= (PojemnikSamolot.il_rzedow + 1))
                    {
                        RowDefinition gridRow = new_row((768 - (2 * PojemnikSamolot.szerokosc_buforowa_rzedow)) / (PojemnikSamolot.il_rzedow + 3));
                    }

                    if (i == PojemnikSamolot.il_rzedow + 2)
                    {
                        //strefa buforowa 
                        RowDefinition gridRow = new_row(PojemnikSamolot.szerokosc_buforowa_rzedow);
                    }
                }
            }
            else //nieparzysta ilość rzędów
            {
                for (int i = 0; i < PojemnikSamolot.il_rzedow + 4; i++) //tworzenie rzędów
                {
                    if (i == 0)
                    {
                        //bufor
                        RowDefinition gridRow = new_row(PojemnikSamolot.szerokosc_buforowa_rzedow);
                    }
                    if (i >= 1 && i <= ((PojemnikSamolot.il_rzedow - 1) / 2))
                    {
                        //miejsca
                        RowDefinition gridRow = new_row((768 - (3 * PojemnikSamolot.szerokosc_buforowa_rzedow)) / (PojemnikSamolot.il_rzedow + 4));
                    }
                    if (i == ((PojemnikSamolot.il_rzedow - 1) / 2 + 1))
                    {
                        //bufor
                        RowDefinition gridRow = new_row(PojemnikSamolot.szerokosc_buforowa_rzedow / 2);
                    }
                    if (i == ((PojemnikSamolot.il_rzedow - 1) / 2 + 2))
                    {
                        //miejsce
                        RowDefinition gridRow = new_row((768 - (3 * PojemnikSamolot.szerokosc_buforowa_rzedow)) / (PojemnikSamolot.il_rzedow + 4));
                    }
                    if (i == ((PojemnikSamolot.il_rzedow - 1) / 2 + 3))
                    {
                        //bufor
                        RowDefinition gridRow = new_row(PojemnikSamolot.szerokosc_buforowa_rzedow / 2);
                    }
                    if (i >= ((PojemnikSamolot.il_rzedow - 1) / 2 + 4) && i <= (PojemnikSamolot.il_rzedow + 2))
                    {
                        //miejsca
                        RowDefinition gridRow = new_row((768 - (3 * PojemnikSamolot.szerokosc_buforowa_rzedow)) / (PojemnikSamolot.il_rzedow + 4));
                    }
                    if (i == PojemnikSamolot.il_rzedow + 3)
                    {
                        //bufor
                        RowDefinition gridRow = new_row(PojemnikSamolot.szerokosc_buforowa_rzedow);
                    }
                }
            }

            if (PojemnikSamolot.il_rzedow % 2 == 0) //dopasowanie przyciskow do parzystego rozkładu rzędów
            {
                for (int r = 0; r < PojemnikSamolot.il_rzedow + 3; r++)
                {
                    if (r == 0 || r == ((PojemnikSamolot.il_rzedow / 2) + 1) || r == PojemnikSamolot.il_rzedow + 2)
                    {
                        //nie tworzyć przycisków
                    }
                    else
                    {
                        for (int k = 1; k < PojemnikSamolot.il_siedzen_w_rzedzie + 1; k++)
                        {
                            if (k >= 1 && k <= 2)
                            {
                                //premium
                                PojemnikSamolot.Button[r, k] = button_create("Free", 15, 5, 5, 5, r, k);
                                button_klik_premium(r, k ,zajetosc);
                            }
                            if (k >= 3 && k <= ((PojemnikSamolot.il_siedzen_w_rzedzie / 2) - 1))
                            {
                                PojemnikSamolot.Button[r, k] = button_create("Free", 5, 5, 5, 5, r, k);
                                button_klik(r, k, zajetosc);
                            }
                            if (k >= ((PojemnikSamolot.il_siedzen_w_rzedzie / 2)) && k <= ((PojemnikSamolot.il_siedzen_w_rzedzie / 2) + 2))
                            {
                                //premium
                                PojemnikSamolot.Button[r, k] = button_create("Free", 15, 5, 5, 5, r, k);
                                button_klik_premium(r, k, zajetosc);
                            }
                            if (k >= ((PojemnikSamolot.il_siedzen_w_rzedzie / 2) + 3) && k <= (PojemnikSamolot.il_siedzen_w_rzedzie - 2))
                            {
                                PojemnikSamolot.Button[r, k] = button_create("Free", 5, 5, 5, 5, r, k);
                                button_klik(r, k, zajetosc);
                            }
                            if (k >= (PojemnikSamolot.il_siedzen_w_rzedzie - 1))
                            {
                                //premium
                                PojemnikSamolot.Button[r, k] = button_create("Free", 15, 5, 5, 5, r, k);
                                button_klik_premium(r, k, zajetosc);
                            }
                        }
                    }
                }
            }
            else //dopasowanie przycisków do nieparzystego rozkładu rzędów 
            {
                for (int r = 0; r < PojemnikSamolot.il_rzedow + 4; r++)
                {
                    if (r == 0 || r == ((PojemnikSamolot.il_rzedow - 1) / 2 + 1) || r == ((PojemnikSamolot.il_rzedow - 1) / 2 + 3) || r == PojemnikSamolot.il_rzedow + 3)
                    {
                        //nie tworzyć przycisków
                    }
                    else
                    {
                        for (int k = 1; k < PojemnikSamolot.il_siedzen_w_rzedzie + 1; k++)
                        {
                            if (k >= 1 && k <= 2)
                            {
                                //premium
                                PojemnikSamolot.Button[r, k] = button_create("Free", 15, 5, 5, 5, r, k);
                                button_klik_premium(r, k, zajetosc);
                            }
                            if (k >= 3 && k <= ((PojemnikSamolot.il_siedzen_w_rzedzie / 2) - 1))
                            {
                                PojemnikSamolot.Button[r, k] = button_create("Free", 5, 5, 5, 5, r, k);
                                button_klik(r, k, zajetosc);
                            }
                            if (k >= ((PojemnikSamolot.il_siedzen_w_rzedzie / 2)) && k <= ((PojemnikSamolot.il_siedzen_w_rzedzie / 2) + 2))
                            {
                                //premium
                                PojemnikSamolot.Button[r, k] = button_create("Free", 15, 5, 5, 5, r, k);
                                button_klik_premium(r, k, zajetosc);
                            }
                            if (k >= ((PojemnikSamolot.il_siedzen_w_rzedzie / 2) + 3) && k <= (PojemnikSamolot.il_siedzen_w_rzedzie - 2))
                            {
                                PojemnikSamolot.Button[r, k] = button_create("Free", 5, 5, 5, 5, r, k);
                                button_klik(r, k, zajetosc);
                            }
                            if (k >= (PojemnikSamolot.il_siedzen_w_rzedzie - 1))
                            {
                                //premium
                                PojemnikSamolot.Button[r, k] = button_create("Free", 15, 5, 5, 5, r, k);
                                button_klik_premium(r, k, zajetosc);
                            }
                        }
                    }
                }
            }

            if (PojemnikSamolot.il_rzedow % 2 == 1) //nieparzysty rozkład rzędów
            {
                for (int i = 1; i < PojemnikSamolot.il_siedzen_w_rzedzie + 1; i++) //tworzenie opisów do poszczególnych kolumn samolotu aby wiadomo było jaka kolumna ma jaki numer dla zamawiającego bilety
                {
                    TextBlock opis = opis_miejscowek(i, PojemnikSamolot.il_rzedow + 4, i, VerticalAlignment.Top, HorizontalAlignment.Center);
                }

                for (int i = 1; i < PojemnikSamolot.il_rzedow + 3; i++) //tworzenie opisów do poszczególnych rzędów samolotu aby wiadomo było jaki rząd posiada jaki numer dla zamawiającego
                {
                    TextBlock opis2 = opis_miejscowek(i, i, PojemnikSamolot.il_siedzen_w_rzedzie + 3, VerticalAlignment.Center, HorizontalAlignment.Left);
                }
            }
            else
            {
                for (int i = 1; i < PojemnikSamolot.il_siedzen_w_rzedzie + 1; i++) //tworzenie opisów do poszczególnych kolumn samolotu aby wiadomo było jaka kolumna ma jaki numer dla zamawiającego bilety
                {
                    TextBlock opis = opis_miejscowek(i, PojemnikSamolot.il_rzedow + 4, i, VerticalAlignment.Top, HorizontalAlignment.Center);
                }

                for (int i = 1; i < PojemnikSamolot.il_rzedow + 2; i++) //tworzenie opisów do poszczególnych rzędów samolotu aby wiadomo było jaki rząd posiada jaki numer dla zamawiającego
                {
                    TextBlock opis2 = opis_miejscowek(i, i, PojemnikSamolot.il_siedzen_w_rzedzie + 3, VerticalAlignment.Center, HorizontalAlignment.Left);
                }
            }
            if (PojemnikSamolot.uprawnienia_uzytkownika != 1)
            {
                //obsługa przycisku dalej 
                Button dalej = button_create("Dalej", 0, 0, 0, 0, PojemnikSamolot.il_rzedow + 4, PojemnikSamolot.il_siedzen_w_rzedzie + 3);
                dalej.Click += (s, e) =>
                {
                    if (PojemnikSamolot.ilosc_miejsc < PojemnikSamolot.max_il_miejsc && PojemnikSamolot.ilosc_miejsc >= 0)
                    {
                        MessageBox.Show("Nie wybrano wymaganej liczby miejsc, proszę to zrobić po czym nacisnąć ponownie dalej");
                    }
                    else
                    {
                        Window1 window = (Window1)this.DataContext; ;
                        DynamicGrid NewGrid = new DynamicGrid();
                        window.AnotherGrid.Children.Clear();
                        window.AnotherGrid.RowDefinitions.Clear();
                        window.AnotherGrid.ColumnDefinitions.Clear();
                        NewGrid.MyGrid(window.AnotherGrid, HorizontalAlignment.Stretch, VerticalAlignment.Stretch, false, Colors.WhiteSmoke);
                        window.AnotherGrid.Background = new SolidColorBrush(Colors.Black);
                        window.AnotherGrid.Background.Opacity = 0;
                        for (int i = 0; i < 2 * (Int16.Parse(window.tbAdults.Text) + Int16.Parse(window.tbChildren.Text)) + 1; i++)
                        {
                            if (i % 2 != 0)
                                NewGrid.NewRow(window.AnotherGrid, 26, GridUnitType.Pixel);
                            else
                                NewGrid.NewRow(window.AnotherGrid, 50, GridUnitType.Pixel);
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            if (i % 2 != 0)
                                NewGrid.NewColumn(window.AnotherGrid, 120, GridUnitType.Pixel);
                            else
                                NewGrid.NewColumn(window.AnotherGrid, 70, GridUnitType.Pixel);
                        }
                        int whichPerson = 1;
                        for (Int16 i = 1; i < 2 * (Int16.Parse(window.tbAdults.Text) + Int16.Parse(window.tbChildren.Text)) + 1; i += 2)
                        {
                            for (Int16 j = 1; j <= 3; j += 2)
                            {
                                if (j == 1)
                                {
                                    TextBox txtBox = new TextBox();
                                    NewGrid.NewLabel(window.AnotherGrid, Colors.White, 12, FontWeights.SemiBold, FontStyles.Normal, "Segoe UI", VerticalAlignment.Bottom, HorizontalAlignment.Center, "Pasażer" + whichPerson.ToString(), i, (Int16)(j - 1));
                                    NewGrid.NewLabel(window.AnotherGrid, Colors.White, 12, FontWeights.SemiBold, FontStyles.Normal, "Segoe UI", VerticalAlignment.Bottom, HorizontalAlignment.Left, "Rząd:", (Int16)(i - 1), j);
                                    txtBox = NewGrid.NewTextBox(window.AnotherGrid, PojemnikSamolot.informacja[whichPerson - 1].rzad.ToString(), 12, FontWeights.SemiBold, FontStyles.Normal, VerticalAlignment.Center, Colors.Black, i, j, false);
                                }
                                else
                                {
                                    TextBox txtBox = new TextBox();
                                    NewGrid.NewLabel(window.AnotherGrid, Colors.White, 12, FontWeights.SemiBold, FontStyles.Normal, "Segoe UI", VerticalAlignment.Bottom, HorizontalAlignment.Left, "Miejsce:", (Int16)(i - 1), j);
                                    txtBox = NewGrid.NewTextBox(window.AnotherGrid, PojemnikSamolot.informacja[whichPerson - 1].miejsce.ToString(), 12, FontWeights.SemiBold, FontStyles.Normal, VerticalAlignment.Center, Colors.Black, i, j, false);
                                }
                            }
                            whichPerson += 1;
                        }
                        this.Close();
                    }
                };
            }

            //blokowanie przycisków w zaleznosci od trasy (tzn. zajetosc miejsc na dana trase)
            try
            {
                if (ilosc_pobran > 0)
                {
                    con.Close();
                    con.Open();
                }
                using (MySqlCommand command = new MySqlCommand("SELECT rzad,miejsce FROM tickets WHERE skad='"+ PojemnikSamolot.skad +"' AND dokad='"+ PojemnikSamolot.dokad+"'", con))
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        button_klik_premium(reader.GetInt32(0), reader.GetInt32(1), 1);
                        ilosc_pobran += 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd odczytu z bazy" + ex);
            }
        }

        public static int zajetosc = 0;

        public void grid_definition(int width, HorizontalAlignment horizontal, VerticalAlignment vertical, bool linie, Color kolor)
        {
            DynamicGrid.Width = width; //szerokość pola 
            DynamicGrid.HorizontalAlignment = horizontal; //położenie w pionie
            DynamicGrid.VerticalAlignment = VerticalAlignment; //położenie w poziomie
            DynamicGrid.ShowGridLines = false; //czy mają być pokazywane linie siatki ?
            DynamicGrid.Background = new SolidColorBrush(kolor); //kolor siatki
        }

        public ColumnDefinition new_column()
        {
            ColumnDefinition gridCol = new ColumnDefinition(); //tworzenie nowej kolumny 
            DynamicGrid.ColumnDefinitions.Add(gridCol); //dodawanie kolumny do siatki
            return gridCol; //zwrot siatki
        }

        public ColumnDefinition new_column_ustalona(int width)
        {
            ColumnDefinition gridCol = new ColumnDefinition(); //tworzenie nowej kolumny 
            gridCol.Width = new GridLength(width);
            DynamicGrid.ColumnDefinitions.Add(gridCol); //dodawanie kolumny do siatki
            return gridCol; //zwrot siatki
        }

        public RowDefinition new_row(int GridLength) //definicja nowego wiersza
        {
            RowDefinition gridRow = new RowDefinition();
            gridRow.Height = new GridLength(GridLength);
            DynamicGrid.RowDefinitions.Add(gridRow);
            return gridRow;
        }

        public Button button_create(string napis, int left, int top, int right, int bottom, int which_row, int which_column) //dynamiczne tworzenie przycisku
        {
            Button button = new Button();
            button.FontSize = 9;
            button.Content = napis;
            button.Margin = new Thickness(left, top, right, bottom);
            Grid.SetRow(button, which_row);
            Grid.SetColumn(button, which_column);
            DynamicGrid.Children.Add(button);
            return button;
        }

        public TextBlock textblock_create(string napis, int fontsize, FontWeight pogrubienie, Color kolor, VerticalAlignment pion, int which_row, int which_column) //dynamiczne tworzenie texboxa
        {
            TextBlock textblock = new TextBlock();
            textblock.Text = napis;
            textblock.FontSize = fontsize;
            textblock.FontWeight = pogrubienie;
            textblock.Foreground = new SolidColorBrush(kolor);
            textblock.VerticalAlignment = pion;
            Grid.SetRow(textblock, which_row);
            Grid.SetColumn(textblock, which_column);
            DynamicGrid.Children.Add(textblock);
            return textblock;
        }

        public TextBlock opis_miejscowek(int numer, int which_row, int which_column, VerticalAlignment pion, HorizontalAlignment poziom) //dynamiczne tworzenie opisu rzędów i kolumn
        {
            TextBlock bloczek = new TextBlock();
            bloczek.Text = numer.ToString();
            bloczek.FontSize = 10;
            bloczek.FontWeight = FontWeights.Bold;
            bloczek.Foreground = new SolidColorBrush(Colors.Black);
            bloczek.VerticalAlignment = pion;
            bloczek.HorizontalAlignment = poziom;
            Grid.SetRow(bloczek, which_row);
            Grid.SetColumn(bloczek, which_column);
            DynamicGrid.Children.Add(bloczek);
            return bloczek;
        }

        public void button_klik(int rzedy, int miejsce_w_rzedzie, int zajete)
        {
            PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Click += (s, e) =>
            {
                //sprawdzanie klasy samolotu
                if (PojemnikSamolot.klasa_samolot == 2 && zajete == 0)
                {
                    //sprawdzanie czy dane miejsce jest wolne
                    if (PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Content.ToString() == "Free")
                    {
                        //sprawdzanie czy miejsc jest więcej niż 0 oraz mniej niż max 
                        if (PojemnikSamolot.ilosc_miejsc < PojemnikSamolot.max_il_miejsc && PojemnikSamolot.ilosc_miejsc >= 0)
                        {
                            //PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].IsEnabled = false;

                            PojemnikSamolot.informacja.Add(new Informacja_o_miejscach(rzedy, miejsce_w_rzedzie));

                            //sprawdzanie zawartości listy, w output w oknie debugowania 
                            System.Diagnostics.Debug.WriteLine("Nowe");
                            foreach (Informacja_o_miejscach ainformacja in PojemnikSamolot.informacja)
                            {
                                System.Diagnostics.Debug.WriteLine(ainformacja);
                            }

                            //przy zajmowaniu zmiana koloru przycisku, napisu na nim oraz inkrementacja zmiennej świadczącej o ilości miejsc
                            PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Content = "Busy";
                            PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Background = Brushes.OrangeRed;
                            PojemnikSamolot.ilosc_miejsc += 1;
                        }
                        else
                        {
                            MessageBox.Show("Max liczba biletów");
                        }

                    }
                    else
                    {
                        if (PojemnikSamolot.ilosc_miejsc >= 0)
                        {
                            //PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].IsEnabled = true;
                            if (PojemnikSamolot.informacja.Contains(new Informacja_o_miejscach(rzedy, miejsce_w_rzedzie)))
                            {
                                PojemnikSamolot.informacja.Remove(new Informacja_o_miejscach(rzedy, miejsce_w_rzedzie));
                            }
                            PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Content = "Free";
                            PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Background = new SolidColorBrush(Color.FromArgb(255, (byte)221, (byte)221, (byte)221));
                            PojemnikSamolot.ilosc_miejsc -= 1;
                        }
                        else
                        {
                            MessageBox.Show("Min liczba biletów");
                        }
                    }
                }
            };
            //jesli w bazie bilet zajety wtedy nie mozna go klikac
            if (zajete == 1)
            {
                IsEnabled = false;
                PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Content = "REZ";
            }
        }

        public void button_klik_premium(int rzedy, int miejsce_w_rzedzie, int zajete)
        {
            PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Click += (s, e) =>
            {
                //sprawdzanie klasy samolotu
                if (PojemnikSamolot.klasa_samolot == 1 && zajete == 0)
                {
                    //sprawdzanie czy dane miejsce jest wolne
                    if (PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Content.ToString() == "Free")
                    {
                        //sprawdzanie czy miejsc jest więcej niż 0 oraz mniej niż max 
                        if (PojemnikSamolot.ilosc_miejsc < PojemnikSamolot.max_il_miejsc && PojemnikSamolot.ilosc_miejsc >= 0)
                        {
                            //PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].IsEnabled = false;

                            PojemnikSamolot.informacja.Add(new Informacja_o_miejscach(rzedy, miejsce_w_rzedzie));

                            //sprawdzanie zawartości listy, w output w oknie debugowania 
                            System.Diagnostics.Debug.WriteLine("Nowe");
                            foreach (Informacja_o_miejscach ainformacja in PojemnikSamolot.informacja)
                            {
                                System.Diagnostics.Debug.WriteLine(ainformacja);
                            }

                            //przy zajmowaniu zmiana koloru przycisku, napisu na nim oraz inkrementacja zmiennej świadczącej o ilości miejsc
                            PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Content = "Busy";
                            PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Background = Brushes.OrangeRed;
                            PojemnikSamolot.ilosc_miejsc += 1;
                        }
                        else
                        {
                            MessageBox.Show("Max liczba biletów");
                        }

                    }
                    else
                    {
                        if (PojemnikSamolot.ilosc_miejsc >= 0)
                        {
                            //PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].IsEnabled = true;
                            if (PojemnikSamolot.informacja.Contains(new Informacja_o_miejscach(rzedy, miejsce_w_rzedzie)))
                            {
                                PojemnikSamolot.informacja.Remove(new Informacja_o_miejscach(rzedy, miejsce_w_rzedzie));
                            }
                            PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Content = "Free";
                            PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Background = new SolidColorBrush(Color.FromArgb(255, (byte)221, (byte)221, (byte)221));
                            PojemnikSamolot.ilosc_miejsc -= 1;
                        }
                        else
                        {
                            MessageBox.Show("Min liczba biletów");
                        }
                    }
                }
                //jesli w bazie bilet zajety wtedy nie mozna go klikac
            };
            if (zajete == 1)
            {
                PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].IsEnabled = false;
                PojemnikSamolot.Button[rzedy, miejsce_w_rzedzie].Content = "REZ";
            }
        }




        private void Powrot_Click(object sender, RoutedEventArgs e) //obsługa przycisku powrot
        {
            if(PojemnikSamolot.uprawnienia_uzytkownika == 1)
            {
                DefinicjaSamolotu powrot = new DefinicjaSamolotu();
                this.Close();
                powrot.Show();

                PojemnikSamolot.ilosc_miejsc = 0;
            }else
            {
                this.Close();

                PojemnikSamolot.ilosc_miejsc = 0;
            }


        }
    }
}
