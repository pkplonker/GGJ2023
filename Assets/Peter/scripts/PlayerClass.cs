using System;
using System.Collections;
using System.Collections.Generic;
using Stuart;
using UnityEngine.UI;
using UnityEngine;

public class PlayerClass
{
    static private float baseSpeed;
    static private float pointDistance;
    static private LayerMask lm;
    static private float timeLim;
    static private float sproutTimeLim;
    static private float sproutSpeed;
    static private Transform controller;
    static private float sprintMultiplier = 2;
    static private float sprintMatMultiplier = 3;

<<<<<<< Updated upstream
=======
    static private int numberOfPlayers = 0;

    private bool sprinting = false;
>>>>>>> Stashed changes

    private GameObject player;
    private int id;

    private Rigidbody rb;
    private LineRenderer lr;
    private MeshCollider mc;

    private Vector3 position;

    private float hor = 0;
    private float ver = 0;
    private float lastHor = -1;

    private bool sprouting = false;

    private int index;
    private float timeLastGrown = -3;

    private GameObject circleCanvas;
    private Image circleProgress;
    private float sproutTime;

    private bool canMove;
    private Inventory invent;
    private bool debug;
    private ResourceUsage resourceUsage;

    [Serializable]
    public struct ResourceUsage
    {
        public float waterRate;
        public float nutRate;

        public ResourceUsage(float movementNut, float movementWater)
        {
            nutRate = movementNut;
            waterRate = movementWater;
        }
    }


    public PlayerClass(GameObject _player, Transform line, GameObject boundary, AnimationCurve curve, ResourceUsage resourceUsage, bool debug = false)
    {
        player = _player;
        this.resourceUsage = resourceUsage;
        rb = player.GetComponent<Rigidbody>();
        mc = line.GetComponent<MeshCollider>();
        lr = line.GetComponent<LineRenderer>();

        circleCanvas = player.transform.GetChild(0).gameObject;
        circleProgress = circleCanvas.transform.GetChild(1).GetComponent<Image>();
        circleCanvas.SetActive(false);

        id = _player.GetComponent<Inventory>().playerId;

        player.transform.position = new Vector3(player.transform.position.x,
            boundary.GetComponent<MeshRenderer>().bounds.max.y - 0.5f, -0.1f);
        LrSetup(curve);
        GameController.OnGameStart += () => { canMove = true; };
        GameController.OnGameEnd += (int x, WinReason fd) => { canMove = false; };
        invent = player.GetComponent<Inventory>();
        this.debug = debug;
    }

