using System;
using System.Windows.Forms;

namespace GranitEditor
{
  public class EnumStripMenu<T> where T : struct, IConvertible
  {

    protected ToolStripMenuItem parentMenuItem;
    public delegate void ClickedHandler(T enumItem);
    public virtual ToolStripItemCollection MenuItems => parentMenuItem.DropDownItems;
    public EnumStripMenuItem<T> CheckedMenuItem;
    private readonly ClickedHandler clickedHandler;

    public EnumStripMenu(ToolStripMenuItem parentMenuItem, ClickedHandler clickedHandler)
    {
      this.parentMenuItem = parentMenuItem ?? throw new ArgumentNullException(nameof(parentMenuItem));
      this.parentMenuItem.Checked = false;
      this.parentMenuItem.Enabled = true;

      this.clickedHandler = clickedHandler;
      foreach (T enumValue in Enum.GetValues(typeof(T)))
      {
        //if (item == 0) continue;
        EnumStripMenuItem<T> menuItem = new EnumStripMenuItem<T>(enumValue, new System.EventHandler(OnClick));
        MenuItems.Insert(0, menuItem);
      }
    }

    private void OnClick(object sender, EventArgs e)
    {
      EnumStripMenuItem<T> menuItem = (EnumStripMenuItem<T>)sender;
      clickedHandler((T)menuItem.Tag);
      ClearAllCheckedState();
      CheckedMenuItem = menuItem;
      menuItem.Checked = true;
    }

    private void ClearAllCheckedState()
    {
      foreach (EnumStripMenuItem<T> item in MenuItems)
      {
        item.Checked = false;
      }
    }

    public void SetCheckedByValue(T value)
    {
      ClearAllCheckedState();
      foreach (EnumStripMenuItem<T> item in MenuItems)
      {
        if ( value.ToString() == item.Tag.ToString() )
        {
          item.Checked = true;
          CheckedMenuItem = item;
          break;
        }
      }
    }
  }

  public class EnumStripMenuItem<T> : ToolStripMenuItem where T : struct, IConvertible 
  {
    public EnumStripMenuItem()
    {
      if (!typeof(T).IsEnum)
        throw new ArgumentException("T must be an enumerated type");

      Tag = "";
    }

    public EnumStripMenuItem(T enumValue, EventHandler clickEventHandler)
    {
      if (!typeof(T).IsEnum)
        throw new ArgumentException("T must be an enumerated type");

      Text = GetTextOfEnumValue(enumValue);
      Tag = enumValue;
      //ToolTipText = text;
      Click += clickEventHandler;
    }

    public virtual string GetTextOfEnumValue(T tag)
    {
      //TODO usage of text resources 
      return tag.ToString();
    }

    public T GetEnumValueFromText(string value)
    {
         return (T)Enum.Parse(typeof(T), value);
    }
  }
}
