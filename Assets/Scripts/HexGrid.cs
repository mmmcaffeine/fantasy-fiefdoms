using UnityEngine;

public class HexGrid : MonoBehaviour
{
    // TODO Create a grid of hexes
    // TODO Store the individual tiles in an array
    // TODO Methods to get, change, add, and remove tiles
    // TODO Gizmo for drawing the grid in the editor

    [field: SerializeField] public HexOrientation Orientation { get; private set; }
    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public int Height { get; private set; }
    [field: SerializeField] public float HexSize { get; private set; }
    [field: SerializeField] public GameObject HexPrefab { get; private set; }
}
