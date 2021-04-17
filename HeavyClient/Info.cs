using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Routing.JSONClasses.Geo;

namespace HeavyClient
{
    public partial class Info : Form
    {
        List<GeoJson> geos;
        public Info(List<GeoJson> data)
        {
            this.geos = data;
            InitializeComponent();
        }

        private void Info_Load(object sender, EventArgs e)
        {
            test.Text = this.geos[0].features[0].type;
        }
    }
}
