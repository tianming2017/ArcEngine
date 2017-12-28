namespace MapControlApplication1_hhx
{
    partial class SelectBandsForm
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
            this.cklb_CompareHistogram = new System.Windows.Forms.CheckedListBox();
            this.btn_DrawCompareHistogram = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cklb_CompareHistogram
            // 
            this.cklb_CompareHistogram.CheckOnClick = true;
            this.cklb_CompareHistogram.FormattingEnabled = true;
            this.cklb_CompareHistogram.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cklb_CompareHistogram.Location = new System.Drawing.Point(26, 25);
            this.cklb_CompareHistogram.Name = "cklb_CompareHistogram";
            this.cklb_CompareHistogram.Size = new System.Drawing.Size(152, 196);
            this.cklb_CompareHistogram.TabIndex = 0;
            // 
            // btn_DrawCompareHistogram
            // 
            this.btn_DrawCompareHistogram.Location = new System.Drawing.Point(197, 112);
            this.btn_DrawCompareHistogram.Name = "btn_DrawCompareHistogram";
            this.btn_DrawCompareHistogram.Size = new System.Drawing.Size(75, 23);
            this.btn_DrawCompareHistogram.TabIndex = 1;
            this.btn_DrawCompareHistogram.Text = "绘制";
            this.btn_DrawCompareHistogram.UseVisualStyleBackColor = true;
            this.btn_DrawCompareHistogram.Click += new System.EventHandler(this.btn_DrawCompareHistogram_Click);
            // 
            // SelectBandsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btn_DrawCompareHistogram);
            this.Controls.Add(this.cklb_CompareHistogram);
            this.Name = "SelectBandsForm";
            this.Text = "选择波段";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox cklb_CompareHistogram;
        private System.Windows.Forms.Button btn_DrawCompareHistogram;
    }
}