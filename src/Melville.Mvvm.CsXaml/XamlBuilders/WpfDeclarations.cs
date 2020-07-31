﻿using Melville.Mvvm.CsXaml.ValueSource;
namespace Melville.Mvvm.CsXaml.XamlBuilders {
public static partial class WpfDeclarations {

//IsInDesignModeProperty
public static TChild WithDesignerProperties_IsInDesignMode<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.ComponentModel.DesignerProperties.IsInDesignModeProperty); return target;}

//StyleProperty
public static TChild WithStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.FrameworkContentElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkContentElement{value?.SetValue(target, System.Windows.FrameworkContentElement.StyleProperty); return target;}
public static TChild WithStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.StyleProperty); return target;}

//OverridesDefaultStyleProperty
public static TChild WithOverridesDefaultStyle<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.FrameworkContentElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkContentElement{value?.SetValue(target, System.Windows.FrameworkContentElement.OverridesDefaultStyleProperty); return target;}
public static TChild WithOverridesDefaultStyle<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.OverridesDefaultStyleProperty); return target;}

//NameProperty
public static TChild WithName<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.FrameworkContentElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkContentElement{value?.SetValue(target, System.Windows.FrameworkContentElement.NameProperty); return target;}
public static TChild WithName<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.NameProperty); return target;}
public static TChild WithJournalEntry_Name<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Navigation.JournalEntry.NameProperty); return target;}
public static TChild WithName<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Navigation.JournalEntry, TChild>? doNotUse = null) where TChild: System.Windows.Navigation.JournalEntry{value?.SetValue(target, System.Windows.Navigation.JournalEntry.NameProperty); return target;}
public static TChild WithAutomationProperties_Name<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.NameProperty); return target;}
public static TChild WithName<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Media.Animation.Timeline, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Timeline{value?.SetValue(target, System.Windows.Media.Animation.Timeline.NameProperty); return target;}

//TagProperty
public static TChild WithTag<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.FrameworkContentElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkContentElement{value?.SetValue(target, System.Windows.FrameworkContentElement.TagProperty); return target;}
public static TChild WithTag<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.TagProperty); return target;}

//LanguageProperty
public static TChild WithLanguage<TChild>(this TChild target, ValueProxy<System.Windows.Markup.XmlLanguage>? value, Disambigator<System.Windows.FrameworkContentElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkContentElement{value?.SetValue(target, System.Windows.FrameworkContentElement.LanguageProperty); return target;}
public static TChild WithLanguage<TChild>(this TChild target, ValueProxy<System.Windows.Markup.XmlLanguage>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.LanguageProperty); return target;}

//FocusVisualStyleProperty
public static TChild WithFocusVisualStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.FrameworkContentElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkContentElement{value?.SetValue(target, System.Windows.FrameworkContentElement.FocusVisualStyleProperty); return target;}
public static TChild WithFocusVisualStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.FocusVisualStyleProperty); return target;}

//CursorProperty
public static TChild WithCursor<TChild>(this TChild target, ValueProxy<System.Windows.Input.Cursor>? value, Disambigator<System.Windows.FrameworkContentElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkContentElement{value?.SetValue(target, System.Windows.FrameworkContentElement.CursorProperty); return target;}
public static TChild WithCursor<TChild>(this TChild target, ValueProxy<System.Windows.Input.Cursor>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.CursorProperty); return target;}

//ForceCursorProperty
public static TChild WithForceCursor<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.FrameworkContentElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkContentElement{value?.SetValue(target, System.Windows.FrameworkContentElement.ForceCursorProperty); return target;}
public static TChild WithForceCursor<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.ForceCursorProperty); return target;}

//InputScopeProperty
public static TChild WithInputScope<TChild>(this TChild target, ValueProxy<System.Windows.Input.InputScope>? value, Disambigator<System.Windows.FrameworkContentElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkContentElement{value?.SetValue(target, System.Windows.FrameworkContentElement.InputScopeProperty); return target;}
public static TChild WithInputScope<TChild>(this TChild target, ValueProxy<System.Windows.Input.InputScope>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.InputScopeProperty); return target;}
public static TChild WithInputMethod_InputScope<TChild>(this TChild target, ValueProxy<System.Windows.Input.InputScope>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.InputMethod.InputScopeProperty); return target;}

//DataContextProperty
public static TChild WithDataContext<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.FrameworkContentElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkContentElement{value?.SetValue(target, System.Windows.FrameworkContentElement.DataContextProperty); return target;}
public static TChild WithDataContext<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.DataContextProperty); return target;}

//BindingGroupProperty
public static TChild WithBindingGroup<TChild>(this TChild target, ValueProxy<System.Windows.Data.BindingGroup>? value, Disambigator<System.Windows.FrameworkContentElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkContentElement{value?.SetValue(target, System.Windows.FrameworkContentElement.BindingGroupProperty); return target;}
public static TChild WithBindingGroup<TChild>(this TChild target, ValueProxy<System.Windows.Data.BindingGroup>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.BindingGroupProperty); return target;}

//ToolTipProperty
public static TChild WithToolTip<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.FrameworkContentElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkContentElement{value?.SetValue(target, System.Windows.FrameworkContentElement.ToolTipProperty); return target;}
public static TChild WithToolTip<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.ToolTipProperty); return target;}
public static TChild WithToolTipService_ToolTip<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolTipService.ToolTipProperty); return target;}

//ContextMenuProperty
public static TChild WithContextMenu<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ContextMenu>? value, Disambigator<System.Windows.FrameworkContentElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkContentElement{value?.SetValue(target, System.Windows.FrameworkContentElement.ContextMenuProperty); return target;}
public static TChild WithContextMenu<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ContextMenu>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.ContextMenuProperty); return target;}
public static TChild WithContextMenuService_ContextMenu<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ContextMenu>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ContextMenuService.ContextMenuProperty); return target;}

//UseLayoutRoundingProperty
public static TChild WithUseLayoutRounding<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.UseLayoutRoundingProperty); return target;}

//ActualWidthProperty
public static TChild WithActualWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.ActualWidthProperty); return target;}
public static TChild WithActualWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.ActualWidthProperty); return target;}

//ActualHeightProperty
public static TChild WithActualHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.ActualHeightProperty); return target;}

//LayoutTransformProperty
public static TChild WithLayoutTransform<TChild>(this TChild target, ValueProxy<System.Windows.Media.Transform>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.LayoutTransformProperty); return target;}

//WidthProperty
public static TChild WithWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.WidthProperty); return target;}
public static TChild WithWidth<TChild>(this TChild target, ValueProxy<System.Windows.FigureLength>? value, Disambigator<System.Windows.Documents.Figure, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Figure{value?.SetValue(target, System.Windows.Documents.Figure.WidthProperty); return target;}
public static TChild WithWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.Floater, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Floater{value?.SetValue(target, System.Windows.Documents.Floater.WidthProperty); return target;}
public static TChild WithWidth<TChild>(this TChild target, ValueProxy<System.Windows.GridLength>? value, Disambigator<System.Windows.Documents.TableColumn, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TableColumn{value?.SetValue(target, System.Windows.Documents.TableColumn.WidthProperty); return target;}
public static TChild WithWidth<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataGridLength>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.WidthProperty); return target;}
public static TChild WithWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.GridViewColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewColumn{value?.SetValue(target, System.Windows.Controls.GridViewColumn.WidthProperty); return target;}
public static TChild WithWidth<TChild>(this TChild target, ValueProxy<System.Windows.GridLength>? value, Disambigator<System.Windows.Controls.ColumnDefinition, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ColumnDefinition{value?.SetValue(target, System.Windows.Controls.ColumnDefinition.WidthProperty); return target;}
public static System.Windows.Media.Media3D.OrthographicCamera WithWidth(this System.Windows.Media.Media3D.OrthographicCamera target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.OrthographicCamera.WidthProperty); return target;}

//MinWidthProperty
public static TChild WithMinWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.MinWidthProperty); return target;}
public static TChild WithMinWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.MinWidthProperty); return target;}
public static TChild WithMinWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ColumnDefinition, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ColumnDefinition{value?.SetValue(target, System.Windows.Controls.ColumnDefinition.MinWidthProperty); return target;}

//MaxWidthProperty
public static TChild WithMaxWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.MaxWidthProperty); return target;}
public static TChild WithMaxWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.MaxWidthProperty); return target;}
public static TChild WithMaxWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ColumnDefinition, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ColumnDefinition{value?.SetValue(target, System.Windows.Controls.ColumnDefinition.MaxWidthProperty); return target;}

//HeightProperty
public static TChild WithHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.HeightProperty); return target;}
public static TChild WithHeight<TChild>(this TChild target, ValueProxy<System.Windows.FigureLength>? value, Disambigator<System.Windows.Documents.Figure, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Figure{value?.SetValue(target, System.Windows.Documents.Figure.HeightProperty); return target;}
public static TChild WithHeight<TChild>(this TChild target, ValueProxy<System.Windows.GridLength>? value, Disambigator<System.Windows.Controls.RowDefinition, TChild>? doNotUse = null) where TChild: System.Windows.Controls.RowDefinition{value?.SetValue(target, System.Windows.Controls.RowDefinition.HeightProperty); return target;}

//MinHeightProperty
public static TChild WithMinHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.MinHeightProperty); return target;}
public static TChild WithMinHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.RowDefinition, TChild>? doNotUse = null) where TChild: System.Windows.Controls.RowDefinition{value?.SetValue(target, System.Windows.Controls.RowDefinition.MinHeightProperty); return target;}

//MaxHeightProperty
public static TChild WithMaxHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.MaxHeightProperty); return target;}
public static TChild WithMaxHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.RowDefinition, TChild>? doNotUse = null) where TChild: System.Windows.Controls.RowDefinition{value?.SetValue(target, System.Windows.Controls.RowDefinition.MaxHeightProperty); return target;}

//FlowDirectionProperty
public static TChild WithFrameworkElement_FlowDirection<TChild>(this TChild target, ValueProxy<System.Windows.FlowDirection>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.FrameworkElement.FlowDirectionProperty); return target;}
public static TChild WithFlowDirection<TChild>(this TChild target, ValueProxy<System.Windows.FlowDirection>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.FlowDirectionProperty); return target;}
public static TChild WithFlowDirection<TChild>(this TChild target, ValueProxy<System.Windows.FlowDirection>? value, Disambigator<System.Windows.Documents.Block, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Block{value?.SetValue(target, System.Windows.Documents.Block.FlowDirectionProperty); return target;}
public static TChild WithFlowDirection<TChild>(this TChild target, ValueProxy<System.Windows.FlowDirection>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.FlowDirectionProperty); return target;}
public static TChild WithFlowDirection<TChild>(this TChild target, ValueProxy<System.Windows.FlowDirection>? value, Disambigator<System.Windows.Documents.Inline, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Inline{value?.SetValue(target, System.Windows.Documents.Inline.FlowDirectionProperty); return target;}
public static TChild WithFlowDirection<TChild>(this TChild target, ValueProxy<System.Windows.FlowDirection>? value, Disambigator<System.Windows.Documents.ListItem, TChild>? doNotUse = null) where TChild: System.Windows.Documents.ListItem{value?.SetValue(target, System.Windows.Documents.ListItem.FlowDirectionProperty); return target;}
public static TChild WithFlowDirection<TChild>(this TChild target, ValueProxy<System.Windows.FlowDirection>? value, Disambigator<System.Windows.Documents.TableCell, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TableCell{value?.SetValue(target, System.Windows.Documents.TableCell.FlowDirectionProperty); return target;}

//MarginProperty
public static TChild WithMargin<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.MarginProperty); return target;}
public static TChild WithMargin<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Documents.AnchoredBlock, TChild>? doNotUse = null) where TChild: System.Windows.Documents.AnchoredBlock{value?.SetValue(target, System.Windows.Documents.AnchoredBlock.MarginProperty); return target;}
public static TChild WithMargin<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Documents.Block, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Block{value?.SetValue(target, System.Windows.Documents.Block.MarginProperty); return target;}
public static TChild WithMargin<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Documents.ListItem, TChild>? doNotUse = null) where TChild: System.Windows.Documents.ListItem{value?.SetValue(target, System.Windows.Documents.ListItem.MarginProperty); return target;}

//HorizontalAlignmentProperty
public static TChild WithHorizontalAlignment<TChild>(this TChild target, ValueProxy<System.Windows.HorizontalAlignment>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.HorizontalAlignmentProperty); return target;}
public static TChild WithHorizontalAlignment<TChild>(this TChild target, ValueProxy<System.Windows.HorizontalAlignment>? value, Disambigator<System.Windows.Documents.Floater, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Floater{value?.SetValue(target, System.Windows.Documents.Floater.HorizontalAlignmentProperty); return target;}

//VerticalAlignmentProperty
public static TChild WithVerticalAlignment<TChild>(this TChild target, ValueProxy<System.Windows.VerticalAlignment>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.FrameworkElement.VerticalAlignmentProperty); return target;}

//CommentsProperty
public static TChild WithLocalization_Comments<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Localization.CommentsProperty); return target;}

//AttributesProperty
public static TChild WithLocalization_Attributes<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Localization.AttributesProperty); return target;}

//CustomVisualStateManagerProperty
public static TChild WithVisualStateManager_CustomVisualStateManager<TChild>(this TChild target, ValueProxy<System.Windows.VisualStateManager>? value, Disambigator<System.Windows.FrameworkElement, TChild>? doNotUse = null) where TChild: System.Windows.FrameworkElement{value?.SetValue(target, System.Windows.VisualStateManager.CustomVisualStateManagerProperty); return target;}

//VisualStateGroupsProperty

//TaskbarItemInfoProperty
public static TChild WithTaskbarItemInfo<TChild>(this TChild target, ValueProxy<System.Windows.Shell.TaskbarItemInfo>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.TaskbarItemInfoProperty); return target;}

//AllowsTransparencyProperty
public static TChild WithAllowsTransparency<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.AllowsTransparencyProperty); return target;}
public static TChild WithAllowsTransparency<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.Popup, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Popup{value?.SetValue(target, System.Windows.Controls.Primitives.Popup.AllowsTransparencyProperty); return target;}

//TitleProperty
public static TChild WithTitle<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.TitleProperty); return target;}
public static TChild WithTitle<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.Page, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Page{value?.SetValue(target, System.Windows.Controls.Page.TitleProperty); return target;}

//IconProperty
public static TChild WithIcon<TChild>(this TChild target, ValueProxy<System.Windows.Media.ImageSource>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.IconProperty); return target;}
public static TChild WithIcon<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.IconProperty); return target;}

//SizeToContentProperty
public static TChild WithSizeToContent<TChild>(this TChild target, ValueProxy<System.Windows.SizeToContent>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.SizeToContentProperty); return target;}

//TopProperty
public static TChild WithTop<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.TopProperty); return target;}
public static TChild WithFixedPage_Top<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Documents.FixedPage.TopProperty); return target;}
public static TChild WithCanvas_Top<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.Canvas.TopProperty); return target;}
public static TChild WithInkCanvas_Top<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.InkCanvas.TopProperty); return target;}

//LeftProperty
public static TChild WithLeft<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.LeftProperty); return target;}
public static TChild WithFixedPage_Left<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Documents.FixedPage.LeftProperty); return target;}
public static TChild WithCanvas_Left<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.Canvas.LeftProperty); return target;}
public static TChild WithInkCanvas_Left<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.InkCanvas.LeftProperty); return target;}

//ShowInTaskbarProperty
public static TChild WithShowInTaskbar<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.ShowInTaskbarProperty); return target;}

//IsActiveProperty
public static TChild WithIsActive<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.IsActiveProperty); return target;}
public static System.Windows.Controls.StickyNoteControl WithIsActive(this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Controls.StickyNoteControl.IsActiveProperty); return target;}

//WindowStyleProperty
public static TChild WithWindowStyle<TChild>(this TChild target, ValueProxy<System.Windows.WindowStyle>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.WindowStyleProperty); return target;}

//WindowStateProperty
public static TChild WithWindowState<TChild>(this TChild target, ValueProxy<System.Windows.WindowState>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.WindowStateProperty); return target;}

//ResizeModeProperty
public static TChild WithResizeMode<TChild>(this TChild target, ValueProxy<System.Windows.ResizeMode>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.ResizeModeProperty); return target;}

//TopmostProperty
public static TChild WithTopmost<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.TopmostProperty); return target;}

//ShowActivatedProperty
public static TChild WithShowActivated<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Window.ShowActivatedProperty); return target;}

//ProgressStateProperty
public static System.Windows.Shell.TaskbarItemInfo WithProgressState(this System.Windows.Shell.TaskbarItemInfo target, ValueProxy<System.Windows.Shell.TaskbarItemProgressState>? value) {value?.SetValue(target, System.Windows.Shell.TaskbarItemInfo.ProgressStateProperty); return target;}

//ProgressValueProperty
public static System.Windows.Shell.TaskbarItemInfo WithProgressValue(this System.Windows.Shell.TaskbarItemInfo target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Shell.TaskbarItemInfo.ProgressValueProperty); return target;}

//OverlayProperty
public static System.Windows.Shell.TaskbarItemInfo WithOverlay(this System.Windows.Shell.TaskbarItemInfo target, ValueProxy<System.Windows.Media.ImageSource>? value) {value?.SetValue(target, System.Windows.Shell.TaskbarItemInfo.OverlayProperty); return target;}

//DescriptionProperty
public static System.Windows.Shell.TaskbarItemInfo WithDescription(this System.Windows.Shell.TaskbarItemInfo target, ValueProxy<System.String>? value) {value?.SetValue(target, System.Windows.Shell.TaskbarItemInfo.DescriptionProperty); return target;}
public static System.Windows.Shell.ThumbButtonInfo WithDescription(this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.String>? value) {value?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.DescriptionProperty); return target;}

//ThumbnailClipMarginProperty
public static System.Windows.Shell.TaskbarItemInfo WithThumbnailClipMargin(this System.Windows.Shell.TaskbarItemInfo target, ValueProxy<System.Windows.Thickness>? value) {value?.SetValue(target, System.Windows.Shell.TaskbarItemInfo.ThumbnailClipMarginProperty); return target;}

//ThumbButtonInfosProperty
public static System.Windows.Shell.TaskbarItemInfo WithThumbButtonInfos(this System.Windows.Shell.TaskbarItemInfo target, ValueProxy<System.Windows.Shell.ThumbButtonInfoCollection>? value) {value?.SetValue(target, System.Windows.Shell.TaskbarItemInfo.ThumbButtonInfosProperty); return target;}

//VisibilityProperty
public static System.Windows.Shell.ThumbButtonInfo WithVisibility(this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Windows.Visibility>? value) {value?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.VisibilityProperty); return target;}
public static TChild WithVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Visibility>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.VisibilityProperty); return target;}
public static TChild WithVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Visibility>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.VisibilityProperty); return target;}
public static TChild WithVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Visibility>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.VisibilityProperty); return target;}

//DismissWhenClickedProperty
public static System.Windows.Shell.ThumbButtonInfo WithDismissWhenClicked(this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.DismissWhenClickedProperty); return target;}

//ImageSourceProperty
public static System.Windows.Shell.ThumbButtonInfo WithImageSource(this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Windows.Media.ImageSource>? value) {value?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.ImageSourceProperty); return target;}
public static System.Windows.Media.ImageDrawing WithImageSource(this System.Windows.Media.ImageDrawing target, ValueProxy<System.Windows.Media.ImageSource>? value) {value?.SetValue(target, System.Windows.Media.ImageDrawing.ImageSourceProperty); return target;}
public static System.Windows.Media.ImageBrush WithImageSource(this System.Windows.Media.ImageBrush target, ValueProxy<System.Windows.Media.ImageSource>? value) {value?.SetValue(target, System.Windows.Media.ImageBrush.ImageSourceProperty); return target;}

//IsBackgroundVisibleProperty
public static System.Windows.Shell.ThumbButtonInfo WithIsBackgroundVisible(this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.IsBackgroundVisibleProperty); return target;}

//IsEnabledProperty
public static System.Windows.Shell.ThumbButtonInfo WithIsEnabled(this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.IsEnabledProperty); return target;}
public static TChild WithContextMenuService_IsEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ContextMenuService.IsEnabledProperty); return target;}
public static TChild WithSpellCheck_IsEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.SpellCheck.IsEnabledProperty); return target;}
public static TChild WithToolTipService_IsEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolTipService.IsEnabledProperty); return target;}
public static TChild WithIsEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.IsEnabledProperty); return target;}
public static TChild WithIsEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsEnabledProperty); return target;}
public static TChild WithIsEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsEnabledProperty); return target;}

//IsInteractiveProperty
public static System.Windows.Shell.ThumbButtonInfo WithIsInteractive(this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.IsInteractiveProperty); return target;}

//CommandProperty
public static System.Windows.Shell.ThumbButtonInfo WithCommand(this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Windows.Input.ICommand>? value) {value?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.CommandProperty); return target;}
public static TChild WithCommand<TChild>(this TChild target, ValueProxy<System.Windows.Input.ICommand>? value, Disambigator<System.Windows.Documents.Hyperlink, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Hyperlink{value?.SetValue(target, System.Windows.Documents.Hyperlink.CommandProperty); return target;}
public static TChild WithCommand<TChild>(this TChild target, ValueProxy<System.Windows.Input.ICommand>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.CommandProperty); return target;}
public static TChild WithCommand<TChild>(this TChild target, ValueProxy<System.Windows.Input.ICommand>? value, Disambigator<System.Windows.Controls.Primitives.ButtonBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.ButtonBase{value?.SetValue(target, System.Windows.Controls.Primitives.ButtonBase.CommandProperty); return target;}
public static TChild WithCommand<TChild>(this TChild target, ValueProxy<System.Windows.Input.ICommand>? value, Disambigator<System.Windows.Input.InputBinding, TChild>? doNotUse = null) where TChild: System.Windows.Input.InputBinding{value?.SetValue(target, System.Windows.Input.InputBinding.CommandProperty); return target;}

//CommandParameterProperty
public static System.Windows.Shell.ThumbButtonInfo WithCommandParameter(this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Object>? value) {value?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.CommandParameterProperty); return target;}
public static TChild WithCommandParameter<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Documents.Hyperlink, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Hyperlink{value?.SetValue(target, System.Windows.Documents.Hyperlink.CommandParameterProperty); return target;}
public static TChild WithCommandParameter<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.CommandParameterProperty); return target;}
public static TChild WithCommandParameter<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.Primitives.ButtonBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.ButtonBase{value?.SetValue(target, System.Windows.Controls.Primitives.ButtonBase.CommandParameterProperty); return target;}
public static TChild WithCommandParameter<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Input.InputBinding, TChild>? doNotUse = null) where TChild: System.Windows.Input.InputBinding{value?.SetValue(target, System.Windows.Input.InputBinding.CommandParameterProperty); return target;}

//CommandTargetProperty
public static System.Windows.Shell.ThumbButtonInfo WithCommandTarget(this System.Windows.Shell.ThumbButtonInfo target, ValueProxy<System.Windows.IInputElement>? value) {value?.SetValue(target, System.Windows.Shell.ThumbButtonInfo.CommandTargetProperty); return target;}
public static TChild WithCommandTarget<TChild>(this TChild target, ValueProxy<System.Windows.IInputElement>? value, Disambigator<System.Windows.Documents.Hyperlink, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Hyperlink{value?.SetValue(target, System.Windows.Documents.Hyperlink.CommandTargetProperty); return target;}
public static TChild WithCommandTarget<TChild>(this TChild target, ValueProxy<System.Windows.IInputElement>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.CommandTargetProperty); return target;}
public static TChild WithCommandTarget<TChild>(this TChild target, ValueProxy<System.Windows.IInputElement>? value, Disambigator<System.Windows.Controls.Primitives.ButtonBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.ButtonBase{value?.SetValue(target, System.Windows.Controls.Primitives.ButtonBase.CommandTargetProperty); return target;}
public static TChild WithCommandTarget<TChild>(this TChild target, ValueProxy<System.Windows.IInputElement>? value, Disambigator<System.Windows.Input.InputBinding, TChild>? doNotUse = null) where TChild: System.Windows.Input.InputBinding{value?.SetValue(target, System.Windows.Input.InputBinding.CommandTargetProperty); return target;}

//WindowChromeProperty
public static TChild WithWindowChrome_WindowChrome<TChild>(this TChild target, ValueProxy<System.Windows.Shell.WindowChrome>? value, Disambigator<System.Windows.Window, TChild>? doNotUse = null) where TChild: System.Windows.Window{value?.SetValue(target, System.Windows.Shell.WindowChrome.WindowChromeProperty); return target;}

//IsHitTestVisibleInChromeProperty
public static TChild WithWindowChrome_IsHitTestVisibleInChrome<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Shell.WindowChrome.IsHitTestVisibleInChromeProperty); return target;}

//ResizeGripDirectionProperty
public static TChild WithWindowChrome_ResizeGripDirection<TChild>(this TChild target, ValueProxy<System.Windows.Shell.ResizeGripDirection>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Shell.WindowChrome.ResizeGripDirectionProperty); return target;}

//CaptionHeightProperty
public static TChild WithCaptionHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Shell.WindowChrome, TChild>? doNotUse = null) where TChild: System.Windows.Shell.WindowChrome{value?.SetValue(target, System.Windows.Shell.WindowChrome.CaptionHeightProperty); return target;}

//ResizeBorderThicknessProperty
public static TChild WithResizeBorderThickness<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Shell.WindowChrome, TChild>? doNotUse = null) where TChild: System.Windows.Shell.WindowChrome{value?.SetValue(target, System.Windows.Shell.WindowChrome.ResizeBorderThicknessProperty); return target;}

//GlassFrameThicknessProperty
public static TChild WithGlassFrameThickness<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Shell.WindowChrome, TChild>? doNotUse = null) where TChild: System.Windows.Shell.WindowChrome{value?.SetValue(target, System.Windows.Shell.WindowChrome.GlassFrameThicknessProperty); return target;}

//UseAeroCaptionButtonsProperty
public static TChild WithUseAeroCaptionButtons<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Shell.WindowChrome, TChild>? doNotUse = null) where TChild: System.Windows.Shell.WindowChrome{value?.SetValue(target, System.Windows.Shell.WindowChrome.UseAeroCaptionButtonsProperty); return target;}

//CornerRadiusProperty
public static TChild WithCornerRadius<TChild>(this TChild target, ValueProxy<System.Windows.CornerRadius>? value, Disambigator<System.Windows.Shell.WindowChrome, TChild>? doNotUse = null) where TChild: System.Windows.Shell.WindowChrome{value?.SetValue(target, System.Windows.Shell.WindowChrome.CornerRadiusProperty); return target;}
public static TChild WithCornerRadius<TChild>(this TChild target, ValueProxy<System.Windows.CornerRadius>? value, Disambigator<System.Windows.Controls.Border, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Border{value?.SetValue(target, System.Windows.Controls.Border.CornerRadiusProperty); return target;}

//NonClientFrameEdgesProperty
public static TChild WithNonClientFrameEdges<TChild>(this TChild target, ValueProxy<System.Windows.Shell.NonClientFrameEdges>? value, Disambigator<System.Windows.Shell.WindowChrome, TChild>? doNotUse = null) where TChild: System.Windows.Shell.WindowChrome{value?.SetValue(target, System.Windows.Shell.WindowChrome.NonClientFrameEdgesProperty); return target;}

//X1Property
public static System.Windows.Shapes.Line WithX1(this System.Windows.Shapes.Line target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Shapes.Line.X1Property); return target;}

//Y1Property
public static System.Windows.Shapes.Line WithY1(this System.Windows.Shapes.Line target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Shapes.Line.Y1Property); return target;}

//X2Property
public static System.Windows.Shapes.Line WithX2(this System.Windows.Shapes.Line target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Shapes.Line.X2Property); return target;}

//Y2Property
public static System.Windows.Shapes.Line WithY2(this System.Windows.Shapes.Line target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Shapes.Line.Y2Property); return target;}

//DataProperty
public static System.Windows.Shapes.Path WithData(this System.Windows.Shapes.Path target, ValueProxy<System.Windows.Media.Geometry>? value) {value?.SetValue(target, System.Windows.Shapes.Path.DataProperty); return target;}

//PointsProperty
public static System.Windows.Shapes.Polygon WithPoints(this System.Windows.Shapes.Polygon target, ValueProxy<System.Windows.Media.PointCollection>? value) {value?.SetValue(target, System.Windows.Shapes.Polygon.PointsProperty); return target;}
public static System.Windows.Shapes.Polyline WithPoints(this System.Windows.Shapes.Polyline target, ValueProxy<System.Windows.Media.PointCollection>? value) {value?.SetValue(target, System.Windows.Shapes.Polyline.PointsProperty); return target;}
public static System.Windows.Media.PolyLineSegment WithPoints(this System.Windows.Media.PolyLineSegment target, ValueProxy<System.Windows.Media.PointCollection>? value) {value?.SetValue(target, System.Windows.Media.PolyLineSegment.PointsProperty); return target;}
public static System.Windows.Media.PolyBezierSegment WithPoints(this System.Windows.Media.PolyBezierSegment target, ValueProxy<System.Windows.Media.PointCollection>? value) {value?.SetValue(target, System.Windows.Media.PolyBezierSegment.PointsProperty); return target;}
public static System.Windows.Media.PolyQuadraticBezierSegment WithPoints(this System.Windows.Media.PolyQuadraticBezierSegment target, ValueProxy<System.Windows.Media.PointCollection>? value) {value?.SetValue(target, System.Windows.Media.PolyQuadraticBezierSegment.PointsProperty); return target;}

//FillRuleProperty
public static System.Windows.Shapes.Polygon WithFillRule(this System.Windows.Shapes.Polygon target, ValueProxy<System.Windows.Media.FillRule>? value) {value?.SetValue(target, System.Windows.Shapes.Polygon.FillRuleProperty); return target;}
public static System.Windows.Shapes.Polyline WithFillRule(this System.Windows.Shapes.Polyline target, ValueProxy<System.Windows.Media.FillRule>? value) {value?.SetValue(target, System.Windows.Shapes.Polyline.FillRuleProperty); return target;}
public static System.Windows.Media.PathGeometry WithFillRule(this System.Windows.Media.PathGeometry target, ValueProxy<System.Windows.Media.FillRule>? value) {value?.SetValue(target, System.Windows.Media.PathGeometry.FillRuleProperty); return target;}
public static System.Windows.Media.StreamGeometry WithFillRule(this System.Windows.Media.StreamGeometry target, ValueProxy<System.Windows.Media.FillRule>? value) {value?.SetValue(target, System.Windows.Media.StreamGeometry.FillRuleProperty); return target;}
public static System.Windows.Media.GeometryGroup WithFillRule(this System.Windows.Media.GeometryGroup target, ValueProxy<System.Windows.Media.FillRule>? value) {value?.SetValue(target, System.Windows.Media.GeometryGroup.FillRuleProperty); return target;}

//RadiusXProperty
public static System.Windows.Shapes.Rectangle WithRadiusX(this System.Windows.Shapes.Rectangle target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Shapes.Rectangle.RadiusXProperty); return target;}
public static System.Windows.Media.RectangleGeometry WithRadiusX(this System.Windows.Media.RectangleGeometry target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.RectangleGeometry.RadiusXProperty); return target;}
public static System.Windows.Media.EllipseGeometry WithRadiusX(this System.Windows.Media.EllipseGeometry target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.EllipseGeometry.RadiusXProperty); return target;}
public static System.Windows.Media.RadialGradientBrush WithRadiusX(this System.Windows.Media.RadialGradientBrush target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.RadialGradientBrush.RadiusXProperty); return target;}

//RadiusYProperty
public static System.Windows.Shapes.Rectangle WithRadiusY(this System.Windows.Shapes.Rectangle target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Shapes.Rectangle.RadiusYProperty); return target;}
public static System.Windows.Media.RectangleGeometry WithRadiusY(this System.Windows.Media.RectangleGeometry target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.RectangleGeometry.RadiusYProperty); return target;}
public static System.Windows.Media.EllipseGeometry WithRadiusY(this System.Windows.Media.EllipseGeometry target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.EllipseGeometry.RadiusYProperty); return target;}
public static System.Windows.Media.RadialGradientBrush WithRadiusY(this System.Windows.Media.RadialGradientBrush target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.RadialGradientBrush.RadiusYProperty); return target;}

//StretchProperty
public static TChild WithStretch<TChild>(this TChild target, ValueProxy<System.Windows.Media.Stretch>? value, Disambigator<System.Windows.Shapes.Shape, TChild>? doNotUse = null) where TChild: System.Windows.Shapes.Shape{value?.SetValue(target, System.Windows.Shapes.Shape.StretchProperty); return target;}
public static TChild WithStretch<TChild>(this TChild target, ValueProxy<System.Windows.Media.Stretch>? value, Disambigator<System.Windows.Controls.Image, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Image{value?.SetValue(target, System.Windows.Controls.Image.StretchProperty); return target;}
public static TChild WithStretch<TChild>(this TChild target, ValueProxy<System.Windows.Media.Stretch>? value, Disambigator<System.Windows.Controls.MediaElement, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MediaElement{value?.SetValue(target, System.Windows.Controls.MediaElement.StretchProperty); return target;}
public static TChild WithStretch<TChild>(this TChild target, ValueProxy<System.Windows.Media.Stretch>? value, Disambigator<System.Windows.Controls.Viewbox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Viewbox{value?.SetValue(target, System.Windows.Controls.Viewbox.StretchProperty); return target;}
public static TChild WithStretch<TChild>(this TChild target, ValueProxy<System.Windows.Media.Stretch>? value, Disambigator<System.Windows.Controls.Primitives.DocumentPageView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DocumentPageView{value?.SetValue(target, System.Windows.Controls.Primitives.DocumentPageView.StretchProperty); return target;}
public static TChild WithStretch<TChild>(this TChild target, ValueProxy<System.Windows.Media.Stretch>? value, Disambigator<System.Windows.Media.TileBrush, TChild>? doNotUse = null) where TChild: System.Windows.Media.TileBrush{value?.SetValue(target, System.Windows.Media.TileBrush.StretchProperty); return target;}

//FillProperty
public static TChild WithFill<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Shapes.Shape, TChild>? doNotUse = null) where TChild: System.Windows.Shapes.Shape{value?.SetValue(target, System.Windows.Shapes.Shape.FillProperty); return target;}
public static System.Windows.Documents.Glyphs WithFill(this System.Windows.Documents.Glyphs target, ValueProxy<System.Windows.Media.Brush>? value) {value?.SetValue(target, System.Windows.Documents.Glyphs.FillProperty); return target;}
public static TChild WithFill<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Primitives.TickBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TickBar{value?.SetValue(target, System.Windows.Controls.Primitives.TickBar.FillProperty); return target;}

//StrokeProperty
public static TChild WithStroke<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Shapes.Shape, TChild>? doNotUse = null) where TChild: System.Windows.Shapes.Shape{value?.SetValue(target, System.Windows.Shapes.Shape.StrokeProperty); return target;}

//StrokeThicknessProperty
public static TChild WithStrokeThickness<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Shapes.Shape, TChild>? doNotUse = null) where TChild: System.Windows.Shapes.Shape{value?.SetValue(target, System.Windows.Shapes.Shape.StrokeThicknessProperty); return target;}

//StrokeStartLineCapProperty
public static TChild WithStrokeStartLineCap<TChild>(this TChild target, ValueProxy<System.Windows.Media.PenLineCap>? value, Disambigator<System.Windows.Shapes.Shape, TChild>? doNotUse = null) where TChild: System.Windows.Shapes.Shape{value?.SetValue(target, System.Windows.Shapes.Shape.StrokeStartLineCapProperty); return target;}

//StrokeEndLineCapProperty
public static TChild WithStrokeEndLineCap<TChild>(this TChild target, ValueProxy<System.Windows.Media.PenLineCap>? value, Disambigator<System.Windows.Shapes.Shape, TChild>? doNotUse = null) where TChild: System.Windows.Shapes.Shape{value?.SetValue(target, System.Windows.Shapes.Shape.StrokeEndLineCapProperty); return target;}

//StrokeDashCapProperty
public static TChild WithStrokeDashCap<TChild>(this TChild target, ValueProxy<System.Windows.Media.PenLineCap>? value, Disambigator<System.Windows.Shapes.Shape, TChild>? doNotUse = null) where TChild: System.Windows.Shapes.Shape{value?.SetValue(target, System.Windows.Shapes.Shape.StrokeDashCapProperty); return target;}

//StrokeLineJoinProperty
public static TChild WithStrokeLineJoin<TChild>(this TChild target, ValueProxy<System.Windows.Media.PenLineJoin>? value, Disambigator<System.Windows.Shapes.Shape, TChild>? doNotUse = null) where TChild: System.Windows.Shapes.Shape{value?.SetValue(target, System.Windows.Shapes.Shape.StrokeLineJoinProperty); return target;}

//StrokeMiterLimitProperty
public static TChild WithStrokeMiterLimit<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Shapes.Shape, TChild>? doNotUse = null) where TChild: System.Windows.Shapes.Shape{value?.SetValue(target, System.Windows.Shapes.Shape.StrokeMiterLimitProperty); return target;}

//StrokeDashOffsetProperty
public static TChild WithStrokeDashOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Shapes.Shape, TChild>? doNotUse = null) where TChild: System.Windows.Shapes.Shape{value?.SetValue(target, System.Windows.Shapes.Shape.StrokeDashOffsetProperty); return target;}

