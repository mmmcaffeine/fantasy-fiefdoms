using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    [field:SerializeField] public int BatchSize { get; private set; }

    //[field: SerializeField] public GameObject HexPrefab { get; private set; }

    [SerializeField] private List<HexCell> cells = new ();

    private Task<List<HexCell>> hexGenerationTask;
    private Vector3 gridOrigin;

    public event System.Action OnMapInfoGenerated;
    public event System.Action<float> OnCellBatchGenerated;
    public event System.Action OnCellInstancesGenerated;

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

    private void Awake() => gridOrigin = transform.position;

    private void Start() => hexGenerationTask = Task.Run(() => GenerateHexCellData());

    private void Update()
    {
        if (hexGenerationTask is not null && hexGenerationTask.IsCompleted)
        {
            cells = hexGenerationTask.Result;
            OnMapInfoGenerated?.Invoke();
            StartCoroutine(InstantiateCells());
            hexGenerationTask = null;
        }
    }

    private List<HexCell> GenerateHexCellData()
    {
        var random = new System.Random();
        var generatedCells = new List<HexCell>();

        for (var x = 0; x < Width; x++)
        {
            for (var z = 0; z < Height; z++)
            {
                var cell = new HexCell();

                cell.SetCoordinates(new Vector2(x, z), Orientation);
                cell.Grid = this;
                cell.HexSize = HexSize;

                // Temporarily generate random terrains until we have a proper implementation
                // However, in spite of this being what the tutorial had in place it is not possible to use
                // FindObjectsOfType if you're not on the main thread, and thus the commented lines below cannot
                // possibly work (because the Singleton base class will attempt to do so)
                var randomTerrainIndex = random.Next(0, ResourceManager.Instance.TerrainTypes.Count);
                var randomTerrain = ResourceManager.Instance.TerrainTypes[randomTerrainIndex];

                cell.SetTerrainType(randomTerrain);
                generatedCells.Add(cell);
            }
        }

        return generatedCells;
    }

    private IEnumerator InstantiateCells()
    {
        var batchCount = 0;
        var totalBatches = Mathf.CeilToInt(cells.Count / BatchSize);

        for (var i = 0; i < cells.Count; i++)
        {
            cells[i].CreateTerrain();
            if (i % BatchSize == 0)
            {
                batchCount++;
                OnCellBatchGenerated?.Invoke((float)batchCount / totalBatches);
                yield return null;
            }
        }

        OnCellInstancesGenerated?.Invoke();
    }
}
