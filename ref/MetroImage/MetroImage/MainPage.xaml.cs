using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Graphics;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.Capture;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Media;


namespace MetroImage
{
   partial class MainPage
   {
      public MainPage()
      {
         InitializeComponent();
         DataTransferManager.GetForCurrentView().DataRequested +=
         new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(MainPage_DataRequested);
      }

      public struct PixelColor
      {
          public byte Blue;
          public byte Green;
          public byte Red;
          public byte Alpha;
      }
      
      public PixelColor GetPixel(int x, int y, byte[] rawpixel,int width,int hight)
      {
          PixelColor pointpixel;
          int offset = y * width * 4 + x * 4;
          pointpixel.Blue = rawpixel[offset + 0];
          pointpixel.Green = rawpixel[offset + 1];
          pointpixel.Red = rawpixel[offset + 2];
          pointpixel.Alpha = rawpixel[offset + 3];  
          return pointpixel;
      }

      public byte[] PutPixel(byte[] rawimagepixel,int width,int hight, PixelColor pixels, int x, int y)
      {           
          int offset = y * width * 4 + x * 4;
          rawimagepixel[offset + 0] = pixels.Blue;
          rawimagepixel[offset + 1] = pixels.Green;
          rawimagepixel[offset + 2] = pixels.Red;
          rawimagepixel[offset + 3] = pixels.Alpha;
          return rawimagepixel;
      }

      private WriteableBitmap NegativeImage(WriteableBitmap modifiedImage)
      {          
          int h = modifiedImage.PixelHeight;
          int w = modifiedImage.PixelWidth;
       
          int i;
          Stream stream = modifiedImage.PixelBuffer.AsStream();        

          byte[] StreamBuffer = new byte[stream.Length];
          stream.Seek(0, 0);
          stream.Read(StreamBuffer, 0, StreamBuffer.Length);

          for (i = 0; i < StreamBuffer.Length-4; i=i+4)
          {
              var a1 = 0xff;
              var r1 = 0xff;
              var g1 = 0xff;
              var b1 = 0xff;

              var a2 = StreamBuffer[i + 3];
              var r2 = StreamBuffer[i + 2];
              var g2 = StreamBuffer[i + 1];
              var b2 = StreamBuffer[i + 0]; 

              StreamBuffer[i + 3] = (byte)(a1 - a2);
              StreamBuffer[i + 2] = (byte)(r1 - r2);
              StreamBuffer[i + 1] = (byte)(g1 - g2);
              StreamBuffer[i + 0] = (byte)(b1 - b2);
          }
         
          stream.Seek(0, 0);
          stream.Write(StreamBuffer, 0, StreamBuffer.Length);
          modifiedImage.Invalidate();
          return modifiedImage;
      }
    
      private WriteableBitmap EmbossmentImage(WriteableBitmap modifiedImage)
      {
          //PixelColor PC1, PC2, PC3;
          int h = modifiedImage.PixelHeight;
          int w = modifiedImage.PixelWidth;
          int i;
          int r = 0, g = 0, b = 0,  a= 0;
          Stream stream = modifiedImage.PixelBuffer.AsStream();
          byte[] StreamBuffer = new byte[stream.Length];
          byte[] tempbuffer = new byte[64000];
          stream.Seek(0, 0);
          stream.Read(StreamBuffer, 0, StreamBuffer.Length);

          for (i = 0; i < StreamBuffer.Length - 4; i = i + 4)
          {
              var a1 = StreamBuffer[i + 3];
              var r1 = StreamBuffer[i + 2];
              var g1 = StreamBuffer[i + 1];
              var b1 = StreamBuffer[i + 0];

              var a2 = StreamBuffer[i + 7];
              var r2 = StreamBuffer[i + 6];
              var g2 = StreamBuffer[i + 5];
              var b2 = StreamBuffer[i + 4];
              
              a = 0xff;
              r = Math.Abs(r1 - r2 + 128);
              g = Math.Abs(g1 - g2 + 128);
              b = Math.Abs(b1 - b2 + 128);             
            
              StreamBuffer[i + 3] = (byte)a;
              StreamBuffer[i + 2] = (byte)(r > 255 ? 255 : (r < 0 ? 0 : r));
              StreamBuffer[i + 1] = (byte)(g > 255 ? 255 : (g < 0 ? 0 : g));
              StreamBuffer[i + 0] = (byte)(b > 255 ? 255 : (b < 0 ? 0 : b));
          }                      

          stream.Seek(0, 0);
          stream.Write(StreamBuffer, 0, StreamBuffer.Length);
          modifiedImage.Invalidate();
          return modifiedImage;
      }       
    
