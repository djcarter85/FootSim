namespace Randomness.Distributions
{
    public interface IDistribution<out T>
    {
        T Sample();
    }
}