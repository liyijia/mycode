using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    /// <summary>
    /// 图表选项，包含图表实例任何可配置选项
    /// </summary>
    /// <typeparam name="X">x轴的数据类型</typeparam>
    /// <typeparam name="Y">y轴的数据类型</typeparam>
    public class option<X, Y> where Y : struct
    {
        private xAxis<X> _xAxis = new xAxis<X>();
        private yAxis _yAxis = new yAxis();
        private ICollection<string> _color = new List<string>();
        private title _title = new title();
        private legend _legend = new legend();
        private toolbox _toolbox = new toolbox();
        private tooltip _tooltip = new tooltip();
        
        /// <summary>
        ///  数值系列的颜色列表，默认为null则采用内置颜色，可配数组，
        ///  eg：['#87cefa', 'rgba(123,123,123,0.5)','...']，
        ///  当系列数量个数比颜色列表长度大时将循环选取 
        /// </summary>
        public ICollection<string> color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// 标题，每个图表最多仅有一个标题控件
        /// </summary>
        public title title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// 图例，每个图表最多仅有一个图例，混搭图表共享
        /// </summary>
        public legend legend
        {
            get { return _legend; }
            set { _legend = value; }
        }

        /// <summary>
        /// 工具箱，每个图表最多仅有一个工具箱 
        /// </summary>
        public toolbox toolbox
        {
            get { return _toolbox; }
            set { _toolbox = value; }
        }

        /// <summary>
        /// 提示框，鼠标悬浮交互时的信息提示 
        /// </summary>
        public tooltip tooltip
        {
            get { return _tooltip; }
            set { _tooltip = value; }
        }

        /// <summary>
        /// 直角坐标系中横轴数组，数组中每一项代表一条横轴坐标轴，仅有一条时可省略数值。最多同时存在2条横轴，单条横轴时可指定安放于grid的底部（默认）或顶部，2条同时存在时位置互斥，默认第一条安放于底部，第二条安放于顶部。
        /// 坐标轴有两种类型，类目型和数值型（区别详见axis），横轴通常为类目型，但条形图时则横轴为数值型，散点图时则横纵均为数值型，具体参数详见axis。
        /// </summary>
        public xAxis<X> xAxis
        {
            get { return _xAxis; }
            set { _xAxis = value; }
        }
        
        /// <summary>
        /// 直角坐标系中纵轴数组，数组中每一项代表一条纵轴坐标轴，仅有一条时可省略数值。最多同时存在2条纵轴，单条纵轴时可指定安放于grid的左侧（默认）或右侧，2条同时存在时位置互斥，默认第一条安放于左侧，第二条安放于右侧。
        /// 坐标轴有两种类型，类目型和数值型（区别详见axis），纵轴通常为数值型，但条形图时则纵轴为类目型，具体参数详见axis。
        /// </summary>
        public yAxis yAxis
        {
            get { return _yAxis; }
            set { _yAxis = value; }
        }

        /// <summary>
        /// 驱动图表生成的数据内容，数组中每一项代表一个系列的特殊选项及数据
        /// </summary>
        public ICollection<series<Y>> series { get; set; }
    }
}
