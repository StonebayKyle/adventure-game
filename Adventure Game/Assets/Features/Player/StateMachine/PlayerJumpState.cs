using UnityEngine;

public class PlayerJumpState : IPlayerMovementState
{
    public void EnterState(PlayerController player)
    {
        player.RigidBody.AddForce(new Vector2(0, player.jumpForce * Time.fixedDeltaTime),ForceMode2D.Impulse);
    }

    public void ExitState(PlayerController player)
    {
        
    }

    public void Update(PlayerController player)
    {
        
    }

    public void FixedUpdate(PlayerController player)
    {

    }

    public void OnCollisionEnter2D(PlayerController player, Collision2D collision)
    {
        if (IsGrounded(player))
        {
            player.ChangeState(new PlayerIdleState());
        }        
    }

    public void OnCollisionExit2D(PlayerController player, Collision2D collision)
    {
        
    }

    private static bool IsGrounded(PlayerController player)
    {
        // sends a raycast downward and detects if it hits a collider within the distance
        const float distanceDownToCheck = 0.01f;

        RaycastHit2D[] hits = Physics2D.RaycastAll(player.transform.position, Vector2.down, distanceDownToCheck);
        return hits.Length > 0;
    }

    public override string ToString()
    {
        return "Player Jump";
    }

}
