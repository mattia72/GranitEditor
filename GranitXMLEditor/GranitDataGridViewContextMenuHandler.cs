using GranitEditor.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace GranitEditor
{
  class GranitDataGridViewContextMenuHandler
  {
    private ContextMenuStrip _contextMenuStrip;
    private DataGridView _dataGridView;
    private GranitXmlToAdapterBinder _xmlToObject;
    private int? _currentMouseOverRow = null;

    public GranitDataGridViewContextMenuHandler(DataGridView dgv, ContextMenuStrip contextMenuStrip, GranitXmlToAdapterBinder xml2Obj)
    {
      _contextMenuStrip = contextMenuStrip;
      _contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(ContextMenuStrip_Opening);
      _dataGridView = dgv;
      _xmlToObject = xml2Obj;
    }

    private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      if (_dataGridView.SelectedRows.Count > 1)
      {
        EnableMenuItem("deleteRowToolStripMenuItem");
      }
    }

    public void EnableMenuItem(string name = null, bool value = true)
    {
      foreach (ToolStripItem item in _contextMenuStrip.Items)
      {
        if (item is ToolStripMenuItem )
        {
          if(name == null)
            item.Enabled = value;
          else if (item.Name == name )
            item.Enabled = value;
        }
      }
    }

    internal void grid_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        DataGridView.HitTestInfo hit = _dataGridView.HitTest(e.X, e.Y);
        if (hit.Type == DataGridViewHitTestType.Cell)
        {
          _currentMouseOverRow = hit.RowIndex;
          Debug.WriteLine("Mouse on row: " + _currentMouseOverRow);
          var currentRow = _dataGridView.Rows[(int)_currentMouseOverRow];

          if (_dataGridView.SelectedRows.Count == 0 || !_dataGridView.SelectedRows.Contains(currentRow))
          {
            _dataGridView.ClearSelection();
            currentRow.Cells[hit.ColumnIndex].Selected = true;
            //currentRow.Selected = true;
          }

          _contextMenuStrip.Show(_dataGridView, new Point(e.X, e.Y));
        }
      }
    }

    internal void Grid_DeleteSelectedRows(object sender, EventArgs e)
    {
      if (_dataGridView.SelectedRows.Count > 0)
        foreach (DataGridViewRow item in this._dataGridView.SelectedRows)
        {
          if (item.DataBoundItem != null)
            _dataGridView.Rows.Remove(item);
        }
      else
      {
        grid_DeleteActiveRow();
      }
      _currentMouseOverRow = null;
    }

    private void grid_DeleteActiveRow()
    {
      if (_currentMouseOverRow != null && _currentMouseOverRow > -1 
        && _currentMouseOverRow <= _dataGridView.RowCount - (_dataGridView.AllowUserToAddRows ? 2 : 1)) // last committed line
      {
        //Transaction t = (Transaction)_dataGridView.Rows[(int)_currentMouseOverRow].DataBoundItem;
        //if(t != null)
        //  _xmlToObject.History.Do(new RemoveTransactionMemento(t));
        _dataGridView.Rows.RemoveAt((int)_currentMouseOverRow);
      }
    }

    private long GetTransactionId(int rowIndex)
    {
      return ((TransactionAdapter)_dataGridView.Rows[rowIndex].DataBoundItem).TransactionId;
    }

    internal void grid_DuplicateRow(object sender, EventArgs e)
    {
      DataGridViewRow row = GetActiveRow();
      if (row != null)
      {
        TransactionAdapter ta = (TransactionAdapter)row.DataBoundItem;
        AddNewRow((TransactionAdapter)ta.Clone());
      }
    }

    private DataGridViewRow GetActiveRow()
    {
      DataGridViewRow row = null;

      if (_dataGridView.SelectedRows.Count == 1)
      {
        row = _dataGridView.SelectedRows[0];
      }
      else
      {
        if (_currentMouseOverRow != null && _currentMouseOverRow > -1)
          row = _dataGridView.Rows[(int)_currentMouseOverRow];
      }
      return row;
    }

    public void AddNewEmptyRow()
    {
        AddNewRow(null);
    }

    public void AddNewRow(TransactionAdapter ta)
    {
      var bindingList = ((SortableBindingList<TransactionAdapter>)_dataGridView.DataSource);

      if(ta == null || ta.GranitXDocument == null)
        bindingList.Add(_xmlToObject.AddEmptyTransactionRow());
      else
        bindingList.Add(_xmlToObject.AddTransactionRow(ta));

      SelectLastRow();
    }

    private void SelectLastRow()
    {
      _dataGridView.ClearSelection();//If you want

      int nRowIndex = _dataGridView.Rows.Count - ( _dataGridView.AllowUserToAddRows ? 2 : 1); // last uncommitted row
      int nColumnIndex = 0;

      _dataGridView.Rows[nRowIndex].Selected = true;
      _dataGridView.Rows[nRowIndex].Cells[nColumnIndex].Selected = true;

      //In case if you want to scroll down as well.
      _dataGridView.FirstDisplayedScrollingRowIndex = nRowIndex;
    }

    internal void grid_AddNewRow(object sender, EventArgs e)
    {
      DataGridViewRow row = GetActiveRow();

      if (row != null)
      {
        AddNewRow(new TransactionAdapter());
      }
    }
  }
}
