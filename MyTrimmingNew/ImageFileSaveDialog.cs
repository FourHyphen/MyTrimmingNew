using Microsoft.Win32;

namespace MyTrimmingNew
{
    class ImageFileSaveDialog
    {
        private SaveFileDialog _sfd;

        public ImageFileSaveDialog(string saveImageNameExample, string saveImageDirPath)
        {
            _sfd = new SaveFileDialog();
            Init(saveImageNameExample, saveImageDirPath);
        }

        /// <summary>
        /// 初期値設定
        /// </summary>
        private void Init(string saveImageNameExample, string saveImageDirPath)
        {
            _sfd.FileName = saveImageNameExample;
            _sfd.InitialDirectory = saveImageDirPath;
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
