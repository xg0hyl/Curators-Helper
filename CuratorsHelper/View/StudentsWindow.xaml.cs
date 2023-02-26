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
using System.Threading;
using Microsoft.Win32;
using System.Windows.Interop;
using System.Diagnostics;

namespace CuratorsHelper.View
{
    /// <summary>
    /// Логика взаимодействия для StudentsWindow.xaml
    /// </summary>
    public partial class StudentsWindow : Window
    {
        bool handler = false;
        student St = new student();
        string SaveText = ""; string SaveText2 = ""; int id;
        Image[] edit; TextBox[] tb; Image[] accept; Image[] cancel;

        OpenFileDialog ofdPicture = new OpenFileDialog();

        List<Hobbies> selectHobby = CuratorsHelperEntities.GetContext().Hobbies.ToList();
        Passport selectPass;
        Parents selectParent;
        Brothers_sisters selectFamily;

        public StudentsWindow()
        {
            InitializeComponent();
            var currentStudent = CuratorsHelperEntities.GetContext().student.ToList();
            var Curator = CuratorsHelperEntities.GetContext().Curators.ToList();
            Curators cur = Curator.Single(p => p.id_curator == UserId.ID);
            currentStudent = currentStudent.Where(p => p.id_group == cur.id_group).ToList();
            studentsCombo.ItemsSource = currentStudent;
            studentsCombo.SelectedIndex = 0;

            edit = new Image[] { Fio_edit, DateBorn_edit, National_edit, School_edit, Passport_edit, Adres_edit, Phone_edit, Hostel_edit, Health_edit, Mother_edit, Father_edit, null , Other_edit, Relationship_edit, Hobbies_edit };
            tb = new TextBox[] { FioText, null, NationalText, SchoolText, PassportText, Adrestext, PhoneText, HostelText, HealthText, MotherText, FatherText, Brothers, Sisters, RelationshipText, HobbiesText };
            accept = new Image[] { Fio_access, DateBord_access, National_access, School_access, Passport_access, Adres_access, Phone_access, Hostel_access, Health_access, Mother_access, Father_access, null, Other_access, Relationship_access, Hobbies_access };
            cancel = new Image[] { Fio_cancel, DateBord_cancel, National_cancel, School_cancel, Passport_cancel, Adres_cancel, Phone_cancel, Hostel_cancel, Health_cancel, Mother_cancel, Father_cancel, null, Other_cancel, Relationship_cancel, Hobbies_cancel };
        }

        private void collapseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void closeWindow_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window back = new MainMenuCurators();
            back.Show();
            this.Close();
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


        private void WriteParent()
        {
            var currentParents = CuratorsHelperEntities.GetContext().Parents.ToList();

            currentParents = currentParents.Where(p => p.id_student == id).ToList();


            var Mother = currentParents.Where(p => p.parent == "Мать").ToList();
            if (Mother.Count != 0)
            {
                DateTime dateMother = new DateTime();
                if (Mother[0].date_bord != null)
                    dateMother = (DateTime)Mother[0].date_bord;
                MotherText.Text = Mother[0].parent + ": " + Mother[0].FIO + ", " + dateMother.Date.ToString("dd.MM.yyyy") + ", " + Mother[0].adres + ", " + Mother[0].phone + ", " + Mother[0].job_place + ", " + Mother[0].job;
            }

            var Father = currentParents.Where(p => p.parent == "Отец").ToList();
            if (Father.Count != 0)
            {
                DateTime dateFather = new DateTime();

                if (Father[0].date_bord != null)
                    dateFather = (DateTime)Father[0].date_bord;

                FatherText.Text = Father[0].parent + ": " + Father[0].FIO + ", " + dateFather.Date.ToString("dd.MM.yyyy") + ", " + Father[0].adres + ", " + Father[0].phone + ", " + Father[0].job_place + ", " + Father[0].job;
            }

            var Opekun = currentParents.Where(p => p.parent == "Опекун").ToList();
            if (Opekun.Count != 0)
            {
                DateTime dateOpekun = new DateTime();

                if (Opekun[0].date_bord != null)
                    dateOpekun = (DateTime)Opekun[0].date_bord;

                MotherText.Text = Opekun[0].parent + ": " + Opekun[0].FIO + ", " + dateOpekun.Date.ToString("dd.MM.yyyy") + ", " + Opekun[0].adres + ", " + Opekun[0].phone + ", " + Opekun[0].job_place + ", " + Opekun[0].job;
            }
        }

