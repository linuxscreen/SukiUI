using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Styling;
using System;

namespace SukiUI.Controls.GlassMorphism;

public class BlurBackground : Control
{
    
    public override void BeginInit()
    {
        base.BeginInit();

        darkmode = Application.Current.ActualThemeVariant == ThemeVariant.Dark;
    }

    private bool darkmode = false;
    
    
    private static string clampLumaSkSL = @"
uniform shader src;
uniform float maxLuma;
uniform float minLuma;

half4 main(float2 coord) {
    half4 c = src.eval(coord);
    float lum = 0.2126 * c.r + 0.7152 * c.g + 0.0722 * c.b;
    float scale = 1.0;
    if (lum > maxLuma) {
        scale = maxLuma / lum;
    } else if (lum < minLuma && lum > 0.0) {
        scale = minLuma / lum;
    }
    
    if (lum == 0.0) scale = 1.0;
    c.rgb *= scale;
    return c;
}
";

    private class BlurBehindRenderOperation : ICustomDrawOperation
    {
  
        private readonly Rect _bounds;
        
        public BlurBehindRenderOperation()
        {
            
        }

        public void Dispose()
        {
        }

        public bool HitTest(Point p) => _bounds.Contains(p);

        private bool IsDarkTheme;
        
       public void Render(ImmediateDrawingContext context)
        {
            var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
            using var lease = leaseFeature.Lease();
            var canvas = lease.SkCanvas;

            if (!canvas.TotalMatrix.TryInvert(out var currentInvertedTransform))
                return;
            
            var sigma = IsDarkTheme ? ( _bounds.Width + _bounds.Height)/42 : 50;
            
            if(sigma <20)
                sigma = 20;
   
        }
       
        public Rect Bounds => _bounds.Inflate(4);

        public bool Equals(ICustomDrawOperation? other)
        {
            return other is BlurBehindRenderOperation op && op._bounds == _bounds ;
        }
    }

    public override void Render(DrawingContext context)
    {
       
       
    }
}
