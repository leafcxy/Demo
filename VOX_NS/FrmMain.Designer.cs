namespace VOX_NS
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.lblProofJobnum = new System.Windows.Forms.Label();
            this.tbProofJobnum = new System.Windows.Forms.TextBox();
            this.tbProofPartnum = new System.Windows.Forms.TextBox();
            this.lblProofPartnum = new System.Windows.Forms.Label();
            this.lblProdJobnum = new System.Windows.Forms.Label();
            this.valProdJobnum = new System.Windows.Forms.Label();
            this.valProdPartnum = new System.Windows.Forms.Label();
            this.lblProdPartnum = new System.Windows.Forms.Label();
            this.tbScan = new System.Windows.Forms.TextBox();
            this.dgvFailed = new System.Windows.Forms.DataGridView();
            this.colFailedUUT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFailedDatecode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFailedScan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFailedOp = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvPass = new System.Windows.Forms.DataGridView();
            this.colPassUUT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPassDatecode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPassScan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPassTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valScan = new System.Windows.Forms.Label();
            this.lblScan = new System.Windows.Forms.Label();
            this.gbFailed = new System.Windows.Forms.GroupBox();
            this.gbPass = new System.Windows.Forms.GroupBox();
            this.btnSend = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFailed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPass)).BeginInit();
            this.gbFailed.SuspendLayout();
            this.gbPass.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblProofJobnum
            // 
            this.lblProofJobnum.AutoSize = true;
            this.lblProofJobnum.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblProofJobnum.Location = new System.Drawing.Point(29, 26);
            this.lblProofJobnum.Name = "lblProofJobnum";
            this.lblProofJobnum.Size = new System.Drawing.Size(112, 24);
            this.lblProofJobnum.TabIndex = 0;
            this.lblProofJobnum.Text = "防错工单:";
            // 
            // tbProofJobnum
            // 
            this.tbProofJobnum.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbProofJobnum.Location = new System.Drawing.Point(147, 25);
            this.tbProofJobnum.Name = "tbProofJobnum";
            this.tbProofJobnum.Size = new System.Drawing.Size(250, 27);
            this.tbProofJobnum.TabIndex = 1;
            // 
            // tbProofPartnum
            // 
            this.tbProofPartnum.Font = new System.Drawing.Font("PMingLiU", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbProofPartnum.Location = new System.Drawing.Point(147, 68);
            this.tbProofPartnum.Name = "tbProofPartnum";
            this.tbProofPartnum.Size = new System.Drawing.Size(250, 27);
            this.tbProofPartnum.TabIndex = 3;
            // 
            // lblProofPartnum
            // 
            this.lblProofPartnum.AutoSize = true;
            this.lblProofPartnum.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblProofPartnum.Location = new System.Drawing.Point(29, 69);
            this.lblProofPartnum.Name = "lblProofPartnum";
            this.lblProofPartnum.Size = new System.Drawing.Size(112, 24);
            this.lblProofPartnum.TabIndex = 2;
            this.lblProofPartnum.Text = "防错型号:";
            // 
            // lblProdJobnum
            // 
            this.lblProdJobnum.AutoSize = true;
            this.lblProdJobnum.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblProdJobnum.Location = new System.Drawing.Point(436, 26);
            this.lblProdJobnum.Name = "lblProdJobnum";
            this.lblProdJobnum.Size = new System.Drawing.Size(112, 24);
            this.lblProdJobnum.TabIndex = 4;
            this.lblProdJobnum.Text = "生产工单:";
            // 
            // valProdJobnum
            // 
            this.valProdJobnum.AutoSize = true;
            this.valProdJobnum.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.valProdJobnum.Location = new System.Drawing.Point(554, 26);
            this.valProdJobnum.Name = "valProdJobnum";
            this.valProdJobnum.Size = new System.Drawing.Size(26, 24);
            this.valProdJobnum.TabIndex = 5;
            this.valProdJobnum.Text = "--";
            // 
            // valProdPartnum
            // 
            this.valProdPartnum.AutoSize = true;
            this.valProdPartnum.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.valProdPartnum.Location = new System.Drawing.Point(554, 69);
            this.valProdPartnum.Name = "valProdPartnum";
            this.valProdPartnum.Size = new System.Drawing.Size(26, 24);
            this.valProdPartnum.TabIndex = 7;
            this.valProdPartnum.Text = "--";
            // 
            // lblProdPartnum
            // 
            this.lblProdPartnum.AutoSize = true;
            this.lblProdPartnum.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblProdPartnum.Location = new System.Drawing.Point(436, 69);
            this.lblProdPartnum.Name = "lblProdPartnum";
            this.lblProdPartnum.Size = new System.Drawing.Size(112, 24);
            this.lblProdPartnum.TabIndex = 6;
            this.lblProdPartnum.Text = "生产型号:";
            // 
            // tbScan
            // 
            this.tbScan.Font = new System.Drawing.Font("PMingLiU", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbScan.Location = new System.Drawing.Point(33, 114);
            this.tbScan.Multiline = true;
            this.tbScan.Name = "tbScan";
            this.tbScan.Size = new System.Drawing.Size(1000, 100);
            this.tbScan.TabIndex = 8;
            // 
            // dgvFailed
            // 
            this.dgvFailed.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFailed.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFailedUUT,
            this.colFailedDatecode,
            this.colFailedScan,
            this.colFailedOp});
            this.dgvFailed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFailed.Location = new System.Drawing.Point(3, 18);
            this.dgvFailed.Name = "dgvFailed";
            this.dgvFailed.RowHeadersVisible = false;
            this.dgvFailed.RowTemplate.Height = 24;
            this.dgvFailed.Size = new System.Drawing.Size(494, 279);
            this.dgvFailed.TabIndex = 9;
            this.dgvFailed.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFailed_CellContentClick);
            // 
            // colFailedUUT
            // 
            this.colFailedUUT.HeaderText = "UUT条码";
            this.colFailedUUT.Name = "colFailedUUT";
            this.colFailedUUT.Width = 120;
            // 
            // colFailedDatecode
            // 
            this.colFailedDatecode.HeaderText = "Datecode";
            this.colFailedDatecode.Name = "colFailedDatecode";
            // 
            // colFailedScan
            // 
            this.colFailedScan.HeaderText = "扫描结果";
            this.colFailedScan.Name = "colFailedScan";
            this.colFailedScan.Width = 120;
            // 
            // colFailedOp
            // 
            this.colFailedOp.HeaderText = "操作";
            this.colFailedOp.Name = "colFailedOp";
            this.colFailedOp.Text = "删除";
            this.colFailedOp.UseColumnTextForButtonValue = true;
            // 
            // dgvPass
            // 
            this.dgvPass.AllowUserToAddRows = false;
            this.dgvPass.AllowUserToDeleteRows = false;
            this.dgvPass.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPass.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPassUUT,
            this.colPassDatecode,
            this.colPassScan,
            this.colPassTime});
            this.dgvPass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPass.Location = new System.Drawing.Point(3, 18);
            this.dgvPass.Name = "dgvPass";
            this.dgvPass.ReadOnly = true;
            this.dgvPass.RowHeadersVisible = false;
            this.dgvPass.RowTemplate.Height = 24;
            this.dgvPass.Size = new System.Drawing.Size(494, 279);
            this.dgvPass.TabIndex = 10;
            // 
            // colPassUUT
            // 
            this.colPassUUT.HeaderText = "UUT条码";
            this.colPassUUT.Name = "colPassUUT";
            this.colPassUUT.ReadOnly = true;
            this.colPassUUT.Width = 120;
            // 
            // colPassDatecode
            // 
            this.colPassDatecode.HeaderText = "Datecode";
            this.colPassDatecode.Name = "colPassDatecode";
            this.colPassDatecode.ReadOnly = true;
            // 
            // colPassScan
            // 
            this.colPassScan.HeaderText = "扫描结果";
            this.colPassScan.Name = "colPassScan";
            this.colPassScan.ReadOnly = true;
            this.colPassScan.Width = 120;
            // 
            // colPassTime
            // 
            this.colPassTime.HeaderText = "扫描时间";
            this.colPassTime.Name = "colPassTime";
            this.colPassTime.ReadOnly = true;
            // 
            // valScan
            // 
            this.valScan.AutoSize = true;
            this.valScan.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.valScan.Location = new System.Drawing.Point(147, 233);
            this.valScan.Name = "valScan";
            this.valScan.Size = new System.Drawing.Size(26, 24);
            this.valScan.TabIndex = 12;
            this.valScan.Text = "--";
            // 
            // lblScan
            // 
            this.lblScan.AutoSize = true;
            this.lblScan.Font = new System.Drawing.Font("PMingLiU", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblScan.Location = new System.Drawing.Point(29, 233);
            this.lblScan.Name = "lblScan";
            this.lblScan.Size = new System.Drawing.Size(112, 24);
            this.lblScan.TabIndex = 11;
            this.lblScan.Text = "扫描结果:";
            // 
            // gbFailed
            // 
            this.gbFailed.Controls.Add(this.dgvFailed);
            this.gbFailed.Location = new System.Drawing.Point(533, 275);
            this.gbFailed.Name = "gbFailed";
            this.gbFailed.Size = new System.Drawing.Size(500, 300);
            this.gbFailed.TabIndex = 13;
            this.gbFailed.TabStop = false;
            this.gbFailed.Text = "Failed";
            // 
            // gbPass
            // 
            this.gbPass.Controls.Add(this.dgvPass);
            this.gbPass.Location = new System.Drawing.Point(28, 275);
            this.gbPass.Name = "gbPass";
            this.gbPass.Size = new System.Drawing.Size(500, 300);
            this.gbPass.TabIndex = 14;
            this.gbPass.TabStop = false;
            this.gbPass.Text = "Pass";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(956, 249);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 15;
            this.btnSend.Text = "修正重发";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 601);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.gbPass);
            this.Controls.Add(this.gbFailed);
            this.Controls.Add(this.valScan);
            this.Controls.Add(this.lblScan);
            this.Controls.Add(this.tbScan);
            this.Controls.Add(this.valProdPartnum);
            this.Controls.Add(this.lblProdPartnum);
            this.Controls.Add(this.valProdJobnum);
            this.Controls.Add(this.lblProdJobnum);
            this.Controls.Add(this.tbProofPartnum);
            this.Controls.Add(this.lblProofPartnum);
            this.Controls.Add(this.tbProofJobnum);
            this.Controls.Add(this.lblProofJobnum);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.Text = "VOX_NS";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFailed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPass)).EndInit();
            this.gbFailed.ResumeLayout(false);
            this.gbPass.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProofJobnum;
        private System.Windows.Forms.TextBox tbProofJobnum;
        private System.Windows.Forms.TextBox tbProofPartnum;
        private System.Windows.Forms.Label lblProofPartnum;
        private System.Windows.Forms.Label lblProdJobnum;
        private System.Windows.Forms.Label valProdJobnum;
        private System.Windows.Forms.Label valProdPartnum;
        private System.Windows.Forms.Label lblProdPartnum;
        private System.Windows.Forms.TextBox tbScan;
        private System.Windows.Forms.DataGridView dgvFailed;
        private System.Windows.Forms.DataGridView dgvPass;
        private System.Windows.Forms.Label valScan;
        private System.Windows.Forms.Label lblScan;
        private System.Windows.Forms.GroupBox gbFailed;
        private System.Windows.Forms.GroupBox gbPass;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFailedUUT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFailedDatecode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFailedScan;
        private System.Windows.Forms.DataGridViewButtonColumn colFailedOp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPassUUT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPassDatecode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPassScan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPassTime;
    }
}