      private WriteableBitmap ColorFilterImage(WriteableBitmap modifiedImage, double R, double G, double B)
      {
          int h = modifiedImage.PixelHeight;
          int w = modifiedImage.PixelWidth;
          int r = 0, g = 0, b = 0, a = 0, i= 0;
          Stream stream = modifiedImage.PixelBuffer.AsStream();
          byte[] StreamBuffer = new byte[stream.Length]; 
          stream.Seek(0, 0);
          stream.Read(StreamBuffer, 0, StreamBuffer.Length);

          for (i = 0; i < StreamBuffer.Length - 4; i = i + 4)
          {
              var a1 = StreamBuffer[i + 3];
              var r1 = StreamBuffer[i + 2];
              var g1 = StreamBuffer[i + 1];
              var b1 = StreamBuffer[i + 0];

              a = 0xff;
              r = (int)(r1 * R);
              g = (int)(g1 * G);
              b = (int)(b1 * B);

              StreamBuffer[i + 3] = (byte)a;
              StreamBuffer[i + 2] = (byte)(r > 255 ? 255 : (r < 0 ? 0 : r));
              StreamBuffer[i + 1] = (byte)(g > 255 ? 255 : (g < 0 ? 0 : g));
              StreamBuffer[i + 0] = (byte)(b > 255 ? 255 : (b < 0 ? 0 : b));
          }
          stream.Seek(0, 0);
          stream.Write(StreamBuffer, 0, StreamBuffer.Length);
          modifiedImage.Invalidate();
          return modifiedImage;
      }
            
      private WriteableBitmap GrayScale(WriteableBitmap modifiedImage)
      {
          int r = 0, a = 0, i = 0;
          int h = modifiedImage.PixelHeight;
          int w = modifiedImage.PixelWidth;
          Stream stream = modifiedImage.PixelBuffer.AsStream();
          byte[] StreamBuffer = new byte[stream.Length];
          byte[] tempbuffer = new byte[64000];
          stream.Seek(0, 0);
          stream.Read(StreamBuffer, 0, StreamBuffer.Length);

          for (i = 0; i < StreamBuffer.Length - 4; i = i + 4)
          {
              var a1 = StreamBuffer[i + 3];
              var r1 = StreamBuffer[i + 2];
              var g1 = StreamBuffer[i + 1];
              var b1 = StreamBuffer[i + 0];

              a = 0xff;
              r = (byte)((0.311 * r1) + (0.486 * g1) + (0.213 * b1));     
              StreamBuffer[i + 3] = (byte)a;
              StreamBuffer[i + 2] = (byte)(r > 255 ? 255 : (r < 0 ? 0 : r));
              StreamBuffer[i + 1] = (byte)(r > 255 ? 255 : (r < 0 ? 0 : r));
              StreamBuffer[i + 0] = (byte)(r > 255 ? 255 : (r < 0 ? 0 : r));
          }
          stream.Seek(0, 0);
          stream.Write(StreamBuffer, 0, StreamBuffer.Length);
          modifiedImage.Invalidate();
          return modifiedImage;
      }
       
