using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    /// <summary>
    /// 线条（线段）样式
    /// </summary>
    public class lineStyle
    {
        private string _color = "各异";
        private string _type = "solid";
        private int _width = 3;
        private string _shadowColor = "rgba(0,0,0,0)";
        private int _shadowBlur = 5;
        private int _shadowOffsetX = 3;
        private int _shadowOffsetY = 3;

        /// <summary>
        /// 颜色
        /// </summary>
        public string color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// 线条样式，可选为：'solid' | 'dotted' | 'dashed' 
        /// </summary>
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 线宽
        /// </summary>
        public int width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// 折线主线(IE8+)有效，阴影色彩，支持rgba 
        /// </summary>
        public string shadowColor
        {
            get { return _shadowColor; }
            set { _shadowColor = value; }
        }

        /// <summary>
        /// 折线主线(IE8+)有效，阴影模糊度，大于0有效 
        /// </summary>
        public int shadowBlur
        {
            get { return _shadowBlur; }
            set { _shadowBlur = value; }
        }

        /// <summary>
        /// 折线主线(IE8+)有效，阴影横向偏移，正值往右，负值往左
        /// </summary>
        public int shadowOffsetX
        {
            get { return _shadowOffsetX; }
            set { _shadowOffsetX = value; }
        }

        /// <summary>
        /// 折线主线(IE8+)有效，阴影纵向偏移，正值往下，负值往上
        /// </summary>
        public int shadowOffsetY
        {
            get { return _shadowOffsetY; }
            set { _shadowOffsetY = value; }
        }

    }
}
