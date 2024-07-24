using Bogus;
using JornadaMilhas.Dados;
using JornadaMilhas.Test.Integracao.Fakers;
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

    public ContextFixture Fixture { get; }

    public RecuperaMaiorDesconto(ContextFixture fixture)
    {
        _context = fixture.Context;
        _transaction = _context.Database.BeginTransaction();
        Fixture = fixture;
    }

    [Fact]
    // destino = são paulo, desconto = 40, preco = 80
    public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto40()
    {
        //arrange
        Fixture.CreateFakeData();
        var rota = new Rota("Curitiba", "São Paulo");
        var periodo = new PeriodDataBuilder() { InitialDate = DateTime.Now }.Build();

        var ofertaEscolhida = new OfertaViagem(rota, periodo, 80)
        {
            Desconto = 40,
            Ativa = true
        };

        var dal = new Dados.OfertaViagemDAL(_context);
        dal.Adicionar(ofertaEscolhida);

        Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
        var precoEsperado = 40;

        //act
        var oferta = dal.RecuperaMaiorDesconto(filtro);

        //assert
        Assert.NotNull(oferta);
        Assert.Equal(precoEsperado, oferta.Preco, 0.0001);
    }

    [Fact]
    public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto60()
    {
        //arrange
        Fixture.CreateFakeData();
        var rota = new Rota("Curitiba", "São Paulo");
        var periodo = new PeriodDataBuilder() { InitialDate = DateTime.Now }.Build();

        var ofertaEscolhida = new OfertaViagem(rota, periodo, 80)
        {
            Desconto = 60,
            Ativa = true
        };

        var dal = new Dados.OfertaViagemDAL(_context);
        dal.Adicionar(ofertaEscolhida);

        Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
        var precoEsperado = 20;

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
