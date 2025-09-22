using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    [Header("Gameplay")]
    public int lives = 3;
    public int score = 0;

    [Tooltip("How long ghosts stay blue (seconds).")]
    public float scaredDuration = 7f;

    [Tooltip("Blink/recover at the tail of scared time (seconds).")]
    public float recoveringDuration = 2f;

    [Header("References")]
    public List<GhostAI> ghosts = new List<GhostAI>();
    public AudioSource sfxEatPellet;
    public AudioSource sfxPowerPellet;
    public AudioSource sfxDeath;

    Coroutine scaredRoutine;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void OnPlayerDied()
    {
        if (sfxDeath) sfxDeath.Play();
        lives = Mathf.Max(0, lives - 1);
        foreach (var g in ghosts) if (g) g.Respawn();
        var player = FindObjectOfType<PlayerController2D>();
        if (player) player.Respawn();
    }

    public void TriggerPowerPellet()
    {
        if (sfxPowerPellet) sfxPowerPellet.Play();
        if (scaredRoutine != null) StopCoroutine(scaredRoutine);
        scaredRoutine = StartCoroutine(ScaredSequence());
    }

    IEnumerator ScaredSequence()
    {
        foreach (var g in ghosts) if (g) g.SetScared(true, false);

        yield return new WaitForSeconds(Mathf.Max(0f, scaredDuration - recoveringDuration));

        foreach (var g in ghosts) if (g) g.SetScared(true, true); 

        yield return new WaitForSeconds(recoveringDuration);

        foreach (var g in ghosts) if (g) g.SetScared(false, false);
        scaredRoutine = null;
    }
}
