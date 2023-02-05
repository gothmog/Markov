using Accord.Statistics.Models.Markov;
using Accord.Statistics.Models.Markov.Learning;

namespace Markov.Classes
{
    public class Computation
    {
        public void Compute()
        {
            // Create a model with given probabilities
            //P = 0, M = 1
            HiddenMarkovModel hmm = new HiddenMarkovModel(
                transitions: new[,] // matrix A
                {
        { 0.8, 0.2 },
        { 0.2, 0.8 }
                },
                emissions: new[,] // matrix B
                {
        { 0.75, 0.25 },
        { 0.92, 0.08 }
                },
                initial: new[]  // vector pi
                {
        0.5, 0.5
                });

            // Create an observation sequence of up to 2 symbols (0 or 1)
            int[] observationSequence = new[] { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1 };

            // Decode the sequence: the path will be 1-1-1-1-2-0-1
            int[] stateSequence = hmm.Decode(observationSequence);
            string ret = "";
            foreach (int i in stateSequence)
            {
                ret += i == 0 ? "P" : "M";
            }
        }

        public void Compute2()
        {
            string seqString = "AGATCCATTGACCGTTACACATCAGATTGATAGATTGATTTTGATCGACAAAGTG";
            IList<int> seq = new List<int>();
            foreach(char ch in seqString)
			{
				switch (ch)
				{
                    case 'A': seq.Add(0); break;
                    case 'C': seq.Add(1); break;
                    case 'G': seq.Add(2); break;
                    case 'T': seq.Add(3); break;
                }
			}
            //A=0, C=1, G=2, T=3, -=4 

            int[][] inputSequences =
            {
    new[] { 0, 1, 0, 4, 4, 4, 0, 3, 2 },
    new[] { 0, 1, 0, 1, 4, 4, 1, 2, 1 },
    new[] { 0, 2, 0, 4, 4, 4, 0, 3, 1 },
    new[] { 0, 1, 1, 2, 4, 4, 0, 3, 1 },
};
            // Now we create a hidden Markov model with arbitrary probabilities
            HiddenMarkovModel hmm = new HiddenMarkovModel(
                transitions: new[,]
                {
            { 0, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0.6, 0.4, 0, 0 },
            { 0, 0, 0, 0.4, 0.6, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 0 },
            { 0, 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 0, 0, 1 },
                },
                emissions: new[,]
                {

            { 0.8, 0, 0, 0.2 },
            { 0, 0.8, 0.2, 0 },
            { 0.8, 0.2, 0, 0 },
            { 0.2, 0.4, 0.2, 0.2 },
            { 1.0, 0, 0, 0 },
            { 0, 0, 0.2, 0.8 },
            { 0, 0.8, 0.2, 0 }
                },
                initial: new[] { 1, 0.0, 0, 0, 0, 0, 0 }

                );

            int[] observationSequence = seq.ToArray();
            var res = hmm.Decode(observationSequence);

            // Decode the sequence: the path will be 1-1-1-1-2-0-1
            foreach (var s in inputSequences)
			{
                var eval = hmm.Evaluate(s);
                var result = hmm.Decode(s);
			}
        }
    }
}
