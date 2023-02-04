using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stuart;

public class controls : MonoBehaviour
{
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;

    [SerializeField] private float baseSpeed;

    [SerializeField] private float pointDistance;

    private Vector3 position1;
    private Vector3 position2;

    private AnimationCurve curve1;
    private AnimationCurve curve2;

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

    private bool start = false;

    private void OnEnable()
    {
        MapGenerator.OnMapGenerated += playerStart;
    }

    private void OnDisable()
    {
        MapGenerator.OnMapGenerated -= playerStart;
    }

    private void Start()
    {
        rb1 = Player1.GetComponent<Rigidbody>();
        rb2 = Player2.GetComponent<Rigidbody>();
        
        mc1 = transform.GetChild(0).GetComponent<MeshCollider>();
        mc2 = transform.GetChild(1).GetComponent<MeshCollider>();
    }

    private void playerStart(GameObject background)
    {
        lr1 = transform.GetChild(0).GetComponent<LineRenderer>();
        Player1.transform.position = new Vector3(-3, background.GetComponent<MeshRenderer>().bounds.max.y - 0.5f, -0.1f);
        lr1.startWidth = 0.1f;
        lr1.endWidth = 0.05f;
        lr1.positionCount = 1;
        lr1.SetPosition(0, Player1.transform.position);
        position1 = Player1.transform.position;

        lr2 = transform.GetChild(1).GetComponent<LineRenderer>();
        Player2.transform.position = new Vector3(3, background.GetComponent<MeshRenderer>().bounds.max.y - 0.5f, -0.1f);
        lr2.startWidth = 0.1f;
        lr2.endWidth = 0.05f;
        lr2.positionCount = 1;
        lr2.SetPosition(0, Player2.transform.position);
        position2 = Player2.transform.position;

        start = true;
    }

    void Update()
    {
        GetControls();
    }

    private void FixedUpdate()
    {
        if (start)
        {
            VelocityLine();

            BakeLineMesh();
        }
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

        if(Vector3.Distance(position1, Player1.transform.position) > pointDistance)
        {
            lr1.positionCount += 1;
            lr1.SetPosition(lr1.positionCount - 1, Player1.transform.position);
            position1 = Player1.transform.position;
        }

        rb1.velocity = velocity1;
        lr1.SetPosition(lr1.positionCount - 1, Player1.transform.position);

        if (Vector3.Distance(position2, Player2.transform.position) > pointDistance)
        {
            lr2.positionCount += 1;
            lr2.SetPosition(lr2.positionCount - 1, Player2.transform.position);
            position2 = Player2.transform.position;
        }

        rb2.velocity = velocity2;
        lr2.SetPosition(lr2.positionCount - 1, Player2.transform.position);
    }

    private void BakeLineMesh()
    {
        if (lr1.positionCount > 2)
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

        if(lr2.positionCount > 2)
        {
            Mesh mesh2 = new Mesh();
            mesh2.name = "mesh2";
            Vector3 temp = lr2.GetPosition(lr2.positionCount - 1);
            Vector3 temp2 = lr2.GetPosition(lr2.positionCount - 2);
            lr2.positionCount -= 2;
            lr2.BakeMesh(mesh2);
            lr2.positionCount += 2;
            lr2.SetPosition(lr2.positionCount - 1, temp);
            lr2.SetPosition(lr2.positionCount - 2, temp2);
            mc2.sharedMesh = mesh2;
        }
    }
}
