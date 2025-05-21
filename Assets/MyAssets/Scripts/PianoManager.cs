using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
public class PianoManager : MonoBehaviour
{
    [SerializeField] public Material whiteMat;
    [SerializeField] public Material blackMat;
    [SerializeField] public Material triggerMatColor;

    [SerializeField] GameObject pianoTilesSet;

    [SerializeField] float _moveSpeed = 1;
    // [SerializeField] Vector3 _moveDirection;
    [SerializeField] Transform _startPosition;

    [SerializeField] bool useEndPositionTransform = false;
    [SerializeField] Vector3 _endPositionVector;
    [SerializeField] Transform _endPositionTransform;
    [SerializeField] float _distToTargetPosThreshold;

    [SerializeField] bool _restartTileSetWhenEndPosReached = false;


    private Vector3 startPosition;
    public enum tileColor
    {
        white = 0,
        black = 1,
    }

    public void SetAllTiles()
    {
        var childCount = pianoTilesSet.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            GameObject previousTileSet = null;

            if (childCount > 0)
            {
                if (i > 0)
                {
                    previousTileSet = pianoTilesSet.transform.GetChild(i - 1).gameObject;
                }
                else // i == 0
                {
                    previousTileSet = pianoTilesSet.transform.GetChild(childCount - 1).gameObject;
                }

                GameObject currentTileSet = pianoTilesSet.transform.GetChild(i).gameObject;

                if (currentTileSet != null)
                {
                    SetColorOfTilesSet(currentTileSet, previousTileSet);
                    StartCoroutine(MoveTilesCoroutine(currentTileSet.transform));

                    if (i== 0)
                    {
                        Debug.Log("set section 1" + currentTileSet.name);
                    }
                }
            }
        }
    }


    public void SetColorOfTilesSet(GameObject thisTileSet, GameObject previousTileSet)
    {
        var childrenInTileSet = thisTileSet.transform.childCount;

      //  int randomTileIndex = Random.Range(0, childrenInTileSet);
        int prevBlackTileIndex = previousTileSet.GetComponent<TileSet>().blackTileIndexNum;
        int thisBlackTileIndex = generateIndexNearPrevIndex(prevBlackTileIndex, childrenInTileSet);

        thisTileSet.GetComponent<TileSet>().blackTileIndexNum = thisBlackTileIndex;

        //  Debug.Log(thisBlackTileIndex);

      //  if("pianoTilesSet1" == thisTileSet.name) Debug.Log(thisTileSet.name + "index " + thisBlackTileIndex);
     
        for (int j = 0; j < childrenInTileSet; j++)
        {
            GameObject tile = thisTileSet.transform.GetChild(j).gameObject;

            if (j == thisBlackTileIndex)
            {
                setTileColor(tile, tileColor.black);
                if (thisTileSet.name == "pianoTilesSet1")  Debug.Log(thisTileSet.name + "index " + thisBlackTileIndex);

            }
            else
            {
                setTileColor(tile, tileColor.white);
            }

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
    }


    void Start()
    {
        SetAllTiles(); Debug.Log("started");

        startPosition = _startPosition.localPosition;

        if (useEndPositionTransform && _endPositionTransform != null)
        {
            _endPositionVector = _endPositionTransform.localPosition;
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartMoveTileSetCoroutine(Transform tileRow)
    {
        StartCoroutine(MoveTilesCoroutine(tileRow));
    }
    
    IEnumerator MoveTilesCoroutine(Transform tileRow)
    {
        var distToEnd = Vector3.Distance(tileRow.localPosition, _endPositionVector);
        while (distToEnd > _distToTargetPosThreshold)
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

         //   Debug.Log((distToEnd > _distToTargetPosThreshold) + "dist " + distToEnd + "name" + tileRow.name);
            yield return null;
        }
            Debug.Log("triggered");
        
        if(_restartTileSetWhenEndPosReached)
        {            Debug.Log("finished");

            tileRow.TryGetComponent(out TileSet tileSetComponent);

            if (tileSetComponent.correctTileTriggered == false)
            {
                // getting the tile that is black in the set and triggering it to flash red/wrong
                tileRow.transform.GetChild(tileSetComponent.blackTileIndexNum).GetComponent<PianoTile>().MissedTile();
                tileRow.localPosition = startPosition;
                tileRow.GetComponent<TileSet>().ResetThisTileSet();
            }
            else if (tileSetComponent.correctTileTriggered == true) 
            {
                tileRow.localPosition = startPosition;
                tileRow.GetComponent<TileSet>().ResetThisTileSet();
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

}
