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
using System.Windows.Media.Animation;
using System.Windows.Markup;
using System.Windows.Media.Media3D;
using System.IO;

namespace IT_Решения
{
    public partial class MainWindow : Window
    {
        string connectionString = @"Server=26.97.227.1, 1433; Database=SP02; Uid=sa; Pwd=123qwe;";// 192.168.0.166  26.97.227.1  Trusted_Connection=True; TrustServerCertificate=True;  25
        public MainWindow()
        {
            InitializeComponent();
        }
        int thisId;
        public void Login(object sender, RoutedEventArgs e)//вход проверка логин пароль
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
                    Comments.Visibility = Visibility.Hidden;
                    login.Visibility = Visibility.Hidden;
                    UserFindCurrentOrder(sender, e);
                }
                else if (answer == "admin")
                {
                    adminCurrentOrder.Visibility = Visibility.Visible;
                    adminTop.Visibility = Visibility.Visible;                   
                    adminPreviousOrder.Visibility = Visibility.Hidden;
                    Stats.Visibility = Visibility.Hidden;
                    login.Visibility = Visibility.Hidden;
                    AdminFindCurrentOrder(sender, e);
                }
                else if (answer == "worker")
                {
                    workerCurrentOrder.Visibility = Visibility.Visible;
                    workerTop.Visibility = Visibility.Visible;
                    workerPreviousOrder.Visibility = Visibility.Hidden;
                    Comments.Visibility = Visibility.Hidden;
                    login.Visibility = Visibility.Hidden;
                    WorkerFindCurrentOrder(sender, e);
                }
                sqlExpression = $"SELECT id FROM [dbo].[entry] WHERE login = \'{Username}\'";
                command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
                answer = Convert.ToString(command.ExecuteScalar());
                answer = answer.Trim();
                thisId = Int32.Parse(answer);
            }
            else
            {
                IncorrectLoginOrPassword.Visibility = Visibility.Visible;
            }
            connection.Close();
        }
        public void Exit(object sender, EventArgs e)
        {
            exit.Visibility = Visibility.Visible;
        }
        public void ConfirmExit(object sender, EventArgs e)
        {
            login.Visibility = Visibility.Visible;
            exit.Visibility = Visibility.Hidden;
            userCurrentOrder.Visibility = Visibility.Hidden;
            userTop.Visibility = Visibility.Hidden;
            newOrder.Visibility = Visibility.Hidden;
            userPreviousOrder.Visibility = Visibility.Hidden;
            adminCurrentOrder.Visibility = Visibility.Hidden;
            adminTop.Visibility = Visibility.Hidden;
            adminPreviousOrder.Visibility = Visibility.Hidden;
            Stats.Visibility = Visibility.Hidden;
            workerCurrentOrder.Visibility = Visibility.Hidden;
            workerTop.Visibility = Visibility.Hidden;
            workerPreviousOrder.Visibility = Visibility.Hidden;
            Comments.Visibility = Visibility.Hidden;
            thisId = 0;
        }
        public void CancelExit(object sender, EventArgs e)
        {
            exit.Visibility = Visibility.Hidden;
        }
        public void NewOrder(object sender, RoutedEventArgs e)
        {
            userCurrentOrder.Visibility = Visibility.Hidden;
            newOrder.Visibility = Visibility.Visible;
            userPreviousOrder.Visibility = Visibility.Hidden;
            exit.Visibility = Visibility.Hidden;
        }
        public void UserCurrentOrder(object sender, RoutedEventArgs e)
        {
            userCurrentOrder.Visibility = Visibility.Visible;
            newOrder.Visibility = Visibility.Hidden;
            userPreviousOrder.Visibility = Visibility.Hidden;
            exit.Visibility = Visibility.Hidden;
            UserFindCurrentOrder(sender, e);
        }
        public void UserPreviousOrder(object sender, RoutedEventArgs e)
        {
            userCurrentOrder.Visibility = Visibility.Hidden;
            newOrder.Visibility = Visibility.Hidden;
            userPreviousOrder.Visibility = Visibility.Visible;
            exit.Visibility = Visibility.Hidden;
            UserFindPreviousOrder(sender, e);
        }
        public void AdminWorkers(object sender, RoutedEventArgs e)
        {
            ShowWorkers(sender, e);
            adminCurrentOrder.Visibility = Visibility.Hidden;
            Workers.Visibility = Visibility.Visible;
            adminPreviousOrder.Visibility = Visibility.Hidden;
            Stats.Visibility = Visibility.Hidden;
            exit.Visibility = Visibility.Hidden;
        }
        public void AdminCurrentOrder(object sender, RoutedEventArgs e)
        {
            adminCurrentOrder.Visibility = Visibility.Visible;
            Workers.Visibility = Visibility.Hidden;
            adminPreviousOrder.Visibility = Visibility.Hidden;
            Stats.Visibility = Visibility.Hidden;
            exit.Visibility = Visibility.Hidden;
            AdminFindCurrentOrder(sender, e);
        }
        public void AdminPreviousOrder(object sender, RoutedEventArgs e)
        {
            adminCurrentOrder.Visibility = Visibility.Hidden;
            Workers.Visibility = Visibility.Hidden;
            adminPreviousOrder.Visibility = Visibility.Visible;
            Stats.Visibility = Visibility.Hidden;
            exit.Visibility = Visibility.Hidden;
            AdminFindPreviousOrder(sender, e);
        }
        public void AdminStats(object sender, RoutedEventArgs e)
        {
            adminCurrentOrder.Visibility = Visibility.Hidden;
            Workers.Visibility = Visibility.Hidden;
            adminPreviousOrder.Visibility = Visibility.Hidden;
            Stats.Visibility = Visibility.Visible;
            exit.Visibility = Visibility.Hidden;
            ShowStats();
        }
        public void WorkerCurrentOrder(object sender, RoutedEventArgs e)
        {
            workerCurrentOrder.Visibility = Visibility.Visible;
            workerPreviousOrder.Visibility = Visibility.Hidden;
            Comments.Visibility = Visibility.Hidden;
            exit.Visibility = Visibility.Hidden;
            WorkerFindCurrentOrder(sender, e);
        }
        public void WorkerPreviousOrder(object sender, RoutedEventArgs e)
        {
            workerCurrentOrder.Visibility = Visibility.Hidden;
            workerPreviousOrder.Visibility = Visibility.Visible;
            Comments.Visibility = Visibility.Hidden;
            exit.Visibility = Visibility.Hidden;
            WorkerFindPreviousOrder(sender, e);
        }
        public void NewComment(object sender, RoutedEventArgs e)
        {
            workerCurrentOrder.Visibility = Visibility.Hidden;
            workerPreviousOrder.Visibility = Visibility.Hidden;
            Comments.Visibility = Visibility.Visible;
            exit.Visibility = Visibility.Hidden;
        }
        public struct tabel
        {
            public TextBox orderId;
            public TextBox date;
            public TextBox equipment;
            public TextBox problem;
            public TextBox description;
            public TextBox author;
            public TextBox status;
            public TextBox id;
            public TextBox workers;
            public Button submit;
            public Button error;
            public string[] coloms;
        }
        private static tabel[] tabels;
        private List<TextBox> cells = new List<TextBox>();
        private List<Button> buttons = new List<Button>();
        private List<Button> error = new List<Button>();
        public void ShowOrder(Canvas canvas, SqlDataReader reader, int i, int m, List<TextBox> cells, bool isAdmin)//отображение заказов 
        {
            int n = 0, wight;
            if (isAdmin)
            {
                wight = 175;
            }
            else
            {
                wight = 204;
            }
            tabels[i].coloms = new string[m];
            for (int j = 0; j < tabels[i].coloms.Length; j++)
            {
                tabels[i].coloms[j] = Convert.ToString(reader.GetValue(j));
            }
            for (int j = 0; j < tabels[i].coloms.Length; j++)
            {
                tabels[i].coloms[j] = tabels[i].coloms[j].Trim();
            }
            tabels[i].orderId = new TextBox
            {
                Width = 50,
                Height = 40,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].orderId, 50);
            Canvas.SetTop(tabels[i].orderId, 190 + (i + 1) * 40);
            tabels[i].orderId.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].orderId);
            canvas.Children.Add(tabels[i].orderId);
            n++;
            tabels[i].date = new TextBox
            {
                Width = 100,
                Height = 40,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].date, 100);
            Canvas.SetTop(tabels[i].date, 190 + (i + 1) * 40);
            tabels[i].date.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].date);
            canvas.Children.Add(tabels[i].date);
            n++;
            tabels[i].equipment = new TextBox
            {
                Width = wight,
                Height = 40,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].equipment, 200 + wight * (n - 2));
            Canvas.SetTop(tabels[i].equipment, 190 + (i + 1) * 40);
            tabels[i].equipment.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].equipment);
            canvas.Children.Add(tabels[i].equipment);
            n++;
            tabels[i].problem = new TextBox
            {
                Width = wight,
                Height = 40,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].problem, 200 + wight * (n - 2));
            Canvas.SetTop(tabels[i].problem, 190 + (i + 1) * 40);
            tabels[i].problem.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].problem);
            canvas.Children.Add(tabels[i].problem);
            n++;
            tabels[i].description = new TextBox
            {
                Width = wight,
                Height = 40,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].description, 200 + wight * (n - 2));
            Canvas.SetTop(tabels[i].description, 190 + (i + 1) * 40);
            tabels[i].description.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].description);
            canvas.Children.Add(tabels[i].description);
            n++;
            tabels[i].status = new TextBox
            {
                Width = 100,
                Height = 40,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].status, 200 + wight * (n - 1));
            Canvas.SetTop(tabels[i].status, 190 + (i + 1) * 40);
            tabels[i].status.Text = tabels[i].coloms[n];
            cells.Add(tabels[i].status);
            canvas.Children.Add(tabels[i].status);
            n++;
            tabels[i].id = new TextBox();
            tabels[i].id.Text = tabels[i].coloms[n];
            if (isAdmin)
            {
                tabels[i].error = new Button
                {
                    Width = 40,
                    Height = 40,
                    Name = $"error{i}",
                    Content = "!",
                    ToolTip = "Исполнитель не найден",
                    FontSize = 27,
                    Visibility = Visibility.Hidden,
                    Cursor = Cursors.Help,
                    Background = new SolidColorBrush(Color.FromRgb(234, 253, 193)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(234, 253, 193))
                };
                Canvas.SetLeft(tabels[i].error, 1110);
                Canvas.SetTop(tabels[i].error, 190 + (i + 1) * 40);
                error.Add(tabels[i].error);
                canvas.Children.Add(tabels[i].error);
                tabels[i].submit = new Button
                {
                    Width = 163,
                    Height = 30,
                    FontSize = 20,
                    Content = "Изменить автора",
                    Name = $"submitButton{i}",
                    Cursor = Cursors.Hand
                };
                tabels[i].submit.Click += SubmitChanges;
                Canvas.SetLeft(tabels[i].submit, 1150);
                Canvas.SetTop(tabels[i].submit, 195 + (i + 1) * 40);
                buttons.Add(tabels[i].submit);
                canvas.Children.Add(tabels[i].submit);
            }
        }
        public void ShowAuthorsInOrders(Canvas canvas, string authorText, int i, List<TextBox> cells, bool isAdmin)
        {
            int wight;
            if (isAdmin)
            {
                wight = 175;
            }
            else
            {
                wight = 204;
            }
            tabels[i].coloms[5] = " ";
            if (authorText != " ")
            {
                tabels[i].coloms[5] = authorText;
                tabels[i].coloms[5] = tabels[i].coloms[5].Trim();
            }
            else
            {
                tabels[i].coloms[5] = "-";
            }
            tabels[i].author = new TextBox
            {
                Width = wight,
                Height = 40,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].author, 200 + wight * 3);
            Canvas.SetTop(tabels[i].author, 190 + (i + 1) * 40);
            tabels[i].author.Text = tabels[i].coloms[5];
            cells.Add(tabels[i].author);
            canvas.Children.Add(tabels[i].author);
            
        }
        public void ShowWorkersInOrders(Canvas canvas, string workerText, int i, int m, List<TextBox> cells, bool isAdmin)//отображение исполнителей заказов
        {
            int width, setLeft;
            if (isAdmin)
            {
                width = 110;
                setLeft = 1000;
            }
            else
            {
                width = 184;
                setLeft = 1116;
            }
            tabels[i].coloms[m - 1] = " ";
            if (workerText != " ")
            {
                tabels[i].coloms[m - 1] = workerText;
                tabels[i].coloms[m - 1] = tabels[i].coloms[m - 1].Trim();
            }
            else
            {
                tabels[i].coloms[m - 1] = "-";
            }
            tabels[i].workers = new TextBox
            {
                Width = width,
                Height = 40,
                FontSize = 20
            };
            Canvas.SetLeft(tabels[i].workers, setLeft);
            Canvas.SetTop(tabels[i].workers, 190 + (i + 1) * 40);
            tabels[i].workers.Text = tabels[i].coloms[m - 1];
            cells.Add(tabels[i].workers);
            canvas.Children.Add(tabels[i].workers);
        }
        public void FindOrder(Canvas canvas, string slqExpressionWhere, string slqExpressionOrder, TextBlock NoOrder, List<TextBox> cells, TextBox textBox, bool isAdmin)// выполняет запросы к бд в соответствии с состоянием строки поиска
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
                sqlExpression = $"SELECT COUNT(*) FROM [dbo].[orders] WHERE (equipment LIKE \'%{textBox.Text}%\' OR problem LIKE \'%{textBox.Text}%\' OR description LIKE \'%{textBox.Text}%\') AND {slqExpressionWhere}";
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
            }
            if(buttons.Count != 0)
            {
                foreach (Button button in buttons)//удаление старой
                {
                    canvas.Children.Remove(button);
                }
            }
            buttons.Clear();
            if (error.Count != 0)
            {
                foreach (Button error in error)//удаление старой
                {
                    canvas.Children.Remove(error);
                }
            }
            error.Clear();
            if (count > 0)// есть результат
            {
                NoOrder.Visibility = Visibility.Hidden;
                tabels = new tabel[count];

                if (textBox.Text == "")
                {
                    sqlExpression = $"SELECT * FROM [dbo].[orders] WHERE {slqExpressionWhere} {slqExpressionOrder}";
                }
                else
                {
                    sqlExpression = $"SELECT * FROM [dbo].[orders] WHERE (equipment LIKE \'%{textBox.Text}%\' OR problem LIKE \'%{textBox.Text}%\' OR description LIKE \'%{textBox.Text}%\') AND {slqExpressionWhere} {slqExpressionOrder}";
                }

                command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                SqlDataReader tempReader;
                SqlCommand tempCommand;
                int i = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ShowOrder(canvas, reader, i, 7, cells, isAdmin);
                        i++;
                    }
                    reader.Close();
                    
		            int i1 = i;
		            for(int j = 0; i1 > 0; j++, i1--)
                    {
                        sqlExpression = $"SELECT name, surname FROM [dbo].[entry] WHERE id = (SELECT id FROM [dbo].[orders] WHERE orderId = {tabels[j].orderId.Text})";
                        tempCommand = new SqlCommand(sqlExpression, connection);
                        tempCommand.ExecuteNonQuery();
                        string authorText = " ";
                        if (Convert.ToString(tempCommand.ExecuteScalar()) != "")
                        {
                            tempReader = tempCommand.ExecuteReader();
                            tempReader.Read();
                            for(int k = 0; k < 2; k++)
                            {
                                authorText = authorText.Insert(authorText.Length, $" {Convert.ToString(tempReader.GetValue(k))}");
                                authorText = authorText.Trim();
                            }
                            tempReader.Close();
			            }
                        ShowAuthorsInOrders(canvas, authorText, j, cells, isAdmin);
			        }
                    for(int j = 0; i > 0; j++, i--)// исполниетели каждого показываемого заказа
                    {
                        sqlExpression = $"SELECT name, surname FROM [dbo].[entry] WHERE id = (SELECT id FROM [dbo].[workers] WHERE orderId = {tabels[j].orderId.Text})";
                        tempCommand = new SqlCommand(sqlExpression, connection);
                        tempCommand.ExecuteNonQuery();
                        string workerText = " ";
                        if (Convert.ToString(tempCommand.ExecuteScalar()) != "")
                        {
                            tempReader = tempCommand.ExecuteReader();
                            tempReader.Read();
                            for(int k = 0; k < 2; k++)
                            {
                                workerText = workerText.Insert(workerText.Length, $" {Convert.ToString(tempReader.GetValue(k))}");
                                workerText = workerText.Trim();
                            }
                            tempReader.Close();
                        }
                        ShowWorkersInOrders(canvas, workerText, j, 7, cells, isAdmin);
                    }
                }
            }
            else// нет результата
            {
                NoOrder.Visibility = Visibility.Visible;
            }
            connection.Close();
        }
        public void UserFindCurrentOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(userCurrentOrder, "(status = \'в работе\' OR status = \'в ожидании\')", "", userNoCurrentOrders, cells, search1, false);
        }
        public void UserFindPreviousOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(userPreviousOrder, "(status = \'выполнено\')", "", userNoPreviousOrders, cells, search2, false);
        }
        public void AddNewOrder(object sender, RoutedEventArgs e)
        {
            OrderAdded.Visibility = Visibility.Hidden;
            if (equipmentName.Text == "")// проверка заполненности полей
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
            if(equipmentName.Text != "" && problemName.Text != "" && descriptionName.Text != "")// добавление в бд нового заказа
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                DateTime dateTime = DateTime.Now;
                string sqlExpression = $"INSERT INTO [dbo].[orders] VALUES (\'{dateTime.Year}-{dateTime.Month}-{dateTime.Day}\', \'{equipmentName.Text}\',\'{problemName.Text}\',\'{descriptionName.Text}\', \'в ожидании\', {thisId})";
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
        public void ShowWorker(Canvas canvas, SqlDataReader reader, int i, int m, List<TextBox> cells)// показ работников
        {
            int n = 0;
            WorkerTabels[i].coloms = new string[m+2];
            for (int j = 0; j < 4; j++)
            {
                WorkerTabels[i].coloms[j] = Convert.ToString(reader.GetValue(j));
            }
            for (int j = 0; j < 4; j++)
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
            cells.Add(WorkerTabels[i].name);
            canvas.Children.Add(WorkerTabels[i].name);
            WorkerTabels[i].surname = new TextBox
            {
                Width = 250,
                Height = 80,
                FontSize = 20
            };
            Canvas.SetLeft(WorkerTabels[i].surname, 450);
            Canvas.SetTop(WorkerTabels[i].surname, 150 + (i + 1) * 80);
            cells.Add(WorkerTabels[i].surname);
            canvas.Children.Add(WorkerTabels[i].surname);
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
        public void ShowNameInWorkers(SqlDataReader reader, int i)//отображение исполнителей заказов
        {
            //WorkerTabels[i].coloms = new string[2];
            for (int j = 4; j < 6; j++)
            {
                WorkerTabels[i].coloms[j] = Convert.ToString(reader.GetValue(j-4));
            }
            for (int j = 4; j < 6; j++)
            {
                WorkerTabels[i].coloms[j] = WorkerTabels[i].coloms[j].Trim();
            }
            WorkerTabels[i].name.Text = WorkerTabels[i].coloms[1];
            WorkerTabels[i].surname.Text = WorkerTabels[i].coloms[2];
        }
        public void FindWorkers(Canvas thisCanvas, Canvas canvas, List<TextBox> cells, TextBox textBox)// запрос к бд на информацию о работниках
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
                        ShowWorker(thisCanvas, reader, i, 4, cells);
                        i++;
                    }
                    reader.Close();

                    for (int j = 0; i > 0; j++, i--)
                    {
                        sqlExpression = $"SELECT name, surname FROM [dbo].[entry] WHERE id = {WorkerTabels[j].id.Text}";
                        SqlCommand tempCommand = new SqlCommand(sqlExpression, connection);
                        tempCommand.ExecuteNonQuery();
                        SqlDataReader tempReader = tempCommand.ExecuteReader();
                        tempReader.Read();
                        ShowNameInWorkers(tempReader, j);
                        tempReader.Close();

                    }
                }
            }
            else
            {
                NoWorkers.Visibility = Visibility.Visible;
            }
            connection.Close();
        }
        public void SubmitChanges(object sender, RoutedEventArgs e)
        {
            if (error.Count != 0)
            {
                foreach (Button error in error)//удаление старой
                {
                    error.Visibility = Visibility.Hidden;
                }
            }
            Button thisButton = sender as Button;
            string buttonName = thisButton.Name.Remove(0, 12);
            int i = Int32.Parse(buttonName);
            int id = Int32.Parse(tabels[i].id.Text);
            string[] nameSurname = tabels[i].workers.Text.Split(' ');

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sqlExpression;
            SqlCommand command;
            error[i].Visibility = Visibility.Hidden;
            if (nameSurname.Length > 1)
            {
                sqlExpression = $"SELECT id FROM [dbo].[workers] WHERE name = \'{nameSurname[0]}\' AND surname = \'{nameSurname[1]}\'";
                command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
                if (int.TryParse(Convert.ToString(command.ExecuteScalar()), out int x))
                {
                    sqlExpression = $"UPDATE [dbo].[workers] SET orderId = {id}, status = \'в работе\' WHERE id = {x}";
                    command = new SqlCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                    sqlExpression = $"UPDATE [dbo].[orders] SET status = \'в работе\' WHERE orderId = {id}";
                    command = new SqlCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                }
                else
                {
                    error[i].Visibility = Visibility.Visible;
                }
            }
            else
            {
                error[i].Visibility = Visibility.Visible;
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
        public void ShowStats()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string sqlExpression = $"SELECT COUNT(*) FROM [dbo].[orders] WHERE status = \'выполнено\'";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.ExecuteNonQuery();
            int count = (int)command.ExecuteScalar();
            CompletedOrdersAmmount.Text = count.ToString();
            sqlExpression = $"SELECT COUNT(*) FROM [dbo].[orders]";
            command = new SqlCommand(sqlExpression, connection);
            command.ExecuteNonQuery();
            count = (int)command.ExecuteScalar();
            allOrdersAmmount.Text = count.ToString();
        }
        public void AdminFindCurrentOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(adminCurrentOrder, "(status = \'в работе\' OR status = \'в ожидании\')", "ORDER BY status ASC", adminNoCurrentOrders, cells, search3, true);
        }
        public void AdminFindPreviousOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(adminPreviousOrder, "(status = \'выполнено\')", "", adminNoPreviousOrders, cells, search4, false);
        }

        public void WorkerFindCurrentOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(workerCurrentOrder, "status = \'в работе\' OR status = \'в ожидании\'", "", workerNoCurrentOrders, cells, search5, false);
        }
        public void WorkerFindPreviousOrder(object sender, RoutedEventArgs e)
        {
            FindOrder(workerPreviousOrder, "status = \'выполнено\'", "", workerNoPreviousOrders, cells, search6, false);
        }
        public void AddComment(object sender, RoutedEventArgs e)
        {

        }
    }
}