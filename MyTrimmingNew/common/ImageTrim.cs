using System;
using System.Drawing;

namespace MyTrimmingNew.common
{
    class ImageTrim
    {
        /// <summary>
        /// 画像をtrimして保存(回転非対応)
        /// </summary>
        /// <param name="image"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public System.Drawing.Bitmap Execute(Bitmap image,
                                             int left,
                                             int top,
                                             int width,
                                             int height)
        {
            return ExecuteCore(image, left, top, width, height);
        }

        private static System.Drawing.Bitmap ExecuteCore(Bitmap image,
                                                         int originX,
                                                         int originY,
                                                         int trimWidth,
                                                         int trimHeight)
        {
            Bitmap trimImage = new Bitmap(trimWidth, trimHeight);
            Graphics g = Graphics.FromImage(trimImage);
            Rectangle trim = new Rectangle(originX, originY, trimWidth, trimHeight);
            Rectangle draw = new Rectangle(0, 0, trimWidth, trimHeight);
            g.DrawImage(image, draw, trim, GraphicsUnit.Pixel);
            g.Dispose();

            return trimImage;
        }

        /// <summary>
        /// 画像のtrimおよび保存(回転対応)
        /// 条件：4点座標は非負であること
        /// </summary>
        /// <param name="image"></param>
        /// <param name="leftTop"></param>
        /// <param name="leftBottom"></param>
        /// <param name="rightTop"></param>
        /// <param name="rightBottom"></param>
        /// <param name="degree"></param>
        public System.Drawing.Bitmap Execute(Bitmap image,
                                             Point leftTop,
                                             Point leftBottom,
                                             Point rightTop,
                                             Point rightBottom,
                                             int degree)
        {
            // 回転パラメーター準備
            int centerX = Common.CalcCenterX(leftTop, rightBottom);
            int centerY = Common.CalcCenterY(leftBottom, rightTop);
            double radian = (double)degree * Math.PI / 180;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);

            // 切り抜き画像作成
            Bitmap trimImage = new Bitmap(image.Width, image.Height);
            TrimRect trimRect = new TrimRect(leftTop, rightTop, rightBottom, leftBottom);
            int minX = image.Width;
            int minY = image.Height;
            int maxX = 0;
            int maxY = 0;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int rotateX = CalcRotateX(x, y, centerX, centerY, cos, sin);
                    int rotateY = CalcRotateY(x, y, centerX, centerY, cos, sin);
                    if (trimRect.IsInside(rotateX, rotateY))
                    {
                        Color c = image.GetPixel(rotateX, rotateY);
                        trimImage.SetPixel(x, y, Color.FromArgb(c.R, c.G, c.B));
                        if (x < minX)
                        {
                            minX = x;
                        }
                        if (x > maxX)
                        {
                            maxX = x;
                        }
                        if (y < minY)
                        {
                            minY = y;
                        }
                        if (y > maxY)
                        {
                            maxY = y;
                        }
                    }
                }
            }

            // 予備実験の結果、端1pixel分は計算誤差として無視した方が良いと判断した
            maxX -= 1;
            maxY -= 1;
            minX += 1;
            minY += 1;
            int width = maxX - minX;
            int height = maxY - minY;

            return ExecuteCore(trimImage, minX, minY,width, height);
        }

        private int CalcRotateX(int x, int y, int centerX, int centerY, double cos, double sin)
        {
            int tmpX = x - centerX;
            int tmpY = y - centerY;
            int rotateX = (int)(tmpX * cos - tmpY * sin);
            rotateX += centerX;

            return rotateX;
        }

        private int CalcRotateY(int x, int y, int centerX, int centerY, double cos, double sin)
        {
            int tmpX = x - centerX;
            int tmpY = y - centerY;
            int rotateY = (int)(tmpX * sin + tmpY * cos);
            rotateY += centerY;

            return rotateY;
        }
    }
}
