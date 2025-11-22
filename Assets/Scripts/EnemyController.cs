using UnityEngine;

public enum EnemyType
{
    Skeleton,
    Ghost,
    GhostParabolic
}
public class EnemyController : MonoBehaviour
{
    //Enemy Type
    public EnemyType enemyType;

    //Components
    private Transform groundDetection;
    private SpriteRenderer spriteRenderer;

    //Ground check
    private LayerMask layer;

    //Movement
    [Header("Enemy movement")]
    [SerializeField] private Vector2 direction;
    [Range(0.5f, 2f)]
    [SerializeField] private float moveSpeed = 1f;

    [Header("Jumper move configuration")]
    [Range(0.5f, 3f)]
    [SerializeField] private float sinAmplitude;
    [Range(0.5f, 3f)]
    [SerializeField] private float sinFrecuency;

    private float sinCenterY;
    private bool dead = false;

    public bool Dead { get => dead; set => dead = value; }

    private void Awake()
    {
        groundDetection = transform.GetChild(1);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        layer = LayerMask.GetMask("Ground","Limit");
    }

    private void Start()
    {
        sinCenterY = transform.position.y;
    }


    private void Update()
    {
        EnemyMove();
    }


    /// <summary>
    /// Controls the way enemy moves in order the type of enemy
    /// </summary>
    private void EnemyMove()
    {
        switch (enemyType)
        {
            case EnemyType.Skeleton:
                SkeletonMove();
                break;
            case EnemyType.Ghost:
                GhostMove();
                break;
            case EnemyType.GhostParabolic:
                GhostParabolic();
                break;
        }
    }

    /// <summary>
    /// Controls the way the crab moves
    /// </summary>
    private void SkeletonMove()
    {
        //if ground bellow is not detected or the wall is detected , change direction
        if (!GeneralDetection(1.5f, Vector3.down, Color.yellow) || GeneralDetection(0.5f, direction, Color.green))
        {
            direction = -direction; //same as doing direction *= -1
            spriteRenderer.flipX = !spriteRenderer.flipX;

            //Invert th groundDetection X point 
            groundDetection.localPosition = new Vector3(-groundDetection.localPosition.x, groundDetection.localPosition.y, groundDetection.localPosition.z);
        }

        //Move the Crab
        if (!dead)
        {
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// Controls the way the octopus move
    /// </summary>
    private void GhostMove()
    {
        //detects ceil or floor
        if (GeneralDetection(1f, direction, Color.magenta))
        {
            direction = -direction;
        }
        if (!dead)
        {
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// Controls the Jumper Moves with Sin Frecuency and Sin Amplitude
    /// </summary>
    private void GhostParabolic()
    {
        if (GeneralDetection(1f, direction, Color.magenta))
        {
            direction = -direction;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        Vector3 enemyPosition = transform.position;

        //return an oscillating value between -1 y 1         
        //sinFrecuency controls how many oscillations per unit of distance
        //sinAmplitude controls jump high
        float sin = Mathf.Sin(enemyPosition.x * sinFrecuency) * sinAmplitude;

        //sinCenterY es el nivel base , mantiene el movimiento centrado
        //y oscillate smoothly in the form of a sinusoidal wave
        enemyPosition.y = sinCenterY + sin;
        enemyPosition.x += direction.x * moveSpeed * Time.deltaTime;
        if (!dead)
        {
            transform.position = enemyPosition;
        }
        
    }
    /// <summary>
    /// Detects if the Enemy is touching any ground collision
    /// </summary>
    /// <param name="rayLength"></param>
    /// <param name="direction"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    private bool GeneralDetection(float rayLength, Vector3 direction, Color color)
    {
        Debug.DrawRay(groundDetection.position, direction * rayLength, color);
        return Physics2D.Raycast(groundDetection.position, direction, rayLength, layer);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (dead) return;
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();
        }
    }
}
