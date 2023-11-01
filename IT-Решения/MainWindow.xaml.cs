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
using System.Data.SqlClient;
using System.Data;

namespace IT_Решения
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString = @"Server=26.97.227.1, 1433; Database=SP02; Uid=sa; Pwd=123qwe;";// 192.168.0.166  26.97.227.1  Trusted_Connection=True; TrustServerCertificate=True; 
        public MainWindow()
        {
            InitializeComponent();
            CloseAll();
        }
        public void Login(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string Username = username.Text;
            string sqlExpression = $"SELECT password FROM [dbo].[entry] WHERE login = \'{Username}\'";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            //SqlDataReader reader = command.ExecuteReader();
            command.ExecuteNonQuery();
            string answer = Convert.ToString(command.ExecuteScalar());
            answer = answer.Trim();
            if (answer == $"{password.Text}")
            {
                login.Opacity = 0;
                Top.Opacity = 1;
                currentOrder.Opacity = 1;
            }
            connection.Close();
        }
        public void CloseAll()
        {
            Top.Opacity = 0;
            currentOrder.Opacity = 0;
            newOrder.Opacity = 0;
            previousOrder.Opacity = 0;
            profile.Opacity = 0;

        }
        public void NewOrder(object sender, RoutedEventArgs e)
        {
            currentOrder.Visibility = Visibility.Hidden;
            newOrder.Visibility = Visibility.Visible;
            previousOrder.Visibility = Visibility.Hidden;
            profile.Visibility = Visibility.Hidden;
        }
        public void CurrentOrder(object sender, RoutedEventArgs e)
        {
            currentOrder.Visibility = Visibility.Visible;
            newOrder.Visibility = Visibility.Hidden; 
            previousOrder.Visibility = Visibility.Hidden;
            profile.Visibility = Visibility.Hidden;
        }
        public void PreviousOrder(object sender, RoutedEventArgs e)
        {
            currentOrder.Visibility = Visibility.Hidden;
            newOrder.Visibility = Visibility.Hidden;
            previousOrder.Visibility = Visibility.Visible;
            profile.Visibility = Visibility.Hidden;
        }
        public void Profile(object sender, RoutedEventArgs e)
        {
            currentOrder.Visibility = Visibility.Hidden;
            newOrder.Visibility = Visibility.Hidden;
            previousOrder.Visibility = Visibility.Hidden;
            profile.Visibility = Visibility.Visible;
        }
        public void FindCurrentOrder(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            //string sqlExpression = $"SELECT password FROM [dbo].[entry] WHERE login = \'{Username}\'";
            string sqlExpression = $"UPDATE [dbo].[orders] SET id = 1, date = 2023/09/16, equipment = \'bake\', problem = \'broken\', description = \'bake dont bake\', author = \'Bebrovich\', status = \'in work\'";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.ExecuteNonQuery();
            SqlDataAdapter dataAdp = new SqlDataAdapter(command);
            DataTable dt = new DataTable("Students");
            dataAdp.Fill(dt);
            currentOrderData.ItemsSource = dt.DefaultView;
            connection.Close();
        }
        public void FindPreviousOrder(object sender, RoutedEventArgs e)
        {   

        }
        public void clearkarps(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sqlExpressionkarpclear = $"TRUNCATE TABLE [dbo].[karp_pos]";
            SqlCommand karpclear = new SqlCommand(sqlExpressionkarpclear, connection);
            karpclear.ExecuteNonQuery();
            connection.Close();
        }
        private void Init(object sender, RoutedEventArgs e)//вода камни рыбы
        {
            login.Opacity = 0;
            currentOrder.Opacity = 1;
        }
        private void Init1(object sender, RoutedEventArgs e)//вода камни рыбы
        {
            login.Opacity = 1;
            currentOrder.Opacity = 0;
        }
    }
}
