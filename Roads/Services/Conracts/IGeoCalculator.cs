namespace Roads.Services.Conracts
{
    public interface IGeoCalculator
    {
        double GaussLengthForMidLatitudeShortLines(double phi1, double lambda1, double phi2, double lambda2);
    }
}