//StrokeDashArrayProperty
public static TChild WithStrokeDashArray<TChild>(this TChild target, ValueProxy<System.Windows.Media.DoubleCollection>? value, Disambigator<System.Windows.Shapes.Shape, TChild>? doNotUse = null) where TChild: System.Windows.Shapes.Shape{value?.SetValue(target, System.Windows.Shapes.Shape.StrokeDashArrayProperty); return target;}

//TabIndexProperty
public static TChild WithKeyboardNavigation_TabIndex<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.KeyboardNavigation.TabIndexProperty); return target;}
public static TChild WithTabIndex<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.TabIndexProperty); return target;}

//IsTabStopProperty
public static TChild WithKeyboardNavigation_IsTabStop<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.KeyboardNavigation.IsTabStopProperty); return target;}
public static TChild WithIsTabStop<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.IsTabStopProperty); return target;}

//TabNavigationProperty
public static TChild WithKeyboardNavigation_TabNavigation<TChild>(this TChild target, ValueProxy<System.Windows.Input.KeyboardNavigationMode>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.KeyboardNavigation.TabNavigationProperty); return target;}

//ControlTabNavigationProperty
public static TChild WithKeyboardNavigation_ControlTabNavigation<TChild>(this TChild target, ValueProxy<System.Windows.Input.KeyboardNavigationMode>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.KeyboardNavigation.ControlTabNavigationProperty); return target;}

//DirectionalNavigationProperty
public static TChild WithKeyboardNavigation_DirectionalNavigation<TChild>(this TChild target, ValueProxy<System.Windows.Input.KeyboardNavigationMode>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.KeyboardNavigation.DirectionalNavigationProperty); return target;}

//AcceptsReturnProperty
public static TChild WithKeyboardNavigation_AcceptsReturn<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.KeyboardNavigation.AcceptsReturnProperty); return target;}
public static TChild WithAcceptsReturn<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.AcceptsReturnProperty); return target;}

//TextFormattingModeProperty
public static TChild WithTextOptions_TextFormattingMode<TChild>(this TChild target, ValueProxy<System.Windows.Media.TextFormattingMode>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.TextOptions.TextFormattingModeProperty); return target;}

//TextRenderingModeProperty
public static TChild WithTextOptions_TextRenderingMode<TChild>(this TChild target, ValueProxy<System.Windows.Media.TextRenderingMode>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.TextOptions.TextRenderingModeProperty); return target;}

//TextHintingModeProperty
public static TChild WithTextOptions_TextHintingMode<TChild>(this TChild target, ValueProxy<System.Windows.Media.TextHintingMode>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.TextOptions.TextHintingModeProperty); return target;}

//StoryboardProperty
public static System.Windows.Media.Animation.BeginStoryboard WithStoryboard(this System.Windows.Media.Animation.BeginStoryboard target, ValueProxy<System.Windows.Media.Animation.Storyboard>? value) {value?.SetValue(target, System.Windows.Media.Animation.BeginStoryboard.StoryboardProperty); return target;}

//EasingFunctionProperty
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingThicknessKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingThicknessKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingThicknessKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.ThicknessAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ThicknessAnimation{value?.SetValue(target, System.Windows.Media.Animation.ThicknessAnimation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingQuaternionKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingQuaternionKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingQuaternionKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.QuaternionAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.QuaternionAnimation{value?.SetValue(target, System.Windows.Media.Animation.QuaternionAnimation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.ByteAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ByteAnimation{value?.SetValue(target, System.Windows.Media.Animation.ByteAnimation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.ColorAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ColorAnimation{value?.SetValue(target, System.Windows.Media.Animation.ColorAnimation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.DecimalAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DecimalAnimation{value?.SetValue(target, System.Windows.Media.Animation.DecimalAnimation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.DoubleAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DoubleAnimation{value?.SetValue(target, System.Windows.Media.Animation.DoubleAnimation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingByteKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingByteKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingByteKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingColorKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingColorKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingColorKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingDecimalKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingDecimalKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingDecimalKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingDoubleKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingDoubleKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingDoubleKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingInt16KeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingInt16KeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingInt16KeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingInt32KeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingInt32KeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingInt32KeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingInt64KeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingInt64KeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingInt64KeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingPointKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingPointKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingPointKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingPoint3DKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingPoint3DKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingPoint3DKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingRotation3DKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingRotation3DKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingRotation3DKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingRectKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingRectKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingRectKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingSingleKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingSingleKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingSingleKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingSizeKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingSizeKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingSizeKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingVectorKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingVectorKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingVectorKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.EasingVector3DKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingVector3DKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingVector3DKeyFrame.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.Int16Animation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int16Animation{value?.SetValue(target, System.Windows.Media.Animation.Int16Animation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.Int32Animation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int32Animation{value?.SetValue(target, System.Windows.Media.Animation.Int32Animation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.Int64Animation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int64Animation{value?.SetValue(target, System.Windows.Media.Animation.Int64Animation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.Point3DAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Point3DAnimation{value?.SetValue(target, System.Windows.Media.Animation.Point3DAnimation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.PointAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.PointAnimation{value?.SetValue(target, System.Windows.Media.Animation.PointAnimation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.RectAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.RectAnimation{value?.SetValue(target, System.Windows.Media.Animation.RectAnimation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.Rotation3DAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Rotation3DAnimation{value?.SetValue(target, System.Windows.Media.Animation.Rotation3DAnimation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.SingleAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SingleAnimation{value?.SetValue(target, System.Windows.Media.Animation.SingleAnimation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.SizeAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SizeAnimation{value?.SetValue(target, System.Windows.Media.Animation.SizeAnimation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.Vector3DAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Vector3DAnimation{value?.SetValue(target, System.Windows.Media.Animation.Vector3DAnimation.EasingFunctionProperty); return target;}
public static TChild WithEasingFunction<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.IEasingFunction>? value, Disambigator<System.Windows.Media.Animation.VectorAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.VectorAnimation{value?.SetValue(target, System.Windows.Media.Animation.VectorAnimation.EasingFunctionProperty); return target;}

//KeyTimeProperty
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.ThicknessKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ThicknessKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.ThicknessKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.QuaternionKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.QuaternionKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.QuaternionKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.BooleanKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.BooleanKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.BooleanKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.ByteKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ByteKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.ByteKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.CharKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.CharKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.CharKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.ColorKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ColorKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.ColorKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.DecimalKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DecimalKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.DecimalKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.DoubleKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DoubleKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.DoubleKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.Int16KeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int16KeyFrame{value?.SetValue(target, System.Windows.Media.Animation.Int16KeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.Int32KeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int32KeyFrame{value?.SetValue(target, System.Windows.Media.Animation.Int32KeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.Int64KeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int64KeyFrame{value?.SetValue(target, System.Windows.Media.Animation.Int64KeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.MatrixKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.MatrixKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.MatrixKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.ObjectKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ObjectKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.ObjectKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.PointKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.PointKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.PointKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.Point3DKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Point3DKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.Point3DKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.Rotation3DKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Rotation3DKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.Rotation3DKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.RectKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.RectKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.RectKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.SingleKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SingleKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SingleKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.SizeKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SizeKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SizeKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.StringKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.StringKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.StringKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.VectorKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.VectorKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.VectorKeyFrame.KeyTimeProperty); return target;}
public static TChild WithKeyTime<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeyTime>? value, Disambigator<System.Windows.Media.Animation.Vector3DKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Vector3DKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.Vector3DKeyFrame.KeyTimeProperty); return target;}

//ValueProperty
public static TChild WithValue<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Media.Animation.ThicknessKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ThicknessKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.ThicknessKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.RangeBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.RangeBase{value?.SetValue(target, System.Windows.Controls.Primitives.RangeBase.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.Track, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Track{value?.SetValue(target, System.Windows.Controls.Primitives.Track.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Quaternion>? value, Disambigator<System.Windows.Media.Animation.QuaternionKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.QuaternionKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.QuaternionKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Media.Animation.BooleanKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.BooleanKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.BooleanKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Byte>? value, Disambigator<System.Windows.Media.Animation.ByteKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ByteKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.ByteKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Char>? value, Disambigator<System.Windows.Media.Animation.CharKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.CharKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.CharKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Windows.Media.Color>? value, Disambigator<System.Windows.Media.Animation.ColorKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ColorKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.ColorKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Decimal>? value, Disambigator<System.Windows.Media.Animation.DecimalKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DecimalKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.DecimalKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Animation.DoubleKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DoubleKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.DoubleKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Int16>? value, Disambigator<System.Windows.Media.Animation.Int16KeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int16KeyFrame{value?.SetValue(target, System.Windows.Media.Animation.Int16KeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Media.Animation.Int32KeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int32KeyFrame{value?.SetValue(target, System.Windows.Media.Animation.Int32KeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Int64>? value, Disambigator<System.Windows.Media.Animation.Int64KeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int64KeyFrame{value?.SetValue(target, System.Windows.Media.Animation.Int64KeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Windows.Media.Matrix>? value, Disambigator<System.Windows.Media.Animation.MatrixKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.MatrixKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.MatrixKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Media.Animation.ObjectKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ObjectKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.ObjectKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Windows.Point>? value, Disambigator<System.Windows.Media.Animation.PointKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.PointKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.PointKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Point3D>? value, Disambigator<System.Windows.Media.Animation.Point3DKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Point3DKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.Point3DKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Rotation3D>? value, Disambigator<System.Windows.Media.Animation.Rotation3DKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Rotation3DKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.Rotation3DKeyFrame.ValueProperty); return target;}

public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Windows.Rect>? value, Disambigator<System.Windows.Media.Animation.RectKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.RectKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.RectKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Single>? value, Disambigator<System.Windows.Media.Animation.SingleKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SingleKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SingleKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Windows.Size>? value, Disambigator<System.Windows.Media.Animation.SizeKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SizeKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SizeKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Media.Animation.StringKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.StringKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.StringKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Windows.Vector>? value, Disambigator<System.Windows.Media.Animation.VectorKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.VectorKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.VectorKeyFrame.ValueProperty); return target;}
public static TChild WithValue<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Vector3D>? value, Disambigator<System.Windows.Media.Animation.Vector3DKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Vector3DKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.Vector3DKeyFrame.ValueProperty); return target;}

//KeySplineProperty
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineThicknessKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineThicknessKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineThicknessKeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineQuaternionKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineQuaternionKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineQuaternionKeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineByteKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineByteKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineByteKeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineColorKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineColorKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineColorKeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineDecimalKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineDecimalKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineDecimalKeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineDoubleKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineDoubleKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineDoubleKeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineInt16KeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineInt16KeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineInt16KeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineInt32KeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineInt32KeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineInt32KeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineInt64KeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineInt64KeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineInt64KeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplinePointKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplinePointKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplinePointKeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplinePoint3DKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplinePoint3DKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplinePoint3DKeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineRotation3DKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineRotation3DKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineRotation3DKeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineRectKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineRectKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineRectKeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineSingleKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineSingleKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineSingleKeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineSizeKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineSizeKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineSizeKeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineVectorKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineVectorKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineVectorKeyFrame.KeySplineProperty); return target;}
public static TChild WithKeySpline<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.KeySpline>? value, Disambigator<System.Windows.Media.Animation.SplineVector3DKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineVector3DKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineVector3DKeyFrame.KeySplineProperty); return target;}

//FromProperty
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Thickness>>? value, Disambigator<System.Windows.Media.Animation.ThicknessAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ThicknessAnimation{value?.SetValue(target, System.Windows.Media.Animation.ThicknessAnimation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Media.Media3D.Quaternion>>? value, Disambigator<System.Windows.Media.Animation.QuaternionAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.QuaternionAnimation{value?.SetValue(target, System.Windows.Media.Animation.QuaternionAnimation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Byte>>? value, Disambigator<System.Windows.Media.Animation.ByteAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ByteAnimation{value?.SetValue(target, System.Windows.Media.Animation.ByteAnimation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Media.Color>>? value, Disambigator<System.Windows.Media.Animation.ColorAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ColorAnimation{value?.SetValue(target, System.Windows.Media.Animation.ColorAnimation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Decimal>>? value, Disambigator<System.Windows.Media.Animation.DecimalAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DecimalAnimation{value?.SetValue(target, System.Windows.Media.Animation.DecimalAnimation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Double>>? value, Disambigator<System.Windows.Media.Animation.DoubleAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DoubleAnimation{value?.SetValue(target, System.Windows.Media.Animation.DoubleAnimation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Int16>>? value, Disambigator<System.Windows.Media.Animation.Int16Animation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int16Animation{value?.SetValue(target, System.Windows.Media.Animation.Int16Animation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Int32>>? value, Disambigator<System.Windows.Media.Animation.Int32Animation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int32Animation{value?.SetValue(target, System.Windows.Media.Animation.Int32Animation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Int64>>? value, Disambigator<System.Windows.Media.Animation.Int64Animation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int64Animation{value?.SetValue(target, System.Windows.Media.Animation.Int64Animation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Media.Media3D.Point3D>>? value, Disambigator<System.Windows.Media.Animation.Point3DAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Point3DAnimation{value?.SetValue(target, System.Windows.Media.Animation.Point3DAnimation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Point>>? value, Disambigator<System.Windows.Media.Animation.PointAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.PointAnimation{value?.SetValue(target, System.Windows.Media.Animation.PointAnimation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Rect>>? value, Disambigator<System.Windows.Media.Animation.RectAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.RectAnimation{value?.SetValue(target, System.Windows.Media.Animation.RectAnimation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Rotation3D>? value, Disambigator<System.Windows.Media.Animation.Rotation3DAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Rotation3DAnimation{value?.SetValue(target, System.Windows.Media.Animation.Rotation3DAnimation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Single>>? value, Disambigator<System.Windows.Media.Animation.SingleAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SingleAnimation{value?.SetValue(target, System.Windows.Media.Animation.SingleAnimation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Size>>? value, Disambigator<System.Windows.Media.Animation.SizeAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SizeAnimation{value?.SetValue(target, System.Windows.Media.Animation.SizeAnimation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Media.Media3D.Vector3D>>? value, Disambigator<System.Windows.Media.Animation.Vector3DAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Vector3DAnimation{value?.SetValue(target, System.Windows.Media.Animation.Vector3DAnimation.FromProperty); return target;}
public static TChild WithFrom<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Vector>>? value, Disambigator<System.Windows.Media.Animation.VectorAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.VectorAnimation{value?.SetValue(target, System.Windows.Media.Animation.VectorAnimation.FromProperty); return target;}

//ToProperty
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Thickness>>? value, Disambigator<System.Windows.Media.Animation.ThicknessAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ThicknessAnimation{value?.SetValue(target, System.Windows.Media.Animation.ThicknessAnimation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Media.Media3D.Quaternion>>? value, Disambigator<System.Windows.Media.Animation.QuaternionAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.QuaternionAnimation{value?.SetValue(target, System.Windows.Media.Animation.QuaternionAnimation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Byte>>? value, Disambigator<System.Windows.Media.Animation.ByteAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ByteAnimation{value?.SetValue(target, System.Windows.Media.Animation.ByteAnimation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Media.Color>>? value, Disambigator<System.Windows.Media.Animation.ColorAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ColorAnimation{value?.SetValue(target, System.Windows.Media.Animation.ColorAnimation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Decimal>>? value, Disambigator<System.Windows.Media.Animation.DecimalAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DecimalAnimation{value?.SetValue(target, System.Windows.Media.Animation.DecimalAnimation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Double>>? value, Disambigator<System.Windows.Media.Animation.DoubleAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DoubleAnimation{value?.SetValue(target, System.Windows.Media.Animation.DoubleAnimation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Int16>>? value, Disambigator<System.Windows.Media.Animation.Int16Animation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int16Animation{value?.SetValue(target, System.Windows.Media.Animation.Int16Animation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Int32>>? value, Disambigator<System.Windows.Media.Animation.Int32Animation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int32Animation{value?.SetValue(target, System.Windows.Media.Animation.Int32Animation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Int64>>? value, Disambigator<System.Windows.Media.Animation.Int64Animation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int64Animation{value?.SetValue(target, System.Windows.Media.Animation.Int64Animation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Media.Media3D.Point3D>>? value, Disambigator<System.Windows.Media.Animation.Point3DAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Point3DAnimation{value?.SetValue(target, System.Windows.Media.Animation.Point3DAnimation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Point>>? value, Disambigator<System.Windows.Media.Animation.PointAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.PointAnimation{value?.SetValue(target, System.Windows.Media.Animation.PointAnimation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Rect>>? value, Disambigator<System.Windows.Media.Animation.RectAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.RectAnimation{value?.SetValue(target, System.Windows.Media.Animation.RectAnimation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Rotation3D>? value, Disambigator<System.Windows.Media.Animation.Rotation3DAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Rotation3DAnimation{value?.SetValue(target, System.Windows.Media.Animation.Rotation3DAnimation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Single>>? value, Disambigator<System.Windows.Media.Animation.SingleAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SingleAnimation{value?.SetValue(target, System.Windows.Media.Animation.SingleAnimation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Size>>? value, Disambigator<System.Windows.Media.Animation.SizeAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SizeAnimation{value?.SetValue(target, System.Windows.Media.Animation.SizeAnimation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Media.Media3D.Vector3D>>? value, Disambigator<System.Windows.Media.Animation.Vector3DAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Vector3DAnimation{value?.SetValue(target, System.Windows.Media.Animation.Vector3DAnimation.ToProperty); return target;}
public static TChild WithTo<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Vector>>? value, Disambigator<System.Windows.Media.Animation.VectorAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.VectorAnimation{value?.SetValue(target, System.Windows.Media.Animation.VectorAnimation.ToProperty); return target;}

//ByProperty
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Thickness>>? value, Disambigator<System.Windows.Media.Animation.ThicknessAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ThicknessAnimation{value?.SetValue(target, System.Windows.Media.Animation.ThicknessAnimation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Media.Media3D.Quaternion>>? value, Disambigator<System.Windows.Media.Animation.QuaternionAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.QuaternionAnimation{value?.SetValue(target, System.Windows.Media.Animation.QuaternionAnimation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Byte>>? value, Disambigator<System.Windows.Media.Animation.ByteAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ByteAnimation{value?.SetValue(target, System.Windows.Media.Animation.ByteAnimation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Media.Color>>? value, Disambigator<System.Windows.Media.Animation.ColorAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ColorAnimation{value?.SetValue(target, System.Windows.Media.Animation.ColorAnimation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Decimal>>? value, Disambigator<System.Windows.Media.Animation.DecimalAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DecimalAnimation{value?.SetValue(target, System.Windows.Media.Animation.DecimalAnimation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Double>>? value, Disambigator<System.Windows.Media.Animation.DoubleAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DoubleAnimation{value?.SetValue(target, System.Windows.Media.Animation.DoubleAnimation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Int16>>? value, Disambigator<System.Windows.Media.Animation.Int16Animation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int16Animation{value?.SetValue(target, System.Windows.Media.Animation.Int16Animation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Int32>>? value, Disambigator<System.Windows.Media.Animation.Int32Animation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int32Animation{value?.SetValue(target, System.Windows.Media.Animation.Int32Animation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Int64>>? value, Disambigator<System.Windows.Media.Animation.Int64Animation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Int64Animation{value?.SetValue(target, System.Windows.Media.Animation.Int64Animation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Media.Media3D.Point3D>>? value, Disambigator<System.Windows.Media.Animation.Point3DAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Point3DAnimation{value?.SetValue(target, System.Windows.Media.Animation.Point3DAnimation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Point>>? value, Disambigator<System.Windows.Media.Animation.PointAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.PointAnimation{value?.SetValue(target, System.Windows.Media.Animation.PointAnimation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Rect>>? value, Disambigator<System.Windows.Media.Animation.RectAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.RectAnimation{value?.SetValue(target, System.Windows.Media.Animation.RectAnimation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Rotation3D>? value, Disambigator<System.Windows.Media.Animation.Rotation3DAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Rotation3DAnimation{value?.SetValue(target, System.Windows.Media.Animation.Rotation3DAnimation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Single>>? value, Disambigator<System.Windows.Media.Animation.SingleAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SingleAnimation{value?.SetValue(target, System.Windows.Media.Animation.SingleAnimation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Size>>? value, Disambigator<System.Windows.Media.Animation.SizeAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SizeAnimation{value?.SetValue(target, System.Windows.Media.Animation.SizeAnimation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Media.Media3D.Vector3D>>? value, Disambigator<System.Windows.Media.Animation.Vector3DAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Vector3DAnimation{value?.SetValue(target, System.Windows.Media.Animation.Vector3DAnimation.ByProperty); return target;}
public static TChild WithBy<TChild>(this TChild target, ValueProxy<System.Nullable<System.Windows.Vector>>? value, Disambigator<System.Windows.Media.Animation.VectorAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.VectorAnimation{value?.SetValue(target, System.Windows.Media.Animation.VectorAnimation.ByProperty); return target;}

//TargetProperty
public static TChild WithStoryboard_Target<TChild>(this TChild target, ValueProxy<System.Windows.DependencyObject>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.Animation.Storyboard.TargetProperty); return target;}
public static TChild WithTarget<TChild>(this TChild target, ValueProxy<System.Windows.UIElement>? value, Disambigator<System.Windows.Controls.Label, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Label{value?.SetValue(target, System.Windows.Controls.Label.TargetProperty); return target;}
public static System.Windows.Media.BitmapCacheBrush WithTarget(this System.Windows.Media.BitmapCacheBrush target, ValueProxy<System.Windows.Media.Visual>? value) {value?.SetValue(target, System.Windows.Media.BitmapCacheBrush.TargetProperty); return target;}

//TargetNameProperty
public static TChild WithStoryboard_TargetName<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.Animation.Storyboard.TargetNameProperty); return target;}
public static TChild WithTargetName<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Documents.Hyperlink, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Hyperlink{value?.SetValue(target, System.Windows.Documents.Hyperlink.TargetNameProperty); return target;}
public static TChild WithTargetName<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.DataGridHyperlinkColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridHyperlinkColumn{value?.SetValue(target, System.Windows.Controls.DataGridHyperlinkColumn.TargetNameProperty); return target;}

//TargetPropertyProperty
public static TChild WithStoryboard_TargetProperty<TChild>(this TChild target, ValueProxy<System.Windows.PropertyPath>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.Animation.Storyboard.TargetPropertyProperty); return target;}

//XmlNamespaceManagerProperty
public static TChild WithBinding_XmlNamespaceManager<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Data.Binding.XmlNamespaceManagerProperty); return target;}

//CollectionProperty
public static TChild WithCollection<TChild>(this TChild target, ValueProxy<System.Collections.IEnumerable>? value, Disambigator<System.Windows.Data.CollectionContainer, TChild>? doNotUse = null) where TChild: System.Windows.Data.CollectionContainer{value?.SetValue(target, System.Windows.Data.CollectionContainer.CollectionProperty); return target;}

//ViewProperty
public static TChild WithView<TChild>(this TChild target, ValueProxy<System.ComponentModel.ICollectionView>? value, Disambigator<System.Windows.Data.CollectionViewSource, TChild>? doNotUse = null) where TChild: System.Windows.Data.CollectionViewSource{value?.SetValue(target, System.Windows.Data.CollectionViewSource.ViewProperty); return target;}
public static TChild WithView<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ViewBase>? value, Disambigator<System.Windows.Controls.ListView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ListView{value?.SetValue(target, System.Windows.Controls.ListView.ViewProperty); return target;}

//SourceProperty
public static TChild WithSource<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Data.CollectionViewSource, TChild>? doNotUse = null) where TChild: System.Windows.Data.CollectionViewSource{value?.SetValue(target, System.Windows.Data.CollectionViewSource.SourceProperty); return target;}
public static TChild WithSource<TChild>(this TChild target, ValueProxy<System.Uri>? value, Disambigator<System.Windows.Navigation.NavigationWindow, TChild>? doNotUse = null) where TChild: System.Windows.Navigation.NavigationWindow{value?.SetValue(target, System.Windows.Navigation.NavigationWindow.SourceProperty); return target;}
public static System.Windows.Documents.DocumentReference WithSource(this System.Windows.Documents.DocumentReference target, ValueProxy<System.Uri>? value) {value?.SetValue(target, System.Windows.Documents.DocumentReference.SourceProperty); return target;}
public static System.Windows.Documents.PageContent WithSource(this System.Windows.Documents.PageContent target, ValueProxy<System.Uri>? value) {value?.SetValue(target, System.Windows.Documents.PageContent.SourceProperty); return target;}
public static TChild WithSource<TChild>(this TChild target, ValueProxy<System.Uri>? value, Disambigator<System.Windows.Controls.Frame, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Frame{value?.SetValue(target, System.Windows.Controls.Frame.SourceProperty); return target;}
public static TChild WithSource<TChild>(this TChild target, ValueProxy<System.Windows.Media.ImageSource>? value, Disambigator<System.Windows.Controls.Image, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Image{value?.SetValue(target, System.Windows.Controls.Image.SourceProperty); return target;}
public static TChild WithSource<TChild>(this TChild target, ValueProxy<System.Uri>? value, Disambigator<System.Windows.Controls.MediaElement, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MediaElement{value?.SetValue(target, System.Windows.Controls.MediaElement.SourceProperty); return target;}
public static TChild WithSource<TChild>(this TChild target, ValueProxy<System.Uri>? value, Disambigator<System.Windows.Controls.SoundPlayerAction, TChild>? doNotUse = null) where TChild: System.Windows.Controls.SoundPlayerAction{value?.SetValue(target, System.Windows.Controls.SoundPlayerAction.SourceProperty); return target;}
public static TChild WithSource<TChild>(this TChild target, ValueProxy<System.Uri>? value, Disambigator<System.Windows.Media.MediaTimeline, TChild>? doNotUse = null) where TChild: System.Windows.Media.MediaTimeline{value?.SetValue(target, System.Windows.Media.MediaTimeline.SourceProperty); return target;}
public static TChild WithSource<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.PathAnimationSource>? value, Disambigator<System.Windows.Media.Animation.DoubleAnimationUsingPath, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DoubleAnimationUsingPath{value?.SetValue(target, System.Windows.Media.Animation.DoubleAnimationUsingPath.SourceProperty); return target;}
public static System.Windows.Media.Imaging.TransformedBitmap WithSource(this System.Windows.Media.Imaging.TransformedBitmap target, ValueProxy<System.Windows.Media.Imaging.BitmapSource>? value) {value?.SetValue(target, System.Windows.Media.Imaging.TransformedBitmap.SourceProperty); return target;}
public static System.Windows.Media.Imaging.CroppedBitmap WithSource(this System.Windows.Media.Imaging.CroppedBitmap target, ValueProxy<System.Windows.Media.Imaging.BitmapSource>? value) {value?.SetValue(target, System.Windows.Media.Imaging.CroppedBitmap.SourceProperty); return target;}
public static System.Windows.Media.Imaging.ColorConvertedBitmap WithSource(this System.Windows.Media.Imaging.ColorConvertedBitmap target, ValueProxy<System.Windows.Media.Imaging.BitmapSource>? value) {value?.SetValue(target, System.Windows.Media.Imaging.ColorConvertedBitmap.SourceProperty); return target;}
public static System.Windows.Media.Imaging.FormatConvertedBitmap WithSource(this System.Windows.Media.Imaging.FormatConvertedBitmap target, ValueProxy<System.Windows.Media.Imaging.BitmapSource>? value) {value?.SetValue(target, System.Windows.Media.Imaging.FormatConvertedBitmap.SourceProperty); return target;}

//CollectionViewTypeProperty
public static TChild WithCollectionViewType<TChild>(this TChild target, ValueProxy<System.Type>? value, Disambigator<System.Windows.Data.CollectionViewSource, TChild>? doNotUse = null) where TChild: System.Windows.Data.CollectionViewSource{value?.SetValue(target, System.Windows.Data.CollectionViewSource.CollectionViewTypeProperty); return target;}

//CanChangeLiveSortingProperty
public static TChild WithCanChangeLiveSorting<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Data.CollectionViewSource, TChild>? doNotUse = null) where TChild: System.Windows.Data.CollectionViewSource{value?.SetValue(target, System.Windows.Data.CollectionViewSource.CanChangeLiveSortingProperty); return target;}

//IsLiveSortingRequestedProperty
public static TChild WithIsLiveSortingRequested<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Data.CollectionViewSource, TChild>? doNotUse = null) where TChild: System.Windows.Data.CollectionViewSource{value?.SetValue(target, System.Windows.Data.CollectionViewSource.IsLiveSortingRequestedProperty); return target;}

//IsLiveSortingProperty
public static TChild WithIsLiveSorting<TChild>(this TChild target, ValueProxy<System.Nullable<System.Boolean>>? value, Disambigator<System.Windows.Data.CollectionViewSource, TChild>? doNotUse = null) where TChild: System.Windows.Data.CollectionViewSource{value?.SetValue(target, System.Windows.Data.CollectionViewSource.IsLiveSortingProperty); return target;}

//CanChangeLiveFilteringProperty
public static TChild WithCanChangeLiveFiltering<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Data.CollectionViewSource, TChild>? doNotUse = null) where TChild: System.Windows.Data.CollectionViewSource{value?.SetValue(target, System.Windows.Data.CollectionViewSource.CanChangeLiveFilteringProperty); return target;}

//IsLiveFilteringRequestedProperty
public static TChild WithIsLiveFilteringRequested<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Data.CollectionViewSource, TChild>? doNotUse = null) where TChild: System.Windows.Data.CollectionViewSource{value?.SetValue(target, System.Windows.Data.CollectionViewSource.IsLiveFilteringRequestedProperty); return target;}

//IsLiveFilteringProperty
public static TChild WithIsLiveFiltering<TChild>(this TChild target, ValueProxy<System.Nullable<System.Boolean>>? value, Disambigator<System.Windows.Data.CollectionViewSource, TChild>? doNotUse = null) where TChild: System.Windows.Data.CollectionViewSource{value?.SetValue(target, System.Windows.Data.CollectionViewSource.IsLiveFilteringProperty); return target;}

//CanChangeLiveGroupingProperty
public static TChild WithCanChangeLiveGrouping<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Data.CollectionViewSource, TChild>? doNotUse = null) where TChild: System.Windows.Data.CollectionViewSource{value?.SetValue(target, System.Windows.Data.CollectionViewSource.CanChangeLiveGroupingProperty); return target;}

//IsLiveGroupingRequestedProperty
public static TChild WithIsLiveGroupingRequested<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Data.CollectionViewSource, TChild>? doNotUse = null) where TChild: System.Windows.Data.CollectionViewSource{value?.SetValue(target, System.Windows.Data.CollectionViewSource.IsLiveGroupingRequestedProperty); return target;}

//IsLiveGroupingProperty
public static TChild WithIsLiveGrouping<TChild>(this TChild target, ValueProxy<System.Nullable<System.Boolean>>? value, Disambigator<System.Windows.Data.CollectionViewSource, TChild>? doNotUse = null) where TChild: System.Windows.Data.CollectionViewSource{value?.SetValue(target, System.Windows.Data.CollectionViewSource.IsLiveGroupingProperty); return target;}

//XmlSpaceProperty
public static TChild WithXmlAttributeProperties_XmlSpace<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Markup.XmlAttributeProperties.XmlSpaceProperty); return target;}

//XmlnsDictionaryProperty
public static TChild WithXmlAttributeProperties_XmlnsDictionary<TChild>(this TChild target, ValueProxy<System.Windows.Markup.XmlnsDictionary>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Markup.XmlAttributeProperties.XmlnsDictionaryProperty); return target;}

//XmlnsDefinitionProperty
public static TChild WithXmlAttributeProperties_XmlnsDefinition<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Markup.XmlAttributeProperties.XmlnsDefinitionProperty); return target;}

//XmlNamespaceMapsProperty
public static TChild WithXmlAttributeProperties_XmlNamespaceMaps<TChild>(this TChild target, ValueProxy<System.Collections.Hashtable>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Markup.XmlAttributeProperties.XmlNamespaceMapsProperty); return target;}

//KeepAliveProperty
public static TChild WithJournalEntry_KeepAlive<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Navigation.JournalEntry.KeepAliveProperty); return target;}
public static TChild WithKeepAlive<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Page, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Page{value?.SetValue(target, System.Windows.Controls.Page.KeepAliveProperty); return target;}

//JournalEntryPositionProperty
public static TChild WithJournalEntryUnifiedViewConverter_JournalEntryPosition<TChild>(this TChild target, ValueProxy<System.Windows.Navigation.JournalEntryPosition>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Navigation.JournalEntryUnifiedViewConverter.JournalEntryPositionProperty); return target;}

//SandboxExternalContentProperty
public static TChild WithSandboxExternalContent<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Navigation.NavigationWindow, TChild>? doNotUse = null) where TChild: System.Windows.Navigation.NavigationWindow{value?.SetValue(target, System.Windows.Navigation.NavigationWindow.SandboxExternalContentProperty); return target;}
public static TChild WithSandboxExternalContent<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Frame, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Frame{value?.SetValue(target, System.Windows.Controls.Frame.SandboxExternalContentProperty); return target;}

//ShowsNavigationUIProperty
public static TChild WithShowsNavigationUI<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Navigation.NavigationWindow, TChild>? doNotUse = null) where TChild: System.Windows.Navigation.NavigationWindow{value?.SetValue(target, System.Windows.Navigation.NavigationWindow.ShowsNavigationUIProperty); return target;}

//BackStackProperty
public static TChild WithBackStack<TChild>(this TChild target, ValueProxy<System.Collections.IEnumerable>? value, Disambigator<System.Windows.Navigation.NavigationWindow, TChild>? doNotUse = null) where TChild: System.Windows.Navigation.NavigationWindow{value?.SetValue(target, System.Windows.Navigation.NavigationWindow.BackStackProperty); return target;}
public static TChild WithBackStack<TChild>(this TChild target, ValueProxy<System.Collections.IEnumerable>? value, Disambigator<System.Windows.Controls.Frame, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Frame{value?.SetValue(target, System.Windows.Controls.Frame.BackStackProperty); return target;}

//ForwardStackProperty
public static TChild WithForwardStack<TChild>(this TChild target, ValueProxy<System.Collections.IEnumerable>? value, Disambigator<System.Windows.Navigation.NavigationWindow, TChild>? doNotUse = null) where TChild: System.Windows.Navigation.NavigationWindow{value?.SetValue(target, System.Windows.Navigation.NavigationWindow.ForwardStackProperty); return target;}
public static TChild WithForwardStack<TChild>(this TChild target, ValueProxy<System.Collections.IEnumerable>? value, Disambigator<System.Windows.Controls.Frame, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Frame{value?.SetValue(target, System.Windows.Controls.Frame.ForwardStackProperty); return target;}

//CanGoBackProperty
public static TChild WithCanGoBack<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Navigation.NavigationWindow, TChild>? doNotUse = null) where TChild: System.Windows.Navigation.NavigationWindow{value?.SetValue(target, System.Windows.Navigation.NavigationWindow.CanGoBackProperty); return target;}
public static TChild WithCanGoBack<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Frame, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Frame{value?.SetValue(target, System.Windows.Controls.Frame.CanGoBackProperty); return target;}

//CanGoForwardProperty
public static TChild WithCanGoForward<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Navigation.NavigationWindow, TChild>? doNotUse = null) where TChild: System.Windows.Navigation.NavigationWindow{value?.SetValue(target, System.Windows.Navigation.NavigationWindow.CanGoForwardProperty); return target;}
public static TChild WithCanGoForward<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Frame, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Frame{value?.SetValue(target, System.Windows.Controls.Frame.CanGoForwardProperty); return target;}

//PaddingProperty
public static TChild WithPadding<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Documents.AnchoredBlock, TChild>? doNotUse = null) where TChild: System.Windows.Documents.AnchoredBlock{value?.SetValue(target, System.Windows.Documents.AnchoredBlock.PaddingProperty); return target;}
public static TChild WithPadding<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Documents.Block, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Block{value?.SetValue(target, System.Windows.Documents.Block.PaddingProperty); return target;}
public static TChild WithPadding<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Documents.ListItem, TChild>? doNotUse = null) where TChild: System.Windows.Documents.ListItem{value?.SetValue(target, System.Windows.Documents.ListItem.PaddingProperty); return target;}
public static TChild WithPadding<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Documents.TableCell, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TableCell{value?.SetValue(target, System.Windows.Documents.TableCell.PaddingProperty); return target;}
public static TChild WithPadding<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Controls.Border, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Border{value?.SetValue(target, System.Windows.Controls.Border.PaddingProperty); return target;}
public static TChild WithPadding<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.PaddingProperty); return target;}
public static TChild WithPadding<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.PaddingProperty); return target;}

//BorderThicknessProperty
public static TChild WithBorderThickness<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Documents.AnchoredBlock, TChild>? doNotUse = null) where TChild: System.Windows.Documents.AnchoredBlock{value?.SetValue(target, System.Windows.Documents.AnchoredBlock.BorderThicknessProperty); return target;}
public static TChild WithBorderThickness<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Documents.Block, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Block{value?.SetValue(target, System.Windows.Documents.Block.BorderThicknessProperty); return target;}
public static TChild WithBorderThickness<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Documents.ListItem, TChild>? doNotUse = null) where TChild: System.Windows.Documents.ListItem{value?.SetValue(target, System.Windows.Documents.ListItem.BorderThicknessProperty); return target;}
public static TChild WithBorderThickness<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Documents.TableCell, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TableCell{value?.SetValue(target, System.Windows.Documents.TableCell.BorderThicknessProperty); return target;}
public static TChild WithBorderThickness<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Controls.Border, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Border{value?.SetValue(target, System.Windows.Controls.Border.BorderThicknessProperty); return target;}
public static TChild WithBorderThickness<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.BorderThicknessProperty); return target;}

//BorderBrushProperty
public static TChild WithBorderBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Documents.AnchoredBlock, TChild>? doNotUse = null) where TChild: System.Windows.Documents.AnchoredBlock{value?.SetValue(target, System.Windows.Documents.AnchoredBlock.BorderBrushProperty); return target;}
public static TChild WithBorderBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Documents.Block, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Block{value?.SetValue(target, System.Windows.Documents.Block.BorderBrushProperty); return target;}
public static TChild WithBorderBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Documents.ListItem, TChild>? doNotUse = null) where TChild: System.Windows.Documents.ListItem{value?.SetValue(target, System.Windows.Documents.ListItem.BorderBrushProperty); return target;}
public static TChild WithBorderBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Documents.TableCell, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TableCell{value?.SetValue(target, System.Windows.Documents.TableCell.BorderBrushProperty); return target;}
public static TChild WithBorderBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Border, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Border{value?.SetValue(target, System.Windows.Controls.Border.BorderBrushProperty); return target;}
public static TChild WithBorderBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.BorderBrushProperty); return target;}

//TextAlignmentProperty
public static TChild WithTextAlignment<TChild>(this TChild target, ValueProxy<System.Windows.TextAlignment>? value, Disambigator<System.Windows.Documents.AnchoredBlock, TChild>? doNotUse = null) where TChild: System.Windows.Documents.AnchoredBlock{value?.SetValue(target, System.Windows.Documents.AnchoredBlock.TextAlignmentProperty); return target;}
public static TChild WithBlock_TextAlignment<TChild>(this TChild target, ValueProxy<System.Windows.TextAlignment>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Block.TextAlignmentProperty); return target;}
public static TChild WithTextAlignment<TChild>(this TChild target, ValueProxy<System.Windows.TextAlignment>? value, Disambigator<System.Windows.Documents.Block, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Block{value?.SetValue(target, System.Windows.Documents.Block.TextAlignmentProperty); return target;}
public static TChild WithTextAlignment<TChild>(this TChild target, ValueProxy<System.Windows.TextAlignment>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.TextAlignmentProperty); return target;}
public static TChild WithTextAlignment<TChild>(this TChild target, ValueProxy<System.Windows.TextAlignment>? value, Disambigator<System.Windows.Documents.ListItem, TChild>? doNotUse = null) where TChild: System.Windows.Documents.ListItem{value?.SetValue(target, System.Windows.Documents.ListItem.TextAlignmentProperty); return target;}
public static TChild WithTextAlignment<TChild>(this TChild target, ValueProxy<System.Windows.TextAlignment>? value, Disambigator<System.Windows.Documents.TableCell, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TableCell{value?.SetValue(target, System.Windows.Documents.TableCell.TextAlignmentProperty); return target;}
public static TChild WithTextAlignment<TChild>(this TChild target, ValueProxy<System.Windows.TextAlignment>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.TextAlignmentProperty); return target;}
public static TChild WithTextBlock_TextAlignment<TChild>(this TChild target, ValueProxy<System.Windows.TextAlignment>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.TextBlock.TextAlignmentProperty); return target;}
public static TChild WithTextAlignment<TChild>(this TChild target, ValueProxy<System.Windows.TextAlignment>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.TextAlignmentProperty); return target;}
public static TChild WithTextAlignment<TChild>(this TChild target, ValueProxy<System.Windows.TextAlignment>? value, Disambigator<System.Windows.Controls.TextBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBox{value?.SetValue(target, System.Windows.Controls.TextBox.TextAlignmentProperty); return target;}

//LineHeightProperty
public static TChild WithLineHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.AnchoredBlock, TChild>? doNotUse = null) where TChild: System.Windows.Documents.AnchoredBlock{value?.SetValue(target, System.Windows.Documents.AnchoredBlock.LineHeightProperty); return target;}
public static TChild WithBlock_LineHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Block.LineHeightProperty); return target;}
public static TChild WithLineHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.Block, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Block{value?.SetValue(target, System.Windows.Documents.Block.LineHeightProperty); return target;}
public static TChild WithLineHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.LineHeightProperty); return target;}
public static TChild WithLineHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.ListItem, TChild>? doNotUse = null) where TChild: System.Windows.Documents.ListItem{value?.SetValue(target, System.Windows.Documents.ListItem.LineHeightProperty); return target;}
public static TChild WithLineHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.TableCell, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TableCell{value?.SetValue(target, System.Windows.Documents.TableCell.LineHeightProperty); return target;}
public static TChild WithLineHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.LineHeightProperty); return target;}
public static TChild WithTextBlock_LineHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.TextBlock.LineHeightProperty); return target;}
public static TChild WithLineHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.LineHeightProperty); return target;}

//LineStackingStrategyProperty
public static TChild WithLineStackingStrategy<TChild>(this TChild target, ValueProxy<System.Windows.LineStackingStrategy>? value, Disambigator<System.Windows.Documents.AnchoredBlock, TChild>? doNotUse = null) where TChild: System.Windows.Documents.AnchoredBlock{value?.SetValue(target, System.Windows.Documents.AnchoredBlock.LineStackingStrategyProperty); return target;}
public static TChild WithBlock_LineStackingStrategy<TChild>(this TChild target, ValueProxy<System.Windows.LineStackingStrategy>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Block.LineStackingStrategyProperty); return target;}
public static TChild WithLineStackingStrategy<TChild>(this TChild target, ValueProxy<System.Windows.LineStackingStrategy>? value, Disambigator<System.Windows.Documents.Block, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Block{value?.SetValue(target, System.Windows.Documents.Block.LineStackingStrategyProperty); return target;}
public static TChild WithLineStackingStrategy<TChild>(this TChild target, ValueProxy<System.Windows.LineStackingStrategy>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.LineStackingStrategyProperty); return target;}
public static TChild WithLineStackingStrategy<TChild>(this TChild target, ValueProxy<System.Windows.LineStackingStrategy>? value, Disambigator<System.Windows.Documents.ListItem, TChild>? doNotUse = null) where TChild: System.Windows.Documents.ListItem{value?.SetValue(target, System.Windows.Documents.ListItem.LineStackingStrategyProperty); return target;}
public static TChild WithLineStackingStrategy<TChild>(this TChild target, ValueProxy<System.Windows.LineStackingStrategy>? value, Disambigator<System.Windows.Documents.TableCell, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TableCell{value?.SetValue(target, System.Windows.Documents.TableCell.LineStackingStrategyProperty); return target;}
public static TChild WithLineStackingStrategy<TChild>(this TChild target, ValueProxy<System.Windows.LineStackingStrategy>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.LineStackingStrategyProperty); return target;}
public static TChild WithTextBlock_LineStackingStrategy<TChild>(this TChild target, ValueProxy<System.Windows.LineStackingStrategy>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.TextBlock.LineStackingStrategyProperty); return target;}
public static TChild WithLineStackingStrategy<TChild>(this TChild target, ValueProxy<System.Windows.LineStackingStrategy>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.LineStackingStrategyProperty); return target;}

//IsHyphenationEnabledProperty
public static TChild WithBlock_IsHyphenationEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Block.IsHyphenationEnabledProperty); return target;}
public static TChild WithIsHyphenationEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Documents.Block, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Block{value?.SetValue(target, System.Windows.Documents.Block.IsHyphenationEnabledProperty); return target;}
public static TChild WithIsHyphenationEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.IsHyphenationEnabledProperty); return target;}
public static TChild WithIsHyphenationEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.IsHyphenationEnabledProperty); return target;}

//BreakPageBeforeProperty
public static TChild WithBreakPageBefore<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Documents.Block, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Block{value?.SetValue(target, System.Windows.Documents.Block.BreakPageBeforeProperty); return target;}

//BreakColumnBeforeProperty
public static TChild WithBreakColumnBefore<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Documents.Block, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Block{value?.SetValue(target, System.Windows.Documents.Block.BreakColumnBeforeProperty); return target;}

//ClearFloatersProperty
public static TChild WithClearFloaters<TChild>(this TChild target, ValueProxy<System.Windows.WrapDirection>? value, Disambigator<System.Windows.Documents.Block, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Block{value?.SetValue(target, System.Windows.Documents.Block.ClearFloatersProperty); return target;}

//PrintTicketProperty
public static TChild WithPrintTicket<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Documents.FixedDocumentSequence, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FixedDocumentSequence{value?.SetValue(target, System.Windows.Documents.FixedDocumentSequence.PrintTicketProperty); return target;}
public static TChild WithPrintTicket<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Documents.FixedDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FixedDocument{value?.SetValue(target, System.Windows.Documents.FixedDocument.PrintTicketProperty); return target;}
public static System.Windows.Documents.FixedPage WithPrintTicket(this System.Windows.Documents.FixedPage target, ValueProxy<System.Object>? value) {value?.SetValue(target, System.Windows.Documents.FixedPage.PrintTicketProperty); return target;}

//HorizontalAnchorProperty
public static TChild WithHorizontalAnchor<TChild>(this TChild target, ValueProxy<System.Windows.FigureHorizontalAnchor>? value, Disambigator<System.Windows.Documents.Figure, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Figure{value?.SetValue(target, System.Windows.Documents.Figure.HorizontalAnchorProperty); return target;}

//VerticalAnchorProperty
public static TChild WithVerticalAnchor<TChild>(this TChild target, ValueProxy<System.Windows.FigureVerticalAnchor>? value, Disambigator<System.Windows.Documents.Figure, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Figure{value?.SetValue(target, System.Windows.Documents.Figure.VerticalAnchorProperty); return target;}

//HorizontalOffsetProperty
public static TChild WithHorizontalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.Figure, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Figure{value?.SetValue(target, System.Windows.Documents.Figure.HorizontalOffsetProperty); return target;}
public static TChild WithHorizontalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ContextMenu, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContextMenu{value?.SetValue(target, System.Windows.Controls.ContextMenu.HorizontalOffsetProperty); return target;}
public static TChild WithContextMenuService_HorizontalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ContextMenuService.HorizontalOffsetProperty); return target;}
public static TChild WithHorizontalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.HorizontalOffsetProperty); return target;}
public static TChild WithHorizontalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.HorizontalOffsetProperty); return target;}
public static TChild WithHorizontalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ToolTip, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolTip{value?.SetValue(target, System.Windows.Controls.ToolTip.HorizontalOffsetProperty); return target;}
public static TChild WithToolTipService_HorizontalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolTipService.HorizontalOffsetProperty); return target;}
public static TChild WithHorizontalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.Popup, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Popup{value?.SetValue(target, System.Windows.Controls.Primitives.Popup.HorizontalOffsetProperty); return target;}

//VerticalOffsetProperty
public static TChild WithVerticalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.Figure, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Figure{value?.SetValue(target, System.Windows.Documents.Figure.VerticalOffsetProperty); return target;}
public static TChild WithVerticalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ContextMenu, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContextMenu{value?.SetValue(target, System.Windows.Controls.ContextMenu.VerticalOffsetProperty); return target;}
public static TChild WithContextMenuService_VerticalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ContextMenuService.VerticalOffsetProperty); return target;}
public static TChild WithVerticalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.VerticalOffsetProperty); return target;}
public static TChild WithVerticalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.VerticalOffsetProperty); return target;}
public static TChild WithVerticalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ToolTip, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolTip{value?.SetValue(target, System.Windows.Controls.ToolTip.VerticalOffsetProperty); return target;}
public static TChild WithToolTipService_VerticalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolTipService.VerticalOffsetProperty); return target;}
public static TChild WithVerticalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.Popup, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Popup{value?.SetValue(target, System.Windows.Controls.Primitives.Popup.VerticalOffsetProperty); return target;}

//CanDelayPlacementProperty
public static TChild WithCanDelayPlacement<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Documents.Figure, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Figure{value?.SetValue(target, System.Windows.Documents.Figure.CanDelayPlacementProperty); return target;}

//WrapDirectionProperty
public static TChild WithWrapDirection<TChild>(this TChild target, ValueProxy<System.Windows.WrapDirection>? value, Disambigator<System.Windows.Documents.Figure, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Figure{value?.SetValue(target, System.Windows.Documents.Figure.WrapDirectionProperty); return target;}

//BackgroundProperty
public static System.Windows.Documents.FixedPage WithBackground(this System.Windows.Documents.FixedPage target, ValueProxy<System.Windows.Media.Brush>? value) {value?.SetValue(target, System.Windows.Documents.FixedPage.BackgroundProperty); return target;}
public static TChild WithBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.BackgroundProperty); return target;}
public static TChild WithBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Documents.TableColumn, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TableColumn{value?.SetValue(target, System.Windows.Documents.TableColumn.BackgroundProperty); return target;}
public static TChild WithBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Documents.TextElement, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TextElement{value?.SetValue(target, System.Windows.Documents.TextElement.BackgroundProperty); return target;}
public static TChild WithBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.BackgroundProperty); return target;}
public static TChild WithBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Border, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Border{value?.SetValue(target, System.Windows.Controls.Border.BackgroundProperty); return target;}
public static TChild WithBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.BackgroundProperty); return target;}
public static TChild WithBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.InkCanvas, TChild>? doNotUse = null) where TChild: System.Windows.Controls.InkCanvas{value?.SetValue(target, System.Windows.Controls.InkCanvas.BackgroundProperty); return target;}
public static TChild WithBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Page, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Page{value?.SetValue(target, System.Windows.Controls.Page.BackgroundProperty); return target;}
public static TChild WithBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Panel, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Panel{value?.SetValue(target, System.Windows.Controls.Panel.BackgroundProperty); return target;}
public static TChild WithBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.BackgroundProperty); return target;}
public static TChild WithBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.ToolBarTray, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolBarTray{value?.SetValue(target, System.Windows.Controls.ToolBarTray.BackgroundProperty); return target;}
public static TChild WithBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Primitives.BulletDecorator, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.BulletDecorator{value?.SetValue(target, System.Windows.Controls.Primitives.BulletDecorator.BackgroundProperty); return target;}

//RightProperty
public static TChild WithFixedPage_Right<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Documents.FixedPage.RightProperty); return target;}
public static TChild WithCanvas_Right<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.Canvas.RightProperty); return target;}
public static TChild WithInkCanvas_Right<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.InkCanvas.RightProperty); return target;}

//BottomProperty
public static TChild WithFixedPage_Bottom<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Documents.FixedPage.BottomProperty); return target;}
public static TChild WithCanvas_Bottom<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.Canvas.BottomProperty); return target;}
public static TChild WithInkCanvas_Bottom<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.InkCanvas.BottomProperty); return target;}

//ContentBoxProperty
public static System.Windows.Documents.FixedPage WithContentBox(this System.Windows.Documents.FixedPage target, ValueProxy<System.Windows.Rect>? value) {value?.SetValue(target, System.Windows.Documents.FixedPage.ContentBoxProperty); return target;}

//BleedBoxProperty
public static System.Windows.Documents.FixedPage WithBleedBox(this System.Windows.Documents.FixedPage target, ValueProxy<System.Windows.Rect>? value) {value?.SetValue(target, System.Windows.Documents.FixedPage.BleedBoxProperty); return target;}

//NavigateUriProperty
public static TChild WithFixedPage_NavigateUri<TChild>(this TChild target, ValueProxy<System.Uri>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Documents.FixedPage.NavigateUriProperty); return target;}
public static TChild WithNavigateUri<TChild>(this TChild target, ValueProxy<System.Uri>? value, Disambigator<System.Windows.Documents.Hyperlink, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Hyperlink{value?.SetValue(target, System.Windows.Documents.Hyperlink.NavigateUriProperty); return target;}

//FontFamilyProperty
public static TChild WithFontFamily<TChild>(this TChild target, ValueProxy<System.Windows.Media.FontFamily>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.FontFamilyProperty); return target;}
public static TChild WithTextElement_FontFamily<TChild>(this TChild target, ValueProxy<System.Windows.Media.FontFamily>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.TextElement.FontFamilyProperty); return target;}
public static TChild WithFontFamily<TChild>(this TChild target, ValueProxy<System.Windows.Media.FontFamily>? value, Disambigator<System.Windows.Documents.TextElement, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TextElement{value?.SetValue(target, System.Windows.Documents.TextElement.FontFamilyProperty); return target;}
public static TChild WithFontFamily<TChild>(this TChild target, ValueProxy<System.Windows.Media.FontFamily>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.FontFamilyProperty); return target;}
public static TChild WithFontFamily<TChild>(this TChild target, ValueProxy<System.Windows.Media.FontFamily>? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.FontFamilyProperty); return target;}
public static TChild WithFontFamily<TChild>(this TChild target, ValueProxy<System.Windows.Media.FontFamily>? value, Disambigator<System.Windows.Controls.DataGridTextColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridTextColumn{value?.SetValue(target, System.Windows.Controls.DataGridTextColumn.FontFamilyProperty); return target;}
public static TChild WithFontFamily<TChild>(this TChild target, ValueProxy<System.Windows.Media.FontFamily>? value, Disambigator<System.Windows.Controls.Page, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Page{value?.SetValue(target, System.Windows.Controls.Page.FontFamilyProperty); return target;}
public static TChild WithTextBlock_FontFamily<TChild>(this TChild target, ValueProxy<System.Windows.Media.FontFamily>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.TextBlock.FontFamilyProperty); return target;}
public static TChild WithFontFamily<TChild>(this TChild target, ValueProxy<System.Windows.Media.FontFamily>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.FontFamilyProperty); return target;}

//FontStyleProperty
public static TChild WithFontStyle<TChild>(this TChild target, ValueProxy<System.Windows.FontStyle>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.FontStyleProperty); return target;}
public static TChild WithTextElement_FontStyle<TChild>(this TChild target, ValueProxy<System.Windows.FontStyle>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.TextElement.FontStyleProperty); return target;}
public static TChild WithFontStyle<TChild>(this TChild target, ValueProxy<System.Windows.FontStyle>? value, Disambigator<System.Windows.Documents.TextElement, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TextElement{value?.SetValue(target, System.Windows.Documents.TextElement.FontStyleProperty); return target;}
public static TChild WithFontStyle<TChild>(this TChild target, ValueProxy<System.Windows.FontStyle>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.FontStyleProperty); return target;}
public static TChild WithFontStyle<TChild>(this TChild target, ValueProxy<System.Windows.FontStyle>? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.FontStyleProperty); return target;}
public static TChild WithFontStyle<TChild>(this TChild target, ValueProxy<System.Windows.FontStyle>? value, Disambigator<System.Windows.Controls.DataGridTextColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridTextColumn{value?.SetValue(target, System.Windows.Controls.DataGridTextColumn.FontStyleProperty); return target;}
public static TChild WithTextBlock_FontStyle<TChild>(this TChild target, ValueProxy<System.Windows.FontStyle>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.TextBlock.FontStyleProperty); return target;}
public static TChild WithFontStyle<TChild>(this TChild target, ValueProxy<System.Windows.FontStyle>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.FontStyleProperty); return target;}

//FontWeightProperty
public static TChild WithFontWeight<TChild>(this TChild target, ValueProxy<System.Windows.FontWeight>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.FontWeightProperty); return target;}
public static TChild WithTextElement_FontWeight<TChild>(this TChild target, ValueProxy<System.Windows.FontWeight>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.TextElement.FontWeightProperty); return target;}
public static TChild WithFontWeight<TChild>(this TChild target, ValueProxy<System.Windows.FontWeight>? value, Disambigator<System.Windows.Documents.TextElement, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TextElement{value?.SetValue(target, System.Windows.Documents.TextElement.FontWeightProperty); return target;}
public static TChild WithFontWeight<TChild>(this TChild target, ValueProxy<System.Windows.FontWeight>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.FontWeightProperty); return target;}
public static TChild WithFontWeight<TChild>(this TChild target, ValueProxy<System.Windows.FontWeight>? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.FontWeightProperty); return target;}
public static TChild WithFontWeight<TChild>(this TChild target, ValueProxy<System.Windows.FontWeight>? value, Disambigator<System.Windows.Controls.DataGridTextColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridTextColumn{value?.SetValue(target, System.Windows.Controls.DataGridTextColumn.FontWeightProperty); return target;}
public static TChild WithTextBlock_FontWeight<TChild>(this TChild target, ValueProxy<System.Windows.FontWeight>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.TextBlock.FontWeightProperty); return target;}
public static TChild WithFontWeight<TChild>(this TChild target, ValueProxy<System.Windows.FontWeight>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.FontWeightProperty); return target;}

//FontStretchProperty
public static TChild WithFontStretch<TChild>(this TChild target, ValueProxy<System.Windows.FontStretch>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.FontStretchProperty); return target;}
public static TChild WithTextElement_FontStretch<TChild>(this TChild target, ValueProxy<System.Windows.FontStretch>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.TextElement.FontStretchProperty); return target;}
public static TChild WithFontStretch<TChild>(this TChild target, ValueProxy<System.Windows.FontStretch>? value, Disambigator<System.Windows.Documents.TextElement, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TextElement{value?.SetValue(target, System.Windows.Documents.TextElement.FontStretchProperty); return target;}
public static TChild WithFontStretch<TChild>(this TChild target, ValueProxy<System.Windows.FontStretch>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.FontStretchProperty); return target;}
public static TChild WithFontStretch<TChild>(this TChild target, ValueProxy<System.Windows.FontStretch>? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.FontStretchProperty); return target;}
public static TChild WithTextBlock_FontStretch<TChild>(this TChild target, ValueProxy<System.Windows.FontStretch>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.TextBlock.FontStretchProperty); return target;}
public static TChild WithFontStretch<TChild>(this TChild target, ValueProxy<System.Windows.FontStretch>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.FontStretchProperty); return target;}

//FontSizeProperty
public static TChild WithFontSize<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.FontSizeProperty); return target;}
public static TChild WithTextElement_FontSize<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.TextElement.FontSizeProperty); return target;}
public static TChild WithFontSize<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.TextElement, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TextElement{value?.SetValue(target, System.Windows.Documents.TextElement.FontSizeProperty); return target;}
public static TChild WithFontSize<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.FontSizeProperty); return target;}
public static TChild WithFontSize<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.FontSizeProperty); return target;}
public static TChild WithFontSize<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DataGridTextColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridTextColumn{value?.SetValue(target, System.Windows.Controls.DataGridTextColumn.FontSizeProperty); return target;}
public static TChild WithFontSize<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Page, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Page{value?.SetValue(target, System.Windows.Controls.Page.FontSizeProperty); return target;}
public static TChild WithTextBlock_FontSize<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.TextBlock.FontSizeProperty); return target;}
public static TChild WithFontSize<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.FontSizeProperty); return target;}

//ForegroundProperty
public static TChild WithForeground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.ForegroundProperty); return target;}
public static TChild WithTextElement_Foreground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.TextElement.ForegroundProperty); return target;}

public static TChild WithForeground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Documents.TextElement, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TextElement{value?.SetValue(target, System.Windows.Documents.TextElement.ForegroundProperty); return target;}
public static TChild WithForeground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.ForegroundProperty); return target;}
public static TChild WithForeground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.ForegroundProperty); return target;}
public static TChild WithForeground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.DataGridTextColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridTextColumn{value?.SetValue(target, System.Windows.Controls.DataGridTextColumn.ForegroundProperty); return target;}
public static TChild WithForeground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Page, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Page{value?.SetValue(target, System.Windows.Controls.Page.ForegroundProperty); return target;}
public static TChild WithTextBlock_Foreground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.TextBlock.ForegroundProperty); return target;}
public static TChild WithForeground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.ForegroundProperty); return target;}
public static System.Windows.Media.TextEffect WithForeground(this System.Windows.Media.TextEffect target, ValueProxy<System.Windows.Media.Brush>? value) {value?.SetValue(target, System.Windows.Media.TextEffect.ForegroundProperty); return target;}

