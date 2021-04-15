using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class NoiseTilemap : MonoBehaviour
{
    [Header("Tile")]

    [Tooltip("Tile to place according to Noise.")]
    public TileBase tile;

    [Tooltip("How many tiles to generate in each direction.")]
    public Vector2Int mapSize; // target map size
    private Vector2Int currentMapSize; // currently created map size

    [Tooltip("Minimum noise value at a tile position required to place a tile.")]
    public float noiseThreshold = 0.5f;
    private Tilemap tilemap;

    private Vector3Int[] tilePositions;
    private TileBase[] tileArray;

    [Header("Noise")]
    public bool randStartingOffset = false;
    public Vector3 offset;
    public bool randStartingRotation = false;
    public Vector3 rotation;

    public float frequency = 1f;

    [Range(1, 8)]
    public int octaves = 1;

    [Range(1f, 4f)]
    public float lacunarity = 2f;

    [Range(0f, 1f)]
    public float persistence = 0.5f;

    [Range(1, 3)]
    public int dimensions = 3;

    public NoiseMethodType type;

    public float[,] perlinSamples;

    private static int OFFSET_RANGE = 25000; // beyond this range, noise gets jagged
    private static int ROTATION_RANGE = 360; // 360 degrees

    private void OnEnable()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }

        if (randStartingOffset)
        {
            RandomizeOffsets();
        }
        if (randStartingRotation)
        {
            RandomizeRotation();
        }

        Refresh();
    }

    public void RandomizeOffsets()
    {
        offset.x = Random.Range(-OFFSET_RANGE, OFFSET_RANGE);
        offset.y = Random.Range(-OFFSET_RANGE, OFFSET_RANGE);
        offset.z = Random.Range(-OFFSET_RANGE, OFFSET_RANGE);
    }

    public void RandomizeRotation()
    {
        rotation.x = Random.Range(0f, ROTATION_RANGE);
        rotation.y = Random.Range(0f, ROTATION_RANGE);
        rotation.z = Random.Range(0f, ROTATION_RANGE);
    }

    public void Refresh()
    {
        if (mapSize.x != currentMapSize.x || mapSize.y != currentMapSize.y)
        {
            // special behaviour for if the bounds of the noisemap changes.
            // currently unused
            currentMapSize.x = mapSize.x;
            currentMapSize.y = mapSize.y;
            tilePositions = GeneratePositionArray(mapSize);
        }

        perlinSamples = GeneratePerlinSamples();
        tileArray = GenerateTiles();

        tilemap.SetTiles(tilePositions, tileArray);
    }

    public float[,] GeneratePerlinSamples()
    {
        float[,] samples = new float[mapSize.x - 1, mapSize.y - 1];
        Quaternion q = Quaternion.Euler(rotation);

        Vector3 point00 = q * new Vector3(-0.5f, -0.5f) + offset;
        Vector3 point10 = q * new Vector3(0.5f, -0.5f) + offset;
        Vector3 point01 = q * new Vector3(-0.5f, 0.5f) + offset;
        Vector3 point11 = q * new Vector3(0.5f, 0.5f) + offset;

        NoiseMethod method = Noise.methods[(int)type][dimensions - 1];
        for (int y = 0; y < samples.Length; y++)
        {
            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f));
            Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f));
            for (int x = 0; x < samples.GetLength(1); x++)
            {
                Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f));
                float sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence);
                if (type != NoiseMethodType.Value)
                {
                    sample = sample * 0.5f + 0.5f;
                }
                //Debug.LogWarning("sample: " + sample + "\tloop:(" + x + "," + y + ")\tlength:(" + samples.GetLength(0) + "," + samples.GetLength(1) + ")");
                samples[y,x] = sample;
            }
        }

        return samples;
    }

    public TileBase[] GenerateTiles()
    {
        TileBase[] tiles = new TileBase[tilePositions.Length];
        int tileCount = 0;
        for (int x = 0; x < perlinSamples.GetLength(0); x++)
        {
            for (int y = 0; y < perlinSamples.GetLength(1); y++)
            {
                if (perlinSamples[x,y] >= noiseThreshold)
                {
                    tiles[tileCount] = tile;
                }
                tileCount++;
            }
        }
        return tiles;
    }

    public static Vector3Int[] GeneratePositionArray(Vector2Int size)
    {
        Vector3Int[] positions = new Vector3Int[size.x * size.y];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Vector3Int(i % size.x, i / size.y, 0);
        }
        return positions;
    }
}
