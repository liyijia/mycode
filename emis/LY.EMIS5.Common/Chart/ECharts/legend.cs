using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    /// <summary>
    /// 图例，每个图表最多仅有一个图例。
    /// </summary>
    public class legend
    {
        private string _orient = "horizontal";
        private string _x = "left";
        private string _y = "top";
        private string _backgroundColor = "rgba(0,0,0,0)";
        private string _borderColor = "#ccc";
        private int _borderWidth = 0;
        private int _padding = 5;
        private int _itemGap = 10;
        private textStyle _textStyle = new textStyle() { color = "#333" };
        private ICollection<string> _data = new List<string>();

        /// <summary>
        /// 布局方式，默认为水平布局，可选为：'horizontal' | 'vertical' 
        /// </summary>
        public string orient
        {
            get { return _orient; }
            set { _orient = value; }
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
        /// 图例内容数组，数组项为{string}，每一项代表一个系列的name。
        /// 使用根据该值索引series中同名系列所用的图表类型和itemStyle，如果索引不到，该item将默认为没启用状态。
        /// </summary>
        public ICollection<string> data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
