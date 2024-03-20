using Oop.Client.Interfaces;
using Oop.Client.Model;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Text.Json;

namespace Oop.Client.Services
{
    [SupportedOSPlatform("browser")]
    public partial class FakeService : IInitialize
    {
        private JSObject _modul;

        public static Action<List<Person>> OnFakePeopleChanged;

        [JSImport("createRandomPeople", nameof(FakeService))]
        public static partial void CreateRandomPeople(int count);


        [JSExport]
        public static void PopulateFakePeople(string peopleJson)
        {
            if (string.IsNullOrEmpty(peopleJson))
                return;

            var fakePeople = JsonSerializer.Deserialize<List<Person>>(peopleJson);

            OnFakePeopleChanged?.Invoke(fakePeople);
        }

        public async Task Initialize()
        {
            if (_modul != null)
                return;

            _modul = await JSHost.ImportAsync(nameof(FakeService), "../../script/fakeservice.js");   
        }
    }
}
