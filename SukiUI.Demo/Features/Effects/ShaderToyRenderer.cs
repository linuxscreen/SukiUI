using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Rendering.Composition;

namespace SukiUI.Demo.Features.Effects
{
    public class ShaderToyRenderer : Control
    {
        private CompositionCustomVisual? _customVisual;

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            Update();
        }
        
        private void Update()
        {
            if (_customVisual == null) return;
            _customVisual.Size = new Vector(Bounds.Width, Bounds.Height);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);
            if(change.Property == BoundsProperty)
                Update();
        }
    }
}