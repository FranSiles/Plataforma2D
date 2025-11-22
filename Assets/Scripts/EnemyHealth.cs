using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Enemy")]
    [Range(0,250)]
    [SerializeField] private int health = 100;
    
    [Header("Blink damage")]
    [SerializeField] private float blinkSeconds = 0.1f;
    [SerializeField] private Color blinkClolor = Color.red;

    [Header("Poitns")]
    [Range(100, 500)]
    [SerializeField] private int pointsPerEnemy = 250;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private EnemyController enemyController;
    private Collider2D enemycollider;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyController = GetComponent<EnemyController>();
        enemycollider = GetComponent<Collider2D>();
    }

    public void TakeDamage(int damage)
    {
        health -=damage;
        
        if (health<=0)
        {
            Die();
        }
        AudioManager.Instance?.PlayDamageEnemySound();
        StartCoroutine(BlinkSprite(2));
    }

    /// <summary>
    /// Funciton for edit the dates when enemy dead
    /// </summary>
    private void Die()
    {
        //Debug.Log("Enemy died!");
        LevelManager.Instance.EnemyKilled();
        GameManager.Instance.PlayerPoints += pointsPerEnemy;
        enemyController.Dead = true;
        animator.SetBool("IsDead",true);
        enemycollider.enabled = false;
        AudioManager.Instance?.PlayDeathSound();
    }
    /// <summary>
    /// Repeats blinkTimes a blink every blinkSeconds 
    /// </summary>
    /// <param name="blinkTimes"></param>
    /// <returns></returns>
    private IEnumerator BlinkSprite(int blinkTimes){
        /*canTakeDamage = false;*/
        do
        {
            spriteRenderer.color = blinkClolor;
            yield return new WaitForSeconds(blinkSeconds);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(blinkSeconds);
            blinkTimes--;
        } while (blinkTimes > 0);

        /*canTakeDamage = true;*/
    }
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
