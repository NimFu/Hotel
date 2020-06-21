using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Logika interakcji dla klasy ResCheckId.xaml
    /// </summary>
    public partial class ResCheckId : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-HBHGS1M\SQLEXPRESS; Initial Catalog=HotelMenagment; Integrated Security =true;");
        public ResCheckId()
        {
            InitializeComponent();
        }

        private void CancleButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            if (ResNumberTxt.Text.Length == 0)
            {
                MessageBox.Show("Nie wprowadzono numeru rezerwacji.", "Błąd", MessageBoxButton.OK);
            }
            else if (SurnameTxt.Text.Length == 0)
            {
                MessageBox.Show("Nie wprowadzono nazwiska klienta.", "Błąd", MessageBoxButton.OK);
            }
            else
            {
                String query = "SELECT Number FROM Rooms Inner Join ReservationsID On Rooms.ID = ReservationsID.Room_ID Inner Join Customer on ReservationsID.Customer_ID = Customer.ID WHERE Is_Reserved = 1 and Customer.Surname =@Surname and ReservationsID.Res_Number=@ResNumber and DATEStart>@Data";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@Surname", SurnameTxt.Text);
                sqlCommand.Parameters.AddWithValue("@ResNumber", ResNumberTxt.Text);
                sqlCommand.Parameters.AddWithValue("@Data", DateTime.Now);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        RoomTxt.Text = "Klient zarezerwował pokój: " + sqlDataReader.GetInt32(0);
                    }
                }
                else MessageBox.Show("Nie znaleziono rezerwacji sprawdź dane.", "Błąd", MessageBoxButton.OK);
                sqlDataReader.Close();
                String query1 = "SELECT NAME FROM Hotels Inner Join Rooms on Hotels.ID = Rooms.Hotel_ID Inner Join ReservationsID on Rooms.ID = ReservationsID.Room_ID Inner Join Customer on ReservationsID.Customer_ID = Customer.ID WHERE Is_Reserved = 1 and Customer.Surname =@Surname and ReservationsID.Res_Number=@ResNumber";
                SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection);
                sqlCommand1.CommandType = CommandType.Text;
                sqlCommand1.Parameters.AddWithValue("@Surname", SurnameTxt.Text);
                sqlCommand1.Parameters.AddWithValue("@ResNumber", ResNumberTxt.Text);
                SqlDataReader sqlDataReader1 = sqlCommand1.ExecuteReader();
                while (sqlDataReader1.Read())
                {
                    HotelTxt.Text = "w hotelu: " + sqlDataReader1.GetString(0);
                }
                sqlConnection.Close();
            }


        }
    }
}
