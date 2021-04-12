﻿using System.Collections;
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

    private Tilemap tilemap;

    [Header("Noise")]

    [Range(2, 512)]
    public int resolution = 256;

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
            // reconstructs perlinSamples list when target size is updated
            currentMapSize.x = mapSize.x;
            currentMapSize.y = mapSize.y;
            perlinSamples = new float[mapSize.x - 1, mapSize.y - 1];
        }
    }

    public float[,] GeneratePerlinSamples() // void(?)
    {
        float[,] samples = new float[mapSize.x - 1, mapSize.y - 1]; // remove(?)
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
                samples[y,x] = sample;
            }
        }

        return samples;
    }
}
