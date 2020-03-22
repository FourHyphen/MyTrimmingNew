using System;
using System.Windows.Forms;

namespace MyTrimmingNew
{
    // TODO: メッセージ等文字列の外部管理
    class ImageProcessResultMessageBox
    {
        public enum Result
        {
            SuccessImageOpen,
            FailureImageOpen,
            SuccessImageSave,
            FailureImageSave
        }

        // 多態する規模ではないと判断、用途や処理が増える場合は多態を検討してください
        public static void Show(Result result)
        {
            if (result == Result.SuccessImageOpen)
            {
                // 画像オープンに成功すればウィンドウに画像が表示されるので、特別通知する必要なし
            }
            else if(result == Result.FailureImageOpen)
            {
                ShowFailureImageOpen();
            }
            else if(result == Result.SuccessImageSave)
            {
                ShowSuccessImageSave();
            }
            else if(result == Result.FailureImageSave)
            {
                ShowFailureImageSave();
            }
        }

        private static void ShowFailureImageOpen()
        {
            String message = "画像のオープンに失敗しました";
            String title = "Notice";
            Show(message, title);
        }

        private static void ShowSuccessImageSave()
        {
            String message = "画像の保存に成功しました";
            String title = "Message";
            Show(message, title);
        }

        private static void ShowFailureImageSave()
        {
            String message = "画像の保存に失敗しました";
            String title = "Notice";
            Show(message, title);
        }

        private static void Show(String message, String title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
