using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSet : MonoBehaviour
{

    [SerializeField] PianoManager pianoManager;
    public int blackTileIndexNum;

    GameObject thisSet;
    [SerializeField] GameObject prevTileSet;

    public bool correctTileTriggered;

    private void Start()
    {
        thisSet = gameObject;
    }

    public void ResetThisTileSet()
    {
        pianoManager.SetColorOfTilesSet(thisSet, prevTileSet);
        pianoManager.StartMoveTileSetCoroutine(transform);
        correctTileTriggered = false;

    }

}
