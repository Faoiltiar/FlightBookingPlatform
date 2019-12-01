using System;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace LOT
{
    /// <summary>
    /// Interaction logic for Rejestracja.xaml
    /// </summary>
    public partial class Rejestracja : Window
    {

        MySqlConnection con = new MySqlConnection(); //odpowiedzialna za polaczenie
        public static int max_id;

        public Rejestracja()
        {
            InitializeComponent();
            con.ConnectionString = "server=localhost;user id=root;password=toor;persistsecurityinfo=True;database=plane"; //tu wklejamy connection stringa 
            con.Open();
        }

        //Wyświetlanie komunikatu o występowaniu różnych haseł
        private void Equality(object sender, KeyEventArgs e)
        {
            if (passwordBox.Password != passwordBox2.Password)
            {
                labWarning.Visibility = Visibility.Visible;
                registerBut.IsEnabled = false;
            }
            else
            {
                labWarning.Visibility = Visibility.Hidden;
                registerBut.IsEnabled = true;
            }
        }

        // wyświetlanie komunikatu o nieprawidłowym adresie e-mail
        private void Compatibility(object sender, RoutedEventArgs e)
        {
            string wzorzec = "[a-z0-9_]+@[a-z0-9]+.[a-z]+."; // wzór prawidłowego e-maila zapisany w postaci wyrażenia regularnego
            Regex regex = new Regex(wzorzec);
            if (!regex.IsMatch(txtBloxEmail.Text))
            {
                labEmailWarning.Visibility = Visibility.Visible;
                registerBut.IsEnabled = false;
            }
            else
            {
                labEmailWarning.Visibility = Visibility.Hidden;
                registerBut.IsEnabled = true;
            }
        }

        // obsługa przycisku rejestracji
        private void registerBut_Click(object sender, RoutedEventArgs e)
        {
            // konieczność wypełnienia wszystkich pól rejestracji
            if (txtBloxLogin.Text == "" || txtBloxEmail.Text == "" || passwordBox.Password == "" || passwordBox2.Password == "")
                labClickWarning.Content = "Wypełnij wszystkie pola!";
            // konieczność zaakceptowania polityki i regulaminu
            else if (acceptation.IsChecked == false)
                labClickWarning.Content = "Zaakceptuj politykę i regulamin!";
            else
            {
                labClickWarning.Content = "";
                try
                {
                    //pobieranie danych z tabeli
                    using (MySqlCommand command = new MySqlCommand("SELECT MAX(id) FROM users", con)) 
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
                    using (MySqlCommand command = new MySqlCommand("INSERT INTO users VALUES(@id, @login, @password,@email,@level)", con))
                    {
                        command.Parameters.Add(new MySqlParameter("id", (max_id + 1)));
                        command.Parameters.Add(new MySqlParameter("login", txtBloxLogin.Text));
                        command.Parameters.Add(new MySqlParameter("password", passwordBox.Password));
                        command.Parameters.Add(new MySqlParameter("email", txtBloxEmail.Text));
                        command.Parameters.Add(new MySqlParameter("level", 2));
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Użytkownik dodany");
                    con.Close();
                    this.Close();

                }
                catch
                {
                    MessageBox.Show("Istnieje już użytkownik o takim loginie,zmień login");
                    con.Close();
                    con.Open();
                }
            }
        }
    }
}
