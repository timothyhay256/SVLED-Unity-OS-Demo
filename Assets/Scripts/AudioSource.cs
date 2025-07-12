using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LiveMicrophoneAudio : MonoBehaviour
{
    [Tooltip("Name of the microphone to use. Leave empty for default.")]
    public string microphoneName = "";

    [Tooltip("Enable microphone loopback on start.")]
    public bool playOnStart = true;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // List all available microphones
        if (Microphone.devices.Length == 0)
        {
            Debug.LogWarning("No microphone devices found.");
            return;
        }

        Debug.Log("Available Microphones:");
        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            Debug.Log($"[{i}] {Microphone.devices[i]}");
        }

        // If no mic specified, use default (first one in the list)
        if (string.IsNullOrEmpty(microphoneName))
        {
            microphoneName = Microphone.devices[0];
            Debug.Log($"Using default microphone: {microphoneName}");
        }

        // Start capturing audio from the microphone
        audioSource.clip = Microphone.Start(microphoneName, true, 1, 44100);
        audioSource.loop = true;

        // Wait until the mic starts recording before playing back
        StartCoroutine(WaitForMicStartAndPlay());
    }

    private System.Collections.IEnumerator WaitForMicStartAndPlay()
    {
        // Wait until the microphone is ready
        while (Microphone.GetPosition(microphoneName) <= 0)
        {
            yield return null;
        }

        if (playOnStart)
        {
            audioSource.Play();
            Debug.Log("Microphone audio playback started.");
        }
    }
}
