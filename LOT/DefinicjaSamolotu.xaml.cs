using System;
using System.Windows;
using MySql.Data.MySqlClient;

namespace LOT
{
    /// <summary>
    /// Interaction logic for DefinicjaSamolotu.xaml
    /// </summary>
    public partial class DefinicjaSamolotu : Window
    {
        MySqlConnection con = new MySqlConnection(); //odpowiedzialna za polaczenie
        public static int max_id;

        public DefinicjaSamolotu()
        {
            InitializeComponent();

            con.ConnectionString = "server=localhost;user id=root;password=toor;persistsecurityinfo=True;database=plane";
            con.Open();
        }

        private void butZapisz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //pobieranie danych z tabeli
                using (MySqlCommand command = new MySqlCommand("SELECT MAX(id) FROM types", con))
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
                using (MySqlCommand command = new MySqlCommand("INSERT INTO types VALUES(@id, @name,@row_no, @place_per_row,@buffor_row,@buffor_place)", con))
                {
                    command.Parameters.Add(new MySqlParameter("id", (max_id + 1)));
                    command.Parameters.Add(new MySqlParameter("name", TextNazwaSamolotu.Text));
                    command.Parameters.Add(new MySqlParameter("row_no", int.Parse(TextRzedy.Text.ToString())));
                    command.Parameters.Add(new MySqlParameter("place_per_row", int.Parse(TextSiedzenia.Text.ToString())));
                    command.Parameters.Add(new MySqlParameter("buffor_row", int.Parse(TextBuforRzedy.Text.ToString())));
                    command.Parameters.Add(new MySqlParameter("buffor_place", int.Parse(TextBuforSiedzenia.Text.ToString())));
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Samolot o nazwie : " + TextNazwaSamolotu.Text + " został dodany do bazy");
            }
            catch
            {
                if(TextNazwaSamolotu.Text == "" || TextRzedy.Text == "" || TextSiedzenia.Text == "" || TextBuforRzedy.Text == "" || TextBuforRzedy.Text == "")
                {
                    MessageBox.Show("Nie podano wszystkich potrzebnych danych");
                }else
                {
                    MessageBox.Show("Istnieje już taki typ samolotu w bazie");
                }
                con.Close();
                con.Open();
            }
        }

        private void butPodglad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //wstawienie nowych wartości samolotu 
                PojemnikSamolot.il_rzedow = int.Parse(TextRzedy.Text.ToString());
                PojemnikSamolot.il_siedzen_w_rzedzie = int.Parse(TextSiedzenia.Text.ToString());
                PojemnikSamolot.szerokosc_buforowa_rzedow = int.Parse(TextBuforRzedy.Text.ToString());
                PojemnikSamolot.szerokosc_buforowa_siedzen = int.Parse(TextBuforSiedzenia.Text.ToString());
                PojemnikSamolot.max_il_miejsc = 0;
                if (PojemnikSamolot.il_siedzen_w_rzedzie < 10 || PojemnikSamolot.il_rzedow < 3 || PojemnikSamolot.szerokosc_buforowa_rzedow < 50 || PojemnikSamolot.szerokosc_buforowa_siedzen < 50)
                {
                    MessageBox.Show("Z takimi danymi to nie zdziw się, że to będzie wyglądać jak helikopter");
                }
                Samolot okno_samolotu = new Samolot();
                okno_samolotu.Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Wybierz jakieś wartości, nie można pozostawić niczego pustego");
            }     
        }

        private void Logout(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            PojemnikSamolot.uprawnienia_uzytkownika = 0;
            this.Close();
        }

        private void ComeBack(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Admin_wybor ad_wyb = new Admin_wybor();
            ad_wyb.Show();
            this.Close();
        }
    }
}