      private WriteableBitmap BrightnessImage(WriteableBitmap modifiedImage, int bright)
      {          
          int r = 0, g = 0, b = 0, a = 0, i = 0;
          int h = modifiedImage.PixelHeight;
          int w = modifiedImage.PixelWidth;
          Stream stream = modifiedImage.PixelBuffer.AsStream();
          byte[] StreamBuffer = new byte[stream.Length];
          stream.Seek(0, 0);
          stream.Read(StreamBuffer, 0, StreamBuffer.Length);

          for (i = 0; i < StreamBuffer.Length - 4; i = i + 4)
          {
              var a1 = StreamBuffer[i + 3];
              var r1 = StreamBuffer[i + 2];
              var g1 = StreamBuffer[i + 1];
              var b1 = StreamBuffer[i + 0];

              a = 0xff;
              r = r1 + bright;
              g = g1 + bright;
              b = b1 + bright;        

              StreamBuffer[i + 3] = (byte)a;
              StreamBuffer[i + 2] = (byte)(r > 255 ? 255 : (r < 0 ? 0 : r));
              StreamBuffer[i + 1] = (byte)(g > 255 ? 255 : (g < 0 ? 0 : g));
              StreamBuffer[i + 0] = (byte)(b > 255 ? 255 : (b < 0 ? 0 : b));
          }         
          stream.Seek(0, 0);
          stream.Write(StreamBuffer, 0, StreamBuffer.Length);
          modifiedImage.Invalidate();
          return modifiedImage;
      }
       
      
      private WriteableBitmap GammaImage(WriteableBitmap modifiedImage, double r, double g, double b)
      {
          int i = 0;
          int h = modifiedImage.PixelHeight;
          int w = modifiedImage.PixelWidth;
          Stream stream = modifiedImage.PixelBuffer.AsStream();
          byte[] StreamBuffer = new byte[stream.Length];
          byte[] tempbuffer = new byte[64000];
          stream.Seek(0, 0);
          stream.Read(StreamBuffer, 0, StreamBuffer.Length);  
          byte[] redGamma = new byte[256];
          byte[] greenGamma = new byte[256];
          byte[] blueGamma = new byte[256];

          for (i = 0; i < 256; ++i)
          {
              redGamma[i]   =  (byte)Math.Min(255, (int)((255.0 * Math.Pow(i / 255.0, 1.0 / r)) + 0.5));
              greenGamma[i] =  (byte)Math.Min(255, (int)((255.0 * Math.Pow(i / 255.0, 1.0 / g)) + 0.5));
              blueGamma[i]  =  (byte)Math.Min(255, (int)((255.0 * Math.Pow(i / 255.0, 1.0 / b)) + 0.5));
          }

          for (i = 0; i < StreamBuffer.Length - 4; i = i + 4)
          {           
              StreamBuffer[i + 3] = (byte)0xff;
              StreamBuffer[i + 2] = (byte)redGamma[StreamBuffer[i + 2]];
              StreamBuffer[i + 1] = (byte)greenGamma[StreamBuffer[i + 1]];
              StreamBuffer[i + 0] = (byte)blueGamma[StreamBuffer[i + 0]];
          }
        
          stream.Seek(0, 0);
          stream.Write(StreamBuffer, 0, StreamBuffer.Length);
          modifiedImage.Invalidate();
          return modifiedImage;
      }
      
      private WriteableBitmap TintImage(WriteableBitmap modifiedImage)
      {
          
          byte A = 255;
          int h = modifiedImage.PixelHeight;
          int w = modifiedImage.PixelWidth;
          int i = 0;  
        
          Stream stream = modifiedImage.PixelBuffer.AsStream();
          byte[] StreamBuffer = new byte[stream.Length];
          stream.Seek(0, 0);
          stream.Read(StreamBuffer, 0, StreamBuffer.Length);

          for (i = 0; i < StreamBuffer.Length - 4; i = i + 4)
          {
              var a1 = StreamBuffer[i + 3];
              var r1 = StreamBuffer[i + 2];
              var g1 = StreamBuffer[i + 1];
              var b1 = StreamBuffer[i + 0];

              int gray = ((byte)(r1 * 0.2126 + g1 * 0.7152 + b1 * 0.0722));

              StreamBuffer[i + 3] = (byte)((A * a1) >> 8);
              StreamBuffer[i + 2] = (byte)((gray * r1) >> 8);
              StreamBuffer[i + 1] = (byte)((gray * g1) >> 8);
              StreamBuffer[i + 0] = (byte)((gray * b1) >> 8);              
          }   
          stream.Seek(0, 0);
          stream.Write(StreamBuffer, 0, StreamBuffer.Length);
          modifiedImage.Invalidate();
          return modifiedImage;
      }  

