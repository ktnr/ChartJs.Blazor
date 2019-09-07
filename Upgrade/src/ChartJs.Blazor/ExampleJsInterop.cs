using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ChartJs.Blazor
{
    public static class ExampleJsInterop
    {
        public static Task<string> Prompt(this IJSRuntime jsRuntime, string message)
        {
            // Implemented in exampleJsInterop.js
            return jsRuntime.InvokeAsync<string>("exampleJsFunctions.showPrompt", message);
        }
    }
}