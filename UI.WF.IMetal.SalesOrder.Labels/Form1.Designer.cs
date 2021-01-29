namespace UI.WF.IMetal.SalesOrder.Labels
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtSalesOrderId = new System.Windows.Forms.TextBox();
            this.btnFindItems = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colItemNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTagNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHeatNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerPartNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrint = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnGenerateLabels = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(27, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Sales Order Number:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtSalesOrderId
            // 
            this.txtSalesOrderId.Location = new System.Drawing.Point(191, 31);
            this.txtSalesOrderId.Name = "txtSalesOrderId";
            this.txtSalesOrderId.Size = new System.Drawing.Size(130, 20);
            this.txtSalesOrderId.TabIndex = 1;
            // 
            // btnFindItems
            // 
            this.btnFindItems.Location = new System.Drawing.Point(336, 31);
            this.btnFindItems.Name = "btnFindItems";
            this.btnFindItems.Size = new System.Drawing.Size(75, 23);
            this.btnFindItems.TabIndex = 2;
            this.btnFindItems.Text = "Find Items";
            this.btnFindItems.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(27, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Items Found";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItemNumber,
            this.colTagNumber,
            this.colHeatNumber,
            this.colProductCode,
            this.colCustomerPartNumber,
            this.colPrint});
            this.dataGridView1.Location = new System.Drawing.Point(30, 80);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(818, 293);
            this.dataGridView1.TabIndex = 5;
            // 
            // colItemNumber
            // 
            this.colItemNumber.HeaderText = "Item#";
            this.colItemNumber.Name = "colItemNumber";
            // 
            // colTagNumber
            // 
            this.colTagNumber.HeaderText = "TagNumber";
            this.colTagNumber.Name = "colTagNumber";
            // 
            // colHeatNumber
            // 
            this.colHeatNumber.HeaderText = "HeatNumber";
            this.colHeatNumber.Name = "colHeatNumber";
            // 
            // colProductCode
            // 
            this.colProductCode.HeaderText = "Product Code";
            this.colProductCode.Name = "colProductCode";
            this.colProductCode.Width = 250;
            // 
            // colCustomerPartNumber
            // 
            this.colCustomerPartNumber.HeaderText = "Customer Part#";
            this.colCustomerPartNumber.Name = "colCustomerPartNumber";
            // 
            // colPrint
            // 
            this.colPrint.HeaderText = "Print?";
            this.colPrint.Name = "colPrint";
            // 
            // btnGenerateLabels
            // 
            this.btnGenerateLabels.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerateLabels.Image")));
            this.btnGenerateLabels.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerateLabels.Location = new System.Drawing.Point(336, 401);
            this.btnGenerateLabels.Name = "btnGenerateLabels";
            this.btnGenerateLabels.Size = new System.Drawing.Size(157, 37);
            this.btnGenerateLabels.TabIndex = 6;
            this.btnGenerateLabels.Text = "Generate Labels";
            this.btnGenerateLabels.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 450);
            this.Controls.Add(this.btnGenerateLabels);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnFindItems);
            this.Controls.Add(this.txtSalesOrderId);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "iMetal Label Printer";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSalesOrderId;
        private System.Windows.Forms.Button btnFindItems;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTagNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHeatNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerPartNumber;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPrint;
        private System.Windows.Forms.Button btnGenerateLabels;
    }
}

