using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stuart
{
    public class MapGenerator : MonoBehaviour
    {
        [field: SerializeField] public float sizeScale { get; private set; } = 1f;
        [SerializeField] private Camera camera;
        [SerializeField] private Vector3 startOrientation;
        private GameObject plane;
        [SerializeField] private Material dirtMaterial;
        [SerializeField] private Material frameMaterial;
        [SerializeField] private Material backgroundMaterial;

        private GameObject leftCollider;
        private GameObject rightCollider;
        private GameObject topCollider;
        private GameObject bottomLeftCollider;
        private GameObject bottomRightCollider;
        public static event Action<GameObject> OnMapGenerated;

        private GameObject frame;
        private GameObject background;
        private float defaultCameraSize;

        private void Awake()
        {
            if (camera == null) camera = Camera.main;
            defaultCameraSize = camera.orthographicSize;
            GenerateMap();
        }


        private void GenerateMap()
        {
            GenerateDirt();
            GenerateFrame();
            //SetCameraStartPosition();
            GenerateColliders();
            OnMapGenerated?.Invoke(plane);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                GenerateMap();
        }

        private void GenerateColliders()
        {
            var rotation = 12.5f;
            var size = 10.5f;
            var posOffset = 4.3f;
            if (leftCollider != null) Destroy(leftCollider);
            if (rightCollider != null) Destroy(rightCollider);
            if (topCollider != null) Destroy(topCollider);
            if (bottomRightCollider != null) Destroy(bottomRightCollider);
            if (bottomLeftCollider != null) Destroy(bottomLeftCollider);

            leftCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftCollider.transform.localScale = new Vector3(1, size * sizeScale, 1);
            leftCollider.transform.eulerAngles = new Vector3(0, 0, rotation / sizeScale);
            leftCollider.transform.position = new Vector3(-posOffset, 0, 0);
            leftCollider.GetComponent<MeshRenderer>().enabled = false;
            leftCollider.transform.parent = transform;

            rightCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightCollider.transform.localScale = new Vector3(1, size * sizeScale, 1);
            rightCollider.transform.eulerAngles = new Vector3(0, 0, -rotation / sizeScale);
            rightCollider.transform.position = new Vector3(posOffset, 0, 0);
            rightCollider.GetComponent<MeshRenderer>().enabled = false;
            rightCollider.transform.parent = transform;

            topCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            topCollider.transform.localScale = new Vector3(10, 1, 1);
            topCollider.transform.position = new Vector3(0, 4.9f * sizeScale, 0);
            topCollider.GetComponent<MeshRenderer>().enabled = false;
            topCollider.transform.parent = transform;

            bottomLeftCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bottomLeftCollider.transform.localScale = new Vector3(5, 1, 1);
            bottomLeftCollider.transform.position = new Vector3(-2.86f, -4.9f * sizeScale, 0);
            bottomLeftCollider.GetComponent<MeshRenderer>().enabled = false;
            bottomLeftCollider.transform.parent = transform;

            bottomRightCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bottomRightCollider.transform.localScale = new Vector3(5, 1, 1);
            bottomRightCollider.transform.position = new Vector3(2.53f, -4.9f * sizeScale, 0);
            bottomRightCollider.GetComponent<MeshRenderer>().enabled = false;
            bottomRightCollider.transform.parent = transform;
        }

        private void SetCameraStartPosition() => camera.orthographicSize = defaultCameraSize * sizeScale;

        private void GenerateFrame()
        {
            if (frame != null) Destroy(frame);
            frame = GeneratePlane(frameMaterial, 1);
            frame.transform.parent = transform;

            if (background != null) Destroy(background);
            background = GeneratePlane(backgroundMaterial, 1);
            background.transform.parent = transform;
        }

        private GameObject GeneratePlane(Material material, float defaultScale, bool scale = false)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Plane);
            go.transform.localScale = new Vector3(1, 1, sizeScale);
            go.transform.rotation = Quaternion.Euler(startOrientation);
            var mr = go.GetComponent<MeshRenderer>();
            mr.material = material;
            if (scale)
                material.mainTextureScale = new Vector2(defaultScale * sizeScale, defaultScale * sizeScale);
            return go;
        }

        private void GenerateDirt()
        {
            if (plane != null) Destroy(plane);
            plane = GeneratePlane(dirtMaterial, 10, true);
            plane.transform.parent = transform;
        }
    }
}