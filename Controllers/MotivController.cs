using Microsoft.AspNetCore.Mvc;

namespace Markov.Controllers
{
	public class MotivController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
