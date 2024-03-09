using SAF.ChatX.Hubs;
using SAF.ChatX.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHubs();
builder.Services.AddCors(option =>
{
    option.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:9000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddPersistence();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseHubs();
app.UseCors("Frontend");

app.Run();
