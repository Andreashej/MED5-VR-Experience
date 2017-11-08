using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Vector2 gridSize;
    public float tileScale; //for 4x3 tile map 0.5 meters is the correct scaling, otherwise the map is either too small or too big, or we'd need a non-int number to have correct size which is impossible.

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(transform.position.x + -gridSize.x * (tileScale / 2f) + tileScale / 2f + x * tileScale, transform.position.y, transform.position.z - gridSize.y * (tileScale / 2f) + tileScale / 2f + y * tileScale); //offset so the grid is in the middle
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.transform.localScale = Vector3.one * tileScale; //scaling so the grid is in the middle
                newTile.GetComponent<Tile>().SetID((int)gridSize.y * x + y);
                newTile.parent = transform;
            }
        }
    }
}
