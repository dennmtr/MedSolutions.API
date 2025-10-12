namespace MedSolutions.Shared.Enums;

public class DatabaseProviderInfo(string? databaseEngine)
{
    public const string MySql = "mysql";
    public const string PostgreSql = "npgsql";
    public const string Sqlite = "sqlite";

    private readonly string _databaseEngine = databaseEngine?.ToLower() ?? string.Empty;

    public bool IsMySql() => _databaseEngine.Contains(MySql);
    public bool IsPostgreSql() => _databaseEngine.Contains(PostgreSql);
    public bool IsSqlite() => _databaseEngine.Contains(Sqlite);

    public override string ToString() => _databaseEngine;
}

