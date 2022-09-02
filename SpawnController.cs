using System.Collections;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [Header("SpawnController settings")]
    [Tooltip("Zombie enemies here")]
    public GameObject[] Zombies;
    [Header("Spawn Point settings")]
    [Tooltip("Spawning points here")]
    [SerializeField] private Transform[] spawnPoints;

    [Tooltip("Interval between two spawning enemies")]
    public float spawninterval = 2.5f;
    [Tooltip("Amount of enemy spawn at the start")]
    public int staringSpawnAmount = 4;
    [Tooltip("This will show how many zombies will spawn next round")]
    public int zombiesToSpawn = 0;
    [Tooltip("How many already spawned in scene")]
    public int zombiesSpawned = 0;

    //Roundsystem reference
    RoundSystem roundSystem;

    private void Start()
    {
        roundSystem = GetComponent<RoundSystem>();
        zombiesToSpawn = staringSpawnAmount;
    }

    private void Update()
    {
        if (roundSystem.roundNo > 6)
        {
            zombiesSpawned *= 2;
            spawninterval = 1.8f;
        }
    }

    public void SpawnZombies(GameObject[] enemy)
    {
        if (zombiesToSpawn > 0 && roundSystem.canSpawn)
        {
            int randomEnemy = Random.Range(0, Zombies.Length);
            int currentSpawnpoint = Random.Range(0, spawnPoints.Length);
            Instantiate(enemy[randomEnemy], spawnPoints[currentSpawnpoint].position, Quaternion.identity);

            zombiesSpawned++;
            zombiesToSpawn--;

            StartCoroutine(Spawn());
        }
    }

    public IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawninterval);

        if (zombiesToSpawn != 0)
        {
            SpawnZombies(Zombies);
        }
        else
        {
            roundSystem.canSpawn = false;
            roundSystem.canStartWave = true;
        }
    }

}
