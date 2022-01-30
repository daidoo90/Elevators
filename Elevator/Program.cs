using Elevator.Extensions;
using Elevator.Services;
using Elevator.Services.StateMachine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

builder.Services.AddSingleton<IElevatorsStateMachine, ElevatorsStateMachine>();
builder.Services.AddScoped<IElevatorManager, ElevatorManager>();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Elevator}/{action=GetStatuses}/{id?}");

app.AddElevator();

app.AddElevator();

app.Run();
