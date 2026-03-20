using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject rockPairPrefab;
    public float spawnInterval = 2f;
    public float spawnX = 10f;

    [Header("Gap Randomization")]
    public float minY = -2f;
    public float maxY = 2f;

    private float spawnTimer;
    private List<GameObject> activeRocks = new List<GameObject>();

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnRockPair();
            spawnTimer = 0f;
        }
    }

    void SpawnRockPair()
{
    float randomY = Random.Range(minY, maxY);
    Vector3 spawnPos = new Vector3(spawnX, randomY, 0f);
    GameObject rock = Instantiate(rockPairPrefab, transform);
    rock.transform.localPosition = spawnPos;
    activeRocks.Add(rock);
}

    /// <summary>
    /// Called by BirdAgent.OnEpisodeBegin() to reset the environment.
    /// </summary>
    public void ResetPipes()
    {
        // Destroy all active rocks
        for (int i = activeRocks.Count - 1; i >= 0; i--)
        {
            if (activeRocks[i] != null)
                Destroy(activeRocks[i]);
        }
        activeRocks.Clear();
        spawnTimer = 0f;
    }

    /// <summary>
    /// Returns the transform of the nearest rock pair that the bird hasn't passed yet.
    /// Used by BirdAgent.CollectObservations().
    /// </summary>
    public Transform GetNextPipe(float birdLocalX)
    {
        activeRocks.RemoveAll(r => r == null);

        Transform closest = null;
        float closestDist = float.MaxValue;

        foreach (var rock in activeRocks)
        {
            float dist = rock.transform.localPosition.x - birdLocalX;
            if (dist > -0.5f && dist < closestDist)
            {
                closestDist = dist;
                closest = rock.transform;
            }
        }
        return closest;
    }
}