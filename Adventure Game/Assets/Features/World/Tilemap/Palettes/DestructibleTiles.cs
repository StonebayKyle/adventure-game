using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTiles : MonoBehaviour{
  public Tilemap destructibleTilemap;

  public void Start(){
    destructibleTilemap = GetComponent<Tilemap>();
  }

  public void OnCollisionEnter2D(Collision2D collision){
    if(collision.gameObject.CompareTag("Player")){
      Vector3 hitPosition = Vector2.zero;
      foreach(ContactPoint2D hit in collision.contacts){
        hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
        hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
        destructibleTilemap.SetTile(destructibleTilemap.WorldToCell(hitPosition), null);
      }
    }
  }
}
