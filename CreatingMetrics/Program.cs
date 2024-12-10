using System;
using System.Diagnostics.Metrics;
using System.Threading;

class Program
{
    // Criação do meter e do contador
    static Meter s_meter = new Meter("HatCo.Store");
    // Agora temos um contador de chapéus vendidos
    // Podemos adicionar tags (dimensões) a cada adição
    static Counter<int> s_hatsSold = s_meter.CreateCounter<int>("hatco.store.hats_sold");

    static void Main(string[] args)
    {
        Console.WriteLine("Pressione qualquer tecla para sair");

        // Suponha que existam dois tipos de chapéus: "bone" e "cartola".
        // A cada segundo vendemos 2 bonés e 2 cartolas, por exemplo.
        while (!Console.KeyAvailable)
        {
            Thread.Sleep(1000);
            
            // Adicionando métricas com tags
            // Tags: tipo de chapéu e cor, por exemplo
            s_hatsSold.Add(2, 
                KeyValuePair.Create<string, object?>("tipo", "bone"),
                KeyValuePair.Create<string, object?>("cor", "azul"));

            s_hatsSold.Add(2, 
                KeyValuePair.Create<string, object?>("tipo", "cartola"),
                KeyValuePair.Create<string, object?>("cor", "preto"));
        }
    }
}
