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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelRezerwacje
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ReservationButton_Click(object sender, RoutedEventArgs e)
        {

            AddReservation addWindow = new AddReservation();
            addWindow.Show();
            this.Close();
        }

        private void EditionButton_Click(object sender, RoutedEventArgs e)
        {
            EditRess addWindow = new EditRess();
            addWindow.Show();
            this.Close();
        }

        private void MenagmentButton_Click(object sender, RoutedEventArgs e)
        {
            ManagerIdCheck addWindow = new ManagerIdCheck();
            addWindow.Show();
            this.Close();
        }


        private void ShowResButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeCheckID addWindow = new EmployeeCheckID();
            addWindow.Show();
            this.Close();
        }
    }
}
