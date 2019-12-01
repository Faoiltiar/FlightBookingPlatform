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
using MySql.Data.MySqlClient;

namespace LOT
{
    /// <summary>
    /// Interaction logic for DopasowanieLotySamoloty.xaml
    /// </summary>
    public partial class DopasowanieLotySamoloty : Window
    {
        MySqlConnection con = new MySqlConnection(); //odpowiedzialna za polaczenie
        public static List<string> types = new List<string>();

        public DopasowanieLotySamoloty()
        {
            InitializeComponent();
            ComBoxFlyIni();

            con.ConnectionString = "server=localhost;user id=root;password=toor;persistsecurityinfo=True;database=plane"; //tu wklejamy connection stringa 
            con.Open();
            try
            {
                using (MySqlCommand command = new MySqlCommand("SELECT name FROM types", con))
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        types.Add(reader.GetString(0));
                    }
                }
                string[] typySamolotow = types.ToArray();
                cBoxTypes.ItemsSource = typySamolotow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd odczytu z bazy" + ex);
            }
            con.Close();
        }

        //***********-----------Listy rozwijalne z wyborem miast wylotu i przylotu-----------***********\\
        private void cBoxFlyFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] cities = { "Warszawa", "Kraków", "Gdańsk", "Poznań" };
            cBoxFlyTo_Ini(cities);
        }
        public void ComBoxFlyIni()
        {
            string[] cities = { "Warszawa", "Kraków", "Gdańsk", "Poznań" };
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
            for (int i = 0; i < cities.Length; i++)
            {
                if (cBoxFlyFrom.SelectedItem.ToString() == cities[i])
                    continue;
                list.Add(cities[i]);
            }
            cboxFlyTo.ItemsSource = list;
        }
        //***********-----------/////////\\\\\\\\\-----------***********\\


        private void Logout(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            PojemnikSamolot.uprawnienia_uzytkownika = 0;
            this.Close();
        }

        private void butZapisz_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            try
            {
                using (MySqlCommand command = new MySqlCommand("UPDATE flyfit SET name=@namee WHERE skad =@skadd AND dokad =@dokadd", con))
                {
                    command.Parameters.Add(new MySqlParameter("namee", cBoxTypes.SelectedItem.ToString()));
                    command.Parameters.Add(new MySqlParameter("skadd", cBoxFlyFrom.SelectedItem.ToString()));
                    command.Parameters.Add(new MySqlParameter("dokadd", cboxFlyTo.SelectedItem.ToString()));
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Trasa zaktualizowana");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd odczytu z bazy" + ex);
            }
            con.Close();
        }

        private void ComeBack(object sender, MouseButtonEventArgs e)
        {
            Admin_wybor ad_wyb = new Admin_wybor();
            ad_wyb.Show();
            this.Close();
        }
    }
}
