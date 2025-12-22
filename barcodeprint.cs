// using System;
// using System.Diagnostics;
// using System.Drawing;
// using System.Drawing.Printing;
// using System.Drawing.Text;

<<<<<<< HEAD
using System.Threading.Tasks;
//using BarcodeLib;
#if WINDOWS
using Microsoft.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
#endif
using ZXing;
using ZXing.Common;
using NetBarcode;
=======
// using System.Threading.Tasks;
// //using BarcodeLib;
// #if WINDOWS
// using Microsoft.UI.Xaml.Media.Imaging;
// using System.Runtime.InteropServices.WindowsRuntime;
// using Windows.Graphics.Imaging;
// using Windows.Storage.Streams;
// #endif
// using ZXing;
// using ZXing.Common;
>>>>>>> c41cdc60e14dbe5f141e814ce70b323d31c00f51

// public class BarcodePrinter
// {
//     bool issmall = false;
//     public  PrintDocument pd;

// public Bitmap img = null;
//     // Convert MM to Pixels (assuming 300 DPI)
//     private const float DPI = 300;
//     private const float MM_TO_INCH = 0.0393701f;
//     private  int barcodeWidth = 415;
//     private  int barcodeHeight = 177;

//     public async  void PrintBarcode(string barcodeText,double size =1,string Printername ="Xprinter XP-365B",string label="")
//     {
//                 try{
//                     if (size != 1){
//                         issmall = true;
//                     }

//    // barcodeHeight =(int)Math.Round( barcodeHeight * size);
//         int printWidth = 150 ; 
// int printHeight = 98;

// var barcode = new Barcode(barcodeText); // default: Code128
// // barcode = barcode.Configure (new BarcodeSettings(){ Text=barcodeText, ShowLabel= true, LabelPosition = LabelPosition.BottomCenter});
// // barcode.GetImage()
//         // ZXing.Net.Maui.BarcodeWriter writer = new ZXing.Net.Maui.BarcodeWriter(){
//         //      Format = BarcodeFormat.EAN_13,Options = new EncodingOptions { Width = 130 , Height = (int)Math.Round( barcodeHeight * size ),NoPadding =true, PureBarcode=true,Margin=0 }
//         // };
    
// // var ll = writer.Write(barcodeText);
// // var bitmab= await ConvertWriteableBitmapToBitmapAsync(ll);

// var type = NetBarcode.Type.Code128;
// if (barcodeText.Length == 13){
// type = NetBarcode.Type.EAN13;
// }
// System.Drawing. Font font = new System.Drawing. Font("Microsoft Sans Serif", 8, FontStyle.Bold );
// System.Drawing. Font font2 = new System.Drawing. Font("Arial", 4, FontStyle.Bold );
// var barcodeImag = new NetBarcode.Barcode( barcodeText, type,true,150,50,font );

// var barcodeImage = barcodeImag.GetImage();
// var barcodee = new Bitmap(barcodeImage);

// int finalWidth = barcodee.Width;
// int finalHeight = barcodee.Height + 30; // extra space for text

// // Create new image with extra space
// Bitmap finalImag = new Bitmap(finalWidth, finalHeight);
// finalImag.SetResolution(300, 300);
// using (Graphics g = Graphics.FromImage(finalImag))
// {
//     g.Clear(System.Drawing.Color.White);

//     // Apply high-quality settings for sharp barcode printing
//     g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;  // Best for sharpness, no smoothing
//     g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;  // High quality for combining graphics
//     g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;  // Disable smoothing (essential for thermal prints)
//     g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;  // Ensures optimal pixel placement
//     g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;  // No anti-aliasing, sharp text for barcodes

//     // Draw barcode
//     g.DrawImage(barcodee, 0, 0, finalWidth, barcodee.Height);

//     // Draw text under it

//     string text = label;

//     StringFormat format = new StringFormat
//     {
//         Alignment = StringAlignment.Center,
//         FormatFlags = StringFormatFlags.DirectionRightToLeft
//     };

//     RectangleF textRect = new RectangleF(0, barcodee.Height + 2, finalWidth, 28);
//     g.DrawString(text, font2, Brushes.Black, textRect, format);
// }
// img = finalImag;
// //var barcodeImage = barcode.Encode(TYPE.CODE128, barcodeText,System.Drawing.Color.Black, System.Drawing.Color.White, 300, 100);

// // Create a new image with space for the text below
// // int width = barcodeImage.Width;
// // int height = barcodeImage.Height + 60; // extra height for text

// // Bitmap finalImage = new Bitmap(width, height);
// // using (Graphics g = Graphics.FromImage(finalImage))
// // {
// //     g.Clear(System.Drawing.Color.White);
// //     g.DrawImage(barcodeImage, new System.Drawing.Point(0, 0));

// //     // Set Arabic font
  
// //     StringFormat format = new StringFormat();
    
// //     // Center text horizontally
// //     format.Alignment = StringAlignment.Center; 
// //        System.Drawing.Font arabicFont = new  System.Drawing.Font("Arial", 12, FontStyle.Bold);
  
// //     // Make sure Arabic text is right-to-left
// //     format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
 
// //     // Enable Anti-aliasing for smoother text rendering
// //     g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

// //     // Draw the Arabic product name centered below the barcode
// //     g.DrawString("بطاطس مقلية",arabicFont , Brushes.Black, new RectangleF(0, barcodeImage.Height, width, 40), format);
// // }

