using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew
{
    class Constant
    {
        // TODO: 各種パラメーターを画面の要素から取得できないか？
        private static int _menuItemHeight = 20;
        private static int _titleHeight = 18;
        private static int _statusBarHeight = 18;
        private static int _frameUpper = 4;
        private static int _frameRight = 4;
        private static int _frameLower = 4;
        private static int _frameLeft = 4;

        /// <summary>
        /// 画像を表示する領域サイズの横幅を正確に求めるための固定値
        /// </summary>
        public static readonly int FixCanvasWidth = _frameRight + _frameLeft;

        /// <summary>
        /// 画像を表示する領域サイズの縦幅を正確に求めるための固定値
        /// </summary>
        public static readonly int FixCanvasHeight = _frameUpper + _menuItemHeight + _titleHeight + _statusBarHeight + _frameLower;

        /// <summary>
        /// MouseDown時、正確な箇所を狙わずともある程度の範囲に収まっていれば良しとするマージン
        /// </summary>
        public static readonly int MouseDownPointMargin = 20;
    }
}
