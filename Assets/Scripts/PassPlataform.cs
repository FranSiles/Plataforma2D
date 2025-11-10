using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class PassPlataform : MonoBehaviour
{
    private InputSystem_Actions playerControls;

    private Collider2D collider2D;
    private bool playerOnPlataform;
    private Animator animator;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
        playerControls.Enable();
    }

    private void Start()
    {
        collider2D = GetComponent<Collider2D>();

    }
    private void Update()
    {
        if (playerOnPlataform && playerControls.Player.Crouch.triggered)
        {
            collider2D.enabled = false;
            StartCoroutine(EnableCollider());
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(2f);
        collider2D.enabled = true;
    }

    private void SetPlayerOnPlataform(Collision2D playerColision,bool value)
    {
        if (playerColision.gameObject.CompareTag("Player"))
        {
            playerOnPlataform=value;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetPlayerOnPlataform(collision, true);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        SetPlayerOnPlataform(collision, false);
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
}
