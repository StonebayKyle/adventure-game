using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SceneConnector : MonoBehaviour
{
    [Tooltip("Name of which scene to connect to.")]
    public string sceneName;
    [Tooltip("Which SpawnPoint to send to in levelID scene.")]
    public int spawnPointID;
}
