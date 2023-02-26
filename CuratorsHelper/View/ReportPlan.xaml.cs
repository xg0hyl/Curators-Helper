using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ReportPlan.xaml
    /// </summary>
    public partial class ReportPlan : Window
    {
        Report report = new Report();
        public int hand = 0;
        public ReportPlan()
        {
            InitializeComponent();
            var currentMounth = CuratorsHelperEntities.GetContext().Mounth.ToList();
            comboMounth.ItemsSource = currentMounth;


            var currentCurator = CuratorsHelperEntities.GetContext().Curators.ToList();
            Curators curator = currentCurator.Single(p => p.id_pass == UserId.ID);

            NumGroupText.Text += curator.Groups.name;
        }


        private void comboMounth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentReport = CuratorsHelperEntities.GetContext().Report.ToList();
            currentReport = currentReport.Where(p => p.mounth == (int?)comboMounth.SelectedValue).ToList();
            ReportList.ItemsSource = currentReport;
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

                report = (Report)ReportList.SelectedItem;

                textForm.Text = report.text_report;

                textHours.Text = report.hours_week.ToString();

                if (report.check_end == "отмеченно")
                    Check.IsChecked = true;
                else Check.IsChecked = false;
                hand = 1;


        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Window modal = new ModalAccept();
            StringBuilder errors = new StringBuilder();
            if (modal.ShowDialog() == true)
            {
                if (hand == 0)
                {
                    double hour;
                    if (double.TryParse(textHours.Text, out hour))
                        report.hours_week = hour;
                    else errors.Append("Введите число\n");
                    
                    report.text_report = textForm.Text;
                    report.mounth = comboMounth.SelectedIndex + 1;

                    if (Check.IsChecked == true)
                        report.check_end = "отмеченно";
                    else report.check_end = "не отмеченно";

                    CuratorsHelperEntities.GetContext().Report.Add(report);
                }
                else if (hand == 1)
                {
                    double hour;
                    if (double.TryParse(textHours.Text, out hour))
                        report.hours_week = hour;
                    else errors.Append("Введите число\n");

                    report.text_report = textForm.Text;

                    report.mounth = comboMounth.SelectedIndex + 1;

                    if (Check.IsChecked == true)
                        report.check_end = "отмеченно";
                    else report.check_end = "не отмеченно";

                    hand = 0;
                }

                if (errors.Length == 0)
                {
                    try
                    {
                        CuratorsHelperEntities.GetContext().SaveChanges();
                        MessageBox.Show("Успешно");


                        textForm.Clear();
                        Check.IsChecked = false;
                        textHours.Text = "";

                        var currentReport = CuratorsHelperEntities.GetContext().Report.ToList();
                        currentReport = currentReport.Where(p => p.mounth == comboMounth.SelectedIndex + 1).ToList();
                        ReportList.ItemsSource = currentReport;

                    }
                    catch (Exception msg)
                    {
                        MessageBox.Show(msg.Message);
                    }
                }
                else MessageBox.Show(errors.ToString());

            }
        }

        private void collapseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("rememberMe.txt", false))
            {
                writer.WriteLine("");
            }
            UserId.Role = null;
            Window back = new MainWindow();
            back.Show();
            this.Close();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window back = new MainMenuCurators();
            back.Show();
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Report report = (Report)ReportList.SelectedItem;
            CuratorsHelperEntities.GetContext().Report.Remove(report);
            try
            {
                CuratorsHelperEntities.GetContext().SaveChanges();
                var currentReport = CuratorsHelperEntities.GetContext().Report.ToList();
                currentReport = currentReport.Where(p => p.mounth == (int?)comboMounth.SelectedValue).ToList();
                ReportList.ItemsSource = currentReport;
            }
            catch 
            {
                MessageBox.Show("Что-то пошло не так");
            }
        }
    }
}
