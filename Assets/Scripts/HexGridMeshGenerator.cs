using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class HexGridMeshGenerator : MonoBehaviour
{
    [field: SerializeField] public LayerMask GridLayer { get; private set; }
    [field: SerializeField] public HexGrid HexGrid { get; private set; }

    public void Awake()
    {
        // If we have a HexGrid component in ourselves the HexGrid property will not be null. If we don't have one then
        // look for one in our parent
        HexGrid ??= GetComponentInParent<HexGrid>();

        if (HexGrid is null)
        {
            Debug.LogErrorFormat
            (
                "{0} could not find a {1} component in itself, or in its parent.",
                nameof(HexGridMeshGenerator),
                nameof(HexGrid)
            );
        }
    }

    public void ClearHexGridMesh()
    {
        GetComponent<MeshFilter>()?.sharedMesh?.Clear();
        GetComponent<MeshCollider>()?.sharedMesh?.Clear();
    }

    public void CreateHexGridMesh()
    {
        CreateHexGridMesh(HexGrid.Width, HexGrid.Height, HexGrid.HexSize, HexGrid.Orientation, GridLayer);
    }

    // TODO Extract repeated calculations
    // TODO Name magic numbers such as `3 * 6` and `z * width + x`
    // TODO Extract method and make public so we can get tests around creation of vertices and triangles
    public void CreateHexGridMesh(int width, int height, float hexSize, HexOrientation orientation, LayerMask layerMask)
    {
        ClearHexGridMesh();

        // 7 vertices for every hexagon; we need one for each of the six points, and one for the centre
        var vertices = new Vector3[7 * width * height];

        for (var z = 0; z < height; z++)
        {
            for (var x = 0; x < width; x++)
            {
                // Calculate the centre position of the specific cell we are dealing with, and where all the corners are
                var centerPosition = HexMetrics.Center(hexSize, x, z, orientation);
                var corners = HexMetrics.Corners(hexSize, orientation);

                // For each cell the centre position becomes the first vertex we store
                vertices[(z * width + x) * 7] = centerPosition;

                // Then store one vertex for each point on the specific cell
                for (var s = 0; s < corners.Length; s++)
                {
                    vertices[(z * width + x) * 7 + s + 1] = centerPosition + corners[s % 6];
                }
            }
        }

        // Each cell is going to need six triangles, and each triangle is defined by three vertices
        var triangles = new int[3 * 6 * width * height];

        for (var z = 0; z < height; z++)
        {
            for (var x = 0; x < width; x++)
            {
                for (var s = 0; s < HexMetrics.Corners(hexSize, orientation).Length; s++)
                {
                    // Define each triangle by three indices in the vertices array. The first is for the centre of the
                    // hexagon, the second for a corner of the hexagon, and the third for a second corner. The corner
                    // index will loop back to the beginning i.e. the second corner of the last triangle is the same
                    // as the first corner of the first triangle
                    var cornerIndex = s + 2 > 6
                        ? s + 2 - 6
                        : s + 2;

                    triangles[3 * 6 * (z * width + x) + s * 3 + 0] = (z * width + x) * 7;
                    triangles[3 * 6 * (z * width + x) + s * 3 + 1] = (z * width + x) * 7 + s + 1;
                    triangles[3 * 6 * (z * width + x) + s * 3 + 2] = (z * width + x) * 7 + cornerIndex;
                }
            }
        }

        var mesh = new Mesh
        {
            name = "Hex Mesh", vertices = vertices, triangles = triangles
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
        mesh.RecalculateUVDistributionMetrics();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

        var gridLayerIndex = GetLayerIndex(layerMask);

        Debug.LogFormat("Grid Layer Index: {0}", gridLayerIndex);

        gameObject.layer = gridLayerIndex;
    }

    private static int GetLayerIndex(LayerMask layerMask)
    {
        var layerMaskValue = layerMask.value;

        Debug.LogFormat("Layer Mask Value: {0}", layerMaskValue);

        for (var i = 0; i < 32; i++)
        {
            if (((1 << i) & layerMaskValue) != 0)
            {
                Debug.LogFormat("Layer Index Loop: {0}", i);

                return i;
            }
        }

        return 0;
    }
}
