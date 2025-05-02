using UnityEngine;

public class HexGrid : MonoBehaviour
{
    // TODO Create a grid of hexes
    // TODO Store the individual tiles in an array
    // TODO Methods to get, change, add, and remove tiles

    [field: SerializeField] public HexOrientation Orientation { get; private set; }
    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public int Height { get; private set; }
    [field: SerializeField] public float HexSize { get; private set; }
    [field: SerializeField] public GameObject HexPrefab { get; private set; }

    private void OnDrawGizmos()
    {
        for (var z = 0; z < Height; z++)
        {
            for (var x = 0; x < Width; x++)
            {
                var centerPosition = HexMetrics.Center(HexSize, x, z, Orientation) + transform.position;
                var corners = HexMetrics.Corners(HexSize, Orientation);

                for (var s = 0; s < corners.Length; s++)
                {
                    Gizmos.DrawLine(centerPosition + corners[s % 6], centerPosition + corners[(s + 1) % 6]);
                }
            }
        }
    }
}
