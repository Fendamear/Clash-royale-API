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
    var jobKeyCRR = new JobKey("RiverRaceScheduler");
    var jobKeyCI = new JobKey("ClanMemberInfo");

    TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
    DateTime utcTime5MinutesBefore = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 30, 0), localTimeZone);
    DateTime utcTime30MinutesBefore = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0), localTimeZone);
    DateTime utcTime1HourBefore = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 30, 0), localTimeZone);
    DateTime utcTime2HoursBefore = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 30, 0), localTimeZone);
    DateTime utcTime3HoursBefore = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 30, 0), localTimeZone);

    q.AddJob<RiverRaceScheduler>(opts => opts.WithIdentity(jobKeyCRR));
    q.AddJob<ClanMemberInfoScheduler>(opts => opts.WithIdentity(jobKeyCI));

    q.AddTrigger(opts => opts.ForJob(jobKeyCI)
    .WithIdentity("ClanMemberInfo-1030")
    .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(18, utcTime5MinutesBefore.Minute,
        new[] { DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday }).InTimeZone(localTimeZone)));

    //scheduler for current river race

    q.AddTrigger(opts => opts.ForJob(jobKeyCRR).UsingJobData("param", (int)SchedulerTime.MINUTESBEFORE5)
    .WithIdentity("riverRaceSchedelar-0930")
    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(utcTime5MinutesBefore.Hour, utcTime5MinutesBefore.Minute)
    .InTimeZone(localTimeZone)));

    q.AddTrigger(opts => opts.ForJob(jobKeyCRR).UsingJobData("param", (int)SchedulerTime.MINUTESBEFORE30)
        .WithIdentity("riverRaceSchedelar-0900")
        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(utcTime30MinutesBefore.Hour, utcTime30MinutesBefore.Minute)
        .InTimeZone(localTimeZone)));

    q.AddTrigger(opts => opts.ForJob(jobKeyCRR).UsingJobData("param", (int)SchedulerTime.MINUTESBEFORE60)
        .WithIdentity("riverRaceSchedelar-0830")
        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(utcTime1HourBefore.Hour, utcTime1HourBefore.Minute)
        .InTimeZone(localTimeZone)));

    q.AddTrigger(opts => opts.ForJob(jobKeyCRR).UsingJobData("param", (int)SchedulerTime.MINUTESBEFORE120)
        .WithIdentity("riverRaceSchedelar-0730")
        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(utcTime2HoursBefore.Hour, utcTime2HoursBefore.Minute)
        .InTimeZone(localTimeZone)));

    q.AddTrigger(opts => opts.ForJob(jobKeyCRR).UsingJobData("param", (int)SchedulerTime.MINUTESBEFORE180)
        .WithIdentity("riverRaceSchedelar-0630")
        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(utcTime3HoursBefore.Hour, utcTime3HoursBefore.Minute)
        .InTimeZone(localTimeZone)));
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
