using JornadaMilhas.Dados;
using JornadaMilhas.Test.Integracao.Fixtures;
using JornadaMilhasV1.Modelos;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao.OfertaViagemDal;

public class Adicionar : IClassFixture<ContextFixture>, IDisposable
{
    private readonly JornadaMilhasContext _context;
    private readonly IDbContextTransaction _transaction;

    public Adicionar(ITestOutputHelper output, ContextFixture fixture)
    {
        _context = fixture.Context;

        output.WriteLine(_context.GetHashCode().ToString());

        _transaction = _context.Database.BeginTransaction();
    }

    [Fact]
    public void RegistersOfferInDatabaseIfOfferIsValid()
    {
        // Arrange
        var originRoute = "Origin";
        var destinationRoute = "Desination";
        var initialDate = DateTime.Now;
        var finalDate = DateTime.Now.AddDays(30);

        var route = new Rota(originRoute, destinationRoute);
        var period = new Periodo(initialDate, finalDate);
        double price = 350.0;

        var offer = new OfertaViagem(route, period, price);
        var dal = new Dados.OfertaViagemDAL(_context);

        // Act
        dal.Adicionar(offer);

        // Assert
        var includedOffer = dal.RecuperarPorId(offer.Id);

        Assert.NotNull(includedOffer);
        Assert.Same(offer, includedOffer);
        Assert.Equal(originRoute, includedOffer.Rota.Origem);
        Assert.Equal(destinationRoute, includedOffer.Rota.Destino);
        Assert.Equal(initialDate, includedOffer.Periodo.DataInicial);
        Assert.Equal(finalDate, includedOffer.Periodo.DataFinal);
        Assert.Equal(price, includedOffer.Preco);
    }

    public void Dispose()
    {
        _transaction.Rollback();
        _transaction.Dispose();
    }
}