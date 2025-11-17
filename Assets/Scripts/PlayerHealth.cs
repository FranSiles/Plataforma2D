using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health")]
    [Range(0, 10)]
    [SerializeField] private int health;

    [Header("Blink damage")]
    [SerializeField] private float blinkSeconds = 0.2f;
    [SerializeField] private Color blinkColor = Color.red;

    private bool canTakeDamage = true;

    //components
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public int Health { get => health; set => health = value; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void TakeDamage()
    {
        if (canTakeDamage)
        {
            //reduce player's health
            Health--;

            //Play Sound
            //AudioManager.Instance?.PlayDamageSound();

            if (Health <= 0)
            {
                //LevelManager.Instance.GameOver();
            }

            StartCoroutine(BlinkSprite(4));
        }
    }

    /// <summary>
    /// Repeats blinkTimes a blink every blinkSeconds
    /// </summary>
    /// <param name="blinkTimes"></param>
    /// <returns></returns>
    private IEnumerator BlinkSprite(int blinkTimes)
    {
        canTakeDamage = false;
        animator.SetTrigger("Damage");
        do
        {
            spriteRenderer.color = blinkColor;
            yield return new WaitForSeconds(blinkSeconds);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(blinkSeconds);
            blinkTimes--;
        } while (blinkTimes > 0);

        canTakeDamage = true;
    }
}
