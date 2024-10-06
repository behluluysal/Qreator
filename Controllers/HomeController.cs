using Microsoft.AspNetCore.Mvc;
using QrCodeGenerator.Data;
using QrCodeGenerator.Models;
using ZXing;
using ZXing.QrCode;
using SkiaSharp;
using System.Runtime.InteropServices;

namespace QrCodeGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/qrcode")]
        public IActionResult GenerateQrCode(Qr model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Lütfen gerekli alanları doldurun.");
            }

            string qrData = "";

            switch (model.DataType)
            {
                case "Url":
                    if (string.IsNullOrEmpty(model.Url))
                    {
                        return BadRequest("URL alanı boş olamaz.");
                    }
                    qrData = model.Url;
                    break;
                
                case "Email":
                    if (string.IsNullOrEmpty(model.Email))
                    {
                        return BadRequest("E-posta alanı boş olamaz.");
                    }
                    qrData = $"mailto:{model.Email}";
                    break;
                
                case "PhoneNumber":
                    if (string.IsNullOrEmpty(model.PhoneNumber))
                    {
                        return BadRequest("Telefon numarası alanı boş olamaz.");
                    }
                    qrData = $"tel:{model.PhoneNumber}";
                    break;
                
                default:
                    return BadRequest("Geçersiz veri tipi.");
            }

            string qrCodeImagePath = GenerateQrCodeForUrl(qrData);

            model.ImagePath = qrCodeImagePath.Replace("wwwroot", "");

            _context.Qrs.Add(model);
            _context.SaveChanges();

            ViewBag.Qr = model.ImagePath;

            return View("QrResult", model);
        }

        private string GenerateQrCodeForUrl(string url)
        {
            string filePath = Path.Combine("wwwroot", "qrcodes", $"{Guid.NewGuid()}.png");

            var qrWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 250,
                    Width = 250,
                    Margin = 1
                }
            };

            var pixelData = qrWriter.Write(url);

            using (var surface = SKSurface.Create(new SKImageInfo(pixelData.Width, pixelData.Height)))
            {
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                var handle = GCHandle.Alloc(pixelData.Pixels, GCHandleType.Pinned);
                try
                {
                    IntPtr ptr = handle.AddrOfPinnedObject();

                    using (var bitmap = new SKBitmap(new SKImageInfo(pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul)))
                    {
                        bitmap.InstallPixels(bitmap.Info, ptr, bitmap.RowBytes);
                        canvas.DrawBitmap(bitmap, 0, 0);
                    }
                }
                finally
                {
                    handle.Free();
                }

                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = System.IO.File.OpenWrite(filePath))
                {
                    data.SaveTo(stream);
                }
            }

            return filePath;
        }
    }
}
