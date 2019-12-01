using System.Windows;

namespace LOT
{
    /// <summary>
    /// Interaction logic for FAQ.xaml
    /// </summary>
    public partial class FAQ : Window
    {
        public FAQ()
        {
            InitializeComponent();
            textBlock1.Text = "W klasie zwykłej - 1 sztuka do 8 kg" + System.Environment.NewLine + "W klasie premium - 2 sztuki w sumie do 12 kg(1 sztuka maksymalnie 8kg)";
        }
    }
}
