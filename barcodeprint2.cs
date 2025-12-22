// using System;
// using System.Diagnostics;
// using System.Drawing;
// using System.Drawing.Printing;
// using System.Drawing.Text;
// using System.Drawing.Drawing2D;
// using System.Threading.Tasks;

// public class BarcodePrinter2
// {
//     // High-quality DPI for barcode generation
//     private const int IMAGE_DPI = 600;  // Higher DPI for better quality
//     private const int PRINTER_DPI = 203; // Thermal printer typical DPI

//     // Paper sizes in hundredths of an inch
//     private const int PAPER_WIDTH = 591;  // 150mm ≈ 591 hundredths of inch
//     private const int PAPER_HEIGHT = 386; // 98mm ≈ 386 hundredths of inch

//     private Bitmap generatedImage = null;
//     private PrintDocument printDoc;
//     private BarcodeConfig config;

//     public class BarcodeConfig
//     {
//         public string BarcodeText { get; set; }
//         public string Label { get; set; } = "";
//         public string PrinterName { get; set; } = "Xprinter XP-365B";
//         public BarcodeSize Size { get; set; } = BarcodeSize.Full;
//         public int BarcodeWidth { get; set; } = 400;
//         public int BarcodeHeight { get; set; } = 120;
//         public bool ShowLabel { get; set; } = true;
//         public float FontSize { get; set; } = 8f;
//         public int Margin { get; set; } = 5;
//     }

//     public enum BarcodeSize
//     {
//         Full,      // Single barcode, full size
//         Half,      // Two barcodes per label
//         Quarter    // Four barcodes per label
//     }

//     public BarcodePrinter2()
//     {
//         printDoc = new PrintDocument();
//     }

//     /// <summary>
//     /// Main method to print barcode with improved quality and size handling
//     /// </summary>
//     public void PrintBarcode(BarcodeConfig configuration)
//     {
//         try
//         {
//             config = configuration;

//             // Generate high-quality barcode image
//             generatedImage = GenerateHighQualityBarcode(config);

//             // Configure print document
//             ConfigurePrintDocument(config);

//             // Print
//             printDoc.Print();
//         }
//         catch (Exception ex)
//         {
//             Debug.WriteLine($"Print error: {ex.Message}");
//             throw;
//         }
//     }

//     /// <summary>
//     /// Generates a high-quality barcode image with proper scaling and anti-aliasing
//     /// </summary>
//     private Bitmap GenerateHighQualityBarcode(BarcodeConfig config)
//     {
//         // Determine barcode type based on length
//         NetBarcode.Type barcodeType = config.BarcodeText.Length == 13
//             ? NetBarcode.Type.EAN13
//             : NetBarcode.Type.Code128;

//         // Calculate dimensions based on IMAGE_DPI for high quality
//         int barcodeWidth = config.BarcodeWidth;
//         int barcodeHeight = config.BarcodeHeight;

<<<<<<< HEAD
        // Generate barcode using NetBarcode
        System.Drawing.Font barcodeFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
        var barcodeGenerator = new NetBarcode.Barcode(
            config.BarcodeText,
            barcodeType,
            true,
            barcodeWidth,
            barcodeHeight,
            barcodeFont
        );
=======
//         // Generate barcode using NetBarcode
//         Font barcodeFont = new Font("Arial", 10, FontStyle.Bold);
//         var barcodeGenerator = new NetBarcode.Barcode(
//             config.BarcodeText,
//             barcodeType,
//             true,
//             barcodeWidth,
//             barcodeHeight,
//             barcodeFont
//         );
>>>>>>> c41cdc60e14dbe5f141e814ce70b323d31c00f51

//         var barcodeImg = barcodeGenerator.GetImage();
//         var barcodeBitmap = new Bitmap(barcodeImg);

//         // Calculate final dimensions with label space
//         int finalWidth = barcodeBitmap.Width;
//         int labelHeight = config.ShowLabel ? 40 : 0;
//         int finalHeight = barcodeBitmap.Height + labelHeight + (config.Margin * 2);