    public static void SetParams(float _baseSpeed, float _pointDistance, LayerMask _lm, float _timeLim, float _sproutTimeLim, float _sproutSpeed, Transform _controller)
    {
        baseSpeed = _baseSpeed;
        pointDistance = _pointDistance;
        lm = _lm;
        timeLim = _timeLim;
        sproutTimeLim = _sproutTimeLim;
        sproutSpeed =  _sproutSpeed;
        controller = _controller;
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
        circleProgress.fillAmount = Mathf.Clamp(1 - (Time.time - sproutTime) / sproutTimeLim, 0, 1);

        if (ver == 1)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, lr.GetPosition(index),
                baseSpeed * sproutSpeed * Time.deltaTime);
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
            player.transform.position = Vector3.MoveTowards(player.transform.position, lr.GetPosition(index + 1),
                baseSpeed * sproutSpeed * Time.deltaTime);
            if (player.transform.position == lr.GetPosition(index + 1))
            {
                if (index < lr.positionCount - 2)
                {
                    index++;
                }
            }
        }

        if(circleProgress.fillAmount == 0)
        {
            if (invent.HasEnough(Resource.Sprout, 1) || debug)
            {
                if (!debug) invent.Remove(Resource.Sprout, 1);
                sproutTime = Time.time;

            }
            else if (!Sprout())
            {
                int winner = (id == 1) ? 2 : 1;
                GameController.PlayerWin(winner, WinReason.OtherPlayerTrapped);
            }
        }
    }

    public void Sprint(bool _sprinting)
    {
        sprinting = _sprinting;
    }

    public bool Sprout()
    {
        if (!sprouting && (invent.HasEnough(Resource.Sprout, 1) || debug))
        {
            if (!debug) invent.Remove(Resource.Sprout, 1);
            sprouting = true;
            lr.positionCount += 3;
            lr.SetPosition(lr.positionCount - 3, player.transform.position);
            lr.SetPosition(lr.positionCount - 2, player.transform.position);
            lr.SetPosition(lr.positionCount - 1, player.transform.position);
            BakeLineMesh();
            rb.isKinematic = true;
            index = lr.positionCount - 1;

            circleCanvas.SetActive(true);
            sproutTime = Time.time;
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
            Vector3 newpos = player.transform.position + (cross * 0.21f * lastHor);
            Vector3 oppos = player.transform.position + (cross * 0.21f * -lastHor);

            Collider[] hits = Physics.OverlapSphere(newpos, 0.1f, lm);
            Collider[] ophits = Physics.OverlapSphere(oppos, 0.1f, lm);

            if (hits.Length == 0)
            {
                player.transform.position = player.transform.position + (cross * 0.1f * lastHor);
            }
            else if (ophits.Length == 0)
            {
                player.transform.position = player.transform.position + (cross * 0.1f * -lastHor);
            }
            else
            {
                return false;
            }

            GameObject go = GameObject.Instantiate(lr.gameObject, controller);
            lr = go.GetComponent<LineRenderer>();
            mc = go.GetComponent<MeshCollider>();
            lr.positionCount = index + 2;
            lr.SetPosition(index + 1, player.transform.position);
            sprouting = false;
            rb.isKinematic = false;
            timeLastGrown = Time.time;

            circleCanvas.SetActive(false);
        }
        return true;
    }

    private void MoveDraw()
    {
        if (hor != 0 || ver != 0)
        {
            if (!HasResources())
            {
                hor = 0;
                ver = 0;
            }
            else
                RemoveResources();
        }

        if (Vector3.Distance(position, player.transform.position) > pointDistance)
        {
            lr.positionCount += 1;
            position = player.transform.position;
            timeLastGrown = Time.time;
        }

        float sprint = sprinting ? sprintMultiplier : 1;
        rb.velocity = Vector3.Normalize(new Vector3(hor, ver, 0)) * baseSpeed * sprint;
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
        if (axis == 0)
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

    public void CheckAlive()
    {
        if(Time.time - timeLastGrown > timeLim && !sprouting)
        {
            int winner = (id == 1) ? 2 : 1;
            GameController.PlayerWin(winner, WinReason.OtherPlayerTrapped);
        }
    }

    public void UpdateLoop()
    {
        if (!canMove)
        {
            rb.velocity = Vector3.zero;
            circleCanvas.SetActive(false);
            timeLastGrown = Time.time;
            return;
        }

        if (!sprouting)
        {
            MoveDraw();
            BakeLineMesh();
        }
        else
        {
            TraverseLine();
        }

        CheckAlive();
    }

    private bool HasResources()
    {
        float sprintRate = sprinting ? sprintMatMultiplier : 1;
        var nutAmountReq = resourceUsage.nutRate * baseSpeed * Time.deltaTime * sprintRate;
        var waterAmountReq = resourceUsage.waterRate * baseSpeed * Time.deltaTime * sprintRate;
        if (!invent.HasEnough(Resource.Water, waterAmountReq) ||
            !invent.HasEnough(Resource.Nutrients, nutAmountReq)) return false;
        RemoveResources();
        return true;
    }

    private void RemoveResources()
    {
        float sprintRate = sprinting ? 3 : 1;
        var nutAmountReq = resourceUsage.nutRate * baseSpeed*Time.deltaTime * sprintRate;
        var waterAmountReq = resourceUsage.waterRate* baseSpeed*Time.deltaTime * sprintRate;
        invent.Remove(Resource.Water, waterAmountReq);
        invent.Remove(Resource.Nutrients, nutAmountReq);
    }
}