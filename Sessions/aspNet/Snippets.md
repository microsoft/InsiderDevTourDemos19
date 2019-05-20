# Snippets

### Demo 1

To be placed in **Pages/FetchData.razor**

```xml
<BlazorGrid Items="@forecasts" PageSize="2">
    <GridHeader>
        <th>Date</th>
        <th>Temp. (C)</th>
        <th>Temp. (F)</th>
        <th>Summary</th>
    </GridHeader>
    <GridRow>
        <td>@context.Date.ToShortDateString()</td>
        <td>@context.TemperatureC</td>
        <td>@context.TemperatureF</td>
        <td>@context.Summary</td>
    </GridRow>
</BlazorGrid>
```

### Demo 3

To be placed in **Program.cs** of the **BlazorInsider.Worker** project:

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args).
        UseWindowsService()
        .ConfigureServices(services =>
        {
            services.AddHostedService<Worker>();
        });
```

To execute from a command prompt in the **03-Orders\01-Start\BlazorInsider.Worker** folder:

- Build the worker:

    ```powershell
    dotnet build
    ```
    
- Publish the worker:

    ```powershell
    dotnet publish -o C:\OrdersService
    ```

- Create the Windows Service:

    ```powershell
    sc create worker1 binPath=C:\OrdersService\BlazorInsider.Worker.exe
    ```
    
- Start the Windows Service:

    ```powershell
    sc start worker1
    ```