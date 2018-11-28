namespace RvtVa3c
{
    partial class ParameterFilter
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
            this.checkUncheck = new System.Windows.Forms.Button();
            this.export_button = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.Description = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkUncheck
            // 
            this.checkUncheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkUncheck.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkUncheck.Location = new System.Drawing.Point(16, 550);
            this.checkUncheck.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkUncheck.Name = "checkUncheck";
            this.checkUncheck.Size = new System.Drawing.Size(185, 41);
            this.checkUncheck.TabIndex = 4;
            this.checkUncheck.Text = "Check / Uncheck All";
            this.checkUncheck.UseVisualStyleBackColor = true;
            this.checkUncheck.Click += new System.EventHandler(this.checkUncheck_Click);
            // 
            // export_button
            // 
            this.export_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.export_button.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.export_button.Location = new System.Drawing.Point(527, 550);
            this.export_button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.export_button.Name = "export_button";
            this.export_button.Size = new System.Drawing.Size(124, 41);
            this.export_button.TabIndex = 5;
            this.export_button.Text = "EXPORT";
            this.export_button.UseVisualStyleBackColor = true;
            this.export_button.Click += new System.EventHandler(this.export_button_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(673, 550);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(116, 41);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "CANCEL";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // Description
            // 
            this.Description.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Description.Location = new System.Drawing.Point(12, 11);
            this.Description.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(531, 27);
            this.Description.TabIndex = 7;
            this.Description.Text = "Select which parameters should be included with each category";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(212, 560);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(239, 21);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Check / Uncheck current tab only";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // ParameterFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(805, 594);
            this.ControlBox = false;
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.export_button);
            this.Controls.Add(this.checkUncheck);
            this.Controls.Add(this.Description);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximumSize = new System.Drawing.Size(1194, 641);
            this.MinimumSize = new System.Drawing.Size(821, 641);
            this.Name = "ParameterFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parameter Filter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button checkUncheck;
        private System.Windows.Forms.Button export_button;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label Description;
        private System.Windows.Forms.CheckBox checkBox1;



    }
}