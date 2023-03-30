using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dwell_Clicker
{
    public partial class SettingsDialog : Form
    {
        private const String _dwellSliderName = "Dwell Time ({0:0.00} Seconds)";
        private const String _movementThresholdSliderName = "Movement Threshold ({0} Pixels)";
        private const String _dwellTimeOnOffSliderName = "Dwell Time On/Off ({0:0.00} Seconds)";

        public int DwellTime => _dwellSlider.Value;
        public int MovementThreshold => _movementSlider.Value;
        public int DwellTimeOnOff => _dwellTimeOnOffSlider.Value;

        public event EventHandler DwellTimeChanged;
        public event EventHandler MovementThresholdChanged;
        public event EventHandler DwellTimeOnOffChanged;

        public SettingsDialog()
        {
            InitializeComponent();

            _dwellSlider.Value = (int)Properties.Settings.Default.DwellTime;
            _movementSlider.Value = Properties.Settings.Default.MovementThreshold;
            _dwellTimeOnOffSlider.Value = (int)Properties.Settings.Default.DwellTimeOnOff;

            updateLabels();
        }

        private void _dwellSlider_Scroll(object sender, EventArgs e)
        {
            Properties.Settings.Default.DwellTime = _dwellSlider.Value;
            Properties.Settings.Default.Save();
            updateLabels();
            DwellTimeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void _movementSlider_Scroll(object sender, EventArgs e)
        {
            Properties.Settings.Default.MovementThreshold = _movementSlider.Value;
            Properties.Settings.Default.Save();
            updateLabels();
            MovementThresholdChanged?.Invoke(this, EventArgs.Empty);
        }

        private void _dwellTimeOnOffSlider_Scroll(object sender, EventArgs e)
        {
            Properties.Settings.Default.DwellTimeOnOff = _dwellTimeOnOffSlider.Value;
            Properties.Settings.Default.Save();
            updateLabels();
            DwellTimeOnOffChanged?.Invoke(this, EventArgs.Empty);
        }

        private void updateLabels()
        {
            _dwellLabel.Text = String.Format(_dwellSliderName, Math.Round((decimal)_dwellSlider.Value / 1000, 2));
            _movementLabel.Text = String.Format(_movementThresholdSliderName, _movementSlider.Value);
            _dwellTimeOnOffLabel.Text = String.Format(_dwellTimeOnOffSliderName, Math.Round((decimal)_dwellTimeOnOffSlider.Value / 1000, 2));
        }

        private void _closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
