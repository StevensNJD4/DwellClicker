using System.Diagnostics;

namespace Dwell_Clicker
{
    public enum ClickState
    {
        Off,
        LeftClick,
        RightClick,
        DoubleClick,
        MiddleClick,
        Drag
    }

    public partial class Form1 : Form
    {
        private Button _moveButton;
        private Button _settingsButton;

        private DwellClickerButton _onOffButton;
        private DwellClickerButton _leftClickButton;
        private DwellClickerButton _doubleClickButton;
        private DwellClickerButton _rightClickButton;
        private DwellClickerButton _middleClickButton;
        private DwellClickerButton _dragButton;
        private DwellClickerButton _previousTemporaryButton;

        private ClickState _defaultClickState;
        private ClickState _temporaryClickState;
        private bool _clickerEnabled;
        private bool _clickPerformed;

        private ClickHandler _clickHandler;
        private DateTime _dwellStartTime;
        private Point _previousCursorPosition;
        private bool _isDragging;
        private Point _dragStart;

        private int _dwellTime = 450; // Time to wait in milliseconds
        private int _dwellTimeOnOff = 1000; // Time to wait in milliseconds
        private int _movementThreshold = 10; // Pixels of allowed movement

        private const int AnimationStep = 10;
        private bool _animateInDirection;
        private int _maxY;
        private int _minY;
        private int _peep = 15;

