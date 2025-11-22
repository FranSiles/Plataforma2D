using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }
    public void DestroyEnemy()
    {
        enemyHealth.DestroyGameObject();
    }
}
