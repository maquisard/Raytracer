using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.tamu.courses.imagesynth.core.imaging
{
   public class ImageData
    {
        private Color[] data = new Color[0];

        public int Width { get; private set; }
        public int Height { get; private set; }

        public ImageData(ImageData data)
        {
            if (data == null) throw new ArgumentNullException("The data must not be null");
            this.Width = data.Width;
            this.Height = data.Height;

            this.data = new Color[this.Width * this.Height];
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    this.SetPixel(x, y, data.GetPixel(x, y));
                }
            }
        }

        public ImageData(int width, int height)
            : this(width, height, Color.BLACK)
        {
        }
        public ImageData(int width, int height, Color color)
        {
            this.Width = width;
            this.Height = height;
            this.data = new Color[this.Width * this.Height];
            this.FillImageData(color);
        }
        public Color GetPixel(int x, int y)
        {
            return this.data[y * this.Width + x];
        }
        public void SetPixel(int x, int y, Color color)
        {
            this.data[y * this.Width + x] = color;
        }

        public System.Drawing.Bitmap ToBitmap()
        {
            System.Drawing.Bitmap temp = new System.Drawing.Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            UnmanagedImage unmanage = UnmanagedImage.FromManagedImage(temp);
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    unmanage.SetPixel(x, y, (System.Drawing.Color)this.GetPixel(x, y));
                }
            }
            //temp.Dispose();
            return unmanage.ToManagedImage();
        }
        public ImageData GetSubImage(int x, int y, int width, int height)
        {
            ImageData subimage = new ImageData(width, height);
            for (int j = y; j < y + height; j++)
            {
                for (int i = x; i < x + width; i++)
                {
                    subimage.SetPixel(i - x, j - y, this.GetPixel(i, j));
                }
            }
            return subimage;
        }

        public ImageData GetSubImage(int x, int y, int size)
        {
            return this.GetSubImage(x, y, size, size);
        }

        public ImageData GetSubImage(System.Drawing.Point p, int size)
        {
            return this.GetSubImage(p.X, p.Y, size);
        }

        public void SetSubImage(int x, int y, ImageData subImage)
        {
            int p = x * subImage.Width;
            int q = y * subImage.Height;

            for (int j = q; j < q + subImage.Height; j++)
            {
                for (int i = p; i < p + subImage.Width; i++)
                {
                    this.SetPixel(i, j, subImage.GetPixel(i - p, j - q));
                }
            }
        }

        public void SetSubImage(System.Drawing.Point p, ImageData subImage)
        {
            this.SetSubImage(p.X, p.Y, subImage);
        }

        public void SaveToFile(string filename)
        {
            this.ToBitmap().Save(filename, System.Drawing.Imaging.ImageFormat.Png);
        }

        private void FillImageData(Color color)
        {
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    this.SetPixel(x, y, color);
                }
            }
        }
    }
}
