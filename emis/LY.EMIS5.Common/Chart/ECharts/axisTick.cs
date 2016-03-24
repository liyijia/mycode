using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    /// <summary>
    /// 坐标轴小标记
    /// </summary>
    public class axisTick
    {
        private bool _show = false;
        private int _length = 4;
        private lineStyle _lineStyle = new lineStyle() { color = "#ccc", width = 1, type = "solid" };

        /// <summary>
        /// 是否显示，默认为false，设为true后下面为默认样式
        /// </summary>
        public bool show
        {
            get { return _show; }
            set { _show = value; }
        }

        /// <summary>
        /// 属性length控制线长
        /// </summary>
        public int length
        {
            get { return _length; }
            set { _length = value; }
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
