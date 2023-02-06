using MongoDB.Bson.Serialization.Attributes;
using System.Globalization;

namespace Markov.Classes.HMModel
{
	public class Node
	{
		public void SetMongoVals()
		{
			ValuesMongo = new Dictionary<string, string>();
			if (Values != null)
			{
				foreach (var kv in Values)
				{
					ValuesMongo.Add(kv.Key.ToString(), kv.Value.ToString());
				}
			}
		}

		public void SetVals()
		{
			Values = new Dictionary<char, double>();
			if (Values != null)
			{
				foreach (var kv in ValuesMongo)
				{
					Values.Add(kv.Key[0], Double.Parse(kv.Value, CultureInfo.InvariantCulture));
				}
			}
		}

		public bool IsIteration { get; set; }
		public IList<Line> Lines { get; set; } = new List<Line>();
		[BsonIgnore]
		public IDictionary<char, double> Values { get; set; } = new Dictionary<char, double>();
		public IDictionary<string, string> ValuesMongo { get; set; } 
	}
}
