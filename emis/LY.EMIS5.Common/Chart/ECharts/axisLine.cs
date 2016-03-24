using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    /// <summary>
    /// 坐标轴线
    /// </summary>
    public class axisLine
    {
        private bool _show = true;
        private lineStyle _lineStyle = new lineStyle() { color = "#48b", width = 2, type = "solid" };

        /// <summary>
        /// 是否显示，默认为true，设为false后下面都没意义了
        /// </summary>
        public bool show
        {
            get { return _show; }
            set { _show = value; }
        }

        /// <summary>
        /// 属性lineStyle控制线条样式
        /// </summary>
        public lineStyle lineStyle
        {
            get { return _lineStyle; }
            set { _lineStyle = value; }
        }
    }
}
