using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;



public class Maze : MonoBehaviour {



   // public int sizeX, sizeZ;
    public MazeCell cellprefab;
    public float generationStepDelay;
    public MazePassage passagePrefab;
    public MazeWall wallPrefab;
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
       List<MazeCell> activeCells = new List<MazeCell>();
       // IntVector2 coordinates = RandomCoordinates;
       DoFirstGenerationStep(activeCells);
       while (activeCells.Count > 0)
       {
           yield return delay;
           DoNextGenerationStep(activeCells);
       }
      
    }
    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {

        activeCells.Add(CreateCell(RandomCoordinates));
    }
    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        int currentIndex = activeCells.Count - 1;
        Debug.Log(currentIndex);    
        MazeCell currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentCell.RandomUninitializedDirection;
       // MazeDirection direction = MazeDirections.RandomValue;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
        if (ContainsCoordinates(coordinates))
        {

            MazeCell neighbor = GetCell(coordinates);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else
            {
                CreatWall(currentCell, neighbor, direction);
               // activeCells.RemoveAt(currentIndex);
            }
            
        }
        else
        {
            CreatWall(currentCell, null, direction);
            //activeCells.RemoveAt(currentIndex);
        }
    }
    private MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellprefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name="MazeCell"+coordinates.x+"," + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0, coordinates.z - size.z * 0.5f - 0.5f);
        return newCell;
    }

    public MazeCell GetCell(IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }

    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    
    }
    public void CreatWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    { 
     MazeWall wall =Instantiate(wallPrefab) as MazeWall;
        wall.Initialize(cell,otherCell,direction);
        if(otherCell!=null)
        {
            wall = Instantiate(wallPrefab) as MazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
