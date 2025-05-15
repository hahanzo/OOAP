using Task.Strategy;

namespace Player.Strategy
{
    public class RandomStrategy : INextTrackStrategy
    {
        public string GetNext(List<string> playlist, string current)
        {
            Random rnd = new Random();
            int myPosition = playlist.IndexOf(current);
            int randomIndex;

            int[] possibleIndices = new int[playlist.Count];
            int j = 0;
            for (int i = 0; i < playlist.Count; i++)
            {
                if (i != myPosition)
                {
                    possibleIndices[j++] = i;
                }
            }

            randomIndex = possibleIndices[rnd.Next(0, possibleIndices.Length)];

            return playlist[randomIndex];
        }
    }
}
