using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class AudioVisualizerCube : MonoBehaviour
{
    public int _band;
    public float _startScale, _scaleMultiplier;
    [SerializeField] AudioPeer _audioPeer;
    public bool _useBuffer;


    [SerializeField] float receivedValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {/*
        if (_useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (_audioPeer._bandBuffer[_band] * _scaleMultiplier) * _startScale);
        }
        if (!_useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (_audioPeer._frequencyBand[_band] * _scaleMultiplier) * _startScale);
        }
        
        */
        if (_useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (_audioPeer._audioBandBuffer[_band] * _scaleMultiplier) * _startScale);
        }
        if (!_useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (_audioPeer._audioBand[_band] * _scaleMultiplier) * _startScale);
        }
        
        receivedValue = _audioPeer._audioBand[_band];
        //  transform.localScale = new Vector3(transform.localScale.x, (AudioPeer._frequencyBand[_band] * _scaleMultiplier) * _startScale, transform.localScale.z);      
    }
}
