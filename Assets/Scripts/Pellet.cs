using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pellet : MonoBehaviour
{
    public int score = 10;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.I.AddScore(score);
        if (GameManager.I.sfxEatPellet) GameManager.I.sfxEatPellet.Play();
        Destroy(gameObject);
    }
}