//TextEffectsProperty
public static TChild WithTextEffects<TChild>(this TChild target, ValueProxy<System.Windows.Media.TextEffectCollection>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.TextEffectsProperty); return target;}
public static TChild WithTextEffects<TChild>(this TChild target, ValueProxy<System.Windows.Media.TextEffectCollection>? value, Disambigator<System.Windows.Documents.TextElement, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TextElement{value?.SetValue(target, System.Windows.Documents.TextElement.TextEffectsProperty); return target;}
public static TChild WithTextEffects<TChild>(this TChild target, ValueProxy<System.Windows.Media.TextEffectCollection>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.TextEffectsProperty); return target;}
public static TChild WithTextEffects<TChild>(this TChild target, ValueProxy<System.Windows.Media.TextEffectCollection>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.TextEffectsProperty); return target;}

//ColumnWidthProperty
public static TChild WithColumnWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.ColumnWidthProperty); return target;}
public static TChild WithColumnWidth<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataGridLength>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.ColumnWidthProperty); return target;}

//ColumnGapProperty
public static TChild WithColumnGap<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.ColumnGapProperty); return target;}

//IsColumnWidthFlexibleProperty
public static TChild WithIsColumnWidthFlexible<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.IsColumnWidthFlexibleProperty); return target;}

//ColumnRuleWidthProperty
public static TChild WithColumnRuleWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.ColumnRuleWidthProperty); return target;}

//ColumnRuleBrushProperty
public static TChild WithColumnRuleBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.ColumnRuleBrushProperty); return target;}

//IsOptimalParagraphEnabledProperty
public static TChild WithIsOptimalParagraphEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.IsOptimalParagraphEnabledProperty); return target;}

//PageWidthProperty
public static TChild WithPageWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.PageWidthProperty); return target;}

//MinPageWidthProperty
public static TChild WithMinPageWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.MinPageWidthProperty); return target;}

//MaxPageWidthProperty
public static TChild WithMaxPageWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.MaxPageWidthProperty); return target;}

//PageHeightProperty
public static TChild WithPageHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.PageHeightProperty); return target;}

//MinPageHeightProperty
public static TChild WithMinPageHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.MinPageHeightProperty); return target;}

//MaxPageHeightProperty
public static TChild WithMaxPageHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.MaxPageHeightProperty); return target;}

//PagePaddingProperty
public static TChild WithPagePadding<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Documents.FlowDocument, TChild>? doNotUse = null) where TChild: System.Windows.Documents.FlowDocument{value?.SetValue(target, System.Windows.Documents.FlowDocument.PagePaddingProperty); return target;}

//IndicesProperty
public static System.Windows.Documents.Glyphs WithIndices(this System.Windows.Documents.Glyphs target, ValueProxy<System.String>? value) {value?.SetValue(target, System.Windows.Documents.Glyphs.IndicesProperty); return target;}

//UnicodeStringProperty
public static System.Windows.Documents.Glyphs WithUnicodeString(this System.Windows.Documents.Glyphs target, ValueProxy<System.String>? value) {value?.SetValue(target, System.Windows.Documents.Glyphs.UnicodeStringProperty); return target;}

//CaretStopsProperty
public static System.Windows.Documents.Glyphs WithCaretStops(this System.Windows.Documents.Glyphs target, ValueProxy<System.String>? value) {value?.SetValue(target, System.Windows.Documents.Glyphs.CaretStopsProperty); return target;}

//FontRenderingEmSizeProperty
public static System.Windows.Documents.Glyphs WithFontRenderingEmSize(this System.Windows.Documents.Glyphs target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Documents.Glyphs.FontRenderingEmSizeProperty); return target;}

//OriginXProperty
public static System.Windows.Documents.Glyphs WithOriginX(this System.Windows.Documents.Glyphs target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Documents.Glyphs.OriginXProperty); return target;}

//OriginYProperty
public static System.Windows.Documents.Glyphs WithOriginY(this System.Windows.Documents.Glyphs target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Documents.Glyphs.OriginYProperty); return target;}

//FontUriProperty
public static System.Windows.Documents.Glyphs WithFontUri(this System.Windows.Documents.Glyphs target, ValueProxy<System.Uri>? value) {value?.SetValue(target, System.Windows.Documents.Glyphs.FontUriProperty); return target;}

//StyleSimulationsProperty
public static System.Windows.Documents.Glyphs WithStyleSimulations(this System.Windows.Documents.Glyphs target, ValueProxy<System.Windows.Media.StyleSimulations>? value) {value?.SetValue(target, System.Windows.Documents.Glyphs.StyleSimulationsProperty); return target;}

//IsSidewaysProperty
public static System.Windows.Documents.Glyphs WithIsSideways(this System.Windows.Documents.Glyphs target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Documents.Glyphs.IsSidewaysProperty); return target;}

//BidiLevelProperty
public static System.Windows.Documents.Glyphs WithBidiLevel(this System.Windows.Documents.Glyphs target, ValueProxy<System.Int32>? value) {value?.SetValue(target, System.Windows.Documents.Glyphs.BidiLevelProperty); return target;}

//DeviceFontNameProperty
public static System.Windows.Documents.Glyphs WithDeviceFontName(this System.Windows.Documents.Glyphs target, ValueProxy<System.String>? value) {value?.SetValue(target, System.Windows.Documents.Glyphs.DeviceFontNameProperty); return target;}

//BaselineAlignmentProperty
public static TChild WithBaselineAlignment<TChild>(this TChild target, ValueProxy<System.Windows.BaselineAlignment>? value, Disambigator<System.Windows.Documents.Inline, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Inline{value?.SetValue(target, System.Windows.Documents.Inline.BaselineAlignmentProperty); return target;}

//TextDecorationsProperty
public static TChild WithTextDecorations<TChild>(this TChild target, ValueProxy<System.Windows.TextDecorationCollection>? value, Disambigator<System.Windows.Documents.Inline, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Inline{value?.SetValue(target, System.Windows.Documents.Inline.TextDecorationsProperty); return target;}
public static TChild WithTextDecorations<TChild>(this TChild target, ValueProxy<System.Windows.TextDecorationCollection>? value, Disambigator<System.Windows.Documents.Paragraph, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Paragraph{value?.SetValue(target, System.Windows.Documents.Paragraph.TextDecorationsProperty); return target;}
public static TChild WithTextDecorations<TChild>(this TChild target, ValueProxy<System.Windows.TextDecorationCollection>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.TextDecorationsProperty); return target;}
public static TChild WithTextDecorations<TChild>(this TChild target, ValueProxy<System.Windows.TextDecorationCollection>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.TextDecorationsProperty); return target;}
public static TChild WithTextDecorations<TChild>(this TChild target, ValueProxy<System.Windows.TextDecorationCollection>? value, Disambigator<System.Windows.Controls.TextBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBox{value?.SetValue(target, System.Windows.Controls.TextBox.TextDecorationsProperty); return target;}

//MarkerStyleProperty
public static TChild WithMarkerStyle<TChild>(this TChild target, ValueProxy<System.Windows.TextMarkerStyle>? value, Disambigator<System.Windows.Documents.List, TChild>? doNotUse = null) where TChild: System.Windows.Documents.List{value?.SetValue(target, System.Windows.Documents.List.MarkerStyleProperty); return target;}

//MarkerOffsetProperty
public static TChild WithMarkerOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.List, TChild>? doNotUse = null) where TChild: System.Windows.Documents.List{value?.SetValue(target, System.Windows.Documents.List.MarkerOffsetProperty); return target;}

//StartIndexProperty
public static TChild WithStartIndex<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Documents.List, TChild>? doNotUse = null) where TChild: System.Windows.Documents.List{value?.SetValue(target, System.Windows.Documents.List.StartIndexProperty); return target;}

//TextIndentProperty
public static TChild WithTextIndent<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.Paragraph, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Paragraph{value?.SetValue(target, System.Windows.Documents.Paragraph.TextIndentProperty); return target;}

//MinOrphanLinesProperty
public static TChild WithMinOrphanLines<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Documents.Paragraph, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Paragraph{value?.SetValue(target, System.Windows.Documents.Paragraph.MinOrphanLinesProperty); return target;}

//MinWidowLinesProperty
public static TChild WithMinWidowLines<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Documents.Paragraph, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Paragraph{value?.SetValue(target, System.Windows.Documents.Paragraph.MinWidowLinesProperty); return target;}

//KeepWithNextProperty
public static TChild WithKeepWithNext<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Documents.Paragraph, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Paragraph{value?.SetValue(target, System.Windows.Documents.Paragraph.KeepWithNextProperty); return target;}

//KeepTogetherProperty
public static TChild WithKeepTogether<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Documents.Paragraph, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Paragraph{value?.SetValue(target, System.Windows.Documents.Paragraph.KeepTogetherProperty); return target;}

//TextProperty
public static TChild WithText<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Documents.Run, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Run{value?.SetValue(target, System.Windows.Documents.Run.TextProperty); return target;}
public static TChild WithText<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.TextProperty); return target;}
public static TChild WithText<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.ComboBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ComboBox{value?.SetValue(target, System.Windows.Controls.ComboBox.TextProperty); return target;}
public static TChild WithText<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.DatePicker, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DatePicker{value?.SetValue(target, System.Windows.Controls.DatePicker.TextProperty); return target;}
public static TChild WithText<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.TextProperty); return target;}
public static TChild WithText<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.TextBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBox{value?.SetValue(target, System.Windows.Controls.TextBox.TextProperty); return target;}
public static TChild WithTextSearch_Text<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.TextSearch.TextProperty); return target;}

//CellSpacingProperty
public static TChild WithCellSpacing<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Documents.Table, TChild>? doNotUse = null) where TChild: System.Windows.Documents.Table{value?.SetValue(target, System.Windows.Documents.Table.CellSpacingProperty); return target;}

//ColumnSpanProperty
public static TChild WithColumnSpan<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Documents.TableCell, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TableCell{value?.SetValue(target, System.Windows.Documents.TableCell.ColumnSpanProperty); return target;}
public static TChild WithGrid_ColumnSpan<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.Grid.ColumnSpanProperty); return target;}

//RowSpanProperty
public static TChild WithRowSpan<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Documents.TableCell, TChild>? doNotUse = null) where TChild: System.Windows.Documents.TableCell{value?.SetValue(target, System.Windows.Documents.TableCell.RowSpanProperty); return target;}
public static TChild WithGrid_RowSpan<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.Grid.RowSpanProperty); return target;}

//StandardLigaturesProperty
public static TChild WithTypography_StandardLigatures<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StandardLigaturesProperty); return target;}

//ContextualLigaturesProperty
public static TChild WithTypography_ContextualLigatures<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.ContextualLigaturesProperty); return target;}

//DiscretionaryLigaturesProperty
public static TChild WithTypography_DiscretionaryLigatures<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.DiscretionaryLigaturesProperty); return target;}

//HistoricalLigaturesProperty
public static TChild WithTypography_HistoricalLigatures<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.HistoricalLigaturesProperty); return target;}

//AnnotationAlternatesProperty
public static TChild WithTypography_AnnotationAlternates<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.AnnotationAlternatesProperty); return target;}

//ContextualAlternatesProperty
public static TChild WithTypography_ContextualAlternates<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.ContextualAlternatesProperty); return target;}

//HistoricalFormsProperty
public static TChild WithTypography_HistoricalForms<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.HistoricalFormsProperty); return target;}

//KerningProperty
public static TChild WithTypography_Kerning<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.KerningProperty); return target;}

//CapitalSpacingProperty
public static TChild WithTypography_CapitalSpacing<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.CapitalSpacingProperty); return target;}

//CaseSensitiveFormsProperty
public static TChild WithTypography_CaseSensitiveForms<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.CaseSensitiveFormsProperty); return target;}

//StylisticSet1Property
public static TChild WithTypography_StylisticSet1<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet1Property); return target;}

//StylisticSet2Property
public static TChild WithTypography_StylisticSet2<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet2Property); return target;}

//StylisticSet3Property
public static TChild WithTypography_StylisticSet3<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet3Property); return target;}

//StylisticSet4Property
public static TChild WithTypography_StylisticSet4<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet4Property); return target;}

//StylisticSet5Property
public static TChild WithTypography_StylisticSet5<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet5Property); return target;}

//StylisticSet6Property
public static TChild WithTypography_StylisticSet6<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet6Property); return target;}

//StylisticSet7Property
public static TChild WithTypography_StylisticSet7<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet7Property); return target;}

//StylisticSet8Property
public static TChild WithTypography_StylisticSet8<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet8Property); return target;}

//StylisticSet9Property
public static TChild WithTypography_StylisticSet9<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet9Property); return target;}

//StylisticSet10Property
public static TChild WithTypography_StylisticSet10<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet10Property); return target;}

//StylisticSet11Property
public static TChild WithTypography_StylisticSet11<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet11Property); return target;}

//StylisticSet12Property
public static TChild WithTypography_StylisticSet12<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet12Property); return target;}

//StylisticSet13Property
public static TChild WithTypography_StylisticSet13<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet13Property); return target;}

//StylisticSet14Property
public static TChild WithTypography_StylisticSet14<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet14Property); return target;}

//StylisticSet15Property
public static TChild WithTypography_StylisticSet15<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet15Property); return target;}

//StylisticSet16Property
public static TChild WithTypography_StylisticSet16<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet16Property); return target;}

//StylisticSet17Property
public static TChild WithTypography_StylisticSet17<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet17Property); return target;}

//StylisticSet18Property
public static TChild WithTypography_StylisticSet18<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet18Property); return target;}

//StylisticSet19Property
public static TChild WithTypography_StylisticSet19<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet19Property); return target;}

//StylisticSet20Property
public static TChild WithTypography_StylisticSet20<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticSet20Property); return target;}

