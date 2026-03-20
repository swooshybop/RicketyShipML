using UnityEngine;

public class PipePair : MonoBehaviour
{
    /// <summary>
    /// Returns the world-space Y of the center of the gap.
    /// Since the RockPair parent's Y position IS the gap center
    /// (top and bottom rocks are offset equally from it), just return it.
    /// </summary>
    public float GapCenterY => transform.position.y;
}