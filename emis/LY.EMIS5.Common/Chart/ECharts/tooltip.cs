using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    /// <summary>
    /// 提示框
    /// </summary>
    public class tooltip
    {
        private string _trigger = "item";
        private bool _show = true;


        /// <summary>
        /// 触发类型
        /// </summary>
        public string trigger
        {
          get { return _trigger; }
          set { _trigger = value; }
        }

        /// <summary>
        /// 显示策略
        /// </summary>
        public bool show
        {
            get { return _show; }
            set { _show = value; }
        }
    }
}
