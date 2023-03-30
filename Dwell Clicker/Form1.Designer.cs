namespace Dwell_Clicker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _positionTrackerTimer = new System.Windows.Forms.Timer(components);
            animationTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // _positionTrackerTimer
            // 
            _positionTrackerTimer.Enabled = true;
            _positionTrackerTimer.Tick += PositionTrackerTimer_Tick;
            // 
            // animationTimer
            // 
            animationTimer.Tick += animationTimer_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Lime;
            ClientSize = new Size(800, 450);
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.None;
            Name = "Form1";
            Text = "Dwell Clicker";
            TopMost = true;
            TransparencyKey = Color.Lime;
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer _positionTrackerTimer;
        private System.Windows.Forms.Timer animationTimer;
    }
}