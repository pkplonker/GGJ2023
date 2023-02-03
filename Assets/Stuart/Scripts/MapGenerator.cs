using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stuart
{
    public class MapGenerator : MonoBehaviour
    {
        [field: SerializeField] public Vector2 sizeScale { get; private set; } = new(1, 1);
        [SerializeField] private Camera camera;
        [SerializeField] private Vector3 startOrientation;
        private GameObject plane;
        [SerializeField] private Material dirtMaterial;
        [SerializeField] private Material frameMaterial;
        [SerializeField] private Material backgroundMaterial;

        private GameObject frame;
        private GameObject background;

        private void Awake()
        {
            if (camera == null) camera = Camera.main;
        }

        void Start() => GenerateMap(sizeScale);

        private void GenerateMap(Vector2 size)
        {
            GenerateDirt();
            GenerateFrame();
            SetCameraStartPosition();
        }

        private void SetCameraStartPosition()
        {
            //set camera start point
        }

        private void GenerateFrame()
        {
            if (frame != null) Destroy(frame);
            frame = GeneratePlane(frameMaterial);
            if (background != null) Destroy(background);
            background = GeneratePlane(backgroundMaterial);
        }

        private GameObject GeneratePlane(Material material)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Plane);
            go.transform.localScale = new Vector3(sizeScale.x, sizeScale.y, 1);
            go.transform.rotation = Quaternion.Euler(startOrientation);
            var mr = go.GetComponent<MeshRenderer>();
            mr.material = material;
            material.mainTextureScale *= sizeScale;
            return go;
        }

        private void GenerateDirt()
        {
            if (plane != null) Destroy(plane);
            plane = GeneratePlane(dirtMaterial);
        }
    }
}