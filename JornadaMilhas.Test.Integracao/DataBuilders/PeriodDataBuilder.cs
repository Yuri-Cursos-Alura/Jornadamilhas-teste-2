using Bogus;
using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test.Integracao.Fakers;

public class PeriodDataBuilder : Faker<Periodo>
{
    public DateTime? InitialDate { get; set; }
    public DateTime? FinalDate { get; set; }

    public PeriodDataBuilder()
    {
        CustomInstantiator(f =>
            {
                DateTime dataInicio = InitialDate ?? f.Date.Soon();
                DateTime dataFim = FinalDate ?? dataInicio.AddDays(30);
                return new Periodo(dataInicio, dataFim);
            });
    }

    public Periodo Build() => Generate();
}