        private void WriteFamily()
        {
            var currentOtherFamily = CuratorsHelperEntities.GetContext().Brothers_sisters.ToList();
            currentOtherFamily = currentOtherFamily.Where(p => p.id_student == id).ToList();
            DateTime OtherDate = new DateTime();
            for (int i = 0; i < currentOtherFamily.Count; i++)
            {
                if (currentOtherFamily[i].date_born != null)
                    OtherDate = (DateTime)currentOtherFamily[i].date_born;
                if (currentOtherFamily[i].whois == "Брат")
                    Brothers.Text += currentOtherFamily[i].whois + ": " + currentOtherFamily[i].FIO + ", " + OtherDate.Date.ToString("dd.MM.yyyy") + ", " + currentOtherFamily[i].status + "\n";
                if (currentOtherFamily[i].whois == "Сестра")
                    Sisters.Text += currentOtherFamily[i].whois + ": " + currentOtherFamily[i].FIO + ", " + OtherDate.Date.ToString("dd.MM.yyyy") + ", " + currentOtherFamily[i].status + "\n";
            }
        }

        private void studentsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                foreach (var item in tb)
                {
                    if (item != null) item.Clear();
                }
            }
            var currentStudent = CuratorsHelperEntities.GetContext().student.ToList();

            var currentHostel = CuratorsHelperEntities.GetContext().Hostel.ToList();

            var currentPassport = CuratorsHelperEntities.GetContext().Passport.ToList();



            St = currentStudent.Single(p => p.id_student == (int)studentsCombo.SelectedValue);
            DataContext = St;
            id = St.id_student;

            try
            {
                HostelText.DataContext = currentHostel.Single(p => p.id_student == St.id_student);
            }
            catch
            {

            }
            
            try
            {
                Passport pass = currentPassport.Single(p => p.id_student == St.id_student);
                DateTime datePassport = new DateTime();
                if (pass.date_issue != null)
                    datePassport = (DateTime)pass.date_issue;
                PassportText.Text = pass.num_passport + ", " + pass.person_issue + ", " + datePassport.Date.ToString("dd.MM.yyyy");
            }
           catch
            {

            }

            try
            {
                WriteParent();
                WriteFamily();
                var currentStudentHobbies = CuratorsHelperEntities.GetContext().Student_Hobbies.ToList();

                currentStudentHobbies = currentStudentHobbies.Where(p => p.id_student == St.id_student).ToList();
                foreach (var item in currentStudentHobbies)
                    HobbiesText.Text += item.Hobbies.hobby + " ";
            }
            catch
            {

            }

            if (St.photo == null)
            {
                StudentPhoto.Source = new BitmapImage(new Uri(@"nonePhoto.png", UriKind.Relative));
            }
            else
            {
                byte[] byteimg = St.photo;
                MemoryStream ms = new MemoryStream(byteimg);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                System.Drawing.Bitmap output = new System.Drawing.Bitmap(img);
                StudentPhoto.Source = Imaging.CreateBitmapSourceFromHBitmap(output.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
 
        }

        private void SaveChanges()
        {
           
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Image).Name == "DateBord_access")
            {


                edit[Array.IndexOf(accept, (sender as Image))].Visibility = Visibility.Visible;
                cancel[Array.IndexOf(accept, (sender as Image))].Visibility = Visibility.Hidden;
                (sender as Image).Visibility = Visibility.Hidden;

                St.date_born = DateBornDate.SelectedDate;
                DataContext = St;
                CuratorsHelperEntities.GetContext().Entry(St).State = System.Data.Entity.EntityState.Modified;
                try
                {
                    CuratorsHelperEntities.GetContext().SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так");
                }


            } 

            else if ((sender as Image).Name == "Hobbies_access")
            {


                edit[Array.IndexOf(accept, (sender as Image))].Visibility = Visibility.Visible;
                cancel[Array.IndexOf(accept, (sender as Image))].Visibility = Visibility.Hidden;
                (sender as Image).Visibility = Visibility.Hidden;

                Student_Hobbies newSt_Hob = new Student_Hobbies();
                newSt_Hob.id_student = id;
                newSt_Hob.id_hobby = selectHobby[0].id_hobby;

                CuratorsHelperEntities.GetContext().Student_Hobbies.Add(newSt_Hob);
                try
                {
                    CuratorsHelperEntities.GetContext().SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так");
                }


            }

