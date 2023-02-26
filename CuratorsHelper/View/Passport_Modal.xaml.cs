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
    /// Логика взаимодействия для Passport_Modal.xaml
    /// </summary>
    public partial class Passport_Modal : Window
    {
        bool IsClosing = false;
        int id;
        static Passport pass = new Passport();

        public static Passport GetPass()
        {
            return pass;
        }

        public Passport_Modal(int id)
        {
            InitializeComponent();
            //this.Left = 58;
            //this.Top = 562;
            /* 216 643 */
            this.id = id;

            var currentPassport = CuratorsHelperEntities.GetContext().Passport.ToList();
            currentPassport = currentPassport.Where(p => p.id_student == id).ToList();
            numText.Text = currentPassport[0].num_passport;
            dateText.SelectedDate = currentPassport[0].date_issue;
            WhoText.Text = currentPassport[0].person_issue;
            pass = currentPassport[0];
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;

            pass.num_passport = numText.Text;
            pass.person_issue = WhoText.Text;
            pass.date_issue = dateText.SelectedDate;

            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsClosing = true;
            this.Close();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (!IsClosing) Close();
        }
    }
}
