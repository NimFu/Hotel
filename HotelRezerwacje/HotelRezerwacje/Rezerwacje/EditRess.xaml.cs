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
    /// Logika interakcji dla klasy EditRess.xaml
    /// </summary>
    public partial class EditRess : Window
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-HBHGS1M\SQLEXPRESS; Initial Catalog=HotelMenagment; Integrated Security =true;");
        public EditRess()
        {
            InitializeComponent();
        }

        private void DataButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (ResNumberTxt.Text.Length == 0)
            {
                MessageBox.Show("Nie podano numeru rejestracji.", "Błąd", MessageBoxButton.OK);
            }
            else if(StartData.SelectedDate < DateTime.Now)
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
            else
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz zmienić date pobytu ?.", "Potwierdzenie", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();
                        String query1 = "SELECT COUNT(1) FROM ReservationsID WHERE Res_Number=@ResNumber";
                        SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection);
                        sqlCommand1.CommandType = CommandType.Text;
                        sqlCommand1.Parameters.AddWithValue("@ResNumber", ResNumberTxt.Text);
                        int count = Convert.ToInt32(sqlCommand1.ExecuteScalar());
                        if (count == 1)
                        {
                            String query = "UPDATE Rooms SET Is_Reserved=0, DATE=@DataEND, DATEStart=@DataStart FROM Rooms INNER JOIN ReservationsID on Rooms.ID=ReservationsID.Room_ID WHERE ReservationsID.Res_Number=@ResNumber";
                            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                            sqlCommand.CommandType = CommandType.Text;
                            sqlCommand.Parameters.AddWithValue("@DataEND", EndData.SelectedDate);
                            sqlCommand.Parameters.AddWithValue("@DataStart", StartData.SelectedDate);
                            sqlCommand.Parameters.AddWithValue("@ResNumber", ResNumberTxt.Text);
                            sqlCommand.ExecuteNonQuery();
                            MessageBox.Show("Zmieniono date pobytu.", "Adnotacja", MessageBoxButton.OK);
                            sqlConnection.Close();
                        }
                        if (count == 0)
                        {
                            MessageBox.Show("Podaj poprawny numer rezerwacji", "Błąd", MessageBoxButton.OK);
                            sqlConnection.Close();
                        }
                        break;
                    case MessageBoxResult.No:
                        MessageBox.Show("Anulowano operacje.", "Adnotacja", MessageBoxButton.OK);
                        break;
                }
            }
        }

        //POprawić data
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResNumberTxt.Text.Length == 0)
            {
                MessageBox.Show("Nie podano numeru rejestracji.", "Błąd", MessageBoxButton.OK);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz zrezygnować z rezerwacji ?.", "Potwierdzenie", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                       if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();
                        String query1 = "Update Rooms SET Is_Reserved=0, DATE = NULL, DATEStart=NULL From Rooms inner join ReservationsID on Rooms.ID=ReservationsID.Room_ID WHERE Res_Number=@ResNumber";
                        SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection);
                        sqlCommand1.CommandType = CommandType.Text;
                        sqlCommand1.Parameters.AddWithValue("@ResNumber", ResNumberTxt.Text);
                        sqlCommand1.ExecuteNonQuery();
                        String query = "Delete from ReservationsID WHERE Res_Number=@ResNumber";
                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.Parameters.AddWithValue("@ResNumber", ResNumberTxt.Text);
                        sqlCommand.ExecuteNonQuery();
                        MessageBox.Show("Anulowano rezerwacje.", "Adnotacja", MessageBoxButton.OK);
                        sqlConnection.Close();
                        break;
                    case MessageBoxResult.No:
                        MessageBox.Show("Anulowano operacje.", "Adnotacja", MessageBoxButton.OK);
                        break;
                }

            }
        }

    
        private void ResButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow hotelManagement = new MainWindow();
            hotelManagement.Show();
            this.Close();
        }
    }
}