        private SettingsDialog _settingsDialog;

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_NOACTIVATE = 0x08000000;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_NOACTIVATE;
                return cp;
            }
        }

        private void AnimateIn()
        {
            _animateInDirection = true;
            _animationProgress = 0;
            animationTimer.Start();
        }

        private void AnimateOut()
        {
            _animateInDirection = false;
            _animationProgress = 0;
            animationTimer.Start();
        }

        public Form1()
        {
            InitializeComponent();
            InitializeButtons();
            _clickHandler = new ClickHandler();
            _clickHandler.ClickPerformed += ClickHandler_ClickPerformed;
            _dwellStartTime = DateTime.Now;
            _previousCursorPosition = Cursor.Position;
            _clickPerformed = false;

            // Set clickerEnabled to true and change the On/Off button color
            _clickerEnabled = true;
            _onOffButton.BackColor = Color.Green;

            // Set the default button state to LeftClick and change the button color
            _defaultClickState = ClickState.LeftClick;
            _leftClickButton.SetAsDefault(true);

            // Set the form size to fit the buttons
            int formWidth = _moveButton.Right + 10; // 10 pixels padding to the right of the last button
            int formHeight = _moveButton.Bottom + 10; // 10 pixels padding below the buttons
            this.ClientSize = new Size(formWidth, formHeight);

        }

        private void InitializeButtons()
        {
            _onOffButton = new DwellClickerButton(ClickState.Off, new Point(10, 5), new Size(40, 50), SystemColors.Control, Color.Black) { Text = "On/Off", Name = "OnOffButton" };
            _leftClickButton = new DwellClickerButton(ClickState.LeftClick, new Point(50, 5), new Size(40, 50), SystemColors.Control, SystemColors.ControlText, isDefault: true) { Text = "Left", Name = "LeftClickButton" };
            _doubleClickButton = new DwellClickerButton(ClickState.DoubleClick, new Point(90, 5), new Size(40, 50), SystemColors.Control, Color.Black) { Text = "Double", Name = "DoubleClickButton" };
            _rightClickButton = new DwellClickerButton(ClickState.RightClick, new Point(130, 5), new Size(40, 50), SystemColors.Control, Color.Black) { Text = "Right", Name = "RightClickButton" };
            _dragButton = new DwellClickerButton(ClickState.Drag, new Point(170, 5), new Size(40, 50), SystemColors.Control, Color.Black) { Text = "Drag", Name = "DragButton" };
            _middleClickButton = new DwellClickerButton(ClickState.MiddleClick, new Point(210, 5), new Size(40, 50), SystemColors.Control, Color.Black) { Text = "Middle", Name = "MiddleClickButton" };
            _settingsButton = new Button { Text = "Settings", Location = new Point(250, 5), Size = new Size(40, 50), BackColor = SystemColors.Control, ForeColor = Color.Black, Name = "SettingsButton" };
            _moveButton = new Button { Text = "Move", Location = new Point(290, 5), Size = new Size(40, 50), BackColor = SystemColors.Control, ForeColor = Color.Black, Name = "MoveButton" };
            _moveButton.MouseDown += MoveButton_MouseDown;
            _moveButton.MouseMove += MoveButton_MouseMove;
            _moveButton.MouseUp += MoveButton_MouseUp;
            _onOffButton.Click += (sender, e) => ToggleClickerEnabled();
            _leftClickButton.Click += (sender, e) => SetDefaultClickState(ClickState.LeftClick);
            _doubleClickButton.Click += (sender, e) => SetDefaultClickState(ClickState.DoubleClick);
            _rightClickButton.Click += (sender, e) => SetDefaultClickState(ClickState.RightClick);
            _middleClickButton.Click += (sender, e) => SetDefaultClickState(ClickState.MiddleClick);
            _settingsButton.Click += _settingsButton_Click;
            _dragButton.Click += (sender, e) => SetDefaultClickState(ClickState.Drag);

            Controls.AddRange(new Control[] { _onOffButton, _leftClickButton, _doubleClickButton, _rightClickButton, _dragButton, _middleClickButton, _settingsButton, _moveButton });
        }

        private void ToggleClickerEnabled()
        {
            _clickerEnabled = !_clickerEnabled;
            _onOffButton.BackColor = _clickerEnabled ? Color.Green : Color.Red;
            ToggleButtonsVisibility(_clickerEnabled);
        }

        private void ToggleButtonsVisibility(bool visible)
        {
            _leftClickButton.Visible = visible;
            _doubleClickButton.Visible = visible;
            _rightClickButton.Visible = visible;
            _dragButton.Visible = visible;
            _middleClickButton.Visible = visible;
            _moveButton.Visible = visible;
            _settingsButton.Visible = visible;

            int formWidth = visible ? _moveButton.Right + 10 : _onOffButton.Right; // 10 pixels padding to the right of the last button
            int formHeight = _onOffButton.Bottom + 10; // 10 pixels padding below the buttons
            this.ClientSize = new Size(formWidth, formHeight);
        }

        private void SetDefaultClickState(ClickState newClickState)
        {
            if (_defaultClickState == newClickState)
            {
                // If it's already the default state, do nothing
                return;
            }
            else if (_temporaryClickState == newClickState)
            {
                // If it's in a temporary state, make it default
                _defaultClickState = newClickState;
                _temporaryClickState = ClickState.Off;
            }
            else
            {
                // If it's not in a temporary or default state, make it temporary
                _temporaryClickState = newClickState;
            }

            UpdateButtonStates();

            // If the new default state is the same as the temporary state, reset the temporary state
            if (_defaultClickState == newClickState && _temporaryClickState == newClickState)
            {
                _temporaryClickState = ClickState.Off;
            }
        }

        private void UpdateButtonStates()
        {
            _leftClickButton.SetAsDefault(_defaultClickState == ClickState.LeftClick);
            _rightClickButton.SetAsDefault(_defaultClickState == ClickState.RightClick);
            _doubleClickButton.SetAsDefault(_defaultClickState == ClickState.DoubleClick);
            _middleClickButton.SetAsDefault(_defaultClickState == ClickState.MiddleClick);
            _dragButton.SetAsDefault(_defaultClickState == ClickState.Drag);

            // Reset previous temporary button state
            if (_previousTemporaryButton != null)
            {
                _previousTemporaryButton.SetAsTemporary(false);
            }

            // Set the new temporary button state and update _previousTemporaryButton
            if (_temporaryClickState == ClickState.LeftClick)
            {
                _leftClickButton.SetAsTemporary(true);
                _previousTemporaryButton = _leftClickButton;
            }
            else if (_temporaryClickState == ClickState.RightClick)
            {
                _rightClickButton.SetAsTemporary(true);
                _previousTemporaryButton = _rightClickButton;
            }
            else if (_temporaryClickState == ClickState.DoubleClick)
            {
                _doubleClickButton.SetAsTemporary(true);
                _previousTemporaryButton = _doubleClickButton;
            }
            else if (_temporaryClickState == ClickState.Drag)
            {
                _dragButton.SetAsTemporary(true);
                _previousTemporaryButton = _dragButton;
            }
            else if (_temporaryClickState == ClickState.MiddleClick)
            {
                _middleClickButton.SetAsTemporary(true);
                _previousTemporaryButton = _middleClickButton;
            }
            else
            {
                _previousTemporaryButton = null;
            }
        }


        private void MoveButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _dragStart = e.Location;
            }
        }

        private void MoveButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point newLocation = this.Location;
                newLocation.Offset(e.Location.X - _dragStart.X, e.Location.Y - _dragStart.Y);

                // Check if the new location is within the screen bounds
                if (newLocation.X >= 0)
                {
                    newLocation.Y = 0;
                    this.Location = newLocation;
                }
            }
        }

        private void MoveButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = false;
                SaveFormLocation(); // Save the location when the user is done moving the form
            }
        }

        private List<Point> _cursorHistory = new List<Point>();

        private void PositionTrackerTimer_Tick(object sender, EventArgs e)
        {
            double distanceToForm = DistanceToForm();
            if (distanceToForm <= 60 && !animationTimer.Enabled)
            {
                if (!_animateInDirection)
                {
                    AnimateIn();
                }
            }
            else if (distanceToForm > 60 && !animationTimer.Enabled)
            {
                if (_animateInDirection)
                {
                    AnimateOut();
                }
            }

            if (!_clickerEnabled && !IsCursorOverOnOffButon())
            {
                return;
            }

            Point currentCursorPosition = Cursor.Position;

            // Add the current cursor position to the history
            _cursorHistory.Add(currentCursorPosition);

            // Check if all points in the cursor history are within the movement threshold
            Point averagePosition = CalculateAveragePosition(_cursorHistory);
            bool allPointsWithinThreshold = _cursorHistory.All(p => Distance(p, averagePosition) <= _movementThreshold);

            if (!allPointsWithinThreshold)
            {
                _dwellStartTime = DateTime.Now;
                _clickPerformed = false;
                _cursorHistory.Clear();
            }

            // If the cursor history is long enough, perform the dwell click
            TimeSpan timeElapsed = DateTime.Now - _dwellStartTime;
            bool isCursorOverButton = IsCursorOverOnOffButon();
            int dwellThreshold = isCursorOverButton ? _dwellTimeOnOff : _dwellTime;

            if (!_clickPerformed && timeElapsed.TotalMilliseconds >= dwellThreshold)
            {
                if (_cursorHistory.Count > 0)
                {
                    PerformDwellClick();
                    _cursorHistory.Clear();
                }
            }
        }

        private Point CalculateAveragePosition(List<Point> points)
        {
            int xSum = points.Sum(p => p.X);
            int ySum = points.Sum(p => p.Y);
            return new Point(xSum / points.Count, ySum / points.Count);
        }

        private void PerformDwellClick()
        {
            if (IsCursorOverClickMethodButton())
            {
                _clickHandler.PerformClick(ClickState.LeftClick);
            }
            else if (!_clickerEnabled || IsCursorOverOnOffButon())
            {
                return;
            }
            else
            {
                if (_temporaryClickState != ClickState.Off)
                {
                    _clickHandler.PerformClick(_temporaryClickState);
                    _temporaryClickState = ClickState.Off;
                }
                else if (_clickHandler.IsDragging())
                {
                    _clickHandler.PerformClick(ClickState.Drag);
                }
                else
                {
                    _clickHandler.PerformClick(_defaultClickState);
                }
            }

            _clickPerformed = true;
        }

        private double Distance(Point p1, Point p2)
        {
            int xDiff = p1.X - p2.X;
            int yDiff = p1.Y - p2.Y;

            return Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
        }

        private void SaveFormLocation()
        {
            Properties.Settings.Default.posX = this.Location.X;
            Properties.Settings.Default.posY = this.Location.Y;
            Properties.Settings.Default.Save();
        }

        private void LoadFormLocation()
        {
            int x = (int)Properties.Settings.Default.posX;
            int y = (int)Properties.Settings.Default.posY;

            this.Location = new Point(x, y);
        }

        private void ClickHandler_ClickPerformed(object sender, EventArgs e)
        {
            if (_previousTemporaryButton != null && _temporaryClickState != ClickState.Drag)
            {
                _previousTemporaryButton.SetAsTemporary(false);
                _previousTemporaryButton = null;
            }
        }

        public bool IsCursorOverClickMethodButton()
        {
            if (_clickerEnabled)
                return _onOffButton.ClientRectangle.Contains(_onOffButton.PointToClient(Cursor.Position)) ||
                       _leftClickButton.ClientRectangle.Contains(_leftClickButton.PointToClient(Cursor.Position)) ||
                       _rightClickButton.ClientRectangle.Contains(_rightClickButton.PointToClient(Cursor.Position)) ||
                       _dragButton.ClientRectangle.Contains(_dragButton.PointToClient(Cursor.Position)) ||
                       _doubleClickButton.ClientRectangle.Contains(_doubleClickButton.PointToClient(Cursor.Position)) ||
                       _middleClickButton.ClientRectangle.Contains(_middleClickButton.PointToClient(Cursor.Position)) ||
                       _settingsButton.ClientRectangle.Contains(_settingsButton.PointToClient(Cursor.Position)) ||
                       _onOffButton.ClientRectangle.Contains(_onOffButton.PointToClient(Cursor.Position));

            return _onOffButton.ClientRectangle.Contains(_onOffButton.PointToClient(Cursor.Position));
        }

        public bool IsCursorOverOnOffButon()
        {
            return _onOffButton.ClientRectangle.Contains(_onOffButton.PointToClient(Cursor.Position));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFormLocation();

            _maxY = -10;// this.Top;
            _minY = (this.Top - _onOffButton.Height) + _peep;

            _dwellTime = (int)Properties.Settings.Default.DwellTime;
            _dwellTimeOnOff = (int)Properties.Settings.Default.DwellTimeOnOff;
            _movementThreshold = Properties.Settings.Default.MovementThreshold;

            AnimateOut();
        }

        private double _animationProgress;

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            double AnimationStepIn = AnimationStep * 3; // Double the speed for AnimateIn
            double AnimationStepOut = AnimationStep;

            double step = _animateInDirection ? AnimationStepIn : AnimationStepOut;
            _animationProgress += step / 100.0;

            if (_animationProgress > 1)
            {
                _animationProgress = 1;
                animationTimer.Stop();
            }

            this.Top = (int)Lerp(_animateInDirection ? _minY : _maxY, _animateInDirection ? _maxY : _minY, _animationProgress);
            Debug.WriteLine(this.Top);
        }

        private double Lerp(double a, double b, double t)
        {
            return a + (b - a) * t;
        }

        private double DistanceToForm()
        {
            Point cursorPosition = Cursor.Position;
            Rectangle formBounds = this.Bounds;

            if (formBounds.Contains(cursorPosition))
            {
                return 0;
            }

            double xDist = Math.Max(Math.Max(formBounds.Left - cursorPosition.X, cursorPosition.X - formBounds.Right), 0);
            double yDist = Math.Max(Math.Max(formBounds.Top - cursorPosition.Y, cursorPosition.Y - formBounds.Bottom), 0);

            return Math.Sqrt(xDist * xDist + yDist * yDist);
        }

        private void _settingsButton_Click(object sender, EventArgs e)
        {
            if (_settingsDialog == null || _settingsDialog.IsDisposed)
            {
                _settingsDialog = new SettingsDialog();
                _settingsDialog.DwellTimeChanged += SettingsDialog_DwellTimeChanged;
                _settingsDialog.DwellTimeOnOffChanged += SettingsDialog_DwellTimeOnOffChanged;
                _settingsDialog.MovementThresholdChanged += SettingsDialog_MovementThresholdChanged;
                _settingsDialog.Show(this);
            }
            else
            {
                _settingsDialog.BringToFront();
            }
        }

        private void SettingsDialog_DwellTimeChanged(object sender, EventArgs e)
        {
            if (sender is SettingsDialog settingsDialog)
            {
                _dwellTime = settingsDialog.DwellTime;
            }
        }

        private void SettingsDialog_MovementThresholdChanged(object sender, EventArgs e)
        {
            if (sender is SettingsDialog settingsDialog)
            {
                _movementThreshold = settingsDialog.MovementThreshold;
            }
        }

        private void SettingsDialog_DwellTimeOnOffChanged(object sender, EventArgs e)
        {
            if (sender is SettingsDialog settingsDialog)
            {
                _dwellTimeOnOff = settingsDialog.DwellTimeOnOff;
            }
        }
    }
}