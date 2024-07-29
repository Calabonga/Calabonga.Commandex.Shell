namespace Calabonga.Commandex.Contracts;

/// <summary>
/// // Calabonga: Summary required (IDbConnectionFactory 2024-07-26 09:17)
/// </summary>
public interface IDbConnectionFactory
{
    string Description { get; }
}

/// <summary>
/// // Calabonga: Summary required (IDbConnectionFactory 2024-07-26 09:14)
/// </summary>
/// <typeparam name="TConnection"></typeparam>
public interface IDbConnectionFactory<out TConnection> : IDbConnectionFactory
{
    TConnection CreateConnection(string connectionString);
}

/// <summary>
/// // Calabonga: Summary required (IDbConnectionFactory 2024-07-26 09:14)
/// </summary>
/// <typeparam name="TConnection"></typeparam>
/// <typeparam name="TParameters"></typeparam>
public interface IDbConnectionFactory<out TConnection, in TParameters> : IDbConnectionFactory
{
    TConnection CreateConnection(string connectionString, TParameters credential);
}