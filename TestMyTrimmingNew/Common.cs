using System;
using System.Drawing;
using MyTrimmingNew;
using static MyTrimmingNew.AuxiliaryLine.AuxiliaryLineParameter;

namespace TestMyTrimmingNew
{
    class Common
    {
        // TODO: リソース管理すればDeploymentItemと共有できるか？
        public const string TestResourceImage001Path = @"test001.jpg";
        public const string TestResourceImage002Path = @"test002.jpg";

        // ファイルアクセスの競合回避のため、保存先をテスト毎に独立にする
        public const string TestToSaveTrimImagePath001 = @"test001_trimmed001.jpg";
        public const string TestToSaveTrimImagePath002 = @"test001_trimmed002.jpg";
        public const string TestToSaveTrimImagePath003 = @"test001_trimmed003.jpg";
        public const string TestToSaveTrimImagePath004 = @"test001_trimmed004.jpg";

        // TODO: リソース管理してxaml側と数字を共有する
        public const int WindowInitWidth = 800;
        public const int WindowInitHeight = 600;

        public const int AuxiliaryLineThickness = 1;

        public static ImageController GetDisplayImage(string filePath)
        {
            return new ImageController(filePath,
                                       WindowInitWidth - Constant.FixCanvasWidth,
                                       WindowInitHeight - Constant.FixCanvasHeight);
        }

        public static AuxiliaryController GetAuxiliaryControllerImage001RatioTypeW16H9()
        {
            return GetAuxiliaryController(TestResourceImage001Path, RatioType.W16H9);
        }

        public static AuxiliaryController GetAuxiliaryController(string filePath, RatioType ratioType)
        {
            ImageController ic = GetDisplayImage(filePath);
            return new AuxiliaryController(ic, ratioType, AuxiliaryLineThickness);
        }

        public static int GetAuxiliaryWidth(AuxiliaryController ac)
        {
            return ac.AuxiliaryRight - ac.AuxiliaryLeft - ac.AuxiliaryLineThickness + 1;
        }

        public static int GetAuxiliaryHeight(AuxiliaryController ac)
        {
            return ac.AuxiliaryBottom - ac.AuxiliaryTop - ac.AuxiliaryLineThickness + 1;
        }

        public static Point CalcRotatePoint(Point p, int degree)
        {
            double rad = ToRadian(degree);
            double rotateX = p.X * Math.Cos(rad) - p.Y * Math.Sin(rad);
            double rotateY = p.Y * Math.Cos(rad) + p.X * Math.Sin(rad);
            return new Point((int)rotateX, (int)rotateY);
        }

        private static double ToRadian(int degree)
        {
            return (double)degree * Math.PI / 180.0;
        }
    }
}
