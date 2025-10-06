using System.ComponentModel;

namespace PixiEditor.Models.Tools;

public enum BlurMode
{
    [Description("BLUR_MODE_FAST")]
    Fast,  // Default
    [Description("BLUR_MODE_BOX")]
    Box,
    [Description("BLUR_MODE_GAUSSIAN")]
    Gaussian,
}
