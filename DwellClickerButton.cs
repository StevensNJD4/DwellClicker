public class DwellClickerButton : Button
{
    public ClickState ClickState { get; private set; }
    public bool IsDefault { get; private set; }

    public DwellClickerButton(ClickState clickState, Point location, Size size, Color backColor, Color foreColor)
    {
        ClickState = clickState;
        Location = location;
        Size = size;
        BackColor = backColor;
        ForeColor = foreColor;
    }

    public void SetAsDefault(bool isDefault)
    {
        IsDefault = isDefault;
        BackColor = isDefault ? Color.Red : SystemColors.Control;
    }

    public void SetAsTemporary()
    {
        BackColor = Color.Blue;
    }
}