// // using (Graphics g = Graphics.FromImage(finalImage))
// // {
// //     g.Clear(System.Drawing. Color.White);
 
// //     // Draw barcode
// //     g.DrawImage(bitmab,arabicFont, 0, 0, 130,(int)Math.Round( 80 *size));

// //     // Draw text below
// //     using (System.Drawing.Font font = new System.Drawing. Font("Arial",8, FontStyle.Bold))
// //     using (System.Drawing.Brush brush = new SolidBrush(System.Drawing.Color.Black))
// //     {
// //     g.TextRenderingHint =System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
// //        System.Drawing. SizeF textSize = g.MeasureString(barcodeText, font);
// //         float textX = (150 - textSize.Width) / 2; // Center text
// //         float textY =(int)Math.Round( 80 * size +5); // Just below barcode
// //         g.DrawString(barcodeText, font, brush, textX, textY);
// //     }
    
  
  
// //     }
//  //   img = finalImage;
    
      

 

//      }catch(Exception ex){
// Debug.WriteLine(ex);
//         }
  
//       pd  = new PrintDocument();
    
//         pd.OriginAtMargins = true;
//        pd.DefaultPageSettings.PaperSize = new PaperSize("Sticker", 150,98 ); // 38.1mm × 24.8mm
// pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0); // Remove margins
//         // Set printer name (replace with your barcode printer's name)
 
//         pd.PrinterSettings.PrinterName =  Printername; 
//         pd.PrintPage += Pd_PrintPage;
       
//         pd.Print();
//     }
//     public void printReceipt(string PrinterName="XP-80C (copy 1)"){
//           pd  = new PrintDocument();
//         pd.OriginAtMargins = true;
     
//    //    pd.DefaultPageSettings.PaperSize.Width = 300;
// pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 10); // Remove margins
//         // Set printer name (replace with your barcode printer's name)
 
//         pd.PrinterSettings.PrinterName = PrinterName; 
//         pd.PrintPage += Pd_PrinRecipt;
       
//         pd.Print();
//     }
//  private  async void Pd_PrinRecipt(object sender, PrintPageEventArgs e)
//     {
//          // Define the font for the receipt
//          System.Drawing.Font font = new  System.Drawing.Font("Arial", 12);

//         // Define the receipt content
//         string[] receiptLines = {
//             "شركة القاضي",
//             "شارع العباسي القديم",
//             "Address Line 2",
//             "",
//             "مقلمة: 10",
//             "مقلمة: 10",
//             "------------------",
//             "Total: 20.00"
//         };

//         // Calculate the horizontal center position
//         float xCenter = 300 / 2;

//         // Set the starting vertical position
//         float yPosition = e.MarginBounds.Top;

//         // Loop through each line of the receipt
//         foreach (string line in receiptLines)
//         {
//             // Measure the width of the current line
//          System.Drawing.SizeF stringSize = e.Graphics.MeasureString(line, font);

//             // Calculate the X position to center the text
//             float xPosition = xCenter - (stringSize.Width / 2);

//             // Draw the string at the calculated position
//             e.Graphics.DrawString(line, font, Brushes.Black, xPosition, yPosition);

//             // Move to the next line
//             yPosition += stringSize.Height;
//         }
//         pd.PrintPage -= Pd_PrinRecipt;
//     }
  

//     private  async void Pd_PrintPage(object sender, PrintPageEventArgs e)
//     {
//         // Apply high-quality settings for sharp barcode printing
//         e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
//         e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
//         e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
//         e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
//         e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

//          if(issmall){

//                 float xCenter = 300 / 2;

//      int hi =(int)Math.Round( e.PageBounds.Height *.5);

//        e.Graphics.DrawImage(img, 1, 5, e.PageBounds.Width-30,49 );
//         e.Graphics.DrawImage(img, 1,5+ hi, e.PageBounds.Width-30, 49);
//  }
//  else{
//  e.Graphics.DrawImage(img, 0, 9, e.PageBounds.Width, e.PageBounds.Height);
//  }



//     }
//     #if WINDOWS
//     public async Task<Bitmap> ConvertWriteableBitmapToBitmapAsync(WriteableBitmap writeableBitmap)
//     {
//         using (var stream = new InMemoryRandomAccessStream())
//         {
//             // Encode WriteableBitmap to PNG
//             BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
//             Stream pixelStream = writeableBitmap.PixelBuffer.AsStream();
//             byte[] pixels = new byte[pixelStream.Length];
//             await pixelStream.ReadAsync(pixels, 0, pixels.Length);

//             encoder.SetPixelData(
//                 BitmapPixelFormat.Bgra8,
//                 BitmapAlphaMode.Ignore,
//                 (uint)writeableBitmap.PixelWidth,
//                 (uint)writeableBitmap.PixelHeight,
//                 300, 300, // DPI
//                 pixels);

//             await encoder.FlushAsync();

//             // Convert PNG stream to System.Drawing.Bitmap
//             using (var memoryStream = new MemoryStream())
//             {
//                 stream.Seek(0);
//                 await stream.AsStreamForRead().CopyToAsync(memoryStream);
//                 return new Bitmap(memoryStream);
//             }
//         }
//     }
// #endif
// }

// Call this function to print
//BarcodePrinter.PrintBarcode();


