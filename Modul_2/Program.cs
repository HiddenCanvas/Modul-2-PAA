using Modul_2.Data;
using Modul_2.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = "Host=localhost;Port=5432;Database=person_db;Username=postgres;Password=ashar";
Console.WriteLine(">>> ConnectionString: " + connectionString);

builder.Services.AddSingleton<SqlDBHelper>(new SqlDBHelper(connectionString));
builder.Services.AddScoped<PersonContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();