      private WriteableBitmap SunLight(WriteableBitmap modifiedImage)
      {
          PixelColor PC;
          int h = modifiedImage.PixelHeight;
          int w = modifiedImage.PixelWidth;
          int i, j;
          Stream stream = modifiedImage.PixelBuffer.AsStream();
          byte[] StreamBuffer = new byte[stream.Length];

          stream.Seek(0, 0);
          stream.Read(StreamBuffer, 0, StreamBuffer.Length);

          Point MyCenter = new Point(w / 2, h / 2);
          int loopcount = modifiedImage.PixelWidth / 10;
          int R = Math.Min(w / 2, h / 2);

          for (int index = 0; index < StreamBuffer.Length - 4; index = index + 4)
          {
              i = (index / 4) % w;
              j = (index / 4) / w;

              float MyLength = (float)Math.Sqrt(Math.Pow((i - MyCenter.X), 2) + Math.Pow((j - MyCenter.Y), 2));

              if (MyLength < R)
              {
                  PC = GetPixel(i, j, StreamBuffer, w, h);
                  int r, g, b;
                  float MyPixel = 200.0f * (1.0f - MyLength / R);
                  r = PC.Red + (int)MyPixel;
                  PC.Red = (byte)Math.Max(0, Math.Min(r, 255));
                  g = PC.Green + (int)MyPixel;
                  PC.Green = (byte)Math.Max(0, Math.Min(g, 255));
                  b = PC.Blue + (int)MyPixel;
                  PC.Blue = (byte)Math.Max(0, Math.Min(b, 255));
                  PutPixel(StreamBuffer, w, h, PC, i, j);
              }
          }

          stream.Seek(0, 0);
          stream.Write(StreamBuffer, 0, StreamBuffer.Length);
          modifiedImage.Invalidate();
          return modifiedImage;
      }
       
      private WriteableBitmap OilPaintImage(WriteableBitmap modifiedImage)
      {
        
         PixelColor PC;
         int w = modifiedImage.PixelWidth;
         int h = modifiedImage.PixelHeight;

         int dw = modifiedImage.PixelWidth / 50;
         int dh = modifiedImage.PixelHeight / 50;

         Stream modifiedstream = modifiedImage.PixelBuffer.AsStream();
         byte[] modifiedStreamBuffer = new byte[modifiedstream.Length];
         modifiedstream.Seek(0, 0);
         modifiedstream.Read(modifiedStreamBuffer, 0, modifiedStreamBuffer.Length);

         WriteableBitmap bitmap = new WriteableBitmap(modifiedImage.PixelWidth, modifiedImage.PixelHeight);

         Stream bitmapstream = bitmap.PixelBuffer.AsStream();
         byte[] bitmapStreamBuffer = new byte[bitmapstream.Length];
         bitmapstream.Seek(0, 0);
         bitmapstream.Read(bitmapStreamBuffer, 0, bitmapStreamBuffer.Length);  
          
         Random rnd = new Random();
         int iModel = 10;
         int i = w - iModel;
         
         while (i > 1)
         {
              int j = h - iModel;
              while (j > 1)
              {
                  int iPos = rnd.Next(100000) % iModel;
                  PC = GetPixel((int)(i + iPos), (int)(j + iPos), modifiedStreamBuffer, w, h);
                  PutPixel(bitmapStreamBuffer, w, h, PC, i, j);
                  j = j - 1;
              }
              i = i - 1;
          }

         bitmapstream.Seek(0, 0);
         bitmapstream.Write(bitmapStreamBuffer, 0, bitmapStreamBuffer.Length);
         bitmap.Invalidate();
         return bitmap;
      }
       
      IRandomAccessStream stream;

      private void MainPage_DataRequested(DataTransferManager sender,DataRequestedEventArgs args)
      {
          if (stream == null)
             args.Request.FailWithDisplayText("No picture taken!");
         else
             args.Request.Data.SetBitmap(stream);
      }   

