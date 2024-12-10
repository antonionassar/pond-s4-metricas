# Ponderada - Criando Métricas

## Descrição

A aplicação simula a venda de chapéus em uma loja, gerando métricas (contadores) sobre quantos chapéus foram vendidos por segundo. A cada iteração do loop, um contador é incrementado, possibilitando a coleta e análise de dados usando a ferramenta `dotnet-counters`.

Além disso, o projeto mostra exemplos de:

- Métricas multidimensionais (usando tags)
- Implementação direta (sem injeção de dependência)
- Implementação usando injeção de dependência (para facilitar manutenção e testes)

## Pré-requisitos

- [.NET SDK](https://dotnet.microsoft.com/download) instalado
- Ferramenta `dotnet-counters` instalada globalmente:
  ```bash
  dotnet tool install --global dotnet-counters
  ```

## Etapas Realizadas no Projeto

**Configuração do Ambiente:**  
Foi configurado o ambiente com o .NET SDK instalado.

**Criação do Projeto:**  
Um novo projeto foi criado usando:

```bash
dotnet new console -n CreatingMetrics
```

**Instalação de Pacotes Necessários:**  
Foram instalados:

```bash
dotnet add package System.Diagnostics.DiagnosticSource
dotnet tool install --global dotnet-counters
dotnet add package Microsoft.Extensions.DependencyInjection
```

**Implementação Inicial (Sem Injeção de Dependência):**  
No arquivo `Program.cs`, foi criado um `Meter` e um `Counter`. O código original incrementa o contador `hatco.store.hats_sold` a cada segundo.

```csharp
static Meter s_meter = new Meter("HatCo.Store");
static Counter<int> s_hatsSold = s_meter.CreateCounter<int>("hatco.store.hats_sold");

while (true)
{
    Thread.Sleep(1000);
    s_hatsSold.Add(4);
}
```

**Métricas Multidimensionais:**  
As métricas foram ampliadas para incluir dimensões (tags). Por exemplo:

```csharp
s_hatsSold.Add(2,
    KeyValuePair.Create<string,object?>("tipo", "bone"),
    KeyValuePair.Create<string,object?>("cor", "azul"));

s_hatsSold.Add(2,
    KeyValuePair.Create<string,object?>("tipo", "cartola"),
    KeyValuePair.Create<string,object?>("cor", "preto"));
```

Essas tags permitem filtrar e analisar as métricas por tipo ou cor de chapéu.

**Introdução de Injeção de Dependência:**  
Foi criado um serviço `HatMetricsService` que recebe um `Meter` via construtor e cria o contador internamente. Depois, usando `Microsoft.Extensions.DependencyInjection`, o `HatMetricsService` foi registrado e resolvido dentro do `Program.cs`.

```csharp
var services = new ServiceCollection();
services.AddSingleton(new Meter("HatCo.Store"));
services.AddSingleton<HatMetricsService>();
var provider = services.BuildServiceProvider();

var hatMetricsService = provider.GetRequiredService<HatMetricsService>();
hatMetricsService.RecordSale(2, "bone", "azul");
hatMetricsService.RecordSale(2, "cartola", "preto");
```

Assim, o código fica mais organizado e facilmente testável.

## Como Executar

**Rodar a Aplicação:**

```bash
dotnet run
```

Deixe a aplicação rodando em um terminal.

**Monitorar as Métricas com dotnet-counters:**
Em outro terminal, verifique o PID do processo:

```bash
dotnet-counters ps
```

Em seguida, monitore as métricas:

```bash
dotnet-counters monitor -p <PID> --all
```

Ou filtre pelo nome do `Meter`:

```bash
dotnet-counters monitor -p <PID> --counters HatCo.Store
```

## Prints de Execução:

**Setup do ambiente:**

![Execução do dotnet run](/imgs/pond-s4-1.png)
![Execução do dotnet run](/imgs/pond-s4-2.png)
![Execução do dotnet run](/imgs/pond-s4-3.png)

## Monitorando Métricas com dotnet-counters monitor:

![Execução do dotnet run](/imgs/pond-s4-4.png)
![Execução do dotnet run](/imgs/pond-s4-5.png)
![Execução do dotnet run](/imgs/pond-s4-6.png)
![Execução do dotnet run](/imgs/pond-s4-7.png)
![Execução do dotnet run](/imgs/pond-s4-8.png)
![Execução do dotnet run](/imgs/pond-s4-10.png)
![Execução do dotnet run](/imgs/pond-s4-9.png)
