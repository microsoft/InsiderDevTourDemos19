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
        private Connector connector = new Connector();

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            var name = await connector.GetUserNameAsync();

            labelUserName.Text = "User: " + name;

            var events = await connector.GetCalendarEventsAsync();

            listViewCalendar.Items.AddRange(events.Select(ev =>
            {
                var lvi = new ListViewItem(ev.Subject);
                lvi.SubItems.Add(ev.Start.ToDateTimeOffset().LocalDateTime.ToString("g"));
                lvi.SubItems.Add(ev.End.ToDateTimeOffset().LocalDateTime.ToString("g"));
                return lvi;
            }).ToArray());
        }

        private void labelUserName_Click(object sender, EventArgs e)
        {
            connector.LogoutAsync();

            // Restore logged out state.
            labelUserName.Text = "Not Logged In";
            listViewCalendar.Items.Clear();
        }
    }
}
