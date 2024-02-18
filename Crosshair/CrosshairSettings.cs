using System.Collections.Generic;
using System.Linq;

internal class CrosshairSettings
{
    private static readonly Dictionary<float, int> LengthMap = new Dictionary<float, int>
    {
        {0, 0}, {0.3f, 1}, {0.7f, 2}, {1.2f, 3}, {1.6f, 4}, {2.1f, 5}, {2.5f, 6}, {2.9f, 7},
        {3.399f, 8}, {3.799f, 9}, {4.299f, 10}, {4.699f, 11}, {5.199f, 12}, {5.599f, 13}, {5.999f, 14},
        {6.499f, 15}, {6.899f, 16}, {7.399f, 17}, {7.799f, 18}, {8.299f, 19}, {8.699f, 20}, {9.199f, 21},
        {9.599f, 22}, {10.099f, 23}
    };


    private static readonly Dictionary<float, int> ThicknessMap = new Dictionary<float, int>
    {
        {0, 1}, {0.7f, 2}, {1.12f, 3}, {1.56f, 4}, {2.01f, 5}, {2.45f, 6}, {2.89f, 7}
    };


    private static readonly Dictionary<float, int> GapMap = new Dictionary<float, int>
    {
        {-7.0f, -3}, {-5.9f, -2},{-4.9f, -1}, {-3.0f, 0}, {-2.0f, 1}, {-1.0f, 2}, {-0.9f, 2}, {-0.8f, 2}, {-0.7f, 2}, {-0.6f, 2}, {-0.5f, 2},{-0.4f, 2}, {-0.3f, 2}, {-0.2f, 2}, {-0.1f, 2}, {-0.05f, 3},{0.00f, 3}, {1.0f, 4},
        {2.0f, 5}, {3.0f, 6}, {4.0f, 7}
    };

    internal static int GetPixelLength(float settingValue)
    {
        return GetMappedValue(LengthMap, settingValue);
    }

    internal static int GetPixelThickness(float settingValue)
    {
        return GetMappedValue(ThicknessMap, settingValue);
    }

    internal static int GetPixelGap(float settingValue)
    {
        return GetMappedValue(GapMap, settingValue);
    }

    private static int GetMappedValue(Dictionary<float, int> map, float settingValue)
    {
        var closestKey = map.Keys.Where(k => k <= settingValue).DefaultIfEmpty(-1).Max();

        if (closestKey == -1)
        {
            return 0; // Or some default value
        }

        return map[closestKey];
    }
}
