using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Obstacle Pooling")]
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] int poolSize = 10;
    [SerializeField] float minSpawnTime = 0.5f;
    [SerializeField] float maxSpawnTime = 2f;
    private List<GameObject> obstaclePool;

    Coroutine spawnObstacles;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    int score = 0;

    [Header("Game Objects")]
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject player;

    [Header("Difficulty Scaling")]
    [SerializeField] float obstacleSpeed = 5f;
    [SerializeField] float speedIncreaseRate = 0.2f;
    [SerializeField] float increaseInterval = 5f;
    [SerializeField] float maxObstacleSpeed = 15f;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playButton.SetActive(true);
        player.SetActive(false);
        InitializeObstaclePool();
    }

    void InitializeObstaclePool()
    {
        obstaclePool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab);
            obj.SetActive(false);
            obstaclePool.Add(obj);
        }
    }

    GameObject GetPooledObstacle()
    {
        foreach (GameObject obj in obstaclePool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        // Optional: Expand pool if needed
        GameObject newObj = Instantiate(obstaclePrefab);
        newObj.SetActive(false);
        obstaclePool.Add(newObj);
        return newObj;
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            GameObject obstacle = GetPooledObstacle();
            obstacle.transform.position = spawnPoint.position;
            obstacle.transform.rotation = Quaternion.identity;

            Obstacle obsScript = obstacle.GetComponent<Obstacle>();
            obsScript.Activate(obstacleSpeed);

            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }

    void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void GameStart()
    {
        player.SetActive(true);
        playButton.SetActive(false);
        spawnObstacles = StartCoroutine(SpawnObstacles());
        InvokeRepeating("ScoreUp", 2f, 1f);
        InvokeRepeating(nameof(IncreaseDifficulty), increaseInterval, increaseInterval);
    }

    void IncreaseDifficulty()
    {
        if (obstacleSpeed > maxObstacleSpeed)
        {
            obstacleSpeed += speedIncreaseRate;
            obstacleSpeed = Mathf.Max(obstacleSpeed, maxObstacleSpeed); // Clamp
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
