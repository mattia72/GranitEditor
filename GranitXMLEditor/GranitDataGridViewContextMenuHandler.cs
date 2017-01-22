using GranitXMLEditor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GranitXMLEditor
{
  class GranitDataGridViewContextMenuHandler
  {
    private ContextMenuStrip _contextMenuStrip;
    private DataGridView _dataGridView;
    private GranitXmlToObjectBinder _xmlToObject;
    private int? _currentMouseOverRow = null;

    public GranitDataGridViewContextMenuHandler(DataGridView dgv, ContextMenuStrip contextMenuStrip, GranitXmlToObjectBinder xml2Obj)
    {
      _contextMenuStrip = contextMenuStrip;
      _dataGridView = dgv;
      _xmlToObject = xml2Obj;
    }
    internal void grid_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        _currentMouseOverRow = _dataGridView.HitTest(e.X, e.Y).RowIndex;
        _contextMenuStrip.Show(_dataGridView, new Point(e.X, e.Y));
      }
    }

    internal void grid_DeleteSelectedRows(object sender, EventArgs e)
    {
      if (_dataGridView.SelectedRows.Count > 0)
        foreach (DataGridViewRow item in this._dataGridView.SelectedRows)
        {
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
      if (_currentMouseOverRow != null && _currentMouseOverRow > -1)
        _dataGridView.Rows.RemoveAt((int)_currentMouseOverRow);
    }

    internal void grid_DuplicateRow(object sender, EventArgs e)
    {
      DataGridViewRow row = null;
      if (_currentMouseOverRow != null && _currentMouseOverRow > -1)
        row = _dataGridView.Rows[(int)_currentMouseOverRow];

      if (row != null)
      {
        TransactionAdapter ta = (TransactionAdapter)row.DataBoundItem;
        var bindingList = ((SortableBindingList<TransactionAdapter>)_dataGridView.DataSource);
        bindingList.Add(_xmlToObject.AddTransactionRow((TransactionAdapter)ta.Clone()));
        _dataGridView.CurrentCell = _dataGridView.Rows[_dataGridView.RowCount - 1].Cells[1];
      }
    }
  }
}
