namespace Markov.Classes.HMModel
{
	public class Node
	{
		public IList<Line> Lines { get; set; } = new List<Line>();
		public IDictionary<char, double> Values { get; set; } = new Dictionary<char, double>();
	}
}
