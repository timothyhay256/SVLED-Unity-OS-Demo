using UnityEngine;
using System.Numerics;
using System;

public class MicVisualizerWithFFT : MonoBehaviour
{
    [Header("Mic Settings")]
    [Tooltip("Leave blank for default mic.")]
    public string microphoneName = "";

    [Header("Visualizer Settings")]
    public GameObject[] visualizerObjects;
    public int sampleSize = 512; // Must be power of 2
    public float heightMultiplier = 10.0f;
    public float baselineHeight = 1.0f;

    [Header("Smoothing")]
    [Range(0f, 1f)]
    public float smoothingFactor = 0.8f; // Higher = more smoothing

    private AudioClip micClip;
    private float[] sampleBuffer;
    private float[] spectrumData;
    private float[] smoothedData;

    void Start()
    {
        if (!Mathf.IsPowerOfTwo(sampleSize))
        {
            Debug.LogError("Sample size must be a power of 2.");
            return;
        }

        if (Microphone.devices.Length == 0)
        {
            Debug.LogWarning("No microphone devices found.");
            return;
        }

        Debug.Log("Available Microphones:");
        foreach (var device in Microphone.devices)
            Debug.Log($" - {device}");

        if (string.IsNullOrEmpty(microphoneName))
        {
            microphoneName = Microphone.devices[0];
            Debug.Log($"Using default microphone: {microphoneName}");
        }

        micClip = Microphone.Start(microphoneName, true, 1, 44100);
        sampleBuffer = new float[sampleSize];
        spectrumData = new float[sampleSize];
        smoothedData = new float[visualizerObjects.Length];
    }

    void Update()
    {
        if (micClip == null || !Microphone.IsRecording(microphoneName)) return;

        int micPos = Microphone.GetPosition(microphoneName) - sampleSize;
        if (micPos < 0) return;

        micClip.GetData(sampleBuffer, micPos);
        ComputeFFT(sampleBuffer, spectrumData);
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        int bands = visualizerObjects.Length;
        int spectrumSegmentSize = sampleSize / 2 / bands;

        for (int i = 0; i < bands; i++)
        {
            float avg = 0f;
            int start = i * spectrumSegmentSize;
            int end = start + spectrumSegmentSize;

            for (int j = start; j < end; j++)
                avg += spectrumData[j];

            avg /= spectrumSegmentSize;

            // Apply exponential smoothing
            smoothedData[i] = Mathf.Lerp(smoothedData[i], avg, 1f - smoothingFactor);

            UnityEngine.Vector3 scale = visualizerObjects[i].transform.localScale;
            scale.y = baselineHeight + smoothedData[i] * heightMultiplier;
            visualizerObjects[i].transform.localScale = scale;
        }
    }

    void ComputeFFT(float[] signal, float[] outputMagnitude)
    {
        int n = signal.Length;
        Complex[] fftBuffer = new Complex[n];

        for (int i = 0; i < n; i++)
            fftBuffer[i] = new Complex(signal[i], 0);

        FFT(fftBuffer);

        for (int i = 0; i < n; i++)
            outputMagnitude[i] = (float)fftBuffer[i].Magnitude;
    }

    void FFT(Complex[] buffer)
    {
        int n = buffer.Length;
        int bits = (int)Math.Log(n, 2);

        // Bit-reverse
        for (int j = 1, i = 0; j < n; j++)
        {
            int bit = n >> 1;
            for (; (i & bit) != 0; bit >>= 1)
                i ^= bit;
            i ^= bit;

            if (j < i)
            {
                var temp = buffer[j];
                buffer[j] = buffer[i];
                buffer[i] = temp;
            }
        }

        // Cooley-Tukey
        for (int len = 2; len <= n; len <<= 1)
        {
            double angle = -2 * Math.PI / len;
            Complex wlen = new Complex(Math.Cos(angle), Math.Sin(angle));

            for (int i = 0; i < n; i += len)
            {
                Complex w = Complex.One;
                for (int j = 0; j < len / 2; j++)
                {
                    Complex u = buffer[i + j];
                    Complex v = w * buffer[i + j + len / 2];
                    buffer[i + j] = u + v;
                    buffer[i + j + len / 2] = u - v;
                    w *= wlen;
                }
            }
        }
    }
}
