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
    float nextTimeAttack = 0f;
    private Transform attackPoint;
    private LayerMask enemyes;
    private Animator animator;
    //Create object for detection contorls
    private InputSystem_Actions playerControls;
    private void Awake()
    {
        //Inizialitize the controls
        playerControls = new InputSystem_Actions();
        playerControls.Enable();
        animator = GetComponentInChildren<Animator>();
        attackPoint = transform.Find("AttackPoint");
        enemyes = LayerMask.GetMask("Enemyes");
    }
    private void Update()
    {
        if (Time.time>=nextTimeAttack) { 
            if (playerControls.Player.Attack.triggered) {
                Attack();
                nextTimeAttack = Time.time+1f/ attackRate;
            }
        }
        
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    private void OnDisable()
    {
        //disable the actions when the object is disabled
        playerControls.Disable();
    }
}
