using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Stuart
{
    public class ItemSpawner : MonoBehaviour
    {
        [Serializable]
        public class SpawnData
        {
            public Pickups pickup;
            public int amount;
        }

        [SerializeField] private List<SpawnData> prefabs;
        private List<Pickups> spawnedItems = new();
        [SerializeField] private Vector3 rayDirection;

        //private void OnEnable() => MapGenerator.OnMapGenerated += GenerateObjects;

       // private void OnDisable() => MapGenerator.OnMapGenerated -= GenerateObjects;

        public void Spawn(GameObject background, float scale, Action callback) =>
            StartCoroutine(GenerateObjectsCor(background, scale,callback));


        private IEnumerator GenerateObjectsCor(GameObject background, float scale, Action callback)
        {
            yield return new WaitForFixedUpdate();
            ClearExisting();
            var bounds = background.GetComponent<MeshCollider>().bounds;
            foreach (var prefab in prefabs)
            {
                int spawned = 0;
                int it = 0;
                while (spawned < prefab.amount * scale)
                {
                    it++;
                    if (it > 1000 * scale) break;
                    var go = Instantiate(prefab.pickup.gameObject,
                        transform);
                    go.transform.position = RandomPointInBounds(bounds, scale * 0.95f);
                    if (IsPositionGood(go, rayDirection) && IsPositionGood(go, -rayDirection))
                    {
                        spawnedItems.Add(go.GetComponent<Pickups>());
                        go.SetActive(false);
                        spawned++;
                    }
                    else Destroy(go);
                }
            }

            yield return new WaitForFixedUpdate();
            for (var i = spawnedItems.Count - 1; i >= 0; i--)
            {
                if (OverLaps(spawnedItems[i].gameObject))
                {
                    Destroy(spawnedItems[i].gameObject);
                }else spawnedItems[i].gameObject.SetActive(true);
            }

            callback?.Invoke();
        }


        private bool OverLaps(GameObject go)
        {
            var hits = Physics.SphereCastAll(go.transform.position + (Vector3.forward / 2), 0.15f, -Vector3.forward,
                1f);
            //Debug.DrawRay(go.transform.position + (Vector3.forward / 2), -Vector3.forward, Color.red, 10f);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Pickup") && hit.collider != go.GetComponent<Collider>()) return true;
            }

            return false;
        }

        private static bool IsPositionGood(GameObject go, Vector3 direction)
        {
            var res = Physics.RaycastAll(go.transform.position, direction, 1000f);
            var hitCount = 0;
            foreach (var hit in res)
            {
                if (hit.collider.CompareTag("Pickup")) continue;
                hitCount++;
            }

            return hitCount == 1 && !Physics.CheckSphere(go.transform.position, 0.3f, LayerMask.GetMask("Bounds"));
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
}