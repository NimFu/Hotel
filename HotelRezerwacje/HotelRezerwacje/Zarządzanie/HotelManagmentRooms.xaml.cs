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
    /// Logika interakcji dla klasy HotelManagmentRooms.xaml
    /// </summary>


    public partial class HotelManagmentRooms : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-HBHGS1M\SQLEXPRESS; Initial Catalog=HotelMenagment; Integrated Security =true;");
        private int Hotel_ID;
        private int RoomNumber;

        public HotelManagmentRooms()
        {
            InitializeComponent();
        }

        private void Get_Hotel_ID()
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            String query = "SELECT ID FROM Hotels WHERE NAME=@NAME";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Parameters.AddWithValue("@NAME", HotelCB.SelectedValue);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Hotel_ID = sqlDataReader.GetInt32(0);
            }
            sqlConnection.Close();

        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            HotelManagement hotelManagement = new HotelManagement();
            hotelManagement.Show();
            this.Close();
        }

        private void HotelButton_Click(object sender, RoutedEventArgs e)
        {
            HotelManagementHotels hotelManagementHotels = new HotelManagementHotels();
            hotelManagementHotels.Show();
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExcludeButton_Click(object sender, RoutedEventArgs e)
        {

            if (HotelCB.SelectedIndex >= 0)
            {
                if (RoomCB.SelectedIndex >= 0)
                {
                    MessageBoxResult result = MessageBox.Show("Czy na wykluczyć pokój z listy dostępnych?.", "Potwierdzenie", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            if (sqlConnection.State == ConnectionState.Closed)
                                sqlConnection.Open();
                            String query = "UPDATE Rooms SET Is_Renovated=1 WHERE Number=@Number";
                            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                            sqlCommand.CommandType = CommandType.Text;
                            sqlCommand.Parameters.AddWithValue("@Number", RoomCB.Text);
                            sqlCommand.ExecuteNonQuery();
                            MessageBox.Show("Operacja zrealizowana.", "Adnotacja", MessageBoxButton.OK);
                            RoomCB.Items.Clear();
                            HotelCB.Items.Clear();
                            sqlConnection.Close();
                            break;
                        case MessageBoxResult.No:
                            MessageBox.Show("Anulowano operacje.", "Adnotacja", MessageBoxButton.OK);
                            break;
                    }
                }
                else MessageBox.Show("Nie wybrano żadnego pokoju.", "Błąd", MessageBoxButton.OK);
            }
            else MessageBox.Show("Wybierz hotel do którego wprowadzić zmiany.", "Błąd", MessageBoxButton.OK);
        }

        private void HotelCB_Initialized(object sender, EventArgs e)
        {
            HotelCB.Items.Clear();
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            String query = "SELECT NAME FROM Hotels";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                HotelCB.Items.Add(sqlDataReader["NAME"]);
            }
            sqlConnection.Close();
        }





        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            if (RoomTxT.Text.Length > 0)
            {
                if (HotelCB.SelectedIndex >= 0)
                {
                    MessageBoxResult result = MessageBox.Show("Czy na pewno dodać pokój ?.", "Potwierdzenie", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Get_Hotel_ID();
                            String query = "Insert into Rooms (Number,Hotel_ID,Is_Reserved,Is_Renovated) values ('" + this.RoomTxT.Text + "','" + Hotel_ID + "' , '" + 0 + "' , '" + 0 + "')";
                            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                            sqlCommand.ExecuteNonQuery();
                            MessageBox.Show("Dodano pokój.", "Adnotacja", MessageBoxButton.OK);
                            RoomCB.Items.Clear();
                            String query1 = "SELECT Number FROM Rooms WHERE Hotel_ID=@Hotel_ID and Is_Reserved=0 and not Is_Renovated=1";
                            SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection);
                            sqlCommand1.CommandType = CommandType.Text;
                            sqlCommand1.Parameters.AddWithValue("@Hotel_ID", Hotel_ID);
                            SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
                            while (sqlDataReader.Read())
                            {
                                RoomCB.Items.Add(sqlDataReader["Number"]);
                            }
                            sqlConnection.Close();
                            break;
                        case MessageBoxResult.No:
                            MessageBox.Show("Anulowano dodawanie.", "Adnotacja", MessageBoxButton.OK);
                            RoomTxT.Clear();
                            break;
                    }
                }
                else MessageBox.Show("Wybierz hotel do którego chcesz dodać pokój.", "Błąd", MessageBoxButton.OK);
            }
            else MessageBox.Show("Podaj numer pokoju.", "Błąd", MessageBoxButton.OK);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            if (RoomCB.SelectedIndex >= 0)
            {
                MessageBoxResult result = MessageBox.Show("Czy na usunąć pokój z listy dostępnych?.", "Potwierdzenie", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        String query = "DELETE FROM Rooms WHERE Number=@Number";
                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.Parameters.AddWithValue("@Number", RoomCB.SelectedIndex);
                        sqlCommand.ExecuteNonQuery();
                        MessageBox.Show("Usunięto pokój.", "Adnotacja", MessageBoxButton.OK);
                        int hotel = HotelCB.SelectedIndex;
                        hotel += 1;
                        RoomCB.Items.Clear();
                        if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();
                        String query1 = "SELECT Number FROM Rooms WHERE Hotel_ID=@Hotel_ID and Is_Reserved=0 and not Is_Renovated=1";
                        SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection);
                        sqlCommand1.CommandType = CommandType.Text;
                        sqlCommand1.Parameters.AddWithValue("@Hotel_ID", hotel);
                        SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
                        while (sqlDataReader.Read())
                        {
                            RoomCB.Items.Add(sqlDataReader["Number"]);
                        }
                        sqlConnection.Close();
                        break;
                        case MessageBoxResult.No:
                            MessageBox.Show("Anulowano operację.", "Adnotacja", MessageBoxButton.OK);
                break;
            }
            }
            else MessageBox.Show("Nie wybrano żadnego pokoju.", "Błąd", MessageBoxButton.OK);
        }

        private void Get_Set_Free_Rooms()
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            String query = "SELECT COUNT(1) FROM Rooms WHERE Hotel_ID=@Hotel_ID and Is_Reserved=1 and DATE<@DATE";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Parameters.AddWithValue("@Hotel_ID", Hotel_ID);
            sqlCommand.Parameters.AddWithValue("@DATE", DateTime.Now);
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            if (count == 1)
            {
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();
                String query1 = "SELECT Top(1) Number FROM Rooms WHERE Hotel_ID=@Hotel_ID and Is_Reserved=1 and DATE<@DATE";
                SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection);
                sqlCommand1.CommandType = CommandType.Text;
                sqlCommand1.Parameters.AddWithValue("@Hotel_ID", Hotel_ID);
                sqlCommand1.Parameters.AddWithValue("@DATE", DateTime.Now);
                SqlDataReader sqlDataReader = sqlCommand1.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    RoomNumber = sqlDataReader.GetInt32(0);
                }
                sqlConnection.Close();
                if (sqlConnection.State == ConnectionState.Closed)
                    sqlConnection.Open();
                String query2 = "UPDATE Rooms SET Is_Reserved=0,DATE=NULL,DATEStart=NULL WHERE Number=@Number";
                SqlCommand sqlCommand2 = new SqlCommand(query2, sqlConnection);
                sqlCommand2.CommandType = CommandType.Text;
                sqlCommand2.Parameters.AddWithValue("@Number", RoomNumber);
                sqlCommand2.ExecuteNonQuery();
                sqlConnection.Close();
                Get_Set_Free_Rooms();
            }
            sqlConnection.Close();

        }

        private void RHotelCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RoomCB.Items.Clear();
            Get_Hotel_ID();
            Get_Set_Free_Rooms();
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            String query = "SELECT Number FROM Rooms WHERE Hotel_ID=@Hotel_ID and Is_Reserved=0 and not Is_Renovated=1";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Parameters.AddWithValue("@Hotel_ID", Hotel_ID);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                RoomCB.Items.Add(sqlDataReader["Number"]);
            }
            sqlConnection.Close();
        }

        private void RoomTxT_LostFocus(object sender, RoutedEventArgs e)
        {
            if(Int32.TryParse(RoomTxT.Text, out int RoomNumber) == false)
            {
                MessageBox.Show("Tylko cyfry.", "Błąd", MessageBoxButton.OK);
                RoomTxT.Clear();

            }
        }
    }
}
