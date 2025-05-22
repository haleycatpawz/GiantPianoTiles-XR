using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour
{
    [SerializeField] AudioPeer _audioPeer;
    public float _startScale, _maxScale;
    public bool _useBuffer;
    Material _material;
    public float _red, _green, _blue;
    void Start()
    {
        _material = GetComponent<MeshRenderer>().materials[0];        
    }
    void Update()
    {
        // setting entire scale based on amplitude, so like radius
        if (!_useBuffer)
        {
            transform.localScale = new Vector3((_audioPeer._Amplitude * _maxScale) + _startScale, (_audioPeer._Amplitude * _maxScale) + _startScale, (_audioPeer._Amplitude * _maxScale) + _startScale);

        }
        if (_useBuffer)
        {
            transform.localScale = new Vector3((_audioPeer._AmplitudeBuffer * _maxScale) + _startScale, (_audioPeer._AmplitudeBuffer * _maxScale) + _startScale, (_audioPeer._AmplitudeBuffer * _maxScale) + _startScale);

        }
        
    }
}
