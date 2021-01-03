using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Tooltip("The transform point for where the laser should be bound to positionally. This is intended for where the laser would be held from (i.e. hands, pedestal, etc).")]
    [SerializeField]
    private Transform holdPoint;

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
        Vector2 targetPosition = holdPoint.position;
        rb.MovePosition(targetPosition);
    }
}

