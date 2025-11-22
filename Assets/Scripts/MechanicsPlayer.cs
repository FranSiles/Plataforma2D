using Unity.VisualScripting;
using UnityEngine;

public class MechanicsPlayer : MonoBehaviour
{
    [Header("Configuration attack")]
    [Range(0f, 1f)]
    [SerializeField] private float attackRange=0.5f;
    [Range(0, 100)]
    [SerializeField] private int attackDamage = 20;
    [Range(0f, 10f)]
    [SerializeField] private float attackRate = 2f;
    private float nextTimeAttack = 0f;
    private Transform attackPoint;
    private LayerMask enemyes;
    private Animator animator;
    private PlayerHealth playerHealth;
    //Create object for detection contorls
    private InputSystem_Actions playerControls;
    private Vector2 movementInput;
    private void Awake()
    {
        //Inizialitize the controls
        playerControls = new InputSystem_Actions();
        playerControls.Enable();
        animator = GetComponentInChildren<Animator>();
        attackPoint = transform.Find("AttackPoint");
        enemyes = LayerMask.GetMask("Enemyes");
        playerHealth = GetComponent<PlayerHealth>();
    }
    private void Update()
    {
        movementInput = playerControls.Player.Move.ReadValue<Vector2>();
        if (Time.time >= nextTimeAttack)
        {
            if (playerControls.Player.Attack.triggered)
            {
                Attack();
                nextTimeAttack = Time.time + 1f / attackRate;
            }
        }
        AttackPointPosition();
    }

    private void Attack()
    {
        //Play animation
        animator.SetTrigger("Attack");
        //Detection enemies
        Collider2D [] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyes);
        //Damage then
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
    }

    private void AttackPointPosition()
    {
        //Obtain the position of gameobject children attackposition 
        Vector3 attackPointPosition = attackPoint.localPosition;
        //save the position of vectorx because I don´t want change the position only want change the position in left and right
        float originalX= attackPointPosition.x;
        //Conditional for change the direction
        if (movementInput.x > 0)
        {
            attackPointPosition.x = Mathf.Abs(originalX); //Right direction
        }
        else if (movementInput.x < 0)
        {
            attackPointPosition.x = -1.60f; //Left direction
        }
        //Save new direction
        attackPoint.localPosition= attackPointPosition;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fall"))
        {
            LevelManager.Instance.GameOver();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            playerHealth.TakeDamage();
        }
    }
    private void OnDisable()
    {
        //disable the actions when the object is disabled
        playerControls.Disable();
    }
}
