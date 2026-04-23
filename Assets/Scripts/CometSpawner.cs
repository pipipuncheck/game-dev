using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    public GameObject cometPrefab;
    
    // Параметры сложности
    public float startSpawnInterval = 1.5f;     // Начальный интервал (1.5 секунды)
    public float minSpawnInterval = 0.3f;       // Минимальный интервал (0.3 секунды)
    public float cometSpeed = 3f;               // Постоянная скорость комет
    public float difficultyIncreaseRate = 0.02f; // Насколько чаще спавнятся
    
    private float currentSpawnInterval;
    private float gameTime;
    private float timer;

    void Start()
    {
        currentSpawnInterval = startSpawnInterval;
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        
        // Уменьшаем интервал спавна со временем (кометы летят ЧАЩЕ)
        currentSpawnInterval = Mathf.Max(minSpawnInterval, startSpawnInterval - gameTime * difficultyIncreaseRate);
        
        timer += Time.deltaTime;
        if (timer >= currentSpawnInterval)
        {
            SpawnComet();
            timer = 0;
        }
    }

    void SpawnComet()
    {
        float randomX = Random.Range(-8f, 8f);
        Vector2 spawnPosition = new Vector2(randomX, 8f);
        
        GameObject newComet = Instantiate(cometPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = newComet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(0, -cometSpeed); // Скорость НЕ меняется
    }
}