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

namespace HotelRezerwacje
{
    /// <summary>
    /// Logika interakcji dla klasy HotelManagement.xaml
    /// </summary>
    public partial class HotelManagement : Window
    {
        public HotelManagement()
        {
            InitializeComponent();
        }

        private void RoomsButton_Click(object sender, RoutedEventArgs e)
        {
            HotelManagmentRooms hotelManagmentRooms = new HotelManagmentRooms();
            hotelManagmentRooms.Show();
            this.Close();
        }

        private void HotelButton_Click(object sender, RoutedEventArgs e)
        {
            HotelManagementHotels hotelManagement = new HotelManagementHotels();
            hotelManagement.Show();
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
