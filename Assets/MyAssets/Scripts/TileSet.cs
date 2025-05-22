using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSet : MonoBehaviour
{

    [SerializeField] public PianoManager pianoManager;
    public int _blackTileIndexNum;

    GameObject _thisSet;
    public GameObject _prevTileSet;
    public int _tileSetIDIndex = 0;

    public bool correctTileTriggered = false;

    private void Start()
    {
        _thisSet = gameObject;
    }

    public void SetThisTileSet()
    {
      //  pianoManager.SetColorOfTilesSet(_thisSet, _prevTileSet, _tileSetIDIndex);
      //  pianoManager.StartMoveTileSetCoroutine(transform);
    }
    public void ResetThisTileSet()
    {
        pianoManager.SpawnTileSet();
      //  Destroy(_thisSet);
        Destroy(gameObject);

 //       correctTileTriggered = false;
    }

}