//         // Create high-quality final image
//         Bitmap finalImage = new Bitmap(finalWidth, finalHeight);
//         finalImage.SetResolution(IMAGE_DPI, IMAGE_DPI);

//         using (Graphics g = Graphics.FromImage(finalImage))
//         {
//             // Set optimal rendering settings for barcode printing
//             ConfigureGraphicsQuality(g, true);

<<<<<<< HEAD
            // White background
            g.Clear(System.Drawing.Color.White);
=======
//             // White background
//             g.Clear(Color.White);
>>>>>>> c41cdc60e14dbe5f141e814ce70b323d31c00f51

//             // Draw barcode
//             int yPosition = config.Margin;
//             g.DrawImage(barcodeBitmap, 0, yPosition, finalWidth, barcodeBitmap.Height);

//             // Draw label if needed
//             if (config.ShowLabel && !string.IsNullOrEmpty(config.Label))
//             {
//                 DrawLabel(g, config.Label, finalWidth, barcodeBitmap.Height + config.Margin,
//                          finalWidth, labelHeight, config.FontSize);
//             }
//         }

//         barcodeBitmap.Dispose();
//         barcodeFont.Dispose();

//         return finalImage;
//     }

<<<<<<< HEAD
    /// <summary>
    /// Draws text label with proper Arabic/RTL support
    /// </summary>
    private void DrawLabel(Graphics g, string text, float x, float y, float width, float height, float fontSize)
    {
        using ( System.Drawing.Font labelFont = new  System.Drawing.Font("Arial", fontSize, FontStyle.Bold))
        {
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.DirectionRightToLeft
            };
=======
//     /// <summary>
//     /// Draws text label with proper Arabic/RTL support
//     /// </summary>
//     private void DrawLabel(Graphics g, string text, float x, float y, float width, float height, float fontSize)
//     {
//         using (Font labelFont = new Font("Arial", fontSize, FontStyle.Bold))
//         {
//             StringFormat format = new StringFormat
//             {
//                 Alignment = StringAlignment.Center,
//                 LineAlignment = StringAlignment.Center,
//                 FormatFlags = StringFormatFlags.DirectionRightToLeft
//             };
>>>>>>> c41cdc60e14dbe5f141e814ce70b323d31c00f51

//             RectangleF textRect = new RectangleF(x, y, width, height);
//             g.DrawString(text, labelFont, Brushes.Black, textRect, format);
//         }
//     }

//     /// <summary>
//     /// Configures graphics object for optimal barcode quality
//     /// </summary>
//     private void ConfigureGraphicsQuality(Graphics g, bool forBarcode = true)
//     {
//         if (forBarcode)
//         {
//             // For barcode: sharp, no anti-aliasing
//             g.InterpolationMode = InterpolationMode.NearestNeighbor;
//             g.SmoothingMode = SmoothingMode.None;
//             g.PixelOffsetMode = PixelOffsetMode.Half;
//             g.CompositingQuality = CompositingQuality.HighQuality;
//             g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
//         }
//         else
//         {
//             // For general graphics: high quality with smoothing
//             g.InterpolationMode = InterpolationMode.HighQualityBicubic;
//             g.SmoothingMode = SmoothingMode.HighQuality;
//             g.PixelOffsetMode = PixelOffsetMode.HighQuality;
//             g.CompositingQuality = CompositingQuality.HighQuality;
//             g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
//         }
//     }

//     /// <summary>
//     /// Configures print document with proper settings
//     /// </summary>
//     private void ConfigurePrintDocument(BarcodeConfig config)
//     {
//         printDoc.DocumentName = $"Barcode_{config.BarcodeText}";
//         printDoc.OriginAtMargins = true;

//         // Set paper size
//         printDoc.DefaultPageSettings.PaperSize = new PaperSize(
//             "Barcode Label",
//             PAPER_WIDTH,
//             PAPER_HEIGHT
//         );

//         // Remove margins for full use of label
//         printDoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

//         // Set printer
//         printDoc.PrinterSettings.PrinterName = config.PrinterName;

//         // Set to maximum printer quality
//         printDoc.DefaultPageSettings.PrinterResolution = GetHighestPrinterResolution(printDoc.PrinterSettings);

//         // Attach print handler
//         printDoc.PrintPage += OnPrintPage;
//     }

//     /// <summary>
//     /// Gets the highest available printer resolution
//     /// </summary>
//     private PrinterResolution GetHighestPrinterResolution(PrinterSettings printerSettings)
//     {
//         PrinterResolution highestResolution = printerSettings.DefaultPageSettings.PrinterResolution;
//         int maxDpi = 0;

//         foreach (PrinterResolution resolution in printerSettings.PrinterResolutions)
//         {
//             if (resolution.Kind == PrinterResolutionKind.Custom)
//             {
//                 int dpi = Math.Max(resolution.X, resolution.Y);
//                 if (dpi > maxDpi)
//                 {
//                     maxDpi = dpi;
//                     highestResolution = resolution;
//                 }
//             }
//         }

//         return highestResolution;
//     }

//     /// <summary>
//     /// Print page event handler with size-aware rendering
//     /// </summary>
//     private void OnPrintPage(object sender, PrintPageEventArgs e)
//     {
//         if (generatedImage == null)
//             return;

//         // Configure print graphics for maximum quality
//         ConfigureGraphicsQuality(e.Graphics, true);

//         // Get printable area
//         Rectangle printArea = e.MarginBounds;
//         if (printDoc.OriginAtMargins == false)
//         {
//             printArea = e.PageBounds;
//         }

//         // Calculate scaling to fit the print area while maintaining aspect ratio
//         float scaleX = (float)printArea.Width / generatedImage.Width;
//         float scaleY = (float)printArea.Height / generatedImage.Height;
//         float scale = Math.Min(scaleX, scaleY);

//         int scaledWidth = (int)(generatedImage.Width * scale);
//         int scaledHeight = (int)(generatedImage.Height * scale);

//         // Center the barcode on the label
//         int x = printArea.X + (printArea.Width - scaledWidth) / 2;
//         int y = printArea.Y + (printArea.Height - scaledHeight) / 2;

//         // Handle different size modes
//         switch (config.Size)
//         {
//             case BarcodeSize.Full:
//                 // Single barcode, full size
//                 e.Graphics.DrawImage(generatedImage, x, y, scaledWidth, scaledHeight);
//                 break;

//             case BarcodeSize.Half:
//                 // Two barcodes vertically
//                 int halfHeight = printArea.Height / 2;
//                 DrawScaledBarcode(e.Graphics, printArea.X, printArea.Y,
//                                  printArea.Width, halfHeight);
//                 DrawScaledBarcode(e.Graphics, printArea.X, printArea.Y + halfHeight,
//                                  printArea.Width, halfHeight);
//                 break;

//             case BarcodeSize.Quarter:
//                 // Four barcodes in a 2x2 grid
//                 int quarterWidth = printArea.Width / 2;
//                 int quarterHeight = printArea.Height / 2;

//                 DrawScaledBarcode(e.Graphics, printArea.X, printArea.Y,
//                                  quarterWidth, quarterHeight);
//                 DrawScaledBarcode(e.Graphics, printArea.X + quarterWidth, printArea.Y,
//                                  quarterWidth, quarterHeight);
//                 DrawScaledBarcode(e.Graphics, printArea.X, printArea.Y + quarterHeight,
//                                  quarterWidth, quarterHeight);
//                 DrawScaledBarcode(e.Graphics, printArea.X + quarterWidth, printArea.Y + quarterHeight,
//                                  quarterWidth, quarterHeight);
//                 break;
//         }

//         e.HasMorePages = false;
//     }

//     /// <summary>
//     /// Draws barcode scaled to fit specific area
//     /// </summary>
//     private void DrawScaledBarcode(Graphics g, int x, int y, int width, int height)
//     {
//         if (generatedImage == null)
//             return;

//         // Calculate aspect ratio scaling
//         float scaleX = (float)width / generatedImage.Width;
//         float scaleY = (float)height / generatedImage.Height;
//         float scale = Math.Min(scaleX, scaleY) * 0.95f; // 95% to add small margin

//         int scaledWidth = (int)(generatedImage.Width * scale);
//         int scaledHeight = (int)(generatedImage.Height * scale);

//         // Center in the area
//         int centeredX = x + (width - scaledWidth) / 2;
//         int centeredY = y + (height - scaledHeight) / 2;

//         g.DrawImage(generatedImage, centeredX, centeredY, scaledWidth, scaledHeight);
//     }

//     /// <summary>
//     /// Cleanup resources
//     /// </summary>
//     public void Dispose()
//     {
//         generatedImage?.Dispose();
//         printDoc?.Dispose();
//     }

<<<<<<< HEAD
    /// <summary>
    /// Simple helper method for quick printing
    /// </summary>
    public static void QuickPrint(string barcodeText, string label = "",
                                  string printerName = "Xprinter XP-365B",
                                  BarcodeSize size = BarcodeSize.Full)
    {
     var printer = new BarcodePrinter2();
 
            var config = new BarcodeConfig
            {
                BarcodeText = barcodeText,
                Label = label,
                PrinterName = printerName,
                Size = size,
                BarcodeWidth = 400,
                BarcodeHeight = 120,
                ShowLabel = !string.IsNullOrEmpty(label),
                FontSize = 8f,
                Margin = 5
            };

            printer.PrintBarcode(config);
        
    }
}
=======
//     /// <summary>
//     /// Simple helper method for quick printing
//     /// </summary>
//     public static void QuickPrint(string barcodeText, string label = "",
//                                   string printerName = "Xprinter XP-365B",
//                                   BarcodeSize size = BarcodeSize.Full)
//     {
//         using (var printer = new BarcodePrinter2())
//         {
//             var config = new BarcodeConfig
//             {
//                 BarcodeText = barcodeText,
//                 Label = label,
//                 PrinterName = printerName,
//                 Size = size,
//                 BarcodeWidth = 400,
//                 BarcodeHeight = 120,
//                 ShowLabel = !string.IsNullOrEmpty(label),
//                 FontSize = 8f,
//                 Margin = 5
//             };

