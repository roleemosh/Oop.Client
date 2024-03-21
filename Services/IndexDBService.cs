using Microsoft.JSInterop;
using System.Runtime.Versioning;

namespace Oop.Client.Services
{
    [SupportedOSPlatform("browser")]
    public partial class IndexDBService :  IAsyncDisposable
    {
        private Lazy<IJSObjectReference> _accessorJsRef = new();
        private readonly IJSRuntime _jsRuntime;

        public IndexDBService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task Initialize(string collectionName, string keyPath)
        {
            await WaitForReference();
            await _accessorJsRef.Value.InvokeVoidAsync("initialize", new[] { collectionName, keyPath });
        }

        public async Task<T> GetValueAsync<T>(string collectionName, int id)
        {
            await WaitForReference();
            var result = await _accessorJsRef.Value.InvokeAsync<T>("get", collectionName, id);

            return result;
        }

        public async Task<IEnumerable<T>> GetAllValueAsync<T>(string collectionName)
        {
            await WaitForReference();
            var result = await _accessorJsRef.Value.InvokeAsync<IEnumerable<T>>("getAll", collectionName);

            return result;
        }

        public async Task<int> CountAsync(string collectionName)
        {
            await WaitForReference();
            var result = await _accessorJsRef.Value.InvokeAsync<int>("count", collectionName);

            return result;
        }

        public async Task PopulateValuesAsync<T>(string collectionName, IEnumerable<T> values)
        {
            await WaitForReference();
            await _accessorJsRef.Value.InvokeVoidAsync("populate", collectionName, values);
        }

        public async Task SetValueAsync<T>(string collectionName, T value)
        {
            await WaitForReference();
            await _accessorJsRef.Value.InvokeVoidAsync("set", collectionName, value);
        }

        private async Task WaitForReference()
        {
            if (_accessorJsRef.IsValueCreated is false)
            {
                _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/script/indexdbservice.js"));
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_accessorJsRef.IsValueCreated)
            {
                await _accessorJsRef.Value.DisposeAsync();
            }
        }
    }
}
