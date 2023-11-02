﻿using System;
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
using System.Runtime.Remoting.Messaging;

namespace IT_Решения
{
    public partial class MainWindow : Window
    {
        string connectionString = @"Server=26.97.227.1, 1433; Database=SP02; Uid=sa; Pwd=123qwe;";// 192.168.0.166  26.97.227.1  Trusted_Connection=True; TrustServerCertificate=True; 
        public MainWindow()
        {
            InitializeComponent();
        }
        public void Login(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string Username = username.Text;
            string sqlExpression = $"SELECT password FROM [dbo].[entry] WHERE login = \'{Username}\'";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.ExecuteNonQuery();
            string answer = Convert.ToString(command.ExecuteScalar());
            answer = answer.Trim();
            if (answer == $"{password.Text}")
            {
                currentOrder.Visibility = Visibility.Visible;
                Top.Visibility = Visibility.Visible;
                newOrder.Visibility = Visibility.Hidden;
                previousOrder.Visibility = Visibility.Hidden;
                profile.Visibility = Visibility.Hidden;
                login.Visibility = Visibility.Hidden;
                FindCurrentOrder(sender, e);
            }
            else
            {
                IncorrectLoginOrPassword.Visibility = Visibility.Visible;
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
            FindCurrentOrder(sender, e);
        }
        public void PreviousOrder(object sender, RoutedEventArgs e)
        {
            currentOrder.Visibility = Visibility.Hidden;
            newOrder.Visibility = Visibility.Hidden;
            previousOrder.Visibility = Visibility.Visible;
            profile.Visibility = Visibility.Hidden;
            FindPreviousOrder(sender, e);
        }
        public void Profile(object sender, RoutedEventArgs e)
        {
            currentOrder.Visibility = Visibility.Hidden;
            newOrder.Visibility = Visibility.Hidden;
            previousOrder.Visibility = Visibility.Hidden;
            profile.Visibility = Visibility.Visible;
        }
        public struct tabel
        {
            public TextBox id;
            public TextBox date;
            public TextBox equipment;
            public TextBox problem;
            public TextBox description;
            public TextBox author;
            public TextBox status;
            public string[] coloms;
        }
        private static tabel[] tabels;
        private List<TextBox> cells = new List<TextBox>();
        public void ShowOrder(Canvas canvas, SqlDataReader reader, int i, List<TextBox> cells)
        {
            int n = 0;
            tabels[i].coloms = new string[7];
            for (int j = 0; j < tabels[i].coloms.Length; j++)
            {
                tabels[i].coloms[j] = Convert.ToString(reader.GetValue(j));
            }
            for (int j = 0; j < tabels[i].coloms.Length; j++)
            {
                tabels[i].coloms[j] = tabels[i].coloms[j].Trim();
            }
            tabels[i].id = new TextBox();
            tabels[i].id.Width = 40;
            tabels[i].id.Height = 80;
            tabels[i].id.FontSize = 20;
            Canvas.SetLeft(tabels[i].id, 50);
            Canvas.SetTop(tabels[i].id, 150 + (i + 1) * 80);
            tabels[i].id.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].id);
            //cells[i * 7 + n] = tabels[i].id;
            canvas.Children.Add(tabels[i].id);
            n++;
            tabels[i].date = new TextBox();
            tabels[i].date.Width = 100;
            tabels[i].date.Height = 80;
            tabels[i].date.FontSize = 20;
            Canvas.SetLeft(tabels[i].date, 90);
            Canvas.SetTop(tabels[i].date, 150 + (i + 1) * 80);
            tabels[i].date.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].date);
            //cells[i * 7 + n] = tabels[i].date;
            canvas.Children.Add(tabels[i].date);
            n++;
            tabels[i].equipment = new TextBox();
            tabels[i].equipment.Width = 240;
            tabels[i].equipment.Height = 80;
            tabels[i].equipment.FontSize = 20;
            Canvas.SetLeft(tabels[i].equipment, 190);
            Canvas.SetTop(tabels[i].equipment, 150 + (i + 1) * 80);
            tabels[i].equipment.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].equipment);
            canvas.Children.Add(tabels[i].equipment);
            n++;
            tabels[i].problem = new TextBox();
            tabels[i].problem.Width = 240;
            tabels[i].problem.Height = 80;
            tabels[i].problem.FontSize = 20;
            Canvas.SetLeft(tabels[i].problem, 430);
            Canvas.SetTop(tabels[i].problem, 150 + (i + 1) * 80);
            tabels[i].problem.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].problem);
            canvas.Children.Add(tabels[i].problem);
            n++;
            tabels[i].description = new TextBox();
            tabels[i].description.Width = 240;
            tabels[i].description.Height = 80;
            tabels[i].description.FontSize = 20;
            Canvas.SetLeft(tabels[i].description, 670);
            Canvas.SetTop(tabels[i].description, 150 + (i + 1) * 80);
            tabels[i].description.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].description);
            canvas.Children.Add(tabels[i].description);
            n++;
            tabels[i].author = new TextBox();
            tabels[i].author.Width = 240;
            tabels[i].author.Height = 80;
            tabels[i].author.FontSize = 20;
            Canvas.SetLeft(tabels[i].author, 910);
            Canvas.SetTop(tabels[i].author, 150 + (i + 1) * 80);
            tabels[i].author.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].author);
            canvas.Children.Add(tabels[i].author);
            n++;
            tabels[i].status = new TextBox();
            tabels[i].status.Width = 150;
            tabels[i].status.Height = 80;
            tabels[i].status.FontSize = 20;
            Canvas.SetLeft(tabels[i].status, 1150);
            Canvas.SetTop(tabels[i].status, 150 + (i + 1) * 80);
            tabels[i].status.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].status);
            canvas.Children.Add(tabels[i].status);
        }
        public void FindOrder(Canvas canvas, string slqExpressionWhere, TextBlock NoOrder, List<TextBox> cells, TextBox textBox)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sqlExpression = "";
            if (textBox.Text == "")
            {
                sqlExpression = $"SELECT COUNT(*) FROM [dbo].[orders] WHERE {slqExpressionWhere}";
            }
            else
            {
                //sqlExpression = $"SELECT COUNT(*) FROM [dbo].[orders] WHERE ({slqExpressionWhere}) AND (CONTAINS(equipment, \'{search1.Text}\') OR CONTAINS(problem, \'{search1.Text}\') OR CONTAINS(description, \'{search1.Text}\') OR CONTAINS(author, \'{search1.Text}\'))";
                sqlExpression = $"SELECT COUNT(*) FROM [dbo].[orders] WHERE ({slqExpressionWhere}) AND (equipment LIKE \'%{search1.Text}%\' OR problem LIKE \'%{search1.Text}%\' OR description LIKE \'%{search1.Text}%\' OR author LIKE \'%{search1.Text}%\')";
            }
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.ExecuteNonQuery();
            int count = (int)command.ExecuteScalar();
            if (cells != null)
            {
                foreach(TextBox text in cells)//удаление старой
                {
                    canvas.Children.Remove(text);
                }
                //cells.Clear();
            }
            if(count > 0)
            {
                //cells = new TextBox[count * 7];
                NoOrder.Visibility = Visibility.Hidden;
                tabels = new tabel[count];

                if (textBox.Text == "")
                {
                    sqlExpression = $"SELECT * FROM [dbo].[orders] WHERE {slqExpressionWhere}";
                }
                else
                {
                    //sqlExpression = $"SELECT * FROM [dbo].[orders] WHERE ({slqExpressionWhere}) AND (CONTAINS(equipment, \'{search1.Text}\') OR CONTAINS(problem, \'{search1.Text}\') OR CONTAINS(description, \'{search1.Text}\') OR CONTAINS(author, \'{search1.Text}\'))";
                    sqlExpression = $"SELECT * FROM [dbo].[orders] WHERE ({slqExpressionWhere}) AND (equipment LIKE \'%{search1.Text}%\' OR problem LIKE \'%{search1.Text}%\' OR description LIKE \'%{search1.Text}%\' OR author LIKE \'%{search1.Text}%\')";
                }

                command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                int i = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ShowOrder(canvas, reader, i, cells);
                        i++;
                    }
                }
            }
            else
            {
                NoOrder.Visibility = Visibility.Visible;
            }
            //Convert.ToString(command.ExecuteScalar());
            //SqlDataAdapter dataAdp = new SqlDataAdapter(command);
            //DataTable dt = new DataTable("Orders");
            //dataAdp.Fill(dt);
            //currentOrderData.ItemsSource = dt.DefaultView;
            connection.Close();
        }
        public void FindCurrentOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(currentOrder, "status = \'в работе\' OR status = \'в ожидании\'", NoCurrentOrders, cells, search1);
        }
        public void FindPreviousOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(previousOrder, "status = \'выполнено\'", NoPreviousOrders, cells, search2);
        }
        public void AddNewOrder(object sender, RoutedEventArgs e)
        {
            OrderAdded.Visibility = Visibility.Hidden;
            if (equipmentName.Text == "")
            {
                warning1.Visibility = Visibility.Visible;
            }
            else
            {
                warning1.Visibility = Visibility.Hidden;
            }
            if (problemName.Text == "")
            {
                warning2.Visibility = Visibility.Visible;
            }
            else
            {
                warning2.Visibility = Visibility.Hidden;
            }
            if (descriptionName.Text == "")
            {
                warning3.Visibility = Visibility.Visible;
            }
            else
            {
                warning3.Visibility = Visibility.Hidden;
            }
            if(equipmentName.Text != "" && problemName.Text != "" && descriptionName.Text != "")
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                DateTime dateTime = DateTime.Now;
                string sqlExpression = $"INSERT INTO [dbo].[orders] VALUES (\'{dateTime.Year}-{dateTime.Month}-{dateTime.Day}\', \'{equipmentName.Text}\',\'{problemName.Text}\',\'{descriptionName.Text}\',\'{username.Text}\', \'в ожидании\')";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
                connection.Close();
                OrderAdded.Visibility = Visibility.Visible;
            }
        }
    }
}
