namespace Dwell_Clicker
{
    partial class SettingsDialog
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
            _dwellLabel = new Label();
            _dwellSlider = new TrackBar();
            _movementLabel = new Label();
            _movementSlider = new TrackBar();
            _dwellTimeOnOffLabel = new Label();
            _dwellTimeOnOffSlider = new TrackBar();
            _closeButton = new Button();
            ((System.ComponentModel.ISupportInitialize)_dwellSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_movementSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_dwellTimeOnOffSlider).BeginInit();
            SuspendLayout();
            // 
            // _dwellLabel
            // 
            _dwellLabel.AutoSize = true;
            _dwellLabel.Location = new Point(18, 5);
            _dwellLabel.Name = "_dwellLabel";
            _dwellLabel.Size = new Size(137, 15);
            _dwellLabel.TabIndex = 0;
            _dwellLabel.Text = "Dwell Time (1 Second(s))";
            // 
            // _dwellSlider
            // 
            _dwellSlider.LargeChange = 300;
            _dwellSlider.Location = new Point(23, 25);
            _dwellSlider.Maximum = 2000;
            _dwellSlider.Minimum = 100;
            _dwellSlider.Name = "_dwellSlider";
            _dwellSlider.Size = new Size(120, 45);
            _dwellSlider.SmallChange = 100;
            _dwellSlider.TabIndex = 1;
            _dwellSlider.TickFrequency = 100;
            _dwellSlider.Value = 1000;
            _dwellSlider.Scroll += _dwellSlider_Scroll;
            // 
            // _movementLabel
            // 
            _movementLabel.AutoSize = true;
            _movementLabel.Location = new Point(18, 70);
            _movementLabel.Name = "_movementLabel";
            _movementLabel.Size = new Size(159, 15);
            _movementLabel.TabIndex = 2;
            _movementLabel.Text = "Movement Threshold (10 px)";
            // 
            // _movementSlider
            // 
            _movementSlider.LargeChange = 3;
            _movementSlider.Location = new Point(23, 90);
            _movementSlider.Maximum = 20;
            _movementSlider.Minimum = 2;
            _movementSlider.Name = "_movementSlider";
            _movementSlider.Size = new Size(120, 45);
            _movementSlider.TabIndex = 3;
            _movementSlider.TickFrequency = 10;
            _movementSlider.Value = 10;
            _movementSlider.Scroll += _movementSlider_Scroll;
            // 
            // _dwellTimeOnOffLabel
            // 
            _dwellTimeOnOffLabel.AutoSize = true;
            _dwellTimeOnOffLabel.Location = new Point(18, 135);
            _dwellTimeOnOffLabel.Name = "_dwellTimeOnOffLabel";
            _dwellTimeOnOffLabel.Size = new Size(178, 15);
            _dwellTimeOnOffLabel.TabIndex = 4;
            _dwellTimeOnOffLabel.Text = "On/Off Dwell Time (1 Second(s))";
            // 
            // _dwellTimeOnOffSlider
            // 
            _dwellTimeOnOffSlider.LargeChange = 300;
            _dwellTimeOnOffSlider.Location = new Point(23, 155);
            _dwellTimeOnOffSlider.Maximum = 2000;
            _dwellTimeOnOffSlider.Minimum = 100;
            _dwellTimeOnOffSlider.Name = "_dwellTimeOnOffSlider";
            _dwellTimeOnOffSlider.Size = new Size(120, 45);
            _dwellTimeOnOffSlider.SmallChange = 100;
            _dwellTimeOnOffSlider.TabIndex = 5;
            _dwellTimeOnOffSlider.TickFrequency = 100;
            _dwellTimeOnOffSlider.Value = 1000;
            _dwellTimeOnOffSlider.Scroll += _dwellTimeOnOffSlider_Scroll;
            // 
            // _closeButton
            // 
            _closeButton.Location = new Point(199, 180);
            _closeButton.Name = "_closeButton";
            _closeButton.Size = new Size(75, 23);
            _closeButton.TabIndex = 6;
            _closeButton.Text = "Close";
            _closeButton.UseVisualStyleBackColor = true;
            _closeButton.Click += _closeButton_Click;
            // 
            // SettingsDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = _closeButton;
            ClientSize = new Size(286, 218);
            ControlBox = false;
            Controls.Add(_closeButton);
            Controls.Add(_dwellTimeOnOffSlider);
            Controls.Add(_dwellTimeOnOffLabel);
            Controls.Add(_movementSlider);
            Controls.Add(_movementLabel);
            Controls.Add(_dwellSlider);
            Controls.Add(_dwellLabel);
            Name = "SettingsDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)_dwellSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)_movementSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)_dwellTimeOnOffSlider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label _dwellLabel;
        private TrackBar _dwellSlider;
        private Label _movementLabel;
        private TrackBar _movementSlider;
        private Label _dwellTimeOnOffLabel;
        private TrackBar _dwellTimeOnOffSlider;
        private Button _closeButton;
    }
}