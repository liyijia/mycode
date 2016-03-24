using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    /// <summary>
    /// 坐标轴
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class axis
    {
        private string _type = "'category' | 'value'";
        private string _position = "'bottom' | 'left'";
        private axisLine _axisLine = new axisLine() { show = true };
        private axisTick _axisTick = new axisTick() { show = true };
        private axisLabel _axisLabel = new axisLabel() { show = true };
        private splitLine _splitLine = new splitLine() { show = true };
        private splitArea _splitArea = new splitArea() { show = false };

        /// <summary>
        /// 坐标轴类型，横轴默认为类目型'category'，纵轴默认为数值型'value' 
        /// </summary>
        public virtual string type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 坐标轴类型，横轴默认为类目型'bottom'，纵轴默认为数值型'left'，可选为：'bottom' | 'top' | 'left' | 'right' 
        /// </summary>
        public virtual string position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// 坐标轴线，默认显示，属性show控制显示与否，属性lineStyle控制线条样式
        /// </summary>
        public axisLine axisLine
        {
            get { return _axisLine; }
            set { _axisLine = value; }
        }

        /// <summary>
        /// 坐标轴小标记，默认不显示，属性show控制显示与否，属性length控制线长，属性lineStyle控制线条样式 
        /// </summary>
        public axisTick axisTick
        {
            get { return _axisTick; }
            set { _axisTick = value; }
        }

        /// <summary>
        /// 坐标轴文本标签
        /// </summary>
        public axisLabel axisLabel
        {
            get { return _axisLabel; }
            set { _axisLabel = value; }
        }

        /// <summary>
        /// 分隔线，默认显示，属性show控制显示与否，属性lineStyle控制线条样式
        /// </summary>
        public splitLine splitLine
        {
            get { return _splitLine; }
            set { _splitLine = value; }
        }

        /// <summary>
        /// 分隔区域，默认不显示，属性show控制显示与否，属性areaStyle控制区域样式 
        /// </summary>
        public splitArea splitArea
        {
            get { return _splitArea; }
            set { _splitArea = value; }
        }
    }

    /// <summary>
    /// 直角坐标系中横轴数组，数组中每一项代表一条横轴坐标轴，仅有一条时可省略数值。最多同时存在2条横轴，单条横轴时可指定安放于grid的底部（默认）或顶部，2条同时存在时位置互斥，默认第一条安放于底部，第二条安放于顶部。
    /// 坐标轴有两种类型，类目型和数值型（区别详见axis），横轴通常为类目型，但条形图时则横轴为数值型，散点图时则横纵均为数值型，具体参数详见axis。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class xAxis<T> : axis
    {
        private bool _boundaryGap = true;
        private ICollection<T> _data = new List<T>();

        /// <summary>
        /// 类目型
        /// </summary>
        public override string type
        {
            get
            {
                return "category";
            }
            set
            {
                
            }
        }

        /// <summary>
        /// 坐标轴类型，横轴默认为类目型'bottom'，纵轴默认为数值型'left'，可选为：'bottom' | 'top' | 'left' | 'right' 
        /// </summary>
        public override string position
        {
            get
            {
                return "bottom";
            }
            set
            {
                
            }
        }

        /// <summary>
        /// 类目起始和结束两端空白策略，默认为true留空，false则顶头 
        /// </summary>
        public bool boundaryGap
        {
            get { return _boundaryGap; }
            set { _boundaryGap = value; }
        }

        /// <summary>
        /// 类目列表，同时也是label内容
        /// </summary>
        public ICollection<T> data
        {
            get { return _data; }
            set { _data = value; }
        }
    }

    public class yAxis : axis
    {
        private string _name = string.Empty;
        private string _nameLocation = "end";
        private ICollection<int> _boundaryGap = new List<int>() { 0, 0 };
        private string _min = "null";
        private string _max = "null";
        private bool _scale = false;
        private int _precision = 0;
        private int _power = 100;
        private int _splitNumber = 5;

        /// <summary>
        /// 类目型
        /// </summary>
        public override string type
        {
            get
            {
                return "value";
            }
            set
            {

            }
        }

        /// <summary>
        /// 坐标轴类型，横轴默认为类目型'bottom'，纵轴默认为数值型'left'，可选为：'bottom' | 'top' | 'left' | 'right' 
        /// </summary>
        public override string position
        {
            get
            {
                return "left";
            }
            set
            {

            }
        }

        /// <summary>
        /// 坐标轴名称，默认为空
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 坐标轴名称位置，默认为'end'，可选为：'start' | 'end'
        /// </summary>
        public string nameLocation
        {
            get { return _nameLocation; }
            set { _nameLocation = value; }
        }

        /// <summary>
        /// 数值轴两端空白策略，数组内数值代表百分比，[原始数据最小值与最终最小值之间的差额，原始数据最大值与最终最大值之间的差额]
        /// </summary>
        public ICollection<int> boundaryGap
        {
            get { return _boundaryGap; }
            set { _boundaryGap = value; }
        }


        /// <summary>
        /// 指定的最小值，eg: 0，默认无，会自动根据具体数值调整，指定后将忽略boundaryGap[0] 
        /// 原谅我这么做:在ECharts中默认是为null的,但是序列化后不能被识别,
        /// ECharts的开发人员说他们这里做了类型转换,所以可以识别到的
        /// </summary>
        public string min
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// 指定的最大值，eg: 100，默认无，会自动根据具体数值调整，指定后将忽略boundaryGap[1] 
        /// 原谅我这么做:在ECharts中默认是为null的,但是序列化后不能被识别,
        /// ECharts的开发人员说他们这里做了类型转换,所以可以识别到的
        /// </summary>
        public string max
        {
            get { return _max; }
            set { _max = value; }
        }

        /// <summary>
        /// 脱离0值比例，放大聚焦到最终_min，_max区间 
        /// </summary>
        public bool scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// 小数精度，默认为0，无小数点
        /// </summary>
        public int precision
        {
            get { return _precision; }
            set { _precision = value; }
        }

        /// <summary>
        /// 整数精度，默认为100，个位和百位为0 
        /// </summary>
        public int power
        {
            get { return _power; }
            set { _power = value; }
        }

        /// <summary>
        /// 分割段数，默认为5 
        /// </summary>
        public int splitNumber
        {
            get { return _splitNumber; }
            set { _splitNumber = value; }
        }
    }
}
