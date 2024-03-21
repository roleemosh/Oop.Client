using Microsoft.AspNetCore.Components.QuickGrid;
using Oop.Client.Model;
using Oop.Client.Services;

namespace Oop.Client.Pages
{
    public partial class People
    {
        #region Fields
        private bool _isLoaded = false;
        private string? _firstNameFilter;
        private string? _employeeIDFilter;
        private byte _maxAccessLevelFilter = maxAccessLevel;
        private byte _minAccessLevelFilter = 0;
        private const byte maxAccessLevel = 255;
        private IQueryable<Person> _people;
        #endregion Fields


        protected override async Task OnInitializedAsync()
        {
            await PopulateDataBase();
        }


        private async Task PopulateDataBase()
        {
            await IndexedDb.Initialize(nameof(Person), "employeeID");

            var count = await IndexedDb.CountAsync(nameof(Person));
            //Populate with fake datas
            if (count < 1)
            {
                await Fake.Initialize();

                FakeService.CreateRandomPeople(500);
                FakeService.OnFakePeopleChanged += async (e) =>
                {
                    //people = e.AsQueryable();
                    await IndexedDb.PopulateValuesAsync(nameof(Person), e.AsEnumerable());
                };
            }

            _people = (await IndexedDb.GetAllValueAsync<Person>(nameof(Person))).AsQueryable();
            _isLoaded = true;
            StateHasChanged();
        }


        private async ValueTask<GridItemsProviderResult<Person>> ProvideVirtualizedItems(GridItemsProviderRequest<Person> request)
        {
            if (_people is null)
            {
                return GridItemsProviderResult.From<Person>(
                    items: Array.Empty<Person>(),
                    totalItemCount: 0);
            }
            else
            {
                // Debounce the requests. This eliminates a lot of redundant queries at the cost of slight lag after interactions.
                // If you wanted, you could try to make it only debounce on the 2nd-and-later request within a cluster.
                await Task.Delay(20);
                if (request.CancellationToken.IsCancellationRequested)
                {
                    return default;
                }

                var records = FilteredPeople().Skip(request.StartIndex).Take(request.Count ?? 0);

                var sortedRecords = request.ApplySorting(records).ToArray();

                var result = GridItemsProviderResult.From<Person>(
                    items: sortedRecords,
                    totalItemCount: _people.Count());
                return result;
            }
        }

        IQueryable<Person> FilteredPeople()
        {
            var result = _people.AsQueryable();

            if (!string.IsNullOrEmpty(_employeeIDFilter))
            {
                result = result.Where(p => p.EmployeeID.Contains(_employeeIDFilter, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!string.IsNullOrEmpty(_firstNameFilter))
            {
                result = result.Where(p => p.FirstName.Contains(_firstNameFilter, StringComparison.CurrentCultureIgnoreCase));
            }

            result = result.Where(x => x.AccessLevel >= _minAccessLevelFilter && x.AccessLevel <= _maxAccessLevelFilter);

            return result;
        }
    }
}
