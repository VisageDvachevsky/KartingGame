using UnityEngine;

namespace Project.RoadGeneration
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private Terrain _terrain;
        [SerializeField] private RoadBuilder _roadBuilder;
        [SerializeField] private float _noiseScale = 0.1f;
        [SerializeField] private float _height = 5;
        [SerializeField] private int _borderSize = 2;

        public void GenerateTerrain()
        {
            int halfBorderSize = (int)_roadBuilder.ChunkSize * _borderSize;
            int resolution = (int)Mathf.Max(_roadBuilder.Width, _roadBuilder.Length) + halfBorderSize * 2;
            //+ halfBorderSize * 2;

            Vector3 offset = new Vector3(halfBorderSize + _roadBuilder.ChunkSize * .5f, 0.05f, halfBorderSize + _roadBuilder.ChunkSize * .5f);
            _terrain.transform.position = _roadBuilder.transform.position
                - offset;


            _terrain.terrainData.size = new Vector3(resolution, _height, resolution);
            _terrain.terrainData.heightmapResolution = resolution;

            int realResolution = _terrain.terrainData.heightmapResolution;
            float[,] heights = new float[realResolution, realResolution];
            for (int i = 0; i < realResolution; i++)
            {
                for (int j = 0; j < realResolution; j++)
                {
                    Vector3 worldPosition = new Vector3(j, 0, i);
                    worldPosition *= (float)resolution / realResolution;
                    worldPosition -= offset;
                    if (_roadBuilder.HasRoadAtPosition(worldPosition))
                        heights[i, j] = 0f;
                    else
                        heights[i, j] = Mathf.PerlinNoise(i * _noiseScale, j * _noiseScale);
                }
            }

            _terrain.terrainData.SetHeights(0, 0, heights); 
        }
    }
}
