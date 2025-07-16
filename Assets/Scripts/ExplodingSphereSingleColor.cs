using UnityEngine;
using System.Collections.Generic;

public class ExpandingSphereEffectSingleColor : MonoBehaviour
{
    [Header("Effect Settings")]
    public GameObject sendManager; // Assign in Inspector
    public Color effectColor = Color.red;
    public float maxRadius = 10f;
    public float shellThickness = 1f;
    public float speed = 5f;

    private float currentRadius = 0f;
    private List<SendCollision> allLEDs = new();
    private HashSet<SendCollision> affected = new();
    private HashSet<SendCollision> cleared = new();

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
                    led.ApplyColor(effectColor);
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
                        Renderer rend = led.GetComponent<Renderer>();
                        if (rend != null)
                            rend.material.color = Color.white;
                    }

                    led.QueueClear();
                    cleared.Add(led);
                }
            }
        }

        if (currentRadius >= maxRadius)
        {
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
