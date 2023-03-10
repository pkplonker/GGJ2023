using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stuart;

public class PlayerController : MonoBehaviour
{
    [Header("Debug")] [SerializeField] private bool isDebug = true;
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    [SerializeField] private float baseSpeed;
    [SerializeField] private float pointDistance;
    [SerializeField] private LayerMask lm;
    [SerializeField] private float timeLim;
    [SerializeField] private float sproutTimeLim;
    [SerializeField] private float sproutSpeed;


    [SerializeField] private AnimationCurve curve;

    [Header("Movement costs")] [SerializeField]
    private float movementNut;

    [SerializeField] private float movementWater;
    private double startTime;

    private bool start = false;

    PlayerClass Player1;
    PlayerClass Player2;
    private bool init;

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
        PlayerClass.SetParams(baseSpeed, pointDistance, lm, timeLim, sproutTimeLim, sproutSpeed, transform);
        startTime = Time.time;
    }

    private void playerStart(GameObject background, float val)
    {
        init = true;
        curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.1f);
        curve.AddKey(1.0f, 0.05f);
        var res = new PlayerClass.ResourceUsage(movementNut, movementWater);
        Player1 = new PlayerClass(player1, transform.GetChild(0), background, curve, res, isDebug);
        Player2 = new PlayerClass(player2, transform.GetChild(1), background, curve, res, isDebug);

        start = true;
    }

    void Update()
    {
        if (!init) return;
        GetControls();
    }

    private void FixedUpdate()
    {
        if (!init) return;
        curve.MoveKey(0, new Keyframe(0, Mathf.Lerp(curve[0].value, 0.3f, Time.deltaTime / 100)));
        Player1.UpdateCurve(curve);
        Player2.UpdateCurve(curve);

        if (start)
        {
            Player1.UpdateLoop();

            Player2.UpdateLoop();
        }
    }

    private void GetControls()
    {
        if (Input.GetKeyDown("h"))
        {
            Player1.Sprout();
        }
        Player1.Sprint(Input.GetKey("g"));

        if (Input.GetKey("a"))
        {
            Player1.Input(0, -1);
        }
        else if (Input.GetKey("d"))
        {
            Player1.Input(0, 1);
        }
        else
        {
            Player1.Input(0, 0);
        }

        if (Input.GetKey("w"))
        {
            Player1.Input(1, 1);
        }
        else if (Input.GetKey("s"))
        {
            Player1.Input(1, -1);
        }
        else
        {
            Player1.Input(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Player2.Sprout();
        }
        Player2.Sprint(Input.GetKey(KeyCode.KeypadPeriod));

        if (Input.GetKey("left"))
        {
            Player2.Input(0, -1);
        }
        else if (Input.GetKey("right"))
        {
            Player2.Input(0, 1);
        }
        else
        {
            Player2.Input(0, 0);
        }

        if (Input.GetKey("up"))
        {
            Player2.Input(1, 1);
        }
        else if (Input.GetKey("down"))
        {
            Player2.Input(1, -1);
        }
        else
        {
            Player2.Input(1, 0);
        }
    }
}