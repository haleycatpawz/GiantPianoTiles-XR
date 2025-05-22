using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
public class PianoManager : MonoBehaviour
{
    [Header("TileSets")]
    [SerializeField] GameManager _gameManager;
    [SerializeField] GameObject _tileSetsParent;
    [SerializeField] GameObject _pianoTilesSetPrefab;

    [Header("Tile Colors")]
    [SerializeField] public Material whiteMat;
    [SerializeField] public Material blackMat;
    [SerializeField] public Material triggerMatColor;


    [Header("Tile Movement")]
    [SerializeField] float _moveSpeed = 1;
    [SerializeField] Transform _startPosition;
   // vector that holds static start pos
    private Vector3 startPosition; 

    [SerializeField] bool _useEndPositionTransform = false;
    [SerializeField] Vector3 _endPositionVector;
    [SerializeField] Transform _endPositionTransform;
    [SerializeField] float _distToTargetPosThreshold;

    [SerializeField] bool _restartTileSetWhenEndPosReached = false;
    [SerializeField] float _timeBeforeDestruction;

    public Transform latestTileSet;
   // [SerializeField] UnityEvent destroyTileSet;

    public enum tileColor
    {
        white = 0,
        black = 1,
    }

    void Start()
    {
        // initializing start and end pos vector variables
        startPosition = _startPosition.localPosition;
        if (_useEndPositionTransform && _endPositionTransform != null)
        {
            _endPositionVector = _endPositionTransform.localPosition;
        }

       // StartPlayingGame();
    }

    public void StartPlayingGame()
    {
      //  _gameManager.StartGame();
        SpawnTileSet();
        
    }

    private void Update()
    {
        if(latestTileSet != null) {

        if(latestTileSet.transform.localPosition.z <= 3f)
        {
            SpawnTileSet();
        } 
        }
    }
    public void SpawnTileSet()
    {

        var tileSet = Instantiate(_pianoTilesSetPrefab, _startPosition.position, Quaternion.identity);

        tileSet.transform.SetParent(_tileSetsParent.transform);
        tileSet.transform.localPosition = _startPosition.localPosition;
       // getting the amount of sets currently in scene and assigning newly instantiated prefab an id num

        int childCount = _tileSetsParent.transform.childCount;
        var tileSetComponent = tileSet.GetComponent<TileSet>();
        int index = tileSetComponent._tileSetIDIndex = childCount - 1;

        tileSet.name = _pianoTilesSetPrefab.name + " " + (index);
        tileSetComponent._prevTileSet = findPreviousTileSetFromIndex(index);
        tileSetComponent.pianoManager = gameObject.GetComponent<PianoManager>();

        Debug.Log("spawning = " + tileSet.name);
        //  tileSet.GetComponent<TileSet>().SetThisTileSet();
        // assigning a black tile and triggering movement of new prefab
        SetColorOfTilesSet(tileSet, tileSetComponent._prevTileSet, tileSetComponent._tileSetIDIndex);
        StartMoveTileSetCoroutine(tileSet.transform);

        latestTileSet = tileSet.transform;
    }

    private GameObject findPreviousTileSetFromIndex(int indexNum)
    {
        var childCount = _tileSetsParent.transform.childCount;
        //   if (childCount == 0 || childCount < 0) return;
        GameObject prevTileSet;

        if (childCount == 1)
        {
            prevTileSet = _tileSetsParent.transform.GetChild(0).gameObject;
        }
        else if (childCount > 1)
        {
            prevTileSet = _tileSetsParent.transform.GetChild(indexNum - 1).gameObject;
        }
        else // childcount == 0 or less than 1 meaning no previous sets to go off of
        {
            prevTileSet = null;
        }
        return prevTileSet;
    }

    public void SetColorOfTilesSet(GameObject thisTileSet, GameObject previousTileSet, int tileSetIndex)
    {
       var childrenInTileSet = thisTileSet.transform.childCount;

        // defining previous index to generate the new black tile index off of, if none we randomly generate
       int prevBlackTileIndex;
        if (previousTileSet != null)
        {
            prevBlackTileIndex = previousTileSet.GetComponent<TileSet>()._blackTileIndexNum;
        }
        else prevBlackTileIndex = Random.Range(0, 3);

            int thisBlackTileIndex = generateIndexNearPrevIndex(prevBlackTileIndex, childrenInTileSet);

            thisTileSet.GetComponent<TileSet>()._blackTileIndexNum = thisBlackTileIndex;

            //  Debug.Log(thisBlackTileIndex);

            //  if("pianoTilesSet1" == thisTileSet.name) Debug.Log(thisTileSet.name + "index " + thisBlackTileIndex);

            for (int j = 0; j < childrenInTileSet; j++)
            {
                GameObject tile = thisTileSet.transform.GetChild(j).gameObject;

                if (j == thisBlackTileIndex)
                {
                    setTileColor(tile, tileColor.black);
                    if (thisTileSet.name == "pianoTilesSet1") Debug.Log(thisTileSet.name + "index " + thisBlackTileIndex);

                }
                else
                {
                    setTileColor(tile, tileColor.white);
                }
           // tile.GetComponent<PianoTile>().pianoManager = gameObject.GetComponent<PianoManager>();
         //   tile.GetComponent<>
                //      Debug.Log(j + tile.name);
                //  if (tile.name == "pianoTilesSet1")  Debug.Log(thisTileSet.name + "index " + thisBlackTileIndex);
            }
        
    }

