using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject obstacle;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float minSpawnTime = 0.5f;
    [SerializeField] float maxSpawnTime = 2f;
    Coroutine spawnObstacles;
    int score = 0;

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            Instantiate(obstacle, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(minSpawnTime,maxSpawnTime));
        }
    }

    public void GameStart()
    {
        spawnObstacles = StartCoroutine(SpawnObstacles());
    }
}
