using System;
using System.Windows.Forms;

namespace PoorMensAboutBox
{
    public partial class Form1 : Form
    {
        AboutBox ab = new AboutBox();        

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {   

            if(ab.IsDisposed)
                ab = new AboutBox();

            if (!ab.Visible)
            {
                ab.Show();
                ab.BringToFront();
            }
            else
            {
                ab.Hide();
            }
        }

    }
}
