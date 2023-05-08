using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

using Org.BouncyCastle.Crypto.Digests;


namespace Recaptcha
{

    public class CustomCaptcha
    {
        public string CaptchaImageFilePath { get; private set; }
        public string AnswerHash { get; private set; }

        public CustomCaptcha()
        {
            GenerateCaptcha();
        }

        private void GenerateCaptcha()
        {
            int width = 200;
            int height = 100;

            var random = new Random();
            var font = new Font(FontFamily.GenericSansSerif, 40, FontStyle.Bold, GraphicsUnit.Pixel);
            var text = GenerateRandomText(6);

            using (var bmp = new Bitmap(width, height))
            using (var gfx = Graphics.FromImage(bmp))
            {
                gfx.Clear(Color.White);

                // Draw text
                var textSize = gfx.MeasureString(text, font);
                var x = (width - textSize.Width) / 2;
                var y = (height - textSize.Height) / 2;
                gfx.DrawString(text, font, Brushes.Black, x, y);

                // Add noise
                for (int i = 0; i < 50; i++)
                {
                    var pen = new Pen(Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)));
                    gfx.DrawLine(pen, random.Next(width), random.Next(height), random.Next(width), random.Next(height));
                }

                // Save the image to a file
                CaptchaImageFilePath = "captcha.png";
                bmp.Save(CaptchaImageFilePath, ImageFormat.Png);
            }

            // Calculate and store the answer hash
            AnswerHash = CalculateSha256(text);
        }

        private string GenerateRandomText(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }

        public string CalculateSha256(string input)
        {
            var digest = new Sha256Digest();
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            digest.BlockUpdate(bytes, 0, bytes.Length);
            var result = new byte[digest.GetDigestSize()];
            digest.DoFinal(result, 0);
            return BitConverter.ToString(result).Replace("-", "").ToLowerInvariant();
        }
    }

}
