using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logika interakcji dla klasy AddReservation.xaml
    /// </summary>
    public partial class AddReservation : Window
    {

        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-HBHGS1M\SQLEXPRESS; Initial Catalog=HotelMenagment; Integrated Security =true;");
        public string finalString;

        public int Customer_ID;
        private int Hotel_ID;
        private int Room_ID;
        private int RoomNumber;

        public AddReservation()
        {
            
            InitializeComponent();
          
            
        }


        public void Get_Res_ID()
        {
  
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[8];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                finalString = new String(stringChars);

            Check_Res_ID();
        }

        public void Check_Res_ID()
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            String query = "SELECT COUNT(1) FROM ReservationsID WHERE Res_Number=@ResNumber";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Parameters.AddWithValue("@ResNumber", finalString);
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            if (count == 1)
            {
                Get_Res_ID();
            }
        }
       

    private void ResButton_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                int count2 = EmailText.Text.Length;
                int count = SurnameText.Text.Length;
                int count1 = NumberTelText.Text.Length;
                int room = RoomCB.SelectedIndex + 1;
                if (count == 0 || count1 == 0 || count2 == 0)
                {
                    MessageBox.Show("Musisz uzupełnić wpszystkie pola.", "Błąd", MessageBoxButton.OK);
                }
                else if (Regex.IsMatch(EmailText.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") != true)
                {
                    MessageBox.Show("Podaj poprawny adres email.", "Błąd", MessageBoxButton.OK);
                }
                else if (HotelCB.SelectedIndex < 0 || RoomCB.SelectedIndex < 0)
                {
                    MessageBox.Show("Wybierz hotel i numer pokoju, który chcesz zarezerwować.", "Błąd", MessageBoxButton.OK);
                }
                else if (StartData.SelectedDate < DateTime.Now)
                {
                    MessageBox.Show("Nie można wybrać daty mniejszej niż dzień dzisiejszy.", "Błąd", MessageBoxButton.OK);
                }
                else if (EndData.SelectedDate < StartData.SelectedDate)
                {
                    MessageBox.Show("Wybrana data końca pobytu wypada wcześniej niż jego początek.", "Błąd", MessageBoxButton.OK);
                }
                else if (EndData.SelectedDate == null || EndData.SelectedDate == null)
                {
                    MessageBox.Show("Wybierz jakąś datę.", "Błąd", MessageBoxButton.OK);
                }
                else if (Int32.TryParse(NumberTelText.Text, out int TeleNumber))
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    String query = "Insert into Customer (Surname, Email, Telefon_Number) values ('" + this.SurnameText.Text + " ', ' " + this.EmailText.Text + "' ,'" + TeleNumber + "')";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                    Get_Res_ID();
                    Get_Customer_ID();
                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    String query2 = "UPDATE Rooms SET Is_Reserved=0, DATE=@DataEND, DATEStart=@DataStart WHERE Number=@Number";
                    SqlCommand sqlCommand2 = new SqlCommand(query2, sqlConnection);
                    sqlCommand2.CommandType = CommandType.Text;
                    sqlCommand2.Parameters.AddWithValue("@DataEND", EndData.SelectedDate);
                    sqlCommand2.Parameters.AddWithValue("@DataStart", StartData.SelectedDate);
                    sqlCommand2.Parameters.AddWithValue("@Number", RoomCB.Text);
                    sqlCommand2.ExecuteNonQuery();
                    Get_Room_ID();
                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    String query1 = "Insert into ReservationsID (Res_Number,Customer_ID,Room_ID) values ('" + finalString + "','" + Customer_ID + "' , '" + Room_ID + "')";
                    SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection);
                    sqlCommand1.ExecuteNonQuery();
                    sqlConnection.Close();
                    MessageBox.Show("Dodano Rezerwacje. Twój numer rezerwacji: " + finalString + ". Prosimy o zapisanie numeru, służy on do identyfikacji", "Błąd", MessageBoxButton.OK);
                }
                else MessageBox.Show("Podano błędny numer telefonu.", "Błąd", MessageBoxButton.OK);
                            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            
        
        }

        private void Get_Room_ID()
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            string query = "Select ID From Rooms WHERE Number=@Number";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Parameters.AddWithValue("@Number", RoomCB.SelectedValue);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Room_ID = sqlDataReader.GetInt32(0);
            }
            sqlConnection.Close();
        }

        private void Get_Customer_ID()
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            string query = "Select ID From Customer WHERE Surname=@Surname";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Parameters.AddWithValue("@Surname", SurnameText.Text);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                Customer_ID = sqlDataReader.GetInt32(0);
            }
            sqlConnection.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

    

        private void HotelCB_Initialized(object sender, EventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            HotelCB.Items.Clear();
            String query = "SELECT NAME FROM Hotels";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                HotelCB.Items.Add(sqlDataReader["NAME"]);
            }
            sqlConnection.Close();
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
        private void HotelCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void HotelCB_DropDownClosed(object sender, EventArgs e)
        {
            if (HotelCB.SelectedIndex < 0)
            {
                MessageBox.Show("Nie wybrano hotelu.", "Błąd", MessageBoxButton.OK);
                
            }
        }

        private void RoomCB_DropDownClosed(object sender, EventArgs e)
        {
            if (RoomCB.SelectedIndex < 0)
            {
                MessageBox.Show("Nie wybrano pokoju.", "Błąd", MessageBoxButton.OK);
            }
        }
    }
}
