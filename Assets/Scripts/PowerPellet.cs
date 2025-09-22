using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PowerPellet : MonoBehaviour
{
    public int score = 50;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.I.AddScore(score);
        GameManager.I.TriggerPowerPellet();
        Destroy(gameObject);
    }
}
