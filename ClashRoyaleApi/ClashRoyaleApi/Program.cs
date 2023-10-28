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
using static ClashRoyaleApi.Models.EnumClass;
using Quartz.Spi;
using ClashRoyaleApi.Logic.Logging;
using System.Text.Json.Serialization;
using ClashRoyaleApi.Logic.RoyaleApi;
using ClashRoyaleApi.Logic.MailHandler;
using ClashRoyaleApi.Logic.MailHandler.MailSubscription;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                      });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataContext>(options =>
{
   options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(5, 7, 31)), b => b.MigrationsAssembly("ClashRoyaleApi"));
  
});

builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddScoped<IRiverRaceLogic, RiverRaceLogic>();
builder.Services.AddScoped<IClanMemberLogic, ClanMemberLogic>();
builder.Services.AddScoped<IAuthenticationLogic, AuthenticationLogic>();
builder.Services.AddScoped<ICurrentRiverRace, CurrentRiverRaceLogic>();
builder.Services.AddScoped<ICrLogger, CrLogger>();  
builder.Services.AddScoped<IJob, RiverRaceScheduler>();
builder.Services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();
builder.Services.AddScoped<HttpClient, HttpClient>();
builder.Services.AddScoped<IMailHandler, MailHandlerLogic>();
builder.Services.AddScoped<IMailSubscription, MailSubscriptionLogic>();
//builder.Services.AddHttpClient<HttpClient>();



builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("RiverRaceSchedular");

    q.AddJob<RiverRaceScheduler>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts.ForJob(jobKey).UsingJobData("param", (int)SchedulerTime.SCHEDULE1030)
    .WithIdentity("riverRaceSchedular-1030")
    .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(11, 25,
        new[] { DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday }
    )));

    q.AddTrigger(opts => opts.ForJob(jobKey).UsingJobData("param", (int)SchedulerTime.SCHEDULE1130)
    .WithIdentity("riverRaceSchedular-1130")
    .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(21, 28,
        new[] { DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday }
    )));
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();

app.Run();
