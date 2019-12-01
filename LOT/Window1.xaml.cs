using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.IO;
using MySql.Data.MySqlClient;

namespace LOT
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 

    public partial class Window1 : Window
    {
        MySqlConnection con = new MySqlConnection(); //odpowiedzialna za polaczenie
        public static int ilosc_pobran=0;
        public static int ilosc_zapisow = 0;
        public static int max_id;
        public static int ChooseTwoWay = 0;

        public Window1()
        {          
            InitializeComponent();
            ComBoxFlyIni();
            ComBoxClassesIni();
            tcTicket.SelectedIndex = 0;

            con.ConnectionString = "server=localhost;user id=root;password=toor;persistsecurityinfo=True;database=plane"; //tu wklejamy connection stringa 
            con.Open();
        }


        //***********-----------Listy rozwijalne z wyborem miast wylotu i przylotu-----------***********\\
        private void cBoxFlyFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] cities = { "Warszawa", "Kraków", "Gdańsk", "Poznań"};
            cBoxFlyTo_Ini(cities);
        }
        public void ComBoxFlyIni()
        {
            string[] cities = { "Warszawa", "Kraków", "Gdańsk", "Poznań"};
            cBoxFlyFrom_Ini(cities);
            cBoxFlyTo_Ini(cities);
        }
        // lista miejsca wylotu jest wypełniana wszystkimi miastami
        public void cBoxFlyFrom_Ini(string[] cities)
        {
            cBoxFlyFrom.ItemsSource = cities;
        }
        // lista miejsca przylotu wypełniana jest wszystkimi miastami poza tym umieszczonym w liście miejsc wylotu
        public void cBoxFlyTo_Ini(string[] cities)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < cities.Length ; i++)
            {
                if (cBoxFlyFrom.SelectedItem.ToString() == cities[i])
                    continue;
                list.Add(cities[i]);
            }
            cboxFlyTo.ItemsSource = list;
        }
        //***********-----------/////////\\\\\\\\\-----------***********\\

        //***********-----------Data wylotu-----------***********\\
        private void dateOfDeparture_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            // data wylotu zaczyna się od dnia dzisiejszego
            dateOfDeparture.DisplayDateStart = DateTime.Today;
            var picker = sender as DatePicker;
            DateTime? date = picker.SelectedDate;
            try
            {
                // jeśli data przylotu została wybrana, to maksymalna data wylotu jest tego dnia co data przylotu
                dateOfDeparture.DisplayDateEnd = dateOfArrival.SelectedDate.Value.Date;
            }
            catch
            {
                // jeśli data przylotu nie została wybrana, to maksymalna data wylotu jest na 60 dni wprzód
                dateOfDeparture.DisplayDateEnd = DateTime.Today.AddDays(60);
            }  
        }
        //***********-----------/////////\\\\\\\\\-----------***********\\

        //***********-----------Data przylotu-----------***********\\
        private void dateOfArrival_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            var picker = sender as DatePicker;
            try
            {
                // jeśli data wylotu została wybrana, to początkowa data przylotu to dzień wylotu
                dateOfArrival.DisplayDateStart = dateOfDeparture.SelectedDate.Value.Date;
            }
            catch
            {
                // jeśli data wylotu nie została wybrana, to początkowa data przylotu to dzień dzisiejszy
                dateOfArrival.DisplayDateStart = DateTime.Today;

            }
            dateOfArrival.DisplayDateEnd = DateTime.Today.AddDays(60);
            DateTime? date = picker.SelectedDate;
        }
        //***********-----------/////////\\\\\\\\\-----------***********\\

        // wykorzystanie wyrażeń regularnych do zablokowania wpisywania do textBoxów innych wartości, niż wartości licznowe   
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //***********-----------Przechodzenie pomiędzy tabami-----------***********\\
        // przyciski "Dalej"
        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            PojemnikSamolot.newIndex = tcTicket.SelectedIndex + 1;
             // obsługa poszczególnych tabów
            switch (PojemnikSamolot.newIndex)
            {
                case 1:
                    tcTicket.SelectedIndex = PojemnikSamolot.newIndex;
                    break;
                case 2:
                    tcTicket.SelectedIndex = PojemnikSamolot.newIndex;
                    // tworzenie nowego Grid'a w zależności od wybranej ilości osób dorosłych i dzieci
                    DynamicGrid GridNew = new DynamicGrid();
                    //tabSpecificInformation.IsEnabled = true;
                    NewGrid.Children.Clear();
                    NewGrid.RowDefinitions.Clear();
                    NewGrid.ColumnDefinitions.Clear();
                    GridNew.MyGrid(NewGrid, HorizontalAlignment.Stretch, VerticalAlignment.Stretch, false, Colors.WhiteSmoke);
                    // dynamiczne tworzenie rzędów siatki 
                    NewGrid.Background = new SolidColorBrush(Colors.Black);
                    NewGrid.Background.Opacity = 0;
                    for (int i = 0; i < 2 * (Int16.Parse(tbAdults.Text) + Int16.Parse(tbChildren.Text)) + 1; i++)
                    {
                        if (i % 2 != 0)
                            GridNew.NewRow(NewGrid, 26, GridUnitType.Pixel);
                        else
                            GridNew.NewRow(NewGrid, 50, GridUnitType.Pixel);
                    }
                    // dynamiczne tworzenie kolumn siatki
                    for (int i = 0; i < 5; i++)
                    {
                        if (i % 2 != 0)
                            GridNew.NewColumn(NewGrid, 120, GridUnitType.Pixel);
                        else
                            GridNew.NewColumn(NewGrid, 70, GridUnitType.Pixel);
                    }
                    int whichPerson = 1;
                    int tmp;
                    // uzupełnianie pól siatki (odpowiednich rzędów i kolumn) wybranymi obiektami 
                    for (Int16 i = 1; i < 2 * (Int16.Parse(tbAdults.Text) + Int16.Parse(tbChildren.Text)) + 1; i += 2)
                    {
                        tmp = 0;
                        for (Int16 j = 1; j <= 3; j += 2)
                        {
                            if (j == 1)
                            {
                                GridNew.NewLabel(NewGrid, Colors.White, 12, FontWeights.SemiBold, FontStyles.Normal, "Segoe UI", VerticalAlignment.Bottom, HorizontalAlignment.Center, "Pasażer" + whichPerson.ToString(), i, (Int16)(j - 1));
                                GridNew.NewLabel(NewGrid, Colors.White, 12, FontWeights.SemiBold, FontStyles.Normal, "Segoe UI", VerticalAlignment.Bottom, HorizontalAlignment.Left, "Imię:", (Int16)(i - 1), j);
                                PojemnikSamolot.textBox[whichPerson - 1, tmp] = GridNew.NewTextBox(NewGrid, "", 12, FontWeights.SemiBold, FontStyles.Normal, VerticalAlignment.Center, Colors.Black, i, j, true);
                                // zabezpieczenie przed wpisywaniem znaków spoza alfabetu w przypadku podawania imienia pasażera
                                PojemnikSamolot.textBox[whichPerson - 1, tmp].PreviewTextInput += (s,TextCompositionEventArgs) =>
                                {
                                    Regex regex = new Regex("[^a-zA-Z]+");
                                    TextCompositionEventArgs.Handled = regex.IsMatch(TextCompositionEventArgs.Text);
                                };
                                tmp = 1;
                            }
                            else
                            {
                                GridNew.NewLabel(NewGrid, Colors.White, 12, FontWeights.SemiBold, FontStyles.Normal, "Segoe UI", VerticalAlignment.Bottom, HorizontalAlignment.Left, "Nazwisko:", (Int16)(i - 1), j);
                                PojemnikSamolot.textBox[whichPerson - 1, tmp] = GridNew.NewTextBox(NewGrid, "", 12, FontWeights.SemiBold, FontStyles.Normal, VerticalAlignment.Center, Colors.Black, i, j, true);
                                // zabezpieczenie przed wpisywaniem znaków spoza alfabetu w przypadku podawania nazwiska pasażera
                                PojemnikSamolot.textBox[whichPerson - 1, tmp].PreviewTextInput += (s, TextCompositionEventArgs) =>
                                {
                                    Regex regex = new Regex("[^a-zA-Z]+");
                                    TextCompositionEventArgs.Handled = regex.IsMatch(TextCompositionEventArgs.Text);
                                };
                            }
                        }
                        whichPerson += 1;
                    }
                    break;
                case 3:
                    bool dalej = true; // flaga pozwalająca na zabezpieczeniem przed kliknięciem przycisku "Dalej" bez podania imion i nazwisk pasażerów
                    for (int i = 0; i < (Int16.Parse(tbAdults.Text) + Int16.Parse(tbChildren.Text)); i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            if (PojemnikSamolot.textBox[i, j].Text == "")
                                dalej = false;
                        }
                    }
                    // jeśli wszystkie pola są zapełnione wyświetlany jest widok samolotu
                    if (dalej == true)
                    {
                        tcTicket.SelectedIndex = PojemnikSamolot.newIndex;

                        //wpisywanie do pojemnika miejsca wylotu i miejsca docelowego
                        PojemnikSamolot.skad=cBoxFlyFrom.SelectedItem.ToString();
                        PojemnikSamolot.dokad=cboxFlyTo.SelectedItem.ToString();
                        //w zaleznosci od wybranych skad i dokad wybierany jest typ wyświetlanego samolotu
                        try
                        {
                            if(ilosc_pobran > 0)
                            {
                                con.Close();
                                con.Open();
                            }
                            using (MySqlCommand command = new MySqlCommand("SELECT ty.row_no,ty.place_per_row,ty.buffor_row,ty.buffor_place FROM types ty JOIN flyfit ft ON ty.name = ft.name WHERE ft.skad = '" + cBoxFlyFrom.SelectedItem.ToString() + "' AND ft.dokad = '" + cboxFlyTo.SelectedItem.ToString() + "'", con))
                            {
                                MySqlDataReader reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    PojemnikSamolot.il_rzedow = reader.GetInt32(0);
                                    PojemnikSamolot.il_siedzen_w_rzedzie = reader.GetInt32(1);
                                    PojemnikSamolot.szerokosc_buforowa_rzedow = reader.GetInt32(2);
                                    PojemnikSamolot.szerokosc_buforowa_siedzen = reader.GetInt32(3);
                                    ilosc_pobran += 1;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Błąd odczytu z bazy" + ex);
                        }
                        con.Close();
                        PojemnikSamolot.max_il_miejsc = (Int16.Parse(tbAdults.Text) + Int16.Parse(tbChildren.Text));
                        Samolot widok_samolot = new Samolot();
                        widok_samolot.DataContext = this;
                        widok_samolot.Show();
                        // w zależności od tego co wybrane zostało w combo boxie tutaj ustawiana jest zmienna
                        if (cBoxClasses.SelectedValue.ToString() == "Premium") 
                        {
                            PojemnikSamolot.klasa_samolot = 1;
                        }
                        else
                        {
                            PojemnikSamolot.klasa_samolot = 2;
                        }
                    }
                    // w przeciwnym wypadku wyświetlany jest komunikat o błędzie
                    else
                        MessageBox.Show("Wpisz imiona i nazwiska wszystkich pasażerów");
                    break;
                case 4:
                    // wypełnianie danych o zakupionym bilecie
                    tcTicket.SelectedIndex = PojemnikSamolot.newIndex;
                    butSaveTicket.IsEnabled = true; // włączenie możliwości zapisania biletu
                    labFlyFrom.Content = cBoxFlyFrom.SelectedItem.ToString();
                    labFlyTo.Content = cboxFlyTo.SelectedItem.ToString();
                    labClass.Content = cBoxClasses.SelectedItem.ToString();
                    numberOfAdults.Content = tbAdults.Text;
                    numberOfChildren.Content = tbChildren.Text;
                    numberOfBabies.Content = tbBabies.Text;
                    // przykładowe ceny biletów w klasie zwykłej
                    if (cBoxClasses.SelectedItem.ToString() == "Zwykła")
                    {
                        adultsCost.Content = "100";
                        childrenCost.Content = "73";
                        babiesCost.Content = "20";
                    }
                    // przykładowe ceny biletów w klasie premium
                    else if (cBoxClasses.SelectedItem.ToString() == "Premium")
                    {
                        adultsCost.Content = "150";
                        childrenCost.Content = "110";
                        babiesCost.Content = "30";
                    }
                    // zapełnienie elementów treścią w zależności od wybrania typu lotu
                    // lot w jedną stronę
                    if (rbOneWay.IsChecked == true)
                    {
                        labTypeOfTravel.Content = rbOneWay.Content;
                        labDateOfDeparture.Content = dateOfDeparture.SelectedDate.Value.Date;
                        labDateOfArrival.Content = "--.--.----";
                        adultsCostSum.Content = (int.Parse(adultsCost.Content.ToString()) * int.Parse(numberOfAdults.Content.ToString())).ToString();
                        childrenCostSum.Content = (int.Parse(childrenCost.Content.ToString()) * int.Parse(numberOfChildren.Content.ToString())).ToString();
                        babiesCostSum.Content = (int.Parse(babiesCost.Content.ToString()) * int.Parse(numberOfBabies.Content.ToString())).ToString();
                    }
                    // lot w dwie strony
                    else if (rbTwoWay.IsChecked == true)
                    {
                        labTypeOfTravel.Content = rbTwoWay.Content;
                        labDateOfDeparture.Content = dateOfDeparture.SelectedDate.Value.Date;
                        labDateOfArrival.Content = dateOfArrival.SelectedDate.Value.Date;
                        adultsCostSum.Content = (2 * int.Parse(adultsCost.Content.ToString()) * int.Parse(numberOfAdults.Content.ToString())).ToString();
                        childrenCostSum.Content = (2 * int.Parse(childrenCost.Content.ToString()) * int.Parse(numberOfChildren.Content.ToString())).ToString();
                        babiesCostSum.Content = (2 * int.Parse(babiesCost.Content.ToString()) * int.Parse(numberOfBabies.Content.ToString())).ToString();
                    }
                    Cost.Content = (int.Parse(adultsCostSum.Content.ToString()) + int.Parse(childrenCostSum.Content.ToString()) + int.Parse(babiesCostSum.Content.ToString())).ToString() + " zł";
                    // tworzenie dynamicznej siatki w celu wypisania danych pasażerów, oraz wybranych przez nich miejsc
                    DynamicGrid SumGrid = new DynamicGrid();
                    SummaryGrid.Children.Clear();
                    SummaryGrid.RowDefinitions.Clear();
                    SummaryGrid.ColumnDefinitions.Clear();
                    SumGrid.MyGrid(SummaryGrid, HorizontalAlignment.Stretch, VerticalAlignment.Stretch, false, Colors.WhiteSmoke);
                    for (int i = 0; i < 2 * (Int16.Parse(tbAdults.Text) + Int16.Parse(tbChildren.Text)) + 1; i++)
                    {
                        if (i % 2 != 0)
                        {
                            SumGrid.NewRow(SummaryGrid, 26, GridUnitType.Pixel);
                            SumGrid.NewRow(SummaryGrid, 26, GridUnitType.Pixel);
                            SumGrid.NewRow(SummaryGrid, 26, GridUnitType.Pixel);
                        }
                        else
                            SumGrid.NewRow(SummaryGrid, 50, GridUnitType.Pixel);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        if (i % 2 != 0)
                            SumGrid.NewColumn(SummaryGrid, 120, GridUnitType.Pixel);
                        else
                            SumGrid.NewColumn(SummaryGrid, 70, GridUnitType.Pixel);
                    }
                    whichPerson = 1;
                    for (Int16 i = 3; i < 4 * (Int16.Parse(tbAdults.Text) + Int16.Parse(tbChildren.Text)) + 1; i += 4)
                    {
                        tmp = 0;
                        for (Int16 j = 1; j <= 3; j += 2)
                        {
                            if (j == 1)
                            {
                                TextBox txtBox = new TextBox();
                                SumGrid.NewLabel(SummaryGrid, Colors.Black, 12, FontWeights.SemiBold, FontStyles.Normal, "Segoe UI", VerticalAlignment.Bottom, HorizontalAlignment.Center, "Pasażer" + whichPerson.ToString(), (Int16)(i -2), (Int16)(j - 1));
                                SumGrid.NewLabel(SummaryGrid, Colors.Black, 12, FontWeights.SemiBold, FontStyles.Normal, "Segoe UI", VerticalAlignment.Bottom, HorizontalAlignment.Left, "Imię:", (Int16)(i - 3), j);
                                SumGrid.NewLabel(SummaryGrid, Colors.Black, 12, FontWeights.SemiBold, FontStyles.Normal, "Segoe UI", VerticalAlignment.Bottom, HorizontalAlignment.Left, "Rząd:", (Int16)(i - 1), j);
                                txtBox = SumGrid.NewTextBox(SummaryGrid, PojemnikSamolot.textBox[whichPerson - 1, tmp].Text, 12, FontWeights.SemiBold, FontStyles.Normal, VerticalAlignment.Center, Colors.Black, (Int16)(i-2), j, false);
                                txtBox = SumGrid.NewTextBox(SummaryGrid, PojemnikSamolot.informacja[whichPerson - 1].rzad.ToString(), 12, FontWeights.SemiBold, FontStyles.Normal, VerticalAlignment.Center, Colors.Black, i, j, false);
                                tmp = 1;
                            }
                            else
                            {
                                TextBox txtBox = new TextBox();
                                SumGrid.NewLabel(SummaryGrid, Colors.Black, 12, FontWeights.SemiBold, FontStyles.Normal, "Segoe UI", VerticalAlignment.Bottom, HorizontalAlignment.Left, "Miejsce:", (Int16)(i - 1), j);
                                SumGrid.NewLabel(SummaryGrid, Colors.Black, 12, FontWeights.SemiBold, FontStyles.Normal, "Segoe UI", VerticalAlignment.Bottom, HorizontalAlignment.Left, "Nazwisko:", (Int16)(i - 3), j);
                                txtBox = SumGrid.NewTextBox(SummaryGrid, PojemnikSamolot.textBox[whichPerson - 1, tmp].Text, 12, FontWeights.SemiBold, FontStyles.Normal, VerticalAlignment.Center, Colors.Black, (Int16)(i - 2), j, false);
                                txtBox = SumGrid.NewTextBox(SummaryGrid, PojemnikSamolot.informacja[whichPerson - 1].miejsce.ToString(), 12, FontWeights.SemiBold, FontStyles.Normal, VerticalAlignment.Center, Colors.Black, i, j, false);
                            }
                        }
                        whichPerson += 1;
                    }
                    break;
            }  

        }
        // przyciski "Wstecz"
        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = tcTicket.SelectedIndex - 1;
            tcTicket.SelectedIndex = newIndex;

            if (newIndex == 2)
            {
                PojemnikSamolot.ilosc_miejsc = 0;
            }
            if (newIndex == 3)
            {
                butSaveTicket.IsEnabled = false; // wyłączenie możliwości zapisu biletu
            }
        }
        //***********-----------/////////\\\\\\\\\-----------***********\\

        //***********-----------Dodawanie i odejmowanie osob-----------***********\\
        // dodawanie
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddingAndSubstracting Add = new AddingAndSubstracting();
            // maksymalna liczba kolejno - dorosłych, dzieci i niemowląt
            int maxAdults = 10;
            int maxChildren = 10-Int16.Parse(tbAdults.Text);
            int maxBabies = Int16.Parse(tbAdults.Text);
            int maxDouble = Int16.Parse(tbAdults.Text);
            // dodawanie zostało stworzone jako zdarzenie dla wciskania każdego z przycisków dodawania osoby, więc należy rozdzielić obsługe każdego z przycisków (przycisku dodawania dorosłych, dzieci oraz niemowląt)
            // akcja zależy od nazwy danego przycisku. Wykorzystywana jest metoda z zewnętrznej klasy "AddingAndSubstracting"
            switch (((Button)sender).Name)
            {
                case "butAdultsAdd":
                    if ((Int16.Parse(tbAdults.Text) + Int16.Parse(tbChildren.Text)) < 10)
                    {
                        Add.NumericUp(tbAdults, maxAdults);
                    }
                    if (cBoxClasses.Text != "")
                    {
                        butNext2.IsEnabled = true;
                    }
                    break;
                case "butChildrenAdd":
                    if ((Int16.Parse(tbAdults.Text) + Int16.Parse(tbChildren.Text)) < 10)
                    {
                        Add.NumericUp(tbChildren, maxChildren);
                    }
                    if (cBoxClasses.Text != "")
                    {
                        butNext2.IsEnabled = true;
                    }
                    break;
                case "butBabiesAdd":
                    Add.NumericUp(tbBabies, maxBabies);
                    break;
            }
        }
        // odejmowanie
        private void ButtonSubtract_Click(object sender, RoutedEventArgs e)
        {
            AddingAndSubstracting Subtract = new AddingAndSubstracting();
            // tak samo jak w dodawaniu
            switch (((Button)sender).Name)
            {
                case "butAdultsSubtract":
                    Subtract.NumericDown(tbAdults);
                    if (Int16.Parse(tbAdults.Text) < Int16.Parse(tbBabies.Text))
                    {
                        Subtract.NumericDown(tbBabies);
                    }
                    if (tbAdults.Text == "0" && tbChildren.Text == "0")
                    {
                        butNext2.IsEnabled = false;
                    }
                    break;
                case "butChildrenSubtract":
                    Subtract.NumericDown(tbChildren);
                    if (tbAdults.Text == "0" && tbChildren.Text == "0")
                    {
                        butNext2.IsEnabled = false;
                    }
                    break;
                case "butBabiesSubtract":
                    Subtract.NumericDown(tbBabies);
                    break;
            }
        }
        //***********-----------/////////\\\\\\\\\-----------***********\\

        //***********-----------Klasy lotów-----------***********\\
        private void ComBoxClassesIni()
        {
            //klasy przeniesione do pojemnika samolotowego !
            //string[] classes = { "Zwykła", "Premium" };
            cBoxClasses.ItemsSource = PojemnikSamolot.classes;
        }
        //***********-----------/////////\\\\\\\\\-----------***********\\

        //***********-----------Zabezpieczenia przed przechodzeniem do kolejnych tabów bez uzupełnienia wszystkich danych-----------***********\\
        // zabezpieczenie przed umożliwieniem przejścia do kolejnego taba w wypadku, gdy wybrano opcję lotu w jedną stronę, a nie podano daty wylotu i miejsca docelowego
        private void OneWay(object sender, RoutedEventArgs e)
        {
            dateOfArrival.IsEnabled = false;
            if (dateOfDeparture.Text != "" && cboxFlyTo.Text != "")
                butNext1.IsEnabled = true;
            else
                butNext1.IsEnabled = false;
        }

        // zabezpieczenie przed umożliwieniem przejścia do kolejnego taba w wypadku, gdy wybrano opcję lotu w dwie, a nie podano daty wylotu, przylotu i miejsca docelowego
        private void TwoWay(object sender, RoutedEventArgs e)
        {
            dateOfArrival.IsEnabled = true;
            if (dateOfDeparture.Text != "" && dateOfArrival.Text != "" && cboxFlyTo.Text != "")
                butNext1.IsEnabled = true;
            else
                butNext1.IsEnabled = false;
        }

        // zabezpieczenie przed umożliwieniem przejścia do kolejnego taba w wypadku, gdy wybrano klasę lotu, a nie wybrano liczby dorosłych lub dzieci
        private void cBoxClasses_MouseLeave(object sender, MouseEventArgs e)
        {
            if (tbAdults.Text == "0" && tbChildren.Text == "0")
            {
                butNext2.IsEnabled = false;
            }
            else if ((tbAdults.Text != "0" || tbChildren.Text != "0") && cBoxClasses.Text != "")
            {
                butNext2.IsEnabled = true;
            }
        }

        // zabezpieczenie przed pozostawieniem pustych dat odlotu i przylotu
        private void Protection(object sender, RoutedEventArgs e)
        {
            if (dateOfArrival.IsEnabled == false && dateOfDeparture.Text != "" && cboxFlyTo.Text != "")
                butNext1.IsEnabled = true;
            else if (dateOfArrival.IsEnabled == true && dateOfDeparture.Text != "" && dateOfArrival.Text != "" && cboxFlyTo.Text != "")
                butNext1.IsEnabled = true;
            else
                butNext1.IsEnabled = false;
        }
        //***********-----------/////////\\\\\\\\\-----------***********\\

        // wylogowywanie się
        private void Logout(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            PojemnikSamolot.uprawnienia_uzytkownika = 0;
            this.Close();
        }

        // zapisywanie do pliku JPG wybranego biletu
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            con.Open();

            if(ChooseTwoWay == 0)
            {
                int whichPerson = 1;
                for (Int16 i = 3; i < 4 * (Int16.Parse(tbAdults.Text) + Int16.Parse(tbChildren.Text)) + 1; i += 4)
                {
                    //jako że połączenie rzad,miejsce,skad,dokad jest unique więc nie ma możliwości nadpisywania 
                    try
                    {
                        if (ilosc_zapisow > 0)
                        {
                            con.Close();
                            con.Open();
                        }
                        //pobieranie danych z tabeli

                        using (MySqlCommand command = new MySqlCommand("SELECT MAX(id) FROM tickets", con))
                        {
                            MySqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                max_id = reader.GetInt32(0);
                                reader.Close();
                                con.Close();
                                con.Open();
                            }
                        }
                        // zapisanie do pazy danych nowego użytkownika
                        using (MySqlCommand command = new MySqlCommand("INSERT INTO tickets VALUES(@id, @imie, @nazwisko,@rzad,@miejsce,@skad,@dokad)", con))
                        {
                            command.Parameters.Add(new MySqlParameter("id", (max_id + 1)));
                            command.Parameters.Add(new MySqlParameter("imie", PojemnikSamolot.textBox[whichPerson - 1, 0].Text));
                            command.Parameters.Add(new MySqlParameter("nazwisko", PojemnikSamolot.textBox[whichPerson - 1, 1].Text));
                            command.Parameters.Add(new MySqlParameter("rzad", PojemnikSamolot.informacja[whichPerson - 1].rzad.ToString()));
                            command.Parameters.Add(new MySqlParameter("miejsce", PojemnikSamolot.informacja[whichPerson - 1].miejsce.ToString()));
                            command.Parameters.Add(new MySqlParameter("skad", cBoxFlyFrom.SelectedItem.ToString()));
                            command.Parameters.Add(new MySqlParameter("dokad", cboxFlyTo.SelectedItem.ToString()));
                            command.ExecuteNonQuery();
                        }
                        whichPerson += 1;
                    }
                    catch
                    {
                        MessageBox.Show("W międzyczasie podczas Twojego kupowania biletu ktoś zarezerwował jedno z wybranych przez Ciebie miejsc, przejdź przez proces zakupu ponownie");
                        con.Close();
                        con.Open();
                    }
                }
                if (whichPerson == 2)
                {
                    MessageBox.Show("Użytkownik dodany");
                }
                else
                {
                    MessageBox.Show("Użytkownicy dodani");
                }
            }
            else
            {
                int whichPerson = 1;
                for (Int16 i = 3; i < 4 * (Int16.Parse(tbAdults.Text) + Int16.Parse(tbChildren.Text)) + 1; i += 4)
                {
                    //jako że połączenie rzad,miejsce,skad,dokad jest unique więc nie ma możliwości nadpisywania 
                    try
                    {
                        if (ilosc_zapisow > 0)
                        {
                            con.Close();
                            con.Open();
                        }
                        //pobieranie danych z tabeli

                        using (MySqlCommand command = new MySqlCommand("SELECT MAX(id) FROM tickets", con))
                        {
                            MySqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                max_id = reader.GetInt32(0);
                                reader.Close();
                                con.Close();
                                con.Open();
                            }
                        }
                        // zapisanie do pazy danych nowego użytkownika
                        using (MySqlCommand command = new MySqlCommand("INSERT INTO tickets VALUES(@id, @imie, @nazwisko,@rzad,@miejsce,@skad,@dokad)", con))
                        {
                            command.Parameters.Add(new MySqlParameter("id", (max_id + 1)));
                            command.Parameters.Add(new MySqlParameter("imie", PojemnikSamolot.textBox[whichPerson - 1, 0].Text));
                            command.Parameters.Add(new MySqlParameter("nazwisko", PojemnikSamolot.textBox[whichPerson - 1, 1].Text));
                            command.Parameters.Add(new MySqlParameter("rzad", PojemnikSamolot.informacja[whichPerson - 1].rzad.ToString()));
                            command.Parameters.Add(new MySqlParameter("miejsce", PojemnikSamolot.informacja[whichPerson - 1].miejsce.ToString()));
                            command.Parameters.Add(new MySqlParameter("skad", cBoxFlyFrom.SelectedItem.ToString()));
                            command.Parameters.Add(new MySqlParameter("dokad", cboxFlyTo.SelectedItem.ToString()));
                            command.ExecuteNonQuery();
                        }
                        whichPerson += 1;
                    }
                    catch
                    {
                        MessageBox.Show("W międzyczasie podczas Twojego kupowania biletu ktoś zarezerwował jedno z wybranych przez Ciebie miejsc, przejdź przez proces zakupu ponownie");
                        con.Close();
                        con.Open();
                    }
                }
                whichPerson = 1;
                for (Int16 i = 3; i < 4 * (Int16.Parse(tbAdults.Text) + Int16.Parse(tbChildren.Text)) + 1; i += 4)
                {
                    //jako że połączenie rzad,miejsce,skad,dokad jest unique więc nie ma możliwości nadpisywania 
                    try
                    {
                        if (ilosc_zapisow > 0)
                        {
                            con.Close();
                            con.Open();
                        }
                        //pobieranie danych z tabeli

                        using (MySqlCommand command = new MySqlCommand("SELECT MAX(id) FROM tickets", con))
                        {
                            MySqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                max_id = reader.GetInt32(0);
                                reader.Close();
                                con.Close();
                                con.Open();
                            }
                        }
                        // zapisanie do pazy danych nowego użytkownika
                        using (MySqlCommand command = new MySqlCommand("INSERT INTO tickets VALUES(@id, @imie, @nazwisko,@rzad,@miejsce,@skad,@dokad)", con))
                        {
                            command.Parameters.Add(new MySqlParameter("id", (max_id + 1)));
                            command.Parameters.Add(new MySqlParameter("imie", PojemnikSamolot.textBox[whichPerson - 1, 0].Text));
                            command.Parameters.Add(new MySqlParameter("nazwisko", PojemnikSamolot.textBox[whichPerson - 1, 1].Text));
                            command.Parameters.Add(new MySqlParameter("rzad", PojemnikSamolot.informacja[whichPerson - 1].rzad.ToString()));
                            command.Parameters.Add(new MySqlParameter("miejsce", PojemnikSamolot.informacja[whichPerson - 1].miejsce.ToString()));
                            command.Parameters.Add(new MySqlParameter("skad", cboxFlyTo.SelectedItem.ToString()));
                            command.Parameters.Add(new MySqlParameter("dokad", cBoxFlyFrom.SelectedItem.ToString()));
                            command.ExecuteNonQuery();
                        }
                        whichPerson += 1;
                    }
                    catch
                    {
                        MessageBox.Show("W międzyczasie podczas Twojego kupowania biletu ktoś zarezerwował jedno z wybranych przez Ciebie miejsc, przejdź przez proces zakupu ponownie");
                        con.Close();
                        con.Open();
                    }
                }
                if (whichPerson == 2)
                {
                    MessageBox.Show("Użytkownik dodany");
                }
                else
                {
                    MessageBox.Show("Użytkownicy dodani");
                }
            }

            // przejście do góry widoku biletu
            scrollView.ScrollToTop();
            // utworzenie katalogu "Aplikacja LOT" w Dokumentach. Tam będą zapisywane bilety
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "Aplikacja PRL");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            Ticket ticket = new Ticket();
            ticket.Owner = this;
            ticket.ShowDialog();
        }

        // otworzenie okna "FAQ"
        private void butFAQ_Click(object sender, RoutedEventArgs e)
        {
            FAQ faq = new FAQ();
            faq.Show();
        }

        private void rbTwoWay_Checked(object sender, RoutedEventArgs e)
        {
            ChooseTwoWay = 1;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Pomoc pomoc = new Pomoc();
            pomoc.Show();
        }
    }
}
