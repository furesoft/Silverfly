namespace Silverfly.Text.Formatting;

public class FormatterConfig
{
    public int IndentSize { get; set; } = 4;
    public bool UseTabs { get; set; } = false;
    public int MaxLineLength { get; set; } = 80;
    public bool SpaceBeforeParens { get; set; } = true;
    public bool SpaceWithinParens { get; set; } = false;
    public BraceStyle BraceStyle { get; set; } = BraceStyle.SameLine;
}
