using JornadaMilhas.Dados;
using JornadaMilhas.Test.Integracao.Fixtures;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao.OfertaViagemDAL;

[Collection(nameof(ContextCollection))]
public class RecuperarPorId : IDisposable
{
    private readonly JornadaMilhasContext _context;
    private readonly IDbContextTransaction _transaction;

    public RecuperarPorId(ContextFixture fixture)
    {
        _context = fixture.Context;

        _transaction = _context.Database.BeginTransaction();
    }

    [Fact]
    public void ReturnsNullIfIdDoesNotExist()
    {
        // Arrange
        var dal = new Dados.OfertaViagemDAL(_context);

        // Act
        var recoveredOffer = dal.RecuperarPorId(-2);

        // Assert
        Assert.Null(recoveredOffer);
    }

    public void Dispose()
    {
        _transaction.Rollback();
        _transaction.Dispose();
    }
}
