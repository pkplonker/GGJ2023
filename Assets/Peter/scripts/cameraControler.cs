using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stuart;

public class cameraControler : MonoBehaviour
{
    [SerializeField] private Camera camera1;
    [SerializeField] private Camera camera2;

    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;

    private GameObject background;

    private float target1;
    private float target2;

    private float max;
    private float min;

    private void OnEnable()
    {
        MapGenerator.OnMapGenerated += bounds;
    }

    private void OnDisable()
    {
        MapGenerator.OnMapGenerated -= bounds;
    }

    private void bounds(GameObject _background)
    {
        background = _background;
        max = background.GetComponent<MeshRenderer>().bounds.max.y;
        min = background.GetComponent<MeshRenderer>().bounds.min.y;
        camera2.orthographicSize = camera1.orthographicSize;
    }

    void Update()
    {
        if (Player1.transform.position.y < Player2.transform.position.y)
        {
            target1 = Player1.transform.position.y;
            target2 = Player2.transform.position.y;
        }
        else
        {
            target2 = Player1.transform.position.y;
            target1 = Player2.transform.position.y;
        }

        if (Mathf.Abs(Player1.transform.position.y - Player2.transform.position.y) < camera1.orthographicSize*2)
        {
            float mid = (target1 + target2) / 2;
            target1 = mid - camera1.orthographicSize;
            target2 = mid + camera2.orthographicSize;
        }

        camera1.transform.position = new Vector3(0, Mathf.Lerp(camera1.transform.position.y, target1, Time.deltaTime * 10), -10);
        camera2.transform.position = new Vector3(0, Mathf.Lerp(camera2.transform.position.y, target2, Time.deltaTime * 10), -10);

        if (camera1.transform.position.y > max - camera1.orthographicSize*3)
        {
            camera1.transform.position = new Vector3(0, background.GetComponent<MeshRenderer>().bounds.max.y - camera1.orthographicSize*3, -10);
        }
        if (camera1.transform.position.y < min + camera1.orthographicSize)
        {
            camera1.transform.position = new Vector3(0, background.GetComponent<MeshRenderer>().bounds.min.y + camera1.orthographicSize, -10);
        }

        if (camera2.transform.position.y > max - camera2.orthographicSize)
        {
            camera2.transform.position = new Vector3(0, background.GetComponent<MeshRenderer>().bounds.max.y - camera2.orthographicSize, -10);
        }
        if (camera2.transform.position.y < min + camera2.orthographicSize*3)
        {
            camera2.transform.position = new Vector3(0, background.GetComponent<MeshRenderer>().bounds.min.y + camera2.orthographicSize*3, -10);
        }
    }
}
