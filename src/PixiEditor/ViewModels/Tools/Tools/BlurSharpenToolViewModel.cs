using Avalonia.Input;
using PixiEditor.ChangeableDocument.Changeables.Graph.Nodes;
using Drawie.Backend.Core.Numerics;
using PixiEditor.Models.Commands.Attributes.Commands;
using PixiEditor.Models.Handlers;
using PixiEditor.Models.Handlers.Tools;
using PixiEditor.Models.Tools;
using Drawie.Numerics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PixiEditor.Models.Handlers.Toolbars;
using PixiEditor.UI.Common.Fonts;
using PixiEditor.UI.Common.Localization;
using PixiEditor.ViewModels.Tools.ToolSettings.Toolbars;
using PixiEditor.Views.Overlays.BrushShapeOverlay;

namespace PixiEditor.ViewModels.Tools.Tools;

[Command.Tool(Key = Key.B)]
internal class BlurSharpenToolViewModel : ToolViewModel, IBlurSharpenToolHandler
{
    private readonly string defaultActionDisplay = "BLUR_SHARPEN_TOOL_ACTION_DISPLAY_DEFAULT";
    private int _correctionFactor;
    public override string ToolNameLocalizationKey => "BLUR_SHARPEN_TOOL";

    public BlurSharpenToolViewModel()
    {
        ActionDisplay = defaultActionDisplay;
        Toolbar = ToolbarFactory.Create<BlurSharpenToolViewModel, BlurSharpenToolbar>(this);
    }

    public override bool IsErasable => false;
    public override LocalizedString Tooltip => new LocalizedString("BLUR_SHARPEN_TOOL_TOOLTIP", Shortcut);

    // TODO: Toggle based on blur type.
    public override BrushShape FinalBrushShape => BrushShape == PaintBrushShape.Square ? Views.Overlays.BrushShapeOverlay.BrushShape.Square : Views.Overlays.BrushShapeOverlay.BrushShape.CirclePixelated;

    public override string DefaultIcon => PixiPerfectIcons.Ghost;  // TODO: Different icon.

    // Blurring is undefined for vector and other types.
    public override Type[]? SupportedLayerTypes { get; } = { typeof(IRasterLayerHandler) };
    
    [Settings.Enum("SCOPE_LABEL")]
    public DocumentScope DocumentScope => GetValue<DocumentScope>();

    [Settings.Inherited]
    public int ToolSize => GetValue<int>();
    
    [Settings.Float("STRENGTH_LABEL", 50, 0, 100)]
    public float BlurSharpenStrength => GetValue<float>();

    [Settings.Enum("MODE_LABEL")]
    public BlurMode Mode => GetValue<BlurMode>();

    [Settings.Enum("PAINT_SHAPE_SETTING", PaintBrushShape.Circle, Notify = nameof(BrushShapeChanged))]
    public PaintBrushShape BrushShape
    {
        get => GetValue<PaintBrushShape>();
        set
        {
            SetValue(value);
            OnPropertyChanged(nameof(FinalBrushShape));
        }
    }
    
    // TODO: Should this be null?  We can't really create on an empty layer.
    public override Type LayerTypeToCreateOnEmptyUse { get; } = typeof(ImageLayerNode);

    public bool Sharpen { get; private set; } = false;
    
    public override void KeyChanged(bool ctrlIsDown, bool shiftIsDown, bool altIsDown, Key argsKey)
    {
        if (!ctrlIsDown)
        {
            ActionDisplay = defaultActionDisplay;
            Sharpen = false;
        }
        else
        {
            ActionDisplay = "BLURSHARPEN_TOOL_ACTION_DISPLAY_CTRL";
            Sharpen = true;
        }
    }

    public override void UseTool(VecD pos)
    {
        ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument?.Tools.UseBlurSharpenTool();
    }

    private void BrushShapeChanged()
    {
        OnPropertyChanged(nameof(FinalBrushShape));
    }
}
