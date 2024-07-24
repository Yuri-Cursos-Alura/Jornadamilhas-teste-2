using Bogus;
using JornadaMilhas.Dados;
using JornadaMilhas.Test.Integracao.Fakers;
using JornadaMilhasV1.Modelos;
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

    public void CreateFakeData()
    {
        var periodFaker = new PeriodDataBuilder();

        var rota = new Rota("Curitiba", "São Paulo");

        var fakerOferta = new Faker<OfertaViagem>()
            .CustomInstantiator(f => new OfertaViagem(
                rota,
                periodFaker.Generate(),
                100 * f.Random.Int(1, 100))
            )
            .RuleFor(o => o.Desconto, f => 40)
            .RuleFor(o => o.Ativa, f => true);

        var lista = fakerOferta.Generate(200);

        Context.OfertasViagem.AddRange(lista);
        Context.SaveChanges();
    }
}
