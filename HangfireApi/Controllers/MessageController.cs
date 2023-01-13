using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Mvc;

namespace HangfireApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MessageController : Controller
	{
		[HttpGet]
		[Route("index")]
		public IActionResult Index()
		{
			var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Welcome to Shopping World!"));
			return Ok($"Job ID: {jobId}. Welcome mail sent to the user!");
			//return Ok("message index");

		}

		[HttpGet]
		[Route("message")]
		public IActionResult CallPrintMessage(string m)
		{
			string message = HttpContext.Request.Query["m"].ToString();
			PrintMessage(message);
			return Ok("message is " + message);
		}

		[HttpGet]
		[Route("delayed")]
		public IActionResult delay(string m)
		{
			string message = m;
			var jobId = BackgroundJob.Schedule(() => Console.WriteLine("delayed message: " + m),TimeSpan.FromSeconds(5));
			Console.WriteLine($"delayed job queued with id: {jobId}");
			return Ok(jobId);
		}

		[HttpGet]
		[Route("minutely")]
		public String DailyOffers()
		{
			//Recurring Job - this job is executed many times on the specified cron schedule
			RecurringJob.AddOrUpdate("minute_job", () => MinutePrint("some message here") , Cron.Minutely);

			return "offer sent!";
		}

		public void MinutePrint(string m)
		{
			Console.WriteLine("minute job at: " + DateTime.Now.ToString() + " Sent similar product offer and suuggestions");
		}

		internal void PrintMessage(string message)
		{ Console.WriteLine(message); }
	}
}
