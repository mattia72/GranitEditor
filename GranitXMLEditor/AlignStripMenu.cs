using System;
using System.Windows.Forms;

namespace GranitXMLEditor
{
  class AutoSizeModeStripMenu
  {

    protected ToolStripMenuItem alignMenuItem;
    public delegate void ClickedHandler(DataGridViewAutoSizeColumnsMode mode);

    public virtual ToolStripItemCollection MenuItems
    {
      get
      {
        return alignMenuItem.DropDownItems;
      }
    }

    private ClickedHandler clickedHandler;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AutoSizeModeStripMenu(ToolStripMenuItem alignMenuItem, ClickedHandler clickedHandler)
    {
      if (alignMenuItem == null)
        throw new ArgumentNullException("alignMenuItem");

      this.alignMenuItem = alignMenuItem;
      this.alignMenuItem.Checked = false;
      this.alignMenuItem.Enabled = true;

      this.clickedHandler = clickedHandler;
      foreach (DataGridViewAutoSizeColumnMode item in Enum.GetValues(typeof(DataGridViewAutoSizeColumnMode)))
      {
        if (item == 0) continue;
        AutoSizeModeMenuItem menuItem = new AutoSizeModeMenuItem(item, new System.EventHandler(OnClick));
        MenuItems.Insert(0, menuItem);
      }

    }

    private void OnClick(object sender, EventArgs e)
    {
      AutoSizeModeMenuItem menuItem = (AutoSizeModeMenuItem)sender;
      clickedHandler((DataGridViewAutoSizeColumnsMode)menuItem.Tag);
      ClearAllCheckedState();
      menuItem.Checked = true;
    }

    private void ClearAllCheckedState()
    {
      foreach (AutoSizeModeMenuItem item in MenuItems)
      {
        item.Checked = false;
      }
    }

    public void SetCheckedByMode(DataGridViewAutoSizeColumnsMode mode)
    {
      ClearAllCheckedState();
      foreach (AutoSizeModeMenuItem item in MenuItems)
      {
        if ((DataGridViewAutoSizeColumnsMode)item.Tag == mode)
        {
          item.Checked = true;
          break;
        }
      }

    }
  }



  public class AutoSizeModeMenuItem : ToolStripMenuItem
  {
    public AutoSizeModeMenuItem()
    {
      Tag = "";
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public AutoSizeModeMenuItem(DataGridViewAutoSizeColumnMode tag, EventHandler eventHandler)
    {
      Text = GetTextOfMode(tag);
      Tag = tag;
      //ToolTipText = text;
      Click += eventHandler;
    }

    private string GetTextOfMode(DataGridViewAutoSizeColumnMode tag)
    {
      //TODO usage of text resources 
      return tag.ToString();
    }
  }

}
