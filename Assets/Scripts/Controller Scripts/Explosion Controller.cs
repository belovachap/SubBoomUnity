using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private float timer = 0.0f;
    private readonly float duration = 1.0f;
    private readonly float scale = 0.8f;

    private GameManager gameManager;
    private AudioManager audioManager;

    private GameObject explodingObj;

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (gameObject.activeSelf && gameManager.IsGameActive)
        {
            audioManager.PlaySFX(audioManager.explosionSFX);

            if (timer < duration)
            {
                timer += Time.deltaTime;
                gameObject.transform.localScale += new Vector3(Time.deltaTime * scale, Time.deltaTime * scale, 1);
            }

            if (timer >= duration)
            {
                timer = 0.0f;

                gameObject.SetActive(false);
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void CreateExplosion(Vector3 spawnPosition, GameObject incObj)
    {
        gameObject.transform.position = spawnPosition;
        explodingObj = incObj;

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && other.gameObject.activeInHierarchy)
        {
            other.gameObject.SetActive(false);

            other.GetComponent<EnemyController>().WaitToRespawn();

            gameManager.UpdateScore(10);
        }
        else if (other.CompareTag("Player") && !explodingObj.CompareTag("Depth Charge"))
        {
            other.gameObject.SetActive(false);
            gameManager.GameOverScreen();
        }
    }
}
