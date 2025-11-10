using System.Collections;
using UnityEngine;

public class FallingPlataform : MonoBehaviour
{
    [Header("Settings for fall and destroy plataform")]
    [Range(1f,10f)]
    [SerializeField] private float fallDelay = 1f;
    [Range(1f,10f)]
    [SerializeField] private float destroyDelay = 2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }
    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyDelay);
    }
}
