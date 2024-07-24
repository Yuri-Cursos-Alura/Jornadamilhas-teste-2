using JornadaMilhas.Dados;
using JornadaMilhas.Test.Integracao.Fixtures;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao.OfertaViagemDAL;

public class RecuperaMaiorDesconto : IDisposable
{
    private readonly JornadaMilhasContext _context;
    private readonly IDbContextTransaction _transaction;

    public RecuperaMaiorDesconto(ITestOutputHelper output, ContextFixture fixture)
    {
        _context = fixture.Context;

        _transaction = _context.Database.BeginTransaction();
    }



    public void Dispose()
    {
        _transaction.Rollback();
        _transaction.Dispose();
    }
}
