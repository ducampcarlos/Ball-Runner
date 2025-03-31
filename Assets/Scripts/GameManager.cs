using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Obstacle Spawning")]
    [SerializeField] GameObject obstacle;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float minSpawnTime = 0.5f;
    [SerializeField] float maxSpawnTime = 2f;
    Coroutine spawnObstacles;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    int score = 0;

    [Header("Game Objects")]
    [SerializeField] GameObject playbutton;
    [SerializeField] GameObject player;

    private void Awake()
    {
        playbutton.SetActive(true);
        player.SetActive(false);
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            Instantiate(obstacle, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(minSpawnTime,maxSpawnTime));
        }
    }

    void ScoreUp()
    {
        scure++;
        scoreText.text = score.ToString();
    }

    public void GameStart()
    {
        player.SetActive(true);
        playbutton.SetActive(false);
        spawnObstacles = StartCoroutine(SpawnObstacles());
    }
}
