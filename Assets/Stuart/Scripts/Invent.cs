using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invent : MonoBehaviour
{
    private Dictionary<Resource, float> resources = new();

  
    public void Add(Resource type, float amount)
    {
        if (resources.ContainsKey(type))
            resources[type] += amount;
        resources.Add(type,amount);
    }

    // public bool Remove(Resource type, float amount)
    // {
    //     if (resources.ContainsKey(type)) //has resource
    //     {
    //         
    //     }
    // }
}
