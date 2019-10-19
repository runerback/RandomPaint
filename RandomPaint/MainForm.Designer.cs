namespace RandomPaint
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
            this.paintBoard1 = new RandomPaint.PaintBoardControl();
            this.SuspendLayout();
            // 
            // paintBoard1
            // 
            this.paintBoard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paintBoard1.Location = new System.Drawing.Point(0, 0);
            this.paintBoard1.Name = "paintBoard1";
            this.paintBoard1.Size = new System.Drawing.Size(800, 450);
            this.paintBoard1.TabIndex = 0;
            this.paintBoard1.Text = "paintBoard1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.paintBoard1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private PaintBoardControl paintBoard1;
    }
}

