using PixiEditor.ChangeableDocument.Enums;
using PixiEditor.Models.Tools;

namespace PixiEditor.Models.Handlers.Tools;

internal interface IBlurSharpenToolHandler : IToolHandler
{
    public BlurMode Mode { get; }
    public bool Sharpen { get; }
    //public SelectionMode SelectMode { get; } // This is not needed to get the current selection.
    public DocumentScope DocumentScope { get; }
    public int ToolSize { get; }  // Isn't this already a part of the Pen Tool?
}
