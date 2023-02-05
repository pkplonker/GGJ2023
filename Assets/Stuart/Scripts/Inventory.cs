using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stuart
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private float startingNut = 10f;
        [SerializeField] private float startingWater = 10f;
        [SerializeField] private float startingsprout = 1f;

        private Dictionary<Resource, float> resources = new();
        public event Action OnInventChanged;
        [field: SerializeField] public int playerId { get; private set; }

        private void Awake()
        {
            Add(Resource.Nutrients,startingNut);
            Add(Resource.Water,startingWater);
            Add(Resource.Sprout,startingsprout);
        }

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