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

namespace IT_Решения
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Init(object sender, RoutedEventArgs e)//вода камни рыбы
        {
            login.Opacity = 0;
            main.Opacity = 1;
        }
        private void Init1(object sender, RoutedEventArgs e)//вода камни рыбы
        {
            login.Opacity = 1;
            main.Opacity = 0;
        }
    }
}
