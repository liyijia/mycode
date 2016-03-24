using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    /// <summary>
    /// 图表序列
    /// </summary>
    /// <typeparam name="T">序列数据类型</typeparam>
    public class series<T> where T : struct
    {
        private chartType _chartType;
        private tooltip _tooltip = new tooltip();

        /// <summary>
        /// 系列名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 提示框样式
        /// </summary>
        public tooltip tooltip
        {
            get { return _tooltip; }
            set { _tooltip = value; }
        }
        
        /// <summary>
        /// 图表类型 字符串
        /// 应设置 Type 设置type无效
        /// </summary>
        public string type
        {
            get { return _chartType.ToString(); }
            set { }
        }
        
        /// <summary>
        /// 图表类型 枚举类型
        /// </summary>
        public chartType chartType
        {
            set { _chartType = value; }
        }

        /// <summary>
        /// 数据
        /// </summary>
        public ICollection<T> data { get; set; }
    }
}
