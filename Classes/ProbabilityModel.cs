using Markov.Classes.HMModel;

namespace Markov.Classes
{
	public class ProbabilityModel
	{
		public string Sequence { get; set; }
		public string Result { get; set; }
		public IList<ProbabilitySequence> Probabilities { get; set; }
		public HMModel.HMModel HMModel { get; set; }

		public void GenerateProbabiliies()
		{
			
			for(int i = 0; i < HMModel.MaxIterations; i++)
			{
				int seqLenght = HMModel.Nodes.Where(x => !(x is IterationNode)).Count() + i;
				int index = 0;
				while (index + seqLenght < Sequence.Length)
				{
					var seq = new ProbabilitySequence();
					seq.Sequence = Sequence.Substring(index, seqLenght);
					seq.IterationCount = i;
					seq.Position = index;
					Probabilities.Add(seq);
					index++;
				}
			}
		}

		public void CountProbalities()
		{
			foreach(var probability in Probabilities)
			{
				probability.Probability = HMModel.GenerateProbality(probability.Sequence, false, probability.IterationCount);	
				probability.BackgroudProbalitiy = HMModel.GenerateProbality(probability.Sequence, true, probability.IterationCount, 0.25);
			}
		}
	}
}
