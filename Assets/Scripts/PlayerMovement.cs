using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 7f;
    private float leftBorder = -8f;
    private float rightBorder = 8f;
    
    // ЩИТ
    public float shieldCharge = 0f;
    public float shieldChargeRate = 0.1f;
    public bool shieldActive = false;
    public float shieldDuration = 5f;
    private float shieldTimer = 0f;
    public GameObject shieldVisual;
    public Slider shieldSlider;
    
    // Смерть
    public bool isDead = false;
    public GameObject gameOverPanel;

    void Update()
    {
        if (isDead) return;

        // ДВИЖЕНИЕ
        float move = Input.GetAxisRaw("Horizontal");
        Vector3 newPosition = transform.position + new Vector3(move, 0, 0) * speed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, leftBorder, rightBorder);
        transform.position = newPosition;
        
        // ЩИТ
        if (!shieldActive)
        {
            if (shieldCharge < 1f)
            {
                shieldCharge += shieldChargeRate * Time.deltaTime;
                if (shieldCharge > 1f) shieldCharge = 1f;
            }
            
            if (Input.GetKeyDown(KeyCode.E) && shieldCharge >= 1f)
            {
                shieldActive = true;
                shieldTimer = shieldDuration;
                shieldCharge = 0f;
                
                if (shieldVisual != null)
                    shieldVisual.SetActive(true);
                
                Debug.Log("ЩИТ АКТИВИРОВАН на 5 секунд!");
            }
        }
        else
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0f)
            {
                shieldActive = false;
                if (shieldVisual != null)
                    shieldVisual.SetActive(false);
                Debug.Log("ЩИТ ЗАКОНЧИЛСЯ");
            }
        }
        
        if (shieldSlider != null)
        {
            shieldSlider.value = shieldCharge;
        }
    }
    
    public void Die()
    {
        if (shieldActive) return;
        
        isDead = true;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Debug.Log("ИГРОК УМЕР!");
    }
}