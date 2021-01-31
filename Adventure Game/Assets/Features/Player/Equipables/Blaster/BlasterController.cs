﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterController : MonoBehaviour
{
    [Tooltip("The transform point for where the blaster should be bound to positionally. This is intended for where the blaster would be held from (i.e. hands, pedestal, etc).")]
    public Transform holdPoint;
    [Header("Bullet")]
    [Tooltip("The transform point for where the blaster should fire from.")]
    public Transform firePoint;
    [Tooltip("The prefab of what the blaster should fire.")]
    public GameObject laserPrefab;
    [Tooltip("How much force should be applied to the laser when the blaster is fired.")]
    public float laserFireForce = 20f;
    [Header("Recoil Force")]
    [Tooltip("Optional: The rigidbody that is 'holding' the blaster. This is used to apply a recoil force when the blaster is fired.")]
    public Rigidbody2D holdingRigidbody;
    [Tooltip("How much force to apply to the holdingRigidbody.")]
    public float recoilForce = 20f;
    [Header("Player")]
    [Tooltip("Optional: The 'holding' object's PlayerController. This is used to tell the player when the blaster is fired. Must also pass reference for its Rigidbody separately for recoil to apply (recoil is not related to this).")] // could potentially be replaced by events, but I have not learned that yet. It could also be improved using inheritance (of blasters) instead of these optional fields.
    public PlayerController playerController;

    private Rigidbody2D rb;

    private BlasterStateMachine stateMachine;

    public bool BlasterFiredInAir
    {
        get
        {
            // it's probably not a good idea to rely on the playerController for this
            return playerController.blasterFiredInAir;
        }
    }



    private bool firePressed = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new BlasterStateMachine(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.ChangeState(new BlasterReadyState());
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        UpdatePosition();
        stateMachine.FixedUpdate();
    }

    private void UpdatePosition()
    {
        Vector2 targetPosition = holdPoint.position;
        rb.MovePosition(targetPosition);
    }

    public void Fire()
    {
        CreateLaser(laserFireForce);
        if (holdingRigidbody != null)
        {
            ApplyRecoilForce(recoilForce);
        }

        stateMachine.OnBlasterFire(this);
        // if there is a playerController to send the fire signal to
        if (playerController != null)
        {
            // send the signal
            playerController.OnBlasterFire(this);
        }
    }

    private void CreateLaser(float force)
    {
        // code inspired by Brackeys
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D laserRigidbody = laser.GetComponent<Rigidbody2D>();
        laserRigidbody.AddForce(firePoint.right * force, ForceMode2D.Impulse);
    }

    private void ApplyRecoilForce(float force)
    {
        Vector2 forceDirection = transform.right * -1;
        //Debug.LogWarning("Force applying: " + forceDirection);
        holdingRigidbody.AddForce(forceDirection * force, ForceMode2D.Impulse);
    }

    public void ChangeState(IBlasterState newState)
    {
        stateMachine.ChangeState(newState);
    }
}