            else if ((sender as Image).Name == "Passport_access")
            {
                edit[Array.IndexOf(accept, (sender as Image))].Visibility = Visibility.Visible;
                cancel[Array.IndexOf(accept, (sender as Image))].Visibility = Visibility.Hidden;
                (sender as Image).Visibility = Visibility.Hidden;

                CuratorsHelperEntities.GetContext().Entry(selectPass).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    CuratorsHelperEntities.GetContext().SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так");
                }
            }

            else if ((sender as Image).Name == "Mother_access" || (sender as Image).Name == "Father_access")
            {
                edit[Array.IndexOf(accept, (sender as Image))].Visibility = Visibility.Visible;
                cancel[Array.IndexOf(accept, (sender as Image))].Visibility = Visibility.Hidden;
                (sender as Image).Visibility = Visibility.Hidden;

                CuratorsHelperEntities.GetContext().Entry(selectParent).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    CuratorsHelperEntities.GetContext().SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так");
                }

            }

            else if ((sender as Image).Name == "Other_edit")
            {

                edit[Array.IndexOf(accept, (sender as Image))].Visibility = Visibility.Visible;
                cancel[Array.IndexOf(accept, (sender as Image))].Visibility = Visibility.Hidden;
                (sender as Image).Visibility = Visibility.Hidden;

                CuratorsHelperEntities.GetContext().Entry(selectFamily).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    CuratorsHelperEntities.GetContext().SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так");
                }


            }

            else
            {


                edit[Array.IndexOf(accept, (sender as Image))].Visibility = Visibility.Visible;
                cancel[Array.IndexOf(accept, (sender as Image))].Visibility = Visibility.Hidden;
                (sender as Image).Visibility = Visibility.Hidden;
                tb[Array.IndexOf(accept, (sender as Image))].IsReadOnly = true;
                PassportText.Focus();
                try
                {
                    CuratorsHelperEntities.GetContext().SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так");
                }


            }

            handler = false;
           
        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Image).Name == "DateBord_cancel")
            {
                accept[Array.IndexOf(cancel, (sender as Image))].Visibility = Visibility.Hidden;
                edit[Array.IndexOf(cancel, (sender as Image))].Visibility = Visibility.Visible;
                (sender as Image).Visibility = Visibility.Hidden;
                DateBornDate.Text = SaveText;
                DateBornDate.IsDropDownOpen = false;
            }
            else
            {
                accept[Array.IndexOf(cancel, (sender as Image))].Visibility = Visibility.Hidden;
                edit[Array.IndexOf(cancel, (sender as Image))].Visibility = Visibility.Visible;
                (sender as Image).Visibility = Visibility.Hidden;
                tb[Array.IndexOf(cancel, (sender as Image))].Text = SaveText;
                tb[Array.IndexOf(cancel, (sender as Image))].IsReadOnly = true;
            }

            if ((sender as Image).Name == "Other_cancel") Brothers.Text = SaveText2;

            handler = false; 
        }

        private void Image_MouseDown_3(object sender, MouseButtonEventArgs e)
        {


            if (!handler)
            {
                if ((sender as Image).Name == "Passport_edit")
                {


                    Window PassportWin = new View.Passport_Modal(id);
                    if (PassportWin.ShowDialog() == true)
                    {
                        accept[Array.IndexOf(edit, (sender as Image))].Visibility = Visibility.Visible;
                        cancel[Array.IndexOf(edit, (sender as Image))].Visibility = Visibility.Visible;
                        (sender as Image).Visibility = Visibility.Hidden;
                        SaveText = PassportText.Text;

                        selectPass = Passport_Modal.GetPass();
                        DateTime datePassport = new DateTime();
                        if (selectPass.date_issue != null)
                            datePassport = (DateTime)selectPass.date_issue;
                        PassportText.Text = selectPass.num_passport + ", " + selectPass.person_issue + ", " + datePassport.Date.ToString("dd.MM.yyyy");
                    }


                }

                else if ((sender as Image).Name == "Mother_edit" || (sender as Image).Name == "Father_edit")
                {
                    Window ParentsWin = new Window();
                    if ((sender as Image).Name == "Mother_edit")
                        ParentsWin = new View.Modal_Parents(id, "Мать");
                    if ((sender as Image).Name == "Father_edit")
                        ParentsWin = new View.Modal_Parents(id, "Отец");

                    if (ParentsWin.ShowDialog() == true)
                    {
                        accept[Array.IndexOf(edit, (sender as Image))].Visibility = Visibility.Visible;
                        cancel[Array.IndexOf(edit, (sender as Image))].Visibility = Visibility.Visible;
                        (sender as Image).Visibility = Visibility.Hidden;
                        SaveText = MotherText.Text;

                        selectParent = Modal_Parents.GetParent();
                        WriteParent();
                    }
                }

                else if ((sender as Image).Name == "Other_edit")
                {
                    Window otherParents = new View.Modal_OtherFamily(id);
                    
                    if (otherParents.ShowDialog() == true)
                    {
                        accept[Array.IndexOf(edit, (sender as Image))].Visibility = Visibility.Visible;
                        cancel[Array.IndexOf(edit, (sender as Image))].Visibility = Visibility.Visible;
                        (sender as Image).Visibility = Visibility.Hidden;
                        SaveText = Sisters.Text;
                        SaveText2 = Brothers.Text;

                        Sisters.Clear();
                        Brothers.Clear();

                        selectFamily = Modal_OtherFamily.GetFamily();
                        WriteFamily();
                    }

                }

                else if ((sender as Image).Name == "Hobbies_edit")
                {
                    Window ho = new Modal_Hobbies(id);
                    if (ho.ShowDialog() == true)
                    {

                        accept[Array.IndexOf(edit, (sender as Image))].Visibility = Visibility.Visible;
                        cancel[Array.IndexOf(edit, (sender as Image))].Visibility = Visibility.Visible;
                        (sender as Image).Visibility = Visibility.Hidden;
                        SaveText = HobbiesText.Text;
                        Student_Hobbies last = Modal_Hobbies.GetHobby();
                        selectHobby = selectHobby.Where(p => p.id_hobby == last.id_hobby).ToList();
                        HobbiesText.Text += selectHobby[0].hobby + " ";
                    }


                }

                else if ((sender as Image).Name == "DateBorn_edit")
                {


                    accept[Array.IndexOf(edit, (sender as Image))].Visibility = Visibility.Visible;
                    cancel[Array.IndexOf(edit, (sender as Image))].Visibility = Visibility.Visible;
                    (sender as Image).Visibility = Visibility.Hidden;
                    SaveText = DateBornDate.Text;

                    DateBornDate.Focus();
                    DateBornDate.IsDropDownOpen = true;
                    handler = true;


                }

                else
                {
                    accept[Array.IndexOf(edit, (sender as Image))].Visibility = Visibility.Visible;
                    cancel[Array.IndexOf(edit, (sender as Image))].Visibility = Visibility.Visible;
                    (sender as Image).Visibility = Visibility.Hidden;
                    tb[Array.IndexOf(edit, (sender as Image))].IsReadOnly = false;
                    tb[Array.IndexOf(edit, (sender as Image))].Focus();
                    SaveText = tb[Array.IndexOf(edit, (sender as Image))].Text;
                    tb[Array.IndexOf(edit, (sender as Image))].Clear();
                    handler = true;
                }
            }
            else MessageBox.Show("Невозможно пока редактируется другое поле");


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateBornDate.IsDropDownOpen = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window PersonalWork = new Window();
            if ((sender as Button).Name == "PersonalWithStudent")
            {
                PersonalWork = new PersonalWorksWindow(true, id);
            }
            if ((sender as Button).Name == "PersonalWithParent")
            {
                PersonalWork = new PersonalWorksWindow(false, id);
            }
            PersonalWork.Show();
            this.Close();
        }

        private void AchivmentBtn_Click(object sender, RoutedEventArgs e)
        {
            Window achivmentWin = new AchivmentStudentWindow(id);
            achivmentWin.Show();
            this.Close();
        }

        private void AssotialBtn_Click(object sender, RoutedEventArgs e)
        {
            Window assotialWin = new AssotialStudentWindow(id);
            assotialWin.Show();
            this.Close();
        }

        private void StudentPhoto_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ofdPicture.Filter =
                "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                StudentPhoto.Source = new BitmapImage(new Uri(ofdPicture.FileName));
                IMGAccept.Visibility = Visibility.Visible;
                IMGCancel.Visibility = Visibility.Visible;
            }


        }

        private void Image_MouseDown_4(object sender, MouseButtonEventArgs e)
        {
            IMGAccept.Visibility = Visibility.Hidden;
            IMGCancel.Visibility = Visibility.Hidden;
            byte[] img = File.ReadAllBytes(ofdPicture.FileName);
            St.photo = img;

            try
            {
                CuratorsHelperEntities.GetContext().SaveChanges();
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так");
            }
        }

        private void Image_MouseDown_5(object sender, MouseButtonEventArgs e)
        {
            IMGAccept.Visibility = Visibility.Hidden;
            IMGCancel.Visibility = Visibility.Hidden;
            byte[] byteimg = St.photo;
            MemoryStream ms = new MemoryStream(byteimg);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            System.Drawing.Bitmap output = new System.Drawing.Bitmap(img);
            StudentPhoto.Source = Imaging.CreateBitmapSourceFromHBitmap(output.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
