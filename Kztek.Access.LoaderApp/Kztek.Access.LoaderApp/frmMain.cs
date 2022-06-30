using Kztek.Model.CustomModel.iAccess;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastMember;
using Kztek.Access.LoaderApp.CustomControl;
using Kztek.Model.CustomModel;
using System.Threading;
using Kztek.Access.LoaderApp.Model;

namespace Kztek.Access.LoaderApp
{
    public partial class frmMain : Form
    {
        DataAccess da = new DataAccess();
        List<AccessControllerAPI> lstController = new List<AccessControllerAPI>();
        List<SelfHostConfig> lstSelfhost = new List<SelfHostConfig>();
        DataTable lstCard = new DataTable();
        DataTable lstFinger = new DataTable();

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Hide();
            if (new frmLogin().ShowDialog() != DialogResult.OK)
            {
                Application.Exit();
                return;
            }
            this.Show();
            InitControl();
            GetControlList();
            GetSelectList();
            GetCardList();
            GetFingerList();
        }

        private void InitControl()
        {
            gridControl.DataSourceChanged += GridControl_DataSourceChanged;
        }

        private void GetControlList()
        {
            lstController = da.ListController();
            gridControl.Columns["colCheckbox"].ReadOnly = false;
            gridControl.DataSource = lstController;
        }

