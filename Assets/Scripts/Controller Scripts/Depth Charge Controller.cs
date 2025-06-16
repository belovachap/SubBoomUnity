using UnityEngine;

public class DepthChargeController : MonoBehaviour
{
    private float timer = 0;
    private float spawnDuration = 0;
    private Vector3 distance;

    private ObjectPooler expManager;
    private GameManager gameManager;

    void Awake()
    {
        expManager = GameObject.Find("Explosion Manager").GetComponent<ObjectPooler>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (gameObject.activeSelf && gameManager.IsGameActive)
        {
            // vertical movement of depth charge
            transform.Translate(distance);

            if (timer < spawnDuration)
            {
                timer += Time.deltaTime;
            }

            if (timer >= spawnDuration)
            {
                timer = 0;

                GameObject exp = expManager.ObjectManager();
                exp.SetActive(true);
                exp.GetComponent<ExplosionController>().CreateExplosion(gameObject.transform.position);

                gameObject.SetActive(false);
            }
        }
    }

    public void SetSpawnDuration(float incTimeHeld)
    {
        spawnDuration = incTimeHeld;
    }

    public void SetDistance(Vector3 incDistance)
    {
        distance = incDistance;
    }
}