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
    /// Логика взаимодействия для Modal_OtherFamily.xaml
    /// </summary>
    public partial class Modal_OtherFamily : Window
    {
        List<string> type = new List<string>();
        bool IsClosing = false;
        int id;
        static Brothers_sisters family = new Brothers_sisters();

        public static Brothers_sisters GetFamily()
        {
            return family;
        }
        public Modal_OtherFamily(int id)
        {
            InitializeComponent();

            this.Left = 797.6;
            this.Top = 531.2;

            this.id = id;

            type.Add("Учится");
            type.Add("Работает");

            TypeCombo.ItemsSource = type;

            List<Brothers_sisters> currentFamily = CuratorsHelperEntities.GetContext().Brothers_sisters.ToList();
            currentFamily = currentFamily.Where(p => p.id_student == id).ToList();
            currentFamily.Insert(0, new Brothers_sisters
            {
                FIO = "Выберите родственника"
            });
            ParentCombo.ItemsSource = currentFamily;
            ParentCombo.SelectedIndex = 0;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();

        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;

            family = (Brothers_sisters)DataContext;
            family.status = TypeCombo.SelectedItem.ToString();
            family.date_born = Date_text.SelectedDate;
            this.DialogResult = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (!IsClosing) Close();
        }

        private void ParentCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParentCombo.SelectedIndex != 0)
            {
                List<Brothers_sisters> currentFamily = CuratorsHelperEntities.GetContext().Brothers_sisters.ToList();
                currentFamily = currentFamily.Where(p => p.id_student == id).ToList();

                currentFamily = currentFamily.Where(p => p.FIO == ParentCombo.SelectedValue.ToString()).ToList();
                DataContext = currentFamily[0];
                TypeCombo.SelectedItem = currentFamily[0].status;
            }
        }
    }
}
