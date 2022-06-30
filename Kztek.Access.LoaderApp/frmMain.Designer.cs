namespace Kztek.Access.LoaderApp
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.gbError = new System.Windows.Forms.GroupBox();
            this.tabcontrolLog = new System.Windows.Forms.TabControl();
            this.tpELog = new System.Windows.Forms.TabPage();
            this.lbELog = new System.Windows.Forms.ListBox();
            this.tpNLog = new System.Windows.Forms.TabPage();
            this.lbNLog = new System.Windows.Forms.ListBox();
            this.gbList = new System.Windows.Forms.GroupBox();
            this.cbPC = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.naviControl = new System.Windows.Forms.BindingNavigator(this.components);
            this.naviControlAdd = new System.Windows.Forms.ToolStripButton();
            this.naviControlCount = new System.Windows.Forms.ToolStripLabel();
            this.naviControlDelete = new System.Windows.Forms.ToolStripButton();
            this.naviControlMoveFirst = new System.Windows.Forms.ToolStripButton();
            this.naviControlMovePrevious = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.naviControlPosition = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.naviControlMoveNext = new System.Windows.Forms.ToolStripButton();
            this.naviControlMoveLast = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.gridControl = new Kztek.Access.LoaderApp.CustomControl.PagingGridView();
            this.colControllerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colControllerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.gbAction = new System.Windows.Forms.GroupBox();
            this.tabcontrolAction = new System.Windows.Forms.TabControl();
            this.tpCard = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbAccessCard = new System.Windows.Forms.ComboBox();
            this.cbCardGroup = new System.Windows.Forms.ComboBox();
            this.cbCustomerGroupCard = new System.Windows.Forms.ComboBox();
            this.gridCard = new Kztek.Access.LoaderApp.CustomControl.PagingGridView();
            this.colCardID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCardNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCardGroupId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCardGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccessLevelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.naviCard = new System.Windows.Forms.BindingNavigator(this.components);
            this.naviCardAdd = new System.Windows.Forms.ToolStripButton();
            this.naviCardCount = new System.Windows.Forms.ToolStripLabel();
            this.naviCardDelete = new System.Windows.Forms.ToolStripButton();
            this.naviCardMoveFirst = new System.Windows.Forms.ToolStripButton();
            this.naviCardMovePrevious = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.naviCardPosition = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.naviCardMoveNext = new System.Windows.Forms.ToolStripButton();
            this.naviCardMoveLast = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSearchCard = new System.Windows.Forms.Button();
            this.txtSearchCard = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnCancelCard = new System.Windows.Forms.Button();
            this.pbCard = new System.Windows.Forms.ProgressBar();
            this.btnDeleteAllCard = new System.Windows.Forms.Button();
            this.btnInsertAllCard = new System.Windows.Forms.Button();
            this.btnDeleteCard = new System.Windows.Forms.Button();
            this.btnInsertCard = new System.Windows.Forms.Button();
            this.chkUseCard = new System.Windows.Forms.CheckBox();
            this.dtpExpireCard = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.tpFinger = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cbAccessFinger = new System.Windows.Forms.ComboBox();
            this.cbCustomerGroupFinger = new System.Windows.Forms.ComboBox();
            this.gridFinger = new Kztek.Access.LoaderApp.CustomControl.PagingGridView();
            this.colCustomerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerGroupName_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.naviFinger = new System.Windows.Forms.BindingNavigator(this.components);
            this.naviFingerAdd = new System.Windows.Forms.ToolStripButton();
            this.naviFingerCount = new System.Windows.Forms.ToolStripLabel();
            this.naviFingerDelete = new System.Windows.Forms.ToolStripButton();
            this.naviFingerMoveFirst = new System.Windows.Forms.ToolStripButton();
            this.naviFingerMovePrevious = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.naviFingerPosition = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.naviFingerMoveNext = new System.Windows.Forms.ToolStripButton();
            this.naviFingerMoveLast = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSearchFinger = new System.Windows.Forms.Button();
            this.txtSearchFinger = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnCancelFinger = new System.Windows.Forms.Button();
            this.pbFinger = new System.Windows.Forms.ProgressBar();
            this.chkUseFinger = new System.Windows.Forms.CheckBox();
            this.btnDeleteAllFinger = new System.Windows.Forms.Button();
            this.btnInsertAllFinger = new System.Windows.Forms.Button();
            this.btnDeleteFinger = new System.Windows.Forms.Button();
            this.btnInsertFinger = new System.Windows.Forms.Button();
            this.dtpExpireFinger = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.gbError.SuspendLayout();
            this.tabcontrolLog.SuspendLayout();
            this.tpELog.SuspendLayout();
            this.tpNLog.SuspendLayout();
            this.gbList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.naviControl)).BeginInit();
            this.naviControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            this.gbAction.SuspendLayout();
            this.tabcontrolAction.SuspendLayout();
            this.tpCard.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.naviCard)).BeginInit();
            this.naviCard.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tpFinger.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFinger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.naviFinger)).BeginInit();
            this.naviFinger.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.IsSplitterFixed = true;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.gbError);
            this.splitMain.Panel1.Controls.Add(this.gbList);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.gbAction);
            this.splitMain.Size = new System.Drawing.Size(1008, 729);
            this.splitMain.SplitterDistance = 336;
            this.splitMain.TabIndex = 0;
            // 
            // gbError
            // 
            this.gbError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbError.Controls.Add(this.tabcontrolLog);
            this.gbError.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbError.Location = new System.Drawing.Point(375, 3);
            this.gbError.Name = "gbError";
            this.gbError.Size = new System.Drawing.Size(621, 330);
            this.gbError.TabIndex = 2;
            this.gbError.TabStop = false;
            this.gbError.Text = "Log";
            // 
            // tabcontrolLog
            // 
            this.tabcontrolLog.Controls.Add(this.tpELog);
            this.tabcontrolLog.Controls.Add(this.tpNLog);
            this.tabcontrolLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabcontrolLog.Location = new System.Drawing.Point(3, 20);
            this.tabcontrolLog.Name = "tabcontrolLog";
            this.tabcontrolLog.SelectedIndex = 0;
            this.tabcontrolLog.Size = new System.Drawing.Size(615, 307);
            this.tabcontrolLog.TabIndex = 0;
            // 
            // tpELog
            // 
            this.tpELog.BackColor = System.Drawing.SystemColors.Control;
            this.tpELog.Controls.Add(this.lbELog);
            this.tpELog.Location = new System.Drawing.Point(4, 27);
            this.tpELog.Name = "tpELog";
            this.tpELog.Padding = new System.Windows.Forms.Padding(3);
            this.tpELog.Size = new System.Drawing.Size(607, 276);
            this.tpELog.TabIndex = 0;
            this.tpELog.Text = "Lỗi";
            // 
            // lbELog
            // 
            this.lbELog.BackColor = System.Drawing.SystemColors.Control;
            this.lbELog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbELog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbELog.FormattingEnabled = true;
            this.lbELog.ItemHeight = 18;
            this.lbELog.Location = new System.Drawing.Point(3, 3);
            this.lbELog.Name = "lbELog";
            this.lbELog.ScrollAlwaysVisible = true;
            this.lbELog.Size = new System.Drawing.Size(601, 270);
            this.lbELog.TabIndex = 0;
            // 
            // tpNLog
            // 
            this.tpNLog.BackColor = System.Drawing.SystemColors.Control;
            this.tpNLog.Controls.Add(this.lbNLog);
            this.tpNLog.Location = new System.Drawing.Point(4, 27);
            this.tpNLog.Name = "tpNLog";
            this.tpNLog.Padding = new System.Windows.Forms.Padding(3);
            this.tpNLog.Size = new System.Drawing.Size(607, 276);
            this.tpNLog.TabIndex = 1;
            this.tpNLog.Text = "Thành công";
            // 
            // lbNLog
            // 
            this.lbNLog.BackColor = System.Drawing.SystemColors.Control;
            this.lbNLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbNLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbNLog.FormattingEnabled = true;
            this.lbNLog.ItemHeight = 18;
            this.lbNLog.Location = new System.Drawing.Point(3, 3);
            this.lbNLog.Name = "lbNLog";
            this.lbNLog.ScrollAlwaysVisible = true;
            this.lbNLog.Size = new System.Drawing.Size(601, 270);
            this.lbNLog.TabIndex = 0;
            // 
            // gbList
            // 
            this.gbList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbList.Controls.Add(this.cbPC);
            this.gbList.Controls.Add(this.btnRefresh);
            this.gbList.Controls.Add(this.naviControl);
            this.gbList.Controls.Add(this.gridControl);
            this.gbList.Controls.Add(this.label1);
            this.gbList.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbList.Location = new System.Drawing.Point(12, 3);
            this.gbList.Name = "gbList";
            this.gbList.Size = new System.Drawing.Size(357, 330);
            this.gbList.TabIndex = 0;
            this.gbList.TabStop = false;
            this.gbList.Text = "Danh sách bộ điều khiển";
            // 
            // cbPC
            // 
            this.cbPC.FormattingEnabled = true;
            this.cbPC.Location = new System.Drawing.Point(83, 33);
            this.cbPC.Name = "cbPC";
            this.cbPC.Size = new System.Drawing.Size(234, 26);
            this.cbPC.TabIndex = 5;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(323, 33);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(21, 24);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "R";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // naviControl
            // 
            this.naviControl.AddNewItem = this.naviControlAdd;
            this.naviControl.CountItem = this.naviControlCount;
            this.naviControl.DeleteItem = this.naviControlDelete;
            this.naviControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.naviControl.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.naviControl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.naviControlMoveFirst,
            this.naviControlMovePrevious,
            this.bindingNavigatorSeparator,
            this.naviControlPosition,
            this.naviControlCount,
            this.bindingNavigatorSeparator1,
            this.naviControlMoveNext,
            this.naviControlMoveLast,
            this.bindingNavigatorSeparator2,
            this.naviControlAdd,
            this.naviControlDelete});
            this.naviControl.Location = new System.Drawing.Point(3, 302);
            this.naviControl.MoveFirstItem = this.naviControlMoveFirst;
            this.naviControl.MoveLastItem = this.naviControlMoveLast;
            this.naviControl.MoveNextItem = this.naviControlMoveNext;
            this.naviControl.MovePreviousItem = this.naviControlMovePrevious;
            this.naviControl.Name = "naviControl";
            this.naviControl.PositionItem = this.naviControlPosition;
            this.naviControl.Size = new System.Drawing.Size(351, 25);
            this.naviControl.TabIndex = 4;
            this.naviControl.Text = "bindingNavigator1";
            // 
            // naviControlAdd
            // 
            this.naviControlAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviControlAdd.Image = ((System.Drawing.Image)(resources.GetObject("naviControlAdd.Image")));
            this.naviControlAdd.Name = "naviControlAdd";
            this.naviControlAdd.RightToLeftAutoMirrorImage = true;
            this.naviControlAdd.Size = new System.Drawing.Size(23, 22);
            this.naviControlAdd.Text = "Add new";
            this.naviControlAdd.Visible = false;
            // 
            // naviControlCount
            // 
            this.naviControlCount.Name = "naviControlCount";
            this.naviControlCount.Size = new System.Drawing.Size(35, 22);
            this.naviControlCount.Text = "of {0}";
            this.naviControlCount.ToolTipText = "Total number of items";
            // 
            // naviControlDelete
            // 
            this.naviControlDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviControlDelete.Image = ((System.Drawing.Image)(resources.GetObject("naviControlDelete.Image")));
            this.naviControlDelete.Name = "naviControlDelete";
            this.naviControlDelete.RightToLeftAutoMirrorImage = true;
            this.naviControlDelete.Size = new System.Drawing.Size(23, 22);
            this.naviControlDelete.Text = "Delete";
            this.naviControlDelete.Visible = false;
            // 
            // naviControlMoveFirst
            // 
            this.naviControlMoveFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviControlMoveFirst.Image = ((System.Drawing.Image)(resources.GetObject("naviControlMoveFirst.Image")));
            this.naviControlMoveFirst.Name = "naviControlMoveFirst";
            this.naviControlMoveFirst.RightToLeftAutoMirrorImage = true;
            this.naviControlMoveFirst.Size = new System.Drawing.Size(23, 22);
            this.naviControlMoveFirst.Text = "Move first";
            // 
            // naviControlMovePrevious
            // 
            this.naviControlMovePrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviControlMovePrevious.Image = ((System.Drawing.Image)(resources.GetObject("naviControlMovePrevious.Image")));
            this.naviControlMovePrevious.Name = "naviControlMovePrevious";
            this.naviControlMovePrevious.RightToLeftAutoMirrorImage = true;
            this.naviControlMovePrevious.Size = new System.Drawing.Size(23, 22);
            this.naviControlMovePrevious.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // naviControlPosition
            // 
            this.naviControlPosition.AccessibleName = "Position";
            this.naviControlPosition.AutoSize = false;
            this.naviControlPosition.Name = "naviControlPosition";
            this.naviControlPosition.Size = new System.Drawing.Size(50, 23);
            this.naviControlPosition.Text = "0";
            this.naviControlPosition.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // naviControlMoveNext
            // 
            this.naviControlMoveNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviControlMoveNext.Image = ((System.Drawing.Image)(resources.GetObject("naviControlMoveNext.Image")));
            this.naviControlMoveNext.Name = "naviControlMoveNext";
            this.naviControlMoveNext.RightToLeftAutoMirrorImage = true;
            this.naviControlMoveNext.Size = new System.Drawing.Size(23, 22);
            this.naviControlMoveNext.Text = "Move next";
            // 
            // naviControlMoveLast
            // 
            this.naviControlMoveLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviControlMoveLast.Image = ((System.Drawing.Image)(resources.GetObject("naviControlMoveLast.Image")));
            this.naviControlMoveLast.Name = "naviControlMoveLast";
            this.naviControlMoveLast.RightToLeftAutoMirrorImage = true;
            this.naviControlMoveLast.Size = new System.Drawing.Size(23, 22);
            this.naviControlMoveLast.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // gridControl
            // 
            this.gridControl.AllowUserToAddRows = false;
            this.gridControl.AllowUserToDeleteRows = false;
            this.gridControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gridControl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridControl.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colControllerID,
            this.colCheckbox,
            this.colSTT,
            this.colControllerName,
            this.colStatus,
            this.colLineID});
            this.gridControl.Location = new System.Drawing.Point(6, 65);
            this.gridControl.Name = "gridControl";
            this.gridControl.originalSource = null;
            this.gridControl.PageSize = 10;
            this.gridControl.Size = new System.Drawing.Size(345, 229);
            this.gridControl.TabIndex = 2;
            // 
            // colControllerID
            // 
            this.colControllerID.DataPropertyName = "ControllerID";
            this.colControllerID.HeaderText = "ControllerID";
            this.colControllerID.Name = "colControllerID";
            this.colControllerID.Visible = false;
            this.colControllerID.Width = 15;
            // 
            // colCheckbox
            // 
            this.colCheckbox.FalseValue = "0";
            this.colCheckbox.HeaderText = "";
            this.colCheckbox.IndeterminateValue = "-1";
            this.colCheckbox.Name = "colCheckbox";
            this.colCheckbox.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCheckbox.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCheckbox.TrueValue = "1";
            this.colCheckbox.Width = 25;
            // 
            // colSTT
            // 
            this.colSTT.HeaderText = "STT";
            this.colSTT.Name = "colSTT";
            this.colSTT.ReadOnly = true;
            this.colSTT.Width = 40;
            // 
            // colControllerName
            // 
            this.colControllerName.DataPropertyName = "ControllerName";
            this.colControllerName.HeaderText = "Tên";
            this.colControllerName.Name = "colControllerName";
            this.colControllerName.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "Trạng thái";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // colLineID
            // 
            this.colLineID.DataPropertyName = "LineID";
            this.colLineID.HeaderText = "LineID";
            this.colLineID.Name = "colLineID";
            this.colLineID.ReadOnly = true;
            this.colLineID.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Máy tính :";
            // 
            // gbAction
            // 
            this.gbAction.Controls.Add(this.tabcontrolAction);
            this.gbAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAction.Location = new System.Drawing.Point(0, 0);
            this.gbAction.Name = "gbAction";
            this.gbAction.Size = new System.Drawing.Size(1008, 389);
            this.gbAction.TabIndex = 0;
            this.gbAction.TabStop = false;
            // 
            // tabcontrolAction
            // 
            this.tabcontrolAction.Controls.Add(this.tpCard);
            this.tabcontrolAction.Controls.Add(this.tpFinger);
            this.tabcontrolAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabcontrolAction.Location = new System.Drawing.Point(3, 20);
            this.tabcontrolAction.Name = "tabcontrolAction";
            this.tabcontrolAction.SelectedIndex = 0;
            this.tabcontrolAction.Size = new System.Drawing.Size(1002, 366);
            this.tabcontrolAction.TabIndex = 0;
            // 
            // tpCard
            // 
            this.tpCard.BackColor = System.Drawing.SystemColors.Control;
            this.tpCard.Controls.Add(this.groupBox5);
            this.tpCard.Controls.Add(this.groupBox4);
            this.tpCard.Location = new System.Drawing.Point(4, 27);
            this.tpCard.Name = "tpCard";
            this.tpCard.Padding = new System.Windows.Forms.Padding(3);
            this.tpCard.Size = new System.Drawing.Size(994, 335);
            this.tpCard.TabIndex = 0;
            this.tpCard.Text = "Xử lý với thẻ";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.cbAccessCard);
            this.groupBox5.Controls.Add(this.cbCardGroup);
            this.groupBox5.Controls.Add(this.cbCustomerGroupCard);
            this.groupBox5.Controls.Add(this.gridCard);
            this.groupBox5.Controls.Add(this.naviCard);
            this.groupBox5.Controls.Add(this.btnSearchCard);
            this.groupBox5.Controls.Add(this.txtSearchCard);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Location = new System.Drawing.Point(368, 6);
            this.groupBox5.MinimumSize = new System.Drawing.Size(621, 326);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(621, 326);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Danh sách thẻ";
            // 
            // cbAccessCard
            // 
            this.cbAccessCard.FormattingEnabled = true;
            this.cbAccessCard.Location = new System.Drawing.Point(203, 57);
            this.cbAccessCard.Name = "cbAccessCard";
            this.cbAccessCard.Size = new System.Drawing.Size(121, 26);
            this.cbAccessCard.TabIndex = 9;
            // 
            // cbCardGroup
            // 
            this.cbCardGroup.FormattingEnabled = true;
            this.cbCardGroup.Location = new System.Drawing.Point(24, 57);
            this.cbCardGroup.Name = "cbCardGroup";
            this.cbCardGroup.Size = new System.Drawing.Size(121, 26);
            this.cbCardGroup.TabIndex = 8;
            // 
            // cbCustomerGroupCard
            // 
            this.cbCustomerGroupCard.FormattingEnabled = true;
            this.cbCustomerGroupCard.Location = new System.Drawing.Point(382, 57);
            this.cbCustomerGroupCard.Name = "cbCustomerGroupCard";
            this.cbCustomerGroupCard.Size = new System.Drawing.Size(121, 26);
            this.cbCustomerGroupCard.TabIndex = 7;
            // 
            // gridCard
            // 
            this.gridCard.AllowUserToAddRows = false;
            this.gridCard.AllowUserToDeleteRows = false;
            this.gridCard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCard.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCardID,
            this.colCardNo,
            this.colCardNumber,
            this.colCardGroupId,
            this.colCardGroupName,
            this.colCustomerGroupName,
            this.colAccessLevelName});
            this.gridCard.Location = new System.Drawing.Point(7, 155);
            this.gridCard.Name = "gridCard";
            this.gridCard.originalSource = null;
            this.gridCard.PageSize = 10;
            this.gridCard.ReadOnly = true;
            this.gridCard.Size = new System.Drawing.Size(608, 140);
            this.gridCard.TabIndex = 6;
            // 
            // colCardID
            // 
            this.colCardID.DataPropertyName = "CardID";
            this.colCardID.HeaderText = "CardID";
            this.colCardID.Name = "colCardID";
            this.colCardID.ReadOnly = true;
            this.colCardID.Visible = false;
            // 
            // colCardNo
            // 
            this.colCardNo.DataPropertyName = "CardNo";
            this.colCardNo.HeaderText = "Mã Thẻ";
            this.colCardNo.Name = "colCardNo";
            this.colCardNo.ReadOnly = true;
            // 
            // colCardNumber
            // 
            this.colCardNumber.DataPropertyName = "CardNumber";
            this.colCardNumber.HeaderText = "Số thẻ";
            this.colCardNumber.Name = "colCardNumber";
            this.colCardNumber.ReadOnly = true;
            // 
            // colCardGroupId
            // 
            this.colCardGroupId.DataPropertyName = "CardGroupId";
            this.colCardGroupId.HeaderText = "CardGroupId";
            this.colCardGroupId.Name = "colCardGroupId";
            this.colCardGroupId.ReadOnly = true;
            this.colCardGroupId.Visible = false;
            // 
            // colCardGroupName
            // 
            this.colCardGroupName.DataPropertyName = "CardGroupName";
            this.colCardGroupName.HeaderText = "Nhóm thẻ";
            this.colCardGroupName.Name = "colCardGroupName";
            this.colCardGroupName.ReadOnly = true;
            // 
            // colCustomerGroupName
            // 
            this.colCustomerGroupName.DataPropertyName = "CustomerGroupName";
            this.colCustomerGroupName.HeaderText = "Nhóm KH";
            this.colCustomerGroupName.Name = "colCustomerGroupName";
            this.colCustomerGroupName.ReadOnly = true;
            // 
            // colAccessLevelName
            // 
            this.colAccessLevelName.DataPropertyName = "AccessLevelName";
            this.colAccessLevelName.HeaderText = "Quyền truy cập";
            this.colAccessLevelName.Name = "colAccessLevelName";
            this.colAccessLevelName.ReadOnly = true;
            this.colAccessLevelName.Width = 165;
            // 
            // naviCard
            // 
            this.naviCard.AddNewItem = this.naviCardAdd;
            this.naviCard.CountItem = this.naviCardCount;
            this.naviCard.DeleteItem = this.naviCardDelete;
            this.naviCard.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.naviCard.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.naviCardMoveFirst,
            this.naviCardMovePrevious,
            this.bindingNavigatorSeparator3,
            this.naviCardPosition,
            this.naviCardCount,
            this.bindingNavigatorSeparator4,
            this.naviCardMoveNext,
            this.naviCardMoveLast,
            this.bindingNavigatorSeparator5,
            this.naviCardAdd,
            this.naviCardDelete});
            this.naviCard.Location = new System.Drawing.Point(3, 298);
            this.naviCard.MoveFirstItem = this.naviCardMoveFirst;
            this.naviCard.MoveLastItem = this.naviCardMoveLast;
            this.naviCard.MoveNextItem = this.naviCardMoveNext;
            this.naviCard.MovePreviousItem = this.naviCardMovePrevious;
            this.naviCard.Name = "naviCard";
            this.naviCard.PositionItem = this.naviCardPosition;
            this.naviCard.Size = new System.Drawing.Size(615, 25);
            this.naviCard.TabIndex = 5;
            this.naviCard.Text = "bindingNavigator1";
            // 
            // naviCardAdd
            // 
            this.naviCardAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviCardAdd.Image = ((System.Drawing.Image)(resources.GetObject("naviCardAdd.Image")));
            this.naviCardAdd.Name = "naviCardAdd";
            this.naviCardAdd.RightToLeftAutoMirrorImage = true;
            this.naviCardAdd.Size = new System.Drawing.Size(23, 22);
            this.naviCardAdd.Text = "Add new";
            this.naviCardAdd.Visible = false;
            // 
            // naviCardCount
            // 
            this.naviCardCount.Name = "naviCardCount";
            this.naviCardCount.Size = new System.Drawing.Size(35, 22);
            this.naviCardCount.Text = "of {0}";
            this.naviCardCount.ToolTipText = "Total number of items";
            // 
            // naviCardDelete
            // 
            this.naviCardDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviCardDelete.Image = ((System.Drawing.Image)(resources.GetObject("naviCardDelete.Image")));
            this.naviCardDelete.Name = "naviCardDelete";
            this.naviCardDelete.RightToLeftAutoMirrorImage = true;
            this.naviCardDelete.Size = new System.Drawing.Size(23, 22);
            this.naviCardDelete.Text = "Delete";
            this.naviCardDelete.Visible = false;
            // 
            // naviCardMoveFirst
            // 
            this.naviCardMoveFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviCardMoveFirst.Image = ((System.Drawing.Image)(resources.GetObject("naviCardMoveFirst.Image")));
            this.naviCardMoveFirst.Name = "naviCardMoveFirst";
            this.naviCardMoveFirst.RightToLeftAutoMirrorImage = true;
            this.naviCardMoveFirst.Size = new System.Drawing.Size(23, 22);
            this.naviCardMoveFirst.Text = "Move first";
            // 
            // naviCardMovePrevious
            // 
            this.naviCardMovePrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviCardMovePrevious.Image = ((System.Drawing.Image)(resources.GetObject("naviCardMovePrevious.Image")));
            this.naviCardMovePrevious.Name = "naviCardMovePrevious";
            this.naviCardMovePrevious.RightToLeftAutoMirrorImage = true;
            this.naviCardMovePrevious.Size = new System.Drawing.Size(23, 22);
            this.naviCardMovePrevious.Text = "Move previous";
            // 
            // bindingNavigatorSeparator3
            // 
            this.bindingNavigatorSeparator3.Name = "bindingNavigatorSeparator3";
            this.bindingNavigatorSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // naviCardPosition
            // 
            this.naviCardPosition.AccessibleName = "Position";
            this.naviCardPosition.AutoSize = false;
            this.naviCardPosition.Name = "naviCardPosition";
            this.naviCardPosition.Size = new System.Drawing.Size(50, 23);
            this.naviCardPosition.Text = "0";
            this.naviCardPosition.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator4
            // 
            this.bindingNavigatorSeparator4.Name = "bindingNavigatorSeparator4";
            this.bindingNavigatorSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // naviCardMoveNext
            // 
            this.naviCardMoveNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviCardMoveNext.Image = ((System.Drawing.Image)(resources.GetObject("naviCardMoveNext.Image")));
            this.naviCardMoveNext.Name = "naviCardMoveNext";
            this.naviCardMoveNext.RightToLeftAutoMirrorImage = true;
            this.naviCardMoveNext.Size = new System.Drawing.Size(23, 22);
            this.naviCardMoveNext.Text = "Move next";
            // 
            // naviCardMoveLast
            // 
            this.naviCardMoveLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviCardMoveLast.Image = ((System.Drawing.Image)(resources.GetObject("naviCardMoveLast.Image")));
            this.naviCardMoveLast.Name = "naviCardMoveLast";
            this.naviCardMoveLast.RightToLeftAutoMirrorImage = true;
            this.naviCardMoveLast.Size = new System.Drawing.Size(23, 22);
            this.naviCardMoveLast.Text = "Move last";
            // 
            // bindingNavigatorSeparator5
            // 
            this.bindingNavigatorSeparator5.Name = "bindingNavigatorSeparator5";
            this.bindingNavigatorSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSearchCard
            // 
            this.btnSearchCard.Location = new System.Drawing.Point(428, 106);
            this.btnSearchCard.Name = "btnSearchCard";
            this.btnSearchCard.Size = new System.Drawing.Size(75, 24);
            this.btnSearchCard.TabIndex = 4;
            this.btnSearchCard.Text = "Tìm";
            this.btnSearchCard.UseVisualStyleBackColor = true;
            this.btnSearchCard.Click += new System.EventHandler(this.btnSearchCard_Click);
            // 
            // txtSearchCard
            // 
            this.txtSearchCard.Location = new System.Drawing.Point(24, 106);
            this.txtSearchCard.Name = "txtSearchCard";
            this.txtSearchCard.Size = new System.Drawing.Size(376, 24);
            this.txtSearchCard.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(379, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "Nhóm khách hàng";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(200, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "Quyền truy cập";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "Nhóm thẻ";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.btnCancelCard);
            this.groupBox4.Controls.Add(this.pbCard);
            this.groupBox4.Controls.Add(this.btnDeleteAllCard);
            this.groupBox4.Controls.Add(this.btnInsertAllCard);
            this.groupBox4.Controls.Add(this.btnDeleteCard);
            this.groupBox4.Controls.Add(this.btnInsertCard);
            this.groupBox4.Controls.Add(this.chkUseCard);
            this.groupBox4.Controls.Add(this.dtpExpireCard);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Location = new System.Drawing.Point(5, 6);
            this.groupBox4.MinimumSize = new System.Drawing.Size(357, 326);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(357, 326);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Xử lí thẻ";
            // 
            // btnCancelCard
            // 
            this.btnCancelCard.Location = new System.Drawing.Point(282, 242);
            this.btnCancelCard.Name = "btnCancelCard";
            this.btnCancelCard.Size = new System.Drawing.Size(50, 50);
            this.btnCancelCard.TabIndex = 5;
            this.btnCancelCard.Text = "X";
            this.btnCancelCard.UseVisualStyleBackColor = true;
            // 
            // pbCard
            // 
            this.pbCard.Location = new System.Drawing.Point(20, 242);
            this.pbCard.Name = "pbCard";
            this.pbCard.Size = new System.Drawing.Size(256, 50);
            this.pbCard.TabIndex = 4;
            // 
            // btnDeleteAllCard
            // 
            this.btnDeleteAllCard.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteAllCard.Location = new System.Drawing.Point(182, 172);
            this.btnDeleteAllCard.Name = "btnDeleteAllCard";
            this.btnDeleteAllCard.Size = new System.Drawing.Size(150, 50);
            this.btnDeleteAllCard.TabIndex = 3;
            this.btnDeleteAllCard.Text = "Hủy toàn bộ thẻ";
            this.btnDeleteAllCard.UseVisualStyleBackColor = false;
            this.btnDeleteAllCard.Click += new System.EventHandler(this.btnDeleteAllCard_Click);
            // 
            // btnInsertAllCard
            // 
            this.btnInsertAllCard.BackColor = System.Drawing.SystemColors.Control;
            this.btnInsertAllCard.Location = new System.Drawing.Point(182, 107);
            this.btnInsertAllCard.Name = "btnInsertAllCard";
            this.btnInsertAllCard.Size = new System.Drawing.Size(150, 50);
            this.btnInsertAllCard.TabIndex = 3;
            this.btnInsertAllCard.Text = "Nạp toàn bộ thẻ";
            this.btnInsertAllCard.UseVisualStyleBackColor = false;
            this.btnInsertAllCard.Click += new System.EventHandler(this.btnInsertAllCard_Click);
            // 
            // btnDeleteCard
            // 
            this.btnDeleteCard.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteCard.Location = new System.Drawing.Point(20, 172);
            this.btnDeleteCard.Name = "btnDeleteCard";
            this.btnDeleteCard.Size = new System.Drawing.Size(150, 50);
            this.btnDeleteCard.TabIndex = 3;
            this.btnDeleteCard.Text = "Hủy thẻ đã chọn";
            this.btnDeleteCard.UseVisualStyleBackColor = false;
            this.btnDeleteCard.Click += new System.EventHandler(this.btnDeleteCard_Click);
            // 
            // btnInsertCard
            // 
            this.btnInsertCard.BackColor = System.Drawing.SystemColors.Control;
            this.btnInsertCard.Location = new System.Drawing.Point(20, 107);
            this.btnInsertCard.Name = "btnInsertCard";
            this.btnInsertCard.Size = new System.Drawing.Size(150, 50);
            this.btnInsertCard.TabIndex = 3;
            this.btnInsertCard.Text = "Nạp thẻ đã chọn";
            this.btnInsertCard.UseVisualStyleBackColor = false;
            this.btnInsertCard.Click += new System.EventHandler(this.btnInsertCard_Click);
            // 
            // chkUseCard
            // 
            this.chkUseCard.AutoSize = true;
            this.chkUseCard.Location = new System.Drawing.Point(251, 59);
            this.chkUseCard.Name = "chkUseCard";
            this.chkUseCard.Size = new System.Drawing.Size(81, 22);
            this.chkUseCard.TabIndex = 2;
            this.chkUseCard.Text = "Sử dụng";
            this.chkUseCard.UseVisualStyleBackColor = true;
            // 
            // dtpExpireCard
            // 
            this.dtpExpireCard.CustomFormat = "dd/MM/yyyy";
            this.dtpExpireCard.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpireCard.Location = new System.Drawing.Point(20, 59);
            this.dtpExpireCard.Name = "dtpExpireCard";
            this.dtpExpireCard.Size = new System.Drawing.Size(225, 24);
            this.dtpExpireCard.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Chọn ngày hết hạn";
            // 
            // tpFinger
            // 
            this.tpFinger.BackColor = System.Drawing.SystemColors.Control;
            this.tpFinger.Controls.Add(this.groupBox7);
            this.tpFinger.Controls.Add(this.groupBox6);
            this.tpFinger.Location = new System.Drawing.Point(4, 27);
            this.tpFinger.Name = "tpFinger";
            this.tpFinger.Padding = new System.Windows.Forms.Padding(3);
            this.tpFinger.Size = new System.Drawing.Size(994, 335);
            this.tpFinger.TabIndex = 1;
            this.tpFinger.Text = "Xử lý với vân tay";
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.cbAccessFinger);
            this.groupBox7.Controls.Add(this.cbCustomerGroupFinger);
            this.groupBox7.Controls.Add(this.gridFinger);
            this.groupBox7.Controls.Add(this.naviFinger);
            this.groupBox7.Controls.Add(this.btnSearchFinger);
            this.groupBox7.Controls.Add(this.txtSearchFinger);
            this.groupBox7.Controls.Add(this.label6);
            this.groupBox7.Controls.Add(this.label7);
            this.groupBox7.Location = new System.Drawing.Point(368, 6);
            this.groupBox7.MinimumSize = new System.Drawing.Size(621, 326);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(621, 326);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Danh sách vân tay";
            // 
            // cbAccessFinger
            // 
            this.cbAccessFinger.FormattingEnabled = true;
            this.cbAccessFinger.Location = new System.Drawing.Point(24, 57);
            this.cbAccessFinger.Name = "cbAccessFinger";
            this.cbAccessFinger.Size = new System.Drawing.Size(121, 26);
            this.cbAccessFinger.TabIndex = 16;
            // 
            // cbCustomerGroupFinger
            // 
            this.cbCustomerGroupFinger.FormattingEnabled = true;
            this.cbCustomerGroupFinger.Location = new System.Drawing.Point(203, 57);
            this.cbCustomerGroupFinger.Name = "cbCustomerGroupFinger";
            this.cbCustomerGroupFinger.Size = new System.Drawing.Size(121, 26);
            this.cbCustomerGroupFinger.TabIndex = 15;
            // 
            // gridFinger
            // 
            this.gridFinger.AllowUserToAddRows = false;
            this.gridFinger.AllowUserToDeleteRows = false;
            this.gridFinger.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFinger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFinger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCustomerID,
            this.colCustomerCode,
            this.colCustomerName,
            this.colCustomerGroupName_2});
            this.gridFinger.Location = new System.Drawing.Point(7, 155);
            this.gridFinger.Name = "gridFinger";
            this.gridFinger.originalSource = null;
            this.gridFinger.PageSize = 10;
            this.gridFinger.ReadOnly = true;
            this.gridFinger.Size = new System.Drawing.Size(608, 140);
            this.gridFinger.TabIndex = 14;
            // 
            // colCustomerID
            // 
            this.colCustomerID.DataPropertyName = "CustomerID";
            this.colCustomerID.HeaderText = "Column1";
            this.colCustomerID.Name = "colCustomerID";
            this.colCustomerID.ReadOnly = true;
            this.colCustomerID.Visible = false;
            // 
            // colCustomerCode
            // 
            this.colCustomerCode.DataPropertyName = "CustomerCode";
            this.colCustomerCode.HeaderText = "Mã KH";
            this.colCustomerCode.Name = "colCustomerCode";
            this.colCustomerCode.ReadOnly = true;
            // 
            // colCustomerName
            // 
            this.colCustomerName.DataPropertyName = "CustomerName";
            this.colCustomerName.HeaderText = "Tên KH";
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.ReadOnly = true;
            // 
            // colCustomerGroupName_2
            // 
            this.colCustomerGroupName_2.DataPropertyName = "CustomerGroupName";
            this.colCustomerGroupName_2.HeaderText = "Nhóm KH";
            this.colCustomerGroupName_2.Name = "colCustomerGroupName_2";
            this.colCustomerGroupName_2.ReadOnly = true;
            // 
            // naviFinger
            // 
            this.naviFinger.AddNewItem = this.naviFingerAdd;
            this.naviFinger.CountItem = this.naviFingerCount;
            this.naviFinger.DeleteItem = this.naviFingerDelete;
            this.naviFinger.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.naviFinger.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.naviFingerMoveFirst,
            this.naviFingerMovePrevious,
            this.bindingNavigatorSeparator6,
            this.naviFingerPosition,
            this.naviFingerCount,
            this.bindingNavigatorSeparator7,
            this.naviFingerMoveNext,
            this.naviFingerMoveLast,
            this.bindingNavigatorSeparator8,
            this.naviFingerAdd,
            this.naviFingerDelete});
            this.naviFinger.Location = new System.Drawing.Point(3, 298);
            this.naviFinger.MoveFirstItem = this.naviFingerMoveFirst;
            this.naviFinger.MoveLastItem = this.naviFingerMoveLast;
            this.naviFinger.MoveNextItem = this.naviFingerMoveNext;
            this.naviFinger.MovePreviousItem = this.naviFingerMovePrevious;
            this.naviFinger.Name = "naviFinger";
            this.naviFinger.PositionItem = this.naviFingerPosition;
            this.naviFinger.Size = new System.Drawing.Size(615, 25);
            this.naviFinger.TabIndex = 13;
            this.naviFinger.Text = "bindingNavigator1";
            // 
            // naviFingerAdd
            // 
            this.naviFingerAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviFingerAdd.Image = ((System.Drawing.Image)(resources.GetObject("naviFingerAdd.Image")));
            this.naviFingerAdd.Name = "naviFingerAdd";
            this.naviFingerAdd.RightToLeftAutoMirrorImage = true;
            this.naviFingerAdd.Size = new System.Drawing.Size(23, 22);
            this.naviFingerAdd.Text = "Add new";
            this.naviFingerAdd.Visible = false;
            // 
            // naviFingerCount
            // 
            this.naviFingerCount.Name = "naviFingerCount";
            this.naviFingerCount.Size = new System.Drawing.Size(35, 22);
            this.naviFingerCount.Text = "of {0}";
            this.naviFingerCount.ToolTipText = "Total number of items";
            // 
            // naviFingerDelete
            // 
            this.naviFingerDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviFingerDelete.Image = ((System.Drawing.Image)(resources.GetObject("naviFingerDelete.Image")));
            this.naviFingerDelete.Name = "naviFingerDelete";
            this.naviFingerDelete.RightToLeftAutoMirrorImage = true;
            this.naviFingerDelete.Size = new System.Drawing.Size(23, 22);
            this.naviFingerDelete.Text = "Delete";
            this.naviFingerDelete.Visible = false;
            // 
            // naviFingerMoveFirst
            // 
            this.naviFingerMoveFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviFingerMoveFirst.Image = ((System.Drawing.Image)(resources.GetObject("naviFingerMoveFirst.Image")));
            this.naviFingerMoveFirst.Name = "naviFingerMoveFirst";
            this.naviFingerMoveFirst.RightToLeftAutoMirrorImage = true;
            this.naviFingerMoveFirst.Size = new System.Drawing.Size(23, 22);
            this.naviFingerMoveFirst.Text = "Move first";
            // 
            // naviFingerMovePrevious
            // 
            this.naviFingerMovePrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviFingerMovePrevious.Image = ((System.Drawing.Image)(resources.GetObject("naviFingerMovePrevious.Image")));
            this.naviFingerMovePrevious.Name = "naviFingerMovePrevious";
            this.naviFingerMovePrevious.RightToLeftAutoMirrorImage = true;
            this.naviFingerMovePrevious.Size = new System.Drawing.Size(23, 22);
            this.naviFingerMovePrevious.Text = "Move previous";
            // 
            // bindingNavigatorSeparator6
            // 
            this.bindingNavigatorSeparator6.Name = "bindingNavigatorSeparator6";
            this.bindingNavigatorSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // naviFingerPosition
            // 
            this.naviFingerPosition.AccessibleName = "Position";
            this.naviFingerPosition.AutoSize = false;
            this.naviFingerPosition.Name = "naviFingerPosition";
            this.naviFingerPosition.Size = new System.Drawing.Size(50, 23);
            this.naviFingerPosition.Text = "0";
            this.naviFingerPosition.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator7
            // 
            this.bindingNavigatorSeparator7.Name = "bindingNavigatorSeparator7";
            this.bindingNavigatorSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // naviFingerMoveNext
            // 
            this.naviFingerMoveNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviFingerMoveNext.Image = ((System.Drawing.Image)(resources.GetObject("naviFingerMoveNext.Image")));
            this.naviFingerMoveNext.Name = "naviFingerMoveNext";
            this.naviFingerMoveNext.RightToLeftAutoMirrorImage = true;
            this.naviFingerMoveNext.Size = new System.Drawing.Size(23, 22);
            this.naviFingerMoveNext.Text = "Move next";
            // 
            // naviFingerMoveLast
            // 
            this.naviFingerMoveLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.naviFingerMoveLast.Image = ((System.Drawing.Image)(resources.GetObject("naviFingerMoveLast.Image")));
            this.naviFingerMoveLast.Name = "naviFingerMoveLast";
            this.naviFingerMoveLast.RightToLeftAutoMirrorImage = true;
            this.naviFingerMoveLast.Size = new System.Drawing.Size(23, 22);
            this.naviFingerMoveLast.Text = "Move last";
            // 
            // bindingNavigatorSeparator8
            // 
            this.bindingNavigatorSeparator8.Name = "bindingNavigatorSeparator8";
            this.bindingNavigatorSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSearchFinger
            // 
            this.btnSearchFinger.Location = new System.Drawing.Point(428, 106);
            this.btnSearchFinger.Name = "btnSearchFinger";
            this.btnSearchFinger.Size = new System.Drawing.Size(75, 24);
            this.btnSearchFinger.TabIndex = 12;
            this.btnSearchFinger.Text = "Tìm";
            this.btnSearchFinger.UseVisualStyleBackColor = true;
            this.btnSearchFinger.Click += new System.EventHandler(this.btnSearchFinger_Click);
            // 
            // txtSearchFinger
            // 
            this.txtSearchFinger.Location = new System.Drawing.Point(24, 106);
            this.txtSearchFinger.Name = "txtSearchFinger";
            this.txtSearchFinger.Size = new System.Drawing.Size(376, 24);
            this.txtSearchFinger.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(200, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 18);
            this.label6.TabIndex = 6;
            this.label6.Text = "Nhóm khách hàng";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 18);
            this.label7.TabIndex = 7;
            this.label7.Text = "Quyền truy cập";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox6.Controls.Add(this.btnCancelFinger);
            this.groupBox6.Controls.Add(this.pbFinger);
            this.groupBox6.Controls.Add(this.chkUseFinger);
            this.groupBox6.Controls.Add(this.btnDeleteAllFinger);
            this.groupBox6.Controls.Add(this.btnInsertAllFinger);
            this.groupBox6.Controls.Add(this.btnDeleteFinger);
            this.groupBox6.Controls.Add(this.btnInsertFinger);
            this.groupBox6.Controls.Add(this.dtpExpireFinger);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Location = new System.Drawing.Point(5, 6);
            this.groupBox6.MinimumSize = new System.Drawing.Size(357, 326);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(357, 326);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Xử lí vân tay";
            // 
            // btnCancelFinger
            // 
            this.btnCancelFinger.Location = new System.Drawing.Point(282, 242);
            this.btnCancelFinger.Name = "btnCancelFinger";
            this.btnCancelFinger.Size = new System.Drawing.Size(50, 50);
            this.btnCancelFinger.TabIndex = 12;
            this.btnCancelFinger.Text = "X";
            this.btnCancelFinger.UseVisualStyleBackColor = true;
            // 
            // pbFinger
            // 
            this.pbFinger.Location = new System.Drawing.Point(20, 242);
            this.pbFinger.Name = "pbFinger";
            this.pbFinger.Size = new System.Drawing.Size(256, 50);
            this.pbFinger.TabIndex = 11;
            // 
            // chkUseFinger
            // 
            this.chkUseFinger.AutoSize = true;
            this.chkUseFinger.Location = new System.Drawing.Point(251, 59);
            this.chkUseFinger.Name = "chkUseFinger";
            this.chkUseFinger.Size = new System.Drawing.Size(81, 22);
            this.chkUseFinger.TabIndex = 10;
            this.chkUseFinger.Text = "Sử dụng";
            this.chkUseFinger.UseVisualStyleBackColor = true;
            // 
            // btnDeleteAllFinger
            // 
            this.btnDeleteAllFinger.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteAllFinger.Location = new System.Drawing.Point(182, 172);
            this.btnDeleteAllFinger.Name = "btnDeleteAllFinger";
            this.btnDeleteAllFinger.Size = new System.Drawing.Size(150, 50);
            this.btnDeleteAllFinger.TabIndex = 6;
            this.btnDeleteAllFinger.Text = "Hủy toàn bộ vân tay";
            this.btnDeleteAllFinger.UseVisualStyleBackColor = false;
            this.btnDeleteAllFinger.Click += new System.EventHandler(this.btnDeleteAllFinger_Click);
            // 
            // btnInsertAllFinger
            // 
            this.btnInsertAllFinger.BackColor = System.Drawing.SystemColors.Control;
            this.btnInsertAllFinger.Location = new System.Drawing.Point(182, 107);
            this.btnInsertAllFinger.Name = "btnInsertAllFinger";
            this.btnInsertAllFinger.Size = new System.Drawing.Size(150, 50);
            this.btnInsertAllFinger.TabIndex = 7;
            this.btnInsertAllFinger.Text = "Nạp toàn bộ vân tay";
            this.btnInsertAllFinger.UseVisualStyleBackColor = false;
            this.btnInsertAllFinger.Click += new System.EventHandler(this.btnInsertAllFinger_Click);
            // 
            // btnDeleteFinger
            // 
            this.btnDeleteFinger.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteFinger.Location = new System.Drawing.Point(20, 172);
            this.btnDeleteFinger.Name = "btnDeleteFinger";
            this.btnDeleteFinger.Size = new System.Drawing.Size(150, 50);
            this.btnDeleteFinger.TabIndex = 8;
            this.btnDeleteFinger.Text = "Hủy vân tay đã chọn";
            this.btnDeleteFinger.UseVisualStyleBackColor = false;
            this.btnDeleteFinger.Click += new System.EventHandler(this.btnDeleteFinger_Click);
            // 
            // btnInsertFinger
            // 
            this.btnInsertFinger.BackColor = System.Drawing.SystemColors.Control;
            this.btnInsertFinger.Location = new System.Drawing.Point(20, 107);
            this.btnInsertFinger.Name = "btnInsertFinger";
            this.btnInsertFinger.Size = new System.Drawing.Size(150, 50);
            this.btnInsertFinger.TabIndex = 9;
            this.btnInsertFinger.Text = "Nạp vân tay đã chọn";
            this.btnInsertFinger.UseVisualStyleBackColor = false;
            this.btnInsertFinger.Click += new System.EventHandler(this.btnInsertFinger_Click);
            // 
            // dtpExpireFinger
            // 
            this.dtpExpireFinger.CustomFormat = "dd/MM/yyyy";
            this.dtpExpireFinger.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpireFinger.Location = new System.Drawing.Point(20, 59);
            this.dtpExpireFinger.Name = "dtpExpireFinger";
            this.dtpExpireFinger.Size = new System.Drawing.Size(225, 24);
            this.dtpExpireFinger.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 18);
            this.label8.TabIndex = 4;
            this.label8.Text = "Chọn ngày hết hạn";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.splitMain);
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Giao tiếp thiết bị";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.gbError.ResumeLayout(false);
            this.tabcontrolLog.ResumeLayout(false);
            this.tpELog.ResumeLayout(false);
            this.tpNLog.ResumeLayout(false);
            this.gbList.ResumeLayout(false);
            this.gbList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.naviControl)).EndInit();
            this.naviControl.ResumeLayout(false);
            this.naviControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            this.gbAction.ResumeLayout(false);
            this.tabcontrolAction.ResumeLayout(false);
            this.tpCard.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.naviCard)).EndInit();
            this.naviCard.ResumeLayout(false);
            this.naviCard.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tpFinger.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFinger)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.naviFinger)).EndInit();
            this.naviFinger.ResumeLayout(false);
            this.naviFinger.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.GroupBox gbError;
        private System.Windows.Forms.GroupBox gbList;
        private System.Windows.Forms.TabControl tabcontrolLog;
        private System.Windows.Forms.TabPage tpELog;
        private System.Windows.Forms.TabPage tpNLog;
        private System.Windows.Forms.Label label1;
        private CustomControl.PagingGridView gridControl;
        private System.Windows.Forms.ListBox lbELog;
        private System.Windows.Forms.ListBox lbNLog;
        private System.Windows.Forms.GroupBox gbAction;
        private System.Windows.Forms.TabControl tabcontrolAction;
        private System.Windows.Forms.TabPage tpCard;
        private System.Windows.Forms.TabPage tpFinger;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSearchCard;
        private System.Windows.Forms.Button btnSearchCard;
        private System.Windows.Forms.BindingNavigator naviControl;
        private System.Windows.Forms.ToolStripButton naviControlAdd;
        private System.Windows.Forms.ToolStripLabel naviControlCount;
        private System.Windows.Forms.ToolStripButton naviControlDelete;
        private System.Windows.Forms.ToolStripButton naviControlMoveFirst;
        private System.Windows.Forms.ToolStripButton naviControlMovePrevious;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox naviControlPosition;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton naviControlMoveNext;
        private System.Windows.Forms.ToolStripButton naviControlMoveLast;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.Button btnSearchFinger;
        private System.Windows.Forms.TextBox txtSearchFinger;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.BindingNavigator naviCard;
        private System.Windows.Forms.ToolStripButton naviCardAdd;
        private System.Windows.Forms.ToolStripLabel naviCardCount;
        private System.Windows.Forms.ToolStripButton naviCardDelete;
        private System.Windows.Forms.ToolStripButton naviCardMoveFirst;
        private System.Windows.Forms.ToolStripButton naviCardMovePrevious;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator3;
        private System.Windows.Forms.ToolStripTextBox naviCardPosition;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator4;
        private System.Windows.Forms.ToolStripButton naviCardMoveNext;
        private System.Windows.Forms.ToolStripButton naviCardMoveLast;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator5;
        private System.Windows.Forms.BindingNavigator naviFinger;
        private System.Windows.Forms.ToolStripButton naviFingerAdd;
        private System.Windows.Forms.ToolStripLabel naviFingerCount;
        private System.Windows.Forms.ToolStripButton naviFingerDelete;
        private System.Windows.Forms.ToolStripButton naviFingerMoveFirst;
        private System.Windows.Forms.ToolStripButton naviFingerMovePrevious;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator6;
        private System.Windows.Forms.ToolStripTextBox naviFingerPosition;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator7;
        private System.Windows.Forms.ToolStripButton naviFingerMoveNext;
        private System.Windows.Forms.ToolStripButton naviFingerMoveLast;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator8;
        private CustomControl.PagingGridView gridFinger;
        private CustomControl.PagingGridView gridCard;
        private System.Windows.Forms.DateTimePicker dtpExpireCard;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkUseCard;
        private System.Windows.Forms.Button btnInsertAllCard;
        private System.Windows.Forms.Button btnInsertCard;
        private System.Windows.Forms.Button btnDeleteAllCard;
        private System.Windows.Forms.Button btnDeleteCard;
        private System.Windows.Forms.CheckBox chkUseFinger;
        private System.Windows.Forms.Button btnDeleteAllFinger;
        private System.Windows.Forms.Button btnInsertAllFinger;
        private System.Windows.Forms.Button btnDeleteFinger;
        private System.Windows.Forms.Button btnInsertFinger;
        private System.Windows.Forms.DateTimePicker dtpExpireFinger;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colControllerID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheckbox;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSTT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colControllerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineID;
        private System.Windows.Forms.ComboBox cbCustomerGroupCard;
        private System.Windows.Forms.ComboBox cbCustomerGroupFinger;
        private System.Windows.Forms.ComboBox cbAccessCard;
        private System.Windows.Forms.ComboBox cbCardGroup;
        private System.Windows.Forms.ComboBox cbAccessFinger;
        private System.Windows.Forms.ComboBox cbPC;
        private System.Windows.Forms.ProgressBar pbCard;
        private System.Windows.Forms.Button btnCancelCard;
        private System.Windows.Forms.Button btnCancelFinger;
        private System.Windows.Forms.ProgressBar pbFinger;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCardID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCardNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCardGroupId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCardGroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerGroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccessLevelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerGroupName_2;
    }
}

