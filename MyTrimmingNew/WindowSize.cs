namespace MyTrimmingNew
{
    class WindowSize
    {
        public int WindowWidth { get; set; }

        public int WindowHeight { get; set; }

        public int ImageFitWindowWidth { get; set; }

        public int ImageFitWindowHeight { get; set; }

        public double ImageFitWindowRatio { get; private set; }

        public WindowSize(int windowWidth, int windowHeight, int imageWidth, int imageHeight)
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            CalcImageSizeWindowFit(imageWidth, imageHeight);
        }

        /// <summary>
        /// windowに画像全部が表示されるよう、画像の縦横比率を維持しつつ縮小するサイズを求める
        /// ex) windowサイズ横1000 * 縦800に横2160 * 縦3000の画像を表示する場合、
        ///     ・横縮小率: 1000 / 2160 = 0.46 | 画像サイズ 横1000 * 縦1388
        ///     ・縦縮小率:  800 / 3000 = 0.27 | 画像サイズ 横 576 * 縦 800
        ///     となり、横576 * 縦800に縮小すれば画像全部が表示される
        /// </summary>
        private void CalcImageSizeWindowFit(int imageWidth, int imageHeight)
        {
            double width = (double)imageWidth;
            double height = (double)imageHeight;
            double widthReductionRatio = (double)WindowWidth / width;
            double heightReductionRatio = (double)WindowHeight / height;

            if (widthReductionRatio > heightReductionRatio)
            {
                ImageFitWindowRatio = heightReductionRatio;
                ImageFitWindowWidth = (int)((double)imageWidth * ImageFitWindowRatio);
                ImageFitWindowHeight = WindowHeight;
            }
            else
            {
                ImageFitWindowRatio = widthReductionRatio;
                ImageFitWindowWidth = WindowWidth;
                ImageFitWindowHeight = (int)((double)imageHeight * ImageFitWindowRatio);
            }
        }
    }
}
