using System;
using System.Collections;
using System.Collections.Generic;
using Stuart;
using UnityEditor;
using UnityEngine;

    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var mg = (MapGenerator)target;
            if (!target) return;
            if (GUILayout.Button("Generate Map"))
                mg.GenerateMap();
            if (GUILayout.Button("Toggle Visable Colliders"))
                mg.enabledSolidColliders = !mg.enabledSolidColliders;
            base.OnInspectorGUI();

        }
    }
