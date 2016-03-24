using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    /// <summary>
    /// 标题
    /// </summary>
    public class title
    {
        private string _text = string.Empty;
        private string _subtext = string.Empty;
        private string _x = "left";
        private string _y = "top";
        private string _textAlign = null;
        private string _backgroundColor = "rgba(0,0,0,0)";
        private string _borderColor = "#ccc";
        private int _borderWidth = 0;
        private int _padding = 5;
        private int _itemGap = 10;
        private textStyle _textStyle = new textStyle() { fontSize = 18, fontWeight = "bolder", color = "#333" };
        private textStyle _subtextStyle = new textStyle() { color = "#aaa" };

        /// <summary>
        /// 主标题文本
        /// </summary>
        public string text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// 副标题文本
        /// </summary>
        public string subtext
        {
            get { return _subtext; }
            set { _subtext = value; }
        }

        /// <summary>
        /// 水平安放位置，默认为左侧，可选为：'center' | 'left' | 'right' | {number}（x坐标，单位px） 
        /// </summary>
        public string x
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        ///  垂直安放位置，默认为全图顶端，可选为：'top' | 'bottom' | 'center' | {number}（y坐标，单位px） 
        /// </summary>
        public string y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        ///  水平对齐方式，默认根据x设置自动调整，可选为： left' | 'right' | 'center'
        /// </summary>
        public string textAlign
        {
            get { return _textAlign; }
            set { _textAlign = value; }
        }

        /// <summary>
        /// 标题背景颜色，默认透明 
        /// </summary>
        public string backgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        /// <summary>
        /// 标题边框颜色
        /// </summary>
        public string borderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        /// <summary>
        /// 标题边框线宽，单位px，默认为0（无边框） 
        /// </summary>
        public int borderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }

        /// <summary>
        /// 标题内边距，单位px，默认各方向内边距为5，接受数组分别设定上右下左边距，同css
        /// </summary>
        public int padding
        {
            get { return _padding; }
            set { _padding = value; }
        }
        
        /// <summary>
        /// 主副标题纵向间隔，单位px，默认为10
        /// </summary>
        public int itemGap
        {
            get { return _itemGap; }
            set { _itemGap = value; }
        }

        /// <summary>
        /// 主标题文本样式
        /// </summary>
        public textStyle textStyle
        {
            get { return _textStyle; }
            set { _textStyle = value; }
        }

        /// <summary>
        /// 副标题文本样式
        /// </summary>
        public textStyle subtextStyle
        {
            get { return _subtextStyle; }
            set { _subtextStyle = value; }
        }
    }
}
