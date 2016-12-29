using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace GranitXMLEditor
{
    public partial class AboutBox : Form
    {
        // BackgroundWorker for the animation
        BackgroundWorker scroller = new BackgroundWorker();
        // If this event is signaled, the scrolling will stop
        ManualResetEvent scrollStop = new ManualResetEvent(true);
        // Closing the form should wait till the thread exits
        AutoResetEvent scrollThreadReady = new AutoResetEvent(false);

        public AboutBox()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;

            // Add the content of the ReadMe.txt into the TextBox
            string readMeFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReadMe.txt");
            if (File.Exists(readMeFile))
                textBoxDescription.Text = File.ReadAllText(readMeFile);

            // With mouse click you can start/stop the animation
            textBoxDescription.MouseClick += new MouseEventHandler(textBoxDescription_MouseClick);
            // Set scroller thread
            scroller.DoWork += new DoWorkEventHandler(scroller_DoWork);
            scroller.WorkerSupportsCancellation = true;
        }

        /// <summary>
        /// MouseClick on textbox stops/starts scrolling
        /// </summary>
        void textBoxDescription_MouseClick(object sender, EventArgs e)
        {
            if (scrollStop.WaitOne(0))
                scrollStop.Reset();
            else
                scrollStop.Set();
        }

        /// <summary>
        /// Scrolls a textbox by line up or down
        /// </summary>
        /// <param name="textBox">The textbox control</param>
        /// <param name="line">if negativ, scrolling will go up, else down</param>
        void ScrollByLine(TextBox textBox, int line)
        {
            int firstCharIndex = textBox.GetFirstCharIndexOfCurrentLine();
            int lineNumber = textBox.GetLineFromCharIndex(firstCharIndex);
            int lineCount = textBox.Lines.Length;

            lineNumber = lineNumber + line < 0 ?
              0 : (lineNumber + line >= lineCount ?
              lineCount - 1 : lineNumber + line);

            int charIndex = textBox.GetFirstCharIndexFromLine(lineNumber);

            // select the button, so selection in the textbox won't be visible 
            okButton.Select();

            // this will do the scrolling
            textBox.Select(0, charIndex);
            textBox.ScrollToCaret();
        }

        /// <summary>
        /// Scroller thread
        /// </summary>
        void scroller_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Debug.WriteLine("Scroll thread started.");

                int direction = 1;
                int line = -1;
                while (true)
                {
                    if (scroller.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    // If mouse clicked on the box, we'll wait until the next click.
                    scrollStop.WaitOne();

                    int maxLine = textBoxDescription.Lines.Length - 1;

                    if (line >= -1 && line <= maxLine) line = direction > 0 ? line + 1 : line - 1;

                    if (line == 0 && direction < 0) direction = 1;

                    if (line == maxLine && direction > 0) direction = -1;

                    BeginInvoke((MethodInvoker)(() => ScrollByLine(textBoxDescription, direction)));

                    if (!scroller.CancellationPending)
                    {
                        Thread.Sleep(300);
                    }
                }
            }
            finally
            {
                Debug.WriteLine("Scroll thread ended.");
                scrollThreadReady.Set();
            }
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutBox_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                scrollThreadReady.Reset();
                scroller.RunWorkerAsync();
            }
            else
            {
                scroller.CancelAsync();
                scrollStop.Set();
            }

        }

        private void AboutBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            scroller.CancelAsync();
            scrollStop.Set();
            Debug.WriteLine("Wait for ending scroll thread.");
            scrollThreadReady.WaitOne();
            Debug.WriteLine("Scroll thread ready event received.");
        }
    }
}
