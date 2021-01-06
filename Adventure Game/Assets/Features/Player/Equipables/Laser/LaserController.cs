using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Tooltip("The transform point for where the laser should be bound to positionally. This is intended for where the laser would be held from (i.e. hands, pedestal, etc).")]
    public Transform holdPoint;
    [Header("Bullet")]
    [Tooltip("The transform point for where the laser should shoot from.")]
    public Transform firePoint;
    [Tooltip("The prefab of what the laser should shoot.")]
    public GameObject bulletPrefab;
    [Tooltip("How much force should be applied to the bullet when the laser is fired.")]
    public float bulletFireForce = 20f;

    private bool shootPressed = false;

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
        shootPressed = Input.GetButtonDown("Fire1");
    }

    private void FixedUpdate()
    {
        UpdatePosition();
        if (shootPressed)
        {
            Shoot();
        }
    }

    private void UpdatePosition()
    {
        Vector2 targetPosition = holdPoint.position;
        rb.MovePosition(targetPosition);
    }

    private void Shoot()
    {
        // code inspired by Brackeys
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.AddForce(firePoint.right * bulletFireForce, ForceMode2D.Impulse);
    }

}

