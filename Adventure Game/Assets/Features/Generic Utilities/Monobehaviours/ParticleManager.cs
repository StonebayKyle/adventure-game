using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Tooltip("A trail ParticleSystem. ParticleSystem is instantiated as a child of the gameobject's transform and emits from the object. A trail effect would be simulated in world space, and a following effect would be simulated in local space.")]
    public ParticleSystem followParticleSystem;

    private GameObject trailParticleGameObject;
    private ParticleSystem objTrailParticleSystem; // the instantiated trail's particle system.

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

    public void CreateFollowParticle()
    {
        trailParticleGameObject = Instantiate(followParticleSystem, transform.position, transform.rotation, transform).gameObject;
        objTrailParticleSystem = trailParticleGameObject.GetComponent<ParticleSystem>();
    }

    public void Explode()
    {
        PersistThenDestroyChild();
        CreateExplosionParticle();
    }

    private void PersistThenDestroyChild()
    {
        if (trailParticleGameObject == null) return;

        // unparents so when parent is destroyed, the child isn't.
        trailParticleGameObject.transform.parent = null;

        // stops particlesystem looping. Assignment to main and emission is required due to interface limitations
        ParticleSystem.MainModule main = objTrailParticleSystem.main;
        main.loop = false;
        ParticleSystem.EmissionModule emission = objTrailParticleSystem.emission;
        emission.rateOverTime = 0f;

        // destroy the trail GameObject after the particleSystem's duration.
        Destroy(trailParticleGameObject, followParticleSystem.main.duration);
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