       private async void Tpicture_Click(object sender, RoutedEventArgs e)
      {
          var ui = new CameraCaptureUI();
          ui.PhotoSettings.CroppedAspectRatio = new Size(4, 3);

          var file = await ui.CaptureFileAsync(CameraCaptureUIMode.Photo);

          if (file != null)
          {
              stream = await file.OpenAsync(FileAccessMode.Read);
              var bitmap = new BitmapImage();
              bitmap.SetSource(stream);
              Image1.Source = bitmap;
          }
      }

      private async void Lfile_Click(object sender, RoutedEventArgs e)
      {
         FileOpenPicker openPicker = new FileOpenPicker();
         openPicker.FileTypeFilter.Add(".jpg");
         openPicker.FileTypeFilter.Add(".cmp");
         openPicker.FileTypeFilter.Add(".png");
         openPicker.FileTypeFilter.Add(".tif");
         openPicker.FileTypeFilter.Add(".gif");
         openPicker.FileTypeFilter.Add(".bmp");
         StorageFile file = await openPicker.PickSingleFileAsync();
         IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
         BitmapImage bmp = new BitmapImage();
         bmp.SetSource(stream);
         Image1.Source = bmp;
      }
       

      private void BlackWhite_Click(object sender, RoutedEventArgs e)
      {
          WriteableBitmap wb = Image1.Source as WriteableBitmap;

          if (wb == null)
          {
              BitmapSource bs = Image1.Source as BitmapSource;
              wb = new WriteableBitmap(bs);
          }
          //wb = NegativeImage(wb);
          //  wb = EmbossmentImage(wb);
          //  wb = ColorFilterImage(wb, 255, 0, 255);
            wb = GrayScale(wb);
          //  wb = BrightnessImage(wb, 150);
          //  wb = GammaImage(wb, 1,0.5,0.5);
          //  wb = SunLight(wb);
          //  wb = OilPaintImage(wb);
          //  wb = WaterfallImage(wb); //40sec
          //  wb = SharpImage(wb);
          //wb = Upsidedown(wb);
          //Imagetwist(wb, 68);
          Image1.Source = wb;
      }

      private void Cherish_Click(object sender, RoutedEventArgs e)
      {
          WriteableBitmap wb = Image1.Source as WriteableBitmap;
        
          if (wb == null)
          {
              BitmapSource bs = Image1.Source as BitmapSource;
              wb = new WriteableBitmap(bs);
          }
          wb = NegativeImage(wb);
          //  wb = EmbossmentImage(wb);
          //  wb = ColorFilterImage(wb, 255, 0, 255);
          //  wb = GrayScale(wb);
          //  wb = BrightnessImage(wb, 150);
          //  wb = GammaImage(wb, 1,0.5,0.5);
          //  wb = SunLight(wb);
          //  wb = OilPaintImage(wb);
          //  wb = WaterfallImage(wb); //40sec
          //  wb = SharpImage(wb);
          //  wb = Upsidedown(wb);
          //Imagetwist(wb, 68);

          Image1.Source = wb;        

      }

      private void SunLight_Click(object sender, RoutedEventArgs e)
      {
          WriteableBitmap wb = Image1.Source as WriteableBitmap;
          this.progress.Value = 20;

          if (wb == null)
          {
              BitmapSource bs = Image1.Source as BitmapSource;
              wb = new WriteableBitmap(bs);
          }
          //wb = NegativeImage(wb);
          //  wb = EmbossmentImage(wb);
          //  wb = ColorFilterImage(wb, 255, 0, 255);
          //wb = GrayScale(wb);
          //  wb = BrightnessImage(wb, 150);
          //  wb = GammaImage(wb, 1,0.5,0.5);
            wb = SunLight(wb);
            this.progress.Value = 50;
          //  wb = OilPaintImage(wb);
          //  wb = WaterfallImage(wb); //40sec
          //  wb = SharpImage(wb);
          //wb = Upsidedown(wb);
          //Imagetwist(wb, 68);
          this.progress.Value = 100;
          Image1.Source = wb;
      }

