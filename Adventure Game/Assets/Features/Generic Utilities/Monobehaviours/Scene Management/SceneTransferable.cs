using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DontDestroy))]
[RequireComponent(typeof(Collider2D))]
public class SceneTransferable : MonoBehaviour
{
    private int NextLevelSpawnPointID;

    public void OnTriggerEnter2D(Collider2D collision)
    {   
        SceneConnector connector = collision.gameObject.GetComponent<SceneConnector>();
        if (connector != null)
        {
            Debug.LogWarning("Collision with connector!");
            NextLevelSpawnPointID = connector.spawnPointID;
            SceneManager.LoadScene(connector.sceneName); // Should make a loading screen or smoother transition.

            SpawnPoint[] spawnPoints = Object.FindObjectsOfType<SpawnPoint>();
            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                if (spawnPoint.spawnPointID == NextLevelSpawnPointID)
                {
                    transform.position = spawnPoint.transform.position;
                    break;
                }
            }
        }
    }
}
