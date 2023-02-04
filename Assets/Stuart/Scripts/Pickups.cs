using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Resource
{
    Water,
    Nutrients
}

namespace Stuart
{
    public class Pickups : MonoBehaviour
    {
        [SerializeField] private Resource resource;
        [SerializeField] private float amount;
        [SerializeField] private float destroyDelay = 0.5f;
        private bool pickedUp;

        private void OnTriggerEnter(Collider other)
        {
            var invent = other.GetComponent<Inventory>();
            if (!invent) return;
            invent.Add(resource, amount);
            PickedUp();
        }

        private void PickedUp()
        {
            if (pickedUp) return;
            StartCoroutine(PickUpCor());
        }


        private IEnumerator PickUpCor()
        {
            //play effects or whatever
            yield return new WaitForSeconds(destroyDelay);
            Destroy(gameObject);
        }
    }
}