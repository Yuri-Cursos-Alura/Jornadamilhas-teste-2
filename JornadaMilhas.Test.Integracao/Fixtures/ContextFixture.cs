using JornadaMilhas.Dados;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace JornadaMilhas.Test.Integracao.Fixtures;

public class ContextFixture : IAsyncLifetime
{
    public JornadaMilhasContext Context { get; private set; }
    private readonly MsSqlContainer _msSQlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    public async Task InitializeAsync()
    {
        await _msSQlContainer.StartAsync();
        var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
            .UseSqlServer(_msSQlContainer.GetConnectionString())
            .Options;

        Context = new JornadaMilhasContext(options);
        Context.Database.Migrate();
    }

    public async Task DisposeAsync()
    {
        await _msSQlContainer.StopAsync();
    }
}
