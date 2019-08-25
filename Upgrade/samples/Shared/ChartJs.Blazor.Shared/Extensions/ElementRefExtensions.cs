using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChartJs.Blazor.Shared.Extensions
{
    public static class ElementRefExtensions
    {
        public static Task<string> GetValue(this ElementReference elementRef, IJSRuntime jsRuntime)
        {
            return jsRuntime.InvokeAsync<string>("SampleFunctions.GetElementValue", elementRef);
        }
    }
}