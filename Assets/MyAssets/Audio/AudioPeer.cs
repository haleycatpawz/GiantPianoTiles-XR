using UnityEngine;
using System.Collections;
using System;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    AudioSource _audioSource;
    public static float[] _samples = new float[512];
    public static float[] _frequencyBand = new float[8];
    public static float[] _bandBuffer = new float[8];
    float[] _bufferDecrease = new float[8];
    public float[] _frequencyBandHighest = new float[8];
    public float[] _audioBand = new float[8];
    public float[] _audioBandBuffer = new float[8];
    public float _Amplitude, _AmplitudeBuffer;
    public float _AmplitudeHighest;
    [SerializeField] float _audioProfileFloat = 0.1f;
    [SerializeField] float _sensitivity = 100f;
    [SerializeField] float _minFrequencyBandHighest = 0.01f; // Minimum value for _frequencyBandHighest

    // Smoothing 
     float[] _smoothedFrequencyBand = new float[8];
    [SerializeField] float _smoothingFactor = 0.1f;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        AudioProfile(_audioProfileFloat);
    }

    void Update()
    {
        if (_gameManager.gameIsPlaying)
        {
            GetSpectrumAudioSource();
            MakeFrequencyBands();
            bandBuffer();
            CreateAudioBands();
            GetAmplitude();
        }
    }

    private void AudioProfile(float audioProfile)
    {
        for (int i = 0; i < _frequencyBandHighest.Length; i++)
        {
            _frequencyBandHighest[i] = audioProfile;
        }
    }

    private void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            currentAmplitude += _audioBand[i];
            currentAmplitudeBuffer += _audioBandBuffer[i];

            if (currentAmplitude > _AmplitudeHighest)
            {
                _AmplitudeHighest = currentAmplitude;
            }
            _Amplitude = currentAmplitudeBuffer / _AmplitudeHighest;
            _AmplitudeBuffer = currentAmplitude / _AmplitudeHighest;
        }
    }

    private void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_frequencyBand[i] > _frequencyBandHighest[i])
            {
                _frequencyBandHighest[i] = _frequencyBand[i];
            }
            float highestValue = Mathf.Max(_frequencyBandHighest[i], _minFrequencyBandHighest);
            _audioBand[i] = (_frequencyBand[i] / highestValue);
            _audioBandBuffer[i] = (_bandBuffer[i] / highestValue);
        }
    }

    private void bandBuffer()
    {
        for (int i = 0; i < 8; ++i)
        {
            if (_frequencyBand[i] > _bandBuffer[i])
            {
                _bandBuffer[i] = _frequencyBand[i];
                _bufferDecrease[i] = 0.005f;
            }
            if (_frequencyBand[i] < _bandBuffer[i])
            {
                _bandBuffer[i] -= _bufferDecrease[i];
                _bufferDecrease[i] *= 1.2f;
            }
        }
    }

    private void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    private void MakeFrequencyBands()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count];
                count++;
                if (count >= _samples.Length) break;
            }
            average /= sampleCount;
            _frequencyBand[i] = average * _sensitivity;

            // Smoothing (optional)
            // _smoothedFrequencyBand[i] = Mathf.Lerp(_smoothedFrequencyBand[i], _frequencyBand[i], _smoothingFactor);
            // _frequencyBand[i] = _smoothedFrequencyBand[i];
        }
    }
}
