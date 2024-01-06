using eShop.Backend.Extensions;
using eShop.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomCors(builder.Configuration);
builder.Services.AddScoped<CarduriService>();
builder.Services.AddScoped<ProduseService>();
builder.Services.AddScoped<PozitiiComenziService>();
builder.Services.AddScoped<ClientiService>();
builder.Services.AddScoped<ComenziService>();
builder.Services.AddScoped<AdreseService>();

builder.Services.AddSql(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
