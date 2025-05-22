using UnityEngine;
using System.Collections;
using System;
using System.Linq;

[RequireComponent (typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{

    AudioSource _audioSource;
    // changing 512 to 64 512 / 8
    public float[] _samples = new float[512];
    //public static float[] _samples = new float[512];

    public float[] _frequencyBand = new float[8];

    public float[] _bandBuffer = new float[8];
   // public static float[] _bandBuffer = new float[8];
    float[] _bufferDecrease = new float[8];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
           _audioSource = GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();

        bandBuffer();
    }

    private void bandBuffer()
    {
        for(int i = 0; i < 8;  ++i)
        {
            if (_frequencyBand[i] > _bandBuffer[i])
            {
                _bandBuffer[i] = _frequencyBand[i];
                _bufferDecrease[i] = 0.005f;
            }
            if(_frequencyBand[i] < _bandBuffer[i])
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
        /*
         * 22050 / 512 = 43 hertz per sample
         * 20 - 60 htz
         * 250 - 500 
         * 500 - 2000
         * 2000 -400
         * 4000 - 6000
         * 6000 - 20000 hertz
         * 
         * 0 - 2 = 86 hertz
         * 1 - 4 = 172 hertz - 87 -258 hertz range
         * 3 * 8 = 344 - 259 - 602
         * */

        int count = 0;
        // 8 because we have 8 bandss
        for (int i = 0; i < 8; i++)
      //  for (int i = 0; i < _frequencyBand.Length; i++)
        {

            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }

            for (int h = 0; h < sampleCount; h++)
            {

                average += _samples[count] * count + 1;
                count++;
            }

            average /= count;
            _frequencyBand[i] = average * 10;
        }
    }


}
