using System;
using System.Windows.Forms;

namespace GranitXMLEditor
{
  public class EnumStripMenu<T> where T : struct, IConvertible
  {

    protected ToolStripMenuItem parentMenuItem;
    public delegate void ClickedHandler(T enumItem);

    public virtual ToolStripItemCollection MenuItems
    {
      get
      {
        return parentMenuItem.DropDownItems;
      }
    }

    private ClickedHandler clickedHandler;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public EnumStripMenu(ToolStripMenuItem parentMenuItem, ClickedHandler clickedHandler)
    {
      if (parentMenuItem == null)
        throw new ArgumentNullException("parentMenuItem");

      this.parentMenuItem = parentMenuItem;
      this.parentMenuItem.Checked = false;
      this.parentMenuItem.Enabled = true;

      this.clickedHandler = clickedHandler;
      foreach (T item in Enum.GetValues(typeof(T)))
      {
        //if (item == 0) continue;
        EnumStripMenuItem<T> menuItem = new EnumStripMenuItem<T>(item, new System.EventHandler(OnClick));
        MenuItems.Insert(0, menuItem);
      }

    }

    private void OnClick(object sender, EventArgs e)
    {
      EnumStripMenuItem<T> menuItem = (EnumStripMenuItem<T>)sender;
      clickedHandler((T)menuItem.Tag);
      ClearAllCheckedState();
      menuItem.Checked = true;
    }

    private void ClearAllCheckedState()
    {
      foreach (EnumStripMenuItem<T> item in MenuItems)
      {
        item.Checked = false;
      }
    }

    public void SetCheckedByMode(T mode)
    {
      ClearAllCheckedState();
      foreach (EnumStripMenuItem<T> item in MenuItems)
      {
        if ( mode.ToString() == item.Tag.ToString() )
        {
          item.Checked = true;
          break;
        }
      }
    }
  }



  public class EnumStripMenuItem<T> : ToolStripMenuItem where T : struct, IConvertible 
  {
    public EnumStripMenuItem()
    {
      Tag = "";
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public EnumStripMenuItem(T tag, EventHandler eventHandler)
    {
      Text = GetTextOfEnumValue(tag);
      Tag = tag;
      //ToolTipText = text;
      Click += eventHandler;
    }

    public virtual string GetTextOfEnumValue(T tag)
    {
      //TODO usage of text resources 
      return tag.ToString();
    }
  }

}
