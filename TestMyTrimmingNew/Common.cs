using MyTrimmingNew;
namespace TestMyTrimmingNew
{
    class Common
    {
        // TODO: リソース管理すればDeploymentItemと共有できるか？
        public const string TestResourceImage001Path = @".\Resource\test001.jpg";
        public const string TestResourceImage002Path = @".\Resource\test002.jpg";
        public const string TestToSaveTrimImagePath = @".\Resource\test001_trimmed.jpg";

        // TODO: リソース管理してxaml側と数字を共有する
        public const int WindowInitWidth = 800;
        public const int WindowInitHeight = 600;

        public const int AuxiliaryLineThickness = 1;

        public static ImageController GetDisplayImage(string filePath)
        {
            return new ImageController(filePath, WindowInitWidth, WindowInitHeight);
        }

        public static AuxiliaryController GetAuxiliaryController(string filePath, int widthRatio, int heightRatio)
        {
            ImageController ic = new ImageController(filePath,
                                                     WindowInitWidth,
                                                     WindowInitHeight);
            return new AuxiliaryController(ic, widthRatio, heightRatio, AuxiliaryLineThickness);
        }

    }
}
