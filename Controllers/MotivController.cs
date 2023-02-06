using Markov.Classes;
using Markov.Classes.HMModel;
using Microsoft.AspNetCore.Mvc;
using MonGothRepository;

namespace Markov.Controllers
{
	public class MotivController : Controller
	{
		public MotivController(IMongoRepositoryProvider<HMModel> modelService)
		{
			_modelService = modelService;
		}

		IMongoRepositoryProvider<HMModel> _modelService { get; set; }
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult ImportModel()
		{
			//HMModel model = new HMModel();
			//model.Name = "HMModel - úkol 7";
			//model.Description = "Skryté Markovovy modely II (hledání vzorů v sekvencích)";
			//model.GenerateModel();
			//_modelService.AddItemAsync(model);
			return RedirectToAction("Index");
		}

		public bool SaveModel(string modelName, string modelDesc, string[] lines)
		{
			HMModel model = new HMModel();
			model.Name = modelName;
			model.Description = modelDesc;
			model.GenerateModel(lines.ToList());
			//_modelService.AddItemAsync(model);
			return true;
		}

		public IActionResult Count()
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
