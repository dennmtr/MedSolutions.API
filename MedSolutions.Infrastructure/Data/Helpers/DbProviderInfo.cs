namespace MedSolutions.Infrastructure.Data.Helpers;

public class DbProviderInfo(string? providerName)
{
    private readonly string _providerName = providerName?.ToLower() ?? string.Empty;

    public bool IsSqlite() => _providerName.Contains("sqlite");
    public bool IsMySql() => _providerName.Contains("mysql");
    public bool IsMariaDb() => IsMySql();
    public override string ToString() => _providerName;
}
