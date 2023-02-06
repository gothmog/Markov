using Markov.Classes;
using Markov.Classes.HMModel;
using Microsoft.AspNetCore.Mvc;

namespace Markov.Controllers
{
	public class MotivController : Controller
	{
		public IActionResult Index()
		{
			HMModel model = new HMModel() { MaxIterations = 3 };
			model.GenerateModel();
			var probalityModel = new ProbabilityModel() 
			{ 
				Probabilities = new List<ProbabilitySequence>(), 
				Sequence = "AGATCCATTGACCGTTACACATCAGATTGATAGATTGATTTTGATCGACAAAGTG",
				HMModel = model
			};
			probalityModel.GenerateProbabiliies();
			probalityModel.CountProbalities();
			var res = probalityModel.Probabilities.Where(x => x.Probability > x.BackgroudProbalitiy).ToList();
			return View();
		}
	}
}
