using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTiles : MonoBehaviour
{
    private Tilemap destructibleTilemap;

    private Vector3 hitPosition;

    public void Start()
    {
        destructibleTilemap = GetComponent<Tilemap>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.LogWarning("Collision");
        //HandleDestruction(collision);
    }

    private void HandleDestruction(Collision2D collision)
    {
        //collision.gameObject.transform.rotation
        if (collision.gameObject.CompareTag("DestroyingProjectile"))
        {
            //direction = collision.gameObject.transform.rotation.eulerAngles;
            //origin = collision.gameObject.transform.position;
            //RaycastHit2D hit = Physics2D.Raycast(origin, direction, 1f);
            
            //if (hit)
            //{
                
            //}

            foreach (ContactPoint2D contactHit in collision.contacts)
            {
                //Debug.LogWarning("hitPosition: " + hitPosition + "\tContactHit: " + contactHit.point + "\tNormal: " + contactHit.normal);
                hitPosition.x = contactHit.point.x + 0.01f * contactHit.normal.x;
                hitPosition.y = contactHit.point.y + 0.01f * contactHit.normal.y;
                
                destructibleTilemap.SetTile(destructibleTilemap.WorldToCell(hitPosition), null);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitPosition, .2f);
        //Gizmos.DrawRay(origin, direction);
    }
}
