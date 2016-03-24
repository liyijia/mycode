using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    /// <summary>
    /// 文字样式
    /// </summary>
    public class textStyle
    {
        private string _color = "各异";
        private string _decoration = "none";
        private string _align = "各异";
        private string _baseline = "各异";
        private string _fontFamily = "Arial, Verdana, sans-serif";
        private int _fontSize = 12;
        private string _fontStyle = "normal";
        private string _fontWeight = "normal";

        /// <summary>
        /// 颜色
        /// </summary>
        public string color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// 修饰，仅对tooltip.textStyle生效 
        /// </summary>
        public string decoration
        {
            get { return _decoration; }
            set { _decoration = value; }
        }

        /// <summary>
        /// 水平对齐方式，可选为：'left' | 'right' | 'center' 
        /// </summary>
        public string align
        {
            get { return _align; }
            set { _align = value; }
        }
        
        /// <summary>
        /// 垂直对齐方式，可选为：'top' | 'bottom' | 'middle' 
        /// </summary>
        public string baseline
        {
            get { return _baseline; }
            set { _baseline = value; }
        }

        /// <summary>
        /// 字体系列
        /// </summary>
        public string fontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; }
        }

        /// <summary>
        /// 字号 ，单位px
        /// </summary>
        public int fontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        /// <summary>
        /// 样式，可选为：'normal' | 'italic' | 'oblique' 
        /// </summary>
        public string fontStyle
        {
            get { return _fontStyle; }
            set { _fontStyle = value; }
        }

        /// <summary>
        /// 粗细，可选为：'normal' | 'bold' | 'bolder' | 'lighter' | 100 | 200 |... | 900
        /// </summary>
        public string fontWeight
        {
            get { return _fontWeight; }
            set { _fontWeight = value; }
        }

    }
}
