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
using System.Windows.Shapes;

namespace CuratorsHelper.View
{
    /// <summary>
    /// Логика взаимодействия для Modal_Parents.xaml
    /// </summary>
    public partial class Modal_Parents : Window
    {
        bool IsClosing = false;
        static Parents par = new Parents();

        public static Parents GetParent()
        {
            return par;
        }

        public Modal_Parents(int id, string parnt)
        {
            InitializeComponent();

            this.Left = 796;
            this.Top = 387;

            var currentParent = CuratorsHelperEntities.GetContext().Parents.ToList();
            currentParent = currentParent.Where(p => p.id_student == id && (p.parent == parnt || p.parent == "Опекун")).ToList();
            DataContext = currentParent[0];
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;

            par = (Parents)DataContext;
            par.date_bord = Date_text.SelectedDate;
            this.DialogResult = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (!IsClosing) Close();
        }
    }
}
