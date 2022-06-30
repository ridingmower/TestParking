using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kztek.Access.LoaderApp.CustomControl
{
    class PagingGridView : DataGridView
    {
        public int PageSize { get; set; } = 10;

        public DataTable originalSource { get; set; }

        BindingSource bs;
        BindingList<DataTable> tables;
        public void SetPagedDataSource(DataTable dataTable, BindingNavigator bnav)
        {
            bs = new BindingSource();
            tables = new BindingList<DataTable>();
            originalSource = dataTable;

            DataTable dt = null;
            int counter = 1;
            foreach (DataRow dr in originalSource.Rows)
            {
                if (counter == 1)
                {
                    dt = originalSource.Clone();
                    tables.Add(dt);
                }
                dt.Rows.Add(dr.ItemArray);
                if (PageSize < ++counter)
                {
                    counter = 1;
                }
            }
            bnav.BindingSource = bs;
            bs.DataSource = tables;
            bs.PositionChanged += bs_PositionChanged;
            bs_PositionChanged(bs, EventArgs.Empty);
        }
        void bs_PositionChanged(object sender, EventArgs e)
        {
            if (bs.Position > -1)
            {
                this.DataSource = tables[bs.Position];
            }
            else
            {
                this.DataSource = originalSource;
            }
        }
    }
}
