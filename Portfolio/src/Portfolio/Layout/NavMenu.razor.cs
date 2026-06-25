using Microsoft.JSInterop;

namespace Portfolio.Layout;

public partial class NavMenu
{
    private record SectionLink(string Id, string Label, string Idx);

    private static readonly SectionLink[] Sections =
    {
        new("about",      "About",      "01"),
        new("experience", "Experience", "02"),
        new("projects",   "Projects",   "03"),
        new("education",  "Education",  "04"),
        new("skills",     "Skills",     "05"),
        new("interests",  "Interests",  "06"),
        new("repositories",  "Repositories",  "07"),
    };

    private string activeSection = "about";
    private bool _drawerOpen = false;
    private DotNetObjectReference<NavMenu>? _dotNetRef;

    private void ToggleDrawer() => _drawerOpen = !_drawerOpen;
    private void CloseDrawer() => _drawerOpen = false;

    private bool IsHomeRoute =>
        string.IsNullOrEmpty(Nav.ToBaseRelativePath(Nav.Uri).TrimEnd('/').Split('?', '#')[0]);

    protected override void OnInitialized()
    {
        Nav.LocationChanged += OnLocationChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            await StartObserver();
            await JSRuntime.InvokeVoidAsync("PortfolioDrawer.bind", _dotNetRef);
        }
    }

    private async Task StartObserver()
    {
        if (!IsHomeRoute || _dotNetRef is null) return;
        var ids = Sections.Select(s => s.Id).ToArray();
        await JSRuntime.InvokeVoidAsync("PortfolioObserveSections", _dotNetRef, ids);
    }

    private async void OnLocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
    {
        // Re-attach the section observer when navigating back to home; the
        // section nodes only exist while Index.razor is rendered.
        StateHasChanged();
        if (IsHomeRoute) await StartObserver();
    }

    [JSInvokable]
    public void SetActiveSection(string sectionId)
    {
        if (activeSection == sectionId) return;
        activeSection = sectionId;
        StateHasChanged();
    }

    [JSInvokable]
    public void CloseDrawerFromJs()
    {
        if (!_drawerOpen) return;
        _drawerOpen = false;
        InvokeAsync(StateHasChanged);
    }

    private void GoSection(string id)
    {
        activeSection = id;
        _drawerOpen = false;
        Nav.NavigateTo($"/#{id}");
    }

    private void GoHome() => GoSection("about");

    public async ValueTask DisposeAsync()
    {
        Nav.LocationChanged -= OnLocationChanged;
        try
        {
            await JSRuntime.InvokeVoidAsync("PortfolioDisposeObservers");
            await JSRuntime.InvokeVoidAsync("PortfolioDrawer.unbind");
        }
        catch { /* JS runtime may already be gone during teardown */ }
        _dotNetRef?.Dispose();
    }
}