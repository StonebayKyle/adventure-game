using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Header("Holding Points")]
    [SerializeField]
    private Transform leftHoldPoint;
    [SerializeField]
    private Transform rightHoldPoint;

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

    }

    private void FixedUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        // TODO targetPosition depends on if the player is facing left or right.
        Vector2 targetPosition = rightHoldPoint.position;
        rb.MovePosition(targetPosition);
    }
}

