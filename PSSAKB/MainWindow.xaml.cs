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
//using System.Windows.Shapes;
using System.Data;
using System.IO;
using System.ServiceModel;
using System.Configuration;
using CRESapi;
using System.IO;
using acl;
namespace PSSAKB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<GlobalAPI.WindowsServices> serverService = new List<GlobalAPI.WindowsServices>();
        public MainWindow()
        {
            
            
            //initialize end points to server
            apiHandler.setEndPoint(string.Format(ConfigurationManager.AppSettings["globalapiuri"], ConfigurationManager.AppSettings["serverIP"]));
            
            InitializeComponent();
            DataContext = new mainViewModel();// listServices();
            //listServices();
            //listProcess();
            
           
        }

        
        
        //public List<GlobalAPI.WindowsServices> listServices()
        //{
        //    var r = apiHandler.gChannel.GetServiceState();
        //    this.Services.ItemsSource = r;
        //    return r;
        //}




        

        private void RestartService(object sender, RoutedEventArgs e)
        {
            //todo : call the api to restart the service
            var process = ((FrameworkElement)sender).DataContext as GlobalAPI.WindowsServices;
            bool restarted = apiHandler.gChannel.PostRestartService(process.serviceName);

        }



        private void ExecuteSQL(object sender, RoutedEventArgs e)
        {
            int totalrecord = 0;
            string secToken = acl.ACLs.genSecToken();
            totalrecord = apiHandler.gCRESChannel.UpdateQueues(this.txtSqlStmt.Text , secToken);
            MessageBox.Show(string.Format("Total records affected : {0}",totalrecord));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EBBSQueue(object sender, RoutedEventArgs e)
        {
            StatusSelection.Children.Clear();
            CheckBox chk91 = new CheckBox();
            chk91.Content="91";
            this.StatusSelection.Children.Add(chk91);
            Grid.SetColumn(chk91, 0);

            CheckBox chk92 = new CheckBox();
            chk92.Content = "92";
            this.StatusSelection.Children.Add(chk92);
            Grid.SetColumn(chk92, 1);

            CheckBox chk93 = new CheckBox();
            chk93.Content = "93";
            this.StatusSelection.Children.Add(chk93);
            Grid.SetColumn(chk93, 2);

            CheckBox chk94 = new CheckBox();
            chk94.Content = "94";
            this.StatusSelection.Children.Add(chk94);
            Grid.SetColumn(chk94, 3);

            CheckBox chk90 = new CheckBox();
            chk90.Content = "90";
            this.StatusSelection.Children.Add(chk90);
            Grid.SetColumn(chk90, 4);

            CheckBox chk99 = new CheckBox();
            chk99.Content = "99";
            this.StatusSelection.Children.Add(chk99);
            Grid.SetColumn(chk99, 5);

            Button GenerateSQL = new Button();
            GenerateSQL.Click +=GenerateSQL_Click;
            GenerateSQL.Content = "Generate SQL statement";
            this.StatusSelection.Children.Add(GenerateSQL);
            Grid.SetColumn(GenerateSQL, 6);
        }

        private void GenerateSQL_Click(object sender, RoutedEventArgs e)
        {
            string status ="";
            foreach (Control ctrl in this.StatusSelection.Children)
            {
                if (ctrl.GetType() == typeof(CheckBox))
                {
                    CheckBox x = (CheckBox)ctrl;
                    if ((bool)x.IsChecked)
                    {
                        status = status + string.Join(",", x.Content);
                     //   status = status + x.Content + ",";
                    }
                }
            }
            //status = status.Remove(status.Length - 1);
            this.txtSqlStmt.Text = string.Format("UPDATE HSTENQUIRY_QUEUE SET HST_EBBS_FLAG = NULL WHERE LN_CASE_NO = {0} AND HST_EBBS_FLAG='ERR' AND HST_EBBS_FLAG IN ({1})", txtSQLCase.Text, status); 
        }


    }
}
