namespace GerPros_Backend_API.Application.FunctionalTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync()
    {
        //var database = new TestcontainersTestDatabase();
        var database = new PostgreSqlContainerTestDatabase();
        
        await database.InitialiseAsync();

        return database;
    }
}
