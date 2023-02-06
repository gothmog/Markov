using Markov.Classes;
using Markov.Classes.HMModel;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
			var models = _modelService.GetCollection(x => true);
			return View(models);
		}

		public IActionResult ImportModel()
		{
			HMModel model = new HMModel();
			model.Name = "HMModel - úkol 7";
			model.Description = "Skryté Markovovy modely II (hledání vzorů v sekvencích)";
			model.GenerateModel();
			_modelService.AddItemAsync(model);
			return RedirectToAction("Index");
		}

		public IActionResult AddModel()
		{
			return View();
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

		public IActionResult CountModel(string id)
		{
			var model = _modelService.GetItem(x => x._id == ObjectId.Parse(id));
			return View(model);
		}

		public IList<string> Count(string sequention, string modelId)
		{
			var model = _modelService.GetItem(x => x._id == ObjectId.Parse(modelId));
			foreach(var node in model.Nodes) { node.SetVals(); }
			var probalityModel = new ProbabilityModel()
			{
				Probabilities = new List<ProbabilitySequence>(),
				Sequence = sequention,
				HMModel = model
			};
			probalityModel.GenerateProbabiliies();
			probalityModel.CountProbalities();
			var res = probalityModel.Probabilities.Where(x => x.Probability > x.BackgroudProbalitiy).ToList();
			IList<string> strResults = res.Select(x => GenerateResult(x, probalityModel.Sequence.Length)).ToList();
			return strResults;
		}

		private string GenerateResult(ProbabilitySequence seqv, int length)
		{
			string result = "";
			for(int i= 0; i < seqv.Position; i++)
			{
				result = result + "B";
			}
			for(int i = seqv.Position; i< seqv.Position + seqv.Sequence.Length; i++)
			{
				result = result + "M";
			}
			for (int i = seqv.Position + seqv.Sequence.Length; i < length; i++)
			{
				result = result + "B";
			}
			return result;
		}
	}
}
