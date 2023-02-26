using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace CuratorsHelper
{
    /// <summary>
    /// Логика взаимодействия для GroupsPage.xaml
    /// </summary>
    public partial class GroupsPage : Page
    {
        WindowCollection windows = Application.Current.Windows;
        public GroupsPage()
        {
            InitializeComponent();
            var moun = CuratorsHelperEntities.GetContext().Mounth.ToList();
            comboMounth.ItemsSource = moun;
            comboMounth.SelectedItem = 1;

            var currentCurator = CuratorsHelperEntities.GetContext().Curators.ToList();
            Curators curator = currentCurator.Single(p => p.id_pass == UserId.ID);

            NumGroupText.Content += curator.Groups.name;
        }

        private void comboMounth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentReport = CuratorsHelperEntities.GetContext().Plan_work.ToList();
            currentReport = currentReport.Where(p => p.mounth == (int?)comboMounth.SelectedValue).ToList();

            currentReport = currentReport.OrderByDescending(p => p.id_type).ToList();

            ReportList.ItemsSource = currentReport;

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window change = new View.ChangePlan();
            change.Show();
            windows[0].Close();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Plan_work selected = (Plan_work)ReportList.SelectedItem;
            Window change = new View.ChangePlan(selected);
            change.Show();
            windows[0].Close();
        }

        private void otchet_Click(object sender, RoutedEventArgs e)
        {
            Window report = new View.ReportPlan();
            report.Show();
            windows[0].Close();
        }

        private void btnStudent_Click(object sender, RoutedEventArgs e)
        {
            Window student = new View.StudentsWindow();
            student.Show();
            windows[0].Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Plan_work selectedPlan = (Plan_work)ReportList.SelectedItem;
            CuratorsHelperEntities.GetContext().Plan_work.Remove(selectedPlan);
            try
            {
                CuratorsHelperEntities.GetContext().SaveChanges();
                var currentReport = CuratorsHelperEntities.GetContext().Plan_work.ToList();
                currentReport = currentReport.Where(p => p.mounth == (int?)comboMounth.SelectedValue).ToList();
                currentReport = currentReport.OrderByDescending(p => p.id_type).ToList();
                ReportList.ItemsSource = currentReport;

            }
            catch
            {
                MessageBox.Show("Что-то пошло не так");
            }
        }
    }
}