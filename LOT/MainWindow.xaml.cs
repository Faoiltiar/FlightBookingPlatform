using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;


namespace LOT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MySqlConnection con = new MySqlConnection(); //odpowiedzialna za polaczenie

        Window1 okno = new Window1();


        public MainWindow()
        {
            InitializeComponent();
            con.ConnectionString = "server=localhost;user id=root;password=toor;persistsecurityinfo=True;database=plane"; //tu wklejamy tego connection stringa 
            con.Open();
        }

        // Obsługa kliknięcia przycisku "Zaloguj się"
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //jeśli nie wypełni się któregoś z wymaganych pól wyświetla się odpowiednie ostrzeżenie 
            if (textBox.Text == "" || passwordBox.Password == "")
            {
                labClickWarning.Content = "Wypełnij wszystkie pola!";
            }
            //w przeciwnym wypadku nastepuje zalogowanie jako jeden z zarejestrowanych użytkowników lub administrator
            else
            {
                labClickWarning.Content = "";
                con.Close();
                con.Open();
                try
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT IFNULL( (SELECT level FROM users WHERE login='" + textBox.Text + "' AND password='" + passwordBox.Password + "' LIMIT 1) ,0)", con)) //pobieranie danych z tabeli
                    {
                        MySqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            PojemnikSamolot.uprawnienia_uzytkownika = reader.GetInt32(0);
                             // jeśli logje się administrator, to otwiera się okno administratora
                            if (PojemnikSamolot.uprawnienia_uzytkownika == 1)
                            {

                                Admin_wybor admin_wybor = new Admin_wybor();
                                admin_wybor.Show();
                                this.Close();
                            }
                            // przy logowaniu się jako zwykły użytkownik uruchomi się platforma do rezerwowania biletów
                            else if (PojemnikSamolot.uprawnienia_uzytkownika == 2)
                            {
                                okno.Show();
                                this.Close();
                            }
                            // przy podaniu nieprawidłowych danych do logowania, wyświetla się okienko z informacją o błędzie logowania
                            else
                            {
                                MessageBox.Show("Nieznany użytkownik");
                                //zamknięcie strumienia             
                                reader.Close();
                                //zamknięcie oraz ponowne otworzenie poł. z bazą danych
                                con.Close();
                                con.Open();
                            }
                        }
                    }
                }
                catch //(Exception Wyj1)
                {
                    //MessageBox.Show("Błąd połączenia z bazą " + Wyj1.ToString());
                }
            }
        }

        // obsługa rejestracji
        private void Register(object sender, MouseButtonEventArgs e)
        {
            con.Close();
            Rejestracja register = new Rejestracja();
            register.Show();
        }
    }
}
