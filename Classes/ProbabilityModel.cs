namespace Markov.Classes
{
	public class ProbabilityModel
	{
		public string Sequence { get; set; }
		public IList<ProbabilitySequence> Probabilities { get; set; }

		public void GenerateProbabiliies()
		{

		}

		public void CountProbalities(HMModel.HMModel model)
		{

		}
	}
}
