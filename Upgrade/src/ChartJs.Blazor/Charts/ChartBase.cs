using ChartJs.Blazor.ChartJS;
using ChartJs.Blazor.ChartJS.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ChartJs.Blazor.Charts
{
    public abstract class ChartBase<TConfig> : ComponentBase where TConfig : ChartConfigBase
    {
        private bool _renderedOnce = false;

        [Inject] private IJSRuntime JsRuntime { get; set; }

        [Parameter] public TConfig Config { get; set; }

        [Parameter] public int Width { get; set; } = 400;

        [Parameter] public int Height { get; set; } = 400;

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            _renderedOnce = _renderedOnce || firstRender;

            try
            {
                base.OnAfterRenderAsync(firstRender);
                return JsRuntime.SetupChart(Config).AsTask();
            }
            catch
            {
            } // https://github.com/aspnet/AspNetCore/issues/8327

            return Task.CompletedTask;
        }

        public async void Update()
        {
            if (!_renderedOnce)
            {
                return;
            }

            //await JsRuntime.Prompt("");
            await JsRuntime.UpdateChart(Config);
        }
    }
}