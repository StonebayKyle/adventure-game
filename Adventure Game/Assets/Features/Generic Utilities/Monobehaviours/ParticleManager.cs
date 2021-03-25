using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem updateParticleSystem;
    public ParticleSystem explodeParticleSystem;

    [System.NonSerialized]
    public float explodeLifetime; // the explode particle's duration. Use to time gameobject destruction

    public void Awake()
    {
        if (explodeParticleSystem != null)
        {
            explodeLifetime = explodeParticleSystem.main.duration;
        }
    }

    private void StartParticleSystem(ParticleSystem particleSystem)
    {
        if (particleSystem == null || particleSystem.isPlaying) return;

        particleSystem.Clear();
        particleSystem.Play();
    }

    private void InstantiateParticleSystem(ParticleSystem particleSystem)
    {
        if (particleSystem == null) return;

        GameObject newParticleSystem = Instantiate(particleSystem, transform.position, transform.rotation).gameObject;
        Destroy(newParticleSystem, particleSystem.main.duration);
    }

    public void UpdateParticle()
    {
        StartParticleSystem(updateParticleSystem);
    }

    public void Explode()
    {
        InstantiateParticleSystem(explodeParticleSystem);
    }
}
