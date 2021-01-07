﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Tooltip("The transform point for where the laser should be bound to positionally. This is intended for where the laser would be held from (i.e. hands, pedestal, etc).")]
    public Transform holdPoint;
    [Header("Bullet")]
    [Tooltip("The transform point for where the laser should fire from.")]
    public Transform firePoint;
    [Tooltip("The prefab of what the laser should fire.")]
    public GameObject bulletPrefab;
    [Tooltip("How much force should be applied to the bullet when the laser is fired.")]
    public float bulletFireForce = 20f;

    private bool firePressed = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //Debug.LogWarning("Fire1 pressed in Update");
            firePressed = true;
        }
    }

    private void FixedUpdate()
    {
        UpdatePosition();
        if (firePressed)
        {
            //Debug.LogWarning("Fire activated in FixedUpdate");
            firePressed = false;
            Fire();
        }
    }

    private void UpdatePosition()
    {
        Vector2 targetPosition = holdPoint.position;
        rb.MovePosition(targetPosition);
    }

    private void Fire()
    {
        // code inspired by Brackeys
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.AddForce(firePoint.right * bulletFireForce, ForceMode2D.Impulse);
    }

}

