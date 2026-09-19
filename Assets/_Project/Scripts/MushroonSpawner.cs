using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MushroomSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject mushroomPrefab;

    [SerializeField] private int maxMushrooms = 8;

    [SerializeField] private float spawnInterval = 2f;

    [SerializeField] private float spawnHeight = 0.5f;

    [Header("Spawn Area")]
    [SerializeField] private float edgePadding = 1f;

    [Header("References")]
    [SerializeField] private Transform enemyParent;
    [SerializeField] private float minimumDistanceFromPlayer = 3f;

    private BoxCollider spawnArea;

    private readonly List<GameObject> spawnedMushrooms =
        new List<GameObject>();

    private Coroutine spawnCoroutine;

    private bool playerInside = false;

    private void Awake()
    {
        spawnArea = GetComponent<BoxCollider>();

        spawnArea.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(
                SpawnRoutine()
            );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (playerInside)
        {
            RemoveDestroyedMushrooms();

            if (spawnedMushrooms.Count < maxMushrooms)
            {
                SpawnMushroom();
            }

            yield return new WaitForSeconds(
                spawnInterval
            );
        }

        spawnCoroutine = null;
    }

    private void SpawnMushroom()
    {
        if (mushroomPrefab == null)
            return;

        Vector3 spawnPosition =
            GetRandomSpawnPosition();

        GameObject mushroom =
            Instantiate(
                mushroomPrefab,
                spawnPosition,
                Quaternion.identity
            );

        if (enemyParent != null)
        {
            mushroom.transform.SetParent(
                enemyParent
            );
        }

        spawnedMushrooms.Add(mushroom);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Bounds bounds = spawnArea.bounds;

        float minX =
            bounds.min.x + edgePadding;

        float maxX =
            bounds.max.x - edgePadding;

        float minZ =
            bounds.min.z + edgePadding;

        float maxZ =
            bounds.max.z - edgePadding;

        float randomX =
            Random.Range(minX, maxX);

        float randomZ =
            Random.Range(minZ, maxZ);

        return new Vector3(
            randomX,
            bounds.min.y + spawnHeight,
            randomZ
        );
    }

    private void RemoveDestroyedMushrooms()
    {
        spawnedMushrooms.RemoveAll(
            mushroom => mushroom == null
        );
    }
}