namespace Markov.Classes
{
	public class ProbabilitySequence
	{
		public string Sequence { get; set; }
		public double Probability { get; set; }
		public double BackgroudProbalitiy { get; set; }
		public double BackgroundValue { get; set; } = 0.25;
		public int IterationCount { get; set; }
	}
}
