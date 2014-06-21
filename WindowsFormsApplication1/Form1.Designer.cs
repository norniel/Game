namespace WindowsFormsApplication1
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ilPanel1 = new ILNumerics.Drawing.ILPanel();
            this.tbPower = new System.Windows.Forms.TrackBar();
            this.tbHeight = new System.Windows.Forms.TrackBar();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.tbPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            this.SuspendLayout();
            // 
            // ilPanel1
            // 
            this.ilPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ilPanel1.Driver = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.ilPanel1.Editor = null;
            this.ilPanel1.Location = new System.Drawing.Point(0, 131);
            this.ilPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ilPanel1.Name = "ilPanel1";
            this.ilPanel1.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("ilPanel1.Rectangle")));
            this.ilPanel1.ShowUIControls = false;
            this.ilPanel1.Size = new System.Drawing.Size(643, 322);
            this.ilPanel1.TabIndex = 0;
            this.ilPanel1.Load += new System.EventHandler(this.ilPanel1_Load);
            // 
            // tbPower
            // 
            this.tbPower.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbPower.Location = new System.Drawing.Point(0, 0);
            this.tbPower.Maximum = 8;
            this.tbPower.Minimum = 1;
            this.tbPower.Name = "tbPower";
            this.tbPower.Size = new System.Drawing.Size(643, 45);
            this.tbPower.TabIndex = 1;
            this.tbPower.Value = 3;
            this.tbPower.ValueChanged += new System.EventHandler(this.trackBar3_ValueChanged);
            // 
            // tbHeight
            // 
            this.tbHeight.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbHeight.Location = new System.Drawing.Point(0, 45);
            this.tbHeight.Maximum = 256;
            this.tbHeight.Minimum = 1;
            this.tbHeight.Name = "tbHeight";
            this.tbHeight.Size = new System.Drawing.Size(643, 45);
            this.tbHeight.TabIndex = 2;
            this.tbHeight.Value = 16;
            this.tbHeight.Scroll += new System.EventHandler(this.tbHeight_Scroll);
            this.tbHeight.ValueChanged += new System.EventHandler(this.trackBar3_ValueChanged);
            // 
            // trackBar3
            // 
            this.trackBar3.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBar3.Location = new System.Drawing.Point(0, 90);
            this.trackBar3.Minimum = 1;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new System.Drawing.Size(643, 45);
            this.trackBar3.TabIndex = 3;
            this.trackBar3.Value = 3;
            this.trackBar3.ValueChanged += new System.EventHandler(this.trackBar3_ValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 453);
            this.Controls.Add(this.trackBar3);
            this.Controls.Add(this.tbHeight);
            this.Controls.Add(this.tbPower);
            this.Controls.Add(this.ilPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ILNumerics.Drawing.ILPanel ilPanel1;
        private System.Windows.Forms.TrackBar tbPower;
        private System.Windows.Forms.TrackBar tbHeight;
        private System.Windows.Forms.TrackBar trackBar3;
    }
}

