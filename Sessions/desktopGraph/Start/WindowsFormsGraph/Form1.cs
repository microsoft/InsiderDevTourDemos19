using MyGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsGraph
{
    public partial class Form1 : Form
    {
        // TODO: Connect to Graph

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            // TODO: Initialize UI Component's Data From Graph
        }

        private void labelUserName_Click(object sender, EventArgs e)
        {
            // TODO: Collapse Me Before Demo
            connector.LogoutAsync();

            // Restore logged out state.
            labelUserName.Text = "Not Logged In";
            listViewCalendar.Items.Clear();
        }
    }
}
