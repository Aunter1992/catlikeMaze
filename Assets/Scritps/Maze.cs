using UnityEngine;
using System.Collections;
using System;




public class Maze : MonoBehaviour {



   // public int sizeX, sizeZ;
    public MazeCell cellprefab;
    public float generationStepDelay;
    private MazeCell[,] cells;

    public IntVector2 size;
    public IntVector2 RandomCoordinates {

        get {
            return new IntVector2(UnityEngine.Random.Range(0, size.x), UnityEngine.Random.Range(0, size.z));

        }
    }
    public bool ContainsCoordinates(IntVector2 coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
    }
    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
       cells = new MazeCell[size.x,size.z];
        IntVector2 coordinates = RandomCoordinates;
        while (ContainsCoordinates(coordinates))
        {
            yield return delay;
            CreateCell(coordinates);
            coordinates.z+= 1;
        }
        //for (int x = 0; x < size.x; x++)
        //{
        //    for (int z = 0; z < size.z; z++)
        //    {
        //        yield return delay;
        //        CreateCell(new IntVector2(x,z));
        //    }
        //}
    }

    private void CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellprefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name="MazeCell"+coordinates.x+"," + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0, coordinates.z - size.z * 0.5f - 0.5f);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
