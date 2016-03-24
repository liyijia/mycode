using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    /// <summary>
    /// 分隔区域
    /// </summary>
    public class splitArea
    {
        private bool _show = false;
        private areaStyle _areaStyle = new areaStyle() { color = "['rgba(250,250,250,0.3)','rgba(200,200,200,0.3)']" };

        /// <summary>
        /// 是否显示，默认为false，设为true后带如下默认样式
        /// </summary>
        public bool show
        {
            get { return _show; }
            set { _show = value; }
        }

        /// <summary>
        ///  属性areaStyle控制区域样式，颜色数组实现间隔变换。 
        /// </summary>
        public areaStyle areaStyle
        {
            get { return _areaStyle; }
            set { _areaStyle = value; }
        }
    }
}
