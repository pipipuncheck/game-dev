using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int coins = 0;
    public int totalCometsDestroyed = 0; // СЧЁТЧИК КОМЕТ
    
    private int[] dronesByLevel = new int[6];
    
    public TMP_Text coinsText;
    public TMP_Text dronesText;
    public TMP_Text cometsText; // ТЕКСТ ДЛЯ СЧЁТЧИКА

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        dronesByLevel[1] = 1;
    }

    void Start()
    {
        UpdateUI();
        UpdateDronesVisual();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
        Debug.Log("Монет: " + coins);
    }
    
    // МЕТОД ДЛЯ СЧЁТЧИКА КОМЕТ
    public void AddCometDestroyed()
    {
        totalCometsDestroyed++;
        UpdateUI();
        Debug.Log("Сбито комет: " + totalCometsDestroyed); // Должно появиться в консоли
    }

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void BuyDrone()
    {
        int cost = 10;
        if (SpendCoins(cost))
        {
            dronesByLevel[1]++;
            UpdateUI();
            UpdateDronesVisual();
            Debug.Log("Куплен дрон 1 уровня! Теперь дронов 1 уровня: " + dronesByLevel[1]);
        }
        else
        {
            Debug.Log("Не хватает монет! Нужно 10");
        }
    }

    public void MergeDrones()
    {
        for (int level = 1; level <= 4; level++)
        {
            if (dronesByLevel[level] >= 2)
            {
                dronesByLevel[level] -= 2;
                dronesByLevel[level + 1]++;
                UpdateUI();
                UpdateDronesVisual();
                Debug.Log($"Объединение! 2 дрона {level} уровня → 1 дрон {level + 1} уровня");
                return;
            }
        }
        Debug.Log("Нет двух дронов одного уровня для объединения!");
    }

    public int GetTotalDrones()
    {
        int total = 0;
        for (int i = 1; i <= 5; i++)
        {
            total += dronesByLevel[i];
        }
        return total;
    }

    public int GetMaxLevel()
    {
        for (int level = 5; level >= 1; level--)
        {
            if (dronesByLevel[level] > 0)
                return level;
        }
        return 1;
    }
    
    public int GetDronesCount(int level)
    {
        if (level >= 1 && level <= 5)
            return dronesByLevel[level];
        return 0;
    }
    
    public int[] GetAllDrones()
    {
        return dronesByLevel;
    }

    void UpdateUI()
    {
        if (coinsText != null)
            coinsText.text = "Монеты: " + coins;
        
        if (dronesText != null)
        {
            int total = GetTotalDrones();
            int maxLevel = GetMaxLevel();
            dronesText.text = $"Дроны: {total} шт. (ур. {maxLevel})";
        }
        
        // ОБНОВЛЯЕМ СЧЁТЧИК КОМЕТ
        if (cometsText != null)
            cometsText.text = "Очки: " + totalCometsDestroyed;
    }
    
    void UpdateDronesVisual()
    {
        DroneManager droneManager = FindFirstObjectByType<DroneManager>();
        if (droneManager != null)
        {
            droneManager.UpdateDronesVisual();
        }
        else
        {
            Debug.LogWarning("DroneManager не найден!");
        }
    }
}