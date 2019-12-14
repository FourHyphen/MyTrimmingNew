using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MyTrimmingNew
{
    class ImageFileOpenDialog
    {
        private static ImageFileOpenDialog _self = null;
        private OpenFileDialog _ofd;

        private ImageFileOpenDialog()
        {
            _ofd = new OpenFileDialog();
            Init();
        }

        /// <summary>
        /// シングルトン: 自身のインスタンスを返す
        /// </summary>
        /// <returns></returns>
        public static ImageFileOpenDialog GetInstance()
        {
            if(_self == null)
            {
                _self = new ImageFileOpenDialog();
            }
            return _self;
        }

        /// <summary>
        /// ファイルオープンダイアログの初期値を設定する
        /// </summary>
        private void Init()
        {
            // TODO: 表示文字列は全て外部管理(setting.ini？)したい
            // TODO: これ消せるか確認したら消す: _ofd.InitialDirectory = _nowInitialDir;
            _ofd.FileName = "";
            _ofd.Filter = "Imageファイル(*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png|すべてのファイル(*.*)|*.*";
            _ofd.FilterIndex = 1;
            _ofd.Title = "開く画像ファイルを選択してください";
            _ofd.RestoreDirectory = true;
            _ofd.CheckFileExists = true;
            _ofd.CheckPathExists = true;
        }

        /// <summary>
        /// FileOpenDialogを表示する
        /// </summary>
        /// <returns>
        /// string filePath -> ユーザーが選択した画像ファイルのパス
        /// string ""       -> ユーザーがキャンセルした場合、空の文字列を返す
        /// </returns>
        public string Show()
        {
            string filePath = "";
            if (_ofd.ShowDialog() == true)
            {
                filePath = _ofd.FileName;
            }

            return filePath;
        }
    }
}
