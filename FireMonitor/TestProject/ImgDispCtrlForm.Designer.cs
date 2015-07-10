namespace TestProject
{
    partial class ImgDispCtrlForm
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
            this.imgDispCtrl1 = new MapHandle.ImgDispCtrl();
            this.SuspendLayout();
            // 
            // imgDispCtrl1
            // 
            this.imgDispCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgDispCtrl1.Location = new System.Drawing.Point(0, 0);
            this.imgDispCtrl1.Name = "imgDispCtrl1";
            this.imgDispCtrl1.Size = new System.Drawing.Size(984, 664);
            this.imgDispCtrl1.TabIndex = 0;
            // 
            // ImgDispCtrlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 664);
            this.Controls.Add(this.imgDispCtrl1);
            this.Name = "ImgDispCtrlForm";
            this.Text = "ImgDispCtrlForm";
            this.ResumeLayout(false);

        }

        #endregion

        private MapHandle.ImgDispCtrl imgDispCtrl1;
    }
}