namespace FootSim.Commands
{
    using FootSim.Core;

    public class WcpPointsCalculator : IPointsCalculator
    {
        public int CalculatePoints(Score predictedScore, Score actualScore)
        {
            if (predictedScore == actualScore)
            {
                return 5;
            }

            if (predictedScore.Result == actualScore.Result)
            {
                return 3;
            }

            if (predictedScore.Home == actualScore.Home || predictedScore.Away == actualScore.Away)
            {
                return 1;
            }

            return 0;
        }
    }
}