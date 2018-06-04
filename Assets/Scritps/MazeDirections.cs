using UnityEngine;
using System.Collections;

public enum MazeDirection
{
    North = 0,
    East,
    South,
    West
}
public class MazeDirections  {

    public const int Count = 4;
    public static MazeDirection RandomValue
    {
        get {
            return (MazeDirection)Random.Range(0, Count);
        }
    }

}