//FractionProperty
public static TChild WithTypography_Fraction<TChild>(this TChild target, ValueProxy<System.Windows.FontFraction>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.FractionProperty); return target;}

//SlashedZeroProperty
public static TChild WithTypography_SlashedZero<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.SlashedZeroProperty); return target;}

//MathematicalGreekProperty
public static TChild WithTypography_MathematicalGreek<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.MathematicalGreekProperty); return target;}

//EastAsianExpertFormsProperty
public static TChild WithTypography_EastAsianExpertForms<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.EastAsianExpertFormsProperty); return target;}

//VariantsProperty
public static TChild WithTypography_Variants<TChild>(this TChild target, ValueProxy<System.Windows.FontVariants>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.VariantsProperty); return target;}

//CapitalsProperty
public static TChild WithTypography_Capitals<TChild>(this TChild target, ValueProxy<System.Windows.FontCapitals>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.CapitalsProperty); return target;}

//NumeralStyleProperty
public static TChild WithTypography_NumeralStyle<TChild>(this TChild target, ValueProxy<System.Windows.FontNumeralStyle>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.NumeralStyleProperty); return target;}

//NumeralAlignmentProperty
public static TChild WithTypography_NumeralAlignment<TChild>(this TChild target, ValueProxy<System.Windows.FontNumeralAlignment>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.NumeralAlignmentProperty); return target;}

//EastAsianWidthsProperty
public static TChild WithTypography_EastAsianWidths<TChild>(this TChild target, ValueProxy<System.Windows.FontEastAsianWidths>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.EastAsianWidthsProperty); return target;}

//EastAsianLanguageProperty
public static TChild WithTypography_EastAsianLanguage<TChild>(this TChild target, ValueProxy<System.Windows.FontEastAsianLanguage>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.EastAsianLanguageProperty); return target;}

//StandardSwashesProperty
public static TChild WithTypography_StandardSwashes<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StandardSwashesProperty); return target;}

//ContextualSwashesProperty
public static TChild WithTypography_ContextualSwashes<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.ContextualSwashesProperty); return target;}

//StylisticAlternatesProperty
public static TChild WithTypography_StylisticAlternates<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Documents.Typography.StylisticAlternatesProperty); return target;}

//AuthorProperty
public static System.Windows.Controls.StickyNoteControl WithAuthor(this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.String>? value) {value?.SetValue(target, System.Windows.Controls.StickyNoteControl.AuthorProperty); return target;}

//IsExpandedProperty
public static System.Windows.Controls.StickyNoteControl WithIsExpanded(this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Controls.StickyNoteControl.IsExpandedProperty); return target;}
public static TChild WithIsExpanded<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Expander, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Expander{value?.SetValue(target, System.Windows.Controls.Expander.IsExpandedProperty); return target;}
public static TChild WithIsExpanded<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.TreeViewItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TreeViewItem{value?.SetValue(target, System.Windows.Controls.TreeViewItem.IsExpandedProperty); return target;}

//IsMouseOverAnchorProperty
public static System.Windows.Controls.StickyNoteControl WithIsMouseOverAnchor(this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Controls.StickyNoteControl.IsMouseOverAnchorProperty); return target;}

//CaptionFontFamilyProperty
public static System.Windows.Controls.StickyNoteControl WithCaptionFontFamily(this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Windows.Media.FontFamily>? value) {value?.SetValue(target, System.Windows.Controls.StickyNoteControl.CaptionFontFamilyProperty); return target;}

//CaptionFontSizeProperty
public static System.Windows.Controls.StickyNoteControl WithCaptionFontSize(this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Controls.StickyNoteControl.CaptionFontSizeProperty); return target;}

//CaptionFontStretchProperty
public static System.Windows.Controls.StickyNoteControl WithCaptionFontStretch(this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Windows.FontStretch>? value) {value?.SetValue(target, System.Windows.Controls.StickyNoteControl.CaptionFontStretchProperty); return target;}

//CaptionFontStyleProperty
public static System.Windows.Controls.StickyNoteControl WithCaptionFontStyle(this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Windows.FontStyle>? value) {value?.SetValue(target, System.Windows.Controls.StickyNoteControl.CaptionFontStyleProperty); return target;}

//CaptionFontWeightProperty
public static System.Windows.Controls.StickyNoteControl WithCaptionFontWeight(this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Windows.FontWeight>? value) {value?.SetValue(target, System.Windows.Controls.StickyNoteControl.CaptionFontWeightProperty); return target;}

//PenWidthProperty
public static System.Windows.Controls.StickyNoteControl WithPenWidth(this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Controls.StickyNoteControl.PenWidthProperty); return target;}

//StickyNoteTypeProperty
public static System.Windows.Controls.StickyNoteControl WithStickyNoteType(this System.Windows.Controls.StickyNoteControl target, ValueProxy<System.Windows.Controls.StickyNoteType>? value) {value?.SetValue(target, System.Windows.Controls.StickyNoteControl.StickyNoteTypeProperty); return target;}

//TextTrimmingProperty
public static TChild WithTextTrimming<TChild>(this TChild target, ValueProxy<System.Windows.TextTrimming>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.TextTrimmingProperty); return target;}
public static TChild WithTextTrimming<TChild>(this TChild target, ValueProxy<System.Windows.TextTrimming>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.TextTrimmingProperty); return target;}

//TextWrappingProperty
public static TChild WithTextWrapping<TChild>(this TChild target, ValueProxy<System.Windows.TextWrapping>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.TextWrappingProperty); return target;}
public static TChild WithTextWrapping<TChild>(this TChild target, ValueProxy<System.Windows.TextWrapping>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.TextWrappingProperty); return target;}
public static TChild WithTextWrapping<TChild>(this TChild target, ValueProxy<System.Windows.TextWrapping>? value, Disambigator<System.Windows.Controls.TextBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBox{value?.SetValue(target, System.Windows.Controls.TextBox.TextWrappingProperty); return target;}

//BaselineOffsetProperty
public static TChild WithBaselineOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.AccessText, TChild>? doNotUse = null) where TChild: System.Windows.Controls.AccessText{value?.SetValue(target, System.Windows.Controls.AccessText.BaselineOffsetProperty); return target;}
public static TChild WithTextBlock_BaselineOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.TextBlock.BaselineOffsetProperty); return target;}
public static TChild WithBaselineOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.TextBlock, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBlock{value?.SetValue(target, System.Windows.Controls.TextBlock.BaselineOffsetProperty); return target;}

//IsDefaultProperty
public static TChild WithIsDefault<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Button, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Button{value?.SetValue(target, System.Windows.Controls.Button.IsDefaultProperty); return target;}

//IsCancelProperty
public static TChild WithIsCancel<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Button, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Button{value?.SetValue(target, System.Windows.Controls.Button.IsCancelProperty); return target;}

//IsDefaultedProperty
public static TChild WithIsDefaulted<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Button, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Button{value?.SetValue(target, System.Windows.Controls.Button.IsDefaultedProperty); return target;}

//CalendarButtonStyleProperty
public static TChild WithCalendarButtonStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.Calendar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Calendar{value?.SetValue(target, System.Windows.Controls.Calendar.CalendarButtonStyleProperty); return target;}

//CalendarDayButtonStyleProperty
public static TChild WithCalendarDayButtonStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.Calendar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Calendar{value?.SetValue(target, System.Windows.Controls.Calendar.CalendarDayButtonStyleProperty); return target;}

//CalendarItemStyleProperty
public static TChild WithCalendarItemStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.Calendar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Calendar{value?.SetValue(target, System.Windows.Controls.Calendar.CalendarItemStyleProperty); return target;}

//DisplayDateProperty
public static TChild WithDisplayDate<TChild>(this TChild target, ValueProxy<System.DateTime>? value, Disambigator<System.Windows.Controls.Calendar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Calendar{value?.SetValue(target, System.Windows.Controls.Calendar.DisplayDateProperty); return target;}
public static TChild WithDisplayDate<TChild>(this TChild target, ValueProxy<System.DateTime>? value, Disambigator<System.Windows.Controls.DatePicker, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DatePicker{value?.SetValue(target, System.Windows.Controls.DatePicker.DisplayDateProperty); return target;}

//DisplayDateEndProperty
public static TChild WithDisplayDateEnd<TChild>(this TChild target, ValueProxy<System.Nullable<System.DateTime>>? value, Disambigator<System.Windows.Controls.Calendar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Calendar{value?.SetValue(target, System.Windows.Controls.Calendar.DisplayDateEndProperty); return target;}
public static TChild WithDisplayDateEnd<TChild>(this TChild target, ValueProxy<System.Nullable<System.DateTime>>? value, Disambigator<System.Windows.Controls.DatePicker, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DatePicker{value?.SetValue(target, System.Windows.Controls.DatePicker.DisplayDateEndProperty); return target;}

//DisplayDateStartProperty
public static TChild WithDisplayDateStart<TChild>(this TChild target, ValueProxy<System.Nullable<System.DateTime>>? value, Disambigator<System.Windows.Controls.Calendar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Calendar{value?.SetValue(target, System.Windows.Controls.Calendar.DisplayDateStartProperty); return target;}
public static TChild WithDisplayDateStart<TChild>(this TChild target, ValueProxy<System.Nullable<System.DateTime>>? value, Disambigator<System.Windows.Controls.DatePicker, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DatePicker{value?.SetValue(target, System.Windows.Controls.DatePicker.DisplayDateStartProperty); return target;}

//DisplayModeProperty
public static TChild WithDisplayMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.CalendarMode>? value, Disambigator<System.Windows.Controls.Calendar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Calendar{value?.SetValue(target, System.Windows.Controls.Calendar.DisplayModeProperty); return target;}

//FirstDayOfWeekProperty
public static TChild WithFirstDayOfWeek<TChild>(this TChild target, ValueProxy<System.DayOfWeek>? value, Disambigator<System.Windows.Controls.Calendar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Calendar{value?.SetValue(target, System.Windows.Controls.Calendar.FirstDayOfWeekProperty); return target;}
public static TChild WithFirstDayOfWeek<TChild>(this TChild target, ValueProxy<System.DayOfWeek>? value, Disambigator<System.Windows.Controls.DatePicker, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DatePicker{value?.SetValue(target, System.Windows.Controls.DatePicker.FirstDayOfWeekProperty); return target;}

//IsTodayHighlightedProperty
public static TChild WithIsTodayHighlighted<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Calendar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Calendar{value?.SetValue(target, System.Windows.Controls.Calendar.IsTodayHighlightedProperty); return target;}
public static TChild WithIsTodayHighlighted<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DatePicker, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DatePicker{value?.SetValue(target, System.Windows.Controls.DatePicker.IsTodayHighlightedProperty); return target;}

//SelectedDateProperty
public static TChild WithSelectedDate<TChild>(this TChild target, ValueProxy<System.Nullable<System.DateTime>>? value, Disambigator<System.Windows.Controls.Calendar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Calendar{value?.SetValue(target, System.Windows.Controls.Calendar.SelectedDateProperty); return target;}
public static TChild WithSelectedDate<TChild>(this TChild target, ValueProxy<System.Nullable<System.DateTime>>? value, Disambigator<System.Windows.Controls.DatePicker, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DatePicker{value?.SetValue(target, System.Windows.Controls.DatePicker.SelectedDateProperty); return target;}

//SelectionModeProperty
public static TChild WithSelectionMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.CalendarSelectionMode>? value, Disambigator<System.Windows.Controls.Calendar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Calendar{value?.SetValue(target, System.Windows.Controls.Calendar.SelectionModeProperty); return target;}
public static TChild WithSelectionMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataGridSelectionMode>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.SelectionModeProperty); return target;}
public static TChild WithSelectionMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.SelectionMode>? value, Disambigator<System.Windows.Controls.ListBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ListBox{value?.SetValue(target, System.Windows.Controls.ListBox.SelectionModeProperty); return target;}

//MaxDropDownHeightProperty
public static TChild WithMaxDropDownHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ComboBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ComboBox{value?.SetValue(target, System.Windows.Controls.ComboBox.MaxDropDownHeightProperty); return target;}

//IsDropDownOpenProperty
public static TChild WithIsDropDownOpen<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ComboBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ComboBox{value?.SetValue(target, System.Windows.Controls.ComboBox.IsDropDownOpenProperty); return target;}
public static TChild WithIsDropDownOpen<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DatePicker, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DatePicker{value?.SetValue(target, System.Windows.Controls.DatePicker.IsDropDownOpenProperty); return target;}

//ShouldPreserveUserEnteredPrefixProperty
public static TChild WithShouldPreserveUserEnteredPrefix<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ComboBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ComboBox{value?.SetValue(target, System.Windows.Controls.ComboBox.ShouldPreserveUserEnteredPrefixProperty); return target;}

//IsEditableProperty
public static TChild WithIsEditable<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ComboBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ComboBox{value?.SetValue(target, System.Windows.Controls.ComboBox.IsEditableProperty); return target;}

//IsReadOnlyProperty
public static TChild WithIsReadOnly<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ComboBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ComboBox{value?.SetValue(target, System.Windows.Controls.ComboBox.IsReadOnlyProperty); return target;}
public static TChild WithIsReadOnly<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.IsReadOnlyProperty); return target;}
public static TChild WithIsReadOnly<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGridCell, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridCell{value?.SetValue(target, System.Windows.Controls.DataGridCell.IsReadOnlyProperty); return target;}
public static TChild WithIsReadOnly<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.IsReadOnlyProperty); return target;}
public static TChild WithIsReadOnly<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.IsReadOnlyProperty); return target;}

//SelectionBoxItemProperty
public static TChild WithSelectionBoxItem<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.ComboBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ComboBox{value?.SetValue(target, System.Windows.Controls.ComboBox.SelectionBoxItemProperty); return target;}

//SelectionBoxItemTemplateProperty
public static TChild WithSelectionBoxItemTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.ComboBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ComboBox{value?.SetValue(target, System.Windows.Controls.ComboBox.SelectionBoxItemTemplateProperty); return target;}

//SelectionBoxItemStringFormatProperty
public static TChild WithSelectionBoxItemStringFormat<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.ComboBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ComboBox{value?.SetValue(target, System.Windows.Controls.ComboBox.SelectionBoxItemStringFormatProperty); return target;}

//StaysOpenOnEditProperty
public static TChild WithStaysOpenOnEdit<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ComboBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ComboBox{value?.SetValue(target, System.Windows.Controls.ComboBox.StaysOpenOnEditProperty); return target;}

//IsHighlightedProperty
public static TChild WithIsHighlighted<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ComboBoxItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ComboBoxItem{value?.SetValue(target, System.Windows.Controls.ComboBoxItem.IsHighlightedProperty); return target;}
public static TChild WithIsHighlighted<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.IsHighlightedProperty); return target;}
public static System.Windows.Controls.Primitives.CalendarDayButton WithIsHighlighted(this System.Windows.Controls.Primitives.CalendarDayButton target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Controls.Primitives.CalendarDayButton.IsHighlightedProperty); return target;}

//ContentProperty
public static TChild WithContent<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.ContentControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContentControl{value?.SetValue(target, System.Windows.Controls.ContentControl.ContentProperty); return target;}
public static TChild WithContent<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.ContentPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContentPresenter{value?.SetValue(target, System.Windows.Controls.ContentPresenter.ContentProperty); return target;}
public static TChild WithContent<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.GridViewRowPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewRowPresenter{value?.SetValue(target, System.Windows.Controls.GridViewRowPresenter.ContentProperty); return target;}
public static TChild WithContent<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.Page, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Page{value?.SetValue(target, System.Windows.Controls.Page.ContentProperty); return target;}
public static TChild WithContent<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Model3D>? value, Disambigator<System.Windows.Media.Media3D.ModelVisual3D, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.ModelVisual3D{value?.SetValue(target, System.Windows.Media.Media3D.ModelVisual3D.ContentProperty); return target;}

//HasContentProperty
public static TChild WithHasContent<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ContentControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContentControl{value?.SetValue(target, System.Windows.Controls.ContentControl.HasContentProperty); return target;}

//ContentTemplateProperty
public static TChild WithContentTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.ContentControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContentControl{value?.SetValue(target, System.Windows.Controls.ContentControl.ContentTemplateProperty); return target;}
public static TChild WithContentTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.ContentPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContentPresenter{value?.SetValue(target, System.Windows.Controls.ContentPresenter.ContentTemplateProperty); return target;}
public static TChild WithContentTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.TabControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TabControl{value?.SetValue(target, System.Windows.Controls.TabControl.ContentTemplateProperty); return target;}

//ContentTemplateSelectorProperty
public static TChild WithContentTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.ContentControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContentControl{value?.SetValue(target, System.Windows.Controls.ContentControl.ContentTemplateSelectorProperty); return target;}
public static TChild WithContentTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.ContentPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContentPresenter{value?.SetValue(target, System.Windows.Controls.ContentPresenter.ContentTemplateSelectorProperty); return target;}
public static TChild WithContentTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.TabControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TabControl{value?.SetValue(target, System.Windows.Controls.TabControl.ContentTemplateSelectorProperty); return target;}

//ContentStringFormatProperty
public static TChild WithContentStringFormat<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.ContentControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContentControl{value?.SetValue(target, System.Windows.Controls.ContentControl.ContentStringFormatProperty); return target;}
public static TChild WithContentStringFormat<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.ContentPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContentPresenter{value?.SetValue(target, System.Windows.Controls.ContentPresenter.ContentStringFormatProperty); return target;}
public static TChild WithContentStringFormat<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.TabControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TabControl{value?.SetValue(target, System.Windows.Controls.TabControl.ContentStringFormatProperty); return target;}

//RecognizesAccessKeyProperty
public static TChild WithRecognizesAccessKey<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ContentPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContentPresenter{value?.SetValue(target, System.Windows.Controls.ContentPresenter.RecognizesAccessKeyProperty); return target;}

//ContentSourceProperty
public static TChild WithContentSource<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.ContentPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContentPresenter{value?.SetValue(target, System.Windows.Controls.ContentPresenter.ContentSourceProperty); return target;}

//IsOpenProperty
public static TChild WithIsOpen<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ContextMenu, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContextMenu{value?.SetValue(target, System.Windows.Controls.ContextMenu.IsOpenProperty); return target;}
public static TChild WithIsOpen<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ToolTip, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolTip{value?.SetValue(target, System.Windows.Controls.ToolTip.IsOpenProperty); return target;}
public static TChild WithIsOpen<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.Popup, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Popup{value?.SetValue(target, System.Windows.Controls.Primitives.Popup.IsOpenProperty); return target;}

//PlacementTargetProperty
public static TChild WithPlacementTarget<TChild>(this TChild target, ValueProxy<System.Windows.UIElement>? value, Disambigator<System.Windows.Controls.ContextMenu, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContextMenu{value?.SetValue(target, System.Windows.Controls.ContextMenu.PlacementTargetProperty); return target;}
public static TChild WithContextMenuService_PlacementTarget<TChild>(this TChild target, ValueProxy<System.Windows.UIElement>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ContextMenuService.PlacementTargetProperty); return target;}
public static TChild WithPlacementTarget<TChild>(this TChild target, ValueProxy<System.Windows.UIElement>? value, Disambigator<System.Windows.Controls.ToolTip, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolTip{value?.SetValue(target, System.Windows.Controls.ToolTip.PlacementTargetProperty); return target;}
public static TChild WithToolTipService_PlacementTarget<TChild>(this TChild target, ValueProxy<System.Windows.UIElement>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolTipService.PlacementTargetProperty); return target;}
public static TChild WithPlacementTarget<TChild>(this TChild target, ValueProxy<System.Windows.UIElement>? value, Disambigator<System.Windows.Controls.Primitives.Popup, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Popup{value?.SetValue(target, System.Windows.Controls.Primitives.Popup.PlacementTargetProperty); return target;}

//PlacementRectangleProperty
public static TChild WithPlacementRectangle<TChild>(this TChild target, ValueProxy<System.Windows.Rect>? value, Disambigator<System.Windows.Controls.ContextMenu, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContextMenu{value?.SetValue(target, System.Windows.Controls.ContextMenu.PlacementRectangleProperty); return target;}
public static TChild WithContextMenuService_PlacementRectangle<TChild>(this TChild target, ValueProxy<System.Windows.Rect>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ContextMenuService.PlacementRectangleProperty); return target;}
public static TChild WithPlacementRectangle<TChild>(this TChild target, ValueProxy<System.Windows.Rect>? value, Disambigator<System.Windows.Controls.ToolTip, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolTip{value?.SetValue(target, System.Windows.Controls.ToolTip.PlacementRectangleProperty); return target;}
public static TChild WithToolTipService_PlacementRectangle<TChild>(this TChild target, ValueProxy<System.Windows.Rect>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolTipService.PlacementRectangleProperty); return target;}
public static TChild WithPlacementRectangle<TChild>(this TChild target, ValueProxy<System.Windows.Rect>? value, Disambigator<System.Windows.Controls.Primitives.Popup, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Popup{value?.SetValue(target, System.Windows.Controls.Primitives.Popup.PlacementRectangleProperty); return target;}

//PlacementProperty
public static TChild WithPlacement<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Primitives.PlacementMode>? value, Disambigator<System.Windows.Controls.ContextMenu, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContextMenu{value?.SetValue(target, System.Windows.Controls.ContextMenu.PlacementProperty); return target;}
public static TChild WithContextMenuService_Placement<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Primitives.PlacementMode>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ContextMenuService.PlacementProperty); return target;}
public static TChild WithPlacement<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Primitives.PlacementMode>? value, Disambigator<System.Windows.Controls.ToolTip, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolTip{value?.SetValue(target, System.Windows.Controls.ToolTip.PlacementProperty); return target;}
public static TChild WithToolTipService_Placement<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Primitives.PlacementMode>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolTipService.PlacementProperty); return target;}
public static TChild WithPlacement<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Primitives.PlacementMode>? value, Disambigator<System.Windows.Controls.Primitives.Popup, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Popup{value?.SetValue(target, System.Windows.Controls.Primitives.Popup.PlacementProperty); return target;}
public static TChild WithPlacement<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Primitives.TickBarPlacement>? value, Disambigator<System.Windows.Controls.Primitives.TickBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TickBar{value?.SetValue(target, System.Windows.Controls.Primitives.TickBar.PlacementProperty); return target;}

//HasDropShadowProperty
public static TChild WithHasDropShadow<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ContextMenu, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContextMenu{value?.SetValue(target, System.Windows.Controls.ContextMenu.HasDropShadowProperty); return target;}
public static TChild WithContextMenuService_HasDropShadow<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ContextMenuService.HasDropShadowProperty); return target;}
public static TChild WithHasDropShadow<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ToolTip, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolTip{value?.SetValue(target, System.Windows.Controls.ToolTip.HasDropShadowProperty); return target;}
public static TChild WithToolTipService_HasDropShadow<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolTipService.HasDropShadowProperty); return target;}
public static TChild WithHasDropShadow<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.Popup, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Popup{value?.SetValue(target, System.Windows.Controls.Primitives.Popup.HasDropShadowProperty); return target;}

//CustomPopupPlacementCallbackProperty
public static TChild WithCustomPopupPlacementCallback<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Primitives.CustomPopupPlacementCallback>? value, Disambigator<System.Windows.Controls.ContextMenu, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContextMenu{value?.SetValue(target, System.Windows.Controls.ContextMenu.CustomPopupPlacementCallbackProperty); return target;}
public static TChild WithCustomPopupPlacementCallback<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Primitives.CustomPopupPlacementCallback>? value, Disambigator<System.Windows.Controls.ToolTip, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolTip{value?.SetValue(target, System.Windows.Controls.ToolTip.CustomPopupPlacementCallbackProperty); return target;}
public static TChild WithCustomPopupPlacementCallback<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Primitives.CustomPopupPlacementCallback>? value, Disambigator<System.Windows.Controls.Primitives.Popup, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Popup{value?.SetValue(target, System.Windows.Controls.Primitives.Popup.CustomPopupPlacementCallbackProperty); return target;}

//StaysOpenProperty
public static TChild WithStaysOpen<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ContextMenu, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ContextMenu{value?.SetValue(target, System.Windows.Controls.ContextMenu.StaysOpenProperty); return target;}
public static TChild WithStaysOpen<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ToolTip, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolTip{value?.SetValue(target, System.Windows.Controls.ToolTip.StaysOpenProperty); return target;}
public static TChild WithStaysOpen<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.Popup, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Popup{value?.SetValue(target, System.Windows.Controls.Primitives.Popup.StaysOpenProperty); return target;}

//ShowOnDisabledProperty
public static TChild WithContextMenuService_ShowOnDisabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ContextMenuService.ShowOnDisabledProperty); return target;}
public static TChild WithToolTipService_ShowOnDisabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolTipService.ShowOnDisabledProperty); return target;}

//HorizontalContentAlignmentProperty
public static TChild WithHorizontalContentAlignment<TChild>(this TChild target, ValueProxy<System.Windows.HorizontalAlignment>? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.HorizontalContentAlignmentProperty); return target;}

//VerticalContentAlignmentProperty
public static TChild WithVerticalContentAlignment<TChild>(this TChild target, ValueProxy<System.Windows.VerticalAlignment>? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.VerticalContentAlignmentProperty); return target;}

//TemplateProperty
public static TChild WithTemplate<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ControlTemplate>? value, Disambigator<System.Windows.Controls.Control, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Control{value?.SetValue(target, System.Windows.Controls.Control.TemplateProperty); return target;}
public static TChild WithTemplate<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ControlTemplate>? value, Disambigator<System.Windows.Controls.Page, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Page{value?.SetValue(target, System.Windows.Controls.Page.TemplateProperty); return target;}

//CanUserResizeColumnsProperty
public static TChild WithCanUserResizeColumns<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.CanUserResizeColumnsProperty); return target;}

//MinColumnWidthProperty
public static TChild WithMinColumnWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.MinColumnWidthProperty); return target;}

//MaxColumnWidthProperty
public static TChild WithMaxColumnWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.MaxColumnWidthProperty); return target;}

//GridLinesVisibilityProperty
public static TChild WithGridLinesVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataGridGridLinesVisibility>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.GridLinesVisibilityProperty); return target;}

//HorizontalGridLinesBrushProperty
public static TChild WithHorizontalGridLinesBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.HorizontalGridLinesBrushProperty); return target;}

//VerticalGridLinesBrushProperty
public static TChild WithVerticalGridLinesBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.VerticalGridLinesBrushProperty); return target;}

//RowStyleProperty
public static TChild WithRowStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.RowStyleProperty); return target;}

//RowValidationErrorTemplateProperty
public static TChild WithRowValidationErrorTemplate<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ControlTemplate>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.RowValidationErrorTemplateProperty); return target;}

//RowStyleSelectorProperty
public static TChild WithRowStyleSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.StyleSelector>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.RowStyleSelectorProperty); return target;}

//RowBackgroundProperty
public static TChild WithRowBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.RowBackgroundProperty); return target;}

//AlternatingRowBackgroundProperty
public static TChild WithAlternatingRowBackground<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.AlternatingRowBackgroundProperty); return target;}

//RowHeightProperty
public static TChild WithRowHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.RowHeightProperty); return target;}

//MinRowHeightProperty
public static TChild WithMinRowHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.MinRowHeightProperty); return target;}

//RowHeaderWidthProperty
public static TChild WithRowHeaderWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.RowHeaderWidthProperty); return target;}

//RowHeaderActualWidthProperty
public static TChild WithRowHeaderActualWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.RowHeaderActualWidthProperty); return target;}

//ColumnHeaderHeightProperty
public static TChild WithColumnHeaderHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.ColumnHeaderHeightProperty); return target;}

//HeadersVisibilityProperty
public static TChild WithHeadersVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataGridHeadersVisibility>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.HeadersVisibilityProperty); return target;}

//CellStyleProperty
public static TChild WithCellStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.CellStyleProperty); return target;}
public static TChild WithCellStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.CellStyleProperty); return target;}

//ColumnHeaderStyleProperty
public static TChild WithColumnHeaderStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.ColumnHeaderStyleProperty); return target;}

//RowHeaderStyleProperty
public static TChild WithRowHeaderStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.RowHeaderStyleProperty); return target;}

//RowHeaderTemplateProperty
public static TChild WithRowHeaderTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.RowHeaderTemplateProperty); return target;}

//RowHeaderTemplateSelectorProperty
public static TChild WithRowHeaderTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.RowHeaderTemplateSelectorProperty); return target;}

//HorizontalScrollBarVisibilityProperty
public static TChild WithHorizontalScrollBarVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ScrollBarVisibility>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.HorizontalScrollBarVisibilityProperty); return target;}
public static TChild WithHorizontalScrollBarVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ScrollBarVisibility>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.HorizontalScrollBarVisibilityProperty); return target;}
public static TChild WithScrollViewer_HorizontalScrollBarVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ScrollBarVisibility>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ScrollViewer.HorizontalScrollBarVisibilityProperty); return target;}
public static TChild WithHorizontalScrollBarVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ScrollBarVisibility>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.HorizontalScrollBarVisibilityProperty); return target;}
public static TChild WithHorizontalScrollBarVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ScrollBarVisibility>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.HorizontalScrollBarVisibilityProperty); return target;}

//VerticalScrollBarVisibilityProperty
public static TChild WithVerticalScrollBarVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ScrollBarVisibility>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.VerticalScrollBarVisibilityProperty); return target;}
public static TChild WithVerticalScrollBarVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ScrollBarVisibility>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.VerticalScrollBarVisibilityProperty); return target;}
public static TChild WithScrollViewer_VerticalScrollBarVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ScrollBarVisibility>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ScrollViewer.VerticalScrollBarVisibilityProperty); return target;}
public static TChild WithVerticalScrollBarVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ScrollBarVisibility>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.VerticalScrollBarVisibilityProperty); return target;}
public static TChild WithVerticalScrollBarVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ScrollBarVisibility>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.VerticalScrollBarVisibilityProperty); return target;}

//CurrentItemProperty
public static TChild WithCurrentItem<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.CurrentItemProperty); return target;}

//CurrentColumnProperty
public static TChild WithCurrentColumn<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataGridColumn>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.CurrentColumnProperty); return target;}

//CurrentCellProperty
public static TChild WithCurrentCell<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataGridCellInfo>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.CurrentCellProperty); return target;}

//CanUserAddRowsProperty
public static TChild WithCanUserAddRows<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.CanUserAddRowsProperty); return target;}

//CanUserDeleteRowsProperty
public static TChild WithCanUserDeleteRows<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.CanUserDeleteRowsProperty); return target;}

//RowDetailsVisibilityModeProperty
public static TChild WithRowDetailsVisibilityMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataGridRowDetailsVisibilityMode>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.RowDetailsVisibilityModeProperty); return target;}

//AreRowDetailsFrozenProperty
public static TChild WithAreRowDetailsFrozen<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.AreRowDetailsFrozenProperty); return target;}

//RowDetailsTemplateProperty
public static TChild WithRowDetailsTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.RowDetailsTemplateProperty); return target;}

//RowDetailsTemplateSelectorProperty
public static TChild WithRowDetailsTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.RowDetailsTemplateSelectorProperty); return target;}

//CanUserResizeRowsProperty
public static TChild WithCanUserResizeRows<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.CanUserResizeRowsProperty); return target;}

//NewItemMarginProperty
public static TChild WithNewItemMargin<TChild>(this TChild target, ThicknessValueProxy? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.NewItemMarginProperty); return target;}

//SelectionUnitProperty
public static TChild WithSelectionUnit<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataGridSelectionUnit>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.SelectionUnitProperty); return target;}

//CanUserSortColumnsProperty
public static TChild WithCanUserSortColumns<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.CanUserSortColumnsProperty); return target;}

//AutoGenerateColumnsProperty
public static TChild WithAutoGenerateColumns<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.AutoGenerateColumnsProperty); return target;}

//FrozenColumnCountProperty
public static TChild WithFrozenColumnCount<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.FrozenColumnCountProperty); return target;}


//NonFrozenColumnsViewportHorizontalOffsetProperty
public static TChild WithNonFrozenColumnsViewportHorizontalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.NonFrozenColumnsViewportHorizontalOffsetProperty); return target;}

//EnableRowVirtualizationProperty
public static TChild WithEnableRowVirtualization<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.EnableRowVirtualizationProperty); return target;}

//EnableColumnVirtualizationProperty
public static TChild WithEnableColumnVirtualization<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.EnableColumnVirtualizationProperty); return target;}

//CanUserReorderColumnsProperty
public static TChild WithCanUserReorderColumns<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.CanUserReorderColumnsProperty); return target;}

//DragIndicatorStyleProperty
public static TChild WithDragIndicatorStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.DragIndicatorStyleProperty); return target;}
public static TChild WithDragIndicatorStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.DragIndicatorStyleProperty); return target;}

//DropLocationIndicatorStyleProperty
public static TChild WithDropLocationIndicatorStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.DropLocationIndicatorStyleProperty); return target;}

//ClipboardCopyModeProperty
public static TChild WithClipboardCopyMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataGridClipboardCopyMode>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.ClipboardCopyModeProperty); return target;}

//CellsPanelHorizontalOffsetProperty
public static TChild WithCellsPanelHorizontalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DataGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGrid{value?.SetValue(target, System.Windows.Controls.DataGrid.CellsPanelHorizontalOffsetProperty); return target;}

//ElementStyleProperty
public static TChild WithElementStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGridBoundColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridBoundColumn{value?.SetValue(target, System.Windows.Controls.DataGridBoundColumn.ElementStyleProperty); return target;}
public static TChild WithElementStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGridComboBoxColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridComboBoxColumn{value?.SetValue(target, System.Windows.Controls.DataGridComboBoxColumn.ElementStyleProperty); return target;}

//EditingElementStyleProperty
public static TChild WithEditingElementStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGridBoundColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridBoundColumn{value?.SetValue(target, System.Windows.Controls.DataGridBoundColumn.EditingElementStyleProperty); return target;}
public static TChild WithEditingElementStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGridComboBoxColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridComboBoxColumn{value?.SetValue(target, System.Windows.Controls.DataGridComboBoxColumn.EditingElementStyleProperty); return target;}

//ColumnProperty
public static TChild WithColumn<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataGridColumn>? value, Disambigator<System.Windows.Controls.DataGridCell, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridCell{value?.SetValue(target, System.Windows.Controls.DataGridCell.ColumnProperty); return target;}
public static TChild WithGrid_Column<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.Grid.ColumnProperty); return target;}
public static TChild WithColumn<TChild>(this TChild target, ValueProxy<System.Windows.Controls.GridViewColumn>? value, Disambigator<System.Windows.Controls.GridViewColumnHeader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewColumnHeader{value?.SetValue(target, System.Windows.Controls.GridViewColumnHeader.ColumnProperty); return target;}

//IsEditingProperty
public static TChild WithIsEditing<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGridCell, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridCell{value?.SetValue(target, System.Windows.Controls.DataGridCell.IsEditingProperty); return target;}
public static TChild WithIsEditing<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.IsEditingProperty); return target;}

//IsSelectedProperty
public static TChild WithIsSelected<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGridCell, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridCell{value?.SetValue(target, System.Windows.Controls.DataGridCell.IsSelectedProperty); return target;}
public static TChild WithIsSelected<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.IsSelectedProperty); return target;}
public static TChild WithIsSelected<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ListBoxItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ListBoxItem{value?.SetValue(target, System.Windows.Controls.ListBoxItem.IsSelectedProperty); return target;}
public static TChild WithIsSelected<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.TabItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TabItem{value?.SetValue(target, System.Windows.Controls.TabItem.IsSelectedProperty); return target;}
public static TChild WithIsSelected<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.TreeViewItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TreeViewItem{value?.SetValue(target, System.Windows.Controls.TreeViewItem.IsSelectedProperty); return target;}
public static System.Windows.Controls.Primitives.CalendarDayButton WithIsSelected(this System.Windows.Controls.Primitives.CalendarDayButton target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Controls.Primitives.CalendarDayButton.IsSelectedProperty); return target;}
public static TChild WithSelector_IsSelected<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.Primitives.Selector.IsSelectedProperty); return target;}

//IsThreeStateProperty
public static TChild WithIsThreeState<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGridCheckBoxColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridCheckBoxColumn{value?.SetValue(target, System.Windows.Controls.DataGridCheckBoxColumn.IsThreeStateProperty); return target;}
public static TChild WithIsThreeState<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.ToggleButton, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.ToggleButton{value?.SetValue(target, System.Windows.Controls.Primitives.ToggleButton.IsThreeStateProperty); return target;}

//HeaderProperty
public static TChild WithHeader<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.HeaderProperty); return target;}
public static TChild WithHeader<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.HeaderProperty); return target;}
public static TChild WithHeader<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.GridViewColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewColumn{value?.SetValue(target, System.Windows.Controls.GridViewColumn.HeaderProperty); return target;}
public static TChild WithHeader<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.HeaderedContentControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.HeaderedContentControl{value?.SetValue(target, System.Windows.Controls.HeaderedContentControl.HeaderProperty); return target;}
public static TChild WithHeader<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.HeaderedItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.HeaderedItemsControl{value?.SetValue(target, System.Windows.Controls.HeaderedItemsControl.HeaderProperty); return target;}

//HeaderStyleProperty
public static TChild WithHeaderStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.HeaderStyleProperty); return target;}
public static TChild WithHeaderStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.HeaderStyleProperty); return target;}

