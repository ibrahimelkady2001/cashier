using System;
using System.Diagnostics;
using System.IO;
using System.Drawing.Printing;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.SkiaSharp;

// Required NuGet Packages:
// Install-Package SkiaSharp
// Install-Package ZXing.Net
// Install-Package System.Drawing.Common (for PrintDocument only)

/// <summary>
/// Cross-platform barcode printer using SkiaSharp instead of System.Drawing
/// Provides better quality and cross-platform compatibility
/// </summary>
public class BarcodePrinter3
{
    private const int HIGH_DPI = 600;
    private const int PAPER_WIDTH_HUNDREDTHS = 591;  // 150mm
    private const int PAPER_HEIGHT_HUNDREDTHS = 386; // 98mm

    private SKBitmap generatedBarcode = null;
    private PrintDocument printDoc;
    private BarcodeConfig config;

    public class BarcodeConfig
    {
        public string BarcodeText { get; set; }
        public string Label { get; set; } = "";
        public string PrinterName { get; set; } = "Xprinter XP-365B";
        public BarcodeSize Size { get; set; } = BarcodeSize.Full;
        public int BarcodeWidth { get; set; } = 600;
        public int BarcodeHeight { get; set; } = 180;
        public bool ShowLabel { get; set; } = true;
        public float FontSize { get; set; } = 24f;
        public int Margin { get; set; } = 10;
        public SKColor BackgroundColor { get; set; } = SKColors.White;
        public SKColor ForegroundColor { get; set; } = SKColors.Black;
    }

    public enum BarcodeSize
    {
        Full,
        Half,
        Quarter
    }

    public BarcodePrinter3()
    {
        printDoc = new PrintDocument();
    }

    /// <summary>
    /// Main printing method with high-quality SkiaSharp rendering
    /// </summary>
    public void PrintBarcode(BarcodeConfig configuration)
    {
        try
        {
            config = configuration;

            // Generate barcode with SkiaSharp
            generatedBarcode = GenerateBarcodeWithSkia(config);

            // Configure and print
            ConfigurePrintDocument(config);
            printDoc.Print();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Print error: {ex.Message}");
            throw;
        }
        finally
        {
            generatedBarcode?.Dispose();
        }
    }

    /// <summary>
    /// Generates high-quality barcode using ZXing and SkiaSharp
    /// </summary>
    private SKBitmap GenerateBarcodeWithSkia(BarcodeConfig config)
    {
        // Determine barcode format
        BarcodeFormat format = config.BarcodeText.Length == 13
            ? BarcodeFormat.EAN_13
            : BarcodeFormat.CODE_128;

        // Configure ZXing barcode writer
        var writer = new BarcodeWriter
        {
            Format = format,
            Options = new EncodingOptions
            {
                Width = config.BarcodeWidth,
                Height = config.BarcodeHeight,
                Margin = 0,
                PureBarcode = false // Show text below barcode
            }
        };

        // Generate barcode bitmap using ZXing.SkiaSharp
        SKBitmap barcodeBitmap = writer.Write(config.BarcodeText);

        // Calculate final dimensions
        int finalWidth = barcodeBitmap.Width + (config.Margin * 2);
        int labelHeight = config.ShowLabel ? 60 : 0;
        int finalHeight = barcodeBitmap.Height + labelHeight + (config.Margin * 2);

        // Create final image with SkiaSharp
        SKBitmap finalBitmap = new SKBitmap(finalWidth, finalHeight);

        using (SKCanvas canvas = new SKCanvas(finalBitmap))
        {
            // Clear background
            canvas.Clear(config.BackgroundColor);

            // Enable high-quality rendering
            canvas.Save();

            // Draw barcode
            int barcodeX = config.Margin;
            int barcodeY = config.Margin;

            using (SKPaint paint = new SKPaint())
            {
                paint.FilterQuality = SKFilterQuality.None; // No filtering for sharp barcode
                paint.IsAntialias = false; // No anti-aliasing for crisp lines

                canvas.DrawBitmap(barcodeBitmap,
                    new SKRect(barcodeX, barcodeY,
                              barcodeX + barcodeBitmap.Width,
                              barcodeY + barcodeBitmap.Height),
                    paint);
            }

            // Draw label if needed
            if (config.ShowLabel && !string.IsNullOrEmpty(config.Label))
            {
                DrawLabelWithSkia(canvas, config.Label,
                    finalWidth,
                    barcodeBitmap.Height + config.Margin,
                    finalWidth,
                    labelHeight,
                    config.FontSize,
                    config.ForegroundColor);
            }

            canvas.Restore();
        }

        barcodeBitmap.Dispose();
        return finalBitmap;
    }

