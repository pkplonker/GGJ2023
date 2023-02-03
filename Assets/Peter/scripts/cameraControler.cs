using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class cameraControler : MonoBehaviour
{
    [SerializeField] private Camera camera1;
    [SerializeField] private Camera camera2;

    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;

    [SerializeField] private GameObject background;

    private bool split = false;

    void Update()
    {
        if (Mathf.Abs(Player1.transform.position.y - Player2.transform.position.y) < camera1.orthographicSize)
        {
            if (split == true)
            {
                camera1.DORect(new Rect(0, 0, 1, 1), 1);
                camera2.DORect(new Rect(0, 0, 1, 1), 1);
            }

            split = false;

            camera1.transform.position = new Vector3(0, (Player1.transform.position.y + Player2.transform.position.y) / 2, -10);
            camera2.transform.position = new Vector3(0, (Player1.transform.position.y + Player2.transform.position.y) / 2, -10);
        }
        else
        {
            if(split == false)
            {
                camera1.DORect(new Rect(0, 0, 0.5f, 1), 1);
                camera2.DORect(new Rect(0.5f, 0, 0.5f, 1), 1);
            }

            split = true;

            camera1.transform.position = new Vector3(0, Player1.transform.position.y, -10);
            camera2.transform.position = new Vector3(0, Player2.transform.position.y, -10);
        }

        if (camera1.transform.position.y > background.GetComponent<MeshRenderer>().bounds.max.y - camera1.orthographicSize)
        {
            camera1.transform.position = new Vector3(0, background.GetComponent<MeshRenderer>().bounds.max.y - camera1.orthographicSize, -10);
        }
        if (camera1.transform.position.y < background.GetComponent<MeshRenderer>().bounds.min.y + camera1.orthographicSize)
        {
            camera1.transform.position = new Vector3(0, background.GetComponent<MeshRenderer>().bounds.min.y + camera1.orthographicSize, -10);
        }

        if (camera2.transform.position.y > background.GetComponent<MeshRenderer>().bounds.max.y - camera2.orthographicSize)
        {
            camera2.transform.position = new Vector3(0, background.GetComponent<MeshRenderer>().bounds.max.y - camera2.orthographicSize, -10);
        }
        if (camera2.transform.position.y < background.GetComponent<MeshRenderer>().bounds.min.y + camera2.orthographicSize)
        {
            camera2.transform.position = new Vector3(0, background.GetComponent<MeshRenderer>().bounds.min.y + camera2.orthographicSize, -10);
        }
    }
}
