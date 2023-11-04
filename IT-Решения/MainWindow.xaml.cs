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
                sqlExpression = $"SELECT post FROM [dbo].[entry] WHERE login = \'{Username}\'";
                command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
                answer = Convert.ToString(command.ExecuteScalar());
                answer = answer.Trim();
                if (answer == "user")
                {
                    userCurrentOrder.Visibility = Visibility.Visible;
                    userTop.Visibility = Visibility.Visible;
                    newOrder.Visibility = Visibility.Hidden;
                    userPreviousOrder.Visibility = Visibility.Hidden;
                    userProfile.Visibility = Visibility.Hidden;
                    login.Visibility = Visibility.Hidden;
                    UserFindCurrentOrder(sender, e);
                }
                else if (answer == "admin")
                {
                    adminCurrentOrder.Visibility = Visibility.Visible;
                    adminTop.Visibility = Visibility.Visible;
                    
                    adminPreviousOrder.Visibility = Visibility.Hidden;
                    adminProfile.Visibility = Visibility.Hidden;
                    login.Visibility = Visibility.Hidden;
                    AdminFindCurrentOrder(sender, e);
                }
                else if (answer == "worker")
                {
                    workerCurrentOrder.Visibility = Visibility.Visible;
                    workerTop.Visibility = Visibility.Visible;

                    workerPreviousOrder.Visibility = Visibility.Hidden;
                    workerProfile.Visibility = Visibility.Hidden;
                    login.Visibility = Visibility.Hidden;
                    WorkerFindCurrentOrder(sender, e);
                }
            }
            else
            {
                IncorrectLoginOrPassword.Visibility = Visibility.Visible;
            }
            connection.Close();
        }
        public void NewOrder(object sender, RoutedEventArgs e)
        {
            userCurrentOrder.Visibility = Visibility.Hidden;
            newOrder.Visibility = Visibility.Visible;
            userPreviousOrder.Visibility = Visibility.Hidden;
            userProfile.Visibility = Visibility.Hidden;
        }
        public void UserCurrentOrder(object sender, RoutedEventArgs e)
        {
            userCurrentOrder.Visibility = Visibility.Visible;
            newOrder.Visibility = Visibility.Hidden;
            userPreviousOrder.Visibility = Visibility.Hidden;
            userProfile.Visibility = Visibility.Hidden;
            UserFindCurrentOrder(sender, e);
        }
        public void UserPreviousOrder(object sender, RoutedEventArgs e)
        {
            userCurrentOrder.Visibility = Visibility.Hidden;
            newOrder.Visibility = Visibility.Hidden;
            userPreviousOrder.Visibility = Visibility.Visible;
            userProfile.Visibility = Visibility.Hidden;
            UserFindPreviousOrder(sender, e);
        }
        public void UserProfile(object sender, RoutedEventArgs e)
        {
            userCurrentOrder.Visibility = Visibility.Hidden;
            //newOrder.Visibility = Visibility.Hidden;
            userPreviousOrder.Visibility = Visibility.Hidden;
            userProfile.Visibility = Visibility.Visible;
        }
        public void AdminWorkers(object sender, RoutedEventArgs e)
        {
            ShowWorkers(sender, e);
            adminCurrentOrder.Visibility = Visibility.Hidden;
            Workers.Visibility = Visibility.Visible;
            adminPreviousOrder.Visibility = Visibility.Hidden;
            adminProfile.Visibility = Visibility.Hidden;
        }
        public void AdminCurrentOrder(object sender, RoutedEventArgs e)
        {
            adminCurrentOrder.Visibility = Visibility.Visible;
            Workers.Visibility = Visibility.Hidden;
            adminPreviousOrder.Visibility = Visibility.Hidden;
            adminProfile.Visibility = Visibility.Hidden;
            AdminFindCurrentOrder(sender, e);
        }
        public void AdminPreviousOrder(object sender, RoutedEventArgs e)
        {
            adminCurrentOrder.Visibility = Visibility.Hidden;
            Workers.Visibility = Visibility.Hidden;
            adminPreviousOrder.Visibility = Visibility.Visible;
            adminProfile.Visibility = Visibility.Hidden;
            AdminFindPreviousOrder(sender, e);
        }
        public void AdminProfile(object sender, RoutedEventArgs e)
        {
            adminCurrentOrder.Visibility = Visibility.Hidden;
            Workers.Visibility = Visibility.Hidden;
            adminPreviousOrder.Visibility = Visibility.Hidden;
            adminProfile.Visibility = Visibility.Visible;
        }

        public void WorkerCurrentOrder(object sender, RoutedEventArgs e)
        {
            workerCurrentOrder.Visibility = Visibility.Visible;
            //newOrder.Visibility = Visibility.Hidden;
            workerPreviousOrder.Visibility = Visibility.Hidden;
            workerProfile.Visibility = Visibility.Hidden;
            WorkerFindCurrentOrder(sender, e);
        }
        public void WorkerPreviousOrder(object sender, RoutedEventArgs e)
        {
            workerCurrentOrder.Visibility = Visibility.Hidden;
            //newOrder.Visibility = Visibility.Hidden;
            workerPreviousOrder.Visibility = Visibility.Visible;
            workerProfile.Visibility = Visibility.Hidden;
            WorkerFindPreviousOrder(sender, e);
        }
        public void WorkerProfile(object sender, RoutedEventArgs e)
        {
            workerCurrentOrder.Visibility = Visibility.Hidden;
            //newOrder.Visibility = Visibility.Hidden;
            workerPreviousOrder.Visibility = Visibility.Hidden;
            workerProfile.Visibility = Visibility.Visible;
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
            public TextBox workers;
            public string[] coloms;
        }
        private static tabel[] tabels;
        private List<TextBox> cells = new List<TextBox>();
        public void ShowOrder(Canvas canvas, SqlDataReader reader, SqlDataReader readerWorkers, int i, int m, List<TextBox> cells)
        {
            int n = 0;
            tabels[i].coloms = new string[m];
            for (int j = 0; j < tabels[i].coloms.Length; j++)
            {
                tabels[i].coloms[j] = Convert.ToString(reader.GetValue(j));
            }
            for (int j = 0; j < tabels[i].coloms.Length; j++)
            {
                tabels[i].coloms[j] = tabels[i].coloms[j].Trim();
            }
            tabels[i].id = new TextBox
            {
                Width = 40,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].id, 50);
            Canvas.SetTop(tabels[i].id, 150 + (i + 1) * 80);
            tabels[i].id.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].id);
            //cells[i * 7 + n] = tabels[i].id;
            canvas.Children.Add(tabels[i].id);
            n++;
            tabels[i].date = new TextBox
            {
                Width = 100,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].date, 90);
            Canvas.SetTop(tabels[i].date, 150 + (i + 1) * 80);
            tabels[i].date.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].date);
            //cells[i * 7 + n] = tabels[i].date;
            canvas.Children.Add(tabels[i].date);
            n++;
            tabels[i].equipment = new TextBox
            {
                Width = 192,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].equipment, 190);
            Canvas.SetTop(tabels[i].equipment, 150 + (i + 1) * 80);
            tabels[i].equipment.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].equipment);
            canvas.Children.Add(tabels[i].equipment);
            n++;
            tabels[i].problem = new TextBox
            {
                Width = 192,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].problem, 382);
            Canvas.SetTop(tabels[i].problem, 150 + (i + 1) * 80);
            tabels[i].problem.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].problem);
            canvas.Children.Add(tabels[i].problem);
            n++;
            tabels[i].description = new TextBox
            {
                Width = 192,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].description, 574);
            Canvas.SetTop(tabels[i].description, 150 + (i + 1) * 80);
            tabels[i].description.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].description);
            canvas.Children.Add(tabels[i].description);
            n++;
            tabels[i].author = new TextBox
            {
                Width = 192,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].author, 766);
            Canvas.SetTop(tabels[i].author, 150 + (i + 1) * 80);
            tabels[i].author.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].author);
            canvas.Children.Add(tabels[i].author);
            n++;
            tabels[i].status = new TextBox
            {
                Width = 150,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].status, 958);
            Canvas.SetTop(tabels[i].status, 150 + (i + 1) * 80);
            tabels[i].status.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].status);
            canvas.Children.Add(tabels[i].status);

            ShowWorkersInOrders(canvas, readerWorkers, i, m, cells);
        }
        public void ShowWorkersInOrders(Canvas canvas, SqlDataReader reader, int i, int m, List<TextBox> cells)
        {
            tabels[i].coloms[m - 1] = " ";
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tabels[i].coloms[m-1] = tabels[i].coloms[m - 1].Insert(tabels[i].coloms[m - 1].Length, $"{Convert.ToString(reader.GetValue(0))}, ");
                    tabels[i].coloms[m - 1] = tabels[i].coloms[m - 1].Trim();
                }
            }
            tabels[i].workers = new TextBox
            {
                Width = 142,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].workers, 1108);
            Canvas.SetTop(tabels[i].workers, 150 + (i + 1) * 80);
            tabels[i].workers.Text = tabels[i].coloms[m-1];
            cells.Add(tabels[i].workers);
            canvas.Children.Add(tabels[i].workers);
        }
        public void FindOrder(Canvas canvas, string slqExpressionWhere, string slqExpressionOrder, TextBlock NoOrder, List<TextBox> cells, TextBox textBox)
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
                sqlExpression = $"SELECT COUNT(*) FROM [dbo].[orders] WHERE (equipment LIKE \'%{search1.Text}%\' OR problem LIKE \'%{search1.Text}%\' OR description LIKE \'%{search1.Text}%\' OR author LIKE \'%{search1.Text}%\') AND {slqExpressionWhere}";
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
                    sqlExpression = $"SELECT * FROM [dbo].[orders] WHERE {slqExpressionWhere} {slqExpressionOrder}";
                }
                else
                {
                    //sqlExpression = $"SELECT * FROM [dbo].[orders] WHERE ({slqExpressionWhere}) AND (CONTAINS(equipment, \'{search1.Text}\') OR CONTAINS(problem, \'{search1.Text}\') OR CONTAINS(description, \'{search1.Text}\') OR CONTAINS(author, \'{search1.Text}\'))";
                    sqlExpression = $"SELECT * FROM [dbo].[orders] WHERE (equipment LIKE \'%{search1.Text}%\' OR problem LIKE \'%{search1.Text}%\' OR description LIKE \'%{search1.Text}%\' OR author LIKE \'%{search1.Text}%\') AND {slqExpressionWhere} {slqExpressionOrder}";
                }

                command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                SqlDataReader tempReader;
                int i = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sqlExpression = $"SELECT name, surname FROM [dbo].[workers] WHERE orderId = {Convert.ToString(reader.GetValue(0))}";
                        command = new SqlCommand(sqlExpression, connection);
                        tempReader = command.ExecuteReader();
                        ShowOrder(canvas, reader, tempReader, i, 7, cells);
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
        public void UserFindCurrentOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(userCurrentOrder, "(status = \'в работе\' OR status = \'в ожидании\')", "", userNoCurrentOrders, cells, search1);
        }
        public void UserFindPreviousOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(userPreviousOrder, "(status = \'выполнено\')", "", userNoPreviousOrders, cells, search2);
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
        public struct WorkerTabel
        {
            public TextBox id;
            public TextBox name;
            public TextBox surname;
            public TextBox status;
            public TextBox profession;
            public TextBox projectId;
            public string[] coloms;
        }
        private static WorkerTabel[] WorkerTabels;
        public void ShowWorker(Canvas canvas, SqlDataReader reader, int i, int m, List<TextBox> cells)
        {
            int n = 0;
            WorkerTabels[i].coloms = new string[m];
            for (int j = 0; j < WorkerTabels[i].coloms.Length; j++)
            {
                WorkerTabels[i].coloms[j] = Convert.ToString(reader.GetValue(j));
            }
            for (int j = 0; j < WorkerTabels[i].coloms.Length; j++)
            {
                WorkerTabels[i].coloms[j] = WorkerTabels[i].coloms[j].Trim();
            }
            WorkerTabels[i].id = new TextBox
            {
                Width = 100,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(WorkerTabels[i].id, 100);
            Canvas.SetTop(WorkerTabels[i].id, 150 + (i + 1) * 80);
            WorkerTabels[i].id.Text = WorkerTabels[i].coloms[n];
            cells.Add(WorkerTabels[i].id);
            canvas.Children.Add(WorkerTabels[i].id);
            n++;
            WorkerTabels[i].name = new TextBox
            {
                Width = 250,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(WorkerTabels[i].name, 200);
            Canvas.SetTop(WorkerTabels[i].name, 150 + (i + 1) * 80);
            WorkerTabels[i].name.Text = WorkerTabels[i].coloms[n];
            cells.Add(WorkerTabels[i].name);
            canvas.Children.Add(WorkerTabels[i].name);
            n++;
            WorkerTabels[i].surname = new TextBox
            {
                Width = 250,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(WorkerTabels[i].surname, 450);
            Canvas.SetTop(WorkerTabels[i].surname, 150 + (i + 1) * 80);
            WorkerTabels[i].surname.Text = WorkerTabels[i].coloms[n];
            cells.Add(WorkerTabels[i].surname);
            canvas.Children.Add(WorkerTabels[i].surname);
            n++;
            WorkerTabels[i].status = new TextBox
            {
                Width = 150,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(WorkerTabels[i].status, 1050);
            Canvas.SetTop(WorkerTabels[i].status, 150 + (i + 1) * 80);
            WorkerTabels[i].status.Text = WorkerTabels[i].coloms[n];
            cells.Add(WorkerTabels[i].status);
            canvas.Children.Add(WorkerTabels[i].status);
            n++;
            WorkerTabels[i].profession = new TextBox
            {
                Width = 250,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(WorkerTabels[i].profession, 700);
            Canvas.SetTop(WorkerTabels[i].profession, 150 + (i + 1) * 80);
            WorkerTabels[i].profession.Text = WorkerTabels[i].coloms[n];
            cells.Add(WorkerTabels[i].profession);
            canvas.Children.Add(WorkerTabels[i].profession);
            n++;
            WorkerTabels[i].projectId = new TextBox
            {
                Width = 100,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(WorkerTabels[i].projectId, 950);
            Canvas.SetTop(WorkerTabels[i].projectId, 150 + (i + 1) * 80);
            WorkerTabels[i].projectId.Text = WorkerTabels[i].coloms[n];
            cells.Add(WorkerTabels[i].projectId);
            canvas.Children.Add(WorkerTabels[i].projectId);
        }
        public void FindWorkers(Canvas thisCanvas, Canvas canvas, List<TextBox> cells, TextBox textBox)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sqlExpression = "";
            if (textBox.Text == "")
            {
                sqlExpression = $"SELECT COUNT(*) FROM [dbo].[workers]";
            }
            else
            {
                sqlExpression = $"SELECT COUNT(*) FROM [dbo].[workers] WHERE (status LIKE \'%{search7.Text}%\' OR profession LIKE \'%{search7.Text}%\' OR projectId LIKE \'%{search7.Text}%\' OR name LIKE \'%{search7.Text}%\' OR surname LIKE \'%{search7.Text}%\' OR projectId LIKE \'%{search7.Text}%\')";
            }
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.ExecuteNonQuery();
            int count = (int)command.ExecuteScalar();
            if (cells != null)
            {
                foreach (TextBox text in cells)//удаление старой
                {
                    canvas.Children.Remove(text);
                }
            }
            if (count > 0)
            {
                NoWorkers.Visibility = Visibility.Hidden;
                WorkerTabels = new WorkerTabel[count];
                if (textBox.Text == "")
                {
                    sqlExpression = $"SELECT * FROM [dbo].[workers]";
                }
                else
                {
                    sqlExpression = $"SELECT * FROM [dbo].[workers] WHERE (status LIKE \'%{search7.Text}%\' OR profession LIKE \'%{search7.Text}%\' OR projectId LIKE \'%{search7.Text}%\' OR name LIKE \'%{search7.Text}%\' OR surname LIKE \'%{search7.Text}%\' OR projectId LIKE \'%{search7.Text}%\')";
                }
                command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                int i = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ShowWorker(thisCanvas, reader, i, 6, cells);
                        i++;
                    }
                }
            }
            else
            {
                NoWorkers.Visibility = Visibility.Visible;
            }
            connection.Close();
        }
        public void ShowWorkers(object sender, RoutedEventArgs e)
        {
            if(adminCurrentOrder.Visibility == Visibility.Visible)
            {
                FindWorkers(Workers, adminCurrentOrder, cells, search7);
            }
            else if(adminPreviousOrder.Visibility == Visibility.Visible)
            {
                FindWorkers(Workers, adminPreviousOrder, cells, search7);
            }
        }
        public void AdminFindCurrentOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(adminCurrentOrder, "(status = \'в работе\' OR status = \'в ожидании\')", "ORDER BY status ASC", adminNoCurrentOrders, cells, search3);
        }
        public void AdminFindPreviousOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(adminPreviousOrder, "(status = \'выполнено\')", "", adminNoPreviousOrders, cells, search4);
        }

        public void WorkerFindCurrentOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(workerCurrentOrder, "status = \'в работе\' OR status = \'в ожидании\'", "", workerNoCurrentOrders, cells, search5);
        }
        public void WorkerFindPreviousOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(workerPreviousOrder, "status = \'выполнено\'", "", workerNoPreviousOrders, cells, search6);
        }
    }
}
