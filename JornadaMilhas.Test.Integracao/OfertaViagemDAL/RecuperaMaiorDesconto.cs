using Bogus;
using JornadaMilhas.Dados;
using JornadaMilhas.Test.Integracao.Fixtures;
using JornadaMilhasV1.Modelos;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao.OfertaViagemDAL;

[Collection(nameof(ContextCollection))]
public class RecuperaMaiorDesconto : IDisposable
{
    private readonly JornadaMilhasContext _context;
    private readonly IDbContextTransaction _transaction;

    public RecuperaMaiorDesconto(ITestOutputHelper output, ContextFixture fixture)
    {
        _context = fixture.Context;

        _transaction = _context.Database.BeginTransaction();
    }

    [Fact]
    // destino = são paulo, desconto = 40, preco = 80
    public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto40()
    {
        //arrange
        var fakerPeriodo = new Faker<Periodo>()
            .CustomInstantiator(f =>
            {
                DateTime dataInicio = f.Date.Soon();
                return new Periodo(dataInicio, dataInicio.AddDays(30));
            });

        var rota = new Rota("Curitiba", "São Paulo");

        var fakerOferta = new Faker<OfertaViagem>()
            .CustomInstantiator(f => new OfertaViagem(
                rota,
                fakerPeriodo.Generate(),
                100 * f.Random.Int(1, 100))
            )
            .RuleFor(o => o.Desconto, f => 40)
            .RuleFor(o => o.Ativa, f => true);

        var ofertaEscolhida = new OfertaViagem(rota, fakerPeriodo.Generate(), 80)
        {
            Desconto = 40,
            Ativa = true
        };

        var ofertaInativa = new OfertaViagem(rota, fakerPeriodo.Generate(), 70)
        {
            Desconto = 40,
            Ativa = false
        };

        var dal = new Dados.OfertaViagemDAL(_context);
        var lista = fakerOferta.Generate(200);
        lista.Add(ofertaEscolhida);
        lista.Add(ofertaInativa);

        _context.OfertasViagem.AddRange(lista);
        _context.SaveChanges();

        Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
        var precoEsperado = 40;

        //act
        var oferta = dal.RecuperaMaiorDesconto(filtro);

        //assert
        Assert.NotNull(oferta);
        Assert.Equal(precoEsperado, oferta.Preco, 0.0001);
    }

    public void Dispose()
    {
        _transaction.Rollback();
        _transaction.Dispose();
    }
}
