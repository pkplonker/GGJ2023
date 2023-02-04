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
        }

        void Start() => GenerateMap();

        private void GenerateMap()
        {
            GenerateDirt();
            GenerateFrame();
            SetCameraStartPosition();
            GenerateColliders();
            OnMapGenerated?.Invoke(plane);
        }

        private void GenerateColliders()
        {
            var rotation = 12f;
            var size = 11f;
            var posOffset = 4.3f;
            if(leftCollider!=null)Destroy(leftCollider);
            if(rightCollider!=null)Destroy(rightCollider);
            if(topCollider!=null)Destroy(topCollider);
            if(bottomRightCollider!=null)Destroy(bottomRightCollider);
            if(bottomLeftCollider!=null)Destroy(bottomLeftCollider);
            
            leftCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftCollider.transform.localScale = new Vector3(1 * sizeScale, size * sizeScale, 1);
            leftCollider.transform.eulerAngles = new Vector3(0, 0, rotation);
            leftCollider.transform.position = new Vector3(-posOffset * sizeScale, 0, 0);
            leftCollider.GetComponent<MeshRenderer>().enabled = false;
            leftCollider.transform.parent = transform;
            
            rightCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightCollider.transform.localScale = new Vector3(1 * sizeScale, size * sizeScale, 1);
            rightCollider.transform.eulerAngles = new Vector3(0, 0, -rotation);
            rightCollider.transform.position = new Vector3(posOffset * sizeScale, 0, 0);
            rightCollider.GetComponent<MeshRenderer>().enabled = false;
            rightCollider.transform.parent = transform;

            topCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            topCollider.transform.localScale = new Vector3(10 * sizeScale, 1 * sizeScale,1);
            topCollider.transform.position = new Vector3(0,5.5f * sizeScale, 0);
            topCollider.GetComponent<MeshRenderer>().enabled = false;
            topCollider.transform.parent = transform;

            bottomLeftCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bottomLeftCollider.transform.localScale = new Vector3(5 * sizeScale, 1 * sizeScale,1);
            bottomLeftCollider.transform.position = new Vector3(-2.86f*sizeScale,-5f * sizeScale, 0);
            bottomLeftCollider.GetComponent<MeshRenderer>().enabled = false;
            bottomLeftCollider.transform.parent = transform;

            bottomRightCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bottomRightCollider.transform.localScale = new Vector3(5 * sizeScale, 1 * sizeScale,1);
            bottomRightCollider.transform.position = new Vector3(2.53f*sizeScale,-5f * sizeScale, 0);
            bottomRightCollider.GetComponent<MeshRenderer>().enabled = false;
            bottomRightCollider.transform.parent = transform;

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) GenerateMap();
        }


        private void SetCameraStartPosition() => camera.orthographicSize = defaultCameraSize * sizeScale;

        private void GenerateFrame()
        {
            if (frame != null) Destroy(frame);
            frame = GeneratePlane(frameMaterial);
            frame.transform.parent = transform;

            if (background != null) Destroy(background);
            background = GeneratePlane(backgroundMaterial);
            background.transform.parent = transform;

        }

        private GameObject GeneratePlane(Material material, bool scale = false)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Plane);
            go.transform.localScale = new Vector3(sizeScale, 1, sizeScale);
            go.transform.rotation = Quaternion.Euler(startOrientation);
            var mr = go.GetComponent<MeshRenderer>();
            mr.material = material;
            if (scale)
                material.mainTextureScale *= sizeScale;
            return go;
        }

        private void GenerateDirt()
        {
            if (plane != null) Destroy(plane);
            plane = GeneratePlane(dirtMaterial, true);
            plane.transform.parent = transform;
        }
    }
}