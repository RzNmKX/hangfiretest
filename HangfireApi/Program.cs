using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(x => x.UseSqlServerStorage("Data Source=127.0.0.1;Initial Catalog=hangfire;User Id=sa;Password=",
	new SqlServerStorageOptions
	{
		QueuePollInterval = TimeSpan.FromSeconds(1)
	})
.WithJobExpirationTimeout(TimeSpan.FromDays(365)));

//var options = new BackgroundJobServerOptions
//{
//	SchedulePollingInterval = TimeSpan.FromSeconds(1)
//};
//var server = new BackgroundJobServer(options);

builder.Services.AddHangfireServer();

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

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
	StatsPollingInterval = 2000
});

app.Run();
