using Microsoft.Win32;

namespace MyTrimmingNew
{
    class ImageFileSaveDialog
    {
        private SaveFileDialog _sfd;
        private static ImageFileSaveDialog _self = null;
        private static string _saveImageNameExample;
        private static string _saveImageDirPath;

        private ImageFileSaveDialog()
        {
            _sfd = new SaveFileDialog();
            Init();
        }

        /// <summary>
        /// シングルトン: 自身のインスタンスを返す
        /// </summary>
        /// <returns></returns>
        public static ImageFileSaveDialog GetInstance(string saveImageNameExample, 
                                                      string saveImageDirPath)
        {
            _saveImageNameExample = saveImageNameExample;
            _saveImageDirPath = saveImageDirPath;

            if (_self == null)
            {
                _self = new ImageFileSaveDialog();
            }

            return _self;
        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        /// <param name="sfd"></param>
        private void Init()
        {
            _sfd.FileName = _saveImageNameExample;
            _sfd.InitialDirectory = _saveImageDirPath;
            _sfd.Title = "保存先を指定してください";
            _sfd.RestoreDirectory = true;
            _sfd.CheckFileExists = false;
            _sfd.CheckPathExists = false;
        }

        /// <summary>
        /// FileSaveDialogを表示する
        /// </summary>
        /// <returns>
        /// string filePath -> ユーザーが選択した画像ファイルのパス
        /// string ""       -> ユーザーがキャンセルした場合、空の文字列を返す
        /// </returns>
        public string Show()
        {
            string filePath = "";
            if (_sfd.ShowDialog() == true)
            {
                filePath = _sfd.FileName;
            }

            return filePath;
        }
    }
}
