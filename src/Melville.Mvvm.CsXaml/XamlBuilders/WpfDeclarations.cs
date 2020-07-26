using Melville.Mvvm.CsXaml.ValueSource;

namespace Melville.Mvvm.CsXaml.XamlBuilders
{
    public static partial class WpfDeclarations
    {
        public static System.Windows.DependencyObject WithDesignerProperties_IsInDesignMode(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.ComponentModel.DesignerProperties.IsInDesignModeProperty);
            return target;
        }

        public static System.Windows.FrameworkContentElement WithStyle(
            this System.Windows.FrameworkContentElement target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkContentElement.StyleProperty);
            return target;
        }

        public static System.Windows.FrameworkContentElement WithOverridesDefaultStyle(
            this System.Windows.FrameworkContentElement target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkContentElement.OverridesDefaultStyleProperty);
            return target;
        }

        public static System.Windows.FrameworkContentElement WithName(
            this System.Windows.FrameworkContentElement target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkContentElement.NameProperty);
            return target;
        }

        public static System.Windows.FrameworkContentElement WithTag(this System.Windows.FrameworkContentElement target,
            ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkContentElement.TagProperty);
            return target;
        }

        public static System.Windows.FrameworkContentElement WithLanguage(
            this System.Windows.FrameworkContentElement target,
            ValueProxy<System.Windows.Markup.XmlLanguage>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkContentElement.LanguageProperty);
            return target;
        }

        public static System.Windows.FrameworkContentElement WithFocusVisualStyle(
            this System.Windows.FrameworkContentElement target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkContentElement.FocusVisualStyleProperty);
            return target;
        }

        public static System.Windows.FrameworkContentElement WithCursor(
            this System.Windows.FrameworkContentElement target, ValueProxy<System.Windows.Input.Cursor>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkContentElement.CursorProperty);
            return target;
        }

        public static System.Windows.FrameworkContentElement WithForceCursor(
            this System.Windows.FrameworkContentElement target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkContentElement.ForceCursorProperty);
            return target;
        }

        public static System.Windows.FrameworkContentElement WithInputScope(
            this System.Windows.FrameworkContentElement target, ValueProxy<System.Windows.Input.InputScope>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkContentElement.InputScopeProperty);
            return target;
        }

        public static System.Windows.FrameworkContentElement WithDataContext(
            this System.Windows.FrameworkContentElement target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkContentElement.DataContextProperty);
            return target;
        }

        public static System.Windows.FrameworkContentElement WithBindingGroup(
            this System.Windows.FrameworkContentElement target, ValueProxy<System.Windows.Data.BindingGroup>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkContentElement.BindingGroupProperty);
            return target;
        }

        public static System.Windows.FrameworkContentElement WithToolTip(
            this System.Windows.FrameworkContentElement target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkContentElement.ToolTipProperty);
            return target;
        }

        public static System.Windows.FrameworkContentElement WithContextMenu(
            this System.Windows.FrameworkContentElement target,
            ValueProxy<System.Windows.Controls.ContextMenu>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkContentElement.ContextMenuProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithStyle(this System.Windows.FrameworkElement target,
            ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.StyleProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithOverridesDefaultStyle(
            this System.Windows.FrameworkElement target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.OverridesDefaultStyleProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithUseLayoutRounding(this System.Windows.FrameworkElement target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.UseLayoutRoundingProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithDataContext(this System.Windows.FrameworkElement target,
            ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.DataContextProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithBindingGroup(this System.Windows.FrameworkElement target,
            ValueProxy<System.Windows.Data.BindingGroup>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.BindingGroupProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithLanguage(this System.Windows.FrameworkElement target,
            ValueProxy<System.Windows.Markup.XmlLanguage>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.LanguageProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithName(this System.Windows.FrameworkElement target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.NameProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithTag(this System.Windows.FrameworkElement target,
            ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.TagProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithInputScope(this System.Windows.FrameworkElement target,
            ValueProxy<System.Windows.Input.InputScope>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.InputScopeProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithLayoutTransform(this System.Windows.FrameworkElement target,
            ValueProxy<System.Windows.Media.Transform>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.LayoutTransformProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithWidth(this System.Windows.FrameworkElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.WidthProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithMinWidth(this System.Windows.FrameworkElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.MinWidthProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithMaxWidth(this System.Windows.FrameworkElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.MaxWidthProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithHeight(this System.Windows.FrameworkElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.HeightProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithMinHeight(this System.Windows.FrameworkElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.MinHeightProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithMaxHeight(this System.Windows.FrameworkElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.MaxHeightProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithFrameworkElement_FlowDirection(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FlowDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.FlowDirectionProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithMargin(this System.Windows.FrameworkElement target,
            ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.MarginProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithHorizontalAlignment(
            this System.Windows.FrameworkElement target, ValueProxy<System.Windows.HorizontalAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.HorizontalAlignmentProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithVerticalAlignment(this System.Windows.FrameworkElement target,
            ValueProxy<System.Windows.VerticalAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.VerticalAlignmentProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithFocusVisualStyle(this System.Windows.FrameworkElement target,
            ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.FocusVisualStyleProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithCursor(this System.Windows.FrameworkElement target,
            ValueProxy<System.Windows.Input.Cursor>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.CursorProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithForceCursor(this System.Windows.FrameworkElement target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.ForceCursorProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithToolTip(this System.Windows.FrameworkElement target,
            ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.ToolTipProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithContextMenu(this System.Windows.FrameworkElement target,
            ValueProxy<System.Windows.Controls.ContextMenu>? propValue)
        {
            propValue?.SetValue(target, System.Windows.FrameworkElement.ContextMenuProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithLocalization_Comments(
            this System.Windows.DependencyObject target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Localization.CommentsProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithLocalization_Attributes(
            this System.Windows.DependencyObject target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Localization.AttributesProperty);
            return target;
        }

        public static System.Windows.FrameworkElement WithVisualStateManager_CustomVisualStateManager(
            this System.Windows.FrameworkElement target, ValueProxy<System.Windows.VisualStateManager>? propValue)
        {
            propValue?.SetValue(target, System.Windows.VisualStateManager.CustomVisualStateManagerProperty);
            return target;
        }

        public static System.Windows.Window WithTaskbarItemInfo(this System.Windows.Window target,
            ValueProxy<System.Windows.Shell.TaskbarItemInfo>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Window.TaskbarItemInfoProperty);
            return target;
        }

        public static System.Windows.Window WithAllowsTransparency(this System.Windows.Window target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Window.AllowsTransparencyProperty);
            return target;
        }

        public static System.Windows.Window WithTitle(this System.Windows.Window target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Window.TitleProperty);
            return target;
        }

        public static System.Windows.Window WithIcon(this System.Windows.Window target,
            ValueProxy<System.Windows.Media.ImageSource>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Window.IconProperty);
            return target;
        }

        public static System.Windows.Window WithSizeToContent(this System.Windows.Window target,
            ValueProxy<System.Windows.SizeToContent>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Window.SizeToContentProperty);
            return target;
        }

        public static System.Windows.Window WithTop(this System.Windows.Window target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Window.TopProperty);
            return target;
        }

        public static System.Windows.Window WithLeft(this System.Windows.Window target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Window.LeftProperty);
            return target;
        }

        public static System.Windows.Window WithShowInTaskbar(this System.Windows.Window target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Window.ShowInTaskbarProperty);
            return target;
        }

        public static System.Windows.Window WithWindowStyle(this System.Windows.Window target,
            ValueProxy<System.Windows.WindowStyle>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Window.WindowStyleProperty);
            return target;
        }

        public static System.Windows.Window WithWindowState(this System.Windows.Window target,
            ValueProxy<System.Windows.WindowState>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Window.WindowStateProperty);
            return target;
        }

        public static System.Windows.Window WithResizeMode(this System.Windows.Window target,
            ValueProxy<System.Windows.ResizeMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Window.ResizeModeProperty);
            return target;
        }

        public static System.Windows.Window WithTopmost(this System.Windows.Window target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Window.TopmostProperty);
            return target;
        }

        public static System.Windows.Window WithShowActivated(this System.Windows.Window target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Window.ShowActivatedProperty);
            return target;
        }

        public static System.Windows.Shell.TaskbarItemInfo WithProgressState(
            this System.Windows.Shell.TaskbarItemInfo target,
            ValueProxy<System.Windows.Shell.TaskbarItemProgressState>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.TaskbarItemInfo.ProgressStateProperty);
            return target;
        }

        public static System.Windows.Shell.TaskbarItemInfo WithProgressValue(
            this System.Windows.Shell.TaskbarItemInfo target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.TaskbarItemInfo.ProgressValueProperty);
            return target;
        }

        public static System.Windows.Shell.TaskbarItemInfo WithOverlay(this System.Windows.Shell.TaskbarItemInfo target,
            ValueProxy<System.Windows.Media.ImageSource>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.TaskbarItemInfo.OverlayProperty);
            return target;
        }

        public static System.Windows.Shell.TaskbarItemInfo WithDescription(
            this System.Windows.Shell.TaskbarItemInfo target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.TaskbarItemInfo.DescriptionProperty);
            return target;
        }

        public static System.Windows.Shell.TaskbarItemInfo WithThumbnailClipMargin(
            this System.Windows.Shell.TaskbarItemInfo target, ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.TaskbarItemInfo.ThumbnailClipMarginProperty);
            return target;
        }

        public static System.Windows.Shell.TaskbarItemInfo WithThumbButtonInfos(
            this System.Windows.Shell.TaskbarItemInfo target,
            ValueProxy<System.Windows.Shell.ThumbButtonInfoCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.TaskbarItemInfo.ThumbButtonInfosProperty);
            return target;
        }

        public static System.Windows.Shell.ThumbButtonInfo WithVisibility(
            this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Windows.Visibility>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.VisibilityProperty);
            return target;
        }

        public static System.Windows.Shell.ThumbButtonInfo WithDismissWhenClicked(
            this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.DismissWhenClickedProperty);
            return target;
        }

        public static System.Windows.Shell.ThumbButtonInfo WithImageSource(
            this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Windows.Media.ImageSource>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.ImageSourceProperty);
            return target;
        }

        public static System.Windows.Shell.ThumbButtonInfo WithIsBackgroundVisible(
            this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.IsBackgroundVisibleProperty);
            return target;
        }

        public static System.Windows.Shell.ThumbButtonInfo WithDescription(
            this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.DescriptionProperty);
            return target;
        }

        public static System.Windows.Shell.ThumbButtonInfo WithIsEnabled(
            this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.IsEnabledProperty);
            return target;
        }

        public static System.Windows.Shell.ThumbButtonInfo WithIsInteractive(
            this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.IsInteractiveProperty);
            return target;
        }

        public static System.Windows.Shell.ThumbButtonInfo WithCommand(this System.Windows.Shell.ThumbButtonInfo target,
            ValueProxy<System.Windows.Input.ICommand>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.CommandProperty);
            return target;
        }

        public static System.Windows.Shell.ThumbButtonInfo WithCommandParameter(
            this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.CommandParameterProperty);
            return target;
        }

        public static System.Windows.Shell.ThumbButtonInfo WithCommandTarget(
            this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Windows.IInputElement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.CommandTargetProperty);
            return target;
        }

        public static System.Windows.Window WithWindowChrome_WindowChrome(this System.Windows.Window target,
            ValueProxy<System.Windows.Shell.WindowChrome>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.WindowChrome.WindowChromeProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithWindowChrome_IsHitTestVisibleInChrome(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.WindowChrome.IsHitTestVisibleInChromeProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithWindowChrome_ResizeGripDirection(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Shell.ResizeGripDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.WindowChrome.ResizeGripDirectionProperty);
            return target;
        }

        public static System.Windows.Shell.WindowChrome WithCaptionHeight(this System.Windows.Shell.WindowChrome target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.WindowChrome.CaptionHeightProperty);
            return target;
        }

        public static System.Windows.Shell.WindowChrome WithResizeBorderThickness(
            this System.Windows.Shell.WindowChrome target, ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.WindowChrome.ResizeBorderThicknessProperty);
            return target;
        }

        public static System.Windows.Shell.WindowChrome WithGlassFrameThickness(
            this System.Windows.Shell.WindowChrome target, ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.WindowChrome.GlassFrameThicknessProperty);
            return target;
        }

        public static System.Windows.Shell.WindowChrome WithUseAeroCaptionButtons(
            this System.Windows.Shell.WindowChrome target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.WindowChrome.UseAeroCaptionButtonsProperty);
            return target;
        }

        public static System.Windows.Shell.WindowChrome WithCornerRadius(this System.Windows.Shell.WindowChrome target,
            ValueProxy<System.Windows.CornerRadius>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.WindowChrome.CornerRadiusProperty);
            return target;
        }

        public static System.Windows.Shell.WindowChrome WithNonClientFrameEdges(
            this System.Windows.Shell.WindowChrome target,
            ValueProxy<System.Windows.Shell.NonClientFrameEdges>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shell.WindowChrome.NonClientFrameEdgesProperty);
            return target;
        }

        public static System.Windows.Shapes.Line WithX1(this System.Windows.Shapes.Line target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Line.X1Property);
            return target;
        }

        public static System.Windows.Shapes.Line WithY1(this System.Windows.Shapes.Line target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Line.Y1Property);
            return target;
        }

        public static System.Windows.Shapes.Line WithX2(this System.Windows.Shapes.Line target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Line.X2Property);
            return target;
        }

        public static System.Windows.Shapes.Line WithY2(this System.Windows.Shapes.Line target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Line.Y2Property);
            return target;
        }

        public static System.Windows.Shapes.Path WithData(this System.Windows.Shapes.Path target,
            ValueProxy<System.Windows.Media.Geometry>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Path.DataProperty);
            return target;
        }

        public static System.Windows.Shapes.Polygon WithPoints(this System.Windows.Shapes.Polygon target,
            ValueProxy<System.Windows.Media.PointCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Polygon.PointsProperty);
            return target;
        }

        public static System.Windows.Shapes.Polygon WithFillRule(this System.Windows.Shapes.Polygon target,
            ValueProxy<System.Windows.Media.FillRule>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Polygon.FillRuleProperty);
            return target;
        }

        public static System.Windows.Shapes.Polyline WithPoints(this System.Windows.Shapes.Polyline target,
            ValueProxy<System.Windows.Media.PointCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Polyline.PointsProperty);
            return target;
        }

        public static System.Windows.Shapes.Polyline WithFillRule(this System.Windows.Shapes.Polyline target,
            ValueProxy<System.Windows.Media.FillRule>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Polyline.FillRuleProperty);
            return target;
        }

        public static System.Windows.Shapes.Rectangle WithRadiusX(this System.Windows.Shapes.Rectangle target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Rectangle.RadiusXProperty);
            return target;
        }

        public static System.Windows.Shapes.Rectangle WithRadiusY(this System.Windows.Shapes.Rectangle target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Rectangle.RadiusYProperty);
            return target;
        }

        public static System.Windows.Shapes.Shape WithStretch(this System.Windows.Shapes.Shape target,
            ValueProxy<System.Windows.Media.Stretch>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Shape.StretchProperty);
            return target;
        }

        public static System.Windows.Shapes.Shape WithFill(this System.Windows.Shapes.Shape target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Shape.FillProperty);
            return target;
        }

        public static System.Windows.Shapes.Shape WithStroke(this System.Windows.Shapes.Shape target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Shape.StrokeProperty);
            return target;
        }

        public static System.Windows.Shapes.Shape WithStrokeThickness(this System.Windows.Shapes.Shape target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Shape.StrokeThicknessProperty);
            return target;
        }

        public static System.Windows.Shapes.Shape WithStrokeStartLineCap(this System.Windows.Shapes.Shape target,
            ValueProxy<System.Windows.Media.PenLineCap>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Shape.StrokeStartLineCapProperty);
            return target;
        }

        public static System.Windows.Shapes.Shape WithStrokeEndLineCap(this System.Windows.Shapes.Shape target,
            ValueProxy<System.Windows.Media.PenLineCap>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Shape.StrokeEndLineCapProperty);
            return target;
        }

        public static System.Windows.Shapes.Shape WithStrokeDashCap(this System.Windows.Shapes.Shape target,
            ValueProxy<System.Windows.Media.PenLineCap>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Shape.StrokeDashCapProperty);
            return target;
        }

        public static System.Windows.Shapes.Shape WithStrokeLineJoin(this System.Windows.Shapes.Shape target,
            ValueProxy<System.Windows.Media.PenLineJoin>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Shape.StrokeLineJoinProperty);
            return target;
        }

        public static System.Windows.Shapes.Shape WithStrokeMiterLimit(this System.Windows.Shapes.Shape target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Shape.StrokeMiterLimitProperty);
            return target;
        }

        public static System.Windows.Shapes.Shape WithStrokeDashOffset(this System.Windows.Shapes.Shape target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Shape.StrokeDashOffsetProperty);
            return target;
        }

        public static System.Windows.Shapes.Shape WithStrokeDashArray(this System.Windows.Shapes.Shape target,
            ValueProxy<System.Windows.Media.DoubleCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Shapes.Shape.StrokeDashArrayProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithKeyboardNavigation_TabIndex(
            this System.Windows.DependencyObject target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Input.KeyboardNavigation.TabIndexProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithKeyboardNavigation_IsTabStop(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Input.KeyboardNavigation.IsTabStopProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithKeyboardNavigation_TabNavigation(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Input.KeyboardNavigationMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Input.KeyboardNavigation.TabNavigationProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithKeyboardNavigation_ControlTabNavigation(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Input.KeyboardNavigationMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Input.KeyboardNavigation.ControlTabNavigationProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithKeyboardNavigation_DirectionalNavigation(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Input.KeyboardNavigationMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Input.KeyboardNavigation.DirectionalNavigationProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithKeyboardNavigation_AcceptsReturn(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Input.KeyboardNavigation.AcceptsReturnProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextOptions_TextFormattingMode(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Media.TextFormattingMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.TextOptions.TextFormattingModeProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextOptions_TextRenderingMode(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Media.TextRenderingMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.TextOptions.TextRenderingModeProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextOptions_TextHintingMode(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Media.TextHintingMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.TextOptions.TextHintingModeProperty);
            return target;
        }

        public static System.Windows.Media.Animation.BeginStoryboard WithStoryboard(
            this System.Windows.Media.Animation.BeginStoryboard target,
            ValueProxy<System.Windows.Media.Animation.Storyboard>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.Animation.BeginStoryboard.StoryboardProperty);
            return target;
        }

        public static System.Windows.Media.Animation.EasingThicknessKeyFrame WithEasingFunction(
            this System.Windows.Media.Animation.EasingThicknessKeyFrame target,
            ValueProxy<System.Windows.Media.Animation.IEasingFunction>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.Animation.EasingThicknessKeyFrame.EasingFunctionProperty);
            return target;
        }

        public static System.Windows.Media.Animation.ThicknessKeyFrame WithKeyTime(
            this System.Windows.Media.Animation.ThicknessKeyFrame target,
            ValueProxy<System.Windows.Media.Animation.KeyTime>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.Animation.ThicknessKeyFrame.KeyTimeProperty);
            return target;
        }

        public static System.Windows.Media.Animation.ThicknessKeyFrame WithValue(
            this System.Windows.Media.Animation.ThicknessKeyFrame target,
            ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.Animation.ThicknessKeyFrame.ValueProperty);
            return target;
        }

        public static System.Windows.Media.Animation.SplineThicknessKeyFrame WithKeySpline(
            this System.Windows.Media.Animation.SplineThicknessKeyFrame target,
            ValueProxy<System.Windows.Media.Animation.KeySpline>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.Animation.SplineThicknessKeyFrame.KeySplineProperty);
            return target;
        }

        public static System.Windows.Media.Animation.ThicknessAnimation WithFrom(
            this System.Windows.Media.Animation.ThicknessAnimation target,
            ValueProxy<System.Nullable<System.Windows.Thickness>>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.Animation.ThicknessAnimation.FromProperty);
            return target;
        }

        public static System.Windows.Media.Animation.ThicknessAnimation WithTo(
            this System.Windows.Media.Animation.ThicknessAnimation target,
            ValueProxy<System.Nullable<System.Windows.Thickness>>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.Animation.ThicknessAnimation.ToProperty);
            return target;
        }

        public static System.Windows.Media.Animation.ThicknessAnimation WithBy(
            this System.Windows.Media.Animation.ThicknessAnimation target,
            ValueProxy<System.Nullable<System.Windows.Thickness>>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.Animation.ThicknessAnimation.ByProperty);
            return target;
        }

        public static System.Windows.Media.Animation.ThicknessAnimation WithEasingFunction(
            this System.Windows.Media.Animation.ThicknessAnimation target,
            ValueProxy<System.Windows.Media.Animation.IEasingFunction>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.Animation.ThicknessAnimation.EasingFunctionProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithStoryboard_Target(this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.DependencyObject>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.Animation.Storyboard.TargetProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithStoryboard_TargetName(
            this System.Windows.DependencyObject target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.Animation.Storyboard.TargetNameProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithStoryboard_TargetProperty(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.PropertyPath>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Media.Animation.Storyboard.TargetPropertyProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithBinding_XmlNamespaceManager(
            this System.Windows.DependencyObject target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Data.Binding.XmlNamespaceManagerProperty);
            return target;
        }

        public static System.Windows.Data.CollectionContainer WithCollection(
            this System.Windows.Data.CollectionContainer target, ValueProxy<System.Collections.IEnumerable>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Data.CollectionContainer.CollectionProperty);
            return target;
        }

        public static System.Windows.Data.CollectionViewSource WithSource(
            this System.Windows.Data.CollectionViewSource target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Data.CollectionViewSource.SourceProperty);
            return target;
        }

        public static System.Windows.Data.CollectionViewSource WithCollectionViewType(
            this System.Windows.Data.CollectionViewSource target, ValueProxy<System.Type>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Data.CollectionViewSource.CollectionViewTypeProperty);
            return target;
        }

        public static System.Windows.Data.CollectionViewSource WithIsLiveSortingRequested(
            this System.Windows.Data.CollectionViewSource target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Data.CollectionViewSource.IsLiveSortingRequestedProperty);
            return target;
        }

        public static System.Windows.Data.CollectionViewSource WithIsLiveFilteringRequested(
            this System.Windows.Data.CollectionViewSource target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Data.CollectionViewSource.IsLiveFilteringRequestedProperty);
            return target;
        }

        public static System.Windows.Data.CollectionViewSource WithIsLiveGroupingRequested(
            this System.Windows.Data.CollectionViewSource target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Data.CollectionViewSource.IsLiveGroupingRequestedProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithXmlAttributeProperties_XmlSpace(
            this System.Windows.DependencyObject target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Markup.XmlAttributeProperties.XmlSpaceProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithXmlAttributeProperties_XmlnsDictionary(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Markup.XmlnsDictionary>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Markup.XmlAttributeProperties.XmlnsDictionaryProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithXmlAttributeProperties_XmlnsDefinition(
            this System.Windows.DependencyObject target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Markup.XmlAttributeProperties.XmlnsDefinitionProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithXmlAttributeProperties_XmlNamespaceMaps(
            this System.Windows.DependencyObject target, ValueProxy<System.Collections.Hashtable>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Markup.XmlAttributeProperties.XmlNamespaceMapsProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithJournalEntry_Name(this System.Windows.DependencyObject target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Navigation.JournalEntry.NameProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithJournalEntry_KeepAlive(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Navigation.JournalEntry.KeepAliveProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithJournalEntryUnifiedViewConverter_JournalEntryPosition(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Navigation.JournalEntryPosition>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Navigation.JournalEntryUnifiedViewConverter.JournalEntryPositionProperty);
            return target;
        }

        public static System.Windows.Navigation.NavigationWindow WithSandboxExternalContent(
            this System.Windows.Navigation.NavigationWindow target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Navigation.NavigationWindow.SandboxExternalContentProperty);
            return target;
        }

        public static System.Windows.Navigation.NavigationWindow WithShowsNavigationUI(
            this System.Windows.Navigation.NavigationWindow target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Navigation.NavigationWindow.ShowsNavigationUIProperty);
            return target;
        }

        public static System.Windows.Navigation.NavigationWindow WithSource(
            this System.Windows.Navigation.NavigationWindow target, ValueProxy<System.Uri>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Navigation.NavigationWindow.SourceProperty);
            return target;
        }

        public static System.Windows.Documents.AnchoredBlock WithMargin(
            this System.Windows.Documents.AnchoredBlock target, ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.AnchoredBlock.MarginProperty);
            return target;
        }

        public static System.Windows.Documents.AnchoredBlock WithPadding(
            this System.Windows.Documents.AnchoredBlock target, ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.AnchoredBlock.PaddingProperty);
            return target;
        }

        public static System.Windows.Documents.AnchoredBlock WithBorderThickness(
            this System.Windows.Documents.AnchoredBlock target, ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.AnchoredBlock.BorderThicknessProperty);
            return target;
        }

        public static System.Windows.Documents.AnchoredBlock WithBorderBrush(
            this System.Windows.Documents.AnchoredBlock target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.AnchoredBlock.BorderBrushProperty);
            return target;
        }

        public static System.Windows.Documents.AnchoredBlock WithTextAlignment(
            this System.Windows.Documents.AnchoredBlock target, ValueProxy<System.Windows.TextAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.AnchoredBlock.TextAlignmentProperty);
            return target;
        }

        public static System.Windows.Documents.AnchoredBlock WithLineHeight(
            this System.Windows.Documents.AnchoredBlock target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.AnchoredBlock.LineHeightProperty);
            return target;
        }

        public static System.Windows.Documents.AnchoredBlock WithLineStackingStrategy(
            this System.Windows.Documents.AnchoredBlock target,
            ValueProxy<System.Windows.LineStackingStrategy>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.AnchoredBlock.LineStackingStrategyProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithBlock_IsHyphenationEnabled(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Block.IsHyphenationEnabledProperty);
            return target;
        }

        public static System.Windows.Documents.Block WithMargin(this System.Windows.Documents.Block target,
            ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Block.MarginProperty);
            return target;
        }

        public static System.Windows.Documents.Block WithPadding(this System.Windows.Documents.Block target,
            ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Block.PaddingProperty);
            return target;
        }

        public static System.Windows.Documents.Block WithBorderThickness(this System.Windows.Documents.Block target,
            ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Block.BorderThicknessProperty);
            return target;
        }

        public static System.Windows.Documents.Block WithBorderBrush(this System.Windows.Documents.Block target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Block.BorderBrushProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithBlock_TextAlignment(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.TextAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Block.TextAlignmentProperty);
            return target;
        }

        public static System.Windows.Documents.Block WithFlowDirection(this System.Windows.Documents.Block target,
            ValueProxy<System.Windows.FlowDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Block.FlowDirectionProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithBlock_LineHeight(this System.Windows.DependencyObject target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Block.LineHeightProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithBlock_LineStackingStrategy(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.LineStackingStrategy>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Block.LineStackingStrategyProperty);
            return target;
        }

        public static System.Windows.Documents.Block WithBreakPageBefore(this System.Windows.Documents.Block target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Block.BreakPageBeforeProperty);
            return target;
        }

        public static System.Windows.Documents.Block WithBreakColumnBefore(this System.Windows.Documents.Block target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Block.BreakColumnBeforeProperty);
            return target;
        }

        public static System.Windows.Documents.Block WithClearFloaters(this System.Windows.Documents.Block target,
            ValueProxy<System.Windows.WrapDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Block.ClearFloatersProperty);
            return target;
        }

        public static System.Windows.Documents.DocumentReference WithSource(
            this System.Windows.Documents.DocumentReference target, ValueProxy<System.Uri>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.DocumentReference.SourceProperty);
            return target;
        }

        public static System.Windows.Documents.FixedDocumentSequence WithPrintTicket(
            this System.Windows.Documents.FixedDocumentSequence target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FixedDocumentSequence.PrintTicketProperty);
            return target;
        }

        public static System.Windows.Documents.Figure WithHorizontalAnchor(this System.Windows.Documents.Figure target,
            ValueProxy<System.Windows.FigureHorizontalAnchor>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Figure.HorizontalAnchorProperty);
            return target;
        }

        public static System.Windows.Documents.Figure WithVerticalAnchor(this System.Windows.Documents.Figure target,
            ValueProxy<System.Windows.FigureVerticalAnchor>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Figure.VerticalAnchorProperty);
            return target;
        }

        public static System.Windows.Documents.Figure WithHorizontalOffset(this System.Windows.Documents.Figure target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Figure.HorizontalOffsetProperty);
            return target;
        }

        public static System.Windows.Documents.Figure WithVerticalOffset(this System.Windows.Documents.Figure target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Figure.VerticalOffsetProperty);
            return target;
        }

        public static System.Windows.Documents.Figure WithCanDelayPlacement(this System.Windows.Documents.Figure target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Figure.CanDelayPlacementProperty);
            return target;
        }

        public static System.Windows.Documents.Figure WithWrapDirection(this System.Windows.Documents.Figure target,
            ValueProxy<System.Windows.WrapDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Figure.WrapDirectionProperty);
            return target;
        }

        public static System.Windows.Documents.Figure WithWidth(this System.Windows.Documents.Figure target,
            ValueProxy<System.Windows.FigureLength>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Figure.WidthProperty);
            return target;
        }

        public static System.Windows.Documents.Figure WithHeight(this System.Windows.Documents.Figure target,
            ValueProxy<System.Windows.FigureLength>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Figure.HeightProperty);
            return target;
        }

        public static System.Windows.Documents.FixedDocument WithPrintTicket(
            this System.Windows.Documents.FixedDocument target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FixedDocument.PrintTicketProperty);
            return target;
        }

        public static System.Windows.Documents.FixedPage WithPrintTicket(this System.Windows.Documents.FixedPage target,
            ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FixedPage.PrintTicketProperty);
            return target;
        }

        public static System.Windows.Documents.FixedPage WithBackground(this System.Windows.Documents.FixedPage target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FixedPage.BackgroundProperty);
            return target;
        }

        public static System.Windows.UIElement WithFixedPage_Left(this System.Windows.UIElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FixedPage.LeftProperty);
            return target;
        }

        public static System.Windows.UIElement WithFixedPage_Top(this System.Windows.UIElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FixedPage.TopProperty);
            return target;
        }

        public static System.Windows.UIElement WithFixedPage_Right(this System.Windows.UIElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FixedPage.RightProperty);
            return target;
        }

        public static System.Windows.UIElement WithFixedPage_Bottom(this System.Windows.UIElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FixedPage.BottomProperty);
            return target;
        }

        public static System.Windows.Documents.FixedPage WithContentBox(this System.Windows.Documents.FixedPage target,
            ValueProxy<System.Windows.Rect>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FixedPage.ContentBoxProperty);
            return target;
        }

        public static System.Windows.Documents.FixedPage WithBleedBox(this System.Windows.Documents.FixedPage target,
            ValueProxy<System.Windows.Rect>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FixedPage.BleedBoxProperty);
            return target;
        }

        public static System.Windows.UIElement WithFixedPage_NavigateUri(this System.Windows.UIElement target,
            ValueProxy<System.Uri>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FixedPage.NavigateUriProperty);
            return target;
        }

        public static System.Windows.Documents.Floater WithHorizontalAlignment(
            this System.Windows.Documents.Floater target, ValueProxy<System.Windows.HorizontalAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Floater.HorizontalAlignmentProperty);
            return target;
        }

        public static System.Windows.Documents.Floater WithWidth(this System.Windows.Documents.Floater target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Floater.WidthProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithFontFamily(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Windows.Media.FontFamily>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.FontFamilyProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithFontStyle(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Windows.FontStyle>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.FontStyleProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithFontWeight(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Windows.FontWeight>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.FontWeightProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithFontStretch(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Windows.FontStretch>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.FontStretchProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithFontSize(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.FontSizeProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithForeground(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.ForegroundProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithBackground(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.BackgroundProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithTextEffects(
            this System.Windows.Documents.FlowDocument target,
            ValueProxy<System.Windows.Media.TextEffectCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.TextEffectsProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithTextAlignment(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Windows.TextAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.TextAlignmentProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithFlowDirection(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Windows.FlowDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.FlowDirectionProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithLineHeight(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.LineHeightProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithLineStackingStrategy(
            this System.Windows.Documents.FlowDocument target,
            ValueProxy<System.Windows.LineStackingStrategy>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.LineStackingStrategyProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithColumnWidth(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.ColumnWidthProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithColumnGap(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.ColumnGapProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithIsColumnWidthFlexible(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.IsColumnWidthFlexibleProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithColumnRuleWidth(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.ColumnRuleWidthProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithColumnRuleBrush(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.ColumnRuleBrushProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithIsOptimalParagraphEnabled(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.IsOptimalParagraphEnabledProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithPageWidth(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.PageWidthProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithMinPageWidth(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.MinPageWidthProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithMaxPageWidth(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.MaxPageWidthProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithPageHeight(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.PageHeightProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithMinPageHeight(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.MinPageHeightProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithMaxPageHeight(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.MaxPageHeightProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithPagePadding(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.PagePaddingProperty);
            return target;
        }

        public static System.Windows.Documents.FlowDocument WithIsHyphenationEnabled(
            this System.Windows.Documents.FlowDocument target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.FlowDocument.IsHyphenationEnabledProperty);
            return target;
        }

        public static System.Windows.Documents.Glyphs WithFill(this System.Windows.Documents.Glyphs target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Glyphs.FillProperty);
            return target;
        }

        public static System.Windows.Documents.Glyphs WithIndices(this System.Windows.Documents.Glyphs target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Glyphs.IndicesProperty);
            return target;
        }

        public static System.Windows.Documents.Glyphs WithUnicodeString(this System.Windows.Documents.Glyphs target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Glyphs.UnicodeStringProperty);
            return target;
        }

        public static System.Windows.Documents.Glyphs WithCaretStops(this System.Windows.Documents.Glyphs target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Glyphs.CaretStopsProperty);
            return target;
        }

        public static System.Windows.Documents.Glyphs WithFontRenderingEmSize(
            this System.Windows.Documents.Glyphs target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Glyphs.FontRenderingEmSizeProperty);
            return target;
        }

        public static System.Windows.Documents.Glyphs WithOriginX(this System.Windows.Documents.Glyphs target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Glyphs.OriginXProperty);
            return target;
        }

        public static System.Windows.Documents.Glyphs WithOriginY(this System.Windows.Documents.Glyphs target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Glyphs.OriginYProperty);
            return target;
        }

        public static System.Windows.Documents.Glyphs WithFontUri(this System.Windows.Documents.Glyphs target,
            ValueProxy<System.Uri>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Glyphs.FontUriProperty);
            return target;
        }

        public static System.Windows.Documents.Glyphs WithStyleSimulations(this System.Windows.Documents.Glyphs target,
            ValueProxy<System.Windows.Media.StyleSimulations>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Glyphs.StyleSimulationsProperty);
            return target;
        }

        public static System.Windows.Documents.Glyphs WithIsSideways(this System.Windows.Documents.Glyphs target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Glyphs.IsSidewaysProperty);
            return target;
        }

        public static System.Windows.Documents.Glyphs WithBidiLevel(this System.Windows.Documents.Glyphs target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Glyphs.BidiLevelProperty);
            return target;
        }

        public static System.Windows.Documents.Glyphs WithDeviceFontName(this System.Windows.Documents.Glyphs target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Glyphs.DeviceFontNameProperty);
            return target;
        }

        public static System.Windows.Documents.Hyperlink WithCommand(this System.Windows.Documents.Hyperlink target,
            ValueProxy<System.Windows.Input.ICommand>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Hyperlink.CommandProperty);
            return target;
        }

        public static System.Windows.Documents.Hyperlink WithCommandParameter(
            this System.Windows.Documents.Hyperlink target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Hyperlink.CommandParameterProperty);
            return target;
        }

        public static System.Windows.Documents.Hyperlink WithCommandTarget(
            this System.Windows.Documents.Hyperlink target, ValueProxy<System.Windows.IInputElement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Hyperlink.CommandTargetProperty);
            return target;
        }

        public static System.Windows.Documents.Hyperlink WithNavigateUri(this System.Windows.Documents.Hyperlink target,
            ValueProxy<System.Uri>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Hyperlink.NavigateUriProperty);
            return target;
        }

        public static System.Windows.Documents.Hyperlink WithTargetName(this System.Windows.Documents.Hyperlink target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Hyperlink.TargetNameProperty);
            return target;
        }

        public static System.Windows.Documents.Inline WithBaselineAlignment(this System.Windows.Documents.Inline target,
            ValueProxy<System.Windows.BaselineAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Inline.BaselineAlignmentProperty);
            return target;
        }

        public static System.Windows.Documents.Inline WithTextDecorations(this System.Windows.Documents.Inline target,
            ValueProxy<System.Windows.TextDecorationCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Inline.TextDecorationsProperty);
            return target;
        }

        public static System.Windows.Documents.Inline WithFlowDirection(this System.Windows.Documents.Inline target,
            ValueProxy<System.Windows.FlowDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Inline.FlowDirectionProperty);
            return target;
        }

        public static System.Windows.Documents.List WithMarkerStyle(this System.Windows.Documents.List target,
            ValueProxy<System.Windows.TextMarkerStyle>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.List.MarkerStyleProperty);
            return target;
        }

        public static System.Windows.Documents.List WithMarkerOffset(this System.Windows.Documents.List target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.List.MarkerOffsetProperty);
            return target;
        }

        public static System.Windows.Documents.List WithStartIndex(this System.Windows.Documents.List target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.List.StartIndexProperty);
            return target;
        }

        public static System.Windows.Documents.ListItem WithMargin(this System.Windows.Documents.ListItem target,
            ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.ListItem.MarginProperty);
            return target;
        }

        public static System.Windows.Documents.ListItem WithPadding(this System.Windows.Documents.ListItem target,
            ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.ListItem.PaddingProperty);
            return target;
        }

        public static System.Windows.Documents.ListItem WithBorderThickness(
            this System.Windows.Documents.ListItem target, ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.ListItem.BorderThicknessProperty);
            return target;
        }

        public static System.Windows.Documents.ListItem WithBorderBrush(this System.Windows.Documents.ListItem target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.ListItem.BorderBrushProperty);
            return target;
        }

        public static System.Windows.Documents.ListItem WithTextAlignment(this System.Windows.Documents.ListItem target,
            ValueProxy<System.Windows.TextAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.ListItem.TextAlignmentProperty);
            return target;
        }

        public static System.Windows.Documents.ListItem WithFlowDirection(this System.Windows.Documents.ListItem target,
            ValueProxy<System.Windows.FlowDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.ListItem.FlowDirectionProperty);
            return target;
        }

        public static System.Windows.Documents.ListItem WithLineHeight(this System.Windows.Documents.ListItem target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.ListItem.LineHeightProperty);
            return target;
        }

        public static System.Windows.Documents.ListItem WithLineStackingStrategy(
            this System.Windows.Documents.ListItem target, ValueProxy<System.Windows.LineStackingStrategy>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.ListItem.LineStackingStrategyProperty);
            return target;
        }

        public static System.Windows.Documents.PageContent WithSource(this System.Windows.Documents.PageContent target,
            ValueProxy<System.Uri>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.PageContent.SourceProperty);
            return target;
        }

        public static System.Windows.Documents.Paragraph WithTextDecorations(
            this System.Windows.Documents.Paragraph target,
            ValueProxy<System.Windows.TextDecorationCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Paragraph.TextDecorationsProperty);
            return target;
        }

        public static System.Windows.Documents.Paragraph WithTextIndent(this System.Windows.Documents.Paragraph target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Paragraph.TextIndentProperty);
            return target;
        }

        public static System.Windows.Documents.Paragraph WithMinOrphanLines(
            this System.Windows.Documents.Paragraph target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Paragraph.MinOrphanLinesProperty);
            return target;
        }

        public static System.Windows.Documents.Paragraph WithMinWidowLines(
            this System.Windows.Documents.Paragraph target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Paragraph.MinWidowLinesProperty);
            return target;
        }

        public static System.Windows.Documents.Paragraph WithKeepWithNext(
            this System.Windows.Documents.Paragraph target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Paragraph.KeepWithNextProperty);
            return target;
        }

        public static System.Windows.Documents.Paragraph WithKeepTogether(
            this System.Windows.Documents.Paragraph target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Paragraph.KeepTogetherProperty);
            return target;
        }

        public static System.Windows.Documents.Run WithText(this System.Windows.Documents.Run target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Run.TextProperty);
            return target;
        }

        public static System.Windows.Documents.Table WithCellSpacing(this System.Windows.Documents.Table target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Table.CellSpacingProperty);
            return target;
        }

        public static System.Windows.Documents.TableCell WithPadding(this System.Windows.Documents.TableCell target,
            ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TableCell.PaddingProperty);
            return target;
        }

        public static System.Windows.Documents.TableCell WithBorderThickness(
            this System.Windows.Documents.TableCell target, ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TableCell.BorderThicknessProperty);
            return target;
        }

        public static System.Windows.Documents.TableCell WithBorderBrush(this System.Windows.Documents.TableCell target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TableCell.BorderBrushProperty);
            return target;
        }

        public static System.Windows.Documents.TableCell WithTextAlignment(
            this System.Windows.Documents.TableCell target, ValueProxy<System.Windows.TextAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TableCell.TextAlignmentProperty);
            return target;
        }

        public static System.Windows.Documents.TableCell WithFlowDirection(
            this System.Windows.Documents.TableCell target, ValueProxy<System.Windows.FlowDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TableCell.FlowDirectionProperty);
            return target;
        }

        public static System.Windows.Documents.TableCell WithLineHeight(this System.Windows.Documents.TableCell target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TableCell.LineHeightProperty);
            return target;
        }

        public static System.Windows.Documents.TableCell WithLineStackingStrategy(
            this System.Windows.Documents.TableCell target, ValueProxy<System.Windows.LineStackingStrategy>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TableCell.LineStackingStrategyProperty);
            return target;
        }

        public static System.Windows.Documents.TableCell WithColumnSpan(this System.Windows.Documents.TableCell target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TableCell.ColumnSpanProperty);
            return target;
        }

        public static System.Windows.Documents.TableCell WithRowSpan(this System.Windows.Documents.TableCell target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TableCell.RowSpanProperty);
            return target;
        }

        public static System.Windows.Documents.TableColumn WithWidth(this System.Windows.Documents.TableColumn target,
            ValueProxy<System.Windows.GridLength>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TableColumn.WidthProperty);
            return target;
        }

        public static System.Windows.Documents.TableColumn WithBackground(
            this System.Windows.Documents.TableColumn target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TableColumn.BackgroundProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextElement_FontFamily(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Media.FontFamily>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TextElement.FontFamilyProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextElement_FontStyle(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FontStyle>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TextElement.FontStyleProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextElement_FontWeight(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FontWeight>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TextElement.FontWeightProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextElement_FontStretch(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FontStretch>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TextElement.FontStretchProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextElement_FontSize(
            this System.Windows.DependencyObject target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TextElement.FontSizeProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextElement_Foreground(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TextElement.ForegroundProperty);
            return target;
        }

        public static System.Windows.Documents.TextElement WithBackground(
            this System.Windows.Documents.TextElement target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TextElement.BackgroundProperty);
            return target;
        }

        public static System.Windows.Documents.TextElement WithTextEffects(
            this System.Windows.Documents.TextElement target,
            ValueProxy<System.Windows.Media.TextEffectCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.TextElement.TextEffectsProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StandardLigatures(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StandardLigaturesProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_ContextualLigatures(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.ContextualLigaturesProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_DiscretionaryLigatures(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.DiscretionaryLigaturesProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_HistoricalLigatures(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.HistoricalLigaturesProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_AnnotationAlternates(
            this System.Windows.DependencyObject target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.AnnotationAlternatesProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_ContextualAlternates(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.ContextualAlternatesProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_HistoricalForms(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.HistoricalFormsProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_Kerning(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.KerningProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_CapitalSpacing(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.CapitalSpacingProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_CaseSensitiveForms(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.CaseSensitiveFormsProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet1(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet1Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet2(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet2Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet3(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet3Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet4(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet4Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet5(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet5Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet6(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet6Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet7(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet7Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet8(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet8Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet9(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet9Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet10(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet10Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet11(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet11Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet12(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet12Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet13(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet13Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet14(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet14Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet15(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet15Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet16(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet16Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet17(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet17Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet18(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet18Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet19(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet19Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticSet20(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticSet20Property);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_Fraction(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FontFraction>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.FractionProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_SlashedZero(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.SlashedZeroProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_MathematicalGreek(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.MathematicalGreekProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_EastAsianExpertForms(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.EastAsianExpertFormsProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_Variants(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FontVariants>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.VariantsProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_Capitals(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FontCapitals>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.CapitalsProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_NumeralStyle(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FontNumeralStyle>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.NumeralStyleProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_NumeralAlignment(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FontNumeralAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.NumeralAlignmentProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_EastAsianWidths(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FontEastAsianWidths>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.EastAsianWidthsProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_EastAsianLanguage(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FontEastAsianLanguage>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.EastAsianLanguageProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StandardSwashes(
            this System.Windows.DependencyObject target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StandardSwashesProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_ContextualSwashes(
            this System.Windows.DependencyObject target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.ContextualSwashesProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTypography_StylisticAlternates(
            this System.Windows.DependencyObject target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Documents.Typography.StylisticAlternatesProperty);
            return target;
        }

        public static System.Windows.Controls.StickyNoteControl WithIsExpanded(
            this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.StickyNoteControl.IsExpandedProperty);
            return target;
        }

        public static System.Windows.Controls.StickyNoteControl WithIsActive(
            this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.StickyNoteControl.IsActiveProperty);
            return target;
        }

        public static System.Windows.Controls.StickyNoteControl WithCaptionFontFamily(
            this System.Windows.Controls.StickyNoteControl target,
            ValueProxy<System.Windows.Media.FontFamily>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.StickyNoteControl.CaptionFontFamilyProperty);
            return target;
        }

        public static System.Windows.Controls.StickyNoteControl WithCaptionFontSize(
            this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.StickyNoteControl.CaptionFontSizeProperty);
            return target;
        }

        public static System.Windows.Controls.StickyNoteControl WithCaptionFontStretch(
            this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Windows.FontStretch>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.StickyNoteControl.CaptionFontStretchProperty);
            return target;
        }

        public static System.Windows.Controls.StickyNoteControl WithCaptionFontStyle(
            this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Windows.FontStyle>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.StickyNoteControl.CaptionFontStyleProperty);
            return target;
        }

        public static System.Windows.Controls.StickyNoteControl WithCaptionFontWeight(
            this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Windows.FontWeight>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.StickyNoteControl.CaptionFontWeightProperty);
            return target;
        }

        public static System.Windows.Controls.StickyNoteControl WithPenWidth(
            this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.StickyNoteControl.PenWidthProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithText(this System.Windows.Controls.AccessText target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.TextProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithFontFamily(this System.Windows.Controls.AccessText target,
            ValueProxy<System.Windows.Media.FontFamily>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.FontFamilyProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithFontStyle(this System.Windows.Controls.AccessText target,
            ValueProxy<System.Windows.FontStyle>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.FontStyleProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithFontWeight(this System.Windows.Controls.AccessText target,
            ValueProxy<System.Windows.FontWeight>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.FontWeightProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithFontStretch(this System.Windows.Controls.AccessText target,
            ValueProxy<System.Windows.FontStretch>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.FontStretchProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithFontSize(this System.Windows.Controls.AccessText target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.FontSizeProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithForeground(this System.Windows.Controls.AccessText target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.ForegroundProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithBackground(this System.Windows.Controls.AccessText target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.BackgroundProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithTextDecorations(
            this System.Windows.Controls.AccessText target,
            ValueProxy<System.Windows.TextDecorationCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.TextDecorationsProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithTextEffects(this System.Windows.Controls.AccessText target,
            ValueProxy<System.Windows.Media.TextEffectCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.TextEffectsProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithLineHeight(this System.Windows.Controls.AccessText target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.LineHeightProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithLineStackingStrategy(
            this System.Windows.Controls.AccessText target, ValueProxy<System.Windows.LineStackingStrategy>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.LineStackingStrategyProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithTextAlignment(
            this System.Windows.Controls.AccessText target, ValueProxy<System.Windows.TextAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.TextAlignmentProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithTextTrimming(
            this System.Windows.Controls.AccessText target, ValueProxy<System.Windows.TextTrimming>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.TextTrimmingProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithTextWrapping(
            this System.Windows.Controls.AccessText target, ValueProxy<System.Windows.TextWrapping>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.TextWrappingProperty);
            return target;
        }

        public static System.Windows.Controls.AccessText WithBaselineOffset(
            this System.Windows.Controls.AccessText target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.AccessText.BaselineOffsetProperty);
            return target;
        }

        public static System.Windows.Controls.Border WithBorderThickness(this System.Windows.Controls.Border target,
            ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Border.BorderThicknessProperty);
            return target;
        }

        public static System.Windows.Controls.Border WithPadding(this System.Windows.Controls.Border target,
            ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Border.PaddingProperty);
            return target;
        }

        public static System.Windows.Controls.Border WithCornerRadius(this System.Windows.Controls.Border target,
            ValueProxy<System.Windows.CornerRadius>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Border.CornerRadiusProperty);
            return target;
        }

        public static System.Windows.Controls.Border WithBorderBrush(this System.Windows.Controls.Border target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Border.BorderBrushProperty);
            return target;
        }

        public static System.Windows.Controls.Border WithBackground(this System.Windows.Controls.Border target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Border.BackgroundProperty);
            return target;
        }

        public static System.Windows.Controls.Button WithIsDefault(this System.Windows.Controls.Button target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Button.IsDefaultProperty);
            return target;
        }

        public static System.Windows.Controls.Button WithIsCancel(this System.Windows.Controls.Button target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Button.IsCancelProperty);
            return target;
        }

        public static System.Windows.Controls.Calendar WithCalendarButtonStyle(
            this System.Windows.Controls.Calendar target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Calendar.CalendarButtonStyleProperty);
            return target;
        }

        public static System.Windows.Controls.Calendar WithCalendarDayButtonStyle(
            this System.Windows.Controls.Calendar target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Calendar.CalendarDayButtonStyleProperty);
            return target;
        }

        public static System.Windows.Controls.Calendar WithCalendarItemStyle(
            this System.Windows.Controls.Calendar target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Calendar.CalendarItemStyleProperty);
            return target;
        }

        public static System.Windows.Controls.Calendar WithDisplayDate(this System.Windows.Controls.Calendar target,
            ValueProxy<System.DateTime>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Calendar.DisplayDateProperty);
            return target;
        }

        public static System.Windows.Controls.Calendar WithDisplayDateEnd(this System.Windows.Controls.Calendar target,
            ValueProxy<System.Nullable<System.DateTime>>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Calendar.DisplayDateEndProperty);
            return target;
        }

        public static System.Windows.Controls.Calendar WithDisplayDateStart(
            this System.Windows.Controls.Calendar target, ValueProxy<System.Nullable<System.DateTime>>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Calendar.DisplayDateStartProperty);
            return target;
        }

        public static System.Windows.Controls.Calendar WithDisplayMode(this System.Windows.Controls.Calendar target,
            ValueProxy<System.Windows.Controls.CalendarMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Calendar.DisplayModeProperty);
            return target;
        }

        public static System.Windows.Controls.Calendar WithFirstDayOfWeek(this System.Windows.Controls.Calendar target,
            ValueProxy<System.DayOfWeek>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Calendar.FirstDayOfWeekProperty);
            return target;
        }

        public static System.Windows.Controls.Calendar WithIsTodayHighlighted(
            this System.Windows.Controls.Calendar target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Calendar.IsTodayHighlightedProperty);
            return target;
        }

        public static System.Windows.Controls.Calendar WithSelectedDate(this System.Windows.Controls.Calendar target,
            ValueProxy<System.Nullable<System.DateTime>>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Calendar.SelectedDateProperty);
            return target;
        }

        public static System.Windows.Controls.Calendar WithSelectionMode(this System.Windows.Controls.Calendar target,
            ValueProxy<System.Windows.Controls.CalendarSelectionMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Calendar.SelectionModeProperty);
            return target;
        }

        public static System.Windows.UIElement WithCanvas_Left(this System.Windows.UIElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Canvas.LeftProperty);
            return target;
        }

        public static System.Windows.UIElement WithCanvas_Top(this System.Windows.UIElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Canvas.TopProperty);
            return target;
        }

        public static System.Windows.UIElement WithCanvas_Right(this System.Windows.UIElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Canvas.RightProperty);
            return target;
        }

        public static System.Windows.UIElement WithCanvas_Bottom(this System.Windows.UIElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Canvas.BottomProperty);
            return target;
        }

        public static System.Windows.Controls.ComboBox WithMaxDropDownHeight(
            this System.Windows.Controls.ComboBox target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ComboBox.MaxDropDownHeightProperty);
            return target;
        }

        public static System.Windows.Controls.ComboBox WithIsDropDownOpen(this System.Windows.Controls.ComboBox target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ComboBox.IsDropDownOpenProperty);
            return target;
        }

        public static System.Windows.Controls.ComboBox WithShouldPreserveUserEnteredPrefix(
            this System.Windows.Controls.ComboBox target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ComboBox.ShouldPreserveUserEnteredPrefixProperty);
            return target;
        }

        public static System.Windows.Controls.ComboBox WithIsEditable(this System.Windows.Controls.ComboBox target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ComboBox.IsEditableProperty);
            return target;
        }

        public static System.Windows.Controls.ComboBox WithText(this System.Windows.Controls.ComboBox target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ComboBox.TextProperty);
            return target;
        }

        public static System.Windows.Controls.ComboBox WithIsReadOnly(this System.Windows.Controls.ComboBox target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ComboBox.IsReadOnlyProperty);
            return target;
        }

        public static System.Windows.Controls.ComboBox WithStaysOpenOnEdit(this System.Windows.Controls.ComboBox target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ComboBox.StaysOpenOnEditProperty);
            return target;
        }

        public static System.Windows.Controls.ContentControl WithContent(
            this System.Windows.Controls.ContentControl target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContentControl.ContentProperty);
            return target;
        }

        public static System.Windows.Controls.ContentControl WithContentTemplate(
            this System.Windows.Controls.ContentControl target, ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContentControl.ContentTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.ContentControl WithContentTemplateSelector(
            this System.Windows.Controls.ContentControl target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContentControl.ContentTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.ContentControl WithContentStringFormat(
            this System.Windows.Controls.ContentControl target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContentControl.ContentStringFormatProperty);
            return target;
        }

        public static System.Windows.Controls.ContentPresenter WithRecognizesAccessKey(
            this System.Windows.Controls.ContentPresenter target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContentPresenter.RecognizesAccessKeyProperty);
            return target;
        }

        public static System.Windows.Controls.ContentPresenter WithContent(
            this System.Windows.Controls.ContentPresenter target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContentPresenter.ContentProperty);
            return target;
        }

        public static System.Windows.Controls.ContentPresenter WithContentTemplate(
            this System.Windows.Controls.ContentPresenter target, ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContentPresenter.ContentTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.ContentPresenter WithContentTemplateSelector(
            this System.Windows.Controls.ContentPresenter target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContentPresenter.ContentTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.ContentPresenter WithContentStringFormat(
            this System.Windows.Controls.ContentPresenter target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContentPresenter.ContentStringFormatProperty);
            return target;
        }

        public static System.Windows.Controls.ContentPresenter WithContentSource(
            this System.Windows.Controls.ContentPresenter target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContentPresenter.ContentSourceProperty);
            return target;
        }

        public static System.Windows.Controls.ContextMenu WithHorizontalOffset(
            this System.Windows.Controls.ContextMenu target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenu.HorizontalOffsetProperty);
            return target;
        }

        public static System.Windows.Controls.ContextMenu WithVerticalOffset(
            this System.Windows.Controls.ContextMenu target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenu.VerticalOffsetProperty);
            return target;
        }

        public static System.Windows.Controls.ContextMenu WithIsOpen(this System.Windows.Controls.ContextMenu target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenu.IsOpenProperty);
            return target;
        }

        public static System.Windows.Controls.ContextMenu WithPlacementTarget(
            this System.Windows.Controls.ContextMenu target, ValueProxy<System.Windows.UIElement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenu.PlacementTargetProperty);
            return target;
        }

        public static System.Windows.Controls.ContextMenu WithPlacementRectangle(
            this System.Windows.Controls.ContextMenu target, ValueProxy<System.Windows.Rect>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenu.PlacementRectangleProperty);
            return target;
        }

        public static System.Windows.Controls.ContextMenu WithPlacement(this System.Windows.Controls.ContextMenu target,
            ValueProxy<System.Windows.Controls.Primitives.PlacementMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenu.PlacementProperty);
            return target;
        }

        public static System.Windows.Controls.ContextMenu WithHasDropShadow(
            this System.Windows.Controls.ContextMenu target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenu.HasDropShadowProperty);
            return target;
        }

        public static System.Windows.Controls.ContextMenu WithCustomPopupPlacementCallback(
            this System.Windows.Controls.ContextMenu target,
            ValueProxy<System.Windows.Controls.Primitives.CustomPopupPlacementCallback>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenu.CustomPopupPlacementCallbackProperty);
            return target;
        }

        public static System.Windows.Controls.ContextMenu WithStaysOpen(this System.Windows.Controls.ContextMenu target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenu.StaysOpenProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithContextMenuService_ContextMenu(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Controls.ContextMenu>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenuService.ContextMenuProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithContextMenuService_HorizontalOffset(
            this System.Windows.DependencyObject target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenuService.HorizontalOffsetProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithContextMenuService_VerticalOffset(
            this System.Windows.DependencyObject target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenuService.VerticalOffsetProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithContextMenuService_HasDropShadow(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenuService.HasDropShadowProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithContextMenuService_PlacementTarget(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.UIElement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenuService.PlacementTargetProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithContextMenuService_PlacementRectangle(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Rect>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenuService.PlacementRectangleProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithContextMenuService_Placement(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Controls.Primitives.PlacementMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenuService.PlacementProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithContextMenuService_ShowOnDisabled(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenuService.ShowOnDisabledProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithContextMenuService_IsEnabled(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ContextMenuService.IsEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithBorderBrush(this System.Windows.Controls.Control target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.BorderBrushProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithBorderThickness(this System.Windows.Controls.Control target,
            ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.BorderThicknessProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithBackground(this System.Windows.Controls.Control target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.BackgroundProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithForeground(this System.Windows.Controls.Control target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.ForegroundProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithFontFamily(this System.Windows.Controls.Control target,
            ValueProxy<System.Windows.Media.FontFamily>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.FontFamilyProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithFontSize(this System.Windows.Controls.Control target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.FontSizeProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithFontStretch(this System.Windows.Controls.Control target,
            ValueProxy<System.Windows.FontStretch>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.FontStretchProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithFontStyle(this System.Windows.Controls.Control target,
            ValueProxy<System.Windows.FontStyle>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.FontStyleProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithFontWeight(this System.Windows.Controls.Control target,
            ValueProxy<System.Windows.FontWeight>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.FontWeightProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithHorizontalContentAlignment(
            this System.Windows.Controls.Control target, ValueProxy<System.Windows.HorizontalAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.HorizontalContentAlignmentProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithVerticalContentAlignment(
            this System.Windows.Controls.Control target, ValueProxy<System.Windows.VerticalAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.VerticalContentAlignmentProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithTabIndex(this System.Windows.Controls.Control target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.TabIndexProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithIsTabStop(this System.Windows.Controls.Control target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.IsTabStopProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithPadding(this System.Windows.Controls.Control target,
            ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.PaddingProperty);
            return target;
        }

        public static System.Windows.Controls.Control WithTemplate(this System.Windows.Controls.Control target,
            ValueProxy<System.Windows.Controls.ControlTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Control.TemplateProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithCanUserResizeColumns(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.CanUserResizeColumnsProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithColumnWidth(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.DataGridLength>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.ColumnWidthProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithMinColumnWidth(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.MinColumnWidthProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithMaxColumnWidth(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.MaxColumnWidthProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithGridLinesVisibility(
            this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.DataGridGridLinesVisibility>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.GridLinesVisibilityProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithHorizontalGridLinesBrush(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.HorizontalGridLinesBrushProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithVerticalGridLinesBrush(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.VerticalGridLinesBrushProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithRowStyle(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.RowStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithRowValidationErrorTemplate(
            this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.ControlTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.RowValidationErrorTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithRowStyleSelector(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Windows.Controls.StyleSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.RowStyleSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithRowBackground(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.RowBackgroundProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithAlternatingRowBackground(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.AlternatingRowBackgroundProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithRowHeight(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.RowHeightProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithMinRowHeight(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.MinRowHeightProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithRowHeaderWidth(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.RowHeaderWidthProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithColumnHeaderHeight(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.ColumnHeaderHeightProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithHeadersVisibility(
            this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.DataGridHeadersVisibility>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.HeadersVisibilityProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithCellStyle(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.CellStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithColumnHeaderStyle(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.ColumnHeaderStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithRowHeaderStyle(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.RowHeaderStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithRowHeaderTemplate(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.RowHeaderTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithRowHeaderTemplateSelector(
            this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.RowHeaderTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithHorizontalScrollBarVisibility(
            this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.ScrollBarVisibility>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.HorizontalScrollBarVisibilityProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithVerticalScrollBarVisibility(
            this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.ScrollBarVisibility>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.VerticalScrollBarVisibilityProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithIsReadOnly(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.IsReadOnlyProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithCurrentItem(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.CurrentItemProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithCurrentColumn(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.DataGridColumn>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.CurrentColumnProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithCurrentCell(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.DataGridCellInfo>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.CurrentCellProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithCanUserAddRows(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.CanUserAddRowsProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithCanUserDeleteRows(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.CanUserDeleteRowsProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithRowDetailsVisibilityMode(
            this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.DataGridRowDetailsVisibilityMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.RowDetailsVisibilityModeProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithAreRowDetailsFrozen(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.AreRowDetailsFrozenProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithRowDetailsTemplate(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.RowDetailsTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithRowDetailsTemplateSelector(
            this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.RowDetailsTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithCanUserResizeRows(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.CanUserResizeRowsProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithSelectionMode(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.DataGridSelectionMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.SelectionModeProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithSelectionUnit(this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.DataGridSelectionUnit>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.SelectionUnitProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithCanUserSortColumns(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.CanUserSortColumnsProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithAutoGenerateColumns(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.AutoGenerateColumnsProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithFrozenColumnCount(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.FrozenColumnCountProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithEnableRowVirtualization(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.EnableRowVirtualizationProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithEnableColumnVirtualization(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.EnableColumnVirtualizationProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithCanUserReorderColumns(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.CanUserReorderColumnsProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithDragIndicatorStyle(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.DragIndicatorStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithDropLocationIndicatorStyle(
            this System.Windows.Controls.DataGrid target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.DropLocationIndicatorStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGrid WithClipboardCopyMode(
            this System.Windows.Controls.DataGrid target,
            ValueProxy<System.Windows.Controls.DataGridClipboardCopyMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGrid.ClipboardCopyModeProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridBoundColumn WithElementStyle(
            this System.Windows.Controls.DataGridBoundColumn target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridBoundColumn.ElementStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridBoundColumn WithEditingElementStyle(
            this System.Windows.Controls.DataGridBoundColumn target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridBoundColumn.EditingElementStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridCell WithIsEditing(
            this System.Windows.Controls.DataGridCell target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridCell.IsEditingProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridCell WithIsSelected(
            this System.Windows.Controls.DataGridCell target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridCell.IsSelectedProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridCheckBoxColumn WithIsThreeState(
            this System.Windows.Controls.DataGridCheckBoxColumn target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridCheckBoxColumn.IsThreeStateProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithHeader(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.HeaderProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithHeaderStyle(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.HeaderStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithHeaderStringFormat(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.HeaderStringFormatProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithHeaderTemplate(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.HeaderTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithHeaderTemplateSelector(
            this System.Windows.Controls.DataGridColumn target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.HeaderTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithCellStyle(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.CellStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithIsReadOnly(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.IsReadOnlyProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithWidth(
            this System.Windows.Controls.DataGridColumn target,
            ValueProxy<System.Windows.Controls.DataGridLength>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.WidthProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithMinWidth(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.MinWidthProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithMaxWidth(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.MaxWidthProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithDisplayIndex(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.DisplayIndexProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithSortMemberPath(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.SortMemberPathProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithCanUserSort(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.CanUserSortProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithSortDirection(
            this System.Windows.Controls.DataGridColumn target,
            ValueProxy<System.Nullable<System.ComponentModel.ListSortDirection>>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.SortDirectionProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithCanUserReorder(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.CanUserReorderProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithDragIndicatorStyle(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.DragIndicatorStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithCanUserResize(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.CanUserResizeProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridColumn WithVisibility(
            this System.Windows.Controls.DataGridColumn target, ValueProxy<System.Windows.Visibility>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridColumn.VisibilityProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridComboBoxColumn WithElementStyle(
            this System.Windows.Controls.DataGridComboBoxColumn target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridComboBoxColumn.ElementStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridComboBoxColumn WithEditingElementStyle(
            this System.Windows.Controls.DataGridComboBoxColumn target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridComboBoxColumn.EditingElementStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridComboBoxColumn WithItemsSource(
            this System.Windows.Controls.DataGridComboBoxColumn target,
            ValueProxy<System.Collections.IEnumerable>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridComboBoxColumn.ItemsSourceProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridComboBoxColumn WithDisplayMemberPath(
            this System.Windows.Controls.DataGridComboBoxColumn target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridComboBoxColumn.DisplayMemberPathProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridComboBoxColumn WithSelectedValuePath(
            this System.Windows.Controls.DataGridComboBoxColumn target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridComboBoxColumn.SelectedValuePathProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridHyperlinkColumn WithTargetName(
            this System.Windows.Controls.DataGridHyperlinkColumn target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridHyperlinkColumn.TargetNameProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridRow WithItem(this System.Windows.Controls.DataGridRow target,
            ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridRow.ItemProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridRow WithItemsPanel(
            this System.Windows.Controls.DataGridRow target,
            ValueProxy<System.Windows.Controls.ItemsPanelTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridRow.ItemsPanelProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridRow WithHeader(this System.Windows.Controls.DataGridRow target,
            ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridRow.HeaderProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridRow WithHeaderStyle(
            this System.Windows.Controls.DataGridRow target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridRow.HeaderStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridRow WithHeaderTemplate(
            this System.Windows.Controls.DataGridRow target, ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridRow.HeaderTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridRow WithHeaderTemplateSelector(
            this System.Windows.Controls.DataGridRow target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridRow.HeaderTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridRow WithValidationErrorTemplate(
            this System.Windows.Controls.DataGridRow target,
            ValueProxy<System.Windows.Controls.ControlTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridRow.ValidationErrorTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridRow WithDetailsTemplate(
            this System.Windows.Controls.DataGridRow target, ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridRow.DetailsTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridRow WithDetailsTemplateSelector(
            this System.Windows.Controls.DataGridRow target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridRow.DetailsTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridRow WithDetailsVisibility(
            this System.Windows.Controls.DataGridRow target, ValueProxy<System.Windows.Visibility>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridRow.DetailsVisibilityProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridRow WithIsSelected(
            this System.Windows.Controls.DataGridRow target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridRow.IsSelectedProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridTemplateColumn WithCellTemplate(
            this System.Windows.Controls.DataGridTemplateColumn target,
            ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridTemplateColumn.CellTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridTemplateColumn WithCellTemplateSelector(
            this System.Windows.Controls.DataGridTemplateColumn target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridTemplateColumn.CellTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridTemplateColumn WithCellEditingTemplate(
            this System.Windows.Controls.DataGridTemplateColumn target,
            ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridTemplateColumn WithCellEditingTemplateSelector(
            this System.Windows.Controls.DataGridTemplateColumn target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridTextColumn WithFontFamily(
            this System.Windows.Controls.DataGridTextColumn target,
            ValueProxy<System.Windows.Media.FontFamily>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridTextColumn.FontFamilyProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridTextColumn WithFontSize(
            this System.Windows.Controls.DataGridTextColumn target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridTextColumn.FontSizeProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridTextColumn WithFontStyle(
            this System.Windows.Controls.DataGridTextColumn target, ValueProxy<System.Windows.FontStyle>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridTextColumn.FontStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridTextColumn WithFontWeight(
            this System.Windows.Controls.DataGridTextColumn target, ValueProxy<System.Windows.FontWeight>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridTextColumn.FontWeightProperty);
            return target;
        }

        public static System.Windows.Controls.DataGridTextColumn WithForeground(
            this System.Windows.Controls.DataGridTextColumn target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DataGridTextColumn.ForegroundProperty);
            return target;
        }

        public static System.Windows.Controls.DatePicker WithCalendarStyle(
            this System.Windows.Controls.DatePicker target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DatePicker.CalendarStyleProperty);
            return target;
        }

        public static System.Windows.Controls.DatePicker WithDisplayDate(this System.Windows.Controls.DatePicker target,
            ValueProxy<System.DateTime>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DatePicker.DisplayDateProperty);
            return target;
        }

        public static System.Windows.Controls.DatePicker WithDisplayDateEnd(
            this System.Windows.Controls.DatePicker target, ValueProxy<System.Nullable<System.DateTime>>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DatePicker.DisplayDateEndProperty);
            return target;
        }

        public static System.Windows.Controls.DatePicker WithDisplayDateStart(
            this System.Windows.Controls.DatePicker target, ValueProxy<System.Nullable<System.DateTime>>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DatePicker.DisplayDateStartProperty);
            return target;
        }

        public static System.Windows.Controls.DatePicker WithFirstDayOfWeek(
            this System.Windows.Controls.DatePicker target, ValueProxy<System.DayOfWeek>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DatePicker.FirstDayOfWeekProperty);
            return target;
        }

        public static System.Windows.Controls.DatePicker WithIsDropDownOpen(
            this System.Windows.Controls.DatePicker target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DatePicker.IsDropDownOpenProperty);
            return target;
        }

        public static System.Windows.Controls.DatePicker WithIsTodayHighlighted(
            this System.Windows.Controls.DatePicker target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DatePicker.IsTodayHighlightedProperty);
            return target;
        }

        public static System.Windows.Controls.DatePicker WithSelectedDate(
            this System.Windows.Controls.DatePicker target, ValueProxy<System.Nullable<System.DateTime>>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DatePicker.SelectedDateProperty);
            return target;
        }

        public static System.Windows.Controls.DatePicker WithSelectedDateFormat(
            this System.Windows.Controls.DatePicker target,
            ValueProxy<System.Windows.Controls.DatePickerFormat>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DatePicker.SelectedDateFormatProperty);
            return target;
        }

        public static System.Windows.Controls.DatePicker WithText(this System.Windows.Controls.DatePicker target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DatePicker.TextProperty);
            return target;
        }

        public static System.Windows.Controls.DefinitionBase WithSharedSizeGroup(
            this System.Windows.Controls.DefinitionBase target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DefinitionBase.SharedSizeGroupProperty);
            return target;
        }

        public static System.Windows.Controls.DockPanel WithLastChildFill(this System.Windows.Controls.DockPanel target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DockPanel.LastChildFillProperty);
            return target;
        }

        public static System.Windows.UIElement WithDockPanel_Dock(this System.Windows.UIElement target,
            ValueProxy<System.Windows.Controls.Dock>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DockPanel.DockProperty);
            return target;
        }

        public static System.Windows.Controls.DocumentViewer WithHorizontalOffset(
            this System.Windows.Controls.DocumentViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DocumentViewer.HorizontalOffsetProperty);
            return target;
        }

        public static System.Windows.Controls.DocumentViewer WithVerticalOffset(
            this System.Windows.Controls.DocumentViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DocumentViewer.VerticalOffsetProperty);
            return target;
        }

        public static System.Windows.Controls.DocumentViewer WithShowPageBorders(
            this System.Windows.Controls.DocumentViewer target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DocumentViewer.ShowPageBordersProperty);
            return target;
        }

        public static System.Windows.Controls.DocumentViewer WithZoom(
            this System.Windows.Controls.DocumentViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DocumentViewer.ZoomProperty);
            return target;
        }

        public static System.Windows.Controls.DocumentViewer WithMaxPagesAcross(
            this System.Windows.Controls.DocumentViewer target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DocumentViewer.MaxPagesAcrossProperty);
            return target;
        }

        public static System.Windows.Controls.DocumentViewer WithVerticalPageSpacing(
            this System.Windows.Controls.DocumentViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DocumentViewer.VerticalPageSpacingProperty);
            return target;
        }

        public static System.Windows.Controls.DocumentViewer WithHorizontalPageSpacing(
            this System.Windows.Controls.DocumentViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.DocumentViewer.HorizontalPageSpacingProperty);
            return target;
        }

        public static System.Windows.Controls.Expander WithExpandDirection(this System.Windows.Controls.Expander target,
            ValueProxy<System.Windows.Controls.ExpandDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Expander.ExpandDirectionProperty);
            return target;
        }

        public static System.Windows.Controls.Expander WithIsExpanded(this System.Windows.Controls.Expander target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Expander.IsExpandedProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithViewingMode(
            this System.Windows.Controls.FlowDocumentReader target,
            ValueProxy<System.Windows.Controls.FlowDocumentReaderViewingMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentReader.ViewingModeProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithIsPageViewEnabled(
            this System.Windows.Controls.FlowDocumentReader target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentReader.IsPageViewEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithIsTwoPageViewEnabled(
            this System.Windows.Controls.FlowDocumentReader target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentReader.IsTwoPageViewEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithIsScrollViewEnabled(
            this System.Windows.Controls.FlowDocumentReader target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentReader.IsScrollViewEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithIsFindEnabled(
            this System.Windows.Controls.FlowDocumentReader target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentReader.IsFindEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithIsPrintEnabled(
            this System.Windows.Controls.FlowDocumentReader target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentReader.IsPrintEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithDocument(
            this System.Windows.Controls.FlowDocumentReader target,
            ValueProxy<System.Windows.Documents.FlowDocument>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentReader.DocumentProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithZoom(
            this System.Windows.Controls.FlowDocumentReader target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentReader.ZoomProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithMaxZoom(
            this System.Windows.Controls.FlowDocumentReader target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentReader.MaxZoomProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithMinZoom(
            this System.Windows.Controls.FlowDocumentReader target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentReader.MinZoomProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithZoomIncrement(
            this System.Windows.Controls.FlowDocumentReader target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentReader.ZoomIncrementProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithSelectionBrush(
            this System.Windows.Controls.FlowDocumentReader target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentReader.SelectionBrushProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithSelectionOpacity(
            this System.Windows.Controls.FlowDocumentReader target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentReader.SelectionOpacityProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentReader WithIsInactiveSelectionHighlightEnabled(
            this System.Windows.Controls.FlowDocumentReader target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.FlowDocumentReader.IsInactiveSelectionHighlightEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentScrollViewer WithDocument(
            this System.Windows.Controls.FlowDocumentScrollViewer target,
            ValueProxy<System.Windows.Documents.FlowDocument>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.DocumentProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentScrollViewer WithZoom(
            this System.Windows.Controls.FlowDocumentScrollViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.ZoomProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentScrollViewer WithMaxZoom(
            this System.Windows.Controls.FlowDocumentScrollViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.MaxZoomProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentScrollViewer WithMinZoom(
            this System.Windows.Controls.FlowDocumentScrollViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.MinZoomProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentScrollViewer WithZoomIncrement(
            this System.Windows.Controls.FlowDocumentScrollViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.ZoomIncrementProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentScrollViewer WithIsSelectionEnabled(
            this System.Windows.Controls.FlowDocumentScrollViewer target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.IsSelectionEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentScrollViewer WithIsToolBarVisible(
            this System.Windows.Controls.FlowDocumentScrollViewer target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.IsToolBarVisibleProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentScrollViewer WithHorizontalScrollBarVisibility(
            this System.Windows.Controls.FlowDocumentScrollViewer target,
            ValueProxy<System.Windows.Controls.ScrollBarVisibility>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.FlowDocumentScrollViewer.HorizontalScrollBarVisibilityProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentScrollViewer WithVerticalScrollBarVisibility(
            this System.Windows.Controls.FlowDocumentScrollViewer target,
            ValueProxy<System.Windows.Controls.ScrollBarVisibility>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.FlowDocumentScrollViewer.VerticalScrollBarVisibilityProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentScrollViewer WithSelectionBrush(
            this System.Windows.Controls.FlowDocumentScrollViewer target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.SelectionBrushProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentScrollViewer WithSelectionOpacity(
            this System.Windows.Controls.FlowDocumentScrollViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.SelectionOpacityProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentScrollViewer WithIsInactiveSelectionHighlightEnabled(
            this System.Windows.Controls.FlowDocumentScrollViewer target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.FlowDocumentScrollViewer.IsInactiveSelectionHighlightEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.Frame WithSource(this System.Windows.Controls.Frame target,
            ValueProxy<System.Uri>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Frame.SourceProperty);
            return target;
        }

        public static System.Windows.Controls.Frame WithNavigationUIVisibility(
            this System.Windows.Controls.Frame target,
            ValueProxy<System.Windows.Navigation.NavigationUIVisibility>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Frame.NavigationUIVisibilityProperty);
            return target;
        }

        public static System.Windows.Controls.Frame WithSandboxExternalContent(
            this System.Windows.Controls.Frame target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Frame.SandboxExternalContentProperty);
            return target;
        }

        public static System.Windows.Controls.Frame WithJournalOwnership(this System.Windows.Controls.Frame target,
            ValueProxy<System.Windows.Navigation.JournalOwnership>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Frame.JournalOwnershipProperty);
            return target;
        }

        public static System.Windows.Controls.Grid WithShowGridLines(this System.Windows.Controls.Grid target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Grid.ShowGridLinesProperty);
            return target;
        }

        public static System.Windows.UIElement WithGrid_Column(this System.Windows.UIElement target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Grid.ColumnProperty);
            return target;
        }

        public static System.Windows.UIElement WithGrid_Row(this System.Windows.UIElement target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Grid.RowProperty);
            return target;
        }

        public static System.Windows.UIElement WithGrid_ColumnSpan(this System.Windows.UIElement target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Grid.ColumnSpanProperty);
            return target;
        }

        public static System.Windows.UIElement WithGrid_RowSpan(this System.Windows.UIElement target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Grid.RowSpanProperty);
            return target;
        }

        public static System.Windows.UIElement WithGrid_IsSharedSizeScope(this System.Windows.UIElement target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Grid.IsSharedSizeScopeProperty);
            return target;
        }

        public static System.Windows.Controls.GridSplitter WithResizeDirection(
            this System.Windows.Controls.GridSplitter target,
            ValueProxy<System.Windows.Controls.GridResizeDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridSplitter.ResizeDirectionProperty);
            return target;
        }

        public static System.Windows.Controls.GridSplitter WithResizeBehavior(
            this System.Windows.Controls.GridSplitter target,
            ValueProxy<System.Windows.Controls.GridResizeBehavior>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridSplitter.ResizeBehaviorProperty);
            return target;
        }

        public static System.Windows.Controls.GridSplitter WithShowsPreview(
            this System.Windows.Controls.GridSplitter target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridSplitter.ShowsPreviewProperty);
            return target;
        }

        public static System.Windows.Controls.GridSplitter WithPreviewStyle(
            this System.Windows.Controls.GridSplitter target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridSplitter.PreviewStyleProperty);
            return target;
        }

        public static System.Windows.Controls.GridSplitter WithKeyboardIncrement(
            this System.Windows.Controls.GridSplitter target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridSplitter.KeyboardIncrementProperty);
            return target;
        }

        public static System.Windows.Controls.GridSplitter WithDragIncrement(
            this System.Windows.Controls.GridSplitter target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridSplitter.DragIncrementProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithGridView_ColumnCollection(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Controls.GridViewColumnCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridView.ColumnCollectionProperty);
            return target;
        }

        public static System.Windows.Controls.GridView WithColumnHeaderContainerStyle(
            this System.Windows.Controls.GridView target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridView.ColumnHeaderContainerStyleProperty);
            return target;
        }

        public static System.Windows.Controls.GridView WithColumnHeaderTemplate(
            this System.Windows.Controls.GridView target, ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridView.ColumnHeaderTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.GridView WithColumnHeaderTemplateSelector(
            this System.Windows.Controls.GridView target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridView.ColumnHeaderTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.GridView WithColumnHeaderStringFormat(
            this System.Windows.Controls.GridView target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridView.ColumnHeaderStringFormatProperty);
            return target;
        }

        public static System.Windows.Controls.GridView WithAllowsColumnReorder(
            this System.Windows.Controls.GridView target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridView.AllowsColumnReorderProperty);
            return target;
        }

        public static System.Windows.Controls.GridView WithColumnHeaderContextMenu(
            this System.Windows.Controls.GridView target, ValueProxy<System.Windows.Controls.ContextMenu>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridView.ColumnHeaderContextMenuProperty);
            return target;
        }

        public static System.Windows.Controls.GridView WithColumnHeaderToolTip(
            this System.Windows.Controls.GridView target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridView.ColumnHeaderToolTipProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewColumn WithHeader(
            this System.Windows.Controls.GridViewColumn target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridViewColumn.HeaderProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewColumn WithHeaderContainerStyle(
            this System.Windows.Controls.GridViewColumn target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridViewColumn.HeaderContainerStyleProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewColumn WithHeaderTemplate(
            this System.Windows.Controls.GridViewColumn target, ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridViewColumn.HeaderTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewColumn WithHeaderTemplateSelector(
            this System.Windows.Controls.GridViewColumn target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridViewColumn.HeaderTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewColumn WithHeaderStringFormat(
            this System.Windows.Controls.GridViewColumn target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridViewColumn.HeaderStringFormatProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewColumn WithCellTemplate(
            this System.Windows.Controls.GridViewColumn target, ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridViewColumn.CellTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewColumn WithCellTemplateSelector(
            this System.Windows.Controls.GridViewColumn target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridViewColumn.CellTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewColumn WithWidth(
            this System.Windows.Controls.GridViewColumn target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridViewColumn.WidthProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewHeaderRowPresenter WithColumnHeaderContainerStyle(
            this System.Windows.Controls.GridViewHeaderRowPresenter target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewHeaderRowPresenter WithColumnHeaderTemplate(
            this System.Windows.Controls.GridViewHeaderRowPresenter target,
            ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewHeaderRowPresenter WithColumnHeaderTemplateSelector(
            this System.Windows.Controls.GridViewHeaderRowPresenter target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewHeaderRowPresenter WithColumnHeaderStringFormat(
            this System.Windows.Controls.GridViewHeaderRowPresenter target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderStringFormatProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewHeaderRowPresenter WithAllowsColumnReorder(
            this System.Windows.Controls.GridViewHeaderRowPresenter target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridViewHeaderRowPresenter.AllowsColumnReorderProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewHeaderRowPresenter WithColumnHeaderContextMenu(
            this System.Windows.Controls.GridViewHeaderRowPresenter target,
            ValueProxy<System.Windows.Controls.ContextMenu>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderContextMenuProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewHeaderRowPresenter WithColumnHeaderToolTip(
            this System.Windows.Controls.GridViewHeaderRowPresenter target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderToolTipProperty);
            return target;
        }

        public static System.Windows.Controls.GridViewRowPresenter WithContent(
            this System.Windows.Controls.GridViewRowPresenter target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.GridViewRowPresenter.ContentProperty);
            return target;
        }

        public static System.Windows.Controls.HeaderedContentControl WithHeader(
            this System.Windows.Controls.HeaderedContentControl target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.HeaderedContentControl.HeaderProperty);
            return target;
        }

        public static System.Windows.Controls.HeaderedContentControl WithHeaderTemplate(
            this System.Windows.Controls.HeaderedContentControl target,
            ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.HeaderedContentControl.HeaderTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.HeaderedContentControl WithHeaderTemplateSelector(
            this System.Windows.Controls.HeaderedContentControl target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.HeaderedContentControl.HeaderTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.HeaderedContentControl WithHeaderStringFormat(
            this System.Windows.Controls.HeaderedContentControl target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.HeaderedContentControl.HeaderStringFormatProperty);
            return target;
        }

        public static System.Windows.Controls.HeaderedItemsControl WithHeader(
            this System.Windows.Controls.HeaderedItemsControl target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.HeaderedItemsControl.HeaderProperty);
            return target;
        }

        public static System.Windows.Controls.HeaderedItemsControl WithHeaderTemplate(
            this System.Windows.Controls.HeaderedItemsControl target,
            ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.HeaderedItemsControl.HeaderTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.HeaderedItemsControl WithHeaderTemplateSelector(
            this System.Windows.Controls.HeaderedItemsControl target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.HeaderedItemsControl.HeaderTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.HeaderedItemsControl WithHeaderStringFormat(
            this System.Windows.Controls.HeaderedItemsControl target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.HeaderedItemsControl.HeaderStringFormatProperty);
            return target;
        }

        public static System.Windows.Controls.Image WithSource(this System.Windows.Controls.Image target,
            ValueProxy<System.Windows.Media.ImageSource>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Image.SourceProperty);
            return target;
        }

        public static System.Windows.Controls.Image WithStretch(this System.Windows.Controls.Image target,
            ValueProxy<System.Windows.Media.Stretch>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Image.StretchProperty);
            return target;
        }

        public static System.Windows.Controls.Image WithStretchDirection(this System.Windows.Controls.Image target,
            ValueProxy<System.Windows.Controls.StretchDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Image.StretchDirectionProperty);
            return target;
        }

        public static System.Windows.Controls.InkCanvas WithBackground(this System.Windows.Controls.InkCanvas target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.InkCanvas.BackgroundProperty);
            return target;
        }

        public static System.Windows.UIElement WithInkCanvas_Top(this System.Windows.UIElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.InkCanvas.TopProperty);
            return target;
        }

        public static System.Windows.UIElement WithInkCanvas_Bottom(this System.Windows.UIElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.InkCanvas.BottomProperty);
            return target;
        }

        public static System.Windows.UIElement WithInkCanvas_Left(this System.Windows.UIElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.InkCanvas.LeftProperty);
            return target;
        }

        public static System.Windows.UIElement WithInkCanvas_Right(this System.Windows.UIElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.InkCanvas.RightProperty);
            return target;
        }

        public static System.Windows.Controls.InkCanvas WithStrokes(this System.Windows.Controls.InkCanvas target,
            ValueProxy<System.Windows.Ink.StrokeCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.InkCanvas.StrokesProperty);
            return target;
        }

        public static System.Windows.Controls.InkCanvas WithDefaultDrawingAttributes(
            this System.Windows.Controls.InkCanvas target, ValueProxy<System.Windows.Ink.DrawingAttributes>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.InkCanvas.DefaultDrawingAttributesProperty);
            return target;
        }

        public static System.Windows.Controls.InkCanvas WithEditingMode(this System.Windows.Controls.InkCanvas target,
            ValueProxy<System.Windows.Controls.InkCanvasEditingMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.InkCanvas.EditingModeProperty);
            return target;
        }

        public static System.Windows.Controls.InkCanvas WithEditingModeInverted(
            this System.Windows.Controls.InkCanvas target,
            ValueProxy<System.Windows.Controls.InkCanvasEditingMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.InkCanvas.EditingModeInvertedProperty);
            return target;
        }

        public static System.Windows.Controls.InkPresenter WithStrokes(this System.Windows.Controls.InkPresenter target,
            ValueProxy<System.Windows.Ink.StrokeCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.InkPresenter.StrokesProperty);
            return target;
        }

        public static System.Windows.Controls.ItemsControl WithItemsSource(
            this System.Windows.Controls.ItemsControl target, ValueProxy<System.Collections.IEnumerable>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ItemsControl.ItemsSourceProperty);
            return target;
        }

        public static System.Windows.Controls.ItemsControl WithDisplayMemberPath(
            this System.Windows.Controls.ItemsControl target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ItemsControl.DisplayMemberPathProperty);
            return target;
        }

        public static System.Windows.Controls.ItemsControl WithItemTemplate(
            this System.Windows.Controls.ItemsControl target, ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ItemsControl.ItemTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.ItemsControl WithItemTemplateSelector(
            this System.Windows.Controls.ItemsControl target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ItemsControl.ItemTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.ItemsControl WithItemStringFormat(
            this System.Windows.Controls.ItemsControl target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ItemsControl.ItemStringFormatProperty);
            return target;
        }

        public static System.Windows.Controls.ItemsControl WithItemBindingGroup(
            this System.Windows.Controls.ItemsControl target, ValueProxy<System.Windows.Data.BindingGroup>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ItemsControl.ItemBindingGroupProperty);
            return target;
        }

        public static System.Windows.Controls.ItemsControl WithItemContainerStyle(
            this System.Windows.Controls.ItemsControl target, ValueProxy<System.Windows.Style>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ItemsControl.ItemContainerStyleProperty);
            return target;
        }

        public static System.Windows.Controls.ItemsControl WithItemContainerStyleSelector(
            this System.Windows.Controls.ItemsControl target,
            ValueProxy<System.Windows.Controls.StyleSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ItemsControl.ItemContainerStyleSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.ItemsControl WithItemsPanel(
            this System.Windows.Controls.ItemsControl target,
            ValueProxy<System.Windows.Controls.ItemsPanelTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ItemsControl.ItemsPanelProperty);
            return target;
        }

        public static System.Windows.Controls.ItemsControl WithGroupStyleSelector(
            this System.Windows.Controls.ItemsControl target,
            ValueProxy<System.Windows.Controls.GroupStyleSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ItemsControl.GroupStyleSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.ItemsControl WithAlternationCount(
            this System.Windows.Controls.ItemsControl target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ItemsControl.AlternationCountProperty);
            return target;
        }

        public static System.Windows.Controls.ItemsControl WithIsTextSearchEnabled(
            this System.Windows.Controls.ItemsControl target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ItemsControl.IsTextSearchEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.ItemsControl WithIsTextSearchCaseSensitive(
            this System.Windows.Controls.ItemsControl target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ItemsControl.IsTextSearchCaseSensitiveProperty);
            return target;
        }

        public static System.Windows.Controls.Label WithTarget(this System.Windows.Controls.Label target,
            ValueProxy<System.Windows.UIElement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Label.TargetProperty);
            return target;
        }

        public static System.Windows.Controls.ListBox WithSelectionMode(this System.Windows.Controls.ListBox target,
            ValueProxy<System.Windows.Controls.SelectionMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ListBox.SelectionModeProperty);
            return target;
        }

        public static System.Windows.Controls.ListBoxItem WithIsSelected(
            this System.Windows.Controls.ListBoxItem target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ListBoxItem.IsSelectedProperty);
            return target;
        }

        public static System.Windows.Controls.ListView WithView(this System.Windows.Controls.ListView target,
            ValueProxy<System.Windows.Controls.ViewBase>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ListView.ViewProperty);
            return target;
        }

        public static System.Windows.Controls.MediaElement WithSource(this System.Windows.Controls.MediaElement target,
            ValueProxy<System.Uri>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MediaElement.SourceProperty);
            return target;
        }

        public static System.Windows.Controls.MediaElement WithVolume(this System.Windows.Controls.MediaElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MediaElement.VolumeProperty);
            return target;
        }

        public static System.Windows.Controls.MediaElement WithBalance(this System.Windows.Controls.MediaElement target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MediaElement.BalanceProperty);
            return target;
        }

        public static System.Windows.Controls.MediaElement WithIsMuted(this System.Windows.Controls.MediaElement target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MediaElement.IsMutedProperty);
            return target;
        }

        public static System.Windows.Controls.MediaElement WithScrubbingEnabled(
            this System.Windows.Controls.MediaElement target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MediaElement.ScrubbingEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.MediaElement WithUnloadedBehavior(
            this System.Windows.Controls.MediaElement target, ValueProxy<System.Windows.Controls.MediaState>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MediaElement.UnloadedBehaviorProperty);
            return target;
        }

        public static System.Windows.Controls.MediaElement WithLoadedBehavior(
            this System.Windows.Controls.MediaElement target, ValueProxy<System.Windows.Controls.MediaState>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MediaElement.LoadedBehaviorProperty);
            return target;
        }

        public static System.Windows.Controls.MediaElement WithStretch(this System.Windows.Controls.MediaElement target,
            ValueProxy<System.Windows.Media.Stretch>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MediaElement.StretchProperty);
            return target;
        }

        public static System.Windows.Controls.MediaElement WithStretchDirection(
            this System.Windows.Controls.MediaElement target,
            ValueProxy<System.Windows.Controls.StretchDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MediaElement.StretchDirectionProperty);
            return target;
        }

        public static System.Windows.Controls.Menu WithIsMainMenu(this System.Windows.Controls.Menu target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Menu.IsMainMenuProperty);
            return target;
        }

        public static System.Windows.Controls.MenuItem WithCommand(this System.Windows.Controls.MenuItem target,
            ValueProxy<System.Windows.Input.ICommand>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MenuItem.CommandProperty);
            return target;
        }

        public static System.Windows.Controls.MenuItem WithCommandParameter(
            this System.Windows.Controls.MenuItem target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MenuItem.CommandParameterProperty);
            return target;
        }

        public static System.Windows.Controls.MenuItem WithCommandTarget(this System.Windows.Controls.MenuItem target,
            ValueProxy<System.Windows.IInputElement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MenuItem.CommandTargetProperty);
            return target;
        }

        public static System.Windows.Controls.MenuItem WithIsSubmenuOpen(this System.Windows.Controls.MenuItem target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MenuItem.IsSubmenuOpenProperty);
            return target;
        }

        public static System.Windows.Controls.MenuItem WithIsCheckable(this System.Windows.Controls.MenuItem target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MenuItem.IsCheckableProperty);
            return target;
        }

        public static System.Windows.Controls.MenuItem WithIsChecked(this System.Windows.Controls.MenuItem target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MenuItem.IsCheckedProperty);
            return target;
        }

        public static System.Windows.Controls.MenuItem WithStaysOpenOnClick(
            this System.Windows.Controls.MenuItem target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MenuItem.StaysOpenOnClickProperty);
            return target;
        }

        public static System.Windows.Controls.MenuItem WithInputGestureText(
            this System.Windows.Controls.MenuItem target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MenuItem.InputGestureTextProperty);
            return target;
        }

        public static System.Windows.Controls.MenuItem WithIcon(this System.Windows.Controls.MenuItem target,
            ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MenuItem.IconProperty);
            return target;
        }

        public static System.Windows.Controls.MenuItem WithItemContainerTemplateSelector(
            this System.Windows.Controls.MenuItem target,
            ValueProxy<System.Windows.Controls.ItemContainerTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MenuItem.ItemContainerTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.MenuItem WithUsesItemContainerTemplate(
            this System.Windows.Controls.MenuItem target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.MenuItem.UsesItemContainerTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.Page WithContent(this System.Windows.Controls.Page target,
            ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Page.ContentProperty);
            return target;
        }

        public static System.Windows.Controls.Page WithBackground(this System.Windows.Controls.Page target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Page.BackgroundProperty);
            return target;
        }

        public static System.Windows.Controls.Page WithTitle(this System.Windows.Controls.Page target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Page.TitleProperty);
            return target;
        }

        public static System.Windows.Controls.Page WithKeepAlive(this System.Windows.Controls.Page target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Page.KeepAliveProperty);
            return target;
        }

        public static System.Windows.Controls.Page WithForeground(this System.Windows.Controls.Page target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Page.ForegroundProperty);
            return target;
        }

        public static System.Windows.Controls.Page WithFontFamily(this System.Windows.Controls.Page target,
            ValueProxy<System.Windows.Media.FontFamily>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Page.FontFamilyProperty);
            return target;
        }

        public static System.Windows.Controls.Page WithFontSize(this System.Windows.Controls.Page target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Page.FontSizeProperty);
            return target;
        }

        public static System.Windows.Controls.Page WithTemplate(this System.Windows.Controls.Page target,
            ValueProxy<System.Windows.Controls.ControlTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Page.TemplateProperty);
            return target;
        }

        public static System.Windows.Controls.Panel WithBackground(this System.Windows.Controls.Panel target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Panel.BackgroundProperty);
            return target;
        }

        public static System.Windows.Controls.Panel WithIsItemsHost(this System.Windows.Controls.Panel target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Panel.IsItemsHostProperty);
            return target;
        }

        public static System.Windows.UIElement WithPanel_ZIndex(this System.Windows.UIElement target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Panel.ZIndexProperty);
            return target;
        }

        public static System.Windows.Controls.PasswordBox WithPasswordChar(
            this System.Windows.Controls.PasswordBox target, ValueProxy<System.Char>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.PasswordBox.PasswordCharProperty);
            return target;
        }

        public static System.Windows.Controls.PasswordBox WithMaxLength(this System.Windows.Controls.PasswordBox target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.PasswordBox.MaxLengthProperty);
            return target;
        }

        public static System.Windows.Controls.PasswordBox WithSelectionBrush(
            this System.Windows.Controls.PasswordBox target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.PasswordBox.SelectionBrushProperty);
            return target;
        }

        public static System.Windows.Controls.PasswordBox WithSelectionTextBrush(
            this System.Windows.Controls.PasswordBox target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.PasswordBox.SelectionTextBrushProperty);
            return target;
        }

        public static System.Windows.Controls.PasswordBox WithSelectionOpacity(
            this System.Windows.Controls.PasswordBox target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.PasswordBox.SelectionOpacityProperty);
            return target;
        }

        public static System.Windows.Controls.PasswordBox WithCaretBrush(
            this System.Windows.Controls.PasswordBox target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.PasswordBox.CaretBrushProperty);
            return target;
        }

        public static System.Windows.Controls.PasswordBox WithIsInactiveSelectionHighlightEnabled(
            this System.Windows.Controls.PasswordBox target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.PasswordBox.IsInactiveSelectionHighlightEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.ScrollContentPresenter WithCanContentScroll(
            this System.Windows.Controls.ScrollContentPresenter target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ScrollContentPresenter.CanContentScrollProperty);
            return target;
        }

        public static System.Windows.Controls.ProgressBar WithIsIndeterminate(
            this System.Windows.Controls.ProgressBar target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ProgressBar.IsIndeterminateProperty);
            return target;
        }

        public static System.Windows.Controls.ProgressBar WithOrientation(
            this System.Windows.Controls.ProgressBar target, ValueProxy<System.Windows.Controls.Orientation>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ProgressBar.OrientationProperty);
            return target;
        }

        public static System.Windows.Controls.RadioButton WithGroupName(this System.Windows.Controls.RadioButton target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.RadioButton.GroupNameProperty);
            return target;
        }

        public static System.Windows.Controls.RichTextBox WithIsDocumentEnabled(
            this System.Windows.Controls.RichTextBox target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.RichTextBox.IsDocumentEnabledProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithScrollViewer_CanContentScroll(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ScrollViewer.CanContentScrollProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithScrollViewer_HorizontalScrollBarVisibility(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Controls.ScrollBarVisibility>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ScrollViewer.HorizontalScrollBarVisibilityProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithScrollViewer_VerticalScrollBarVisibility(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Controls.ScrollBarVisibility>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ScrollViewer.VerticalScrollBarVisibilityProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithScrollViewer_IsDeferredScrollingEnabled(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ScrollViewer.IsDeferredScrollingEnabledProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithScrollViewer_PanningMode(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Controls.PanningMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ScrollViewer.PanningModeProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithScrollViewer_PanningDeceleration(
            this System.Windows.DependencyObject target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ScrollViewer.PanningDecelerationProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithScrollViewer_PanningRatio(
            this System.Windows.DependencyObject target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ScrollViewer.PanningRatioProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentPageViewer WithZoom(
            this System.Windows.Controls.FlowDocumentPageViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.ZoomProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentPageViewer WithMaxZoom(
            this System.Windows.Controls.FlowDocumentPageViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.MaxZoomProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentPageViewer WithMinZoom(
            this System.Windows.Controls.FlowDocumentPageViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.MinZoomProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentPageViewer WithZoomIncrement(
            this System.Windows.Controls.FlowDocumentPageViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.ZoomIncrementProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentPageViewer WithSelectionBrush(
            this System.Windows.Controls.FlowDocumentPageViewer target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.SelectionBrushProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentPageViewer WithSelectionOpacity(
            this System.Windows.Controls.FlowDocumentPageViewer target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.SelectionOpacityProperty);
            return target;
        }

        public static System.Windows.Controls.FlowDocumentPageViewer WithIsInactiveSelectionHighlightEnabled(
            this System.Windows.Controls.FlowDocumentPageViewer target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.FlowDocumentPageViewer.IsInactiveSelectionHighlightEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithOrientation(this System.Windows.Controls.Slider target,
            ValueProxy<System.Windows.Controls.Orientation>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.OrientationProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithIsDirectionReversed(this System.Windows.Controls.Slider target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.IsDirectionReversedProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithDelay(this System.Windows.Controls.Slider target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.DelayProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithInterval(this System.Windows.Controls.Slider target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.IntervalProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithAutoToolTipPlacement(
            this System.Windows.Controls.Slider target,
            ValueProxy<System.Windows.Controls.Primitives.AutoToolTipPlacement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.AutoToolTipPlacementProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithAutoToolTipPrecision(
            this System.Windows.Controls.Slider target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.AutoToolTipPrecisionProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithIsSnapToTickEnabled(this System.Windows.Controls.Slider target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.IsSnapToTickEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithTickPlacement(this System.Windows.Controls.Slider target,
            ValueProxy<System.Windows.Controls.Primitives.TickPlacement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.TickPlacementProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithTickFrequency(this System.Windows.Controls.Slider target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.TickFrequencyProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithTicks(this System.Windows.Controls.Slider target,
            ValueProxy<System.Windows.Media.DoubleCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.TicksProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithIsSelectionRangeEnabled(
            this System.Windows.Controls.Slider target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.IsSelectionRangeEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithSelectionStart(this System.Windows.Controls.Slider target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.SelectionStartProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithSelectionEnd(this System.Windows.Controls.Slider target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.SelectionEndProperty);
            return target;
        }

        public static System.Windows.Controls.Slider WithIsMoveToPointEnabled(
            this System.Windows.Controls.Slider target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Slider.IsMoveToPointEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.SoundPlayerAction WithSource(
            this System.Windows.Controls.SoundPlayerAction target, ValueProxy<System.Uri>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.SoundPlayerAction.SourceProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithSpellCheck_IsEnabled(
            this System.Windows.Controls.Primitives.TextBoxBase target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.SpellCheck.IsEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithSpellCheck_SpellingReform(
            this System.Windows.Controls.Primitives.TextBoxBase target,
            ValueProxy<System.Windows.Controls.SpellingReform>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.SpellCheck.SpellingReformProperty);
            return target;
        }

        public static System.Windows.Controls.StackPanel WithOrientation(this System.Windows.Controls.StackPanel target,
            ValueProxy<System.Windows.Controls.Orientation>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.StackPanel.OrientationProperty);
            return target;
        }

        public static System.Windows.Controls.TabControl WithTabStripPlacement(
            this System.Windows.Controls.TabControl target, ValueProxy<System.Windows.Controls.Dock>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TabControl.TabStripPlacementProperty);
            return target;
        }

        public static System.Windows.Controls.TabControl WithContentTemplate(
            this System.Windows.Controls.TabControl target, ValueProxy<System.Windows.DataTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TabControl.ContentTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.TabControl WithContentTemplateSelector(
            this System.Windows.Controls.TabControl target,
            ValueProxy<System.Windows.Controls.DataTemplateSelector>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TabControl.ContentTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.TabControl WithContentStringFormat(
            this System.Windows.Controls.TabControl target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TabControl.ContentStringFormatProperty);
            return target;
        }

        public static System.Windows.Controls.TabItem WithIsSelected(this System.Windows.Controls.TabItem target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TabItem.IsSelectedProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextBlock_BaselineOffset(
            this System.Windows.DependencyObject target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.BaselineOffsetProperty);
            return target;
        }

        public static System.Windows.Controls.TextBlock WithText(this System.Windows.Controls.TextBlock target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.TextProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextBlock_FontFamily(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Media.FontFamily>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.FontFamilyProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextBlock_FontStyle(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FontStyle>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.FontStyleProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextBlock_FontWeight(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FontWeight>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.FontWeightProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextBlock_FontStretch(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.FontStretch>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.FontStretchProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextBlock_FontSize(
            this System.Windows.DependencyObject target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.FontSizeProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextBlock_Foreground(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.ForegroundProperty);
            return target;
        }

        public static System.Windows.Controls.TextBlock WithBackground(this System.Windows.Controls.TextBlock target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.BackgroundProperty);
            return target;
        }

        public static System.Windows.Controls.TextBlock WithTextDecorations(
            this System.Windows.Controls.TextBlock target,
            ValueProxy<System.Windows.TextDecorationCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.TextDecorationsProperty);
            return target;
        }

        public static System.Windows.Controls.TextBlock WithTextEffects(this System.Windows.Controls.TextBlock target,
            ValueProxy<System.Windows.Media.TextEffectCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.TextEffectsProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextBlock_LineHeight(
            this System.Windows.DependencyObject target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.LineHeightProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextBlock_LineStackingStrategy(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.LineStackingStrategy>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.LineStackingStrategyProperty);
            return target;
        }

        public static System.Windows.Controls.TextBlock WithPadding(this System.Windows.Controls.TextBlock target,
            ValueProxy<System.Windows.Thickness>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.PaddingProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextBlock_TextAlignment(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.TextAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.TextAlignmentProperty);
            return target;
        }

        public static System.Windows.Controls.TextBlock WithTextTrimming(this System.Windows.Controls.TextBlock target,
            ValueProxy<System.Windows.TextTrimming>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.TextTrimmingProperty);
            return target;
        }

        public static System.Windows.Controls.TextBlock WithTextWrapping(this System.Windows.Controls.TextBlock target,
            ValueProxy<System.Windows.TextWrapping>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.TextWrappingProperty);
            return target;
        }

        public static System.Windows.Controls.TextBlock WithIsHyphenationEnabled(
            this System.Windows.Controls.TextBlock target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBlock.IsHyphenationEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.TextBox WithTextWrapping(this System.Windows.Controls.TextBox target,
            ValueProxy<System.Windows.TextWrapping>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBox.TextWrappingProperty);
            return target;
        }

        public static System.Windows.Controls.TextBox WithMinLines(this System.Windows.Controls.TextBox target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBox.MinLinesProperty);
            return target;
        }

        public static System.Windows.Controls.TextBox WithMaxLines(this System.Windows.Controls.TextBox target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBox.MaxLinesProperty);
            return target;
        }

        public static System.Windows.Controls.TextBox WithText(this System.Windows.Controls.TextBox target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBox.TextProperty);
            return target;
        }

        public static System.Windows.Controls.TextBox WithCharacterCasing(this System.Windows.Controls.TextBox target,
            ValueProxy<System.Windows.Controls.CharacterCasing>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBox.CharacterCasingProperty);
            return target;
        }

        public static System.Windows.Controls.TextBox WithMaxLength(this System.Windows.Controls.TextBox target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBox.MaxLengthProperty);
            return target;
        }

        public static System.Windows.Controls.TextBox WithTextAlignment(this System.Windows.Controls.TextBox target,
            ValueProxy<System.Windows.TextAlignment>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBox.TextAlignmentProperty);
            return target;
        }

        public static System.Windows.Controls.TextBox WithTextDecorations(this System.Windows.Controls.TextBox target,
            ValueProxy<System.Windows.TextDecorationCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextBox.TextDecorationsProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextSearch_TextPath(
            this System.Windows.DependencyObject target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextSearch.TextPathProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithTextSearch_Text(this System.Windows.DependencyObject target,
            ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TextSearch.TextProperty);
            return target;
        }

        public static System.Windows.Controls.ToolBar WithBand(this System.Windows.Controls.ToolBar target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolBar.BandProperty);
            return target;
        }

        public static System.Windows.Controls.ToolBar WithBandIndex(this System.Windows.Controls.ToolBar target,
            ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolBar.BandIndexProperty);
            return target;
        }

        public static System.Windows.Controls.ToolBar WithIsOverflowOpen(this System.Windows.Controls.ToolBar target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolBar.IsOverflowOpenProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolBar_OverflowMode(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Controls.OverflowMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolBar.OverflowModeProperty);
            return target;
        }

        public static System.Windows.Controls.ToolBarTray WithBackground(
            this System.Windows.Controls.ToolBarTray target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolBarTray.BackgroundProperty);
            return target;
        }

        public static System.Windows.Controls.ToolBarTray WithOrientation(
            this System.Windows.Controls.ToolBarTray target, ValueProxy<System.Windows.Controls.Orientation>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolBarTray.OrientationProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolBarTray_IsLocked(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolBarTray.IsLockedProperty);
            return target;
        }

        public static System.Windows.Controls.ToolTip WithHorizontalOffset(this System.Windows.Controls.ToolTip target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTip.HorizontalOffsetProperty);
            return target;
        }

        public static System.Windows.Controls.ToolTip WithVerticalOffset(this System.Windows.Controls.ToolTip target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTip.VerticalOffsetProperty);
            return target;
        }

        public static System.Windows.Controls.ToolTip WithIsOpen(this System.Windows.Controls.ToolTip target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTip.IsOpenProperty);
            return target;
        }

        public static System.Windows.Controls.ToolTip WithHasDropShadow(this System.Windows.Controls.ToolTip target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTip.HasDropShadowProperty);
            return target;
        }

        public static System.Windows.Controls.ToolTip WithPlacementTarget(this System.Windows.Controls.ToolTip target,
            ValueProxy<System.Windows.UIElement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTip.PlacementTargetProperty);
            return target;
        }

        public static System.Windows.Controls.ToolTip WithPlacementRectangle(
            this System.Windows.Controls.ToolTip target, ValueProxy<System.Windows.Rect>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTip.PlacementRectangleProperty);
            return target;
        }

        public static System.Windows.Controls.ToolTip WithPlacement(this System.Windows.Controls.ToolTip target,
            ValueProxy<System.Windows.Controls.Primitives.PlacementMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTip.PlacementProperty);
            return target;
        }

        public static System.Windows.Controls.ToolTip WithCustomPopupPlacementCallback(
            this System.Windows.Controls.ToolTip target,
            ValueProxy<System.Windows.Controls.Primitives.CustomPopupPlacementCallback>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTip.CustomPopupPlacementCallbackProperty);
            return target;
        }

        public static System.Windows.Controls.ToolTip WithStaysOpen(this System.Windows.Controls.ToolTip target,
            ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTip.StaysOpenProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolTipService_ToolTip(
            this System.Windows.DependencyObject target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTipService.ToolTipProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolTipService_HorizontalOffset(
            this System.Windows.DependencyObject target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTipService.HorizontalOffsetProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolTipService_VerticalOffset(
            this System.Windows.DependencyObject target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTipService.VerticalOffsetProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolTipService_HasDropShadow(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTipService.HasDropShadowProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolTipService_PlacementTarget(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.UIElement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTipService.PlacementTargetProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolTipService_PlacementRectangle(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Rect>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTipService.PlacementRectangleProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolTipService_Placement(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Controls.Primitives.PlacementMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTipService.PlacementProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolTipService_ShowOnDisabled(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTipService.ShowOnDisabledProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolTipService_IsEnabled(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTipService.IsEnabledProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolTipService_ShowDuration(
            this System.Windows.DependencyObject target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTipService.ShowDurationProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolTipService_InitialShowDelay(
            this System.Windows.DependencyObject target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTipService.InitialShowDelayProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithToolTipService_BetweenShowDelay(
            this System.Windows.DependencyObject target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ToolTipService.BetweenShowDelayProperty);
            return target;
        }

        public static System.Windows.Controls.TreeView WithSelectedValuePath(
            this System.Windows.Controls.TreeView target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TreeView.SelectedValuePathProperty);
            return target;
        }

        public static System.Windows.Controls.TreeViewItem WithIsExpanded(
            this System.Windows.Controls.TreeViewItem target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TreeViewItem.IsExpandedProperty);
            return target;
        }

        public static System.Windows.Controls.TreeViewItem WithIsSelected(
            this System.Windows.Controls.TreeViewItem target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.TreeViewItem.IsSelectedProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithValidation_ErrorTemplate(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Controls.ControlTemplate>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Validation.ErrorTemplateProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithValidation_ValidationAdornerSite(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.DependencyObject>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Validation.ValidationAdornerSiteProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithValidation_ValidationAdornerSiteFor(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.DependencyObject>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Validation.ValidationAdornerSiteForProperty);
            return target;
        }

        public static System.Windows.Controls.Viewbox WithStretch(this System.Windows.Controls.Viewbox target,
            ValueProxy<System.Windows.Media.Stretch>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Viewbox.StretchProperty);
            return target;
        }

        public static System.Windows.Controls.Viewbox WithStretchDirection(this System.Windows.Controls.Viewbox target,
            ValueProxy<System.Windows.Controls.StretchDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Viewbox.StretchDirectionProperty);
            return target;
        }

        public static System.Windows.Controls.Viewport3D WithCamera(this System.Windows.Controls.Viewport3D target,
            ValueProxy<System.Windows.Media.Media3D.Camera>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Viewport3D.CameraProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithVirtualizingPanel_IsVirtualizing(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.VirtualizingPanel.IsVirtualizingProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithVirtualizingPanel_VirtualizationMode(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Controls.VirtualizationMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.VirtualizingPanel.VirtualizationModeProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithVirtualizingPanel_IsVirtualizingWhenGrouping(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.VirtualizingPanel.IsVirtualizingWhenGroupingProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithVirtualizingPanel_ScrollUnit(
            this System.Windows.DependencyObject target, ValueProxy<System.Windows.Controls.ScrollUnit>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.VirtualizingPanel.ScrollUnitProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithVirtualizingPanel_CacheLength(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Controls.VirtualizationCacheLength>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.VirtualizingPanel.CacheLengthProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithVirtualizingPanel_CacheLengthUnit(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Controls.VirtualizationCacheLengthUnit>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.VirtualizingPanel.CacheLengthUnitProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithVirtualizingPanel_IsContainerVirtualizable(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.VirtualizingPanel.IsContainerVirtualizableProperty);
            return target;
        }

        public static System.Windows.Controls.VirtualizingStackPanel WithIsVirtualizing(
            this System.Windows.Controls.VirtualizingStackPanel target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.VirtualizingStackPanel.IsVirtualizingProperty);
            return target;
        }

        public static System.Windows.Controls.VirtualizingStackPanel WithVirtualizationMode(
            this System.Windows.Controls.VirtualizingStackPanel target,
            ValueProxy<System.Windows.Controls.VirtualizationMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.VirtualizingStackPanel.VirtualizationModeProperty);
            return target;
        }

        public static System.Windows.Controls.VirtualizingStackPanel WithOrientation(
            this System.Windows.Controls.VirtualizingStackPanel target,
            ValueProxy<System.Windows.Controls.Orientation>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.VirtualizingStackPanel.OrientationProperty);
            return target;
        }

        public static System.Windows.Controls.WrapPanel WithItemWidth(this System.Windows.Controls.WrapPanel target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.WrapPanel.ItemWidthProperty);
            return target;
        }

        public static System.Windows.Controls.WrapPanel WithItemHeight(this System.Windows.Controls.WrapPanel target,
            ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.WrapPanel.ItemHeightProperty);
            return target;
        }

        public static System.Windows.Controls.WrapPanel WithOrientation(this System.Windows.Controls.WrapPanel target,
            ValueProxy<System.Windows.Controls.Orientation>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.WrapPanel.OrientationProperty);
            return target;
        }

        public static System.Windows.Controls.ColumnDefinition WithWidth(
            this System.Windows.Controls.ColumnDefinition target, ValueProxy<System.Windows.GridLength>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ColumnDefinition.WidthProperty);
            return target;
        }

        public static System.Windows.Controls.ColumnDefinition WithMinWidth(
            this System.Windows.Controls.ColumnDefinition target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ColumnDefinition.MinWidthProperty);
            return target;
        }

        public static System.Windows.Controls.ColumnDefinition WithMaxWidth(
            this System.Windows.Controls.ColumnDefinition target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.ColumnDefinition.MaxWidthProperty);
            return target;
        }

        public static System.Windows.Controls.RowDefinition WithHeight(
            this System.Windows.Controls.RowDefinition target, ValueProxy<System.Windows.GridLength>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.RowDefinition.HeightProperty);
            return target;
        }

        public static System.Windows.Controls.RowDefinition WithMinHeight(
            this System.Windows.Controls.RowDefinition target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.RowDefinition.MinHeightProperty);
            return target;
        }

        public static System.Windows.Controls.RowDefinition WithMaxHeight(
            this System.Windows.Controls.RowDefinition target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.RowDefinition.MaxHeightProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.BulletDecorator WithBackground(
            this System.Windows.Controls.Primitives.BulletDecorator target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.BulletDecorator.BackgroundProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.ButtonBase WithCommand(
            this System.Windows.Controls.Primitives.ButtonBase target,
            ValueProxy<System.Windows.Input.ICommand>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.ButtonBase.CommandProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.ButtonBase WithCommandParameter(
            this System.Windows.Controls.Primitives.ButtonBase target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.ButtonBase.CommandParameterProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.ButtonBase WithCommandTarget(
            this System.Windows.Controls.Primitives.ButtonBase target,
            ValueProxy<System.Windows.IInputElement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.ButtonBase.CommandTargetProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.ButtonBase WithClickMode(
            this System.Windows.Controls.Primitives.ButtonBase target,
            ValueProxy<System.Windows.Controls.ClickMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.ButtonBase.ClickModeProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.DataGridColumnHeader WithSeparatorBrush(
            this System.Windows.Controls.Primitives.DataGridColumnHeader target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.DataGridColumnHeader.SeparatorBrushProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.DataGridColumnHeader WithSeparatorVisibility(
            this System.Windows.Controls.Primitives.DataGridColumnHeader target,
            ValueProxy<System.Windows.Visibility>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.Primitives.DataGridColumnHeader.SeparatorVisibilityProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.DataGridRowHeader WithSeparatorBrush(
            this System.Windows.Controls.Primitives.DataGridRowHeader target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.DataGridRowHeader.SeparatorBrushProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.DataGridRowHeader WithSeparatorVisibility(
            this System.Windows.Controls.Primitives.DataGridRowHeader target,
            ValueProxy<System.Windows.Visibility>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.Primitives.DataGridRowHeader.SeparatorVisibilityProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.DocumentPageView WithPageNumber(
            this System.Windows.Controls.Primitives.DocumentPageView target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.DocumentPageView.PageNumberProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.DocumentPageView WithStretch(
            this System.Windows.Controls.Primitives.DocumentPageView target,
            ValueProxy<System.Windows.Media.Stretch>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.DocumentPageView.StretchProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.DocumentPageView WithStretchDirection(
            this System.Windows.Controls.Primitives.DocumentPageView target,
            ValueProxy<System.Windows.Controls.StretchDirection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.DocumentPageView.StretchDirectionProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.DocumentViewerBase WithDocument(
            this System.Windows.Controls.Primitives.DocumentViewerBase target,
            ValueProxy<System.Windows.Documents.IDocumentPaginatorSource>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.DocumentViewerBase.DocumentProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithDocumentViewerBase_IsMasterPage(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.DocumentViewerBase.IsMasterPageProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.GridViewRowPresenterBase WithColumns(
            this System.Windows.Controls.Primitives.GridViewRowPresenterBase target,
            ValueProxy<System.Windows.Controls.GridViewColumnCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.GridViewRowPresenterBase.ColumnsProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.MenuBase WithItemContainerTemplateSelector(
            this System.Windows.Controls.Primitives.MenuBase target,
            ValueProxy<System.Windows.Controls.ItemContainerTemplateSelector>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.Primitives.MenuBase.ItemContainerTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.MenuBase WithUsesItemContainerTemplate(
            this System.Windows.Controls.Primitives.MenuBase target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.MenuBase.UsesItemContainerTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Popup WithChild(
            this System.Windows.Controls.Primitives.Popup target, ValueProxy<System.Windows.UIElement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Popup.ChildProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Popup WithIsOpen(
            this System.Windows.Controls.Primitives.Popup target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Popup.IsOpenProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Popup WithPlacement(
            this System.Windows.Controls.Primitives.Popup target,
            ValueProxy<System.Windows.Controls.Primitives.PlacementMode>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Popup.PlacementProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Popup WithCustomPopupPlacementCallback(
            this System.Windows.Controls.Primitives.Popup target,
            ValueProxy<System.Windows.Controls.Primitives.CustomPopupPlacementCallback>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Popup.CustomPopupPlacementCallbackProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Popup WithStaysOpen(
            this System.Windows.Controls.Primitives.Popup target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Popup.StaysOpenProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Popup WithHorizontalOffset(
            this System.Windows.Controls.Primitives.Popup target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Popup.HorizontalOffsetProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Popup WithVerticalOffset(
            this System.Windows.Controls.Primitives.Popup target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Popup.VerticalOffsetProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Popup WithPlacementTarget(
            this System.Windows.Controls.Primitives.Popup target, ValueProxy<System.Windows.UIElement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Popup.PlacementTargetProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Popup WithPlacementRectangle(
            this System.Windows.Controls.Primitives.Popup target, ValueProxy<System.Windows.Rect>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Popup.PlacementRectangleProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Popup WithPopupAnimation(
            this System.Windows.Controls.Primitives.Popup target,
            ValueProxy<System.Windows.Controls.Primitives.PopupAnimation>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Popup.PopupAnimationProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Popup WithAllowsTransparency(
            this System.Windows.Controls.Primitives.Popup target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Popup.AllowsTransparencyProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.RangeBase WithMinimum(
            this System.Windows.Controls.Primitives.RangeBase target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.RangeBase.MinimumProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.RangeBase WithMaximum(
            this System.Windows.Controls.Primitives.RangeBase target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.RangeBase.MaximumProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.RangeBase WithValue(
            this System.Windows.Controls.Primitives.RangeBase target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.RangeBase.ValueProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.RangeBase WithLargeChange(
            this System.Windows.Controls.Primitives.RangeBase target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.RangeBase.LargeChangeProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.RangeBase WithSmallChange(
            this System.Windows.Controls.Primitives.RangeBase target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.RangeBase.SmallChangeProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.RepeatButton WithDelay(
            this System.Windows.Controls.Primitives.RepeatButton target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.RepeatButton.DelayProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.RepeatButton WithInterval(
            this System.Windows.Controls.Primitives.RepeatButton target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.RepeatButton.IntervalProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.ScrollBar WithOrientation(
            this System.Windows.Controls.Primitives.ScrollBar target,
            ValueProxy<System.Windows.Controls.Orientation>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.ScrollBar.OrientationProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.ScrollBar WithViewportSize(
            this System.Windows.Controls.Primitives.ScrollBar target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.ScrollBar.ViewportSizeProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithSelectiveScrollingGrid_SelectiveScrollingOrientation(
            this System.Windows.DependencyObject target,
            ValueProxy<System.Windows.Controls.SelectiveScrollingOrientation>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.Primitives.SelectiveScrollingGrid.SelectiveScrollingOrientationProperty);
            return target;
        }

        public static System.Windows.DependencyObject WithSelector_IsSelected(
            this System.Windows.DependencyObject target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Selector.IsSelectedProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Selector WithIsSynchronizedWithCurrentItem(
            this System.Windows.Controls.Primitives.Selector target,
            ValueProxy<System.Nullable<System.Boolean>>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.Primitives.Selector.IsSynchronizedWithCurrentItemProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Selector WithSelectedIndex(
            this System.Windows.Controls.Primitives.Selector target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Selector.SelectedIndexProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Selector WithSelectedItem(
            this System.Windows.Controls.Primitives.Selector target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Selector.SelectedItemProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Selector WithSelectedValue(
            this System.Windows.Controls.Primitives.Selector target, ValueProxy<System.Object>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Selector.SelectedValueProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Selector WithSelectedValuePath(
            this System.Windows.Controls.Primitives.Selector target, ValueProxy<System.String>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Selector.SelectedValuePathProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.StatusBar WithItemContainerTemplateSelector(
            this System.Windows.Controls.Primitives.StatusBar target,
            ValueProxy<System.Windows.Controls.ItemContainerTemplateSelector>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.Primitives.StatusBar.ItemContainerTemplateSelectorProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.StatusBar WithUsesItemContainerTemplate(
            this System.Windows.Controls.Primitives.StatusBar target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.StatusBar.UsesItemContainerTemplateProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithIsReadOnly(
            this System.Windows.Controls.Primitives.TextBoxBase target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.IsReadOnlyProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithIsReadOnlyCaretVisible(
            this System.Windows.Controls.Primitives.TextBoxBase target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.IsReadOnlyCaretVisibleProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithAcceptsReturn(
            this System.Windows.Controls.Primitives.TextBoxBase target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.AcceptsReturnProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithAcceptsTab(
            this System.Windows.Controls.Primitives.TextBoxBase target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.AcceptsTabProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithHorizontalScrollBarVisibility(
            this System.Windows.Controls.Primitives.TextBoxBase target,
            ValueProxy<System.Windows.Controls.ScrollBarVisibility>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.Primitives.TextBoxBase.HorizontalScrollBarVisibilityProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithVerticalScrollBarVisibility(
            this System.Windows.Controls.Primitives.TextBoxBase target,
            ValueProxy<System.Windows.Controls.ScrollBarVisibility>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.Primitives.TextBoxBase.VerticalScrollBarVisibilityProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithIsUndoEnabled(
            this System.Windows.Controls.Primitives.TextBoxBase target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.IsUndoEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithUndoLimit(
            this System.Windows.Controls.Primitives.TextBoxBase target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.UndoLimitProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithAutoWordSelection(
            this System.Windows.Controls.Primitives.TextBoxBase target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.AutoWordSelectionProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithSelectionBrush(
            this System.Windows.Controls.Primitives.TextBoxBase target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.SelectionBrushProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithSelectionTextBrush(
            this System.Windows.Controls.Primitives.TextBoxBase target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.SelectionTextBrushProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithSelectionOpacity(
            this System.Windows.Controls.Primitives.TextBoxBase target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.SelectionOpacityProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithCaretBrush(
            this System.Windows.Controls.Primitives.TextBoxBase target,
            ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.CaretBrushProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TextBoxBase WithIsInactiveSelectionHighlightEnabled(
            this System.Windows.Controls.Primitives.TextBoxBase target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target,
                System.Windows.Controls.Primitives.TextBoxBase.IsInactiveSelectionHighlightEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TickBar WithFill(
            this System.Windows.Controls.Primitives.TickBar target, ValueProxy<System.Windows.Media.Brush>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TickBar.FillProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TickBar WithMinimum(
            this System.Windows.Controls.Primitives.TickBar target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TickBar.MinimumProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TickBar WithMaximum(
            this System.Windows.Controls.Primitives.TickBar target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TickBar.MaximumProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TickBar WithSelectionStart(
            this System.Windows.Controls.Primitives.TickBar target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TickBar.SelectionStartProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TickBar WithSelectionEnd(
            this System.Windows.Controls.Primitives.TickBar target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TickBar.SelectionEndProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TickBar WithIsSelectionRangeEnabled(
            this System.Windows.Controls.Primitives.TickBar target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TickBar.IsSelectionRangeEnabledProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TickBar WithTickFrequency(
            this System.Windows.Controls.Primitives.TickBar target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TickBar.TickFrequencyProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TickBar WithTicks(
            this System.Windows.Controls.Primitives.TickBar target,
            ValueProxy<System.Windows.Media.DoubleCollection>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TickBar.TicksProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TickBar WithIsDirectionReversed(
            this System.Windows.Controls.Primitives.TickBar target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TickBar.IsDirectionReversedProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TickBar WithPlacement(
            this System.Windows.Controls.Primitives.TickBar target,
            ValueProxy<System.Windows.Controls.Primitives.TickBarPlacement>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TickBar.PlacementProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.TickBar WithReservedSpace(
            this System.Windows.Controls.Primitives.TickBar target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.TickBar.ReservedSpaceProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.ToggleButton WithIsChecked(
            this System.Windows.Controls.Primitives.ToggleButton target,
            ValueProxy<System.Nullable<System.Boolean>>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.ToggleButton WithIsThreeState(
            this System.Windows.Controls.Primitives.ToggleButton target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.ToggleButton.IsThreeStateProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.ToolBarOverflowPanel WithWrapWidth(
            this System.Windows.Controls.Primitives.ToolBarOverflowPanel target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.ToolBarOverflowPanel.WrapWidthProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Track WithOrientation(
            this System.Windows.Controls.Primitives.Track target,
            ValueProxy<System.Windows.Controls.Orientation>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Track.OrientationProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Track WithMinimum(
            this System.Windows.Controls.Primitives.Track target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Track.MinimumProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Track WithMaximum(
            this System.Windows.Controls.Primitives.Track target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Track.MaximumProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Track WithValue(
            this System.Windows.Controls.Primitives.Track target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Track.ValueProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Track WithViewportSize(
            this System.Windows.Controls.Primitives.Track target, ValueProxy<System.Double>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Track.ViewportSizeProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.Track WithIsDirectionReversed(
            this System.Windows.Controls.Primitives.Track target, ValueProxy<System.Boolean>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.Track.IsDirectionReversedProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.UniformGrid WithFirstColumn(
            this System.Windows.Controls.Primitives.UniformGrid target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.UniformGrid.FirstColumnProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.UniformGrid WithColumns(
            this System.Windows.Controls.Primitives.UniformGrid target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.UniformGrid.ColumnsProperty);
            return target;
        }

        public static System.Windows.Controls.Primitives.UniformGrid WithRows(
            this System.Windows.Controls.Primitives.UniformGrid target, ValueProxy<System.Int32>? propValue)
        {
            propValue?.SetValue(target, System.Windows.Controls.Primitives.UniformGrid.RowsProperty);
            return target;
        }
    }
}