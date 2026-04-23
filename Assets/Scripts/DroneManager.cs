using UnityEngine;
using System.Collections.Generic;

public class DroneManager : MonoBehaviour
{
    public GameObject dronePrefab;
    public float radius = 1.5f;
    
    private List<GameObject> drones = new List<GameObject>();
    
    void Start()
    {
        UpdateDronesVisual();
    }
    
    void Update()
    {
        UpdateDronesPositions();
    }
    
    public void UpdateDronesVisual()
    {
        foreach (GameObject drone in drones)
        {
            if (drone != null)
                Destroy(drone);
        }
        drones.Clear();
        
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager не найден!");
            return;
        }
        
        for (int level = 1; level <= 5; level++)
        {
            int count = GameManager.Instance.GetDronesCount(level);
            
            for (int i = 0; i < count; i++)
            {
                GameObject newDrone = Instantiate(dronePrefab, transform);
                
                SpriteRenderer renderer = newDrone.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    switch (level)
                    {
                        case 1: renderer.color = Color.gray; break;
                        case 2: renderer.color = Color.green; break;
                        case 3: renderer.color = Color.blue; break;
                        case 4: renderer.color = new Color(0.7f, 0, 0.7f); break;
                        case 5: renderer.color = Color.yellow; break;
                    }
                }
                
                newDrone.transform.localScale = Vector3.one * (0.3f + level * 0.05f);
                drones.Add(newDrone);
            }
        }
        
        UpdateDronesPositions();
    }
    
    void UpdateDronesPositions()
    {
        int count = drones.Count;
        if (count == 0) return;
        
        for (int i = 0; i < count; i++)
        {
            if (drones[i] != null)
            {
                float angle = (360f / count) * i * Mathf.Deg2Rad;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                drones[i].transform.position = transform.position + new Vector3(x, y, 0);
            }
        }
    }
    
    // НОВЫЙ МЕТОД
    public List<GameObject> GetAllDrones()
    {
        return drones;
    }
}