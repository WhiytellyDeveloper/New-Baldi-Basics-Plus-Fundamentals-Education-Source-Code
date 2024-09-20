
namespace nbbpfe.FundamentalsManager
{
    public static class FundamentalLoaderManager
    {
        public class FloorData(string floor = "None")
        {
            public string Floor => _floor;
            readonly string _floor = floor;
        }
    }
}
