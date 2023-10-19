using ClashRoyaleApi.Logic.ClanMembers;
using ClashRoyaleApi.Data;
using ClashRoyaleApi.Logic.RiverRace;
using Microsoft.EntityFrameworkCore;
using ClashRoyaleApi.Logic.Authentication;
using ClashRoyaleApi.Logic.CurrentRiverRace;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using ClashRoyaleApi.Logic.EventScheduler;

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
builder.Services.AddScoped<IAuthenticationLogic, AuthenticationLogic>();
builder.Services.AddScoped<ICurrentRiverRace, CurrentRiverRace>();

var app = builder.Build();

ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
IScheduler scheduler = schedulerFactory.GetScheduler().Result;

IJobDetail jobDetail = JobBuilder.Create<TestScheduler>()
    .WithIdentity("SampleJob", "group1")
    .Build();

jobDetail.JobDataMap.Put("param1", 0);

ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity("sampleTrigger", "group1")
    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(8, 0))
    .Build();

scheduler.ScheduleJob(jobDetail, trigger).Wait();
scheduler.Start().Wait();
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
