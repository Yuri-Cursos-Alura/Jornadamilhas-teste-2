using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using Microsoft.EntityFrameworkCore;

namespace JornadaMilhas.Test.Integracao.OfertaViagemDal;

public class Adicionar
{
    private readonly JornadaMilhasContext _context;

    public Adicionar()
    {
        var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhas;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
            .Options;

        _context = new JornadaMilhasContext(options);

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
        var dal = new OfertaViagemDAL(_context);

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
}