using System;
using UnityEngine;

public static class HexMetrics
{
    public static float OuterRadius(float hexSize)
    {
        return hexSize;
    }

    public static float InnerRadius(float hexSize)
    {
        return hexSize * 0.866025404f;
    }

    public static Vector3[] Corners(float hexSize, HexOrientation orientation)
    {
        var corners = new Vector3[6];

        for (var i = 0; i < 6; i++)
        {
            corners[i] = Corner(hexSize, orientation, i);
        }

        return corners;
    }

    public static Vector3 Corner(float hexSize, HexOrientation orientation, int index)
    {
        var angle = 60f * -index;

        if (orientation == HexOrientation.PointyTop)
        {
            angle -= 30f;
        }

        var x = hexSize * Mathf.Cos(angle * Mathf.Deg2Rad);
        var z = hexSize * Mathf.Sin(angle * Mathf.Deg2Rad);

        return new Vector3(x, 0f, z);
    }

    public static Vector3 Center(float hexSize, int x, int z, HexOrientation orientation)
    {
        float centerX;
        float centerZ;

        var innerRadius = InnerRadius(hexSize);
        var outerRadius = OuterRadius(hexSize);

        switch (orientation)
        {
            case HexOrientation.PointyTop:
                centerX = (x + z * 0.5f - z / 2) * innerRadius * 2f;
                centerZ = z * outerRadius * 1.5f;

                break;
            case HexOrientation.FlatTop:
                centerX = x * outerRadius * 1.5f;
                centerZ = (z + x * 0.5f - x / 2) * innerRadius * 2f;

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(orientation), orientation,
                    "The orientation of the hex grid is invalid.");
        }

        return new Vector3(centerX, 0f, centerZ);
    }

    public static Vector3 OffsetToCube(int col, int row, HexOrientation orientation) =>
        orientation == HexOrientation.PointyTop
            ? AxialToCube(OffsetToAxialPointy(col, row))
            : AxialToCube(OffsetToAxialFlat(col, row));

    public static Vector3 AxialToCube(Vector2 axial)
    {
        var x = axial.x;
        var z = axial.y;
        var y = -x - z;

        return new Vector3(x, z, y);
    }

    public static Vector3 AxialToCube(float q, float r) => new(q, r, -q - r);

    private static Vector2 OffsetToAxialFlat(int col, int row)
    {
        var q = col;
        var r = row - (col + (col & 1)) / 2;

        return new Vector2(q, r);
    }

    private static Vector2 OffsetToAxialPointy(int col, int row)
    {
        var q = col - (row + (row & 1)) / 2;
        var r = row;

        return new Vector2(q, r);
    }

    public static Vector2 CoordinateToOffset(float x, float z, float hexSize, HexOrientation orientation)
    {
        return CubeToOffset(AxialToCube(CoordinateToAxial(x, z, hexSize, orientation)), orientation);
    }

    private static Vector2 CubeToOffset(Vector3 offsetCoord, HexOrientation orientation)
    {
        return CubeToOffset((int)offsetCoord.x, (int)offsetCoord.y, (int)offsetCoord.z, orientation);
    }

    private static Vector2 CubeToOffset(int x, int y, int z, HexOrientation orientation) =>
        orientation == HexOrientation.PointyTop
            ? CubeToOffsetPointy(x, y, z)
            : CubeToOffsetFlat(x, y, z);

    private static Vector2 CubeToOffsetPointy(int x, int y, int z) => new(x + (y - (y & 1)) / 2, y);

    private static Vector2 CubeToOffsetFlat(int x, int y, int z) => new(x, y + (x - (x & 1)) / 2);

    private static Vector2 CoordinateToAxial(float x, float z, float hexSize, HexOrientation orientation) =>
        orientation == HexOrientation.PointyTop
            ? CoordinateToPointyAxial(x, z, hexSize)
            : CoordinateToFlatAxial(x, z, hexSize);

    private static Vector2 CoordinateToPointyAxial(float x, float z, float hexSize)
    {
        var pointyHexCoordinates = new Vector2
        {
            x = (Mathf.Sqrt(3) / 3 * x - 1f / 3 * z) / hexSize,
            y = (2f / 3 * z) / hexSize
        };

        return AxialRound(pointyHexCoordinates);
    }

    private static Vector2 CoordinateToFlatAxial(float x, float z, float hexSize)
    {
        var flatHexCoordinates = new Vector2
        {
            x = (2f / 3 * x) / hexSize,
            y = (-1f / 3 * x + Mathf.Sqrt(3) / 3 * z) / hexSize
        };

        return AxialRound(flatHexCoordinates);
    }

    private static Vector2 AxialRound(Vector2 coordinates) =>
        CubeToAxial(CubeRound(AxialToCube(coordinates.x, coordinates.y)));

    private static Vector2 CubeToAxial(Vector3 cube) => new(cube.x, cube.z);

    private static Vector3 CubeRound(Vector3 frac)
    {
        var roundedCoordinates = new Vector3();

        var rx = Mathf.RoundToInt(frac.x);
        var ry = Mathf.RoundToInt(frac.y);
        var rz = Mathf.RoundToInt(frac.z);

        var xDiff = Mathf.Abs(rx - frac.x);
        var yDiff = Mathf.Abs(ry - frac.y);
        var zDiff = Mathf.Abs(rz - frac.z);

        if (xDiff > yDiff && xDiff > zDiff)
        {
            rx = -ry - rz;
        }
        else if (yDiff > zDiff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        roundedCoordinates.x = rx;
        roundedCoordinates.y = ry;
        roundedCoordinates.z = rz;

        return roundedCoordinates;
    }
}
