using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using LY.EMIS5.Common.Chart.ECharts;
using LY.EMIS5.Common.Utilities;

namespace LY.EMIS5.Common.Chart.Extensions
{
    /// <summary>
    /// ECharts的扩展方法
    /// </summary>
    public static class EChartsExtensions
    {
        /// <summary>
        /// 获取表达式的值
        /// </summary>
        /// <typeparam name="T">结果的类型</typeparam>
        /// <param name="container">实体容器</param>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        private static T Eval<T>(object container, string expression) 
        {
            if (!String.IsNullOrEmpty(expression))
            {
                var value = DataBinder.Eval(container, expression);
                return (T)value;
            }
        
            return default(T);
        }

        /// <summary>
        /// 将集合转化为ECharts的图表选项配置
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TxAxis"></typeparam>
        /// <typeparam name="TyAxis"></typeparam>
        /// <param name="entList"></param>
        /// <param name="_seriesName"></param>
        /// <param name="_seriesChartType"></param>
        /// <param name="xField"></param>
        /// <param name="yField"></param>
        /// <returns></returns>
        public static option<TxAxis, TyAxis> AsEChartsOption<TEntity, TxAxis, TyAxis>(this IEnumerable<TEntity> entList, string _seriesName, chartType _seriesChartType, Expression<Func<TEntity, TxAxis>> xField, Expression<Func<TEntity, TyAxis>> yField) 
            where TyAxis : struct 
        {
            var x = ExpressionHelper.GetExpressionText(xField);
            var y = ExpressionHelper.GetExpressionText(yField);
            
            var xList = new List<TxAxis>();
            var yList = new List<TyAxis>();
            
            foreach (var obj in entList)
            {
                var tmpX = Eval<TxAxis>(obj, x);
                if ( ! xList.Where(m=> m.Equals(tmpX)).Any())
                {
                    xList.Add(tmpX);
                }
                
                yList.Add(Eval<TyAxis>(obj, y));
            }

            option<TxAxis, TyAxis> opt = new option<TxAxis, TyAxis>();
            opt.series = new List<series<TyAxis>>();
            opt.series.Add(new series<TyAxis>() { name = _seriesName, chartType = _seriesChartType, data = yList });

            opt.xAxis = new xAxis<TxAxis>();
            opt.xAxis.data = xList;

            return opt;
        }

        public static string AsEChartsOptionJson<TEntity, TxAxis, TyAxis>(this IEnumerable<TEntity> entList, string _name, chartType _chartType, Expression<Func<TEntity, TxAxis>> xField, Expression<Func<TEntity, TyAxis>> yField)
            where TyAxis : struct
        {
            return SerializeUtils.JsonSerialize(AsEChartsOption(entList, _name, _chartType, xField, yField));
        }

    }
}
