using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class PianoManager : MonoBehaviour
{
    [SerializeField] public Material whiteMat;
    [SerializeField] public Material blackMat;
    [SerializeField] public Material triggerMatColor;

    [SerializeField] GameObject pianoTilesSet;
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
            GameObject previousTileSet = transform.gameObject; // if there is previous set, get previous
            if (i >= 1)
            {
                previousTileSet = pianoTilesSet.transform.GetChild(i - 1).gameObject;

              //  Debug.Log("if = " + previousTileSet.name);
            }
            else if (i == 0)
            {
                previousTileSet = pianoTilesSet.transform.GetChild(childCount - 1).gameObject;
              //  Debug.Log("else if = " + previousTileSet.name);
            }
            
            var tileSet = pianoTilesSet.transform.GetChild(i).gameObject;
    
            if (tileSet != null)
            {
                SetTilesSet(tileSet, previousTileSet);
                Debug.Log("tile set " + tileSet.name + " prev tile set " + previousTileSet.name);
            }
        }
    }

    public void SetTilesSet(GameObject thisTileSet, GameObject previousTileSet)
    {
        var childrenInTileSet = thisTileSet.transform.childCount;

      //  int randomTileIndex = Random.Range(0, childrenInTileSet);
        int prevBlackTileIndex = previousTileSet.GetComponent<TileSet>().blackTileIndexNum;
        int thisBlackTileIndex = generateIndexNearPrevIndex(prevBlackTileIndex, childrenInTileSet);

        thisTileSet.GetComponent<TileSet>().blackTileIndexNum = thisBlackTileIndex;

        Debug.Log(thisBlackTileIndex);


        for (int j = 0; j < childrenInTileSet; j++)
        {
            GameObject tile = thisTileSet.transform.GetChild(j).gameObject;

            if (j == thisBlackTileIndex)
            {
                setTileColor(tile, tileColor.black);
            }
            else
            {
                setTileColor(tile, tileColor.white);
            }


        //    Debug.Log(tile.name);
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

    /*
    private int generateIndexNearPrevIndex(int previousSetIndex, int maxRange)
    {
        int randomTileIndex;
        int thisNewTileIndex = 0;
        bool generateTileIndex = true;

        while (generateTileIndex)
        {
            randomTileIndex = Random.Range(0, maxRange);
            // seeing if generated index is near the previous tile's index enough for the user to jump to it, if not, we generate another index
            if (randomTileIndex == previousSetIndex || randomTileIndex == previousSetIndex + 1 || randomTileIndex == previousSetIndex - 1)
            {
                thisNewTileIndex = randomTileIndex;
                generateTileIndex = false;
                
            }
        }
        return thisNewTileIndex;

    }
    */
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



    // Start is called before the first frame update
    void Start()
    {
        SetAllTiles(); Debug.Log("started");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
