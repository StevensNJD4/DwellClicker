public class DwellClickerButton : Button
{
    public Dwell_Clicker.ClickState ClickState { get; private set; }
    public bool IsDefault { get; private set; }
    public bool IsTemporary { get; private set; }

    public DwellClickerButton(Dwell_Clicker.ClickState clickState, Point location, Size size, Color backColor, Color foreColor, bool isDefault = false)
    {
        ClickState = clickState;
        IsDefault = isDefault;
        Location = location;
        Size = size;
        UpdateButtonColors();
        ForeColor = foreColor;
    }

    public void SetAsDefault(bool isDefault)
    {
        IsDefault = isDefault;
        UpdateButtonColors();
    }

    public void SetAsTemporary(bool isTemporary)
    {
        IsTemporary = isTemporary;
        UpdateButtonColors();
    }

    private void UpdateButtonColors()
    {
        if (IsDefault)
        {
            BackColor = Color.Red;
        }
        else if (IsTemporary)
        {
            BackColor = Color.Blue;
        }
        else
        {
            BackColor = SystemColors.Control;
        }
    }
}
