using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public ParticleSystem explosion;
    // UI Variables
    public GameObject mainMenuPanel;
    public GameObject gameStatPanel;
    public GameObject gameOverPanel;

    public TextMeshProUGUI livesCountText;
    public TextMeshProUGUI scoreText;
    [SerializeField]
    private AudioClip buttonClickSound,
        explosionSound;
    private AudioSource gameManagerAudioSource;
    [SerializeField]
    private int lives = 3,
        score = 0;
    [SerializeField]
    private float respawnTime = 3.0f;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        player.gameObject.SetActive(false);
        gameManagerAudioSource = GetComponent<AudioSource>();
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        explosion.transform.position = asteroid.transform.position;
        explosion.Play();
        gameManagerAudioSource.PlayOneShot(explosionSound);
        if (asteroid.GetSize()<.75f)
        {
            score += 100;
        }
        else if (asteroid.GetSize()<1.2f)
        {
            score += 50;
        }
        else
        {
            score += 25;
        }

        scoreText.text = "Score : "+score;
    }

    public void PlayerDied()
    {
        explosion.transform.position = player.transform.position;
        explosion.Play();
        gameManagerAudioSource.PlayOneShot(explosionSound);
        this.lives--;
        livesCountText.text = "x " + lives;
        if (lives < 1)
        {
            GameOver();
        }
        else { 
            Invoke(nameof(RespawnPlayer),respawnTime);
        }
    }

    void RespawnPlayer()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
        player.gameObject.SetActive(true);
        Invoke(nameof(TurnOnCollision), respawnTime);
    }
    void TurnOnCollision()
    {
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }
    void GameOver()
    {
        gameStatPanel.SetActive(false);
        player.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
    }
    public void StartGame()
    {
        lives = 3;
        score = 0;

        mainMenuPanel.gameObject.SetActive(false);
        gameStatPanel.gameObject.SetActive(true);
        gameManagerAudioSource.PlayOneShot(buttonClickSound);
        Invoke(nameof(RespawnPlayer), respawnTime);
    }
    public void ExitGame()
    {
        gameManagerAudioSource.PlayOneShot(buttonClickSound);
        Application.Quit();
    }
    private void Update()
    {
        if (gameOverPanel.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                gameOverPanel.gameObject.SetActive(false);
                mainMenuPanel.gameObject.SetActive(true);
            }
        }
    }
}