//             printer.PrintBarcode(config);
//         }
//     }
// }
>>>>>>> c41cdc60e14dbe5f141e814ce70b323d31c00f51

// Usage Examples:
//
// Example 1: Simple quick print
// BarcodePrinter2.QuickPrint("1234567890128", "منتج 1");
//
// Example 2: Full-size barcode with custom settings
// var printer = new BarcodePrinter2();
// var config = new BarcodePrinter2.BarcodeConfig
// {
//     BarcodeText = "1234567890128",
//     Label = "منتج 1",
//     PrinterName = "Xprinter XP-365B",
//     Size = BarcodePrinter2.BarcodeSize.Full,
//     BarcodeWidth = 450,
//     BarcodeHeight = 140,
//     ShowLabel = true,
//     FontSize = 10f,
//     Margin = 8
// };
// printer.PrintBarcode(config);
// printer.Dispose();
//
// Example 3: Print two barcodes per label
// BarcodePrinter2.QuickPrint("1234567890128", "منتج صغير", "Xprinter XP-365B", BarcodePrinter2.BarcodeSize.Half);
//
// Example 4: Print four small barcodes per label
// BarcodePrinter2.QuickPrint("123456789", "", "Xprinter XP-365B", BarcodePrinter2.BarcodeSize.Quarter);
