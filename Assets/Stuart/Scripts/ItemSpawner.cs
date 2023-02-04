using System;
using System.Collections;
using System.Collections.Generic;
using Stuart;
using UnityEngine;
using Random = System.Random;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<Pickups> prefabs;
    private List<Pickups> spawnedItems = new();
    [SerializeField] private Vector3 rayDirection;
    [SerializeField] private int itemsToSpawnPerArea;

    private void OnEnable() => MapGenerator.OnMapGenerated += GenerateObjects;

    private void OnDisable() => MapGenerator.OnMapGenerated -= GenerateObjects;

    private void GenerateObjects(GameObject background, float scale)=>StartCoroutine(GenerateObjectsCor(background, scale));
    

    private IEnumerator GenerateObjectsCor(GameObject background, float scale)
    {
        yield return new WaitForFixedUpdate();
        ClearExisting();
        int catcher = 0;
        var bounds = background.GetComponent<MeshCollider>().bounds;
        while (spawnedItems.Count < itemsToSpawnPerArea * scale)
        {
            catcher++;
            if (catcher > 5000)
            {
                Debug.LogWarning("Failed to spawn all elements");
                yield break;
            }

            var go = GameObject.Instantiate(prefabs[UnityEngine.Random.Range(0, prefabs.Count)].gameObject, transform);
            go.transform.position = RandomPointInBounds(bounds, scale * 0.95f);
            if (IsPositionGood(go, rayDirection) && IsPositionGood(go, -rayDirection))
                spawnedItems.Add(go.GetComponent<Pickups>());
            else Destroy(go);
        }
    }

    private static bool IsPositionGood(GameObject go, Vector3 direction)
    {
        var res = Physics.RaycastAll(go.transform.position, direction, 1000f);
        if (res.Length > 0) Debug.Log(res[0].collider.name);
        Debug.DrawLine(go.transform.position, go.transform.position + (direction * 1000), Color.red, 10f);
        var hitCount = 0;
        foreach (var hit in res)
        {
            if (hit.collider.CompareTag("Pickup")) continue;
            hitCount++;
        }
        return hitCount == 1 && !Physics.CheckSphere(go.transform.position,0.3f,LayerMask.GetMask("Bounds"));
    }

    private static Vector3 RandomPointInBounds(Bounds bounds, float scale)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y) * scale,
            -0.05f
        );
    }

    private void ClearExisting()
    {
        if (spawnedItems == null || spawnedItems.Count <= 0) return;
        foreach (var item in spawnedItems)
        {
            DestroyImmediate(item);
        }

        spawnedItems.Clear();
    }
}