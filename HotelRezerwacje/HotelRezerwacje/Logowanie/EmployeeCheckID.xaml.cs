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
    /// Logika interakcji dla klasy EmployeeCheckID.xaml
    /// </summary>
    public partial class EmployeeCheckID : Window
    {
        public EmployeeCheckID()
        {
            InitializeComponent();
        }


        private void CancleButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IdText.Text.Length < 9)
            {
                MessageBox.Show("Nie wpisano żadnego znaku, minimalnie 9", "Błąd", MessageBoxButton.OK);
            }
            else if (IdText.Text.Length > 30)
            {
                MessageBox.Show("Podano za dużo znaków, maksymalnie 30", "Błąd", MessageBoxButton.OK);
            }
            else
            {
                SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-HBHGS1M\SQLEXPRESS; Initial Catalog=HotelMenagment; Integrated Security =true;");
                try
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    String query = "SELECT COUNT(1) FROM Employee WHERE EmployeeID=@EmployeeID";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@EmployeeID", IdText.Text);
                    int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    if (count == 1)
                    {
                        ResCheckId resCheckId = new ResCheckId();
                        resCheckId.Show();
                        this.Close();
                    }
                    if (count == 0)
                    {
                        MessageBox.Show("Podaj poprawne dane", "Błąd", MessageBoxButton.OK);
                    }
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
        }
    }
}
