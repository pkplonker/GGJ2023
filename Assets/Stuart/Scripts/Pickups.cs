using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Stuart
{
public enum Resource
{
    Water,
    Nutrients
}


    public class Pickups : MonoBehaviour
    {
        [SerializeField] private Resource resource;
        [SerializeField] private float amount;
        [SerializeField] private float destroyDelay = 0.5f;
        [SerializeField] private ParticleSystem particleSystem;
        private bool pickedUp;
        private Renderer sprite;
        private void Awake()
        {
            sprite = GetComponent<Renderer>();
        }



        private void OnTriggerEnter(Collider other)
        {
            var invent = other.GetComponent<Inventory>();
            if (!invent)
            {
                Destroy(gameObject);
            }
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
            sprite.enabled = false;
            if(particleSystem!=null) particleSystem.Play();
            yield return new WaitForSeconds(destroyDelay);
            Destroy(gameObject);
        }
    }
}