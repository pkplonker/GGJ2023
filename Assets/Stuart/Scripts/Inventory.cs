using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stuart
{
    public class Inventory : MonoBehaviour
    {
        private Dictionary<Resource, float> resources = new();

        public void Add(Resource type, float amount)
        {
            if (resources.ContainsKey(type))
<<<<<<< Updated upstream
            {
                resources[type] += amount;
                return;
            }
=======
                resources[type] += amount;
>>>>>>> Stashed changes
            resources.Add(type, amount);
        }

        public bool Remove(Resource type, float amount)
        {
            if (!resources.ContainsKey(type)) return false;
            if (!(resources[type] >= amount)) return false;
            resources[type] -= amount;
            return true;
        }

        public bool HasEnough(Resource type, float amount)
        {
            if (!resources.ContainsKey(type)) return false;
            return resources[type] >= amount;
        }

        private float GetResource(Resource type) => resources.ContainsKey(type) ? resources[type] : 0.0f;
    }
}