using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Resource
{
    Water,
    Nutrients
}
public class Pickups : MonoBehaviour
{
    [SerializeField] private Resource resource;
    [SerializeField] private float amount;
    private void OnTriggerEnter(Collider other)
    {
        var invent = other.GetComponent<Invent>();
        if (invent)
        {
            invent.Add(resource, amount);
        }
    }
}
