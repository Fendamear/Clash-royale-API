using System.Diagnostics;
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
using ClashRoyaleApi.Logic.CallList;

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

DateTime Timestamp;

if (builder.Environment.IsDevelopment())
{
    Timestamp = DateTime.Now;
}
else
{
    string myVariable = Environment.GetEnvironmentVariable("TimeStamp");
    if (!DateTime.TryParse(myVariable, out Timestamp))
    {
        throw new InvalidDataException();
    }
}

Debug.WriteLine(Timestamp.ToString());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseMySql(builder.Configuration.GetConnectionString("LocalConnection"), new MySqlServerVersion(new Version(5, 7, 31)), b => b.MigrationsAssembly("ClashRoyaleApi"));
    }
    else
    {
        options.UseMySql(builder.Configuration.GetConnectionString("DockerConnection"), new MySqlServerVersion(new Version(5, 7, 31)), b => b.MigrationsAssembly("ClashRoyaleApi"));
    }
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
builder.Services.AddScoped<ICallList, CallListLogic>();

builder.Services.AddQuartz(q =>
{
    var jobKeyCRR = new JobKey("RiverRaceScheduler");
    var jobKeyCI = new JobKey("ClanMemberInfo");

    TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
    DateTime utcTime5MinutesBefore = Timestamp.AddMinutes(-5);
    DateTime utcTime30MinutesBefore = Timestamp.AddMinutes(-30);
    DateTime utcTime1HourBefore = Timestamp.AddMinutes(-60);
    DateTime utcTime2HoursBefore = Timestamp.AddMinutes(-120);
    DateTime utcTime3HoursBefore = Timestamp.AddMinutes(-180);

    q.AddJob<RiverRaceScheduler>(opts => opts.WithIdentity(jobKeyCRR));
    q.AddJob<ClanMemberInfoScheduler>(opts => opts.WithIdentity(jobKeyCI));

    q.AddTrigger(opts => opts.ForJob(jobKeyCI)
    .WithIdentity("ClanMemberInfo-1100")
    .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(11, 0).InTimeZone(localTimeZone)));

    q.AddTrigger(opts => opts.ForJob(jobKeyCI)
        .WithIdentity("ClanMemberInfo-2300")
        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(23, 0).InTimeZone(localTimeZone)));

    //scheduler for current river race

    //5 minutes before
    q.AddTrigger(opts => opts.ForJob(jobKeyCRR).UsingJobData("param", (int)SchedulerTime.MINUTESBEFORE30)
        .WithIdentity("riverRaceScheduler-5Minutes")
        .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(utcTime5MinutesBefore.Hour, utcTime5MinutesBefore.Minute,
                new[] { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday })
            .InTimeZone(localTimeZone)));

    //30 minutes before
    q.AddTrigger(opts => opts.ForJob(jobKeyCRR).UsingJobData("param", (int)SchedulerTime.MINUTESBEFORE5)
        .WithIdentity("riverRaceScheduler-30Minutes")
        .WithSchedule(
            CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(utcTime30MinutesBefore.Hour, utcTime30MinutesBefore.Minute,
                new[] { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday }).InTimeZone(localTimeZone)));

    //1 hour before
    q.AddTrigger(opts => opts.ForJob(jobKeyCRR).UsingJobData("param", (int)SchedulerTime.MINUTESBEFORE60)
        .WithIdentity("riverRaceScheduler-1Hour")
        .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(utcTime1HourBefore.Hour, utcTime1HourBefore.Minute,
                new[] { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday })
        .InTimeZone(localTimeZone)));

    //2 hours before
    q.AddTrigger(opts => opts.ForJob(jobKeyCRR).UsingJobData("param", (int)SchedulerTime.MINUTESBEFORE120)
        .WithIdentity("riverRaceScheduler-2Hour")
        .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(utcTime2HoursBefore.Hour, utcTime2HoursBefore.Minute,
                new[] { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday })
        .InTimeZone(localTimeZone)));

    //3 hours before
    q.AddTrigger(opts => opts.ForJob(jobKeyCRR).UsingJobData("param", (int)SchedulerTime.MINUTESBEFORE180)
        .WithIdentity("riverRaceScheduler-3Hour")
        .WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(utcTime3HoursBefore.Hour, utcTime3HoursBefore.Minute,
                new[] { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday })
        .InTimeZone(localTimeZone)));
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();
    context.Database.Migrate();
}
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();

app.Run();
