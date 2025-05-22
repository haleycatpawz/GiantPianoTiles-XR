using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class AudioVisualizerCube : MonoBehaviour
{
    public int _band;
    public float _startScale = 1f;
    public float _scaleMultiplier = 1f;
    [SerializeField] AudioPeer _audioPeer;
    public bool _useBuffer;
    [SerializeField] float _maxScale = 5f; // Maximum scale to prevent extreme growth

    [SerializeField] float receivedValue;

    void Start()
    {
        if (_audioPeer == null)
        {
            Debug.LogError("AudioPeer not assigned to AudioVisualizerCube!");
            enabled = false;
        }
    }

    void Update()
    {
        if (_audioPeer != null && _band < _audioPeer._audioBand.Length)
        {
            float audioValue = _useBuffer ? _audioPeer._audioBandBuffer[_band] : _audioPeer._audioBand[_band];
            audioValue = Mathf.Clamp(audioValue, 0f, 1f); // Clamp the normalized audio value

            float scaleFactor = _startScale + (audioValue * _scaleMultiplier);
            scaleFactor = Mathf.Max(_startScale, Mathf.Min(scaleFactor, _maxScale)); // Ensure scale doesn't go below start or above max

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, scaleFactor);
            receivedValue = audioValue; // For debugging
        }
    }
}

