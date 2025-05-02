using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(HexGrid))]
    public class HexGridEditor : UnityEditor.Editor
    {
        void OnSceneGUI()
        {
            var hexGrid = (HexGrid)target;

            for (var z = 0; z < hexGrid.Height; z++)
            {
                for (var x = 0; x < hexGrid.Width; x++)
                {
                    var centerPosition = HexMetrics.Center(hexGrid.HexSize, x, z, hexGrid.Orientation) +
                                         hexGrid.transform.position;
                    var cubeCoord = HexMetrics.OffsetToCube(x, z, hexGrid.Orientation);

                    Handles.Label(centerPosition + Vector3.forward * 2.0f, $"[{x}, {z}]");
                    Handles.Label(centerPosition, $"({cubeCoord.x}, {cubeCoord.y}, {cubeCoord.z})");
                }
            }
        }
    }
}
