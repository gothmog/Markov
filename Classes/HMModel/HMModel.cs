using Accord.Collections;
using MongoDB.Bson;
using MonGothRepository;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Markov.Classes.HMModel
{
    public class HMModel : IBaseEntity
    {
        public IList<Node> Nodes { get; set; }
		public int MaxIterations { get; set; } = 3;
		public ObjectId _id { get; set; }
		public DateTime CreationTime { get; set; }
		public long? UserId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public double GenerateProbality(string sequence, bool isBackGround, int iterationSteps = 0, double defaultBackgroud = 0.25)
		{
			double result = 1;
			for(int i = 0; i < sequence.Length; i++)
			{
				var actualNode = Nodes.Where(x=> !x.IsIteration).ToList()[i];
				if(actualNode.Lines.Count == 2)
				{
					if (iterationSteps > 0)
					{
						result = result * (isBackGround ? defaultBackgroud : actualNode.Values[sequence[i]]);
						result = result * CountIteration(sequence.Substring(i, iterationSteps), isBackGround, defaultBackgroud) * actualNode.Lines.FirstOrDefault(x => x.ToIteration).Weight;
						sequence = sequence.Remove(i + 1, iterationSteps);
					}
					else
					{
						result = result * (isBackGround ? defaultBackgroud : actualNode.Values[sequence[i]]) * actualNode.Lines.FirstOrDefault(x => !x.ToIteration).Weight;
					}
				}
				if(actualNode.Lines.Count == 1)
				{
					result = result * (isBackGround ? defaultBackgroud : actualNode.Values[sequence[i]]) * actualNode.Lines[0].Weight;
				}
				if(actualNode.Lines.Count == 0)
				{
					result = result * (isBackGround ? defaultBackgroud : actualNode.Values[sequence[i]]);
				}
			}
			return result;
		}

		public double CountIteration(string subSequention, bool isBackGround, double defaultBackgroud = 0.25)
		{
			double result = 1;
			var iterNode = this.Nodes.FirstOrDefault(x => x.IsIteration);
			if (iterNode != null)
			{
				var iterLine = iterNode.Lines.FirstOrDefault(x => x.IsIteration);
				for (int i = 0; i < subSequention.Length; i++)
				{
					result = result * (isBackGround ? defaultBackgroud : iterNode.Values[subSequention[i]]);
					if (subSequention.Length > 1 && i < subSequention.Length - 1)
					{
						result = result * iterLine.Weight;
					}
				}
				return result * iterNode.Lines.FirstOrDefault(x => !x.IsIteration).Weight;
			}
			else return 1;
		}

		public void GenerateModel(IList<string> _lines = null)
		{
			this.Nodes = new List<Node>();
			string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Data", "Model.txt");
			IList<string> lines = _lines == null ? File.ReadAllLines(path).ToList() : _lines;
			foreach (string line in lines)
			{
				double? lineOut = null;
				if (line.StartsWith('|'))
				{
					double iterLine = 1;
					Node node = new Node() { IsIteration = true, Values = ParseIterLine(line, ref lineOut, ref iterLine)};
					node.Lines.Add(new Line() { IsIteration = true, Weight = iterLine });
					node.Lines.Add(new Line() { IsIteration = false, Weight = lineOut.Value });
					node.SetMongoVals();
					this.Nodes.Add(node);
				}
				else
				{
					double? iterLine = null;
					bool isToIteration = line.Contains('|');
					Node node = new Node() { Values = ParseLine(line, ref lineOut, ref iterLine) };
					if(lineOut != null)
					{
						node.Lines.Add(new Line() { IsIteration = false, Weight = lineOut.Value });
					}
					if(iterLine != null)
					{
						node.Lines.Add(new Line() { IsIteration = false, ToIteration = true, Weight = iterLine.Value }) ;
					}
					node.SetMongoVals();
					this.Nodes.Add(node);
				}
			}
		}

		private Dictionary<char, double> ParseLine(string line, ref double? lineOut, ref double? iterLine)
		{
			string[] splited = line.Split('/');
			var dict = ParseValues(splited[0]);
			if (splited.Length > 1)
			{
				if (splited[1].Contains('|'))
				{
					string[] lineSplited = splited[1].Split('|');
					lineOut = Double.Parse(lineSplited[1], CultureInfo.InvariantCulture);
					iterLine = Double.Parse(lineSplited[0], CultureInfo.InvariantCulture);
				}
				else lineOut = Double.Parse(splited[1]);
			}
			return dict;
		}

		private Dictionary<char, double> ParseIterLine(string line, ref double? lineOut, ref double iterLine)
		{
			line = line.Trim('|');
			string[] splited = line.Split('/');
			var dict = ParseValues(splited[0]);
			string[] lineSplited = splited[1].Split('|');
			lineOut = Double.Parse(lineSplited[1], CultureInfo.InvariantCulture);
			iterLine = Double.Parse(lineSplited[0], CultureInfo.InvariantCulture);
			return dict;
		}

		private Dictionary<char, double> ParseValues(string valStr)
		{
			var dict = new Dictionary<char, double>();
			string[] vals = valStr.Split(';');
			dict.Add('A', Double.Parse(vals[0], CultureInfo.InvariantCulture));
			dict.Add('C', Double.Parse(vals[1], CultureInfo.InvariantCulture));
			dict.Add('G', Double.Parse(vals[2], CultureInfo.InvariantCulture));
			dict.Add('T', Double.Parse(vals[3], CultureInfo.InvariantCulture));
			return dict;
		}
	}
}
