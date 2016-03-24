using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    /// <summary>
    /// 区域填充样式
    /// </summary>
    public class areaStyle
    {
        private string _color = "各异";
        private string _type = "default";

        /// <summary>
        /// 颜色
        /// </summary>
        public string color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// 填充样式，目前仅支持'default'(实填充)
        /// </summary>
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