    /// <summary>
    /// Draws text label using SkiaSharp with RTL support
    /// </summary>
    private void DrawLabelWithSkia(SKCanvas canvas, string text,
        float x, float y, float width, float height,
        float fontSize, SKColor color)
    {
        using (SKPaint textPaint = new SKPaint())
        {
            textPaint.Color = color;
            textPaint.IsAntialias = false; // Sharp text for thermal printing
            textPaint.TextSize = fontSize;
            textPaint.Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold);
            textPaint.TextAlign = SKTextAlign.Center;

            // Measure text
            SKRect textBounds = new SKRect();
            textPaint.MeasureText(text, ref textBounds);

            // Calculate center position
            float textX = x + (width / 2);
            float textY = y + (height / 2) + (textBounds.Height / 2);

            // Draw text
            canvas.DrawText(text, textX, textY, textPaint);
        }
    }

    /// <summary>
    /// Configure print document
    /// </summary>
    private void ConfigurePrintDocument(BarcodeConfig config)
    {
        printDoc.DocumentName = $"Barcode_{config.BarcodeText}";
        printDoc.OriginAtMargins = true;

        printDoc.DefaultPageSettings.PaperSize = new PaperSize(
            "Barcode Label",
            PAPER_WIDTH_HUNDREDTHS,
            PAPER_HEIGHT_HUNDREDTHS
        );

        printDoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
        printDoc.PrinterSettings.PrinterName = config.PrinterName;

        // Get highest resolution
        PrinterResolution highestRes = GetHighestPrinterResolution(printDoc.PrinterSettings);
        printDoc.DefaultPageSettings.PrinterResolution = highestRes;

        printDoc.PrintPage += OnPrintPage;
    }

    /// <summary>
    /// Get highest available printer resolution
    /// </summary>
    private PrinterResolution GetHighestPrinterResolution(PrinterSettings settings)
    {
        PrinterResolution highest = settings.DefaultPageSettings.PrinterResolution;
        int maxDpi = 0;

        foreach (PrinterResolution res in settings.PrinterResolutions)
        {
            if (res.Kind == PrinterResolutionKind.Custom)
            {
                int dpi = Math.Max(res.X, res.Y);
                if (dpi > maxDpi)
                {
                    maxDpi = dpi;
                    highest = res;
                }
            }
        }

        return highest;
    }

    /// <summary>
    /// Print page event handler
    /// </summary>
    private void OnPrintPage(object sender, PrintPageEventArgs e)
    {
        if (generatedBarcode == null)
            return;

        // Convert SKBitmap to System.Drawing.Bitmap for printing
        using (var image = SKImageToBitmap(generatedBarcode))
        {
            // Set high quality graphics
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            Rectangle printArea = printDoc.OriginAtMargins ? e.MarginBounds : e.PageBounds;

            switch (config.Size)
            {
                case BarcodeSize.Full:
                    DrawImageScaled(e.Graphics, image, printArea);
                    break;

                case BarcodeSize.Half:
                    int halfHeight = printArea.Height / 2;
                    DrawImageScaled(e.Graphics, image,
                        new Rectangle(printArea.X, printArea.Y, printArea.Width, halfHeight));
                    DrawImageScaled(e.Graphics, image,
                        new Rectangle(printArea.X, printArea.Y + halfHeight, printArea.Width, halfHeight));
                    break;

                case BarcodeSize.Quarter:
                    int qW = printArea.Width / 2;
                    int qH = printArea.Height / 2;
                    DrawImageScaled(e.Graphics, image, new Rectangle(printArea.X, printArea.Y, qW, qH));
                    DrawImageScaled(e.Graphics, image, new Rectangle(printArea.X + qW, printArea.Y, qW, qH));
                    DrawImageScaled(e.Graphics, image, new Rectangle(printArea.X, printArea.Y + qH, qW, qH));
                    DrawImageScaled(e.Graphics, image, new Rectangle(printArea.X + qW, printArea.Y + qH, qW, qH));
                    break;
            }
        }

        e.HasMorePages = false;
    }

    /// <summary>
    /// Draw image scaled to fit area while maintaining aspect ratio
    /// </summary>
    private void DrawImageScaled(System.Drawing.Graphics g, System.Drawing.Image img, Rectangle area)
    {
        float scaleX = (float)area.Width / img.Width;
        float scaleY = (float)area.Height / img.Height;
        float scale = Math.Min(scaleX, scaleY) * 0.95f;

        int scaledWidth = (int)(img.Width * scale);
        int scaledHeight = (int)(img.Height * scale);

        int x = area.X + (area.Width - scaledWidth) / 2;
        int y = area.Y + (area.Height - scaledHeight) / 2;

        g.DrawImage(img, x, y, scaledWidth, scaledHeight);
    }

    /// <summary>
    /// Convert SKBitmap to System.Drawing.Bitmap
    /// </summary>
    private System.Drawing.Bitmap SKImageToBitmap(SKBitmap skBitmap)
    {
        using (var image = SKImage.FromBitmap(skBitmap))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
        using (var stream = new MemoryStream(data.ToArray()))
        {
            return new System.Drawing.Bitmap(stream);
        }
    }

    /// <summary>
    /// Save barcode to file (useful for debugging)
    /// </summary>
    public void SaveBarcodeToFile(string filePath)
    {
        if (generatedBarcode == null)
            return;

        using (var image = SKImage.FromBitmap(generatedBarcode))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
        using (var stream = File.OpenWrite(filePath))
        {
            data.SaveTo(stream);
        }
    }

    /// <summary>
    /// Cleanup
    /// </summary>
    public void Dispose()
    {
        generatedBarcode?.Dispose();
        printDoc?.Dispose();
    }

    /// <summary>
    /// Quick print helper
    /// </summary>
    public static void QuickPrint(string barcodeText, string label = "",
                                  string printerName = "Xprinter XP-365B",
                                  BarcodeSize size = BarcodeSize.Full)
    {
        using (var printer = new BarcodePrinter3())
        {
            var config = new BarcodeConfig
            {
                BarcodeText = barcodeText,
                Label = label,
                PrinterName = printerName,
                Size = size,
                BarcodeWidth = 600,
                BarcodeHeight = 180,
                ShowLabel = !string.IsNullOrEmpty(label),
                FontSize = 24f,
                Margin = 10
            };

            printer.PrintBarcode(config);
        }
    }
}

// Usage Examples:
//
// Example 1: Simple print
// BarcodePrinter3.QuickPrint("1234567890128", "منتج 1");
//
// Example 2: With custom configuration
// var printer = new BarcodePrinter3();
// var config = new BarcodePrinter3.BarcodeConfig
// {
//     BarcodeText = "1234567890128",
//     Label = "منتج 1",
//     PrinterName = "Xprinter XP-365B",
//     Size = BarcodePrinter3.BarcodeSize.Full,
//     BarcodeWidth = 700,
//     BarcodeHeight = 200,
//     ShowLabel = true,
//     FontSize = 28f,
//     Margin = 15
// };
// printer.PrintBarcode(config);
// printer.Dispose();
//
// Example 3: Save barcode to file (for testing)
// var printer = new BarcodePrinter3();
// var config = new BarcodePrinter3.BarcodeConfig
// {
//     BarcodeText = "1234567890128",
//     Label = "Test Product",
//     Size = BarcodePrinter3.BarcodeSize.Full
// };
// printer.PrintBarcode(config);
// printer.SaveBarcodeToFile("test_barcode.png");
// printer.Dispose();
