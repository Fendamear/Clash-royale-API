using ClashRoyaleCodeBase.Logic.ClanMembers;
using ClashRoyaleCodeBase.Data;
using ClashRoyaleCodeBase.Logic.RiverRace;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataContext>(options =>
{
   options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(5, 7, 31)), b => b.MigrationsAssembly("ClashRoyaleApi"));
  
});

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRiverRaceLogic, RiverRaceLogic>();
builder.Services.AddScoped<IClanMemberLogic, ClanMemberLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