//HeaderStringFormatProperty
public static TChild WithHeaderStringFormat<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.HeaderStringFormatProperty); return target;}
public static TChild WithHeaderStringFormat<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.GridViewColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewColumn{value?.SetValue(target, System.Windows.Controls.GridViewColumn.HeaderStringFormatProperty); return target;}
public static TChild WithHeaderStringFormat<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.HeaderedContentControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.HeaderedContentControl{value?.SetValue(target, System.Windows.Controls.HeaderedContentControl.HeaderStringFormatProperty); return target;}
public static TChild WithHeaderStringFormat<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.HeaderedItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.HeaderedItemsControl{value?.SetValue(target, System.Windows.Controls.HeaderedItemsControl.HeaderStringFormatProperty); return target;}

//HeaderTemplateProperty
public static TChild WithHeaderTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.HeaderTemplateProperty); return target;}
public static TChild WithHeaderTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.HeaderTemplateProperty); return target;}
public static TChild WithHeaderTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.GridViewColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewColumn{value?.SetValue(target, System.Windows.Controls.GridViewColumn.HeaderTemplateProperty); return target;}
public static TChild WithHeaderTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.HeaderedContentControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.HeaderedContentControl{value?.SetValue(target, System.Windows.Controls.HeaderedContentControl.HeaderTemplateProperty); return target;}
public static TChild WithHeaderTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.HeaderedItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.HeaderedItemsControl{value?.SetValue(target, System.Windows.Controls.HeaderedItemsControl.HeaderTemplateProperty); return target;}

//HeaderTemplateSelectorProperty
public static TChild WithHeaderTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.HeaderTemplateSelectorProperty); return target;}
public static TChild WithHeaderTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.HeaderTemplateSelectorProperty); return target;}
public static TChild WithHeaderTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.GridViewColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewColumn{value?.SetValue(target, System.Windows.Controls.GridViewColumn.HeaderTemplateSelectorProperty); return target;}
public static TChild WithHeaderTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.HeaderedContentControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.HeaderedContentControl{value?.SetValue(target, System.Windows.Controls.HeaderedContentControl.HeaderTemplateSelectorProperty); return target;}
public static TChild WithHeaderTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.HeaderedItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.HeaderedItemsControl{value?.SetValue(target, System.Windows.Controls.HeaderedItemsControl.HeaderTemplateSelectorProperty); return target;}

//DisplayIndexProperty
public static TChild WithDisplayIndex<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.DisplayIndexProperty); return target;}
public static TChild WithDisplayIndex<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Primitives.DataGridColumnHeader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DataGridColumnHeader{value?.SetValue(target, System.Windows.Controls.Primitives.DataGridColumnHeader.DisplayIndexProperty); return target;}

//SortMemberPathProperty
public static TChild WithSortMemberPath<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.SortMemberPathProperty); return target;}

//CanUserSortProperty
public static TChild WithCanUserSort<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.CanUserSortProperty); return target;}
public static TChild WithCanUserSort<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.DataGridColumnHeader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DataGridColumnHeader{value?.SetValue(target, System.Windows.Controls.Primitives.DataGridColumnHeader.CanUserSortProperty); return target;}

//SortDirectionProperty
public static TChild WithSortDirection<TChild>(this TChild target, ValueProxy<System.Nullable<System.ComponentModel.ListSortDirection>>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.SortDirectionProperty); return target;}
public static TChild WithSortDirection<TChild>(this TChild target, ValueProxy<System.Nullable<System.ComponentModel.ListSortDirection>>? value, Disambigator<System.Windows.Controls.Primitives.DataGridColumnHeader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DataGridColumnHeader{value?.SetValue(target, System.Windows.Controls.Primitives.DataGridColumnHeader.SortDirectionProperty); return target;}

//IsAutoGeneratedProperty
public static TChild WithIsAutoGenerated<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.IsAutoGeneratedProperty); return target;}

//IsFrozenProperty
public static TChild WithIsFrozen<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.IsFrozenProperty); return target;}
public static TChild WithIsFrozen<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.DataGridColumnHeader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DataGridColumnHeader{value?.SetValue(target, System.Windows.Controls.Primitives.DataGridColumnHeader.IsFrozenProperty); return target;}

//CanUserReorderProperty
public static TChild WithCanUserReorder<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.CanUserReorderProperty); return target;}

//CanUserResizeProperty
public static TChild WithCanUserResize<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGridColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridColumn{value?.SetValue(target, System.Windows.Controls.DataGridColumn.CanUserResizeProperty); return target;}

//ItemsSourceProperty
public static TChild WithItemsSource<TChild>(this TChild target, ValueProxy<System.Collections.IEnumerable>? value, Disambigator<System.Windows.Controls.DataGridComboBoxColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridComboBoxColumn{value?.SetValue(target, System.Windows.Controls.DataGridComboBoxColumn.ItemsSourceProperty); return target;}
public static TChild WithItemsSource<TChild>(this TChild target, ValueProxy<System.Collections.IEnumerable>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.ItemsSourceProperty); return target;}

//DisplayMemberPathProperty
public static TChild WithDisplayMemberPath<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.DataGridComboBoxColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridComboBoxColumn{value?.SetValue(target, System.Windows.Controls.DataGridComboBoxColumn.DisplayMemberPathProperty); return target;}
public static TChild WithDisplayMemberPath<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.DisplayMemberPathProperty); return target;}

//SelectedValuePathProperty
public static TChild WithSelectedValuePath<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.DataGridComboBoxColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridComboBoxColumn{value?.SetValue(target, System.Windows.Controls.DataGridComboBoxColumn.SelectedValuePathProperty); return target;}
public static TChild WithSelectedValuePath<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.TreeView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TreeView{value?.SetValue(target, System.Windows.Controls.TreeView.SelectedValuePathProperty); return target;}
public static TChild WithSelectedValuePath<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.Primitives.Selector, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Selector{value?.SetValue(target, System.Windows.Controls.Primitives.Selector.SelectedValuePathProperty); return target;}

//ItemProperty
public static TChild WithItem<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.ItemProperty); return target;}

//ItemsPanelProperty
public static TChild WithItemsPanel<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ItemsPanelTemplate>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.ItemsPanelProperty); return target;}
public static TChild WithItemsPanel<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ItemsPanelTemplate>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.ItemsPanelProperty); return target;}

//ValidationErrorTemplateProperty
public static TChild WithValidationErrorTemplate<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ControlTemplate>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.ValidationErrorTemplateProperty); return target;}

//DetailsTemplateProperty
public static TChild WithDetailsTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.DetailsTemplateProperty); return target;}

//DetailsTemplateSelectorProperty
public static TChild WithDetailsTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.DetailsTemplateSelectorProperty); return target;}

//DetailsVisibilityProperty
public static TChild WithDetailsVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Visibility>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.DetailsVisibilityProperty); return target;}

//AlternationIndexProperty
public static TChild WithAlternationIndex<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.AlternationIndexProperty); return target;}

//IsNewItemProperty
public static TChild WithIsNewItem<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DataGridRow, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridRow{value?.SetValue(target, System.Windows.Controls.DataGridRow.IsNewItemProperty); return target;}

//CellTemplateProperty
public static TChild WithCellTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.DataGridTemplateColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridTemplateColumn{value?.SetValue(target, System.Windows.Controls.DataGridTemplateColumn.CellTemplateProperty); return target;}
public static TChild WithCellTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.GridViewColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewColumn{value?.SetValue(target, System.Windows.Controls.GridViewColumn.CellTemplateProperty); return target;}

//CellTemplateSelectorProperty
public static TChild WithCellTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.DataGridTemplateColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridTemplateColumn{value?.SetValue(target, System.Windows.Controls.DataGridTemplateColumn.CellTemplateSelectorProperty); return target;}
public static TChild WithCellTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.GridViewColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewColumn{value?.SetValue(target, System.Windows.Controls.GridViewColumn.CellTemplateSelectorProperty); return target;}

//CellEditingTemplateProperty
public static TChild WithCellEditingTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.DataGridTemplateColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridTemplateColumn{value?.SetValue(target, System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplateProperty); return target;}

//CellEditingTemplateSelectorProperty
public static TChild WithCellEditingTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.DataGridTemplateColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DataGridTemplateColumn{value?.SetValue(target, System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplateSelectorProperty); return target;}

//CalendarStyleProperty
public static TChild WithCalendarStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.DatePicker, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DatePicker{value?.SetValue(target, System.Windows.Controls.DatePicker.CalendarStyleProperty); return target;}

//SelectedDateFormatProperty
public static TChild WithSelectedDateFormat<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DatePickerFormat>? value, Disambigator<System.Windows.Controls.DatePicker, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DatePicker{value?.SetValue(target, System.Windows.Controls.DatePicker.SelectedDateFormatProperty); return target;}

//SharedSizeGroupProperty
public static TChild WithSharedSizeGroup<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.DefinitionBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DefinitionBase{value?.SetValue(target, System.Windows.Controls.DefinitionBase.SharedSizeGroupProperty); return target;}

//LastChildFillProperty
public static TChild WithLastChildFill<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DockPanel, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DockPanel{value?.SetValue(target, System.Windows.Controls.DockPanel.LastChildFillProperty); return target;}

//DockProperty
public static TChild WithDockPanel_Dock<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Dock>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.DockPanel.DockProperty); return target;}

//ExtentWidthProperty
public static TChild WithExtentWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.ExtentWidthProperty); return target;}
public static TChild WithExtentWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.ExtentWidthProperty); return target;}

//ExtentHeightProperty
public static TChild WithExtentHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.ExtentHeightProperty); return target;}
public static TChild WithExtentHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.ExtentHeightProperty); return target;}

//ViewportWidthProperty
public static TChild WithViewportWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.ViewportWidthProperty); return target;}
public static TChild WithViewportWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.ViewportWidthProperty); return target;}

//ViewportHeightProperty
public static TChild WithViewportHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.ViewportHeightProperty); return target;}
public static TChild WithViewportHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.ViewportHeightProperty); return target;}

//ShowPageBordersProperty
public static TChild WithShowPageBorders<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.ShowPageBordersProperty); return target;}

//ZoomProperty
public static TChild WithZoom<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.ZoomProperty); return target;}
public static TChild WithZoom<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.ZoomProperty); return target;}
public static TChild WithZoom<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.ZoomProperty); return target;}
public static TChild WithZoom<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentPageViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentPageViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.ZoomProperty); return target;}

//MaxPagesAcrossProperty
public static TChild WithMaxPagesAcross<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.MaxPagesAcrossProperty); return target;}

//VerticalPageSpacingProperty
public static TChild WithVerticalPageSpacing<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.VerticalPageSpacingProperty); return target;}

//HorizontalPageSpacingProperty
public static TChild WithHorizontalPageSpacing<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.HorizontalPageSpacingProperty); return target;}

//CanMoveUpProperty
public static TChild WithCanMoveUp<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.CanMoveUpProperty); return target;}

//CanMoveDownProperty
public static TChild WithCanMoveDown<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.CanMoveDownProperty); return target;}

//CanMoveLeftProperty
public static TChild WithCanMoveLeft<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.CanMoveLeftProperty); return target;}

//CanMoveRightProperty
public static TChild WithCanMoveRight<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.CanMoveRightProperty); return target;}

//CanIncreaseZoomProperty
public static TChild WithCanIncreaseZoom<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.CanIncreaseZoomProperty); return target;}
public static TChild WithCanIncreaseZoom<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.CanIncreaseZoomProperty); return target;}
public static TChild WithCanIncreaseZoom<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.CanIncreaseZoomProperty); return target;}
public static TChild WithCanIncreaseZoom<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentPageViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentPageViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.CanIncreaseZoomProperty); return target;}

//CanDecreaseZoomProperty
public static TChild WithCanDecreaseZoom<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.DocumentViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.DocumentViewer{value?.SetValue(target, System.Windows.Controls.DocumentViewer.CanDecreaseZoomProperty); return target;}
public static TChild WithCanDecreaseZoom<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.CanDecreaseZoomProperty); return target;}
public static TChild WithCanDecreaseZoom<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.CanDecreaseZoomProperty); return target;}
public static TChild WithCanDecreaseZoom<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentPageViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentPageViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.CanDecreaseZoomProperty); return target;}

//ExpandDirectionProperty
public static TChild WithExpandDirection<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ExpandDirection>? value, Disambigator<System.Windows.Controls.Expander, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Expander{value?.SetValue(target, System.Windows.Controls.Expander.ExpandDirectionProperty); return target;}

//ViewingModeProperty
public static TChild WithViewingMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.FlowDocumentReaderViewingMode>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.ViewingModeProperty); return target;}

//IsPageViewEnabledProperty
public static TChild WithIsPageViewEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.IsPageViewEnabledProperty); return target;}

//IsTwoPageViewEnabledProperty
public static TChild WithIsTwoPageViewEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.IsTwoPageViewEnabledProperty); return target;}

//IsScrollViewEnabledProperty
public static TChild WithIsScrollViewEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.IsScrollViewEnabledProperty); return target;}

//PageCountProperty
public static TChild WithPageCount<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.PageCountProperty); return target;}
public static TChild WithPageCount<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Primitives.DocumentViewerBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DocumentViewerBase{value?.SetValue(target, System.Windows.Controls.Primitives.DocumentViewerBase.PageCountProperty); return target;}

//PageNumberProperty
public static TChild WithPageNumber<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.PageNumberProperty); return target;}
public static TChild WithPageNumber<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Primitives.DocumentPageView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DocumentPageView{value?.SetValue(target, System.Windows.Controls.Primitives.DocumentPageView.PageNumberProperty); return target;}

//CanGoToPreviousPageProperty
public static TChild WithCanGoToPreviousPage<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.CanGoToPreviousPageProperty); return target;}
public static TChild WithCanGoToPreviousPage<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.DocumentViewerBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DocumentViewerBase{value?.SetValue(target, System.Windows.Controls.Primitives.DocumentViewerBase.CanGoToPreviousPageProperty); return target;}

//CanGoToNextPageProperty
public static TChild WithCanGoToNextPage<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.CanGoToNextPageProperty); return target;}
public static TChild WithCanGoToNextPage<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.DocumentViewerBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DocumentViewerBase{value?.SetValue(target, System.Windows.Controls.Primitives.DocumentViewerBase.CanGoToNextPageProperty); return target;}

//IsFindEnabledProperty
public static TChild WithIsFindEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.IsFindEnabledProperty); return target;}

//IsPrintEnabledProperty
public static TChild WithIsPrintEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.IsPrintEnabledProperty); return target;}

//DocumentProperty
public static TChild WithDocument<TChild>(this TChild target, ValueProxy<System.Windows.Documents.FlowDocument>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.DocumentProperty); return target;}
public static TChild WithDocument<TChild>(this TChild target, ValueProxy<System.Windows.Documents.FlowDocument>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.DocumentProperty); return target;}
public static TChild WithDocument<TChild>(this TChild target, ValueProxy<System.Windows.Documents.IDocumentPaginatorSource>? value, Disambigator<System.Windows.Controls.Primitives.DocumentViewerBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DocumentViewerBase{value?.SetValue(target, System.Windows.Controls.Primitives.DocumentViewerBase.DocumentProperty); return target;}

//MaxZoomProperty
public static TChild WithMaxZoom<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.MaxZoomProperty); return target;}
public static TChild WithMaxZoom<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.MaxZoomProperty); return target;}
public static TChild WithMaxZoom<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentPageViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentPageViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.MaxZoomProperty); return target;}

//MinZoomProperty
public static TChild WithMinZoom<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.MinZoomProperty); return target;}
public static TChild WithMinZoom<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.MinZoomProperty); return target;}
public static TChild WithMinZoom<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentPageViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentPageViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.MinZoomProperty); return target;}

//ZoomIncrementProperty
public static TChild WithZoomIncrement<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.ZoomIncrementProperty); return target;}
public static TChild WithZoomIncrement<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.ZoomIncrementProperty); return target;}
public static TChild WithZoomIncrement<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentPageViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentPageViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.ZoomIncrementProperty); return target;}

//SelectionBrushProperty
public static TChild WithSelectionBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.SelectionBrushProperty); return target;}
public static TChild WithSelectionBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.SelectionBrushProperty); return target;}
public static System.Windows.Controls.PasswordBox WithSelectionBrush(this System.Windows.Controls.PasswordBox target, ValueProxy<System.Windows.Media.Brush>? value) {value?.SetValue(target, System.Windows.Controls.PasswordBox.SelectionBrushProperty); return target;}
public static TChild WithSelectionBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.FlowDocumentPageViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentPageViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.SelectionBrushProperty); return target;}
public static TChild WithSelectionBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.SelectionBrushProperty); return target;}

//SelectionOpacityProperty
public static TChild WithSelectionOpacity<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.SelectionOpacityProperty); return target;}
public static TChild WithSelectionOpacity<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.SelectionOpacityProperty); return target;}
public static System.Windows.Controls.PasswordBox WithSelectionOpacity(this System.Windows.Controls.PasswordBox target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Controls.PasswordBox.SelectionOpacityProperty); return target;}
public static TChild WithSelectionOpacity<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.FlowDocumentPageViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentPageViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.SelectionOpacityProperty); return target;}
public static TChild WithSelectionOpacity<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.SelectionOpacityProperty); return target;}

//IsSelectionActiveProperty
public static TChild WithIsSelectionActive<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.IsSelectionActiveProperty); return target;}
public static TChild WithIsSelectionActive<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.IsSelectionActiveProperty); return target;}
public static System.Windows.Controls.PasswordBox WithIsSelectionActive(this System.Windows.Controls.PasswordBox target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Controls.PasswordBox.IsSelectionActiveProperty); return target;}
public static TChild WithIsSelectionActive<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentPageViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentPageViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.IsSelectionActiveProperty); return target;}
public static TChild WithIsSelectionActive<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.TreeViewItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TreeViewItem{value?.SetValue(target, System.Windows.Controls.TreeViewItem.IsSelectionActiveProperty); return target;}
public static TChild WithIsSelectionActive<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.IsSelectionActiveProperty); return target;}

//IsInactiveSelectionHighlightEnabledProperty
public static TChild WithIsInactiveSelectionHighlightEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentReader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentReader{value?.SetValue(target, System.Windows.Controls.FlowDocumentReader.IsInactiveSelectionHighlightEnabledProperty); return target;}
public static TChild WithIsInactiveSelectionHighlightEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.IsInactiveSelectionHighlightEnabledProperty); return target;}
public static System.Windows.Controls.PasswordBox WithIsInactiveSelectionHighlightEnabled(this System.Windows.Controls.PasswordBox target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Controls.PasswordBox.IsInactiveSelectionHighlightEnabledProperty); return target;}
public static TChild WithIsInactiveSelectionHighlightEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentPageViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentPageViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentPageViewer.IsInactiveSelectionHighlightEnabledProperty); return target;}
public static TChild WithIsInactiveSelectionHighlightEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.IsInactiveSelectionHighlightEnabledProperty); return target;}

//IsSelectionEnabledProperty
public static TChild WithIsSelectionEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.IsSelectionEnabledProperty); return target;}

//IsToolBarVisibleProperty
public static TChild WithIsToolBarVisible<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.FlowDocumentScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.FlowDocumentScrollViewer{value?.SetValue(target, System.Windows.Controls.FlowDocumentScrollViewer.IsToolBarVisibleProperty); return target;}

//NavigationUIVisibilityProperty
public static TChild WithNavigationUIVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Navigation.NavigationUIVisibility>? value, Disambigator<System.Windows.Controls.Frame, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Frame{value?.SetValue(target, System.Windows.Controls.Frame.NavigationUIVisibilityProperty); return target;}

//JournalOwnershipProperty
public static TChild WithJournalOwnership<TChild>(this TChild target, ValueProxy<System.Windows.Navigation.JournalOwnership>? value, Disambigator<System.Windows.Controls.Frame, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Frame{value?.SetValue(target, System.Windows.Controls.Frame.JournalOwnershipProperty); return target;}

//ShowGridLinesProperty
public static TChild WithShowGridLines<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Grid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Grid{value?.SetValue(target, System.Windows.Controls.Grid.ShowGridLinesProperty); return target;}

//RowProperty
public static TChild WithGrid_Row<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.Grid.RowProperty); return target;}

//IsSharedSizeScopeProperty
public static TChild WithGrid_IsSharedSizeScope<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.Grid.IsSharedSizeScopeProperty); return target;}

//ResizeDirectionProperty
public static TChild WithResizeDirection<TChild>(this TChild target, ValueProxy<System.Windows.Controls.GridResizeDirection>? value, Disambigator<System.Windows.Controls.GridSplitter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridSplitter{value?.SetValue(target, System.Windows.Controls.GridSplitter.ResizeDirectionProperty); return target;}

//ResizeBehaviorProperty
public static TChild WithResizeBehavior<TChild>(this TChild target, ValueProxy<System.Windows.Controls.GridResizeBehavior>? value, Disambigator<System.Windows.Controls.GridSplitter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridSplitter{value?.SetValue(target, System.Windows.Controls.GridSplitter.ResizeBehaviorProperty); return target;}

//ShowsPreviewProperty
public static TChild WithShowsPreview<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.GridSplitter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridSplitter{value?.SetValue(target, System.Windows.Controls.GridSplitter.ShowsPreviewProperty); return target;}

//PreviewStyleProperty
public static TChild WithPreviewStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.GridSplitter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridSplitter{value?.SetValue(target, System.Windows.Controls.GridSplitter.PreviewStyleProperty); return target;}

//KeyboardIncrementProperty
public static TChild WithKeyboardIncrement<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.GridSplitter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridSplitter{value?.SetValue(target, System.Windows.Controls.GridSplitter.KeyboardIncrementProperty); return target;}

//DragIncrementProperty
public static TChild WithDragIncrement<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.GridSplitter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridSplitter{value?.SetValue(target, System.Windows.Controls.GridSplitter.DragIncrementProperty); return target;}

//ColumnCollectionProperty
public static TChild WithGridView_ColumnCollection<TChild>(this TChild target, ValueProxy<System.Windows.Controls.GridViewColumnCollection>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.GridView.ColumnCollectionProperty); return target;}

//ColumnHeaderContainerStyleProperty
public static TChild WithColumnHeaderContainerStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.GridView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridView{value?.SetValue(target, System.Windows.Controls.GridView.ColumnHeaderContainerStyleProperty); return target;}
public static TChild WithColumnHeaderContainerStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.GridViewHeaderRowPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewHeaderRowPresenter{value?.SetValue(target, System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty); return target;}

//ColumnHeaderTemplateProperty
public static TChild WithColumnHeaderTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.GridView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridView{value?.SetValue(target, System.Windows.Controls.GridView.ColumnHeaderTemplateProperty); return target;}
public static TChild WithColumnHeaderTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.GridViewHeaderRowPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewHeaderRowPresenter{value?.SetValue(target, System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty); return target;}

//ColumnHeaderTemplateSelectorProperty
public static TChild WithColumnHeaderTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.GridView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridView{value?.SetValue(target, System.Windows.Controls.GridView.ColumnHeaderTemplateSelectorProperty); return target;}
public static TChild WithColumnHeaderTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.GridViewHeaderRowPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewHeaderRowPresenter{value?.SetValue(target, System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty); return target;}

//ColumnHeaderStringFormatProperty
public static TChild WithColumnHeaderStringFormat<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.GridView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridView{value?.SetValue(target, System.Windows.Controls.GridView.ColumnHeaderStringFormatProperty); return target;}
public static TChild WithColumnHeaderStringFormat<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.GridViewHeaderRowPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewHeaderRowPresenter{value?.SetValue(target, System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderStringFormatProperty); return target;}

//AllowsColumnReorderProperty
public static TChild WithAllowsColumnReorder<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.GridView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridView{value?.SetValue(target, System.Windows.Controls.GridView.AllowsColumnReorderProperty); return target;}
public static TChild WithAllowsColumnReorder<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.GridViewHeaderRowPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewHeaderRowPresenter{value?.SetValue(target, System.Windows.Controls.GridViewHeaderRowPresenter.AllowsColumnReorderProperty); return target;}

//ColumnHeaderContextMenuProperty
public static TChild WithColumnHeaderContextMenu<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ContextMenu>? value, Disambigator<System.Windows.Controls.GridView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridView{value?.SetValue(target, System.Windows.Controls.GridView.ColumnHeaderContextMenuProperty); return target;}
public static TChild WithColumnHeaderContextMenu<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ContextMenu>? value, Disambigator<System.Windows.Controls.GridViewHeaderRowPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewHeaderRowPresenter{value?.SetValue(target, System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderContextMenuProperty); return target;}

//ColumnHeaderToolTipProperty
public static TChild WithColumnHeaderToolTip<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.GridView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridView{value?.SetValue(target, System.Windows.Controls.GridView.ColumnHeaderToolTipProperty); return target;}
public static TChild WithColumnHeaderToolTip<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.GridViewHeaderRowPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewHeaderRowPresenter{value?.SetValue(target, System.Windows.Controls.GridViewHeaderRowPresenter.ColumnHeaderToolTipProperty); return target;}

//HeaderContainerStyleProperty
public static TChild WithHeaderContainerStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.GridViewColumn, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewColumn{value?.SetValue(target, System.Windows.Controls.GridViewColumn.HeaderContainerStyleProperty); return target;}

//RoleProperty
public static TChild WithRole<TChild>(this TChild target, ValueProxy<System.Windows.Controls.GridViewColumnHeaderRole>? value, Disambigator<System.Windows.Controls.GridViewColumnHeader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.GridViewColumnHeader{value?.SetValue(target, System.Windows.Controls.GridViewColumnHeader.RoleProperty); return target;}
public static TChild WithRole<TChild>(this TChild target, ValueProxy<System.Windows.Controls.MenuItemRole>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.RoleProperty); return target;}

//HasHeaderProperty
public static TChild WithHasHeader<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.HeaderedContentControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.HeaderedContentControl{value?.SetValue(target, System.Windows.Controls.HeaderedContentControl.HasHeaderProperty); return target;}
public static TChild WithHasHeader<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.HeaderedItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.HeaderedItemsControl{value?.SetValue(target, System.Windows.Controls.HeaderedItemsControl.HasHeaderProperty); return target;}

//StretchDirectionProperty
public static TChild WithStretchDirection<TChild>(this TChild target, ValueProxy<System.Windows.Controls.StretchDirection>? value, Disambigator<System.Windows.Controls.Image, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Image{value?.SetValue(target, System.Windows.Controls.Image.StretchDirectionProperty); return target;}
public static TChild WithStretchDirection<TChild>(this TChild target, ValueProxy<System.Windows.Controls.StretchDirection>? value, Disambigator<System.Windows.Controls.MediaElement, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MediaElement{value?.SetValue(target, System.Windows.Controls.MediaElement.StretchDirectionProperty); return target;}
public static TChild WithStretchDirection<TChild>(this TChild target, ValueProxy<System.Windows.Controls.StretchDirection>? value, Disambigator<System.Windows.Controls.Viewbox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Viewbox{value?.SetValue(target, System.Windows.Controls.Viewbox.StretchDirectionProperty); return target;}
public static TChild WithStretchDirection<TChild>(this TChild target, ValueProxy<System.Windows.Controls.StretchDirection>? value, Disambigator<System.Windows.Controls.Primitives.DocumentPageView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DocumentPageView{value?.SetValue(target, System.Windows.Controls.Primitives.DocumentPageView.StretchDirectionProperty); return target;}

//StrokesProperty
public static TChild WithStrokes<TChild>(this TChild target, ValueProxy<System.Windows.Ink.StrokeCollection>? value, Disambigator<System.Windows.Controls.InkCanvas, TChild>? doNotUse = null) where TChild: System.Windows.Controls.InkCanvas{value?.SetValue(target, System.Windows.Controls.InkCanvas.StrokesProperty); return target;}
public static TChild WithStrokes<TChild>(this TChild target, ValueProxy<System.Windows.Ink.StrokeCollection>? value, Disambigator<System.Windows.Controls.InkPresenter, TChild>? doNotUse = null) where TChild: System.Windows.Controls.InkPresenter{value?.SetValue(target, System.Windows.Controls.InkPresenter.StrokesProperty); return target;}

//DefaultDrawingAttributesProperty
public static TChild WithDefaultDrawingAttributes<TChild>(this TChild target, ValueProxy<System.Windows.Ink.DrawingAttributes>? value, Disambigator<System.Windows.Controls.InkCanvas, TChild>? doNotUse = null) where TChild: System.Windows.Controls.InkCanvas{value?.SetValue(target, System.Windows.Controls.InkCanvas.DefaultDrawingAttributesProperty); return target;}

//ActiveEditingModeProperty
public static TChild WithActiveEditingMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.InkCanvasEditingMode>? value, Disambigator<System.Windows.Controls.InkCanvas, TChild>? doNotUse = null) where TChild: System.Windows.Controls.InkCanvas{value?.SetValue(target, System.Windows.Controls.InkCanvas.ActiveEditingModeProperty); return target;}

//EditingModeProperty
public static TChild WithEditingMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.InkCanvasEditingMode>? value, Disambigator<System.Windows.Controls.InkCanvas, TChild>? doNotUse = null) where TChild: System.Windows.Controls.InkCanvas{value?.SetValue(target, System.Windows.Controls.InkCanvas.EditingModeProperty); return target;}

//EditingModeInvertedProperty
public static TChild WithEditingModeInverted<TChild>(this TChild target, ValueProxy<System.Windows.Controls.InkCanvasEditingMode>? value, Disambigator<System.Windows.Controls.InkCanvas, TChild>? doNotUse = null) where TChild: System.Windows.Controls.InkCanvas{value?.SetValue(target, System.Windows.Controls.InkCanvas.EditingModeInvertedProperty); return target;}

//HasItemsProperty
public static TChild WithHasItems<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.HasItemsProperty); return target;}

//ItemTemplateProperty
public static TChild WithItemTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.ItemTemplateProperty); return target;}

//ItemTemplateSelectorProperty
public static TChild WithItemTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.ItemTemplateSelectorProperty); return target;}

//ItemStringFormatProperty
public static TChild WithItemStringFormat<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.ItemStringFormatProperty); return target;}

//ItemBindingGroupProperty
public static TChild WithItemBindingGroup<TChild>(this TChild target, ValueProxy<System.Windows.Data.BindingGroup>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.ItemBindingGroupProperty); return target;}

//ItemContainerStyleProperty
public static TChild WithItemContainerStyle<TChild>(this TChild target, ValueProxy<System.Windows.Style>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.ItemContainerStyleProperty); return target;}

//ItemContainerStyleSelectorProperty
public static TChild WithItemContainerStyleSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.StyleSelector>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.ItemContainerStyleSelectorProperty); return target;}

//IsGroupingProperty
public static TChild WithIsGrouping<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.IsGroupingProperty); return target;}

//GroupStyleSelectorProperty
public static TChild WithGroupStyleSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.GroupStyleSelector>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.GroupStyleSelectorProperty); return target;}

//AlternationCountProperty
public static TChild WithAlternationCount<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.AlternationCountProperty); return target;}

//IsTextSearchEnabledProperty
public static TChild WithIsTextSearchEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.IsTextSearchEnabledProperty); return target;}

//IsTextSearchCaseSensitiveProperty
public static TChild WithIsTextSearchCaseSensitive<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ItemsControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ItemsControl{value?.SetValue(target, System.Windows.Controls.ItemsControl.IsTextSearchCaseSensitiveProperty); return target;}

//SelectedItemsProperty
public static TChild WithSelectedItems<TChild>(this TChild target, ValueProxy<System.Collections.IList>? value, Disambigator<System.Windows.Controls.ListBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ListBox{value?.SetValue(target, System.Windows.Controls.ListBox.SelectedItemsProperty); return target;}

//VolumeProperty
public static TChild WithVolume<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.MediaElement, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MediaElement{value?.SetValue(target, System.Windows.Controls.MediaElement.VolumeProperty); return target;}

//BalanceProperty
public static TChild WithBalance<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.MediaElement, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MediaElement{value?.SetValue(target, System.Windows.Controls.MediaElement.BalanceProperty); return target;}

//IsMutedProperty
public static TChild WithIsMuted<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.MediaElement, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MediaElement{value?.SetValue(target, System.Windows.Controls.MediaElement.IsMutedProperty); return target;}

//ScrubbingEnabledProperty
public static TChild WithScrubbingEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.MediaElement, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MediaElement{value?.SetValue(target, System.Windows.Controls.MediaElement.ScrubbingEnabledProperty); return target;}

//UnloadedBehaviorProperty
public static TChild WithUnloadedBehavior<TChild>(this TChild target, ValueProxy<System.Windows.Controls.MediaState>? value, Disambigator<System.Windows.Controls.MediaElement, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MediaElement{value?.SetValue(target, System.Windows.Controls.MediaElement.UnloadedBehaviorProperty); return target;}

//LoadedBehaviorProperty
public static TChild WithLoadedBehavior<TChild>(this TChild target, ValueProxy<System.Windows.Controls.MediaState>? value, Disambigator<System.Windows.Controls.MediaElement, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MediaElement{value?.SetValue(target, System.Windows.Controls.MediaElement.LoadedBehaviorProperty); return target;}

//IsMainMenuProperty
public static TChild WithIsMainMenu<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Menu, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Menu{value?.SetValue(target, System.Windows.Controls.Menu.IsMainMenuProperty); return target;}

//IsSubmenuOpenProperty
public static TChild WithIsSubmenuOpen<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.IsSubmenuOpenProperty); return target;}

//IsCheckableProperty
public static TChild WithIsCheckable<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.IsCheckableProperty); return target;}

//IsPressedProperty
public static TChild WithIsPressed<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.IsPressedProperty); return target;}
public static TChild WithIsPressed<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.ButtonBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.ButtonBase{value?.SetValue(target, System.Windows.Controls.Primitives.ButtonBase.IsPressedProperty); return target;}

//IsCheckedProperty
public static TChild WithIsChecked<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.IsCheckedProperty); return target;}
public static TChild WithIsChecked<TChild>(this TChild target, ValueProxy<System.Nullable<System.Boolean>>? value, Disambigator<System.Windows.Controls.Primitives.ToggleButton, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.ToggleButton{value?.SetValue(target, System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty); return target;}

//StaysOpenOnClickProperty
public static TChild WithStaysOpenOnClick<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.StaysOpenOnClickProperty); return target;}

//InputGestureTextProperty
public static TChild WithInputGestureText<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.InputGestureTextProperty); return target;}

//IsSuspendingPopupAnimationProperty
public static TChild WithIsSuspendingPopupAnimation<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.IsSuspendingPopupAnimationProperty); return target;}

//ItemContainerTemplateSelectorProperty
public static TChild WithItemContainerTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ItemContainerTemplateSelector>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.ItemContainerTemplateSelectorProperty); return target;}
public static TChild WithItemContainerTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ItemContainerTemplateSelector>? value, Disambigator<System.Windows.Controls.Primitives.MenuBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.MenuBase{value?.SetValue(target, System.Windows.Controls.Primitives.MenuBase.ItemContainerTemplateSelectorProperty); return target;}
public static TChild WithItemContainerTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ItemContainerTemplateSelector>? value, Disambigator<System.Windows.Controls.Primitives.StatusBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.StatusBar{value?.SetValue(target, System.Windows.Controls.Primitives.StatusBar.ItemContainerTemplateSelectorProperty); return target;}

//UsesItemContainerTemplateProperty
public static TChild WithUsesItemContainerTemplate<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.MenuItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.MenuItem{value?.SetValue(target, System.Windows.Controls.MenuItem.UsesItemContainerTemplateProperty); return target;}
public static TChild WithUsesItemContainerTemplate<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.MenuBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.MenuBase{value?.SetValue(target, System.Windows.Controls.Primitives.MenuBase.UsesItemContainerTemplateProperty); return target;}
public static TChild WithUsesItemContainerTemplate<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.StatusBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.StatusBar{value?.SetValue(target, System.Windows.Controls.Primitives.StatusBar.UsesItemContainerTemplateProperty); return target;}

//IsItemsHostProperty
public static TChild WithIsItemsHost<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Panel, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Panel{value?.SetValue(target, System.Windows.Controls.Panel.IsItemsHostProperty); return target;}

//ZIndexProperty
public static TChild WithPanel_ZIndex<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.Controls.Panel.ZIndexProperty); return target;}

//PasswordCharProperty
public static System.Windows.Controls.PasswordBox WithPasswordChar(this System.Windows.Controls.PasswordBox target, ValueProxy<System.Char>? value) {value?.SetValue(target, System.Windows.Controls.PasswordBox.PasswordCharProperty); return target;}

//MaxLengthProperty
public static System.Windows.Controls.PasswordBox WithMaxLength(this System.Windows.Controls.PasswordBox target, ValueProxy<System.Int32>? value) {value?.SetValue(target, System.Windows.Controls.PasswordBox.MaxLengthProperty); return target;}
public static TChild WithMaxLength<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.TextBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBox{value?.SetValue(target, System.Windows.Controls.TextBox.MaxLengthProperty); return target;}

//SelectionTextBrushProperty
public static System.Windows.Controls.PasswordBox WithSelectionTextBrush(this System.Windows.Controls.PasswordBox target, ValueProxy<System.Windows.Media.Brush>? value) {value?.SetValue(target, System.Windows.Controls.PasswordBox.SelectionTextBrushProperty); return target;}
public static TChild WithSelectionTextBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.SelectionTextBrushProperty); return target;}

