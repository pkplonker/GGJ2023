using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controls : MonoBehaviour
{
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;

    [SerializeField] private float baseSpeed;

    private Rigidbody rb1;
    private Rigidbody rb2;

    private LineRenderer lr1;
    private LineRenderer lr2;

    private MeshCollider mc1;
    private MeshCollider mc2;

    private Vector3 velocity1;
    private Vector3 velocity2;

    private float hor1 = 0;
    private float hor2 = 0;
    private float ver1 = 0;
    private float ver2 = 0;

    private void Start()
    {
        rb1 = Player1.GetComponent<Rigidbody>();
        rb2 = Player2.GetComponent<Rigidbody>();

        lr1 = transform.GetChild(0).GetComponent<LineRenderer>();
        lr2 = transform.GetChild(1).GetComponent<LineRenderer>();

        mc1 = transform.GetChild(0).GetComponent<MeshCollider>();
        mc2 = transform.GetChild(1).GetComponent<MeshCollider>();

        lr1.positionCount = 1;
        lr1.SetPosition(0, Player1.transform.position);

        lr2.positionCount = 1;
        lr2.SetPosition(0, Player2.transform.position);

        lr1.startWidth = 0.1f;
        lr1.endWidth = 0.1f;

        lr2.startWidth = 0.1f;
        lr2.endWidth = 0.1f;
    }

    void Update()
    {
        GetControls();

        VelocityLine();

        BakeLineMesh();
    }

    private void GetControls()
    {
        if (Input.GetKey("a"))
        {
            hor1 = -1;
        }
        else if (Input.GetKey("d"))
        {
            hor1 = 1;
        }
        else
        {
            hor1 = 0;
        }
        if (Input.GetKey("w"))
        {
            ver1 = 1;
        }
        else if (Input.GetKey("s"))
        {
            ver1 = -1;
        }
        else
        {
            ver1 = 0;
        }

        if (Input.GetKey("left"))
        {
            hor2 = -1;
        }
        else if (Input.GetKey("right"))
        {
            hor2 = 1;
        }
        else
        {
            hor2 = 0;
        }
        if (Input.GetKey("up"))
        {
            ver2 = 1;
        }
        else if (Input.GetKey("down"))
        {
            ver2 = -1;
        }
        else
        {
            ver2 = 0;
        }
    }

    private void VelocityLine()
    {
        velocity1 = Vector3.Normalize(new Vector3(hor1, ver1, 0)) * baseSpeed;
        velocity2 = Vector3.Normalize(new Vector3(hor2, ver2, 0)) * baseSpeed;

        if (velocity1 != rb1.velocity)
        {
            lr1.positionCount += 1;
            lr1.SetPosition(lr1.positionCount - 1, Player1.transform.position);
            rb1.velocity = velocity1;
        }
        else
        {
            lr1.SetPosition(lr1.positionCount - 1, Player1.transform.position);
        }

        if (velocity2 != rb2.velocity)
        {
            lr2.positionCount += 1;
            lr2.SetPosition(lr2.positionCount - 1, Player2.transform.position);
            rb2.velocity = velocity2;
        }
        else
        {
            lr2.SetPosition(lr2.positionCount - 1, Player1.transform.position);
        }
    }

    private void BakeLineMesh()
    {
        if (lr1.positionCount > 3)
        {
            Mesh mesh1 = new Mesh();
            mesh1.name = "mesh1";
            Vector3 temp = lr1.GetPosition(lr1.positionCount - 1);
            Vector3 temp2 = lr1.GetPosition(lr1.positionCount - 2);
            lr1.positionCount -= 2;
            lr1.BakeMesh(mesh1);
            lr1.positionCount += 2;
            lr1.SetPosition(lr1.positionCount - 1, temp);
            lr1.SetPosition(lr1.positionCount - 2, temp2);
            mc1.sharedMesh = mesh1;
        }

        if(lr2.positionCount > 3)
        {
            Mesh mesh2 = new Mesh();
            mesh2.name = "mesh2";
            Vector3 temp = lr1.GetPosition(lr2.positionCount - 1);
            Vector3 temp2 = lr1.GetPosition(lr2.positionCount - 2);
            lr2.positionCount -= 2;
            lr2.BakeMesh(mesh2);
            lr2.positionCount += 1;
            lr2.SetPosition(lr2.positionCount - 1, temp);
            lr2.SetPosition(lr2.positionCount - 2, temp2);
            mc1.sharedMesh = mesh2;
        }
    }
}
