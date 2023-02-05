using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Stuart;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Camera camera1;
    [SerializeField] private Camera camera2;

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    [SerializeField] private Image border;

    private GameObject background;

    private float target1;
    private float target2;

    private float max;
    private float min;
    private void OnEnable()
    {
        MapGenerator.OnMapGenerated += SetBounds;
    }

    private void OnDisable()
    {
        MapGenerator.OnMapGenerated -= SetBounds;
    }

    private void SetBounds(GameObject _background,float val)
    {
        background = _background;
        max = background.GetComponent<MeshRenderer>().bounds.max.y;
        min = background.GetComponent<MeshRenderer>().bounds.min.y;
        camera2.orthographicSize = camera1.orthographicSize;
    }

    void Update()
    {
        if (player1.transform.position.y < player2.transform.position.y)
        {
            target1 = player1.transform.position.y;
            target2 = player2.transform.position.y;
        }
        else
        {
            target2 = player1.transform.position.y;
            target1 = player2.transform.position.y;
        }

        if (Mathf.Abs(target2 - target1) < camera1.orthographicSize*2)
        {
            float mid = (target1 + target2) / 2;
            target1 = mid - camera1.orthographicSize;
            target2 = mid + camera2.orthographicSize;
        }

        float scale = Mathf.Clamp(Mathf.Abs(target2 - target1) - (camera1.orthographicSize * 2), 0, 2f) * 0.25f;
        border.rectTransform.localScale = new Vector3(1, scale, 1);

        camera1.transform.position = new Vector3(0, Mathf.Lerp(camera1.transform.position.y, target1, Time.deltaTime * 10), -10);
        camera2.transform.position = new Vector3(0, Mathf.Lerp(camera2.transform.position.y, target2, Time.deltaTime * 10), -10);

        if (camera1.transform.position.y > max - camera1.orthographicSize*3)
        {
            camera1.transform.position = new Vector3(0, max - camera1.orthographicSize*3, -10);
        }
        if (camera1.transform.position.y < min + camera1.orthographicSize)
        {
            camera1.transform.position = new Vector3(0, min + camera1.orthographicSize, -10);
        }

        if (camera2.transform.position.y > max - camera2.orthographicSize)
        {
            camera2.transform.position = new Vector3(0, max - camera2.orthographicSize, -10);
        }
        if (camera2.transform.position.y < min + camera2.orthographicSize*3)
        {
            camera2.transform.position = new Vector3(0, min + camera2.orthographicSize*3, -10);
        }
    }
}
