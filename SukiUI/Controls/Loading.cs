using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Rendering.Composition;
using SukiUI.Extensions;

namespace SukiUI.Controls
{
    public class Loading : Control
    {
        public static readonly StyledProperty<LoadingStyle> LoadingStyleProperty =
            AvaloniaProperty.Register<Loading, LoadingStyle>(nameof(LoadingStyle), defaultValue: LoadingStyle.Simple);

        public LoadingStyle LoadingStyle
        {
            get => GetValue(LoadingStyleProperty);
            set => SetValue(LoadingStyleProperty, value);
        }

        public static readonly StyledProperty<IBrush?> ForegroundProperty =
            AvaloniaProperty.Register<Loading, IBrush?>(nameof(Foreground));

        public IBrush? Foreground
        {
            get => GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }
        
        
        private CompositionCustomVisual? _customVisual;
        
        public Loading()
        {
            Width = 50;
            Height = 50;
        }

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

        private readonly float[] _color = new float[3];

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);
            if (change.Property == BoundsProperty)
                Update();
            else if (change.Property == ForegroundProperty && Foreground is ImmutableSolidColorBrush brush)
            {
                brush.Color.ToFloatArrayNonAlloc(_color);
                _customVisual?.SendHandlerMessage(_color);
            }
        }
    }

    public enum LoadingStyle
    {
        Simple, 
        Glow,
        Pellets
    }
}