using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using System;


public class ProceduralTilesController : MonoBehaviour
{
    public TileBase tileA;
    public TileBase tileB;

    public GameObject gameController;

    int sizeX;
    public TMP_InputField playerAnswer;

    // Start is called before the first frame update

    public int getSizeX(){
        return sizeX;
    }

    // Update is called once per frame
    void Update()
    {
        //string distance = gameController.GetComponent<GameManager>().question.GetArguments()[0].ToString();

        //TODO: fix this
        sizeX = Convert.ToInt32(5) + 8;

        Vector3Int[] positions = new Vector3Int[sizeX * 1];
        TileBase[] tileArray = new TileBase[positions.Length];

        for (int index = 0; index < positions.Length; index++)
        {
            positions[index] = new Vector3Int((index % sizeX) - 1, -3, 0);
            tileArray[index] = index % 2 == 0 ? tileA : tileB;
        }        

        Tilemap tilemap = GetComponent<Tilemap>();
        tilemap.ClearAllTiles();
        tilemap.SetTiles(positions, tileArray);
    }
}
