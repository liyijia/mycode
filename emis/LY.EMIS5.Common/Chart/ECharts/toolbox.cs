using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    /// <summary>
    ///  工具箱，每个图表最多仅有一个工具箱。
    /// </summary>
    public class toolbox
    {
        private bool _show = false;
        private string _orient = "horizontal";
        private string _x = "center";
        private string _y = "top";
        private string _backgroundColor = "rgba(0,0,0,0)";
        private string _borderColor = "#ccc";
        private int _borderWidth = 0;
        private int _padding = 5;
        private int _itemGap = 10;
        private int _itemSize = 16;
        private ICollection<string> _color = new List<string>() { "#1e90ff", "#22bb22", "#4b0082", "#d2691e" };
        private feature _feature = new feature();

        /// <summary>
        /// 显示策略，可选为：true（显示） | false（隐藏） 
        /// </summary>
        public bool show
        {
            get { return _show; }
            set { _show = value; }
        }

        /// <summary>
        /// 布局方式，默认为水平布局，可选为：'horizontal' | 'vertical' 
        /// </summary>
        public string orient
        {
            get { return _orient; }
            set { _orient = value; }
        }

        /// <summary>
        /// 水平安放位置，默认为全图居中，可选为：'center' | 'left' | 'right' | {number}（x坐标，单位px） 
        /// </summary>
        public string x
        {
            get { return _x; }
            set { _x = value; }
        }
        
        /// <summary>
        /// 垂直安放位置，默认为全图顶端，可选为：'top' | 'bottom' | 'center' | {number}（y坐标，单位px） 
        /// </summary>
        public string y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// 工具箱背景颜色，默认透明 
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
        /// 工具箱icon大小，单位（px）
        /// </summary>
        public int itemSize
        {
            get { return _itemSize; }
            set { _itemSize = value; }
        }

        /// <summary>
        /// 工具箱icon颜色序列，循环使用
        /// </summary>
        public ICollection<string> color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// 启用功能
        /// </summary>
        public feature feature
        {
            get { return _feature; }
            set { _feature = value; }
        }

    }

    /// <summary>
    /// 工具箱功能
    /// </summary>
    public class feature
    {
        private bool _mark = true;
        private bool _dataView = true;
        private ICollection<string> _magicType = new List<string>() { "line", "bar" };
        private bool _saveAsImage = true;
        private bool _restore = true;
        /// <summary>
        /// 辅助线标志
        /// </summary>
        public bool mark
        {
            get { return _mark; }
            set { _mark = value; }
        }

        /// <summary>
        /// 数据视图
        /// </summary>
        public bool dataView
        {
            get { return _dataView; }
            set { _dataView = value; }
        }

        /// <summary>
        /// 图表类型切换，当前仅支持直角系下的折线图、柱状图转换 
        /// </summary>
        public ICollection<string> magicType
        {
            get { return _magicType; }
            set { _magicType = value; }
        }

        /// <summary>
        /// 保存图片（IE8-不支持） 
        /// </summary>
        public bool saveAsImage
        {
            get { return _saveAsImage; }
            set { _saveAsImage = value; }
        }

        /// <summary>
        /// 还原，复位原始图表
        /// </summary>
        public bool restore
        {
            get { return _restore; }
            set { _restore = value; }
        }

    }
}
