using System.Data.Common;
using Npgsql;
using Testcontainers.PostgreSql;

namespace GerPros_Backend_API.Application.FunctionalTests;

public class PostgreSqlContainerTestDatabase : ITestDatabase
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithDatabase("GerPros_Backend_APITestDb")
        .WithUsername("TestUser")
        .WithPassword("TestUser123")
        .WithExposedPort(55868)
        .Build();

    private string ConnectionString => _postgreSqlContainer.GetConnectionString();
    
    public async Task InitialiseAsync()
    {
        await _postgreSqlContainer.StartAsync();
        Console.WriteLine("PostgreSQL container started with connection string: " + ConnectionString);
    }

    public DbConnection GetConnection()
    {
        var connection = new NpgsqlConnection(ConnectionString);
        return connection;
    }

    public async Task ResetAsync()
    {
        await using var connection = GetConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = """
                              
                                          DO $$ DECLARE
                                              r RECORD;
                                          BEGIN
                                              FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = 'public') LOOP
                                                  EXECUTE 'TRUNCATE TABLE ' || quote_ident(r.tablename) || ' CASCADE';
                                              END LOOP;
                                          END $$;
                              """;
        await command.ExecuteNonQueryAsync();
        Console.WriteLine("PostgreSQL database reset by truncating all tables.");
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
        Console.WriteLine("PostgreSQL container stopped and disposed.");
    }
}
