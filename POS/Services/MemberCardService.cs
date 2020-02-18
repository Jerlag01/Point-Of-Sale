using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using NLog;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Dal.Model;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace Pos.Services
{
    public class MembercardService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public string FilePath { get; private set; }

        public string PrinterName { get; set; }

        public string GenerateMembercard(MemberCard card, string memberYear, string memberActive)
        {
            if (card == null || card.Member == null)
                return string.Empty;
            string filename = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\Templates\\Pdf\\Template_Membercard.pdf";
            this.FilePath = Path.GetTempPath() + (object)Guid.NewGuid() + ".pdf";
            try
            {
                using (FileStream fileStream = new FileStream(this.FilePath, FileMode.Create))
                {
                    PdfReader reader = new PdfReader(filename);
                    PdfStamper pdfStamper = new PdfStamper(reader, (Stream)fileStream);
                    pdfStamper.Writer.CloseStream = false;
                    AcroFields acroFields = pdfStamper.AcroFields;
                    acroFields.SetField("txtName", card.Member.Fullname);
                    acroFields.SetField("txtYear", memberYear);
                    acroFields.SetField("txtMemberType", memberActive);
                    iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance(this.GenerateBarcode(card.Id), new BaseColor((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue));
                    PdfContentByte overContent = pdfStamper.GetOverContent(1);
                    iTextSharp.text.Rectangle position = acroFields.GetFieldPositions("txtQrCode").First<AcroFields.FieldPosition>().position;
                    instance.ScaleToFit(position.Width, position.Height);
                    instance.SetAbsolutePosition((float)((double)position.Right - (double)instance.ScaledWidth + ((double)position.Width - (double)instance.ScaledWidth) / 2.0), position.Bottom + (float)(((double)position.Height - (double)instance.ScaledHeight) / 2.0));
                    overContent.AddImage(instance);
                    pdfStamper.FormFlattening = true;
                    pdfStamper.Close();
                    reader.Close();
                    MembercardService.logger.Info("Membercard PDF created: " + this.FilePath);
                }
            }
            catch (IOException ex)
            {
                MembercardService.logger.Error("Could not create temporary PDF file:" + ex.Message);
                return string.Empty;
            }
            return this.FilePath;
        }

        public bool PrintMembercard()
        {
            return this.PrintMembercard(this.FilePath);
        }

        public bool PrintMembercard(string filePath)
        {
            try
            {
                string str1 = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\AcroRD32.exe", "", (object)null);
                string str2 = string.Format("{0} {1} {2}", (object)"/s", (object)"/h", (object)string.Format("/t \"{0}\" \"{1}\"", (object)filePath, (object)this.PrinterName));
                Process process = Process.Start(new ProcessStartInfo()
                {
                    FileName = str1,
                    Arguments = str2,
                    CreateNoWindow = true,
                    ErrorDialog = false,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden
                });
                process.WaitForInputIdle();
                Thread.Sleep(5000);
                if (!process.CloseMainWindow())
                    process.Kill();
            }
            catch (Exception ex)
            {
                MembercardService.logger.Error("Could not print membercard:" + ex.Message + (object)ex.InnerException);
                return false;
            }
            MembercardService.logger.Info("Membercard PDF printed.");
            return true;
        }

        private System.Drawing.Image GenerateBarcode(Guid guid)
        {
            QRCodeWriter qrCodeWriter = new QRCodeWriter();
            Dictionary<EncodeHintType, object> dictionary1 = new Dictionary<EncodeHintType, object>()
      {
        {
          EncodeHintType.ERROR_CORRECTION,
          (object) ErrorCorrectionLevel.H
        }
      };
            string contents = guid.ToString();
            Dictionary<EncodeHintType, object> dictionary2 = dictionary1;
            return (System.Drawing.Image)new BarcodeWriter().Write(qrCodeWriter.encode(contents, BarcodeFormat.QR_CODE, 100, 100, (IDictionary<EncodeHintType, object>)dictionary2));
        }

        public System.Drawing.Image ConvertPdfToImage()
        {
            try
            {
                Spire.Pdf.PdfDocument pdfDocument = new Spire.Pdf.PdfDocument();
                pdfDocument.LoadFromFile(this.FilePath);
                System.Drawing.Image image1 = pdfDocument.SaveAsImage(0, PdfImageType.Metafile);
                Size size = image1.Size;
                int width = size.Width * 5;
                size = image1.Size;
                int height = size.Height * 5;
                System.Drawing.Image image2 = (System.Drawing.Image)new Bitmap(width, height);
                using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(image2))
                {
                    graphics.ScaleTransform(5f, 5f);
                    graphics.DrawImage(image1, new System.Drawing.Rectangle(new Point(0, 0), image1.Size), new System.Drawing.Rectangle(new Point(0, 0), image1.Size), GraphicsUnit.Pixel);
                }
                MembercardService.logger.Debug("PDF conversion to image succeeded.");
                return image2;
            }
            catch (Exception ex)
            {
                MembercardService.logger.Error("PDF conversion to image failed:" + ex.Message);
                return (System.Drawing.Image)null;
            }
        }
    }
}
