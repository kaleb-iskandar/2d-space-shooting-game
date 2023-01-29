using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    private Asteroid asteroidPrefab;
    [SerializeField]
    private float startTime = 2f,
        spawnRate = 2f,
        spawnDistance = 18f,
        trajectoryVariance = 15;
    [SerializeField]
    private int spawnAmount = 1;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Spawn),startTime,spawnRate);
    }
    void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
            Vector3 spawnPoint = transform.position + spawnDirection;
            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);
            asteroid.SetSize(Random.Range(asteroid.GetMinSize(), asteroid.GetMaxSize()));
            asteroid.SetTrejectory(rotation * -spawnDirection);
        }
    }
}
