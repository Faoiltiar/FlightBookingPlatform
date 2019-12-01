using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LOT
{
    class DynamicGrid
    {
        // metoda tworząca nową siatkę
        public void MyGrid(Grid grid, HorizontalAlignment horAlignment, VerticalAlignment verAlignment, bool showGrid, Color backgroundColor)
        {
            grid.HorizontalAlignment = horAlignment; // położenie w poziomie
            grid.VerticalAlignment = verAlignment; // położenie w pionie
            grid.ShowGridLines = showGrid; // pokazywanie linii siatki
            grid.Background = new SolidColorBrush(backgroundColor); // tło siatki
        }
        // metoda wstawiająca do siatki nową kolumnę
        public void NewColumn(Grid grid, Int16 columnWidth, GridUnitType type)
        {
            ColumnDefinition gridCol = new ColumnDefinition();
            gridCol.Width = new GridLength(columnWidth, type); // szerokość kolumny
            grid.ColumnDefinitions.Add(gridCol);
        }
        //metoda wstawiająca do siatki nowy rząd
        public void NewRow(Grid grid, Int16 rowHeight, GridUnitType type)
        {
            RowDefinition gridRow = new RowDefinition();
            gridRow.Height = new GridLength(rowHeight, type); // wysokość rzędu
            grid.RowDefinitions.Add(gridRow);
        }
        // metoda wstawiająca element "textBox"
        public TextBox NewTextBox(Grid grid, string text, Int16 size, FontWeight fontWeight, FontStyle fontStyle, VerticalAlignment verAlignment, Color foregroundColor, Int16 whichRow, Int16 whichColumn, bool enabled)
        {
            TextBox txtBox = new TextBox();
            txtBox.Text = text; // tekst w textBoxie
            txtBox.FontSize = size; // rozmiar tekstu
            txtBox.FontWeight = fontWeight; // pogrubienie czcionki
            txtBox.FontStyle = fontStyle; // styl czcionki
            txtBox.VerticalContentAlignment = verAlignment; // położenie tekstu w poziomie
            txtBox.Foreground = new SolidColorBrush(foregroundColor); // kolor tła
            Grid.SetRow(txtBox, whichRow);
            Grid.SetColumn(txtBox, whichColumn);
            grid.Children.Add(txtBox);
            txtBox.IsEnabled = enabled;
            return txtBox;
        }
        // metoda wstawiająca element "label"
        public void NewLabel(Grid grid, Color foreground ,Int16 size, FontWeight fontWeight, FontStyle fontStyle, string fontFamily, VerticalAlignment verAlignment, HorizontalAlignment horAlignment, string text, Int16 whichRow, Int16 whichColumn)
        {
            Label lab = new Label();
            lab.Foreground = new SolidColorBrush(foreground); // kolor tekstu
            lab.FontSize = size; // rozmiar tekstu
            lab.FontWeight = fontWeight; // pogrubienie czcionki
            lab.FontStyle = fontStyle; // styl czcionki
            lab.FontFamily = new FontFamily(fontFamily); //rodzaj czcionki
            lab.VerticalAlignment = verAlignment; // położenie w pionie
            lab.HorizontalAlignment = horAlignment; // połozenie w poziomie
            lab.Content = text; // zawartość elementu label
            Grid.SetRow(lab, whichRow);
            Grid.SetColumn(lab, whichColumn);
            grid.Children.Add(lab);
        }
    }
}
