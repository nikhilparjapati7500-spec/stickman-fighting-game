using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Utility helper functions used throughout the game.
/// </summary>
public static class GameHelper
{
    /// <summary>
    /// Calculates distance between two characters.
    /// </summary>
    public static float GetDistanceBetweenCharacters(Transform char1, Transform char2)
    {
        return Vector3.Distance(char1.position, char2.position);
    }
    
    /// <summary>
    /// Gets direction from one transform to another (normalized).
    /// </summary>
    public static Vector3 GetDirectionBetween(Transform from, Transform to)
    {
        return (to.position - from.position).normalized;
    }
    
    /// <summary>
    /// Checks if a point is within a certain distance range.
    /// </summary>
    public static bool IsInRange(Vector3 from, Vector3 to, float range)
    {
        return Vector3.Distance(from, to) <= range;
    }
    
    /// <summary>
    /// Clamps a value between min and max.
    /// </summary>
    public static float Clamp(float value, float min, float max)
    {
        return Mathf.Max(min, Mathf.Min(max, value));
    }
    
    /// <summary>
    /// Converts percentage to 0-1 range.
    /// </summary>
    public static float PercentToNormalized(float percent)
    {
        return Mathf.Clamp01(percent / 100f);
    }
    
    /// <summary>
    /// Gets a random element from a list.
    /// </summary>
    public static T GetRandomFromList<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
            return default(T);
        
        return list[Random.Range(0, list.Count)];
    }
    
    /// <summary>
    /// Checks if a value is approximately equal to another (with epsilon tolerance).
    /// </summary>
    public static bool ApproximatelyEqual(float a, float b, float epsilon = 0.01f)
    {
        return Mathf.Abs(a - b) < epsilon;
    }
    
    /// <summary>
    /// Creates a smooth interpolation between two values.
    /// </summary>
    public static float SmoothStep(float from, float to, float t)
    {
        t = Mathf.Clamp01(t);
        t = t * t * (3f - 2f * t); // Smoothstep formula
        return Mathf.Lerp(from, to, t);
    }
}
