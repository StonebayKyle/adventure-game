using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Tooltip("FollowParticleSystems follow the gameobject. A ParticleSystem is instantiated as a child of the gameobject's transform and emits from the object. A trail effect would be simulated in world space, and a following effect would be simulated in local space.")]
    public ParticleSystem[] followParticleSystems;

    private GameObject[] trailParticleGameObjects;
    private ParticleSystem[] objTrailParticleSystems; // the instantiated trail's particle systems.

    [Tooltip("ParticleSystem that instantiates when the object explodes.")]
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

    }

    public void CreateFollowParticles()
    {
        objTrailParticleSystems = new ParticleSystem[followParticleSystems.Length];
        trailParticleGameObjects = new GameObject[followParticleSystems.Length];
        for (int i = 0; i < followParticleSystems.Length; i++)
        {
            // instantiated as a child to the gameobject's transform.
            objTrailParticleSystems[i] = Instantiate(followParticleSystems[i], transform.position, transform.rotation, transform);
            trailParticleGameObjects[i] = objTrailParticleSystems[i].gameObject;
        }
    }

    public void Explode()
    {
        PersistThenDestroyChildren();
        CreateExplosionParticle();
    }

    private void PersistThenDestroyChildren()
    {
        for (int i = 0; i < followParticleSystems.Length; i++)
        {
            if (trailParticleGameObjects[i] == null) continue;

            // unparents so when parent is destroyed, the child isn't.
            trailParticleGameObjects[i].transform.parent = null;

            // stops particlesystem looping. Assignment to main and emission is required due to interface limitations
            ParticleSystem.MainModule main = objTrailParticleSystems[i].main;
            main.loop = false;
            ParticleSystem.EmissionModule emission = objTrailParticleSystems[i].emission;
            emission.rateOverTime = 0f;

            // destroy the follow GameObject after the particleSystem's duration.
            Destroy(trailParticleGameObjects[i], followParticleSystems[i].main.duration);
        }
    }

    private void CreateExplosionParticle()
    {
        GameObject newExplodeParticle = InstantiateParticleSystem(explodeParticleSystem);
        if (newExplodeParticle != null)
        {
            Destroy(newExplodeParticle, explodeParticleSystem.main.duration);
        }
    }
}
