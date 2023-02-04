using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass
{
    static private float baseSpeed;
    static private float pointDistance;
    static private LayerMask lm;

    private GameObject player;

    private Rigidbody rb;
    private LineRenderer lr;
    private MeshCollider mc;

    private Vector3 velocity;
    private Vector3 position;

    private float hor = 0;
    private float ver = 0;
    private float lastHor = -1;

    private bool sprouting = false;

    private int index;

    public PlayerClass(GameObject _player, Transform line, GameObject boundary, AnimationCurve curve)
    {
        rb = player.GetComponent<Rigidbody>();
        mc = line.GetComponent<MeshCollider>();
        lr = line.GetComponent<LineRenderer>();

        player.transform.position = new Vector3(player.transform.position.x, boundary.GetComponent<MeshRenderer>().bounds.max.y - 0.5f, -0.1f);

        LrSetup(curve);
    }

    public static void SetPerams(float _baseSpeed, float _pointDistance, LayerMask _lm)
    {
        baseSpeed = _baseSpeed;
        pointDistance = _pointDistance;
        lm = _lm;
    }

    public void UpdateCurve(AnimationCurve curve)
    {
        lr.widthCurve = curve;
    }

    private void LrSetup(AnimationCurve curve)
    {
        lr.widthCurve = curve;
        lr.positionCount = 1;
        lr.SetPosition(0, player.transform.position);
        position = player.transform.position;
    }

    private void TraverseLine()
    {
        if (ver == 1)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, lr.GetPosition(index), baseSpeed * 3 * Time.deltaTime);
            if (player.transform.position == lr.GetPosition(index))
            {
                if (index > 0)
                {
                    index--;
                }
            }
        }
        if (ver == -1)
        {
            if (index + 1 == lr.positionCount) return;
            player.transform.position = Vector3.MoveTowards(player.transform.position, lr.GetPosition(index + 1), baseSpeed * 3 * Time.deltaTime);
            if (player.transform.position == lr.GetPosition(index + 1))
            {
                if (index < lr.positionCount - 2)
                {
                    index++;
                }
            }
        }
    }

    public void Sprout(Transform controller)
    {
        if (!sprouting)
        {
            sprouting = true;
            lr.positionCount += 3;
            lr.SetPosition(lr.positionCount - 3, player.transform.position);
            lr.SetPosition(lr.positionCount - 2, player.transform.position);
            lr.SetPosition(lr.positionCount - 1, player.transform.position);
            BakeLineMesh();
            rb.isKinematic = true;
            index = lr.positionCount - 1;
        }
        else if (sprouting)
        {
            if (index >= lr.positionCount - 1)
            {
                index = lr.positionCount - 2;
            }
            Vector3 direction = lr.GetPosition(index) - lr.GetPosition(index + 1);
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
                return;
            }

            GameObject go = GameObject.Instantiate(lr.gameObject, controller);
            lr = go.GetComponent<LineRenderer>();
            mc = go.GetComponent<MeshCollider>();
            lr.positionCount = index + 2;
            lr.SetPosition(index + 1, player.transform.position);
            sprouting = false;
            rb.isKinematic = false;
        }
    }

    private void MoveDraw()
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

    private void BakeLineMesh()
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

    public void Input(int axis, float direction)
    {
        if(axis == 0)
        {
            if (direction != 0)
            {
                lastHor = direction;
            }
            hor = direction;
        }
        else
        {
            ver = direction;
        }
    }

    public void UpdateLoop()
    {
        if (!sprouting)
        {
            MoveDraw();
            BakeLineMesh();
        }
        else
        {
            TraverseLine();
        }
    }
}
