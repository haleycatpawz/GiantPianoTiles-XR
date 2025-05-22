using UnityEngine;

public class AudioVisualizerCube : MonoBehaviour
{
    public int _band;
    public float _startScale, _scaleMultiplier;
    [SerializeField] AudioPeer _audioPeer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (_audioPeer._frequencyBand[_band] * _scaleMultiplier) * _startScale);      
      //  transform.localScale = new Vector3(transform.localScale.x, (AudioPeer._frequencyBand[_band] * _scaleMultiplier) * _startScale, transform.localScale.z);      
    }
}
