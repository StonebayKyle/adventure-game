using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DontDestroy))]
[RequireComponent(typeof(Collider2D))]
public class SceneTransferable : MonoBehaviour
{
    private int NextLevelSpawnPointID;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        FindAndStartLoad(collision);
    }

    private void FindAndStartLoad(Collider2D collision)
    {
        SceneConnector connector = collision.gameObject.GetComponent<SceneConnector>();
        if (connector != null)
        {
            //Debug.LogWarning("Collision with connector!");
            NextLevelSpawnPointID = connector.spawnPointID;
            // When the scene is loaded, OnSceneLoaded will finish the process
            SceneManager.LoadScene(connector.sceneName); // Should make a loading screen or smoother transition.
        }
    }

    private void TransportToSpawnPoint()
    {
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        //Debug.LogWarning("Amount of spawnPoints: " + spawnPoints.Length);
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            //Debug.LogWarning("Searching spawnPoints...");
            if (spawnPoint.spawnPointID == NextLevelSpawnPointID)
            {
                //Debug.LogWarning("Found equal spawnPoint!");
                transform.position = spawnPoint.transform.position;
                break;
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // delegate that is called when the scene is loaded.
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        //Debug.LogWarning("Load finished");
        TransportToSpawnPoint();
    }
}
