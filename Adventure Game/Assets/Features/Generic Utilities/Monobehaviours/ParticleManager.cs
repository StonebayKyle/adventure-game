using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem updateParticleSystem;
    private GameObject updateParticleGameObject;

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

    private GameObject InstantiateParticleSystem(ParticleSystem particleSystem)
    {
        if (particleSystem == null) return null;

        return Instantiate(particleSystem, transform.position, transform.rotation).gameObject;
    }

    public void UpdateParticle()
    {
        if (updateParticleSystem != null && updateParticleGameObject == null)
        {
            updateParticleGameObject = Instantiate(updateParticleSystem, transform.position, transform.rotation, transform).gameObject;
        }
    }

    public void Explode()
    {
        GameObject newExplodeParticle = InstantiateParticleSystem(explodeParticleSystem);
        if (newExplodeParticle != null)
        {
            Destroy(newExplodeParticle, explodeParticleSystem.main.duration);
        }
    }
}
