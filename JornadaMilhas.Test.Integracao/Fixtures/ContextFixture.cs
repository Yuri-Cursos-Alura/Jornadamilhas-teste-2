using JornadaMilhas.Dados;
using Microsoft.EntityFrameworkCore;

namespace JornadaMilhas.Test.Integracao.Fixtures;

public class ContextFixture
{
    public JornadaMilhasContext Context { get; }

    public ContextFixture()
    {
        var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhas;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
            .Options;

        Context = new JornadaMilhasContext(options);
    }
}
