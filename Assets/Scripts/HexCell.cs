using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell
{
    [Header("Cell Properties")] [SerializeField]
    private HexOrientation orientation;

    [field: SerializeField] public HexGrid Grid { get; set; }
    [field:SerializeField] public float HexSize { get; set; }
    [field:SerializeField] public TerrainType TerrainType { get; private set; }
    [field:SerializeField] public Vector2 OffsetCoordinates { get; set; }
    [field:SerializeField] public Vector3 CubeCoordinates { get; private set; }
    [field:SerializeField] public Vector2 AxialCoordinates { get; private set; }
    [field:NonSerialized] public List<HexCell> Neighbors { get; private set; }

    [field:SerializeField] private Transform Terrain { get; set; }

    public void SetCoordinates(Vector2 offsetCoordinates, HexOrientation orientation)
    {
        this.orientation = orientation;

        OffsetCoordinates = offsetCoordinates;
        CubeCoordinates = HexMetrics.OffsetToCube(offsetCoordinates, orientation);
        AxialCoordinates = HexMetrics.CubeToAxial(CubeCoordinates);
    }

    public void SetTerrainType(TerrainType terrainType)
    {
        TerrainType = terrainType;
    }

    // TODO Add the implementation (which I'm assuming comes in a later chapter of the tutorial
    public void CreateTerrain()
    {

    }
}
