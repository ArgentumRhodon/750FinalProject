using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class BossCollisionResponse : CollisionResponse
{
    [SerializeField]
    private Health health;

    public override void OnCollision(GameObject other)
    {
        if (other.tag == "Weapon")
        {
            health.TakeDamage(1);

            if (health.health <= 0)
            {
                AudioManager.Instance.PlaySFX("EnemyDeath");
                Destroy(gameObject);
                GameManager.Instance.hasWon = true;
            }
            else
            {
                AudioManager.Instance.PlaySFX("Damage");
            }
        }

        if (other.tag == "EnemyDel")
        {
            AudioManager.Instance.PlaySFX("Damage");
            GameManager.Instance.GetComponent<HeartsManager>().playerHealth.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