//CaretBrushProperty
public static System.Windows.Controls.PasswordBox WithCaretBrush(this System.Windows.Controls.PasswordBox target, ValueProxy<System.Windows.Media.Brush>? value) {value?.SetValue(target, System.Windows.Controls.PasswordBox.CaretBrushProperty); return target;}
public static TChild WithCaretBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.CaretBrushProperty); return target;}

//CanContentScrollProperty
public static System.Windows.Controls.ScrollContentPresenter WithCanContentScroll(this System.Windows.Controls.ScrollContentPresenter target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Controls.ScrollContentPresenter.CanContentScrollProperty); return target;}
public static TChild WithScrollViewer_CanContentScroll<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ScrollViewer.CanContentScrollProperty); return target;}
public static TChild WithCanContentScroll<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.CanContentScrollProperty); return target;}

//IsIndeterminateProperty
public static TChild WithIsIndeterminate<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ProgressBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ProgressBar{value?.SetValue(target, System.Windows.Controls.ProgressBar.IsIndeterminateProperty); return target;}

//OrientationProperty

public static TChild WithOrientation<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Orientation>? value, Disambigator<System.Windows.Controls.ProgressBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ProgressBar{value?.SetValue(target, System.Windows.Controls.ProgressBar.OrientationProperty); return target;}
public static TChild WithOrientation<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Orientation>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.OrientationProperty); return target;}
public static TChild WithOrientation<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Orientation>? value, Disambigator<System.Windows.Controls.StackPanel, TChild>? doNotUse = null) where TChild: System.Windows.Controls.StackPanel{value?.SetValue(target, System.Windows.Controls.StackPanel.OrientationProperty); return target;}
public static TChild WithOrientation<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Orientation>? value, Disambigator<System.Windows.Controls.ToolBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolBar{value?.SetValue(target, System.Windows.Controls.ToolBar.OrientationProperty); return target;}
public static TChild WithOrientation<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Orientation>? value, Disambigator<System.Windows.Controls.ToolBarTray, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolBarTray{value?.SetValue(target, System.Windows.Controls.ToolBarTray.OrientationProperty); return target;}
public static TChild WithOrientation<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Orientation>? value, Disambigator<System.Windows.Controls.VirtualizingStackPanel, TChild>? doNotUse = null) where TChild: System.Windows.Controls.VirtualizingStackPanel{value?.SetValue(target, System.Windows.Controls.VirtualizingStackPanel.OrientationProperty); return target;}
public static TChild WithOrientation<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Orientation>? value, Disambigator<System.Windows.Controls.WrapPanel, TChild>? doNotUse = null) where TChild: System.Windows.Controls.WrapPanel{value?.SetValue(target, System.Windows.Controls.WrapPanel.OrientationProperty); return target;}
public static TChild WithOrientation<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Orientation>? value, Disambigator<System.Windows.Controls.Primitives.ScrollBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.ScrollBar{value?.SetValue(target, System.Windows.Controls.Primitives.ScrollBar.OrientationProperty); return target;}
public static TChild WithOrientation<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Orientation>? value, Disambigator<System.Windows.Controls.Primitives.Track, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Track{value?.SetValue(target, System.Windows.Controls.Primitives.Track.OrientationProperty); return target;}

//GroupNameProperty
public static TChild WithGroupName<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.RadioButton, TChild>? doNotUse = null) where TChild: System.Windows.Controls.RadioButton{value?.SetValue(target, System.Windows.Controls.RadioButton.GroupNameProperty); return target;}

//IsDocumentEnabledProperty
public static TChild WithIsDocumentEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.RichTextBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.RichTextBox{value?.SetValue(target, System.Windows.Controls.RichTextBox.IsDocumentEnabledProperty); return target;}

//ComputedHorizontalScrollBarVisibilityProperty
public static TChild WithComputedHorizontalScrollBarVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Visibility>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.ComputedHorizontalScrollBarVisibilityProperty); return target;}

//ComputedVerticalScrollBarVisibilityProperty
public static TChild WithComputedVerticalScrollBarVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Visibility>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.ComputedVerticalScrollBarVisibilityProperty); return target;}

//ContentVerticalOffsetProperty
public static TChild WithContentVerticalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.ContentVerticalOffsetProperty); return target;}

//ContentHorizontalOffsetProperty
public static TChild WithContentHorizontalOffset<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.ContentHorizontalOffsetProperty); return target;}

//ScrollableWidthProperty
public static TChild WithScrollableWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.ScrollableWidthProperty); return target;}

//ScrollableHeightProperty
public static TChild WithScrollableHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.ScrollableHeightProperty); return target;}

//IsDeferredScrollingEnabledProperty
public static TChild WithScrollViewer_IsDeferredScrollingEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ScrollViewer.IsDeferredScrollingEnabledProperty); return target;}
public static TChild WithIsDeferredScrollingEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.IsDeferredScrollingEnabledProperty); return target;}

//PanningModeProperty
public static TChild WithScrollViewer_PanningMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.PanningMode>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ScrollViewer.PanningModeProperty); return target;}
public static TChild WithPanningMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.PanningMode>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.PanningModeProperty); return target;}

//PanningDecelerationProperty
public static TChild WithScrollViewer_PanningDeceleration<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ScrollViewer.PanningDecelerationProperty); return target;}
public static TChild WithPanningDeceleration<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.PanningDecelerationProperty); return target;}

//PanningRatioProperty
public static TChild WithScrollViewer_PanningRatio<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ScrollViewer.PanningRatioProperty); return target;}
public static TChild WithPanningRatio<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.ScrollViewer, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ScrollViewer{value?.SetValue(target, System.Windows.Controls.ScrollViewer.PanningRatioProperty); return target;}

//IsDirectionReversedProperty
public static TChild WithIsDirectionReversed<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.IsDirectionReversedProperty); return target;}
public static TChild WithIsDirectionReversed<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.TickBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TickBar{value?.SetValue(target, System.Windows.Controls.Primitives.TickBar.IsDirectionReversedProperty); return target;}
public static TChild WithIsDirectionReversed<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.Track, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Track{value?.SetValue(target, System.Windows.Controls.Primitives.Track.IsDirectionReversedProperty); return target;}

//DelayProperty
public static TChild WithDelay<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.DelayProperty); return target;}
public static TChild WithDelay<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Primitives.RepeatButton, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.RepeatButton{value?.SetValue(target, System.Windows.Controls.Primitives.RepeatButton.DelayProperty); return target;}

//IntervalProperty
public static TChild WithInterval<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.IntervalProperty); return target;}
public static TChild WithInterval<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Primitives.RepeatButton, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.RepeatButton{value?.SetValue(target, System.Windows.Controls.Primitives.RepeatButton.IntervalProperty); return target;}

//AutoToolTipPlacementProperty
public static TChild WithAutoToolTipPlacement<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Primitives.AutoToolTipPlacement>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.AutoToolTipPlacementProperty); return target;}

//AutoToolTipPrecisionProperty
public static TChild WithAutoToolTipPrecision<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.AutoToolTipPrecisionProperty); return target;}

//IsSnapToTickEnabledProperty
public static TChild WithIsSnapToTickEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.IsSnapToTickEnabledProperty); return target;}

//TickPlacementProperty
public static TChild WithTickPlacement<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Primitives.TickPlacement>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.TickPlacementProperty); return target;}

//TickFrequencyProperty
public static TChild WithTickFrequency<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.TickFrequencyProperty); return target;}
public static TChild WithTickFrequency<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.TickBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TickBar{value?.SetValue(target, System.Windows.Controls.Primitives.TickBar.TickFrequencyProperty); return target;}

//TicksProperty
public static TChild WithTicks<TChild>(this TChild target, ValueProxy<System.Windows.Media.DoubleCollection>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.TicksProperty); return target;}
public static TChild WithTicks<TChild>(this TChild target, ValueProxy<System.Windows.Media.DoubleCollection>? value, Disambigator<System.Windows.Controls.Primitives.TickBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TickBar{value?.SetValue(target, System.Windows.Controls.Primitives.TickBar.TicksProperty); return target;}

//IsSelectionRangeEnabledProperty
public static TChild WithIsSelectionRangeEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.IsSelectionRangeEnabledProperty); return target;}
public static TChild WithIsSelectionRangeEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.TickBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TickBar{value?.SetValue(target, System.Windows.Controls.Primitives.TickBar.IsSelectionRangeEnabledProperty); return target;}

//SelectionStartProperty
public static TChild WithSelectionStart<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.SelectionStartProperty); return target;}
public static TChild WithSelectionStart<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.TickBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TickBar{value?.SetValue(target, System.Windows.Controls.Primitives.TickBar.SelectionStartProperty); return target;}

//SelectionEndProperty
public static TChild WithSelectionEnd<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.SelectionEndProperty); return target;}
public static TChild WithSelectionEnd<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.TickBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TickBar{value?.SetValue(target, System.Windows.Controls.Primitives.TickBar.SelectionEndProperty); return target;}

//IsMoveToPointEnabledProperty
public static TChild WithIsMoveToPointEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Slider, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Slider{value?.SetValue(target, System.Windows.Controls.Slider.IsMoveToPointEnabledProperty); return target;}

//SpellingReformProperty
public static TChild WithSpellCheck_SpellingReform<TChild>(this TChild target, ValueProxy<System.Windows.Controls.SpellingReform>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.SpellCheck.SpellingReformProperty); return target;}

//CustomDictionariesProperty

//TabStripPlacementProperty
public static TChild WithTabStripPlacement<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Dock>? value, Disambigator<System.Windows.Controls.TabControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TabControl{value?.SetValue(target, System.Windows.Controls.TabControl.TabStripPlacementProperty); return target;}
public static TChild WithTabStripPlacement<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Dock>? value, Disambigator<System.Windows.Controls.TabItem, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TabItem{value?.SetValue(target, System.Windows.Controls.TabItem.TabStripPlacementProperty); return target;}

//SelectedContentProperty
public static TChild WithSelectedContent<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.TabControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TabControl{value?.SetValue(target, System.Windows.Controls.TabControl.SelectedContentProperty); return target;}

//SelectedContentTemplateProperty
public static TChild WithSelectedContentTemplate<TChild>(this TChild target, ValueProxy<System.Windows.DataTemplate>? value, Disambigator<System.Windows.Controls.TabControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TabControl{value?.SetValue(target, System.Windows.Controls.TabControl.SelectedContentTemplateProperty); return target;}

//SelectedContentTemplateSelectorProperty
public static TChild WithSelectedContentTemplateSelector<TChild>(this TChild target, ValueProxy<System.Windows.Controls.DataTemplateSelector>? value, Disambigator<System.Windows.Controls.TabControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TabControl{value?.SetValue(target, System.Windows.Controls.TabControl.SelectedContentTemplateSelectorProperty); return target;}

//SelectedContentStringFormatProperty
public static TChild WithSelectedContentStringFormat<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.Controls.TabControl, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TabControl{value?.SetValue(target, System.Windows.Controls.TabControl.SelectedContentStringFormatProperty); return target;}

//MinLinesProperty
public static TChild WithMinLines<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.TextBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBox{value?.SetValue(target, System.Windows.Controls.TextBox.MinLinesProperty); return target;}

//MaxLinesProperty
public static TChild WithMaxLines<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.TextBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBox{value?.SetValue(target, System.Windows.Controls.TextBox.MaxLinesProperty); return target;}

//CharacterCasingProperty
public static TChild WithCharacterCasing<TChild>(this TChild target, ValueProxy<System.Windows.Controls.CharacterCasing>? value, Disambigator<System.Windows.Controls.TextBox, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TextBox{value?.SetValue(target, System.Windows.Controls.TextBox.CharacterCasingProperty); return target;}

//TextPathProperty
public static TChild WithTextSearch_TextPath<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.TextSearch.TextPathProperty); return target;}

//BandProperty
public static TChild WithBand<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.ToolBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolBar{value?.SetValue(target, System.Windows.Controls.ToolBar.BandProperty); return target;}

//BandIndexProperty
public static TChild WithBandIndex<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.ToolBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolBar{value?.SetValue(target, System.Windows.Controls.ToolBar.BandIndexProperty); return target;}

//IsOverflowOpenProperty
public static TChild WithIsOverflowOpen<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ToolBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolBar{value?.SetValue(target, System.Windows.Controls.ToolBar.IsOverflowOpenProperty); return target;}

//HasOverflowItemsProperty
public static TChild WithHasOverflowItems<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ToolBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolBar{value?.SetValue(target, System.Windows.Controls.ToolBar.HasOverflowItemsProperty); return target;}

//IsOverflowItemProperty

//OverflowModeProperty
public static TChild WithToolBar_OverflowMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.OverflowMode>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolBar.OverflowModeProperty); return target;}

//IsLockedProperty
public static TChild WithToolBarTray_IsLocked<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolBarTray.IsLockedProperty); return target;}
public static TChild WithIsLocked<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.ToolBarTray, TChild>? doNotUse = null) where TChild: System.Windows.Controls.ToolBarTray{value?.SetValue(target, System.Windows.Controls.ToolBarTray.IsLockedProperty); return target;}

//ShowDurationProperty
public static TChild WithToolTipService_ShowDuration<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolTipService.ShowDurationProperty); return target;}

//InitialShowDelayProperty
public static TChild WithToolTipService_InitialShowDelay<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolTipService.InitialShowDelayProperty); return target;}

//BetweenShowDelayProperty
public static TChild WithToolTipService_BetweenShowDelay<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.ToolTipService.BetweenShowDelayProperty); return target;}

//SelectedItemProperty
public static TChild WithSelectedItem<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.TreeView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TreeView{value?.SetValue(target, System.Windows.Controls.TreeView.SelectedItemProperty); return target;}
public static TChild WithSelectedItem<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.Primitives.Selector, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Selector{value?.SetValue(target, System.Windows.Controls.Primitives.Selector.SelectedItemProperty); return target;}

//SelectedValueProperty
public static TChild WithSelectedValue<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.TreeView, TChild>? doNotUse = null) where TChild: System.Windows.Controls.TreeView{value?.SetValue(target, System.Windows.Controls.TreeView.SelectedValueProperty); return target;}
public static TChild WithSelectedValue<TChild>(this TChild target, ValueProxy<System.Object>? value, Disambigator<System.Windows.Controls.Primitives.Selector, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Selector{value?.SetValue(target, System.Windows.Controls.Primitives.Selector.SelectedValueProperty); return target;}

//ErrorsProperty

//HasErrorProperty

//ErrorTemplateProperty
public static TChild WithValidation_ErrorTemplate<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ControlTemplate>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.Validation.ErrorTemplateProperty); return target;}

//ValidationAdornerSiteProperty
public static TChild WithValidation_ValidationAdornerSite<TChild>(this TChild target, ValueProxy<System.Windows.DependencyObject>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.Validation.ValidationAdornerSiteProperty); return target;}

//ValidationAdornerSiteForProperty
public static TChild WithValidation_ValidationAdornerSiteFor<TChild>(this TChild target, ValueProxy<System.Windows.DependencyObject>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.Validation.ValidationAdornerSiteForProperty); return target;}

//CameraProperty
public static TChild WithCamera<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Camera>? value, Disambigator<System.Windows.Controls.Viewport3D, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Viewport3D{value?.SetValue(target, System.Windows.Controls.Viewport3D.CameraProperty); return target;}
public static System.Windows.Media.Media3D.Viewport3DVisual WithCamera(this System.Windows.Media.Media3D.Viewport3DVisual target, ValueProxy<System.Windows.Media.Media3D.Camera>? value) {value?.SetValue(target, System.Windows.Media.Media3D.Viewport3DVisual.CameraProperty); return target;}

//ChildrenProperty
public static TChild WithChildren<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Visual3DCollection>? value, Disambigator<System.Windows.Controls.Viewport3D, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Viewport3D{value?.SetValue(target, System.Windows.Controls.Viewport3D.ChildrenProperty); return target;}
public static System.Windows.Media.GeneralTransformGroup WithChildren(this System.Windows.Media.GeneralTransformGroup target, ValueProxy<System.Windows.Media.GeneralTransformCollection>? value) {value?.SetValue(target, System.Windows.Media.GeneralTransformGroup.ChildrenProperty); return target;}
public static System.Windows.Media.TransformGroup WithChildren(this System.Windows.Media.TransformGroup target, ValueProxy<System.Windows.Media.TransformCollection>? value) {value?.SetValue(target, System.Windows.Media.TransformGroup.ChildrenProperty); return target;}
public static System.Windows.Media.DrawingGroup WithChildren(this System.Windows.Media.DrawingGroup target, ValueProxy<System.Windows.Media.DrawingCollection>? value) {value?.SetValue(target, System.Windows.Media.DrawingGroup.ChildrenProperty); return target;}
public static System.Windows.Media.GeometryGroup WithChildren(this System.Windows.Media.GeometryGroup target, ValueProxy<System.Windows.Media.GeometryCollection>? value) {value?.SetValue(target, System.Windows.Media.GeometryGroup.ChildrenProperty); return target;}
public static System.Windows.Media.Media3D.GeneralTransform3DGroup WithChildren(this System.Windows.Media.Media3D.GeneralTransform3DGroup target, ValueProxy<System.Windows.Media.Media3D.GeneralTransform3DCollection>? value) {value?.SetValue(target, System.Windows.Media.Media3D.GeneralTransform3DGroup.ChildrenProperty); return target;}
public static System.Windows.Media.Media3D.MaterialGroup WithChildren(this System.Windows.Media.Media3D.MaterialGroup target, ValueProxy<System.Windows.Media.Media3D.MaterialCollection>? value) {value?.SetValue(target, System.Windows.Media.Media3D.MaterialGroup.ChildrenProperty); return target;}
public static System.Windows.Media.Media3D.Model3DGroup WithChildren(this System.Windows.Media.Media3D.Model3DGroup target, ValueProxy<System.Windows.Media.Media3D.Model3DCollection>? value) {value?.SetValue(target, System.Windows.Media.Media3D.Model3DGroup.ChildrenProperty); return target;}
public static System.Windows.Media.Media3D.Transform3DGroup WithChildren(this System.Windows.Media.Media3D.Transform3DGroup target, ValueProxy<System.Windows.Media.Media3D.Transform3DCollection>? value) {value?.SetValue(target, System.Windows.Media.Media3D.Transform3DGroup.ChildrenProperty); return target;}
public static TChild WithChildren<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.TimelineCollection>? value, Disambigator<System.Windows.Media.Animation.TimelineGroup, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.TimelineGroup{value?.SetValue(target, System.Windows.Media.Animation.TimelineGroup.ChildrenProperty); return target;}
public static System.Windows.Media.Effects.BitmapEffectGroup WithChildren(this System.Windows.Media.Effects.BitmapEffectGroup target, ValueProxy<System.Windows.Media.Effects.BitmapEffectCollection>? value) {value?.SetValue(target, System.Windows.Media.Effects.BitmapEffectGroup.ChildrenProperty); return target;}

//IsVirtualizingProperty
public static TChild WithVirtualizingPanel_IsVirtualizing<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.VirtualizingPanel.IsVirtualizingProperty); return target;}

//VirtualizationModeProperty
public static TChild WithVirtualizingPanel_VirtualizationMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.VirtualizationMode>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.VirtualizingPanel.VirtualizationModeProperty); return target;}

//IsVirtualizingWhenGroupingProperty
public static TChild WithVirtualizingPanel_IsVirtualizingWhenGrouping<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.VirtualizingPanel.IsVirtualizingWhenGroupingProperty); return target;}

//ScrollUnitProperty
public static TChild WithVirtualizingPanel_ScrollUnit<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ScrollUnit>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.VirtualizingPanel.ScrollUnitProperty); return target;}

//CacheLengthProperty
public static TChild WithVirtualizingPanel_CacheLength<TChild>(this TChild target, ValueProxy<System.Windows.Controls.VirtualizationCacheLength>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.VirtualizingPanel.CacheLengthProperty); return target;}

//CacheLengthUnitProperty
public static TChild WithVirtualizingPanel_CacheLengthUnit<TChild>(this TChild target, ValueProxy<System.Windows.Controls.VirtualizationCacheLengthUnit>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.VirtualizingPanel.CacheLengthUnitProperty); return target;}

//IsContainerVirtualizableProperty
public static TChild WithVirtualizingPanel_IsContainerVirtualizable<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.VirtualizingPanel.IsContainerVirtualizableProperty); return target;}

//ItemWidthProperty
public static TChild WithItemWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.WrapPanel, TChild>? doNotUse = null) where TChild: System.Windows.Controls.WrapPanel{value?.SetValue(target, System.Windows.Controls.WrapPanel.ItemWidthProperty); return target;}

//ItemHeightProperty
public static TChild WithItemHeight<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.WrapPanel, TChild>? doNotUse = null) where TChild: System.Windows.Controls.WrapPanel{value?.SetValue(target, System.Windows.Controls.WrapPanel.ItemHeightProperty); return target;}

//ClickModeProperty
public static TChild WithClickMode<TChild>(this TChild target, ValueProxy<System.Windows.Controls.ClickMode>? value, Disambigator<System.Windows.Controls.Primitives.ButtonBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.ButtonBase{value?.SetValue(target, System.Windows.Controls.Primitives.ButtonBase.ClickModeProperty); return target;}

//HasSelectedDaysProperty
public static System.Windows.Controls.Primitives.CalendarButton WithHasSelectedDays(this System.Windows.Controls.Primitives.CalendarButton target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Controls.Primitives.CalendarButton.HasSelectedDaysProperty); return target;}

//IsInactiveProperty
public static System.Windows.Controls.Primitives.CalendarButton WithIsInactive(this System.Windows.Controls.Primitives.CalendarButton target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Controls.Primitives.CalendarButton.IsInactiveProperty); return target;}
public static System.Windows.Controls.Primitives.CalendarDayButton WithIsInactive(this System.Windows.Controls.Primitives.CalendarDayButton target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Controls.Primitives.CalendarDayButton.IsInactiveProperty); return target;}

//IsTodayProperty
public static System.Windows.Controls.Primitives.CalendarDayButton WithIsToday(this System.Windows.Controls.Primitives.CalendarDayButton target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Controls.Primitives.CalendarDayButton.IsTodayProperty); return target;}

//IsBlackedOutProperty
public static System.Windows.Controls.Primitives.CalendarDayButton WithIsBlackedOut(this System.Windows.Controls.Primitives.CalendarDayButton target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Controls.Primitives.CalendarDayButton.IsBlackedOutProperty); return target;}

//SeparatorBrushProperty
public static TChild WithSeparatorBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Primitives.DataGridColumnHeader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DataGridColumnHeader{value?.SetValue(target, System.Windows.Controls.Primitives.DataGridColumnHeader.SeparatorBrushProperty); return target;}
public static TChild WithSeparatorBrush<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.Controls.Primitives.DataGridRowHeader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DataGridRowHeader{value?.SetValue(target, System.Windows.Controls.Primitives.DataGridRowHeader.SeparatorBrushProperty); return target;}

//SeparatorVisibilityProperty
public static TChild WithSeparatorVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Visibility>? value, Disambigator<System.Windows.Controls.Primitives.DataGridColumnHeader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DataGridColumnHeader{value?.SetValue(target, System.Windows.Controls.Primitives.DataGridColumnHeader.SeparatorVisibilityProperty); return target;}
public static TChild WithSeparatorVisibility<TChild>(this TChild target, ValueProxy<System.Windows.Visibility>? value, Disambigator<System.Windows.Controls.Primitives.DataGridRowHeader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DataGridRowHeader{value?.SetValue(target, System.Windows.Controls.Primitives.DataGridRowHeader.SeparatorVisibilityProperty); return target;}

//IsRowSelectedProperty
public static TChild WithIsRowSelected<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.DataGridRowHeader, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DataGridRowHeader{value?.SetValue(target, System.Windows.Controls.Primitives.DataGridRowHeader.IsRowSelectedProperty); return target;}

//MasterPageNumberProperty
public static TChild WithMasterPageNumber<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Primitives.DocumentViewerBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.DocumentViewerBase{value?.SetValue(target, System.Windows.Controls.Primitives.DocumentViewerBase.MasterPageNumberProperty); return target;}

//IsMasterPageProperty
public static TChild WithDocumentViewerBase_IsMasterPage<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.Primitives.DocumentViewerBase.IsMasterPageProperty); return target;}

//ColumnsProperty
public static TChild WithColumns<TChild>(this TChild target, ValueProxy<System.Windows.Controls.GridViewColumnCollection>? value, Disambigator<System.Windows.Controls.Primitives.GridViewRowPresenterBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.GridViewRowPresenterBase{value?.SetValue(target, System.Windows.Controls.Primitives.GridViewRowPresenterBase.ColumnsProperty); return target;}
public static TChild WithColumns<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Primitives.UniformGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.UniformGrid{value?.SetValue(target, System.Windows.Controls.Primitives.UniformGrid.ColumnsProperty); return target;}

//ChildProperty
public static TChild WithChild<TChild>(this TChild target, ValueProxy<System.Windows.UIElement>? value, Disambigator<System.Windows.Controls.Primitives.Popup, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Popup{value?.SetValue(target, System.Windows.Controls.Primitives.Popup.ChildProperty); return target;}

//PopupAnimationProperty
public static TChild WithPopupAnimation<TChild>(this TChild target, ValueProxy<System.Windows.Controls.Primitives.PopupAnimation>? value, Disambigator<System.Windows.Controls.Primitives.Popup, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Popup{value?.SetValue(target, System.Windows.Controls.Primitives.Popup.PopupAnimationProperty); return target;}

//MinimumProperty
public static TChild WithMinimum<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.RangeBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.RangeBase{value?.SetValue(target, System.Windows.Controls.Primitives.RangeBase.MinimumProperty); return target;}
public static TChild WithMinimum<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.TickBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TickBar{value?.SetValue(target, System.Windows.Controls.Primitives.TickBar.MinimumProperty); return target;}
public static TChild WithMinimum<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.Track, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Track{value?.SetValue(target, System.Windows.Controls.Primitives.Track.MinimumProperty); return target;}

//MaximumProperty
public static TChild WithMaximum<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.RangeBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.RangeBase{value?.SetValue(target, System.Windows.Controls.Primitives.RangeBase.MaximumProperty); return target;}
public static TChild WithMaximum<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.TickBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TickBar{value?.SetValue(target, System.Windows.Controls.Primitives.TickBar.MaximumProperty); return target;}
public static TChild WithMaximum<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.Track, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Track{value?.SetValue(target, System.Windows.Controls.Primitives.Track.MaximumProperty); return target;}

//LargeChangeProperty
public static TChild WithLargeChange<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.RangeBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.RangeBase{value?.SetValue(target, System.Windows.Controls.Primitives.RangeBase.LargeChangeProperty); return target;}

//SmallChangeProperty
public static TChild WithSmallChange<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.RangeBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.RangeBase{value?.SetValue(target, System.Windows.Controls.Primitives.RangeBase.SmallChangeProperty); return target;}

//ViewportSizeProperty
public static TChild WithViewportSize<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.ScrollBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.ScrollBar{value?.SetValue(target, System.Windows.Controls.Primitives.ScrollBar.ViewportSizeProperty); return target;}
public static TChild WithViewportSize<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.Track, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Track{value?.SetValue(target, System.Windows.Controls.Primitives.Track.ViewportSizeProperty); return target;}

//SelectiveScrollingOrientationProperty
public static TChild WithSelectiveScrollingGrid_SelectiveScrollingOrientation<TChild>(this TChild target, ValueProxy<System.Windows.Controls.SelectiveScrollingOrientation>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Controls.Primitives.SelectiveScrollingGrid.SelectiveScrollingOrientationProperty); return target;}

//IsSynchronizedWithCurrentItemProperty
public static TChild WithIsSynchronizedWithCurrentItem<TChild>(this TChild target, ValueProxy<System.Nullable<System.Boolean>>? value, Disambigator<System.Windows.Controls.Primitives.Selector, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Selector{value?.SetValue(target, System.Windows.Controls.Primitives.Selector.IsSynchronizedWithCurrentItemProperty); return target;}

//SelectedIndexProperty
public static TChild WithSelectedIndex<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Primitives.Selector, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Selector{value?.SetValue(target, System.Windows.Controls.Primitives.Selector.SelectedIndexProperty); return target;}

//IsReadOnlyCaretVisibleProperty
public static TChild WithIsReadOnlyCaretVisible<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.IsReadOnlyCaretVisibleProperty); return target;}

//AcceptsTabProperty
public static TChild WithAcceptsTab<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.AcceptsTabProperty); return target;}

//IsUndoEnabledProperty
public static TChild WithIsUndoEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.IsUndoEnabledProperty); return target;}

//UndoLimitProperty
public static TChild WithUndoLimit<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.UndoLimitProperty); return target;}

//AutoWordSelectionProperty
public static TChild WithAutoWordSelection<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.TextBoxBase, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TextBoxBase{value?.SetValue(target, System.Windows.Controls.Primitives.TextBoxBase.AutoWordSelectionProperty); return target;}

//IsDraggingProperty
public static TChild WithIsDragging<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Controls.Primitives.Thumb, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.Thumb{value?.SetValue(target, System.Windows.Controls.Primitives.Thumb.IsDraggingProperty); return target;}

//ReservedSpaceProperty
public static TChild WithReservedSpace<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.TickBar, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.TickBar{value?.SetValue(target, System.Windows.Controls.Primitives.TickBar.ReservedSpaceProperty); return target;}

//WrapWidthProperty
public static TChild WithWrapWidth<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Controls.Primitives.ToolBarOverflowPanel, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.ToolBarOverflowPanel{value?.SetValue(target, System.Windows.Controls.Primitives.ToolBarOverflowPanel.WrapWidthProperty); return target;}

//FirstColumnProperty
public static TChild WithFirstColumn<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Primitives.UniformGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.UniformGrid{value?.SetValue(target, System.Windows.Controls.Primitives.UniformGrid.FirstColumnProperty); return target;}

//RowsProperty
public static TChild WithRows<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Controls.Primitives.UniformGrid, TChild>? doNotUse = null) where TChild: System.Windows.Controls.Primitives.UniformGrid{value?.SetValue(target, System.Windows.Controls.Primitives.UniformGrid.RowsProperty); return target;}

//IsMouseDirectlyOverProperty
public static TChild WithIsMouseDirectlyOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.IsMouseDirectlyOverProperty); return target;}
public static TChild WithIsMouseDirectlyOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsMouseDirectlyOverProperty); return target;}
public static TChild WithIsMouseDirectlyOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsMouseDirectlyOverProperty); return target;}

//IsMouseOverProperty
public static TChild WithIsMouseOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.IsMouseOverProperty); return target;}
public static TChild WithIsMouseOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsMouseOverProperty); return target;}
public static TChild WithIsMouseOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsMouseOverProperty); return target;}

//IsStylusOverProperty
public static TChild WithIsStylusOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.IsStylusOverProperty); return target;}
public static TChild WithIsStylusOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsStylusOverProperty); return target;}
public static TChild WithIsStylusOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsStylusOverProperty); return target;}

//IsKeyboardFocusWithinProperty
public static TChild WithIsKeyboardFocusWithin<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.IsKeyboardFocusWithinProperty); return target;}
public static TChild WithIsKeyboardFocusWithin<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsKeyboardFocusWithinProperty); return target;}
public static TChild WithIsKeyboardFocusWithin<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsKeyboardFocusWithinProperty); return target;}

//IsMouseCapturedProperty
public static TChild WithIsMouseCaptured<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.IsMouseCapturedProperty); return target;}
public static TChild WithIsMouseCaptured<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsMouseCapturedProperty); return target;}
public static TChild WithIsMouseCaptured<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsMouseCapturedProperty); return target;}

//IsMouseCaptureWithinProperty
public static TChild WithIsMouseCaptureWithin<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.IsMouseCaptureWithinProperty); return target;}
public static TChild WithIsMouseCaptureWithin<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsMouseCaptureWithinProperty); return target;}
public static TChild WithIsMouseCaptureWithin<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsMouseCaptureWithinProperty); return target;}

//IsStylusDirectlyOverProperty
public static TChild WithIsStylusDirectlyOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.IsStylusDirectlyOverProperty); return target;}
public static TChild WithIsStylusDirectlyOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsStylusDirectlyOverProperty); return target;}
public static TChild WithIsStylusDirectlyOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsStylusDirectlyOverProperty); return target;}

//IsStylusCapturedProperty
public static TChild WithIsStylusCaptured<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.IsStylusCapturedProperty); return target;}
public static TChild WithIsStylusCaptured<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsStylusCapturedProperty); return target;}
public static TChild WithIsStylusCaptured<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsStylusCapturedProperty); return target;}

//IsStylusCaptureWithinProperty
public static TChild WithIsStylusCaptureWithin<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.IsStylusCaptureWithinProperty); return target;}
public static TChild WithIsStylusCaptureWithin<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsStylusCaptureWithinProperty); return target;}
public static TChild WithIsStylusCaptureWithin<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsStylusCaptureWithinProperty); return target;}

//IsKeyboardFocusedProperty
public static TChild WithIsKeyboardFocused<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.IsKeyboardFocusedProperty); return target;}
public static TChild WithIsKeyboardFocused<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsKeyboardFocusedProperty); return target;}
public static TChild WithIsKeyboardFocused<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsKeyboardFocusedProperty); return target;}

//AreAnyTouchesDirectlyOverProperty
public static TChild WithAreAnyTouchesDirectlyOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.AreAnyTouchesDirectlyOverProperty); return target;}
public static TChild WithAreAnyTouchesDirectlyOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.AreAnyTouchesDirectlyOverProperty); return target;}
public static TChild WithAreAnyTouchesDirectlyOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.AreAnyTouchesDirectlyOverProperty); return target;}

//AreAnyTouchesOverProperty
public static TChild WithAreAnyTouchesOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.AreAnyTouchesOverProperty); return target;}
public static TChild WithAreAnyTouchesOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.AreAnyTouchesOverProperty); return target;}
public static TChild WithAreAnyTouchesOver<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.AreAnyTouchesOverProperty); return target;}

//AreAnyTouchesCapturedProperty
public static TChild WithAreAnyTouchesCaptured<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.AreAnyTouchesCapturedProperty); return target;}
public static TChild WithAreAnyTouchesCaptured<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.AreAnyTouchesCapturedProperty); return target;}
public static TChild WithAreAnyTouchesCaptured<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.AreAnyTouchesCapturedProperty); return target;}

//AreAnyTouchesCapturedWithinProperty
public static TChild WithAreAnyTouchesCapturedWithin<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.AreAnyTouchesCapturedWithinProperty); return target;}
public static TChild WithAreAnyTouchesCapturedWithin<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.AreAnyTouchesCapturedWithinProperty); return target;}
public static TChild WithAreAnyTouchesCapturedWithin<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.AreAnyTouchesCapturedWithinProperty); return target;}

//IsFocusedProperty
public static TChild WithIsFocused<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.IsFocusedProperty); return target;}
public static TChild WithIsFocused<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsFocusedProperty); return target;}
public static TChild WithIsFocused<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsFocusedProperty); return target;}

//FocusableProperty
public static TChild WithFocusable<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.FocusableProperty); return target;}
public static TChild WithFocusable<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.FocusableProperty); return target;}
public static TChild WithFocusable<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.FocusableProperty); return target;}

//AllowDropProperty
public static TChild WithAllowDrop<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.ContentElement, TChild>? doNotUse = null) where TChild: System.Windows.ContentElement{value?.SetValue(target, System.Windows.ContentElement.AllowDropProperty); return target;}
public static TChild WithAllowDrop<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.AllowDropProperty); return target;}
public static TChild WithAllowDrop<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.AllowDropProperty); return target;}

//TransformProperty
public static TChild WithTransform<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Transform3D>? value, Disambigator<System.Windows.Media.Media3D.Visual3D, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.Visual3D{value?.SetValue(target, System.Windows.Media.Media3D.Visual3D.TransformProperty); return target;}
public static TChild WithTransform<TChild>(this TChild target, ValueProxy<System.Windows.Media.Transform>? value, Disambigator<System.Windows.Media.Brush, TChild>? doNotUse = null) where TChild: System.Windows.Media.Brush{value?.SetValue(target, System.Windows.Media.Brush.TransformProperty); return target;}
public static TChild WithTransform<TChild>(this TChild target, ValueProxy<System.Windows.Media.Transform>? value, Disambigator<System.Windows.Media.Geometry, TChild>? doNotUse = null) where TChild: System.Windows.Media.Geometry{value?.SetValue(target, System.Windows.Media.Geometry.TransformProperty); return target;}
public static TChild WithTransform<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Transform3D>? value, Disambigator<System.Windows.Media.Media3D.Camera, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.Camera{value?.SetValue(target, System.Windows.Media.Media3D.Camera.TransformProperty); return target;}
public static System.Windows.Media.TextEffect WithTransform(this System.Windows.Media.TextEffect target, ValueProxy<System.Windows.Media.Transform>? value) {value?.SetValue(target, System.Windows.Media.TextEffect.TransformProperty); return target;}
public static System.Windows.Media.DrawingGroup WithTransform(this System.Windows.Media.DrawingGroup target, ValueProxy<System.Windows.Media.Transform>? value) {value?.SetValue(target, System.Windows.Media.DrawingGroup.TransformProperty); return target;}
public static TChild WithTransform<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Transform3D>? value, Disambigator<System.Windows.Media.Media3D.Model3D, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.Model3D{value?.SetValue(target, System.Windows.Media.Media3D.Model3D.TransformProperty); return target;}
public static TChild WithTransform<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Transform3D>? value, Disambigator<System.Windows.Media.Media3D.ModelVisual3D, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.ModelVisual3D{value?.SetValue(target, System.Windows.Media.Media3D.ModelVisual3D.TransformProperty); return target;}
public static System.Windows.Media.Imaging.TransformedBitmap WithTransform(this System.Windows.Media.Imaging.TransformedBitmap target, ValueProxy<System.Windows.Media.Transform>? value) {value?.SetValue(target, System.Windows.Media.Imaging.TransformedBitmap.TransformProperty); return target;}

