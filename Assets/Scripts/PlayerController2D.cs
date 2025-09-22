using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController2D : MonoBehaviour
{
    public float speed = 5f;
    public LayerMask wallMask; 
    public Vector2 startPosition;

    Rigidbody2D rb;
    Animator anim;

    Vector2 input;
    Vector2 currentDir; 
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
    }

    void Update()
    {
        Vector2 raw = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        if (Mathf.Abs(raw.x) > Mathf.Abs(raw.y)) raw.y = 0;
        else raw.x = 0;

        input = raw.normalized;

        if (input != Vector2.zero && CanMove(input))
            currentDir = input;

        if (currentDir == Vector2.right) anim.Play("Pac_WalkRight");
        else if (currentDir == Vector2.left) anim.Play("Pac_WalkLeft");
        else if (currentDir == Vector2.up) anim.Play("Pac_WalkUp");
        else if (currentDir == Vector2.down) anim.Play("Pac_WalkDown");
    }

    void FixedUpdate()
    {
        rb.velocity = currentDir * speed;
    }

    bool CanMove(Vector2 dir)
    {
        var hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.45f, 0f, dir, 0.6f, wallMask);
        return hit.collider == null;
    }

    public void Die()
    {
        rb.velocity = Vector2.zero;
        anim.Play("Pac_Dead");
        GameManager.I.OnPlayerDied();
    }

    public void Respawn()
    {
        transform.position = startPosition;
        currentDir = Vector2.zero;
        anim.Play("Pac_WalkLeft"); 
    }

    void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Pellet")) {
        Destroy(other.gameObject);
    } else if (other.CompareTag("PowerPellet")) {
        Destroy(other.gameObject);
    }
}

}
