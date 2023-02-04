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

    [SerializeField] private AnimationCurve curve;

    private Rigidbody rb1;
    private Rigidbody rb2;

    private LineRenderer lr1;
    private LineRenderer lr2;

    private MeshCollider mc1;
    private MeshCollider mc2;

    private Vector3 velocity1;
    private Vector3 velocity2;

    private Vector3 position1;
    private Vector3 position2;

    private float hor1 = 0;
    private float hor2 = 0;
    private float ver1 = 0;
    private float ver2 = 0;

    private double startTime;

    private bool start = false;

    private bool power1 = false;
    private bool power2 = false;

    private int i1;
    private int i2;

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

        startTime = Time.time;
    }

    private void playerStart(GameObject background,float val)
    {
        curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.1f);
        curve.AddKey(1.0f, 0.05f);

        Player1.transform.position = new Vector3(Player1.transform.position.x, background.GetComponent<MeshRenderer>().bounds.max.y - 0.5f, -0.1f);
        lr1 = transform.GetChild(0).GetComponent<LineRenderer>();
        lrSetup(lr1, Player1, position1);

        Player2.transform.position = new Vector3(Player2.transform.position.x, background.GetComponent<MeshRenderer>().bounds.max.y - 0.5f, -0.1f);
        lr2 = transform.GetChild(1).GetComponent<LineRenderer>();
        lrSetup(lr2, Player2, position2);

        start = true;
    }

    private void lrSetup(LineRenderer lr, GameObject player, Vector3 position)
    {
        lr.widthCurve = curve;
        lr.positionCount = 1;
        lr.SetPosition(0, player.transform.position);
        position = player.transform.position;
    }

    void Update()
    {
        GetControls1();
        GetControls2();
    }

    private void FixedUpdate()
    {
        curve.MoveKey(0, new Keyframe(0, Mathf.Lerp(curve[0].value, 0.3f, Time.deltaTime / 100)));
        lr1.widthCurve = curve;
        lr2.widthCurve = curve;

        if (start)
        {
            if (!power1)
            {
                VelocityLine1();
                BakeLineMesh1();
            }
            else
            {
                traverseLine1();
            }

            if (!power2)
            {
                VelocityLine2();
                BakeLineMesh2();
            }
            else
            {
                traverseLine2();
            }
        }
    }

    private void traverseLine1()
    {
        if (ver1 == 1)
        {
            Player1.transform.position = Vector3.MoveTowards(Player1.transform.position, lr1.GetPosition(i1), baseSpeed * 3 * Time.deltaTime);
            if (Player1.transform.position == lr1.GetPosition(i1))
            {
                if (i1 > 0)
                {
                    i1--;
                }
            }
        }
        if (ver1 == -1)
        {
            Player1.transform.position = Vector3.MoveTowards(Player1.transform.position, lr1.GetPosition(i1 + 1), baseSpeed * 3 * Time.deltaTime);
            if (Player1.transform.position == lr1.GetPosition(i1 + 1))
            {
                if (i1 < lr1.positionCount - 2)
                {
                    i1++;
                }
            }
        }
    }

    private void traverseLine2()
    {
        if (ver2 == 1)
        {
            Player2.transform.position = Vector3.MoveTowards(Player2.transform.position, lr2.GetPosition(i2), baseSpeed * Time.deltaTime);
            if (Player2.transform.position == lr2.GetPosition(i2))
            {
                if (i2 > 0)
                {
                    i2--;
                }
            }
        }
        if (ver2 == -1)
        {
            Player2.transform.position = Vector3.MoveTowards(Player2.transform.position, lr2.GetPosition(i2 + 1), baseSpeed * Time.deltaTime);
            if (Player2.transform.position == lr2.GetPosition(i2 + 1))
            {
                if (i2 < lr2.positionCount - 2)
                {
                    i2++;
                }
            }
        }
    }

    private void GetControls1()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!power1)
            {
                power1 = true;
                lr1.positionCount += 3;
                lr1.SetPosition(lr1.positionCount - 3, Player1.transform.position);
                lr1.SetPosition(lr1.positionCount - 2, Player1.transform.position);
                lr1.SetPosition(lr1.positionCount - 1, Player1.transform.position);
                rb1.isKinematic = true;
                i1 = lr1.positionCount - 1;
            }
            else if (power1)
            {
                GameObject go = Instantiate(lr1.gameObject, transform);
                lr1 = go.GetComponent<LineRenderer>();
                mc1 = go.GetComponent<MeshCollider>();
                lr1.positionCount = i1 + 1;
                Player1.transform.position = new Vector3(Player1.transform.position.x, Player1.transform.position.y - 0.1f, Player1.transform.position.z);
                power1 = false;
                rb1.isKinematic = false;
            }
        }
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
    }

    private void GetControls2()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (!power2)
            {
                power2 = true;
                lr2.positionCount += 3;
                lr2.SetPosition(lr2.positionCount - 3, Player2.transform.position);
                lr2.SetPosition(lr2.positionCount - 2, Player2.transform.position);
                lr2.SetPosition(lr2.positionCount - 1, Player2.transform.position);
                rb2.isKinematic = true;
                i2 = lr2.positionCount - 1;
            }
            else if (power2)
            {
                GameObject go = Instantiate(lr2.gameObject, transform);
                lr2 = go.GetComponent<LineRenderer>();
                mc2 = go.GetComponent<MeshCollider>();
                lr2.positionCount = i2 + 1;
                Player2.transform.position = new Vector3(Player2.transform.position.x, Player2.transform.position.y - 0.1f, Player2.transform.position.z);
                power2 = false;
                rb2.isKinematic = false;
            }
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

    private void VelocityLine1()
    {
        velocity1 = Vector3.Normalize(new Vector3(hor1, ver1, 0)) * baseSpeed;

        if (Vector3.Distance(position1, Player1.transform.position) > pointDistance)
        {
            lr1.positionCount += 1;
            position1 = Player1.transform.position;
        }

        rb1.velocity = velocity1;
        lr1.SetPosition(lr1.positionCount - 1, Player1.transform.position);
    }

    private void VelocityLine2()
    {
        velocity2 = Vector3.Normalize(new Vector3(hor2, ver2, 0)) * baseSpeed;

        if (Vector3.Distance(position2, Player2.transform.position) > pointDistance)
        {
            lr2.positionCount += 1;
            position2 = Player2.transform.position;
        }

        rb2.velocity = velocity2;
        lr2.SetPosition(lr2.positionCount - 1, Player2.transform.position);
    }

    private void BakeLineMesh1()
    {
        if (lr1.positionCount > 4)
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
    }

    private void BakeLineMesh2()
    {
        if (lr2.positionCount > 4)
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
