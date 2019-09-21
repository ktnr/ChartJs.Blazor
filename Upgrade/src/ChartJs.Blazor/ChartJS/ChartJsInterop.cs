using ChartJs.Blazor.ChartJS.Common;
using ChartJs.Blazor.ChartJS.Common.Legends.OnClickHandler;
using ChartJs.Blazor.ChartJS.Common.Legends.OnHover;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ChartJs.Blazor.ChartJS
{
    public static class ChartJsInterop
    {
        public static ValueTask<bool> SetupChart(this IJSRuntime jsRuntime, ChartConfigBase chartConfig)
        {
            try
            {
                //return jsRuntime.InvokeAsync<bool>("ChartJSInterop.SetupChart", StripNulls(chartConfig));
                return jsRuntime.InvokeAsync<bool>("ChartJSInterop.SetupChart", chartConfig);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SetupChart failed with: {ex}");
            }

            return new ValueTask<bool>(false);
        }

        public static ValueTask<bool> UpdateChart(this IJSRuntime jsRuntime, ChartConfigBase chartConfig)
        {
            try
            {
                //return jsRuntime.InvokeAsync<bool>("ChartJSInterop.UpdateChart", StripNulls(chartConfig));
                return jsRuntime.InvokeAsync<bool>("ChartJSInterop.UpdateChart", chartConfig);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateChart failed with: {ex}");
            }

            return new ValueTask<bool>(false);
        }

        private static object StripNulls(ChartConfigBase chartConfig)
        {
            // Serializing with the custom serializer settings to remove null members and switch to camel case
            var cleanChartConfigStr = JsonConvert.SerializeObject(chartConfig, JsonSerializerSettings);

            // Get back an a dynamic object in order to enhance it with .Net instance handlers
            //dynamic cleanChartConfig = System.Text.Json.JsonSerializer.Deserialize<ExpandoObject>(cleanChartConfigStr);
            dynamic cleanChartConfig = JsonConvert.DeserializeObject(cleanChartConfigStr,
                typeof(ExpandoObject),
                new ExpandoObjectConverter());

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

            return (cleanChartConfig as ExpandoObject).ToList();
        }

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