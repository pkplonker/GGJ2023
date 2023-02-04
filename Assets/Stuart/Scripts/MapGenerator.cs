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
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) GenerateMap();
        }
    

        private void SetCameraStartPosition()=>camera.orthographicSize =defaultCameraSize* sizeScale;
        
        private void GenerateFrame()
        {
            if (frame != null) Destroy(frame);
            frame = GeneratePlane(frameMaterial);
            if (background != null) Destroy(background);
            background = GeneratePlane(backgroundMaterial);
        }

        private GameObject GeneratePlane(Material material, bool scale= false)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Plane);
            go.transform.localScale = new Vector3(sizeScale, 1,sizeScale);
            go.transform.rotation = Quaternion.Euler(startOrientation);
            var mr = go.GetComponent<MeshRenderer>();
            mr.material = material;
            if(scale)
                material.mainTextureScale *= sizeScale;
            return go;
        }

        private void GenerateDirt()
        {
            if (plane != null) Destroy(plane);
            plane = GeneratePlane(dirtMaterial,true);
        }
    }
}