      private async void SaveBitmapImageToFile(WriteableBitmap writeableBitmap)
      {
          try
          {
              Stream stream = writeableBitmap.PixelBuffer.AsStream();
              byte[] StreamBuffer = new byte[(uint)stream.Length];
              await stream.ReadAsync(StreamBuffer, 0, StreamBuffer.Length);

              for (int i = 0; i < StreamBuffer.Length - 4; i = i + 4)
              {
                  byte B = StreamBuffer[i];
                  byte G = StreamBuffer[i + 1];
                  byte R = StreamBuffer[i + 2];
                  byte A = StreamBuffer[i + 3];         // convert to RGBA format for  BitmapEncoder
                  StreamBuffer[i] = R; // Red 
                  StreamBuffer[i + 1] = G; // Green
                  StreamBuffer[i + 2] = B; // Blue
                  StreamBuffer[i + 3] = A; // Alpha
              }

              StorageFolder pictureFolder = KnownFolders.PicturesLibrary;
              StorageFile sampleFile = await pictureFolder.CreateFileAsync("test.bmp", CreationCollisionOption.ReplaceExisting);
              IRandomAccessStream writeStream = await sampleFile.OpenAsync(FileAccessMode.ReadWrite);
              BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, writeStream);
              encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied, (uint)writeableBitmap.PixelWidth, (uint)writeableBitmap.PixelHeight, 96, 96, StreamBuffer);
              await encoder.FlushAsync();
              writeStream.GetOutputStreamAt(0).FlushAsync().Start();
          }

          catch (Exception ex)
          {
              string s = ex.ToString();
          }
      }    
 
      private void OilPaint_Click(object sender, RoutedEventArgs e)
      {
          WriteableBitmap wb = Image1.Source as WriteableBitmap;

          if (wb == null)
          {
              BitmapSource bs = Image1.Source as BitmapSource;
              wb = new WriteableBitmap(bs);
          }
          
          wb = OilPaintImage(wb);
          Image1.Source = wb;
      }

      private void Emboss_Click(object sender, RoutedEventArgs e)
      {
          WriteableBitmap wb = Image1.Source as WriteableBitmap;

          if (wb == null)
          {
              BitmapSource bs = Image1.Source as BitmapSource;
              wb = new WriteableBitmap(bs);
          }
          wb = EmbossmentImage(wb);         
          Image1.Source = wb;
      }

      private void Save_Click(object sender, RoutedEventArgs e)
      {
          WriteableBitmap wb = Image1.Source as WriteableBitmap;

          if (wb == null)
          {
              BitmapSource bs = Image1.Source as BitmapSource;
              wb = new WriteableBitmap(bs);
          }
          SaveBitmapImageToFile(wb);
          Image1.Source = wb;
         
      }



      private void Colorfilter_Click(object sender, RoutedEventArgs e)
      {
          WriteableBitmap wb = Image1.Source as WriteableBitmap;

          if (wb == null)
          {
              BitmapSource bs = Image1.Source as BitmapSource;
              wb = new WriteableBitmap(bs);
          }
          wb=ColorFilterImage(wb,0,0,1);
          Image1.Source = wb;
      }

      private void Brightness_Click(object sender, RoutedEventArgs e)
      {
          WriteableBitmap wb = Image1.Source as WriteableBitmap;

          if (wb == null)
          {
              BitmapSource bs = Image1.Source as BitmapSource;
              wb = new WriteableBitmap(bs);
          }
          wb = BrightnessImage(wb,128);
          Image1.Source = wb;
      }

      private void Tint_Click(object sender, RoutedEventArgs e)
      {
          WriteableBitmap wb = Image1.Source as WriteableBitmap;

          if (wb == null)
          {
              BitmapSource bs = Image1.Source as BitmapSource;
              wb = new WriteableBitmap(bs);
          }
          wb = TintImage(wb);
          Image1.Source = wb;
      }

      private void Image1_Loaded(object sender, RoutedEventArgs e)
      {

      }
   }
}
