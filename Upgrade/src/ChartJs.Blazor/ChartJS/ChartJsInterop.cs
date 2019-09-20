using ChartJs.Blazor.ChartJS.Common;
using ChartJs.Blazor.ChartJS.Common.Legends.OnClickHandler;
using ChartJs.Blazor.ChartJS.Common.Legends.OnHover;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChartJs.Blazor.ChartJS
{
    public static class ChartJsInterop
    {
        public static ValueTask<bool> SetupChart(this IJSRuntime jsRuntime, ChartConfigBase chartConfig)
        {
            try
            {
                return jsRuntime.InvokeAsync<bool>("ChartJSInterop.SetupChart", StripNulls2(chartConfig));
                //return jsRuntime.InvokeAsync<bool>("ChartJSInterop.SetupChart", chartConfig);
            }
            catch (Exception exp)
            {
            }

            return new ValueTask<bool>(false);
        }

        public static ValueTask<bool> UpdateChart(this IJSRuntime jsRuntime, ChartConfigBase chartConfig)
        {
            try
            {
                return jsRuntime.InvokeAsync<bool>("ChartJSInterop.UpdateChart", StripNulls2(chartConfig));
                //return jsRuntime.InvokeAsync<bool>("ChartJSInterop.UpdateChart", chartConfig);
            }
            catch (Exception exp)
            {
            }

            return new ValueTask<bool>(false);
        }

        /// <summary>
        /// Returns an object that is equivalent to the given parameter but without any null member AND it preserves DotNetInstanceClickHandler/DotNetInstanceHoverHandler members intact
        ///
        /// <para>Preserving DotNetInstanceClick/HoverHandler members is important because they contain DotNetObjectRefs to the instance whose method should be invoked on click/hover</para>
        ///
        /// <para>This whole method is hacky af but necessary. Stripping null members is only needed because the default config for the Line charts on the Blazor side is somehow messed up. If this were not the case no null member stripping were necessary and hence, the recovery of the DotNetObjectRef members would also not be needed. Nevertheless, The Show must go on!</para>
        /// </summary>
        /// <param name="chartConfig"></param>
        /// <returns></returns>
        private static object StripNulls(ChartConfigBase chartConfig)
        {
            // Serializing with the custom serializer settings remove null members
            var cleanChartConfigStr = JsonConvert.SerializeObject(chartConfig, JsonSerializerSettings);

            // Get back an ExpandoObject dynamic with the clean config - having an ExpandoObject allows us to add/replace members regardless of type
            dynamic clearConfigExpando = JsonConvert.DeserializeObject(cleanChartConfigStr,
                typeof(ExpandoObject),
                new ExpandoObjectConverter(){});

            // Restore any .net refs that need to be passed intact
            var dynamicChartConfig = (dynamic)chartConfig;
            if (dynamicChartConfig?.Options?.Legend?.OnClick != null
                && dynamicChartConfig?.Options?.Legend?.OnClick is DotNetInstanceClickHandler)
            {
                clearConfigExpando.options = clearConfigExpando.options ?? new { };
                clearConfigExpando.options.legend = clearConfigExpando.options.legend ?? new { };
                clearConfigExpando.options.legend.onClick = dynamicChartConfig.Options.Legend.OnClick;
            }

            if (dynamicChartConfig?.Options?.Legend?.OnHover != null
                && dynamicChartConfig?.Options?.Legend?.OnHover is DotNetInstanceHoverHandler)
            {
                clearConfigExpando.options = clearConfigExpando.options ?? new { };
                clearConfigExpando.options.legend = clearConfigExpando.options.legend ?? new { };
                clearConfigExpando.options.legend.onHover = dynamicChartConfig.Options.Legend.OnHover;
            }

            return new Dictionary<string, object>(clearConfigExpando);
        }

        private static object StripNulls2(ChartConfigBase chartConfig)
        {
            // Serializing with the custom serializer settings remove null members
            var cleanChartConfigStr = JsonConvert.SerializeObject(chartConfig, JsonSerializerSettings);

            // Get back an ExpandoObject dynamic with the clean config - having an ExpandoObject allows us to add/replace members regardless of type
            dynamic cleanChartConfig = System.Text.Json.JsonSerializer.Deserialize(cleanChartConfigStr, typeof(object));

            // Restore any .net refs that need to be passed intact
            var dynamicChartConfig = (dynamic)chartConfig;
            if (dynamicChartConfig?.Options?.Legend?.OnClick != null
                && dynamicChartConfig?.Options?.Legend?.OnClick is DotNetInstanceClickHandler)
            {
                cleanChartConfig.options = cleanChartConfig.options ?? new { };
                cleanChartConfig.options.legend = cleanChartConfig.options.legend ?? new { };
                cleanChartConfig.options.legend.onClick = dynamicChartConfig.Options.Legend.OnClick;
            }

            if (dynamicChartConfig?.Options?.Legend?.OnHover != null
                && dynamicChartConfig?.Options?.Legend?.OnHover is DotNetInstanceHoverHandler)
            {
                cleanChartConfig.options = cleanChartConfig.options ?? new { };
                cleanChartConfig.options.legend = cleanChartConfig.options.legend ?? new { };
                cleanChartConfig.options.legend.onHover = dynamicChartConfig.Options.Legend.OnHover;
            }

            return cleanChartConfig;
        }

        public static JsonSerializerOptions JsonSerializerSettings2 => new JsonSerializerOptions()
        {
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            
        };

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };
    }
}