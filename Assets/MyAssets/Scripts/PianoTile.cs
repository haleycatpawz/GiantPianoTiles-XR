using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoTile : MonoBehaviour
{
    // piano manager script that defines the enums, materials, etc. which
    // we will reference here to ensure all piano tiles are alike and can easly be adjusted dynamically
    public PianoManager pianoManager;
    [SerializeField] GameManager gameManager;
    

    // this tile's color
    [SerializeField] public PianoManager.tileColor pianoTileColor;
    public bool tileIsTriggered = false;

  //  public Renderer targetRenderer;
    public float transitionDuration = 2f;

    public Color startColor;
    public Color endColorCorrect = Color.green;
    public Color endColorWrong = Color.red;

    // mesh renderer for this tile
    private MeshRenderer meshRenderer;


    // variables that hold the materials used for white & black that are defined in piano manager script
   // private Material whiteMat;
   // private Material blackMat;


    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

       // var currentColorMat = meshRenderer.material;
        startColor = meshRenderer.material.color;

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

       // StartColorTransition();
        Debug.Log("collided with" + collision.gameObject.name);

       
        if (meshRenderer == null)
        {
            Debug.LogError("Target Renderer not assigned!");
            return;
        }

        if (tileIsTriggered)
        {

        }

        if (pianoTileColor == PianoManager.tileColor.black)
        {

            CorrectTile();
            transform.parent.GetComponent<TileSet>().correctTileTriggered = true;

        }
        else
        {
            MissedTile();
          //  transform.parent.GetComponent<TileSet>().correctTileTriggered = false;
        }

    }


    private void CorrectTile()
    {
        gameManager.IncreaseGameScore();
        StartCoroutine(TransitionColor(startColor, endColorCorrect, transitionDuration));
        tileIsTriggered = true;
    }

    public void MissedTile()
    {
        gameManager.addStrike();
        StartCoroutine(TransitionColor(startColor, endColorWrong, transitionDuration));
        tileIsTriggered = true;
    }

    IEnumerator TransitionColor(Color fromColor, Color toColor, float duration)
    {
        if (meshRenderer == null || meshRenderer.material == null)
        {
            yield break; // Exit the coroutine if the renderer or material is missing
        }

        float timeElapsed = 0f;
        Material materialToChange = meshRenderer.material; // Get a reference to the material

        while (timeElapsed < duration)
        {
            // Lerp (Linear Interpolation) between the start and end colors
            Color currentColor = Color.Lerp(fromColor, toColor, timeElapsed / duration);

            // Apply the new color to the material
            materialToChange.color = currentColor;

            // Increment the elapsed time
            timeElapsed += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final color is set exactly
        materialToChange.color = toColor;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