//RenderTransformProperty
public static TChild WithRenderTransform<TChild>(this TChild target, ValueProxy<System.Windows.Media.Transform>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.RenderTransformProperty); return target;}

//RenderTransformOriginProperty
public static TChild WithRenderTransformOrigin<TChild>(this TChild target, ValueProxy<System.Windows.Point>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.RenderTransformOriginProperty); return target;}

//OpacityProperty
public static TChild WithOpacity<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.OpacityProperty); return target;}
public static TChild WithOpacity<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Brush, TChild>? doNotUse = null) where TChild: System.Windows.Media.Brush{value?.SetValue(target, System.Windows.Media.Brush.OpacityProperty); return target;}
public static System.Windows.Media.DrawingGroup WithOpacity(this System.Windows.Media.DrawingGroup target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.DrawingGroup.OpacityProperty); return target;}
public static System.Windows.Media.Effects.DropShadowBitmapEffect WithOpacity(this System.Windows.Media.Effects.DropShadowBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.DropShadowBitmapEffect.OpacityProperty); return target;}
public static System.Windows.Media.Effects.DropShadowEffect WithOpacity(this System.Windows.Media.Effects.DropShadowEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.DropShadowEffect.OpacityProperty); return target;}
public static System.Windows.Media.Effects.OuterGlowBitmapEffect WithOpacity(this System.Windows.Media.Effects.OuterGlowBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.OuterGlowBitmapEffect.OpacityProperty); return target;}

//OpacityMaskProperty
public static TChild WithOpacityMask<TChild>(this TChild target, ValueProxy<System.Windows.Media.Brush>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.OpacityMaskProperty); return target;}
public static System.Windows.Media.DrawingGroup WithOpacityMask(this System.Windows.Media.DrawingGroup target, ValueProxy<System.Windows.Media.Brush>? value) {value?.SetValue(target, System.Windows.Media.DrawingGroup.OpacityMaskProperty); return target;}

//BitmapEffectProperty
public static TChild WithBitmapEffect<TChild>(this TChild target, ValueProxy<System.Windows.Media.Effects.BitmapEffect>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.BitmapEffectProperty); return target;}
public static System.Windows.Media.DrawingGroup WithBitmapEffect(this System.Windows.Media.DrawingGroup target, ValueProxy<System.Windows.Media.Effects.BitmapEffect>? value) {value?.SetValue(target, System.Windows.Media.DrawingGroup.BitmapEffectProperty); return target;}

//EffectProperty
public static TChild WithEffect<TChild>(this TChild target, ValueProxy<System.Windows.Media.Effects.Effect>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.EffectProperty); return target;}

//BitmapEffectInputProperty
public static TChild WithBitmapEffectInput<TChild>(this TChild target, ValueProxy<System.Windows.Media.Effects.BitmapEffectInput>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.BitmapEffectInputProperty); return target;}
public static System.Windows.Media.DrawingGroup WithBitmapEffectInput(this System.Windows.Media.DrawingGroup target, ValueProxy<System.Windows.Media.Effects.BitmapEffectInput>? value) {value?.SetValue(target, System.Windows.Media.DrawingGroup.BitmapEffectInputProperty); return target;}

//CacheModeProperty
public static TChild WithCacheMode<TChild>(this TChild target, ValueProxy<System.Windows.Media.CacheMode>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.CacheModeProperty); return target;}
public static System.Windows.Media.Media3D.Viewport2DVisual3D WithCacheMode(this System.Windows.Media.Media3D.Viewport2DVisual3D target, ValueProxy<System.Windows.Media.CacheMode>? value) {value?.SetValue(target, System.Windows.Media.Media3D.Viewport2DVisual3D.CacheModeProperty); return target;}

//UidProperty
public static TChild WithUid<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.UidProperty); return target;}

//ClipToBoundsProperty
public static TChild WithClipToBounds<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.ClipToBoundsProperty); return target;}

//ClipProperty
public static TChild WithClip<TChild>(this TChild target, ValueProxy<System.Windows.Media.Geometry>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.ClipProperty); return target;}
public static System.Windows.Media.TextEffect WithClip(this System.Windows.Media.TextEffect target, ValueProxy<System.Windows.Media.Geometry>? value) {value?.SetValue(target, System.Windows.Media.TextEffect.ClipProperty); return target;}

//SnapsToDevicePixelsProperty
public static TChild WithSnapsToDevicePixels<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.SnapsToDevicePixelsProperty); return target;}
public static System.Windows.Media.BitmapCache WithSnapsToDevicePixels(this System.Windows.Media.BitmapCache target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Media.BitmapCache.SnapsToDevicePixelsProperty); return target;}

//IsHitTestVisibleProperty
public static TChild WithIsHitTestVisible<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsHitTestVisibleProperty); return target;}
public static TChild WithIsHitTestVisible<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsHitTestVisibleProperty); return target;}

//IsVisibleProperty
public static TChild WithIsVisible<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsVisibleProperty); return target;}
public static TChild WithIsVisible<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement3D, TChild>? doNotUse = null) where TChild: System.Windows.UIElement3D{value?.SetValue(target, System.Windows.UIElement3D.IsVisibleProperty); return target;}

//IsManipulationEnabledProperty
public static TChild WithIsManipulationEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.UIElement, TChild>? doNotUse = null) where TChild: System.Windows.UIElement{value?.SetValue(target, System.Windows.UIElement.IsManipulationEnabledProperty); return target;}

//BaseUriProperty

//RelativeTransformProperty
public static TChild WithRelativeTransform<TChild>(this TChild target, ValueProxy<System.Windows.Media.Transform>? value, Disambigator<System.Windows.Media.Brush, TChild>? doNotUse = null) where TChild: System.Windows.Media.Brush{value?.SetValue(target, System.Windows.Media.Brush.RelativeTransformProperty); return target;}

//ColorProperty
public static System.Windows.Media.SolidColorBrush WithColor(this System.Windows.Media.SolidColorBrush target, ValueProxy<System.Windows.Media.Color>? value) {value?.SetValue(target, System.Windows.Media.SolidColorBrush.ColorProperty); return target;}
public static System.Windows.Media.GradientStop WithColor(this System.Windows.Media.GradientStop target, ValueProxy<System.Windows.Media.Color>? value) {value?.SetValue(target, System.Windows.Media.GradientStop.ColorProperty); return target;}
public static TChild WithColor<TChild>(this TChild target, ValueProxy<System.Windows.Media.Color>? value, Disambigator<System.Windows.Media.Media3D.Light, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.Light{value?.SetValue(target, System.Windows.Media.Media3D.Light.ColorProperty); return target;}
public static System.Windows.Media.Media3D.DiffuseMaterial WithColor(this System.Windows.Media.Media3D.DiffuseMaterial target, ValueProxy<System.Windows.Media.Color>? value) {value?.SetValue(target, System.Windows.Media.Media3D.DiffuseMaterial.ColorProperty); return target;}
public static System.Windows.Media.Media3D.EmissiveMaterial WithColor(this System.Windows.Media.Media3D.EmissiveMaterial target, ValueProxy<System.Windows.Media.Color>? value) {value?.SetValue(target, System.Windows.Media.Media3D.EmissiveMaterial.ColorProperty); return target;}
public static System.Windows.Media.Media3D.SpecularMaterial WithColor(this System.Windows.Media.Media3D.SpecularMaterial target, ValueProxy<System.Windows.Media.Color>? value) {value?.SetValue(target, System.Windows.Media.Media3D.SpecularMaterial.ColorProperty); return target;}
public static System.Windows.Media.Effects.DropShadowBitmapEffect WithColor(this System.Windows.Media.Effects.DropShadowBitmapEffect target, ValueProxy<System.Windows.Media.Color>? value) {value?.SetValue(target, System.Windows.Media.Effects.DropShadowBitmapEffect.ColorProperty); return target;}
public static System.Windows.Media.Effects.DropShadowEffect WithColor(this System.Windows.Media.Effects.DropShadowEffect target, ValueProxy<System.Windows.Media.Color>? value) {value?.SetValue(target, System.Windows.Media.Effects.DropShadowEffect.ColorProperty); return target;}

//MatrixProperty
public static System.Windows.Media.MatrixTransform WithMatrix(this System.Windows.Media.MatrixTransform target, ValueProxy<System.Windows.Media.Matrix>? value) {value?.SetValue(target, System.Windows.Media.MatrixTransform.MatrixProperty); return target;}
public static System.Windows.Media.Media3D.MatrixTransform3D WithMatrix(this System.Windows.Media.Media3D.MatrixTransform3D target, ValueProxy<System.Windows.Media.Media3D.Matrix3D>? value) {value?.SetValue(target, System.Windows.Media.Media3D.MatrixTransform3D.MatrixProperty); return target;}

//FiguresProperty
public static System.Windows.Media.PathGeometry WithFigures(this System.Windows.Media.PathGeometry target, ValueProxy<System.Windows.Media.PathFigureCollection>? value) {value?.SetValue(target, System.Windows.Media.PathGeometry.FiguresProperty); return target;}

//VisualProperty
public static System.Windows.Media.Media3D.Viewport2DVisual3D WithVisual(this System.Windows.Media.Media3D.Viewport2DVisual3D target, ValueProxy<System.Windows.Media.Visual>? value) {value?.SetValue(target, System.Windows.Media.Media3D.Viewport2DVisual3D.VisualProperty); return target;}
public static System.Windows.Media.VisualBrush WithVisual(this System.Windows.Media.VisualBrush target, ValueProxy<System.Windows.Media.Visual>? value) {value?.SetValue(target, System.Windows.Media.VisualBrush.VisualProperty); return target;}

//GeometryProperty
public static System.Windows.Media.Media3D.Viewport2DVisual3D WithGeometry(this System.Windows.Media.Media3D.Viewport2DVisual3D target, ValueProxy<System.Windows.Media.Media3D.Geometry3D>? value) {value?.SetValue(target, System.Windows.Media.Media3D.Viewport2DVisual3D.GeometryProperty); return target;}
public static System.Windows.Media.GeometryDrawing WithGeometry(this System.Windows.Media.GeometryDrawing target, ValueProxy<System.Windows.Media.Geometry>? value) {value?.SetValue(target, System.Windows.Media.GeometryDrawing.GeometryProperty); return target;}
public static System.Windows.Media.Media3D.GeometryModel3D WithGeometry(this System.Windows.Media.Media3D.GeometryModel3D target, ValueProxy<System.Windows.Media.Media3D.Geometry3D>? value) {value?.SetValue(target, System.Windows.Media.Media3D.GeometryModel3D.GeometryProperty); return target;}

//MaterialProperty
public static System.Windows.Media.Media3D.Viewport2DVisual3D WithMaterial(this System.Windows.Media.Media3D.Viewport2DVisual3D target, ValueProxy<System.Windows.Media.Media3D.Material>? value) {value?.SetValue(target, System.Windows.Media.Media3D.Viewport2DVisual3D.MaterialProperty); return target;}
public static System.Windows.Media.Media3D.GeometryModel3D WithMaterial(this System.Windows.Media.Media3D.GeometryModel3D target, ValueProxy<System.Windows.Media.Media3D.Material>? value) {value?.SetValue(target, System.Windows.Media.Media3D.GeometryModel3D.MaterialProperty); return target;}

//IsVisualHostMaterialProperty
public static TChild WithViewport2DVisual3D_IsVisualHostMaterial<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Media.Media3D.Material, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.Material{value?.SetValue(target, System.Windows.Media.Media3D.Viewport2DVisual3D.IsVisualHostMaterialProperty); return target;}

//PositionsProperty
public static System.Windows.Media.Media3D.MeshGeometry3D WithPositions(this System.Windows.Media.Media3D.MeshGeometry3D target, ValueProxy<System.Windows.Media.Media3D.Point3DCollection>? value) {value?.SetValue(target, System.Windows.Media.Media3D.MeshGeometry3D.PositionsProperty); return target;}

//NormalsProperty
public static System.Windows.Media.Media3D.MeshGeometry3D WithNormals(this System.Windows.Media.Media3D.MeshGeometry3D target, ValueProxy<System.Windows.Media.Media3D.Vector3DCollection>? value) {value?.SetValue(target, System.Windows.Media.Media3D.MeshGeometry3D.NormalsProperty); return target;}

//TextureCoordinatesProperty
public static System.Windows.Media.Media3D.MeshGeometry3D WithTextureCoordinates(this System.Windows.Media.Media3D.MeshGeometry3D target, ValueProxy<System.Windows.Media.PointCollection>? value) {value?.SetValue(target, System.Windows.Media.Media3D.MeshGeometry3D.TextureCoordinatesProperty); return target;}

//TriangleIndicesProperty
public static System.Windows.Media.Media3D.MeshGeometry3D WithTriangleIndices(this System.Windows.Media.Media3D.MeshGeometry3D target, ValueProxy<System.Windows.Media.Int32Collection>? value) {value?.SetValue(target, System.Windows.Media.Media3D.MeshGeometry3D.TriangleIndicesProperty); return target;}

//ViewportProperty
public static System.Windows.Media.Media3D.Viewport3DVisual WithViewport(this System.Windows.Media.Media3D.Viewport3DVisual target, ValueProxy<System.Windows.Rect>? value) {value?.SetValue(target, System.Windows.Media.Media3D.Viewport3DVisual.ViewportProperty); return target;}
public static TChild WithViewport<TChild>(this TChild target, ValueProxy<System.Windows.Rect>? value, Disambigator<System.Windows.Media.TileBrush, TChild>? doNotUse = null) where TChild: System.Windows.Media.TileBrush{value?.SetValue(target, System.Windows.Media.TileBrush.ViewportProperty); return target;}

//CultureSourceProperty
public static TChild WithNumberSubstitution_CultureSource<TChild>(this TChild target, ValueProxy<System.Windows.Media.NumberCultureSource>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.NumberSubstitution.CultureSourceProperty); return target;}

//CultureOverrideProperty
public static TChild WithNumberSubstitution_CultureOverride<TChild>(this TChild target, ValueProxy<System.Globalization.CultureInfo>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.NumberSubstitution.CultureOverrideProperty); return target;}

//SubstitutionProperty
public static TChild WithNumberSubstitution_Substitution<TChild>(this TChild target, ValueProxy<System.Windows.Media.NumberSubstitutionMethod>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.NumberSubstitution.SubstitutionProperty); return target;}

//PenProperty
public static System.Windows.TextDecoration WithPen(this System.Windows.TextDecoration target, ValueProxy<System.Windows.Media.Pen>? value) {value?.SetValue(target, System.Windows.TextDecoration.PenProperty); return target;}
public static System.Windows.Media.GeometryDrawing WithPen(this System.Windows.Media.GeometryDrawing target, ValueProxy<System.Windows.Media.Pen>? value) {value?.SetValue(target, System.Windows.Media.GeometryDrawing.PenProperty); return target;}

//PenOffsetProperty
public static System.Windows.TextDecoration WithPenOffset(this System.Windows.TextDecoration target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.TextDecoration.PenOffsetProperty); return target;}

//PenOffsetUnitProperty
public static System.Windows.TextDecoration WithPenOffsetUnit(this System.Windows.TextDecoration target, ValueProxy<System.Windows.TextDecorationUnit>? value) {value?.SetValue(target, System.Windows.TextDecoration.PenOffsetUnitProperty); return target;}

//PenThicknessUnitProperty
public static System.Windows.TextDecoration WithPenThicknessUnit(this System.Windows.TextDecoration target, ValueProxy<System.Windows.TextDecorationUnit>? value) {value?.SetValue(target, System.Windows.TextDecoration.PenThicknessUnitProperty); return target;}

//LocationProperty
public static System.Windows.TextDecoration WithLocation(this System.Windows.TextDecoration target, ValueProxy<System.Windows.TextDecorationLocation>? value) {value?.SetValue(target, System.Windows.TextDecoration.LocationProperty); return target;}

//ScaleXProperty
public static System.Windows.Media.ScaleTransform WithScaleX(this System.Windows.Media.ScaleTransform target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.ScaleTransform.ScaleXProperty); return target;}
public static System.Windows.Media.Media3D.ScaleTransform3D WithScaleX(this System.Windows.Media.Media3D.ScaleTransform3D target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.ScaleTransform3D.ScaleXProperty); return target;}

//ScaleYProperty
public static System.Windows.Media.ScaleTransform WithScaleY(this System.Windows.Media.ScaleTransform target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.ScaleTransform.ScaleYProperty); return target;}
public static System.Windows.Media.Media3D.ScaleTransform3D WithScaleY(this System.Windows.Media.Media3D.ScaleTransform3D target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.ScaleTransform3D.ScaleYProperty); return target;}

//CenterXProperty
public static System.Windows.Media.ScaleTransform WithCenterX(this System.Windows.Media.ScaleTransform target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.ScaleTransform.CenterXProperty); return target;}
public static System.Windows.Media.RotateTransform WithCenterX(this System.Windows.Media.RotateTransform target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.RotateTransform.CenterXProperty); return target;}
public static System.Windows.Media.SkewTransform WithCenterX(this System.Windows.Media.SkewTransform target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.SkewTransform.CenterXProperty); return target;}
public static System.Windows.Media.Media3D.RotateTransform3D WithCenterX(this System.Windows.Media.Media3D.RotateTransform3D target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.RotateTransform3D.CenterXProperty); return target;}
public static System.Windows.Media.Media3D.ScaleTransform3D WithCenterX(this System.Windows.Media.Media3D.ScaleTransform3D target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.ScaleTransform3D.CenterXProperty); return target;}

//CenterYProperty
public static System.Windows.Media.ScaleTransform WithCenterY(this System.Windows.Media.ScaleTransform target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.ScaleTransform.CenterYProperty); return target;}
public static System.Windows.Media.RotateTransform WithCenterY(this System.Windows.Media.RotateTransform target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.RotateTransform.CenterYProperty); return target;}
public static System.Windows.Media.SkewTransform WithCenterY(this System.Windows.Media.SkewTransform target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.SkewTransform.CenterYProperty); return target;}
public static System.Windows.Media.Media3D.RotateTransform3D WithCenterY(this System.Windows.Media.Media3D.RotateTransform3D target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.RotateTransform3D.CenterYProperty); return target;}
public static System.Windows.Media.Media3D.ScaleTransform3D WithCenterY(this System.Windows.Media.Media3D.ScaleTransform3D target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.ScaleTransform3D.CenterYProperty); return target;}

//XProperty
public static System.Windows.Media.TranslateTransform WithX(this System.Windows.Media.TranslateTransform target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.TranslateTransform.XProperty); return target;}

//YProperty
public static System.Windows.Media.TranslateTransform WithY(this System.Windows.Media.TranslateTransform target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.TranslateTransform.YProperty); return target;}

//BrushProperty
public static System.Windows.Media.Pen WithBrush(this System.Windows.Media.Pen target, ValueProxy<System.Windows.Media.Brush>? value) {value?.SetValue(target, System.Windows.Media.Pen.BrushProperty); return target;}
public static System.Windows.Media.GeometryDrawing WithBrush(this System.Windows.Media.GeometryDrawing target, ValueProxy<System.Windows.Media.Brush>? value) {value?.SetValue(target, System.Windows.Media.GeometryDrawing.BrushProperty); return target;}
public static System.Windows.Media.Media3D.DiffuseMaterial WithBrush(this System.Windows.Media.Media3D.DiffuseMaterial target, ValueProxy<System.Windows.Media.Brush>? value) {value?.SetValue(target, System.Windows.Media.Media3D.DiffuseMaterial.BrushProperty); return target;}
public static System.Windows.Media.Media3D.EmissiveMaterial WithBrush(this System.Windows.Media.Media3D.EmissiveMaterial target, ValueProxy<System.Windows.Media.Brush>? value) {value?.SetValue(target, System.Windows.Media.Media3D.EmissiveMaterial.BrushProperty); return target;}
public static System.Windows.Media.Media3D.SpecularMaterial WithBrush(this System.Windows.Media.Media3D.SpecularMaterial target, ValueProxy<System.Windows.Media.Brush>? value) {value?.SetValue(target, System.Windows.Media.Media3D.SpecularMaterial.BrushProperty); return target;}


//ThicknessProperty
public static System.Windows.Media.Pen WithThickness(this System.Windows.Media.Pen target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Pen.ThicknessProperty); return target;}

//StartLineCapProperty
public static System.Windows.Media.Pen WithStartLineCap(this System.Windows.Media.Pen target, ValueProxy<System.Windows.Media.PenLineCap>? value) {value?.SetValue(target, System.Windows.Media.Pen.StartLineCapProperty); return target;}

//EndLineCapProperty
public static System.Windows.Media.Pen WithEndLineCap(this System.Windows.Media.Pen target, ValueProxy<System.Windows.Media.PenLineCap>? value) {value?.SetValue(target, System.Windows.Media.Pen.EndLineCapProperty); return target;}

//DashCapProperty
public static System.Windows.Media.Pen WithDashCap(this System.Windows.Media.Pen target, ValueProxy<System.Windows.Media.PenLineCap>? value) {value?.SetValue(target, System.Windows.Media.Pen.DashCapProperty); return target;}

//LineJoinProperty
public static System.Windows.Media.Pen WithLineJoin(this System.Windows.Media.Pen target, ValueProxy<System.Windows.Media.PenLineJoin>? value) {value?.SetValue(target, System.Windows.Media.Pen.LineJoinProperty); return target;}

//MiterLimitProperty
public static System.Windows.Media.Pen WithMiterLimit(this System.Windows.Media.Pen target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Pen.MiterLimitProperty); return target;}

//DashStyleProperty
public static System.Windows.Media.Pen WithDashStyle(this System.Windows.Media.Pen target, ValueProxy<System.Windows.Media.DashStyle>? value) {value?.SetValue(target, System.Windows.Media.Pen.DashStyleProperty); return target;}

//PositionStartProperty
public static System.Windows.Media.TextEffect WithPositionStart(this System.Windows.Media.TextEffect target, ValueProxy<System.Int32>? value) {value?.SetValue(target, System.Windows.Media.TextEffect.PositionStartProperty); return target;}

//PositionCountProperty
public static System.Windows.Media.TextEffect WithPositionCount(this System.Windows.Media.TextEffect target, ValueProxy<System.Int32>? value) {value?.SetValue(target, System.Windows.Media.TextEffect.PositionCountProperty); return target;}

//QuaternionProperty
public static System.Windows.Media.Media3D.QuaternionRotation3D WithQuaternion(this System.Windows.Media.Media3D.QuaternionRotation3D target, ValueProxy<System.Windows.Media.Media3D.Quaternion>? value) {value?.SetValue(target, System.Windows.Media.Media3D.QuaternionRotation3D.QuaternionProperty); return target;}

//IsAdditiveProperty

//IsCumulativeProperty

//IsPressAndHoldEnabledProperty
public static TChild WithStylus_IsPressAndHoldEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.Stylus.IsPressAndHoldEnabledProperty); return target;}

//IsFlicksEnabledProperty
public static TChild WithStylus_IsFlicksEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.Stylus.IsFlicksEnabledProperty); return target;}

//IsTapFeedbackEnabledProperty
public static TChild WithStylus_IsTapFeedbackEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.Stylus.IsTapFeedbackEnabledProperty); return target;}

//IsTouchFeedbackEnabledProperty
public static TChild WithStylus_IsTouchFeedbackEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.Stylus.IsTouchFeedbackEnabledProperty); return target;}

//FocusedElementProperty
public static TChild WithFocusManager_FocusedElement<TChild>(this TChild target, ValueProxy<System.Windows.IInputElement>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.FocusManager.FocusedElementProperty); return target;}

//IsFocusScopeProperty
public static TChild WithFocusManager_IsFocusScope<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.FocusManager.IsFocusScopeProperty); return target;}

//IsInputMethodEnabledProperty
public static TChild WithInputMethod_IsInputMethodEnabled<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.InputMethod.IsInputMethodEnabledProperty); return target;}

//IsInputMethodSuspendedProperty
public static TChild WithInputMethod_IsInputMethodSuspended<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.InputMethod.IsInputMethodSuspendedProperty); return target;}

//PreferredImeStateProperty
public static TChild WithInputMethod_PreferredImeState<TChild>(this TChild target, ValueProxy<System.Windows.Input.InputMethodState>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.InputMethod.PreferredImeStateProperty); return target;}

//PreferredImeConversionModeProperty
public static TChild WithInputMethod_PreferredImeConversionMode<TChild>(this TChild target, ValueProxy<System.Windows.Input.ImeConversionModeValues>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.InputMethod.PreferredImeConversionModeProperty); return target;}

//PreferredImeSentenceModeProperty
public static TChild WithInputMethod_PreferredImeSentenceMode<TChild>(this TChild target, ValueProxy<System.Windows.Input.ImeSentenceModeValues>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.InputMethod.PreferredImeSentenceModeProperty); return target;}

//InputProperty
public static System.Windows.Media.Effects.BitmapEffectInput WithInput(this System.Windows.Media.Effects.BitmapEffectInput target, ValueProxy<System.Windows.Media.Imaging.BitmapSource>? value) {value?.SetValue(target, System.Windows.Media.Effects.BitmapEffectInput.InputProperty); return target;}

//AreaToApplyEffectUnitsProperty
public static System.Windows.Media.Effects.BitmapEffectInput WithAreaToApplyEffectUnits(this System.Windows.Media.Effects.BitmapEffectInput target, ValueProxy<System.Windows.Media.BrushMappingMode>? value) {value?.SetValue(target, System.Windows.Media.Effects.BitmapEffectInput.AreaToApplyEffectUnitsProperty); return target;}

//AreaToApplyEffectProperty
public static System.Windows.Media.Effects.BitmapEffectInput WithAreaToApplyEffect(this System.Windows.Media.Effects.BitmapEffectInput target, ValueProxy<System.Windows.Rect>? value) {value?.SetValue(target, System.Windows.Media.Effects.BitmapEffectInput.AreaToApplyEffectProperty); return target;}

//EdgeModeProperty
public static TChild WithRenderOptions_EdgeMode<TChild>(this TChild target, ValueProxy<System.Windows.Media.EdgeMode>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.RenderOptions.EdgeModeProperty); return target;}

//BitmapScalingModeProperty
public static TChild WithRenderOptions_BitmapScalingMode<TChild>(this TChild target, ValueProxy<System.Windows.Media.BitmapScalingMode>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.RenderOptions.BitmapScalingModeProperty); return target;}

//ClearTypeHintProperty
public static TChild WithRenderOptions_ClearTypeHint<TChild>(this TChild target, ValueProxy<System.Windows.Media.ClearTypeHint>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.RenderOptions.ClearTypeHintProperty); return target;}

//CachingHintProperty
public static TChild WithRenderOptions_CachingHint<TChild>(this TChild target, ValueProxy<System.Windows.Media.CachingHint>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.RenderOptions.CachingHintProperty); return target;}

//CacheInvalidationThresholdMinimumProperty
public static TChild WithRenderOptions_CacheInvalidationThresholdMinimum<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.RenderOptions.CacheInvalidationThresholdMinimumProperty); return target;}

//CacheInvalidationThresholdMaximumProperty
public static TChild WithRenderOptions_CacheInvalidationThresholdMaximum<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Media.RenderOptions.CacheInvalidationThresholdMaximumProperty); return target;}

//GeometryCombineModeProperty
public static System.Windows.Media.CombinedGeometry WithGeometryCombineMode(this System.Windows.Media.CombinedGeometry target, ValueProxy<System.Windows.Media.GeometryCombineMode>? value) {value?.SetValue(target, System.Windows.Media.CombinedGeometry.GeometryCombineModeProperty); return target;}

//Geometry1Property
public static System.Windows.Media.CombinedGeometry WithGeometry1(this System.Windows.Media.CombinedGeometry target, ValueProxy<System.Windows.Media.Geometry>? value) {value?.SetValue(target, System.Windows.Media.CombinedGeometry.Geometry1Property); return target;}

//Geometry2Property
public static System.Windows.Media.CombinedGeometry WithGeometry2(this System.Windows.Media.CombinedGeometry target, ValueProxy<System.Windows.Media.Geometry>? value) {value?.SetValue(target, System.Windows.Media.CombinedGeometry.Geometry2Property); return target;}

//ClipGeometryProperty
public static System.Windows.Media.DrawingGroup WithClipGeometry(this System.Windows.Media.DrawingGroup target, ValueProxy<System.Windows.Media.Geometry>? value) {value?.SetValue(target, System.Windows.Media.DrawingGroup.ClipGeometryProperty); return target;}

//GuidelineSetProperty
public static System.Windows.Media.DrawingGroup WithGuidelineSet(this System.Windows.Media.DrawingGroup target, ValueProxy<System.Windows.Media.GuidelineSet>? value) {value?.SetValue(target, System.Windows.Media.DrawingGroup.GuidelineSetProperty); return target;}

//RectProperty
public static System.Windows.Media.RectangleGeometry WithRect(this System.Windows.Media.RectangleGeometry target, ValueProxy<System.Windows.Rect>? value) {value?.SetValue(target, System.Windows.Media.RectangleGeometry.RectProperty); return target;}
public static System.Windows.Media.ImageDrawing WithRect(this System.Windows.Media.ImageDrawing target, ValueProxy<System.Windows.Rect>? value) {value?.SetValue(target, System.Windows.Media.ImageDrawing.RectProperty); return target;}
public static System.Windows.Media.VideoDrawing WithRect(this System.Windows.Media.VideoDrawing target, ValueProxy<System.Windows.Rect>? value) {value?.SetValue(target, System.Windows.Media.VideoDrawing.RectProperty); return target;}

//InputLanguageProperty
public static TChild WithInputLanguageManager_InputLanguage<TChild>(this TChild target, ValueProxy<System.Globalization.CultureInfo>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.InputLanguageManager.InputLanguageProperty); return target;}

//RestoreInputLanguageProperty
public static TChild WithInputLanguageManager_RestoreInputLanguage<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Input.InputLanguageManager.RestoreInputLanguageProperty); return target;}

//ModifiersProperty
public static TChild WithModifiers<TChild>(this TChild target, ValueProxy<System.Windows.Input.ModifierKeys>? value, Disambigator<System.Windows.Input.KeyBinding, TChild>? doNotUse = null) where TChild: System.Windows.Input.KeyBinding{value?.SetValue(target, System.Windows.Input.KeyBinding.ModifiersProperty); return target;}

//KeyProperty
public static TChild WithKey<TChild>(this TChild target, ValueProxy<System.Windows.Input.Key>? value, Disambigator<System.Windows.Input.KeyBinding, TChild>? doNotUse = null) where TChild: System.Windows.Input.KeyBinding{value?.SetValue(target, System.Windows.Input.KeyBinding.KeyProperty); return target;}

//MouseActionProperty
public static TChild WithMouseAction<TChild>(this TChild target, ValueProxy<System.Windows.Input.MouseAction>? value, Disambigator<System.Windows.Input.MouseBinding, TChild>? doNotUse = null) where TChild: System.Windows.Input.MouseBinding{value?.SetValue(target, System.Windows.Input.MouseBinding.MouseActionProperty); return target;}

//AutomationIdProperty
public static TChild WithAutomationProperties_AutomationId<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.AutomationIdProperty); return target;}

//HelpTextProperty
public static TChild WithAutomationProperties_HelpText<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.HelpTextProperty); return target;}

//AcceleratorKeyProperty
public static TChild WithAutomationProperties_AcceleratorKey<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.AcceleratorKeyProperty); return target;}

//AccessKeyProperty
public static TChild WithAutomationProperties_AccessKey<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.AccessKeyProperty); return target;}

//ItemStatusProperty
public static TChild WithAutomationProperties_ItemStatus<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.ItemStatusProperty); return target;}

//ItemTypeProperty
public static TChild WithAutomationProperties_ItemType<TChild>(this TChild target, ValueProxy<System.String>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.ItemTypeProperty); return target;}

//IsColumnHeaderProperty
public static TChild WithAutomationProperties_IsColumnHeader<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.IsColumnHeaderProperty); return target;}

//IsRowHeaderProperty
public static TChild WithAutomationProperties_IsRowHeader<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.IsRowHeaderProperty); return target;}

//IsRequiredForFormProperty
public static TChild WithAutomationProperties_IsRequiredForForm<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.IsRequiredForFormProperty); return target;}

//LabeledByProperty
public static TChild WithAutomationProperties_LabeledBy<TChild>(this TChild target, ValueProxy<System.Windows.UIElement>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.LabeledByProperty); return target;}

//IsOffscreenBehaviorProperty
public static TChild WithAutomationProperties_IsOffscreenBehavior<TChild>(this TChild target, ValueProxy<System.Windows.Automation.IsOffscreenBehavior>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.IsOffscreenBehaviorProperty); return target;}

//LiveSettingProperty
public static TChild WithAutomationProperties_LiveSetting<TChild>(this TChild target, ValueProxy<System.Windows.Automation.AutomationLiveSetting>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.LiveSettingProperty); return target;}

//PositionInSetProperty
public static TChild WithAutomationProperties_PositionInSet<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.PositionInSetProperty); return target;}

//SizeOfSetProperty
public static TChild WithAutomationProperties_SizeOfSet<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.DependencyObject, TChild>? doNotUse = null) where TChild: System.Windows.DependencyObject{value?.SetValue(target, System.Windows.Automation.AutomationProperties.SizeOfSetProperty); return target;}

//IsFrontBufferAvailableProperty
public static TChild WithIsFrontBufferAvailable<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Interop.D3DImage, TChild>? doNotUse = null) where TChild: System.Windows.Interop.D3DImage{value?.SetValue(target, System.Windows.Interop.D3DImage.IsFrontBufferAvailableProperty); return target;}

//PointProperty
public static System.Windows.Media.ArcSegment WithPoint(this System.Windows.Media.ArcSegment target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.ArcSegment.PointProperty); return target;}
public static System.Windows.Media.LineSegment WithPoint(this System.Windows.Media.LineSegment target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.LineSegment.PointProperty); return target;}

//SizeProperty
public static System.Windows.Media.ArcSegment WithSize(this System.Windows.Media.ArcSegment target, ValueProxy<System.Windows.Size>? value) {value?.SetValue(target, System.Windows.Media.ArcSegment.SizeProperty); return target;}

//RotationAngleProperty
public static System.Windows.Media.ArcSegment WithRotationAngle(this System.Windows.Media.ArcSegment target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.ArcSegment.RotationAngleProperty); return target;}

//IsLargeArcProperty
public static System.Windows.Media.ArcSegment WithIsLargeArc(this System.Windows.Media.ArcSegment target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Media.ArcSegment.IsLargeArcProperty); return target;}

//SweepDirectionProperty
public static System.Windows.Media.ArcSegment WithSweepDirection(this System.Windows.Media.ArcSegment target, ValueProxy<System.Windows.Media.SweepDirection>? value) {value?.SetValue(target, System.Windows.Media.ArcSegment.SweepDirectionProperty); return target;}

//IsStrokedProperty
public static TChild WithIsStroked<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Media.PathSegment, TChild>? doNotUse = null) where TChild: System.Windows.Media.PathSegment{value?.SetValue(target, System.Windows.Media.PathSegment.IsStrokedProperty); return target;}

//IsSmoothJoinProperty
public static TChild WithIsSmoothJoin<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Media.PathSegment, TChild>? doNotUse = null) where TChild: System.Windows.Media.PathSegment{value?.SetValue(target, System.Windows.Media.PathSegment.IsSmoothJoinProperty); return target;}

//StartPointProperty
public static System.Windows.Media.PathFigure WithStartPoint(this System.Windows.Media.PathFigure target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.PathFigure.StartPointProperty); return target;}
public static System.Windows.Media.LineGeometry WithStartPoint(this System.Windows.Media.LineGeometry target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.LineGeometry.StartPointProperty); return target;}
public static System.Windows.Media.LinearGradientBrush WithStartPoint(this System.Windows.Media.LinearGradientBrush target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.LinearGradientBrush.StartPointProperty); return target;}

//IsFilledProperty
public static System.Windows.Media.PathFigure WithIsFilled(this System.Windows.Media.PathFigure target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Media.PathFigure.IsFilledProperty); return target;}

//SegmentsProperty
public static System.Windows.Media.PathFigure WithSegments(this System.Windows.Media.PathFigure target, ValueProxy<System.Windows.Media.PathSegmentCollection>? value) {value?.SetValue(target, System.Windows.Media.PathFigure.SegmentsProperty); return target;}

//IsClosedProperty
public static System.Windows.Media.PathFigure WithIsClosed(this System.Windows.Media.PathFigure target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Media.PathFigure.IsClosedProperty); return target;}

//Point1Property
public static System.Windows.Media.BezierSegment WithPoint1(this System.Windows.Media.BezierSegment target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.BezierSegment.Point1Property); return target;}
public static System.Windows.Media.QuadraticBezierSegment WithPoint1(this System.Windows.Media.QuadraticBezierSegment target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.QuadraticBezierSegment.Point1Property); return target;}

//Point2Property
public static System.Windows.Media.BezierSegment WithPoint2(this System.Windows.Media.BezierSegment target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.BezierSegment.Point2Property); return target;}
public static System.Windows.Media.QuadraticBezierSegment WithPoint2(this System.Windows.Media.QuadraticBezierSegment target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.QuadraticBezierSegment.Point2Property); return target;}

