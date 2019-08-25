using ChartJs.Blazor.ChartJS;
using ChartJs.Blazor.ChartJS.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChartJs.Blazor.Charts
{
    public abstract class ChartBase<TConfig> : ComponentBase where TConfig : ChartConfigBase
    {
        [Inject] private IJSRuntime JsRuntime { get; set; }

        [Parameter] public TConfig Config { get; set; }

        [Parameter] public int Width { get; set; } = 400;

        [Parameter] public int Height { get; set; } = 400;

        protected override void OnAfterRender()
        {
            try
            {
                base.OnAfterRender();
                JsRuntime.SetupChart(Config);
            }
            catch
            {
            } // https://github.com/aspnet/AspNetCore/issues/8327
        }

        public void Update()
        {
            JsRuntime.UpdateChart(Config);
        }
    }
}