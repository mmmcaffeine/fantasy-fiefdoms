using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(HexGridMeshGenerator))]
    public class HexGridMeshGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var hexGridMeshGenerator = (HexGridMeshGenerator)target;

            if (GUILayout.Button("Generate Hex Mesh"))
            {
                hexGridMeshGenerator?.CreateHexGridMesh();
            }

            if (GUILayout.Button("Clear Hex Mesh"))
            {
                hexGridMeshGenerator?.ClearHexGridMesh();
            }
        }
    }
}
