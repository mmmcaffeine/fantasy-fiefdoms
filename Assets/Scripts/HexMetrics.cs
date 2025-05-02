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
}
