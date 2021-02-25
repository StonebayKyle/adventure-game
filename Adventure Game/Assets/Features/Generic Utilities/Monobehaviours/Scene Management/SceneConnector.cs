using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SceneConnector : MonoBehaviour
{
    [Tooltip("Name of which scene to connect to (send the player to this scene).")]
    public string sceneName;
    [Tooltip("Which SpawnPoint to spawn the player at in the new scene.")]
    public int spawnPointID;
}
