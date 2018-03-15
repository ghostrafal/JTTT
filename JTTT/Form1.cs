using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Diagnostics;
using System.Threading;
namespace JTTT
{
    public partial class Form1 : Form
    {
        public class HtmlSample
        {
            private readonly string _url;

            public HtmlSample(string url)
            {
                this._url = url;
            }

            //Metoda zwraca zawartość HTML podanej strony www
            public string GetPageHtml()
            {
                var wc = new WebClient();
                
                    wc.Encoding = Encoding.UTF8;
                    var html = System.Net.WebUtility.HtmlDecode(wc.DownloadString(_url));
                    return html;
                
            }
            //Nu tu ugolnie zapisuje obrazek z url xd
            public string Save(string uri, string nameFile){
                WebClient wC = new WebClient();
                
                    wC.DownloadFile(uri, nameFile);
                    return nameFile;
            }
            //stworzenie i wyslanie emaila
            public void CreatEmail(string nameFile ,string adressfrom,string passfrom, string adressto)
            {
                string file = nameFile;
                //wyslanie email od nadawcy do odbiorcy
                MailMessage mess = new MailMessage(adressfrom, adressto);

                //temat, tresc 
                mess.Subject = "Dobry memuch";
                mess.Body = "smich";
                mess.IsBodyHtml = false;
                
                //dodaje zalocznik typu .jpg
                Attachment a = new Attachment(nameFile, System.Net.Mime.MediaTypeNames.Image.Jpeg);
                mess.Attachments.Add(a);
               
                //klient logowania i dane do konta z ktoregomamy wyslac
                SmtpClient post = new SmtpClient();
                post.Credentials = new NetworkCredential(adressfrom, passfrom);
                post.Host = "smtp.wp.pl";

                post.Send(mess);

            }
        }
        //Funckja obsluguje historie wykonania akcji.
        public void New_Log(string lines)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("jttt_logger.txt", true);
            file.WriteLine(lines);
            file.Close();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Pobieranie z okien wartosc
            string url = textBox1.Text.ToString();
            string tekst = textBox2.Text.ToString();
            string emial = textBox3.Text.ToString();

            //Sprawdzenie czy string jest pusty
            if (String.IsNullOrEmpty(url))
            {
                MessageBox.Show("Uzupelnij pole URL!");
            }
            else if (String.IsNullOrEmpty(tekst))
            {
                MessageBox.Show("Uzupelnij pole Tekst!");
            }
            else if (String.IsNullOrEmpty(emial))
            {
                MessageBox.Show("Uzupelnij pole E-mail!");
            }
            //Jezeli podano wszystkie wartosc
            else
            {
                var doc = new HtmlAgilityPack.HtmlDocument();
                HtmlSample docHtml = new HtmlSample(url);

                //Pobieranie zawartosci strony
                var pageHtml = docHtml.GetPageHtml();

                //Zaladowanie zawartosci strony do obiektu HtmlAgilityPack
                doc.LoadHtml(pageHtml);

                // Metoda Descendants pozwala wybrać zestaw node'ów o określonej nazwie
                var nodes = doc.DocumentNode.Descendants("img");

                //Przeszukuje wszystkie node
                foreach (var node in nodes)
                {
                    //Jeżeli znalazł obrazek z nazwą podaną w polu Tekst
                    if (node.GetAttributeValue("alt", "").Contains(tekst))
                    {
                        string scr = node.GetAttributeValue("src", "");
                        string filename;
                        New_Log(", zrodlo: " + scr + ", emial: " + emial);
                        MessageBox.Show(scr);
                        filename=docHtml.Save(scr, "hehe.jpg");
                        docHtml.CreatEmail(filename, "sznur3@wp.pl", "dupadupa", emial);
                       
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
