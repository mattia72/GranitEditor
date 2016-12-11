using GranitXMLTemplate;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Xml2CSharp;

namespace GranitXMLTemplate
{
    public partial class Form1 : Form
    {
        public HUFTransactions HUFTransactions { get; private set; }

        public Form1()
        {
            InitializeComponent();
            HUFTransactions = new HUFTransactions();
        }

        public void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                GranitXmlToObject xmlGen = new GranitXmlToObject(Path.GetFullPath(openFileDialog1.FileName));
                var list = new BindingList<TransactionAdapter>(xmlGen.HUFTransactionAdapter.Transactions);
                dataGridView1.DataSource = list;
            }
        }
    }
}