    private int generateIndexNearPrevIndex(int previousSetIndex, int maxRange)
    {
        int minIndex = Mathf.Max(0, previousSetIndex - 1);
      //  int maxIndex = Mathf.Min(maxRange - 1, previousSetIndex + 1);
        int maxIndex = Mathf.Min(maxRange - 1, previousSetIndex + 1);

        if(minIndex == maxIndex)
        {
            return minIndex;
        }
        else 
        {
            return Random.Range(minIndex, maxIndex + 1);
        }
    }

    public void setTileColor(GameObject tile, tileColor color)
    {
        var pianoTileComponent = tile.GetComponent<PianoTile>();
        pianoTileComponent.pianoTileColor = color;

        var tileMeshRenderer = tile.GetComponent<MeshRenderer>();

        // if this tile's color is white set white, if black set black
        if (color == tileColor.white)
        {
            tileMeshRenderer.material = whiteMat;
        }
        if (color == tileColor.black)
        {
            tileMeshRenderer.material = blackMat;
        }

        // resetting if tile is triggered so user can trigger it
        pianoTileComponent.tileIsTriggered = false;
        // initiallising tiles's pianotilecomponent script's values
        pianoTileComponent.gameManager = _gameManager;
        pianoTileComponent.pianoManager = gameObject.GetComponent<PianoManager>();
    }
    public void StartMoveTileSetCoroutine(Transform tileRow)
    {
        StartCoroutine(MoveTilesCoroutine(tileRow));
    }
    
    IEnumerator MoveTilesCoroutine(Transform tileRow)
    {
        var distToEnd = Vector3.Distance(tileRow.localPosition, _endPositionVector);
        // move tileset only if game is playing and it hasn't reached the endpoint.
        while (distToEnd > _distToTargetPosThreshold && _gameManager.gameIsPlaying)
        {
            distToEnd = Vector3.Distance(tileRow.localPosition, _endPositionVector);
         //   Debug.Log(tileRow.localPosition);

            var xPos = tileRow.localPosition.x;
            var xDest = _endPositionVector.x;
            var yPos = tileRow.localPosition.y;
            var yDest = _endPositionVector.y;
            var zPos = tileRow.localPosition.z;
            var zDest = _endPositionVector.z;


            var newX = moveToPosition(xPos, xDest);
            var newY = moveToPosition(yPos, yDest);
            var newZ = moveToPosition(zPos, zDest);

            tileRow.localPosition = new Vector3(newX, newY, newZ);

         //   Debug.Log(tileRow.name);

          //  Debug.Log((distToEnd > _distToTargetPosThreshold) + "dist " + distToEnd + "name" + tileRow.name);
            yield return null;
        }
            Debug.Log("triggered");
        
        if(_restartTileSetWhenEndPosReached)
        {            Debug.Log("finished");

            tileRow.TryGetComponent(out TileSet tileSetComponent);

            if (tileSetComponent.correctTileTriggered == false)
            {
                // getting the tile that is black in the set and triggering it to flash red/wrong
                tileRow.transform.GetChild(tileSetComponent._blackTileIndexNum).GetComponent<PianoTile>().MissedTile();
                //    tileRow.localPosition = startPosition;
                //  tileRow.GetComponent<TileSet>().ResetThisTileSet();
             //   Debug.Log("correct tile not triggered");
                tileSetDestruction(tileRow.gameObject);
            }
            else if (tileSetComponent.correctTileTriggered == true) 
            {

                tileSetDestruction(tileRow.gameObject);
                //   tileRow.localPosition = startPosition;
                //  tileRow.GetComponent<TileSet>().ResetThisTileSet();
            }
        }

       // if (_restartWhenTargetPosReached)
       // {

         //   StartMoveCoroutine();
       // }
    }


    private float moveToPosition(float axisCurrentPosition, float axisEndPosition)
    {
        if (axisCurrentPosition < axisEndPosition)
        {

            axisCurrentPosition = axisCurrentPosition + (_moveSpeed);
        }
        else if (axisCurrentPosition > axisEndPosition)
        {

            axisCurrentPosition = axisCurrentPosition - (_moveSpeed);
        }
        else
        {
            //  Debug.Log("dist within range");
        }

        return axisCurrentPosition;
    }

    private void tileSetDestruction(GameObject tileSet)
    {
        StartCoroutine(DelayDestroyTileSetCoroutine(tileSet, _timeBeforeDestruction));
    }
    IEnumerator DelayDestroyTileSetCoroutine(GameObject tileSet, float timeToDelay)
    {
        Debug.Log("waiting to destroy");
        yield return new WaitForSeconds(timeToDelay);

        Debug.Log("triggering destroy");
        tileSet.GetComponent<TileSet>().ResetThisTileSet();

        // eventToCall.Invoke();
    }

    /*IEnumerator DelayFunctionCouroutine(UnityEvent eventToCall, float timeToDelay)
    {
        yield return new WaitForSeconds(timeToDelay);
        eventToCall.Invoke();
    }

    */
}
