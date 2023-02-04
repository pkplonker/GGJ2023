using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stuart;

public class controls : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    [SerializeField] private float baseSpeed;

    [SerializeField] private float pointDistance;

    [SerializeField] private AnimationCurve curve;

    [SerializeField] private LayerMask lm;

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
    private float lasthor1 = -1;
    private float lasthor2 = -1;

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
        rb1 = player1.GetComponent<Rigidbody>();
        rb2 = player2.GetComponent<Rigidbody>();
        
        mc1 = transform.GetChild(0).GetComponent<MeshCollider>();
        mc2 = transform.GetChild(1).GetComponent<MeshCollider>();

        startTime = Time.time;
    }

    private void playerStart(GameObject background,float val)
    {
        curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.1f);
        curve.AddKey(1.0f, 0.05f);

        player1.transform.position = new Vector3(player1.transform.position.x, background.GetComponent<MeshRenderer>().bounds.max.y - 0.5f, -0.1f);
        lr1 = transform.GetChild(0).GetComponent<LineRenderer>();
        lrSetup(lr1, player1, position1);

        player2.transform.position = new Vector3(player2.transform.position.x, background.GetComponent<MeshRenderer>().bounds.max.y - 0.5f, -0.1f);
        lr2 = transform.GetChild(1).GetComponent<LineRenderer>();
        lrSetup(lr2, player2, position2);

        start = true;
    }

    void Update()
    {
        GetControls();
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
                VelocityLine(ref velocity1, hor1, ver1, player1, lr1, ref position1, rb1);
                BakeLineMesh(lr1, mc1);
            }
            else
            {
                traverseLine(player1, lr1, ref i1, ver1, hor1);
            }

            if (!power2)
            {
                VelocityLine(ref velocity2, hor2, ver2, player2, lr2, ref position2, rb2);
                BakeLineMesh(lr2, mc2);
            }
            else
            {
                traverseLine(player2, lr2, ref i2, ver2, hor2);
            }
        }
    }

    private void GetControls()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = powerUse(ref power1, lr1, player1, rb1, ref i1, mc1, lasthor1);
            if (go != null)
            {
                lr1 = go.GetComponent<LineRenderer>();
                mc1 = go.GetComponent<MeshCollider>();
                lr1.positionCount = i1 + 2;
                lr1.SetPosition(i1 + 1, player1.transform.position);
            }
        }
        if (Input.GetKey("a"))
        {
            hor1 = -1;
            lasthor1 = -1;
        }
        else if (Input.GetKey("d"))
        {
            hor1 = 1;
            lasthor1 = 1;
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

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GameObject go = powerUse(ref power2, lr2, player2, rb2, ref i2, mc2, lasthor2);
            if (go != null)
            {
                lr2 = go.GetComponent<LineRenderer>();
                mc2 = go.GetComponent<MeshCollider>();
                lr2.positionCount = i2 + 2;
                lr2.SetPosition(i2 + 1, player2.transform.position);
            }
        }
        if (Input.GetKey("left"))
        {
            hor2 = -1;
            lasthor2 = -1;
        }
        else if (Input.GetKey("right"))
        {
            hor2 = 1;
            lasthor2 = 1;
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

    private void lrSetup(LineRenderer lr, GameObject player, Vector3 position)
    {
        lr.widthCurve = curve;
        lr.positionCount = 1;
        lr.SetPosition(0, player.transform.position);
        position = player.transform.position;
    }

    private void traverseLine(GameObject player, LineRenderer lr, ref int i, float ver, float hor)
    {
        if (ver == 1)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, lr.GetPosition(i), baseSpeed * 3 * Time.deltaTime);
            if (player.transform.position == lr.GetPosition(i))
            {
                if (i > 0)
                {
                    i--;
                }
            }
        }
        if (ver == -1)
        {
            if (i + 1 == lr.positionCount) return;
            player.transform.position = Vector3.MoveTowards(player.transform.position, lr.GetPosition(i + 1), baseSpeed * 3 * Time.deltaTime);
            if (player.transform.position == lr.GetPosition(i + 1))
            {
                if (i < lr.positionCount - 2)
                {
                    i++;
                }
            }
        }
    }

    private GameObject powerUse(ref bool power, LineRenderer lr, GameObject player, Rigidbody rb, ref int i, MeshCollider mc, float hor)
    {
        if (!power)
        {
            power = true;
            lr.positionCount += 3;
            lr.SetPosition(lr.positionCount - 3, player.transform.position);
            lr.SetPosition(lr.positionCount - 2, player.transform.position);
            lr.SetPosition(lr.positionCount - 1, player.transform.position);
            BakeLineMesh(lr, mc);
            rb.isKinematic = true;
            i = lr.positionCount - 1;
        }
        else if (power)
        {
            if(i >= lr.positionCount - 1)
            {
                i = lr.positionCount - 2;
            }
            Vector3 direction = lr.GetPosition(i) - lr.GetPosition(i + 1);
            if (direction.y < 0)
            {
                direction = -direction;
            }
            Vector3 cross = Vector3.Cross(Vector3.Normalize(direction), new Vector3(0, 0, 1));
            Vector3 newpos = player.transform.position + (cross * 0.21f * hor);
            Vector3 oppos = player.transform.position + (cross * 0.21f * -hor);
            
            Collider[] hits = Physics.OverlapSphere(newpos, 0.1f, lm);
            Collider[] ophits = Physics.OverlapSphere(oppos, 0.1f, lm);

            if (hits.Length == 0)
            {
                player.transform.position = newpos;
            }
            else if (ophits.Length == 0) 
            {
                player.transform.position = player.transform.position + (cross * 0.1f * -hor);
            }
            else
            {
                return null;
            }

            GameObject go = Instantiate(lr.gameObject, transform);
            power = false;
            rb.isKinematic = false;
            return go;
        }
        return null;
    }

    private void VelocityLine(ref Vector3 velocity, float hor, float ver, GameObject player, LineRenderer lr, ref Vector3 position, Rigidbody rb)
    {
        velocity = Vector3.Normalize(new Vector3(hor, ver, 0)) * baseSpeed;

        if (Vector3.Distance(position, player.transform.position) > pointDistance)
        {
            lr.positionCount += 1;
            position = player.transform.position;
        }

        rb.velocity = velocity;
        lr.SetPosition(lr.positionCount - 1, player.transform.position);
    }

    private void BakeLineMesh(LineRenderer lr, MeshCollider mc)
    {
        if (lr.positionCount > 4)
        {
            Mesh mesh = new Mesh();
            mesh.name = "mesh";
            Vector3 temp = lr.GetPosition(lr.positionCount - 1);
            Vector3 temp2 = lr.GetPosition(lr.positionCount - 2);
            lr.positionCount -= 2;
            lr.BakeMesh(mesh);
            lr.positionCount += 2;
            lr.SetPosition(lr.positionCount - 1, temp);
            lr.SetPosition(lr.positionCount - 2, temp2);
            mc.sharedMesh = mesh;
        }
    }
}
