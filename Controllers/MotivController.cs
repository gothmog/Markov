using Markov.Classes.HMModel;
using Microsoft.AspNetCore.Mvc;

namespace Markov.Controllers
{
	public class MotivController : Controller
	{
		public IActionResult Index()
		{
			HMModel model = new HMModel();
			model.GenerateModel();
			
			return View();
		}

		private 
	}
}
