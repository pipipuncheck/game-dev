using UnityEngine;

public class Comet : MonoBehaviour
{
    private bool isDead = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        
        // Если пуля попала в комету
        if (other.CompareTag("Bullet"))
        {
            isDead = true;
            Destroy(gameObject);
            Destroy(other.gameObject);
            GameManager.Instance.AddCoins(1);
            GameManager.Instance.AddCometDestroyed(); // НОВАЯ СТРОЧКА
            Debug.Log("Комета сбита! +1 монета");
            return;
        }
        
        // Если комета коснулась игрока
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            
            if (player != null)
            {
                if (player.shieldActive)
                {
                    isDead = true;
                    Destroy(gameObject);
                    Debug.Log("Комета поглощена щитом!");
                }
                else
                {
                    isDead = true;
                    player.Die();
                    Destroy(gameObject);
                    Debug.Log("ИГРОК УМЕР ОТ КОМЕТЫ!");
                }
            }
        }
    }

    void OnBecameInvisible()
    {
        if (!isDead)
            Destroy(gameObject);
    }
}