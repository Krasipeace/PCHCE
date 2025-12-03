# PCHCE(Personal Computer Hardware Component Evaluator)

This library provides simple evaluation of hardware components, so it can be used for ranking systems, comparing hardware components. It can be used in various applications, such as PC building websites, hardware review platforms etc.

## Table of Contents

- [Installation](#installation)
- [Features](#features)
- [Components](#components)
- [RankEvaluator](#rankevaluator)
- [Usage Examples](#usage-examples)

## Installation

You can install the library via NuGet Package Manager or .NET CLI.

```bash

```

## Features

- Evaluate CPU, GPU, RAM, motherboard, storage devices, pc cases, power supplies, and cooling systems, based on their specifications.
- Lightweight and fast.
- No external dependencies.

## Components

The library includes the following component models in the `Core.Components` namespace. Each class represents a specific hardware component with relevant properties:

- **CPU**: Central Processing Unit (Cores, Threads, Clock Speeds, Cache).
- **GPU**: Graphics Processing Unit (Cores, VRAM Capacity, Frequency, Interface).
- **RAM**: Random Access Memory (Speed, Voltage, Latency, XMP/Expo support).
- **Motherboard**: Main circuit board (PCIe Version, Max Memory Capacity).
- **Storage**: Storage devices (Read/Write Speed, NVMe/SSD/HDD flags).
- **PowerSupply**: Power Supply Unit (Watts, Efficiency).
- **Cooler**: CPU Cooler (RPM, CFM, Air/Liquid).
- **Fan**: Case Fan (RPM, CFM).
- **Case**: PC Case chassis.

## RankEvaluator

The `Core.RankEvaluator` class provides methods to evaluate and rank components on a scale of **0 to 100** based on their basic performance characteristics.

### How it works

The evaluator calculates a score by normalizing key performance metrics against a baseline "high-end" reference value.
- **0**: Represents the lowest possible score.
- **100**: Represents the highest possible score (top-tier performance).

Each component type has a specific ranking method:
- `RankCpu(CPU cpu)`
- `RankGpu(GPU gpu)`
- `RankRam(RAM ram)`
- `RankMotherboard(Motherboard motherboard)`
- `RankStorage(Storage storage)`
- `RankPsu(PowerSupply psu)`
- `RankCooler(Cooler cooler)`
- `RankFan(Fan fan)`

## Usage Examples

Below are examples of how to use the `RankEvaluator` once the library is referenced as a DLL or NuGet package.

<details>
<summary>### 1. Ranking a CPU(click to view example)</summary>

```csharp
var evaluator = new RankEvaluator();
var myCpu = new CPU
{
    Name = "CPU",
    Cores = 16,
    Threads = 32,
    BaseClockGhz = 3.5,
    TurboClockGhz = 5.5,
    CacheSize = 64
};

double cpuScore = evaluator.RankCpu(myCpu);
Console.WriteLine($"CPU Score: {cpuScore:F2} / 100");
```

</details>

<details>
<summary>### 2. Ranking a GPU(click to view example)</summary>

```csharp
var evaluator = new RankEvaluator();
var myGpu = new GPU
{
    Cores = 16384,
    Capacity = 24,
    Frequency = 2500,
    Interface = 384,
    HasCuda = true
};

double gpuScore = evaluator.RankGpu(myGpu);
Console.WriteLine($"GPU Score: {gpuScore:F2}");
```

</details>

<details>
<summary>### 3. Ranking Storage(click to view example)</summary>

```csharp
var evaluator = new RankEvaluator();
var mySsd = new Storage
{
    ReadSpeed = 7000,
    WriteSpeed = 6000,
    IsNVMe = true
};

double storageScore = evaluator.RankStorage(mySsd);
Console.WriteLine($"Storage Score: {storageScore:F2}");
```

</details>

<details>
<summary>### 4. Ranking Motherboard(click to view example)</summary>

```csharp
var evaluator = new RankEvaluator();
var myMotherboard = new Motherboard
{
    Name = "Motherboard",
    PCIeVersion = 5,
    MaxMemoryCapacity = 256
};

double motherboardScore = evaluator.RankMotherboard(myMotherboard);
Console.WriteLine($"Motherboard Score: {motherboardScore:F2}");
```

</details>

<details>
<summary>### 5. Ranking Power Supply[API Call](click to view example)</summary>

```csharp
/// Assumes a POST endpoint at `/api/rank/psu` that accepts a JSON body
/// like `{ "efficiencyRating": "80 Plus Gold", "wattage": 750 }`
/// and returns a JSON response like `{ "score": 85.5 }`.
///
public class PsuRequest
{
    public int Wattage { get; set; }
    public string EfficiencyRating { get; set; }
}

public class PsuResponse
{
    public double Score { get; set; }
}

public async Task RankPsuViaApi()
{
    using var client = new HttpClient();
    client.BaseAddress = new Uri("http://localhost:5000/"); // Replace with your API base URL

    var psuData = new PsuRequest
    {
        Name = "Power Supply",
        EfficiencyRating = 90,
        Wattage = 1050
    };

    var jsonContent = JsonSerializer.Serialize(psuData);
    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
    var response = await client.PostAsync("api/rank/psu", content);
    response.EnsureSuccessStatusCode(); 

    var jsonResponse = await response.Content.ReadAsStringAsync();
    var psuResult = JsonSerializer.Deserialize<PsuResponse>(jsonResponse);

    Console.WriteLine($"API-ranked PSU Score: {psuResult.Score:F2}");
    if (psuResult.Score >= 90) Console.WriteLine("Top-tier!");
}
```

</details>

<details>
<summary>### 6. Ranking Cooler(click to view example)</summary>

```csharp
var evaluator = new RankEvaluator();
var myCooler = new Cooler
{
    IsAir = false,
    RPM = 3000,
    CFM = 150
};

double coolerScore = evaluator.RankCooler(myCooler);
if (coolerScore >= 90) Console.WriteLine("Can FREEZE you!");
```

</details>

<details>
<summary>### 7. Ranking Fan(click to view example)</summary>

```csharp
var evaluator = new RankEvaluator();
var myFan = new Fan
{
    RPM = 2500,
    CFM = 88.6
};

double fanScore = evaluator.RankFan(myFan);
Console.WriteLine($"Fan Score: {fanScore:F2}");
```

</details>
