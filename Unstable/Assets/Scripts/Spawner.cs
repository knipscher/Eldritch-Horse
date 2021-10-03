using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToSpawn;

    [SerializeField]
    private Vector2 minPosition;
    [SerializeField]
    private Vector2 maxPosition;

    [SerializeField]
    private float timeBetweenSpawns = 3f;

    [SerializeField]
    private bool spawnInWorldSpaceY = false;

    private IEnumerator Start()
    {
        while (true)
        {
            Vector3 randomPosition = Vector3.zero;
            if (spawnInWorldSpaceY)
            {
                randomPosition = new Vector2(transform.position.x + Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y));
            }
            else
            {
                randomPosition = new Vector2(transform.position.x + Random.Range(minPosition.x, maxPosition.x), transform.position.y + Random.Range(minPosition.y, maxPosition.y));
            }
            Instantiate(prefabToSpawn, randomPosition, Quaternion.identity, null);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}