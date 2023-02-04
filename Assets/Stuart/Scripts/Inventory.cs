using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stuart
{
    public class Inventory : MonoBehaviour
    {
        private Dictionary<Resource, float> resources = new();
        public event Action OnInventChanged;
        [field: SerializeField] public int playerId { get; private set; }

        private void Start() => OnInventChanged?.Invoke();


        public void Add(Resource type, float amount)
        {
            if (resources.ContainsKey(type))

            {
                resources[type] += amount;
                OnInventChanged?.Invoke();
                return;
            }

            resources.Add(type, amount);
            OnInventChanged?.Invoke();
        }

        public bool Remove(Resource type, float amount)
        {
            if (!resources.ContainsKey(type)) return false;
            if (!(resources[type] >= amount)) return false;
            resources[type] -= amount;
            OnInventChanged?.Invoke();
            return true;
        }

        public bool HasEnough(Resource type, float amount)
        {
            if (!resources.ContainsKey(type)) return false;
            return resources[type] >= amount;
        }

        public float GetResource(Resource type) => resources.ContainsKey(type) ? resources[type] : 0.0f;
    }
}