﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Laser : MonoBehaviour
{
    [Tooltip("How far the explosion destroys destructible tiles from the collision point.")]
    public int destructionRadius = 3;

    [Tooltip("How much time, in seconds, it takes for the Laser to destroy itself after creation. The player isn't meant to notice this, as it is just an optimization for reducing active objects (think: despawning).")]
    public float lifetime = 10f;

    [Header("Sound")]
    [Tooltip("AudioClip that plays when the laser is created.")]
    public AudioClip creationSound;
    [Tooltip("AudioClip that plays when the laser collides with the environment and destroys itself.")]
    public AudioClip collisionSound;
    public float soundVolume = 1f;
    [Tooltip("Percentage of soundVolume to play collisionSound at")]
    [Range(0,1)]
    public float collisionSoundVolumeScale = 1f;


    private Grid grid;
    private Tilemap destructibleTilemap;

    private Vector3 hitPosition;

    private void Awake()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();

        destructibleTilemap = GameObject.Find("Destructible").GetComponent<Tilemap>();
    }

    private void Start()
    {
        PlaySound(creationSound, soundVolume);
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlaySound(collisionSound, soundVolume*collisionSoundVolumeScale);
        //Debug.LogWarning("Collision");
        HandleDestruction(collision);

        Destroy(gameObject);
    }

    private void HandleDestruction(Collision2D collision)
    {
        if (grid == null || destructibleTilemap == null)
        {
            Debug.LogWarning("Grid or destructibleTilemap is null!");
            return;
        }
        
        foreach (ContactPoint2D hit in collision.contacts)
        {
            hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
            hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
            
            //Debug.LogWarning("HitPosition: " + hitPosition);
            Vector3Int tilePosition = destructibleTilemap.WorldToCell(hitPosition);
            //Debug.LogWarning("tilePosition: " + tilePosition);
            ExplodeNearbyCells(tilePosition, destructionRadius);
        }
    }

    // recursive
    private void ExplodeNearbyCells(Vector3Int position, int radius)
    {
        //Debug.LogWarning("Radius: " + radius);
        if (radius < 1)
        {
            return;
        }

        if (radius == 1)
        {
            ExplodeCell(position);
            return;
        }

        radius--;

        ExplodeCell(position);
        ExplodeNearbyCells(position + new Vector3Int(-1, 0, 0), radius);
        ExplodeNearbyCells(position + new Vector3Int(1, 0, 0), radius);
        ExplodeNearbyCells(position + new Vector3Int(0, -1, 0), radius);
        ExplodeNearbyCells(position + new Vector3Int(0, 1, 0), radius);
    }


    private void ExplodeCell(Vector3Int position)
    {
        destructibleTilemap.SetTile(position, null);
    }

    private void PlaySound(AudioClip audioClip, float volume)
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume);
    }

    public void OnDrawGizmos()
    {
        // Currently doesn't show collision because once the collision happens the object gets destroyed.
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitPosition, .2f);
    }
}
