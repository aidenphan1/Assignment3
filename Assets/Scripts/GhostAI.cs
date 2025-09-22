using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class GhostAI : MonoBehaviour
{
    public float speed = 4f;
    public float scaredSpeed = 2.5f;
    public float deadSpeed = 7f;

    public LayerMask wallMask;
    public Vector2 homePosition; 

    Rigidbody2D rb;
    Animator anim;

    Vector2 dir = Vector2.left; 
    bool scared = false;
    bool recovering = false;
    bool dead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        homePosition = transform.position;
    }

    void Update()
    {
        if (dead)           anim.Play("Ghost_Dead");
        else if (scared && recovering) anim.Play("Ghost_Recovering");
        else if (scared)    anim.Play("Ghost_Scared");
        else
        {
            if (dir == Vector2.right) anim.Play("Ghost1_WalkRight");
            else if (dir == Vector2.left) anim.Play("Ghost1_WalkLeft");
            else if (dir == Vector2.up) anim.Play("Ghost1_WalkUp");
            else if (dir == Vector2.down) anim.Play("Ghost1_WalkDown");
        }

        if (IsAtCellCenter())
        {
            dir = ChooseDirection(dir);
        }
    }

    void FixedUpdate()
    {
        float spd = dead ? deadSpeed : (scared ? scaredSpeed : speed);
        rb.velocity = dir * spd;
    }

    Vector2 ChooseDirection(Vector2 current)
    {
        List<Vector2> options = new() { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        options.Remove(-current);

        List<Vector2> valid = new();
        foreach (var d in options)
        {
            if (!Blocked(d)) valid.Add(d);
        }

        if (valid.Count == 0)
        {
            return Blocked(-current) ? Vector2.zero : -current;
        }

        return valid[Random.Range(0, valid.Count)];
    }

    bool Blocked(Vector2 d)
    {
        var hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.45f, 0f, d, 0.6f, wallMask);
        return hit.collider != null;
    }

    bool IsAtCellCenter()
    {
        Vector2 p = transform.position;
        return Mathf.Abs(p.x - Mathf.Round(p.x)) < 0.05f &&
               Mathf.Abs(p.y - Mathf.Round(p.y)) < 0.05f;
    }

    public void SetScared(bool isScared, bool isRecovering)
    {
        if (dead) return; 
        scared = isScared;
        recovering = isRecovering;
    }

    public void Kill() 
    {
        dead = true;
        scared = recovering = false;
        Vector2 toHome = (homePosition - (Vector2)transform.position).normalized;
        if (!Blocked(toHome)) dir = toHome;
    }

    public void Respawn()
    {
        transform.position = homePosition;
        dead = scared = recovering = false;
        dir = Vector2.left;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerController2D>();
        if (!player) return;

        if (dead) return;

        if (scared)
        {
            GameManager.I.AddScore(200);
            Kill();
        }
        else
        {
            player.Die();
        }
    }
}