//Point3Property
public static System.Windows.Media.BezierSegment WithPoint3(this System.Windows.Media.BezierSegment target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.BezierSegment.Point3Property); return target;}

//RenderAtScaleProperty
public static System.Windows.Media.BitmapCache WithRenderAtScale(this System.Windows.Media.BitmapCache target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.BitmapCache.RenderAtScaleProperty); return target;}

//EnableClearTypeProperty
public static System.Windows.Media.BitmapCache WithEnableClearType(this System.Windows.Media.BitmapCache target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Media.BitmapCache.EnableClearTypeProperty); return target;}

//BitmapCacheProperty
public static System.Windows.Media.BitmapCacheBrush WithBitmapCache(this System.Windows.Media.BitmapCacheBrush target, ValueProxy<System.Windows.Media.BitmapCache>? value) {value?.SetValue(target, System.Windows.Media.BitmapCacheBrush.BitmapCacheProperty); return target;}

//AutoLayoutContentProperty
public static System.Windows.Media.BitmapCacheBrush WithAutoLayoutContent(this System.Windows.Media.BitmapCacheBrush target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Media.BitmapCacheBrush.AutoLayoutContentProperty); return target;}
public static System.Windows.Media.VisualBrush WithAutoLayoutContent(this System.Windows.Media.VisualBrush target, ValueProxy<System.Boolean>? value) {value?.SetValue(target, System.Windows.Media.VisualBrush.AutoLayoutContentProperty); return target;}

//EndPointProperty
public static System.Windows.Media.LineGeometry WithEndPoint(this System.Windows.Media.LineGeometry target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.LineGeometry.EndPointProperty); return target;}
public static System.Windows.Media.LinearGradientBrush WithEndPoint(this System.Windows.Media.LinearGradientBrush target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.LinearGradientBrush.EndPointProperty); return target;}

//CenterProperty
public static System.Windows.Media.EllipseGeometry WithCenter(this System.Windows.Media.EllipseGeometry target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.EllipseGeometry.CenterProperty); return target;}
public static System.Windows.Media.RadialGradientBrush WithCenter(this System.Windows.Media.RadialGradientBrush target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.RadialGradientBrush.CenterProperty); return target;}

//GuidelinesXProperty
public static System.Windows.Media.GuidelineSet WithGuidelinesX(this System.Windows.Media.GuidelineSet target, ValueProxy<System.Windows.Media.DoubleCollection>? value) {value?.SetValue(target, System.Windows.Media.GuidelineSet.GuidelinesXProperty); return target;}

//GuidelinesYProperty
public static System.Windows.Media.GuidelineSet WithGuidelinesY(this System.Windows.Media.GuidelineSet target, ValueProxy<System.Windows.Media.DoubleCollection>? value) {value?.SetValue(target, System.Windows.Media.GuidelineSet.GuidelinesYProperty); return target;}

//OffsetProperty
public static System.Windows.Media.DashStyle WithOffset(this System.Windows.Media.DashStyle target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.DashStyle.OffsetProperty); return target;}
public static System.Windows.Media.GradientStop WithOffset(this System.Windows.Media.GradientStop target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.GradientStop.OffsetProperty); return target;}

//DashesProperty
public static System.Windows.Media.DashStyle WithDashes(this System.Windows.Media.DashStyle target, ValueProxy<System.Windows.Media.DoubleCollection>? value) {value?.SetValue(target, System.Windows.Media.DashStyle.DashesProperty); return target;}

//DrawingProperty
public static System.Windows.Media.DrawingBrush WithDrawing(this System.Windows.Media.DrawingBrush target, ValueProxy<System.Windows.Media.Drawing>? value) {value?.SetValue(target, System.Windows.Media.DrawingBrush.DrawingProperty); return target;}
public static System.Windows.Media.DrawingImage WithDrawing(this System.Windows.Media.DrawingImage target, ValueProxy<System.Windows.Media.Drawing>? value) {value?.SetValue(target, System.Windows.Media.DrawingImage.DrawingProperty); return target;}

//ViewportUnitsProperty
public static TChild WithViewportUnits<TChild>(this TChild target, ValueProxy<System.Windows.Media.BrushMappingMode>? value, Disambigator<System.Windows.Media.TileBrush, TChild>? doNotUse = null) where TChild: System.Windows.Media.TileBrush{value?.SetValue(target, System.Windows.Media.TileBrush.ViewportUnitsProperty); return target;}

//ViewboxUnitsProperty
public static TChild WithViewboxUnits<TChild>(this TChild target, ValueProxy<System.Windows.Media.BrushMappingMode>? value, Disambigator<System.Windows.Media.TileBrush, TChild>? doNotUse = null) where TChild: System.Windows.Media.TileBrush{value?.SetValue(target, System.Windows.Media.TileBrush.ViewboxUnitsProperty); return target;}

//ViewboxProperty
public static TChild WithViewbox<TChild>(this TChild target, ValueProxy<System.Windows.Rect>? value, Disambigator<System.Windows.Media.TileBrush, TChild>? doNotUse = null) where TChild: System.Windows.Media.TileBrush{value?.SetValue(target, System.Windows.Media.TileBrush.ViewboxProperty); return target;}

//TileModeProperty
public static TChild WithTileMode<TChild>(this TChild target, ValueProxy<System.Windows.Media.TileMode>? value, Disambigator<System.Windows.Media.TileBrush, TChild>? doNotUse = null) where TChild: System.Windows.Media.TileBrush{value?.SetValue(target, System.Windows.Media.TileBrush.TileModeProperty); return target;}

//AlignmentXProperty
public static TChild WithAlignmentX<TChild>(this TChild target, ValueProxy<System.Windows.Media.AlignmentX>? value, Disambigator<System.Windows.Media.TileBrush, TChild>? doNotUse = null) where TChild: System.Windows.Media.TileBrush{value?.SetValue(target, System.Windows.Media.TileBrush.AlignmentXProperty); return target;}

//AlignmentYProperty
public static TChild WithAlignmentY<TChild>(this TChild target, ValueProxy<System.Windows.Media.AlignmentY>? value, Disambigator<System.Windows.Media.TileBrush, TChild>? doNotUse = null) where TChild: System.Windows.Media.TileBrush{value?.SetValue(target, System.Windows.Media.TileBrush.AlignmentYProperty); return target;}

//PlayerProperty
public static System.Windows.Media.VideoDrawing WithPlayer(this System.Windows.Media.VideoDrawing target, ValueProxy<System.Windows.Media.MediaPlayer>? value) {value?.SetValue(target, System.Windows.Media.VideoDrawing.PlayerProperty); return target;}

//GlyphRunProperty
public static System.Windows.Media.GlyphRunDrawing WithGlyphRun(this System.Windows.Media.GlyphRunDrawing target, ValueProxy<System.Windows.Media.GlyphRun>? value) {value?.SetValue(target, System.Windows.Media.GlyphRunDrawing.GlyphRunProperty); return target;}

//ForegroundBrushProperty
public static System.Windows.Media.GlyphRunDrawing WithForegroundBrush(this System.Windows.Media.GlyphRunDrawing target, ValueProxy<System.Windows.Media.Brush>? value) {value?.SetValue(target, System.Windows.Media.GlyphRunDrawing.ForegroundBrushProperty); return target;}

//ColorInterpolationModeProperty
public static TChild WithColorInterpolationMode<TChild>(this TChild target, ValueProxy<System.Windows.Media.ColorInterpolationMode>? value, Disambigator<System.Windows.Media.GradientBrush, TChild>? doNotUse = null) where TChild: System.Windows.Media.GradientBrush{value?.SetValue(target, System.Windows.Media.GradientBrush.ColorInterpolationModeProperty); return target;}

//MappingModeProperty
public static TChild WithMappingMode<TChild>(this TChild target, ValueProxy<System.Windows.Media.BrushMappingMode>? value, Disambigator<System.Windows.Media.GradientBrush, TChild>? doNotUse = null) where TChild: System.Windows.Media.GradientBrush{value?.SetValue(target, System.Windows.Media.GradientBrush.MappingModeProperty); return target;}

//SpreadMethodProperty
public static TChild WithSpreadMethod<TChild>(this TChild target, ValueProxy<System.Windows.Media.GradientSpreadMethod>? value, Disambigator<System.Windows.Media.GradientBrush, TChild>? doNotUse = null) where TChild: System.Windows.Media.GradientBrush{value?.SetValue(target, System.Windows.Media.GradientBrush.SpreadMethodProperty); return target;}

//GradientStopsProperty
public static TChild WithGradientStops<TChild>(this TChild target, ValueProxy<System.Windows.Media.GradientStopCollection>? value, Disambigator<System.Windows.Media.GradientBrush, TChild>? doNotUse = null) where TChild: System.Windows.Media.GradientBrush{value?.SetValue(target, System.Windows.Media.GradientBrush.GradientStopsProperty); return target;}

//AccelerationRatioProperty
public static TChild WithAccelerationRatio<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Animation.Timeline, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Timeline{value?.SetValue(target, System.Windows.Media.Animation.Timeline.AccelerationRatioProperty); return target;}

//AutoReverseProperty
public static TChild WithAutoReverse<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Media.Animation.Timeline, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Timeline{value?.SetValue(target, System.Windows.Media.Animation.Timeline.AutoReverseProperty); return target;}

//BeginTimeProperty
public static TChild WithBeginTime<TChild>(this TChild target, ValueProxy<System.Nullable<System.TimeSpan>>? value, Disambigator<System.Windows.Media.Animation.Timeline, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Timeline{value?.SetValue(target, System.Windows.Media.Animation.Timeline.BeginTimeProperty); return target;}

//DecelerationRatioProperty
public static TChild WithDecelerationRatio<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Animation.Timeline, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Timeline{value?.SetValue(target, System.Windows.Media.Animation.Timeline.DecelerationRatioProperty); return target;}

//DesiredFrameRateProperty
public static TChild WithTimeline_DesiredFrameRate<TChild>(this TChild target, ValueProxy<System.Nullable<System.Int32>>? value, Disambigator<System.Windows.Media.Animation.Timeline, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Timeline{value?.SetValue(target, System.Windows.Media.Animation.Timeline.DesiredFrameRateProperty); return target;}

//DurationProperty
public static TChild WithDuration<TChild>(this TChild target, ValueProxy<System.Windows.Duration>? value, Disambigator<System.Windows.Media.Animation.Timeline, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Timeline{value?.SetValue(target, System.Windows.Media.Animation.Timeline.DurationProperty); return target;}

//FillBehaviorProperty
public static TChild WithFillBehavior<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.FillBehavior>? value, Disambigator<System.Windows.Media.Animation.Timeline, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Timeline{value?.SetValue(target, System.Windows.Media.Animation.Timeline.FillBehaviorProperty); return target;}

//RepeatBehaviorProperty
public static TChild WithRepeatBehavior<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.RepeatBehavior>? value, Disambigator<System.Windows.Media.Animation.Timeline, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Timeline{value?.SetValue(target, System.Windows.Media.Animation.Timeline.RepeatBehaviorProperty); return target;}

//SpeedRatioProperty
public static TChild WithSpeedRatio<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Animation.Timeline, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.Timeline{value?.SetValue(target, System.Windows.Media.Animation.Timeline.SpeedRatioProperty); return target;}

//GradientOriginProperty
public static System.Windows.Media.RadialGradientBrush WithGradientOrigin(this System.Windows.Media.RadialGradientBrush target, ValueProxy<System.Windows.Point>? value) {value?.SetValue(target, System.Windows.Media.RadialGradientBrush.GradientOriginProperty); return target;}

//AngleProperty
public static System.Windows.Media.RotateTransform WithAngle(this System.Windows.Media.RotateTransform target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.RotateTransform.AngleProperty); return target;}
public static System.Windows.Media.Media3D.AxisAngleRotation3D WithAngle(this System.Windows.Media.Media3D.AxisAngleRotation3D target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.AxisAngleRotation3D.AngleProperty); return target;}

//AngleXProperty
public static System.Windows.Media.SkewTransform WithAngleX(this System.Windows.Media.SkewTransform target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.SkewTransform.AngleXProperty); return target;}

//AngleYProperty
public static System.Windows.Media.SkewTransform WithAngleY(this System.Windows.Media.SkewTransform target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.SkewTransform.AngleYProperty); return target;}

//AxisProperty
public static System.Windows.Media.Media3D.AxisAngleRotation3D WithAxis(this System.Windows.Media.Media3D.AxisAngleRotation3D target, ValueProxy<System.Windows.Media.Media3D.Vector3D>? value) {value?.SetValue(target, System.Windows.Media.Media3D.AxisAngleRotation3D.AxisProperty); return target;}

//AmbientColorProperty
public static System.Windows.Media.Media3D.DiffuseMaterial WithAmbientColor(this System.Windows.Media.Media3D.DiffuseMaterial target, ValueProxy<System.Windows.Media.Color>? value) {value?.SetValue(target, System.Windows.Media.Media3D.DiffuseMaterial.AmbientColorProperty); return target;}

//DirectionProperty
public static System.Windows.Media.Media3D.DirectionalLight WithDirection(this System.Windows.Media.Media3D.DirectionalLight target, ValueProxy<System.Windows.Media.Media3D.Vector3D>? value) {value?.SetValue(target, System.Windows.Media.Media3D.DirectionalLight.DirectionProperty); return target;}
public static System.Windows.Media.Media3D.SpotLight WithDirection(this System.Windows.Media.Media3D.SpotLight target, ValueProxy<System.Windows.Media.Media3D.Vector3D>? value) {value?.SetValue(target, System.Windows.Media.Media3D.SpotLight.DirectionProperty); return target;}
public static System.Windows.Media.Effects.DropShadowBitmapEffect WithDirection(this System.Windows.Media.Effects.DropShadowBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.DropShadowBitmapEffect.DirectionProperty); return target;}
public static System.Windows.Media.Effects.DropShadowEffect WithDirection(this System.Windows.Media.Effects.DropShadowEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.DropShadowEffect.DirectionProperty); return target;}

//BackMaterialProperty
public static System.Windows.Media.Media3D.GeometryModel3D WithBackMaterial(this System.Windows.Media.Media3D.GeometryModel3D target, ValueProxy<System.Windows.Media.Media3D.Material>? value) {value?.SetValue(target, System.Windows.Media.Media3D.GeometryModel3D.BackMaterialProperty); return target;}

//ViewMatrixProperty
public static System.Windows.Media.Media3D.MatrixCamera WithViewMatrix(this System.Windows.Media.Media3D.MatrixCamera target, ValueProxy<System.Windows.Media.Media3D.Matrix3D>? value) {value?.SetValue(target, System.Windows.Media.Media3D.MatrixCamera.ViewMatrixProperty); return target;}

//ProjectionMatrixProperty
public static System.Windows.Media.Media3D.MatrixCamera WithProjectionMatrix(this System.Windows.Media.Media3D.MatrixCamera target, ValueProxy<System.Windows.Media.Media3D.Matrix3D>? value) {value?.SetValue(target, System.Windows.Media.Media3D.MatrixCamera.ProjectionMatrixProperty); return target;}

//ModelProperty
public static System.Windows.Media.Media3D.ModelUIElement3D WithModel(this System.Windows.Media.Media3D.ModelUIElement3D target, ValueProxy<System.Windows.Media.Media3D.Model3D>? value) {value?.SetValue(target, System.Windows.Media.Media3D.ModelUIElement3D.ModelProperty); return target;}

//NearPlaneDistanceProperty
public static TChild WithNearPlaneDistance<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Media3D.ProjectionCamera, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.ProjectionCamera{value?.SetValue(target, System.Windows.Media.Media3D.ProjectionCamera.NearPlaneDistanceProperty); return target;}

//FarPlaneDistanceProperty
public static TChild WithFarPlaneDistance<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Media3D.ProjectionCamera, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.ProjectionCamera{value?.SetValue(target, System.Windows.Media.Media3D.ProjectionCamera.FarPlaneDistanceProperty); return target;}

//PositionProperty
public static TChild WithPosition<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Point3D>? value, Disambigator<System.Windows.Media.Media3D.ProjectionCamera, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.ProjectionCamera{value?.SetValue(target, System.Windows.Media.Media3D.ProjectionCamera.PositionProperty); return target;}
public static TChild WithPosition<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Point3D>? value, Disambigator<System.Windows.Media.Media3D.PointLightBase, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.PointLightBase{value?.SetValue(target, System.Windows.Media.Media3D.PointLightBase.PositionProperty); return target;}

//LookDirectionProperty
public static TChild WithLookDirection<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Vector3D>? value, Disambigator<System.Windows.Media.Media3D.ProjectionCamera, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.ProjectionCamera{value?.SetValue(target, System.Windows.Media.Media3D.ProjectionCamera.LookDirectionProperty); return target;}

//UpDirectionProperty
public static TChild WithUpDirection<TChild>(this TChild target, ValueProxy<System.Windows.Media.Media3D.Vector3D>? value, Disambigator<System.Windows.Media.Media3D.ProjectionCamera, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.ProjectionCamera{value?.SetValue(target, System.Windows.Media.Media3D.ProjectionCamera.UpDirectionProperty); return target;}

//FieldOfViewProperty
public static System.Windows.Media.Media3D.PerspectiveCamera WithFieldOfView(this System.Windows.Media.Media3D.PerspectiveCamera target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.PerspectiveCamera.FieldOfViewProperty); return target;}

//RangeProperty
public static TChild WithRange<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Media3D.PointLightBase, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.PointLightBase{value?.SetValue(target, System.Windows.Media.Media3D.PointLightBase.RangeProperty); return target;}

//ConstantAttenuationProperty
public static TChild WithConstantAttenuation<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Media3D.PointLightBase, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.PointLightBase{value?.SetValue(target, System.Windows.Media.Media3D.PointLightBase.ConstantAttenuationProperty); return target;}

//LinearAttenuationProperty
public static TChild WithLinearAttenuation<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Media3D.PointLightBase, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.PointLightBase{value?.SetValue(target, System.Windows.Media.Media3D.PointLightBase.LinearAttenuationProperty); return target;}

//QuadraticAttenuationProperty
public static TChild WithQuadraticAttenuation<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Media3D.PointLightBase, TChild>? doNotUse = null) where TChild: System.Windows.Media.Media3D.PointLightBase{value?.SetValue(target, System.Windows.Media.Media3D.PointLightBase.QuadraticAttenuationProperty); return target;}

//CenterZProperty
public static System.Windows.Media.Media3D.RotateTransform3D WithCenterZ(this System.Windows.Media.Media3D.RotateTransform3D target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.RotateTransform3D.CenterZProperty); return target;}
public static System.Windows.Media.Media3D.ScaleTransform3D WithCenterZ(this System.Windows.Media.Media3D.ScaleTransform3D target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.ScaleTransform3D.CenterZProperty); return target;}

//RotationProperty
public static System.Windows.Media.Media3D.RotateTransform3D WithRotation(this System.Windows.Media.Media3D.RotateTransform3D target, ValueProxy<System.Windows.Media.Media3D.Rotation3D>? value) {value?.SetValue(target, System.Windows.Media.Media3D.RotateTransform3D.RotationProperty); return target;}
public static System.Windows.Media.Imaging.BitmapImage WithRotation(this System.Windows.Media.Imaging.BitmapImage target, ValueProxy<System.Windows.Media.Imaging.Rotation>? value) {value?.SetValue(target, System.Windows.Media.Imaging.BitmapImage.RotationProperty); return target;}

//ScaleZProperty
public static System.Windows.Media.Media3D.ScaleTransform3D WithScaleZ(this System.Windows.Media.Media3D.ScaleTransform3D target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.ScaleTransform3D.ScaleZProperty); return target;}

//SpecularPowerProperty
public static System.Windows.Media.Media3D.SpecularMaterial WithSpecularPower(this System.Windows.Media.Media3D.SpecularMaterial target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.SpecularMaterial.SpecularPowerProperty); return target;}

//OuterConeAngleProperty
public static System.Windows.Media.Media3D.SpotLight WithOuterConeAngle(this System.Windows.Media.Media3D.SpotLight target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.SpotLight.OuterConeAngleProperty); return target;}

//InnerConeAngleProperty
public static System.Windows.Media.Media3D.SpotLight WithInnerConeAngle(this System.Windows.Media.Media3D.SpotLight target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.SpotLight.InnerConeAngleProperty); return target;}

//OffsetXProperty
public static System.Windows.Media.Media3D.TranslateTransform3D WithOffsetX(this System.Windows.Media.Media3D.TranslateTransform3D target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.TranslateTransform3D.OffsetXProperty); return target;}

//OffsetYProperty
public static System.Windows.Media.Media3D.TranslateTransform3D WithOffsetY(this System.Windows.Media.Media3D.TranslateTransform3D target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.TranslateTransform3D.OffsetYProperty); return target;}

//OffsetZProperty
public static System.Windows.Media.Media3D.TranslateTransform3D WithOffsetZ(this System.Windows.Media.Media3D.TranslateTransform3D target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Media3D.TranslateTransform3D.OffsetZProperty); return target;}

//AmplitudeProperty
public static TChild WithAmplitude<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Animation.BackEase, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.BackEase{value?.SetValue(target, System.Windows.Media.Animation.BackEase.AmplitudeProperty); return target;}

//EasingModeProperty
public static TChild WithEasingMode<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.EasingMode>? value, Disambigator<System.Windows.Media.Animation.EasingFunctionBase, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingFunctionBase{value?.SetValue(target, System.Windows.Media.Animation.EasingFunctionBase.EasingModeProperty); return target;}

//BouncesProperty
public static TChild WithBounces<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Media.Animation.BounceEase, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.BounceEase{value?.SetValue(target, System.Windows.Media.Animation.BounceEase.BouncesProperty); return target;}

//BouncinessProperty
public static TChild WithBounciness<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Animation.BounceEase, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.BounceEase{value?.SetValue(target, System.Windows.Media.Animation.BounceEase.BouncinessProperty); return target;}

//SlipBehaviorProperty
public static TChild WithSlipBehavior<TChild>(this TChild target, ValueProxy<System.Windows.Media.Animation.SlipBehavior>? value, Disambigator<System.Windows.Media.Animation.ParallelTimeline, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ParallelTimeline{value?.SetValue(target, System.Windows.Media.Animation.ParallelTimeline.SlipBehaviorProperty); return target;}

//PathGeometryProperty
public static TChild WithPathGeometry<TChild>(this TChild target, ValueProxy<System.Windows.Media.PathGeometry>? value, Disambigator<System.Windows.Media.Animation.DoubleAnimationUsingPath, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.DoubleAnimationUsingPath{value?.SetValue(target, System.Windows.Media.Animation.DoubleAnimationUsingPath.PathGeometryProperty); return target;}
public static TChild WithPathGeometry<TChild>(this TChild target, ValueProxy<System.Windows.Media.PathGeometry>? value, Disambigator<System.Windows.Media.Animation.MatrixAnimationUsingPath, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.MatrixAnimationUsingPath{value?.SetValue(target, System.Windows.Media.Animation.MatrixAnimationUsingPath.PathGeometryProperty); return target;}
public static TChild WithPathGeometry<TChild>(this TChild target, ValueProxy<System.Windows.Media.PathGeometry>? value, Disambigator<System.Windows.Media.Animation.PointAnimationUsingPath, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.PointAnimationUsingPath{value?.SetValue(target, System.Windows.Media.Animation.PointAnimationUsingPath.PathGeometryProperty); return target;}

//UseShortestPathProperty
public static TChild WithUseShortestPath<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Media.Animation.EasingQuaternionKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.EasingQuaternionKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.EasingQuaternionKeyFrame.UseShortestPathProperty); return target;}
public static TChild WithUseShortestPath<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Media.Animation.LinearQuaternionKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.LinearQuaternionKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.LinearQuaternionKeyFrame.UseShortestPathProperty); return target;}
public static TChild WithUseShortestPath<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Media.Animation.QuaternionAnimation, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.QuaternionAnimation{value?.SetValue(target, System.Windows.Media.Animation.QuaternionAnimation.UseShortestPathProperty); return target;}
public static TChild WithUseShortestPath<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Media.Animation.SplineQuaternionKeyFrame, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.SplineQuaternionKeyFrame{value?.SetValue(target, System.Windows.Media.Animation.SplineQuaternionKeyFrame.UseShortestPathProperty); return target;}

//OscillationsProperty
public static TChild WithOscillations<TChild>(this TChild target, ValueProxy<System.Int32>? value, Disambigator<System.Windows.Media.Animation.ElasticEase, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ElasticEase{value?.SetValue(target, System.Windows.Media.Animation.ElasticEase.OscillationsProperty); return target;}

//SpringinessProperty
public static TChild WithSpringiness<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Animation.ElasticEase, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ElasticEase{value?.SetValue(target, System.Windows.Media.Animation.ElasticEase.SpringinessProperty); return target;}

//ExponentProperty
public static TChild WithExponent<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Animation.ExponentialEase, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.ExponentialEase{value?.SetValue(target, System.Windows.Media.Animation.ExponentialEase.ExponentProperty); return target;}

//DoesRotateWithTangentProperty
public static TChild WithDoesRotateWithTangent<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Media.Animation.MatrixAnimationUsingPath, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.MatrixAnimationUsingPath{value?.SetValue(target, System.Windows.Media.Animation.MatrixAnimationUsingPath.DoesRotateWithTangentProperty); return target;}

//IsAngleCumulativeProperty
public static TChild WithIsAngleCumulative<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Media.Animation.MatrixAnimationUsingPath, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.MatrixAnimationUsingPath{value?.SetValue(target, System.Windows.Media.Animation.MatrixAnimationUsingPath.IsAngleCumulativeProperty); return target;}

//IsOffsetCumulativeProperty
public static TChild WithIsOffsetCumulative<TChild>(this TChild target, ValueProxy<System.Boolean>? value, Disambigator<System.Windows.Media.Animation.MatrixAnimationUsingPath, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.MatrixAnimationUsingPath{value?.SetValue(target, System.Windows.Media.Animation.MatrixAnimationUsingPath.IsOffsetCumulativeProperty); return target;}

//PowerProperty
public static TChild WithPower<TChild>(this TChild target, ValueProxy<System.Double>? value, Disambigator<System.Windows.Media.Animation.PowerEase, TChild>? doNotUse = null) where TChild: System.Windows.Media.Animation.PowerEase{value?.SetValue(target, System.Windows.Media.Animation.PowerEase.PowerProperty); return target;}

//UriCachePolicyProperty
public static System.Windows.Media.Imaging.BitmapImage WithUriCachePolicy(this System.Windows.Media.Imaging.BitmapImage target, ValueProxy<System.Net.Cache.RequestCachePolicy>? value) {value?.SetValue(target, System.Windows.Media.Imaging.BitmapImage.UriCachePolicyProperty); return target;}

//UriSourceProperty
public static System.Windows.Media.Imaging.BitmapImage WithUriSource(this System.Windows.Media.Imaging.BitmapImage target, ValueProxy<System.Uri>? value) {value?.SetValue(target, System.Windows.Media.Imaging.BitmapImage.UriSourceProperty); return target;}
public static System.Windows.Media.Effects.PixelShader WithUriSource(this System.Windows.Media.Effects.PixelShader target, ValueProxy<System.Uri>? value) {value?.SetValue(target, System.Windows.Media.Effects.PixelShader.UriSourceProperty); return target;}

//StreamSourceProperty
public static System.Windows.Media.Imaging.BitmapImage WithStreamSource(this System.Windows.Media.Imaging.BitmapImage target, ValueProxy<System.IO.Stream>? value) {value?.SetValue(target, System.Windows.Media.Imaging.BitmapImage.StreamSourceProperty); return target;}

//DecodePixelWidthProperty
public static System.Windows.Media.Imaging.BitmapImage WithDecodePixelWidth(this System.Windows.Media.Imaging.BitmapImage target, ValueProxy<System.Int32>? value) {value?.SetValue(target, System.Windows.Media.Imaging.BitmapImage.DecodePixelWidthProperty); return target;}

//DecodePixelHeightProperty
public static System.Windows.Media.Imaging.BitmapImage WithDecodePixelHeight(this System.Windows.Media.Imaging.BitmapImage target, ValueProxy<System.Int32>? value) {value?.SetValue(target, System.Windows.Media.Imaging.BitmapImage.DecodePixelHeightProperty); return target;}

//SourceRectProperty
public static System.Windows.Media.Imaging.BitmapImage WithSourceRect(this System.Windows.Media.Imaging.BitmapImage target, ValueProxy<System.Windows.Int32Rect>? value) {value?.SetValue(target, System.Windows.Media.Imaging.BitmapImage.SourceRectProperty); return target;}
public static System.Windows.Media.Imaging.CroppedBitmap WithSourceRect(this System.Windows.Media.Imaging.CroppedBitmap target, ValueProxy<System.Windows.Int32Rect>? value) {value?.SetValue(target, System.Windows.Media.Imaging.CroppedBitmap.SourceRectProperty); return target;}

//CreateOptionsProperty
public static System.Windows.Media.Imaging.BitmapImage WithCreateOptions(this System.Windows.Media.Imaging.BitmapImage target, ValueProxy<System.Windows.Media.Imaging.BitmapCreateOptions>? value) {value?.SetValue(target, System.Windows.Media.Imaging.BitmapImage.CreateOptionsProperty); return target;}

//CacheOptionProperty
public static System.Windows.Media.Imaging.BitmapImage WithCacheOption(this System.Windows.Media.Imaging.BitmapImage target, ValueProxy<System.Windows.Media.Imaging.BitmapCacheOption>? value) {value?.SetValue(target, System.Windows.Media.Imaging.BitmapImage.CacheOptionProperty); return target;}

//SourceColorContextProperty
public static System.Windows.Media.Imaging.ColorConvertedBitmap WithSourceColorContext(this System.Windows.Media.Imaging.ColorConvertedBitmap target, ValueProxy<System.Windows.Media.ColorContext>? value) {value?.SetValue(target, System.Windows.Media.Imaging.ColorConvertedBitmap.SourceColorContextProperty); return target;}

//DestinationColorContextProperty
public static System.Windows.Media.Imaging.ColorConvertedBitmap WithDestinationColorContext(this System.Windows.Media.Imaging.ColorConvertedBitmap target, ValueProxy<System.Windows.Media.ColorContext>? value) {value?.SetValue(target, System.Windows.Media.Imaging.ColorConvertedBitmap.DestinationColorContextProperty); return target;}

//DestinationFormatProperty
public static System.Windows.Media.Imaging.ColorConvertedBitmap WithDestinationFormat(this System.Windows.Media.Imaging.ColorConvertedBitmap target, ValueProxy<System.Windows.Media.PixelFormat>? value) {value?.SetValue(target, System.Windows.Media.Imaging.ColorConvertedBitmap.DestinationFormatProperty); return target;}
public static System.Windows.Media.Imaging.FormatConvertedBitmap WithDestinationFormat(this System.Windows.Media.Imaging.FormatConvertedBitmap target, ValueProxy<System.Windows.Media.PixelFormat>? value) {value?.SetValue(target, System.Windows.Media.Imaging.FormatConvertedBitmap.DestinationFormatProperty); return target;}

//DestinationPaletteProperty
public static System.Windows.Media.Imaging.FormatConvertedBitmap WithDestinationPalette(this System.Windows.Media.Imaging.FormatConvertedBitmap target, ValueProxy<System.Windows.Media.Imaging.BitmapPalette>? value) {value?.SetValue(target, System.Windows.Media.Imaging.FormatConvertedBitmap.DestinationPaletteProperty); return target;}

//AlphaThresholdProperty
public static System.Windows.Media.Imaging.FormatConvertedBitmap WithAlphaThreshold(this System.Windows.Media.Imaging.FormatConvertedBitmap target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Imaging.FormatConvertedBitmap.AlphaThresholdProperty); return target;}

//BevelWidthProperty
public static System.Windows.Media.Effects.BevelBitmapEffect WithBevelWidth(this System.Windows.Media.Effects.BevelBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.BevelBitmapEffect.BevelWidthProperty); return target;}

//ReliefProperty
public static System.Windows.Media.Effects.BevelBitmapEffect WithRelief(this System.Windows.Media.Effects.BevelBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.BevelBitmapEffect.ReliefProperty); return target;}
public static System.Windows.Media.Effects.EmbossBitmapEffect WithRelief(this System.Windows.Media.Effects.EmbossBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.EmbossBitmapEffect.ReliefProperty); return target;}

//LightAngleProperty
public static System.Windows.Media.Effects.BevelBitmapEffect WithLightAngle(this System.Windows.Media.Effects.BevelBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.BevelBitmapEffect.LightAngleProperty); return target;}
public static System.Windows.Media.Effects.EmbossBitmapEffect WithLightAngle(this System.Windows.Media.Effects.EmbossBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.EmbossBitmapEffect.LightAngleProperty); return target;}

//SmoothnessProperty
public static System.Windows.Media.Effects.BevelBitmapEffect WithSmoothness(this System.Windows.Media.Effects.BevelBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.BevelBitmapEffect.SmoothnessProperty); return target;}

//EdgeProfileProperty
public static System.Windows.Media.Effects.BevelBitmapEffect WithEdgeProfile(this System.Windows.Media.Effects.BevelBitmapEffect target, ValueProxy<System.Windows.Media.Effects.EdgeProfile>? value) {value?.SetValue(target, System.Windows.Media.Effects.BevelBitmapEffect.EdgeProfileProperty); return target;}

//RadiusProperty
public static System.Windows.Media.Effects.BlurBitmapEffect WithRadius(this System.Windows.Media.Effects.BlurBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.BlurBitmapEffect.RadiusProperty); return target;}
public static System.Windows.Media.Effects.BlurEffect WithRadius(this System.Windows.Media.Effects.BlurEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.BlurEffect.RadiusProperty); return target;}

//KernelTypeProperty
public static System.Windows.Media.Effects.BlurBitmapEffect WithKernelType(this System.Windows.Media.Effects.BlurBitmapEffect target, ValueProxy<System.Windows.Media.Effects.KernelType>? value) {value?.SetValue(target, System.Windows.Media.Effects.BlurBitmapEffect.KernelTypeProperty); return target;}
public static System.Windows.Media.Effects.BlurEffect WithKernelType(this System.Windows.Media.Effects.BlurEffect target, ValueProxy<System.Windows.Media.Effects.KernelType>? value) {value?.SetValue(target, System.Windows.Media.Effects.BlurEffect.KernelTypeProperty); return target;}

//RenderingBiasProperty
public static System.Windows.Media.Effects.BlurEffect WithRenderingBias(this System.Windows.Media.Effects.BlurEffect target, ValueProxy<System.Windows.Media.Effects.RenderingBias>? value) {value?.SetValue(target, System.Windows.Media.Effects.BlurEffect.RenderingBiasProperty); return target;}
public static System.Windows.Media.Effects.DropShadowEffect WithRenderingBias(this System.Windows.Media.Effects.DropShadowEffect target, ValueProxy<System.Windows.Media.Effects.RenderingBias>? value) {value?.SetValue(target, System.Windows.Media.Effects.DropShadowEffect.RenderingBiasProperty); return target;}

//ShadowDepthProperty
public static System.Windows.Media.Effects.DropShadowBitmapEffect WithShadowDepth(this System.Windows.Media.Effects.DropShadowBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.DropShadowBitmapEffect.ShadowDepthProperty); return target;}
public static System.Windows.Media.Effects.DropShadowEffect WithShadowDepth(this System.Windows.Media.Effects.DropShadowEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.DropShadowEffect.ShadowDepthProperty); return target;}

//NoiseProperty
public static System.Windows.Media.Effects.DropShadowBitmapEffect WithNoise(this System.Windows.Media.Effects.DropShadowBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.DropShadowBitmapEffect.NoiseProperty); return target;}
public static System.Windows.Media.Effects.OuterGlowBitmapEffect WithNoise(this System.Windows.Media.Effects.OuterGlowBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.OuterGlowBitmapEffect.NoiseProperty); return target;}

//SoftnessProperty
public static System.Windows.Media.Effects.DropShadowBitmapEffect WithSoftness(this System.Windows.Media.Effects.DropShadowBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.DropShadowBitmapEffect.SoftnessProperty); return target;}

//BlurRadiusProperty
public static System.Windows.Media.Effects.DropShadowEffect WithBlurRadius(this System.Windows.Media.Effects.DropShadowEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.DropShadowEffect.BlurRadiusProperty); return target;}

//GlowColorProperty
public static System.Windows.Media.Effects.OuterGlowBitmapEffect WithGlowColor(this System.Windows.Media.Effects.OuterGlowBitmapEffect target, ValueProxy<System.Windows.Media.Color>? value) {value?.SetValue(target, System.Windows.Media.Effects.OuterGlowBitmapEffect.GlowColorProperty); return target;}

//GlowSizeProperty
public static System.Windows.Media.Effects.OuterGlowBitmapEffect WithGlowSize(this System.Windows.Media.Effects.OuterGlowBitmapEffect target, ValueProxy<System.Double>? value) {value?.SetValue(target, System.Windows.Media.Effects.OuterGlowBitmapEffect.GlowSizeProperty); return target;}

//ShaderRenderModeProperty
public static System.Windows.Media.Effects.PixelShader WithShaderRenderMode(this System.Windows.Media.Effects.PixelShader target, ValueProxy<System.Windows.Media.Effects.ShaderRenderMode>? value) {value?.SetValue(target, System.Windows.Media.Effects.PixelShader.ShaderRenderModeProperty); return target;}
}
}
