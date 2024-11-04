using UnityEngine;

public class Projectile : MonoBehaviour
{
    void Update()
    {
        CheckAttack();
    }

    void CheckAttack()
    {
        // Check if the sword is colliding with an enemy
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 0.25f);        
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Player"))
            {
                enemy.GetComponent<Knight>().TakeDamage();
            }
        }
    }
}
