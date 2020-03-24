using System.Windows.Input;

namespace MyTrimmingNew
{
    public class Keys
    {
        /// <summary>
        /// アプリケーションで有効なキー
        /// </summary>
        public enum EnableKeys
        {
            Up,
            Down,
            Left,
            Right,
            RotatePlus,
            RotateMinus,
            Cancel,
            Redo,
            FileOpen,
            FileSave,
            Else
        }

        /// <summary>
        /// キー入力内容をキーイベントから抜き出す
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static EnableKeys ToEnableKeys(System.Windows.Input.Key key, System.Windows.Input.KeyboardDevice keyboard)
        {
            EnableKeys keyConbination = ToEnableKeysConbination(key, keyboard);
            if (keyConbination != EnableKeys.Else)
            {
                return keyConbination;
            }

            return ToEnableKeysOneKey(key);
        }

        private static EnableKeys ToEnableKeysConbination(System.Windows.Input.Key key, System.Windows.Input.KeyboardDevice keyboard)
        {
            // Ctrl + 何か
            if (keyboard.Modifiers == ModifierKeys.Control)
            {
                if (key == Key.Z)
                {
                    return EnableKeys.Cancel;
                }
                else if (key == Key.Y)
                {
                    return EnableKeys.Redo;
                }
                else if (key == Key.OemMinus)
                {
                    return EnableKeys.RotateMinus;
                }
                else if (key == Key.O)
                {
                    return EnableKeys.FileOpen;
                }
                else if (key == Key.S)
                {
                    return EnableKeys.FileSave;
                }
            }

            // Ctrl + Shift + 何か
            if (keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
            {
                if (key == Key.OemPlus)
                {
                    return EnableKeys.RotatePlus;
                }
            }

            return EnableKeys.Else;
        }

        private static EnableKeys ToEnableKeysOneKey(System.Windows.Input.Key key)
        {
            if (key == Key.Up)
            {
                return EnableKeys.Up;
            }
            else if (key == Key.Down)
            {
                return EnableKeys.Down;
            }
            else if (key == Key.Left)
            {
                return EnableKeys.Left;
            }
            else if (key == Key.Right)
            {
                return EnableKeys.Right;
            }

            return EnableKeys.Else;
        }

        public static bool IsKeyCursor(EnableKeys key)
        {
            if (key == EnableKeys.Up || 
                key == EnableKeys.Right ||
                key == EnableKeys.Down ||
                key == EnableKeys.Left)
            {
                return true;
            }

            return false;
        }
    }
}
