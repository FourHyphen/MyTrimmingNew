using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew
{
    class ImageSaveDirector
    {
        private ImageFileSaveDialog _ifsd;

        public ImageSaveDirector() { }

        /// <summary>
        /// 画像を保存する一連の処理を実行する
        /// </summary>
        /// <returns>
        /// True: 画像を保存した
        /// False: 画像を保存しなかった(ユーザーがキャンセルした)
        /// </returns>
        /// <exception>
        /// 画像保存エラー
        /// </exception>
        public bool ImageSave(DisplayImage image,
                              IAuxiliaryLine auxiliaryLine,
                              System.Windows.Controls.Primitives.StatusBarItem SaveResultField)
        {
            _ifsd = ImageFileSaveDialog.GetInstance(image.SaveNameExample, image.DirPath);
            string filePath = _ifsd.Show();

            if (filePath == "")
            {
                return false;
            }

            // TODO: 保存結果の表示後、何かユーザー操作があったタイミングで結果表示しない方が良さそう
            //       (次の保存試行の結果と混ざるというか誤解させそう)
            try
            {
                image.Save(filePath, auxiliaryLine);
                SaveResultField.Content = "保存に成功しました";
            }
            catch (Exception ex){
                SaveResultField.Content = "保存に失敗しました";
            }

            return true;
        }
    }
}
