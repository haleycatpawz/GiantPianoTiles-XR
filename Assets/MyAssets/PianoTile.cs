using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoTile : MonoBehaviour
{
    // piano manager script that defines the enums, materials, etc. which
    // we will reference here to ensure all piano tiles are alike and can easly be adjusted dynamically
    public PianoManager pianoManager;

    // this tile's color
    [SerializeField] public PianoManager.tileColor pianoTileColor;

    // mesh renderer for this tile
    private MeshRenderer meshRenderer;

    // variables that hold the materials used for white & black that are defined in piano manager script
   // private Material whiteMat;
   // private Material blackMat;


    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        //  whiteMat = pianoManager.whiteMat;
        // blackMat = pianoManager.blackMat;

        //  setTileColor(pianoTileColor);

    }
    /*
    // function that can be called to set the tile to using either a white or black material
    public void setTileColor(PianoManager.tileColor color)
    {
        pianoTileColor = color;
        // if this tile's color is white set white, if black set black
        if (pianoTileColor == PianoManager.tileColor.white)
        {
            meshRenderer.material = whiteMat;
        }
        if (pianoTileColor == PianoManager.tileColor.black)
        {
            meshRenderer.material = blackMat;
        }
    }

    */

    private void OnCollisionEnter(Collision collision)
    {
        var currentColorMat = meshRenderer.material;



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
