using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HeavyClient.Routing;
using static Routing.JSONClasses.Geo;

namespace HeavyClient
{
    public partial class Main : Form
    {
        public static Service1Client routing;

        public Main()
        {
            InitializeComponent();
            routing = new Service1Client();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            var data = routing.GetGeoData(departureTextbox.Text, arrivalTextBox.Text);
            Info info = new Info(data.Cast<GeoJson>().ToList());
            
            info.ShowDialog();
        }

        private void departureTextbox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
