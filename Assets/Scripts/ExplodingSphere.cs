using UnityEngine;
using System.Collections.Generic;

public class ExpandingSphereEffect : MonoBehaviour
{
    [Header("Effect Settings")]
    public GameObject sendManager; // Assign in Inspector
    public float maxRadius = 10f;
    public float shellThickness = 1f;
    public float speed = 5f;

    private float currentRadius = 0f;
    private List<SendCollision> allLEDs = new();
    private HashSet<SendCollision> affected = new();
    private HashSet<SendCollision> cleared = new();
    public GameObject updateState;

    void Start()
    {
        if (sendManager == null)
        {
            Debug.LogError("SendManager not assigned to ExpandingSphereEffect.");
            Destroy(gameObject);
            return;
        }

        foreach (Transform child in sendManager.transform)
        {
            var led = child.GetComponent<SendCollision>();
            if (led != null)
                allLEDs.Add(led);
        }
        updateState = GameObject.Find("UpdateState");
    }

    void Update()
    {
        currentRadius += speed * Time.deltaTime;
        float innerRadius = currentRadius - shellThickness;
        float outerRadius = currentRadius;

        foreach (var led in allLEDs)
        {
            float dist = Vector3.Distance(transform.position, led.transform.position);

            // Inside shell
            if (dist >= innerRadius && dist <= outerRadius)
            {
                if (!affected.Contains(led))
                {
                    float normalized = Mathf.Clamp01(dist / maxRadius);
                    Color rainbowColor = Color.HSVToRGB(normalized, 1f, 1f);

                    led.ApplyColor(rainbowColor);
                    affected.Add(led);
                }
            }
            // Behind shell, clear
            else if (dist < innerRadius)
            {
                if (affected.Contains(led) && !cleared.Contains(led))
                {
                    if (led != null)
                    {
                        led.GetComponent<Renderer>().material.color = Color.white;
                    }
                    led.QueueClear();

                    cleared.Add(led);
                }
            }
        }

        if (currentRadius >= maxRadius)
        {
            updateState.GetComponent<UpdateState>().updateState = true;
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, currentRadius);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, Mathf.Max(currentRadius - shellThickness, 0f));
    }
}
