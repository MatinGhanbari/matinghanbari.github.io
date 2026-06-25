using System.Net.Http.Json;

namespace Portfolio.Pages;

public partial class GitHub
{
    private List<Repo>? repositories;
    private string query = "";

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var data = await Http.GetFromJsonAsync<List<Repo>>(
                "https://api.github.com/users/matinghanbari/repos?per_page=100");
            repositories = (data ?? new())
                .OrderByDescending(r => r.Pushed_At ?? r.Updated_At)
                .ToList();
        }
        catch
        {
            repositories = new List<Repo>();
        }
    }

    private void GoHome() => Nav.NavigateTo("/#about");

    private static List<Repo> Filter(List<Repo> all, string q)
    {
        if (string.IsNullOrWhiteSpace(q)) return all;
        var needle = q.Trim();
        return all.Where(r =>
            (r.Name?.Contains(needle, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (r.Description?.Contains(needle, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (r.Language?.Contains(needle, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();
    }

    private static string Relative(DateTime utc)
    {
        var span = DateTime.UtcNow - utc;
        if (span.TotalDays < 1) return "today";
        if (span.TotalDays < 7) return $"{(int)span.TotalDays}d";
        if (span.TotalDays < 30) return $"{(int)(span.TotalDays / 7)}w";
        if (span.TotalDays < 365) return $"{(int)(span.TotalDays / 30)}mo";
        return $"{(int)(span.TotalDays / 365)}y";
    }

    private class Repo
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Html_Url { get; set; }
        public string? Language { get; set; }
        public int Stargazers_Count { get; set; }
        public DateTime? Pushed_At { get; set; }
        public DateTime Updated_At { get; set; }
        public bool Fork { get; set; }
    }
}