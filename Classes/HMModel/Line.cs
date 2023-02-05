using MongoDB.Bson;

namespace Markov.Classes.HMModel
{
	public class Line
	{
		public bool IsIteration { get; set; }
		public bool ToIteration { get; set; }
		public double Weight { get; set; }
	}
}
