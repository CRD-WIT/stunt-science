using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RullerController : MonoBehaviour
{
    public GameObject TileManager;
    private ProceduralTilesController tilesController;
    // Start is called before the first frame update
    void Start()
    {
        tilesController = TileManager.GetComponent<ProceduralTilesController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(tilesController.getSizeX() / 1.9f, 0.5f, 1);
    }
}
