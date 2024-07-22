using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test.Integracao.OfertaViagemDal;

public class Adicionar
{
    [Fact]
    public void RegistersOfferInDatabaseIfOfferIsValid()
    {
        // Arrange
        var route = new Rota("Irrelevant", "Irrelevant");
        var period = new Periodo(DateTime.Now, DateTime.Now);
        double price = 350.0;

        var offer = new OfertaViagem(route, period, price);
        var dal = new OfertaViagemDAL();

        // Act
        dal.Adicionar(offer);

        // Assert
        var includedOffer = dal.RecuperarPorId(offer.Id);

        Assert.NotNull(includedOffer);
        Assert.Same(offer, includedOffer);
    }
}