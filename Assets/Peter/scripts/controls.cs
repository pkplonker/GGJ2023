using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controls : MonoBehaviour
{
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;

    [SerializeField] private float baseSpeed;

    void Update()
    {
        if (Input.GetKey("a"))
        {
            Player1.transform.position = Player1.transform.position + new Vector3(-baseSpeed, 0, 0);
        }
        if (Input.GetKey("d"))
        {
            Player1.transform.position = Player1.transform.position + new Vector3(baseSpeed, 0, 0);
        }
        if (Input.GetKey("w"))
        {
            Player1.transform.position = Player1.transform.position + new Vector3(0, baseSpeed, 0);
        }
        if (Input.GetKey("s"))
        {
            Player1.transform.position = Player1.transform.position + new Vector3(0, -baseSpeed, 0);
        }

        if (Input.GetKey("left"))
        {
            Player2.transform.position = Player2.transform.position + new Vector3(-baseSpeed, 0, 0);
        }
        if (Input.GetKey("right"))
        {
            Player2.transform.position = Player2.transform.position + new Vector3(baseSpeed, 0, 0);
        }
        if (Input.GetKey("up"))
        {
            Player2.transform.position = Player2.transform.position + new Vector3(0, baseSpeed, 0);
        }
        if (Input.GetKey("down"))
        {
            Player2.transform.position = Player2.transform.position + new Vector3(0, -baseSpeed, 0);
        }
    }
}
