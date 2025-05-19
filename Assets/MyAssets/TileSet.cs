using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSet : MonoBehaviour
{

    [SerializeField] PianoManager pianoManager;
    public int blackTileIndexNum;

    GameObject thisSet;
    [SerializeField] GameObject prevTileSet;

    private void Start()
    {
        thisSet = gameObject;
    }

    public void ResetThisTileSet()
    {
        pianoManager.SetTilesSet(thisSet, prevTileSet);
    }

}
