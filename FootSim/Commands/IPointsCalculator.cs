namespace FootSim.Commands
{
    using FootSim.Core;

    interface IPointsCalculator
    {
        int CalculatePoints(Score predictedScore, Score actualScore);
    }
}