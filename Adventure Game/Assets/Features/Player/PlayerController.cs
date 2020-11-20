using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rbody;
    public Rigidbody2D RigidBody
    {
        get
        {
            return rbody;
        }
    }

    private BoxCollider2D boxCollider;

    private PlayerStateMachine stateMachine;
    
    public float jumpForce = 5f;


    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        stateMachine = new PlayerStateMachine(this);
    }

    public void ChangeState(IPlayerMovementState newState)
    {
        stateMachine.ChangeState(newState);
    }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.ChangeState(new PlayerIdleState());
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        stateMachine.OnCollisionEnter(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        stateMachine.OnCollisionExit(collision);
    }

}
