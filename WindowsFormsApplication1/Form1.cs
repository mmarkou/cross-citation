using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.IO;
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sUrl;
            XmlTextReader rssReader;
            XmlDocument rssDoc;
            string slink;

            SqlConnection myConnection = new SqlConnection("Database=wofra;Server=.;Password=1;Integrated Security=True;connect timeout = 30");
            try
            {
                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("select * from paper", myConnection);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    sUrl = "http://www.scopus.com/results/citedbyresults.url?sort=plf-f&cite=" + myReader["eid"].ToString().Substring(4) + "&src=s&imp=t&sid=6F6736F6CCBE873EC26BF849ACB5E065.WeLimyRvBMk2ky9SFKc8Q%3a20&sot=cite&sdt=a&sl=0&origin=recordpage&txGid=6F6736F6CCBE873EC26BF849ACB5E065.WeLimyRvBMk2ky9SFKc8Q%3a2";

                    string html = string.Empty;
                    using (WebClient client = new WebClient())
                    {
                        Uri innUri = null;
                        Uri.TryCreate(sUrl, UriKind.RelativeOrAbsolute, out innUri);

                        try
                        {
                            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                            //client.DownloadString(innUri);
                            using (StreamReader str = new StreamReader(client.OpenRead(innUri)))
                            {
                                html = str.ReadToEnd();
                            }
                        }
                        catch (WebException we)
                        {
                            throw we;
                        }
                       
                    }

                    System.IO.File.WriteAllText(@"C:\data\wofra\code\05cross_citations\html\" + myReader["ID"].ToString() + ".html", html);
                   
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("You failed!" + ex.Message);
            }




            //sUrl = "http://in.gr";
            

        
        }
    }
}