        private void GridControl_DataSourceChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridControl.Rows.Count; i++)
            {
                gridControl.Rows[i].Cells["colSTT"].Value = (i + 1).ToString();

                if (gridControl.Rows[i].Cells["colStatus"].Value.ToString() == "Connect")
                {
                    gridControl.Rows[i].Cells["colStatus"].Style.ForeColor = Color.Green;
                }
                else
                {
                    gridControl.Rows[i].Cells["colStatus"].Style.ForeColor = Color.Red;
                }

                gridControl.Rows[i].Cells["colCheckbox"].Value = "0";
            }

            gridControl.Refresh();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                btnRefresh.Enabled = false;
                GetControlList();
                GetSelectList();
                GetCardList();
                GetFingerList();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                btnRefresh.Enabled = true;
            }

        }

        private void GetSelectList()
        {
            var model = da.GetSelectList();

            cbPC.SetSelectList(model.dtComputer, "PCName", "PCID");
            cbCardGroup.SetSelectList(model.dtCardGroup, "CardGroupName", "CardGroupID");
            cbAccessCard.SetSelectList(model.dtAccessLevel, "AccessLevelName", "AccessLevelID");
            cbAccessFinger.SetSelectList(model.dtAccessLevel, "AccessLevelName", "AccessLevelID");
            cbCustomerGroupCard.SetSelectList(model.dtCustomerGroup, "ItemText", "ItemValue");
            cbCustomerGroupFinger.SetSelectList(model.dtCustomerGroup, "ItemText", "ItemValue");
        }

        private List<SelfHostConfig> GetSelectedHost()
        {
            List<SelfHostConfig> result = new List<SelfHostConfig>();

            List<string> listLineID = new List<string>();

            foreach (DataGridViewRow row in gridControl.Rows)
            {
                var chk = row.Cells["colCheckbox"] as DataGridViewCheckBoxCell;

                if (chk.Value.ToString() == "1")
                {
                    var LineID = row.Cells["colLineID"].Value.ToString();
                    listLineID.Add(LineID);
                }
            }

            if (listLineID.Any())
            {
                result = da.ListSelfHost(listLineID);
            }

            return result;
        }

        private List<AccessControllerAPI> GetSelectedController()
        {
            List<AccessControllerAPI> result = new List<AccessControllerAPI>();

            foreach (DataGridViewRow row in gridControl.Rows)
            {
                var chk = row.Cells["colCheckbox"] as DataGridViewCheckBoxCell;

                AccessControllerAPI obj = new AccessControllerAPI();

                if (chk.Value.ToString() == "1")
                {
                    obj.ControllerID = row.Cells["colControllerID"].Value.ToString();
                    obj.Status = row.Cells["colStatus"].Value.ToString();
                    obj.LineID = row.Cells["colLineID"].Value.ToString();
                    result.Add(obj);
                }
            }

            return result;
        }

        private void GetCardList(string key = "", string cardgroups = "", string customergroupid = "", string accesslevelids = "")
        {
            lstCard = new DataTable();

            var _lstCard = da.GetCardList(key, cardgroups, customergroupid, accesslevelids);

            using (var reader = ObjectReader.Create(_lstCard, "CardID", "CardGroupId", "CardGroupName", "CardNo", "CardNumber",  "CustomerGroupName", "AccessLevelName"))
            {
                lstCard.Load(reader);
            }

            gridCard.PageSize = 20;

            gridCard.SetPagedDataSource(lstCard, naviCard);
        }

        private void GetFingerList(string key = "", string customergoupid = "", string accesslevelids = "")
        {
            lstFinger = new DataTable();

            var _lstFinger = da.GetCustomerList(key, customergoupid, accesslevelids);

            using (var reader = ObjectReader.Create(_lstFinger, "CustomerID", "CustomerCode", "CustomerName", "CustomerGroupName"))
            {
                lstFinger.Load(reader);
            }

            gridFinger.PageSize = 20;
            gridFinger.SetPagedDataSource(lstFinger, naviFinger);            
        }

        private void btnSearchCard_Click(object sender, EventArgs e)
        {
            GetCardList($"{txtSearchCard.Text}", $"{cbCardGroup.SelectedValue}", $"{cbCustomerGroupCard.SelectedValue}", $"{cbAccessCard.SelectedValue}");
        }


        private void btnSearchFinger_Click(object sender, EventArgs e)
        {
            GetFingerList($"{txtSearchFinger.Text}", $"{cbCustomerGroupFinger.SelectedValue}", $"{cbAccessFinger.SelectedValue}");
        }

        private void btnInsertCard_Click(object sender, EventArgs e)
        {
            ActionClick(SendDataCard, "ADD", false);
        }

        private void btnInsertAllCard_Click(object sender, EventArgs e)
        {
            ActionClick(SendDataCard, "ADD", true);
        }

        private void btnDeleteCard_Click(object sender, EventArgs e)
        {
            ActionClick(SendDataCard, "DELETE", false);
        }

        private void btnDeleteAllCard_Click(object sender, EventArgs e)
        {
            ActionClick(SendDataCard, "DELETE", true);
        }

        private void btnInsertFinger_Click(object sender, EventArgs e)
        {
            ActionClick(SendDataFinger, "ADD", false);
        }

        private void btnInsertAllFinger_Click(object sender, EventArgs e)
        {
            ActionClick(SendDataFinger, "ADD", true);
        }

        private void btnDeleteFinger_Click(object sender, EventArgs e)
        {
            ActionClick(SendDataFinger, "DELETE", false);
        }

        private void btnDeleteAllFinger_Click(object sender, EventArgs e)
        {
            ActionClick(SendDataFinger, "DELETE", true);
        }

        private List<string> GetDataFromSelectedRows(PagingGridView grid, string columnName, bool isGetAll = false)
        {
            List<string> result = new List<string>();

            if (isGetAll)
            {
                foreach (DataRow row in grid.originalSource.Rows)
                {
                    var data = row[columnName].ToString();

                    result.Add(data);
                }
            }
            else
            {
                foreach (DataGridViewRow row in grid.SelectedRows)
                {
                    var data = row.Cells[columnName].Value.ToString();

                    result.Add(data);
                }
            }

            return result;
        }

        private List<string> GetSelectedCard()
        {
            return GetDataFromSelectedRows(gridCard, "colCardID");
        }

        private List<string> GetSelectedFinger()
        {
            return GetDataFromSelectedRows(gridFinger, "colCustomerID");
        }

        private void SendDataCard(string type, bool isAll)
        {
            var objUpload = new CardUploadAPI();
            objUpload.ListSelfHost = GetSelectedHost();
            objUpload.ListController = GetSelectedController().ToListController();
            objUpload.ListFilter = new SelectListModelCardUpload() { isall = isAll };

            if (isAll)
            {
                objUpload.ListFilter.accesslevelids = cbAccessCard.SelectedValue.ToString();
                objUpload.ListFilter.cardgroupids = cbCardGroup.SelectedValue.ToString();
                objUpload.ListFilter.customergroupid = cbCustomerGroupCard.SelectedValue.ToString();
                objUpload.ListFilter.key = txtSearchCard.Text;
                if (chkUseCard.Checked)
                {
                    objUpload.ListFilter.isusenewdate = true;
                    objUpload.ListFilter.newdateexpire = dtpExpireCard.Value.ToString("yyyy-MM-dd");
                }
                else
                {
                    objUpload.ListFilter.isusenewdate = false;
                }
            }
            else
            {
                objUpload.ListCardId = GetSelectedCard();
            }

            objUpload.CurrentUser = FunctionHelper.CurrentUser;

            var lstUpload = da.GetListCardWantToUse(objUpload);

            pbCard.Invoke(new Action(() =>
            {
                pbCard.Maximum = lstUpload.ListEmployee.Count;
                pbCard.Step = 1;
                pbCard.Value = 0;
            }));

            foreach (var employee in lstUpload.ListEmployee)
            {
                foreach (var host in lstUpload.ListSelfHost)
                {
                    if (type == "ADD")
                    {
                        var result = da.SendUpload(employee, host.Address);
                        WriteLog(result);
                    }
                    else
                    {
                        var result = da.SendDelete(employee, host.Address);
                        WriteLog(result);
                    }
                }

                pbCard.Invoke(new Action(() => { pbCard.PerformStep(); }));
                Thread.Sleep(100);
            }
        }

        private void SendDataFinger(string type, bool isAll)
        {
            var objUpload = new CardUploadAPI();
            objUpload.ListSelfHost = GetSelectedHost();
            objUpload.ListController = GetSelectedController().ToListController();
            objUpload.ListFilter = new SelectListModelCardUpload() { isall = isAll };

            if (isAll)
            {
                objUpload.ListFilter.accesslevelids = cbAccessFinger.SelectedValue.ToString();

                objUpload.ListFilter.customergroupid = cbCustomerGroupFinger.SelectedValue.ToString();

                objUpload.ListFilter.key = txtSearchFinger.Text;

                if (chkUseFinger.Checked)
                {
                    objUpload.ListFilter.isusenewdate = true;
                    objUpload.ListFilter.newdateexpire = dtpExpireFinger.Value.ToString("yyyy-MM-dd");
                }
                else
                {
                    objUpload.ListFilter.isusenewdate = false;
                }
            }
            else
            {
                objUpload.ListCustomerId = GetSelectedFinger();
            }

            objUpload.CurrentUser = FunctionHelper.CurrentUser;

            var lstUpload = da.GetListCustomerWantToUse(objUpload);

            pbFinger.Invoke(new Action(() =>
            {
                pbFinger.Maximum = lstUpload.ListEmployee.Count;
                pbFinger.Step = 1;
                pbFinger.Value = 0;
            }));

            foreach (var employee in lstUpload.ListEmployee)
            {
                foreach (var host in lstUpload.ListSelfHost)
                {
                    if (type == "ADD")
                    {
                        var result = da.SendUpload(employee, host.Address);
                        WriteLog(result);
                    }
                    else
                    {
                        var result = da.SendDelete(employee, host.Address);
                        WriteLog(result);
                    }
                }

                pbFinger.Invoke(new Action(() => { pbFinger.PerformStep(); }));
                Thread.Sleep(100);
            }
        }

        private void SetButtonsState(bool buttonState)
        {
            btnInsertCard.Enabled = buttonState;
            btnInsertAllCard.Enabled = buttonState;
            btnInsertCard.Enabled = buttonState;
            btnInsertAllCard.Enabled = buttonState;
            btnDeleteCard.Enabled = buttonState;
            btnDeleteAllCard.Enabled = buttonState;
            btnDeleteFinger.Enabled = buttonState;
            btnDeleteAllFinger.Enabled = buttonState;
        }

        private delegate void DataAction(string ActionType, bool isAll);

        private void ActionClick(DataAction DataAction, string ActionType, bool isAll)
        {
            try
            {
                SetButtonsState(false);

                DataAction(ActionType, isAll);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                SetButtonsState(true);
            }
        }

        public void WriteLog(ReportResult obj)
        {
            if(obj.Success == true)
            {
                lbNLog.Items.Add(obj.Message);
            }
            else
            {
                lbELog.Items.Add(obj.Message);
            }
        }
    }
}
