using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.ServiceModel;
using System.Configuration;
namespace PSSClient
{
    public partial class InterfaceFile : Form
    {
        public InterfaceFile()
        {
            InitializeComponent();
        }
        private List<iFileConfig> ifileList = new List<iFileConfig>();
        private void InterfaceFile_Load(object sender, EventArgs e)
        {
            
            ifileList.Add(new iFileConfig { serverName = "CRESMY MORT", system = "EBBS", path = "e:\\ebbs\\done" });
            ifileList.Add(new iFileConfig { serverName = "CRESMY MORT", system = "ICM", path = "e:\\ebbs\\done" });
            ifileList.Add(new iFileConfig { serverName = "CRESMY CCPL", system = "EBBS", path = "e:\\ebbs\\done" });
            ifileList.Add(new iFileConfig { serverName = "CRESMY CCPL", system = "CCMS", path = "e:\\ebbs\\done" });

            var _serverList = ifileList.Select(x => x.serverName).Distinct();

            this.cmbServer.DataSource = _serverList.ToList();
            this.cmbServer.Refresh();
        }

        private void cmbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ifileList.Count<1)
            {
                return;
            }
            var _sysList = ifileList.Where(y => y.serverName == this.cmbServer.SelectedValue.ToString()).Select(x => x.system);
            this.cmbSystem.DataSource = _sysList.ToList();
            this.cmbSystem.Refresh();
        }

        private void cmbSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _path = ifileList.Where(x => x.serverName == cmbServer.SelectedItem.ToString() && x.system == cmbSystem.SelectedItem.ToString()).Select(x => x.path).ToList();
            this.lblPath.Text = _path[0];
        }

        private void btngetFile_Click(object sender, EventArgs e)
        {
            try
            {
                BasicHttpBinding CRESbinding = new BasicHttpBinding();
                CRESbinding.Security.Mode = BasicHttpSecurityMode.None;
                CRESbinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                CRESbinding.MaxReceivedMessageSize = 2000000;
                EndpointAddress CRESaddress = new EndpointAddress(string.Format(ConfigurationManager.AppSettings["CRESapiUri"], ConfigurationManager.AppSettings["serverIP"]));

                ChannelFactory<CRESapi.ICRESapi> CRESApiFac = new ChannelFactory<CRESapi.ICRESapi>(CRESbinding, CRESaddress);
                var CRESServiceContract = CRESApiFac.CreateChannel();
                int stop = 0;
                string caseno = this.txtCaseNo.Text;
                List<CRESapi.interfaceFiles> Results = new List<CRESapi.interfaceFiles>();
                //while (stop == 0)
                //{
                    //Console.WriteLine("Enter case number or enter to end :");
                    //caseno = Console.ReadLine();
                    //if (caseno == "")
                    //{
                    //    stop = 1;
                    //    break;
                    //}
                    string secToken = ACLs.genSecToken();
                    Results = CRESServiceContract.GetInterFaceFile(caseno, "91,92,93,94,95,96,97", lblPath.Text, secToken);
                    if (Results.Count > 0)
                    {
                        string destfolder = "c:\\xml\\";
                        if (!Directory.Exists(destfolder))
                        {
                            Directory.CreateDirectory(destfolder);
                        }
                        if (!Directory.Exists(Path.Combine(destfolder, caseno)))
                        {
                            Directory.CreateDirectory(Path.Combine(destfolder, caseno));
                        }
                        foreach (CRESapi.interfaceFiles ff in Results)
                        {

                            File.WriteAllBytes(Path.Combine(Path.Combine(destfolder, caseno), ff.fileName), ACLs.decryptedFile(ff.encFile, secToken));
                        }
                        MessageBox.Show(string.Format("Total files copied : {0}", Results.Count));
                    }
                    else
                    {
                        
                        //Console.WriteLine("          _ ._  _ , _ ._");
                        //Console.WriteLine("        (_ ' ( `  )_  .__)");
                        //Console.WriteLine("      ( (  (    )   `)  ) _)");
                        //Console.WriteLine("     (__ (_   (_ . _) _) ,__)");
                        //Console.WriteLine("         `~~`\\ ' . /`~~`");
                        //Console.WriteLine("              ;   ;");
                        //Console.WriteLine("              /   \\");
                        //Console.WriteLine("_____________/_ __ \\_____________");
                    }
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
               // Console.ReadKey();
            }
        }
    }
}
