using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Chart.ECharts
{
    public class axisLabel
    {
        private bool _show = true;
        private string _interval = "auto";
        private int _rotate = 0;
        private int _margin = 8;
        private string _formatter = string.Empty;
        private textStyle _textStyle = new textStyle() { color = "#333" };

        /// <summary>
        /// 是否显示，默认为true，设为false后下面都没意义了
        /// </summary>
        public bool show
        {
            get { return _show; }
            set { _show = value; }
        }

        /// <summary>
        /// 标签显示挑选间隔，默认为'auto'，可选为：'auto'（自动隐藏显示不下的） | 0（全部显示） | {number}（用户指定选择间隔）
        /// </summary>
        public string interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        /// <summary>
        /// 标签旋转角度，默认为0，不旋转，正直为逆时针，负值为顺时针，可选为：-90 ~ 90 
        /// </summary>
        public int rotate
        {
            get { return _rotate; }
            set { _rotate = value; }
        }

        /// <summary>
        /// 坐标轴文本标签与坐标轴的间距，默认为8，单位px 
        /// </summary>
        public int margin
        {
            get { return _margin; }
            set { _margin = value; }
        }


        /// <summary>
        /// 间隔名称格式器：{string}（Template） | {Function} 
        /// </summary>
        public string formatter
        {
            get { return _formatter; }
            set { _formatter = value; }
        }

        /// <summary>
        /// 文本样式
        /// </summary>
        public textStyle textStyle
        {
            get { return _textStyle; }
            set { _textStyle = value; }
        } 


    }
}
