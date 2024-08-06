namespace Roads.Models
{
    public struct Way
    {
        public long Id { get; set; }

        public long[] NodeIds { get; set; }

        //public Dictionary<string, string> Tags { get; set; }

        public string? Name { get; set; }

        public string? HighwayType { get; set; }

        public double Distance { get; set; }
    }
}
