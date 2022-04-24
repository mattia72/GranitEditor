using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace GranitEditor
{
    public partial class AboutBox : Form
  {
    private const string programHomeUrl = @"https://github.com/mattia72/GranitEditor";

    // BackgroundWorker for the animation
    BackgroundWorker scroller = new BackgroundWorker();
    // If this event is signaled, the scrolling will stop
    ManualResetEvent scrollStop = new ManualResetEvent(true);
    // Closing the form should wait till the thread exits
    AutoResetEvent scrollThreadReady = new AutoResetEvent(false);

    public AboutBox()
    {
      InitializeComponent();
      Text = $"About {AssemblyTitle}";
      labelProductNameText.Text = AssemblyProduct;
      labelVersionText.Text = AssemblyVersion;
      labelCopyright.Text = Regex.Replace(AssemblyCopyright, @"(.*©).*", "$1"); //
      labelCopyrightText.Text = Regex.Replace(AssemblyCopyright, @".*© (.*)", "$1");
      labelBuildDateTimeText.Text = AssemblyBuildDateTime.ToString("f", CultureInfo.InvariantCulture);
#pragma warning disable CA1303 // Do not pass literals as localized parameters
      linkHomePage.Text = programHomeUrl;
#pragma warning restore CA1303 // Do not pass literals as localized parameters

      // Add the content of the ReadMe.txt into the TextBox
      string readMeFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LICENSE");
      if (File.Exists(readMeFile))
        textBoxDescription.Text = File.ReadAllText(readMeFile).Replace("\n", Environment.NewLine);

      // With mouse click you can start/stop the animation
      textBoxDescription.MouseClick += new MouseEventHandler(TextBoxDescription_MouseClick);
      // Set scroller thread
      scroller.DoWork += new DoWorkEventHandler(Scroller_DoWork);
      scroller.WorkerSupportsCancellation = true;
    }

    /// <summary>
    /// MouseClick on textbox stops/starts scrolling
    /// </summary>
    void TextBoxDescription_MouseClick(object sender, EventArgs e)
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
    void Scroller_DoWork(object sender, DoWorkEventArgs e)
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

    public static string AssemblyTitle
    {
      get
      {
        object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
        if (attributes.Length > 0)
        {
          AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
          if (!string.IsNullOrEmpty(titleAttribute.Title))
          {
            return titleAttribute.Title;
          }
        }
        return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
      }
    }

    public static string AssemblyVersion
    {
      get
      {
        //return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        var assembly = Assembly.GetExecutingAssembly();
        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        return fvi.FileVersion;
      }
    }

    public static string AssemblyDescription
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

    public static string AssemblyProduct
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

    public static DateTime AssemblyBuildDateTime
    {
      get
      {
        //In AssemblyInfo.cs: [assembly: AssemblyVersion("1.0.*")]
        var version = Assembly.GetEntryAssembly().GetName().Version;
        var buildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(
        TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
        TimeSpan.TicksPerSecond * 2 * version.Revision)); /* seconds since midnight,(multiply by 2 to get original) */
        return buildDateTime;
      }
    }

    public static string AssemblyCopyright
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

    public static string AssemblyCompany
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

    private void OkButton_Click(object sender, EventArgs e)
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

    private void LogoPictureBox_Click(object sender, EventArgs e)
    {
      // Navigate to a URL.
      Process.Start(linkHomePage.Text);
    }

    private void LinkHomePage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      // Specify that the link was visited.
      this.linkHomePage.LinkVisited = true;
      // Navigate to a URL.
      Process.Start(linkHomePage.Text);
    }
  }
}
