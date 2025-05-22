using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSet : MonoBehaviour
{

    [SerializeField] PianoManager pianoManager;
    public int blackTileIndexNum;

    GameObject thisSet;
    public GameObject prevTileSet;
    public int tileSetIndex = 0;

    public bool correctTileTriggered = false;

    private void Start()
    {
        thisSet = gameObject;
    }

    public void SetThisTileSet()
    {
        pianoManager.SetColorOfTilesSet(thisSet, prevTileSet, tileSetIndex);
   //     pianoManager.SetColorOfTilesSet(thisSet, prevTileSet, tileSetIndex);
        pianoManager.StartMoveTileSetCoroutine(transform);
    }
    public void ResetThisTileSet()
    {
        pianoManager.SpawnTileSet();
        Destroy(thisSet);
        Destroy(gameObject);

 //       correctTileTriggered = false;
    }

}
