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
    private int? _currentMouseOverRow = null;

    public GranitDataGridViewContextMenuHandler(DataGridView dgv, ContextMenuStrip contextMenuStrip)
    {
      this._contextMenuStrip = contextMenuStrip;
      _dataGridView = dgv;
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
        //TODO copy transaction
      }
      
    }
  }
}
