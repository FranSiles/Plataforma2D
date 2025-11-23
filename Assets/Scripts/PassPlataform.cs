using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class PassPlataform : MonoBehaviour
{
    private InputSystem_Actions playerControls;

    //Comentario provisional esto ha sido modificado antes ponia Collider2D pero daba un warning por si falla ver esto
    private Collider2D myCollider;
    private bool playerOnPlataform;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
        playerControls.Enable();
    }

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();

    }
    private void Update()
    {
        if (playerOnPlataform && playerControls.Player.Crouch.triggered)
        {
            myCollider.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("NotGround");
            Debug.Log("El layer es: "+gameObject.layer);
            StartCoroutine(EnableCollider());
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().ContinueCrouch();
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.7f);
        myCollider.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Ground");
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
