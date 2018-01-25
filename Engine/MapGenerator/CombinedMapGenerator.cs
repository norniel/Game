namespace Engine.MapGenerator
{
    public class CombinedMapGenerator
    {
        public float[,] GetCombinedMap(int power, int height, int seedCount)
        {
            var baseMap = new DiamondSquareGenerator().GenerateBaseMap(power, height);
            var voronoyMap = new VoronoyGenerator().GenerateMapWithVoronoyDiagrams(power, seedCount);
            var size = (1 << power)+ 1; 

            var resultMap = new float[size,size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    resultMap[i, j] = (2*baseMap[i, j] + voronoyMap[i, j])/3;
                }
            }

            return resultMap;
        }
    }
}
