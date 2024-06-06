using FufosFrontend.DTOS;
using FufosFrontend.Services;
using Microsoft.AspNetCore.Components;

namespace FufosFrontend.Shared;

public partial class NavMenu : ComponentBase
{
    [Inject] public LocalDataService DataService {get; set;} = null!;
    [Inject] public NavigationManager Navigation {get; set;} = null!;
    [Parameter] public List<MenuItemDTO>? Data { get; set; }

    protected override void OnInitialized()
    {
        Data ??= DataService.GetLocalData<List<MenuItemDTO>>("menu.json");
        base.OnInitialized();
    }

    private void Navigate(string? Url)
    {
        if (string.IsNullOrEmpty(Url))
            return;

        Navigation.NavigateTo(Url);
    }
}