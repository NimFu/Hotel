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
    /// Logika interakcji dla klasy HotelManagementHotels.xaml
    /// </summary>
    public partial class HotelManagementHotels : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-HBHGS1M\SQLEXPRESS; Initial Catalog=HotelMenagment; Integrated Security =true;");
        public HotelManagementHotels()
        {
            InitializeComponent();
        }

        private void RoomsButton_Click(object sender, RoutedEventArgs e)
        {
            HotelManagmentRooms hotelManagmentRooms = new HotelManagmentRooms();
            hotelManagmentRooms.Show();
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
            if (HotelTxt.Text.Length > 0)
            {
               MessageBoxResult result = MessageBox.Show("Czy na pewno dodać hotel ?.", "Potwierdzenie", MessageBoxButton.YesNo);
                switch(result)
                {
                    case MessageBoxResult.Yes:
                    String query = "Insert into Hotels (NAME) values ('" + this.HotelTxt.Text + "')";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                        MessageBox.Show("Dodano hotel.", "Adnotacja", MessageBoxButton.OK);
                        HotelCB.Items.Clear();
                        String query1 = "SELECT NAME FROM Hotels";
                        SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection);
                        SqlDataReader sqlDataReader1 = sqlCommand1.ExecuteReader();
                        while (sqlDataReader1.Read())
                        {
                            HotelCB.Items.Add(sqlDataReader1["NAME"]);
                        }
                        sqlConnection.Close();
                        break;
                    case MessageBoxResult.No:
                        MessageBox.Show("Anulowano dodawanie.", "Adnotacja", MessageBoxButton.OK);
                        HotelTxt.Clear();
                        break;
                }
            }
            else MessageBox.Show("Podaj nazwe hotelu.", "Błąd", MessageBoxButton.OK);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string hotel = HotelCB.Text;
            if (HotelCB.SelectedIndex > 0)
            {
                MessageBoxResult result = MessageBox.Show("Usuniesz hotel i powiązanie pokoje. Napewno chesz to zrobić ?.", "Potwierdzenie", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();
                        String query2 = "Delete from Rooms WHERE Hotel_ID =ANY(SELECT ID FROM Hotels WHERE NAME=@NAME)";
                        String query = "Delete from Hotels WHERE NAME=@NAME";
                        SqlCommand sqlCommand2 = new SqlCommand(query2, sqlConnection);
                        sqlCommand2.CommandType = CommandType.Text;
                        sqlCommand2.Parameters.AddWithValue("@NAME", hotel);
                        sqlCommand2.ExecuteNonQuery();
                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.Parameters.AddWithValue("@NAME", hotel);
                        sqlCommand.ExecuteNonQuery();
                        MessageBox.Show("Usunięto hotel i powiązane do niego pokoje.", "Adnotacja", MessageBoxButton.OK);
                        HotelCB.Items.Clear();
                        String query1 = "SELECT NAME FROM Hotels";
                        SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection);
                        SqlDataReader sqlDataReader1 = sqlCommand1.ExecuteReader();
                        while (sqlDataReader1.Read())
                        {
                            HotelCB.Items.Add(sqlDataReader1["NAME"]);
                        }
                        sqlConnection.Close();
                        break;
                    case MessageBoxResult.No:
                        MessageBox.Show("Anulowano usuwanie.", "Adnotacja", MessageBoxButton.OK);
                        break;
                }
            }
            else MessageBox.Show("Wybierz hotel do usunięcia.", "Błąd", MessageBoxButton.OK);
        }
    }
}
