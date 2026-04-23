using UnityEngine;
using System.Collections.Generic;

public class DroneShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float baseShootDelay = 1f; // Базовая задержка для 1 уровня
    
    private DroneManager droneManager;
    private Dictionary<GameObject, float> shootTimers = new Dictionary<GameObject, float>();

    void Start()
    {
        droneManager = GetComponent<DroneManager>();
    }

    void Update()
    {
        if (droneManager == null || GameManager.Instance == null) return;
        
        var drones = droneManager.GetAllDrones();
        
        foreach (GameObject drone in drones)
        {
            if (drone == null) continue;
            
            // Получаем уровень дрона (по цвету или по индексу)
            int droneLevel = GetDroneLevel(drone);
            
            // Скорость стрельбы: базовый уровень / уровень дрона
            // 1 уровень: 1 выстрел/сек, 2 уровень: 2 выстрела/сек, 5 уровень: 5 выстрелов/сек
            float shootDelay = baseShootDelay / droneLevel;
            
            // Инициализируем таймер для дрона
            if (!shootTimers.ContainsKey(drone))
            {
                shootTimers[drone] = 0f;
            }
            
            shootTimers[drone] += Time.deltaTime;
            
            if (shootTimers[drone] >= shootDelay)
            {
                ShootFromPosition(drone.transform.position);
                shootTimers[drone] = 0;
            }
        }
        
        // Очищаем таймеры для уничтоженных дронов
        CleanupTimers(drones);
    }
    
    int GetDroneLevel(GameObject drone)
    {
        // Пытаемся получить уровень из цвета или имени
        SpriteRenderer renderer = drone.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            if (renderer.color == Color.gray) return 1;
            if (renderer.color == Color.green) return 2;
            if (renderer.color == Color.blue) return 3;
            if (renderer.color == new Color(0.7f, 0, 0.7f)) return 4;
            if (renderer.color == Color.yellow) return 5;
        }
        
        // Если не нашли по цвету, ищем по имени
        string name = drone.name;
        if (name.Contains("Lvl1") || name.Contains("_1_")) return 1;
        if (name.Contains("Lvl2") || name.Contains("_2_")) return 2;
        if (name.Contains("Lvl3") || name.Contains("_3_")) return 3;
        if (name.Contains("Lvl4") || name.Contains("_4_")) return 4;
        if (name.Contains("Lvl5") || name.Contains("_5_")) return 5;
        
        return 1; // По умолчанию 1 уровень
    }
    
    void ShootFromPosition(Vector3 position)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0, 8f);
        }
    }
    
    void CleanupTimers(List<GameObject> existingDrones)
    {
        List<GameObject> toRemove = new List<GameObject>();
        
        foreach (var drone in shootTimers.Keys)
        {
            if (drone == null || !existingDrones.Contains(drone))
            {
                toRemove.Add(drone);
            }
        }
        
        foreach (var drone in toRemove)
        {
            shootTimers.Remove(drone);
        }
    }
}