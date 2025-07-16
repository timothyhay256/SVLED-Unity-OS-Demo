using UnityEngine;
using Leap;
using System.Collections;

public class PaintSystem : MonoBehaviour
{
    public GameObject brushElement;
    public PinchDetector pinchIndex;
    public GameObject ledHolder;

    public PinchDetector pinchIndexRight;

    public Color activeColor;
    private Hand left;
    private Hand right;

    public float cycleSpeed;

    private bool pinchActive;
    private bool blockClear = false;
    private float hue = 0f;
    void Start()
    {
        left = Hands.Provider.GetHand(Chirality.Left);
    }

    // Update is called once per frame
    void Update()
    {
        left = Hands.Provider.GetHand(Chirality.Left);
        right = Hands.Provider.GetHand(Chirality.Right);

        if (left != null)
        {
            float grabStrength = left.GrabStrength;

            if (grabStrength >= 1f)
            {
                hue += Time.deltaTime * cycleSpeed;
                hue = hue % 1f; // Wrap hue back to 0-1 range

                Debug.Log(hue);

                activeColor = Color.HSVToRGB(hue, 1f, 1f);
            }

            if (pinchIndex.IsPinching)
            {
                // Debug.Log("Index pinching");
                activeColor.a = 1f;
                brushElement.tag = "Brush";
            }
            else
            {
                activeColor.a = .4f;
                brushElement.tag = "BrushHidden";
            }

            brushElement.GetComponent<Renderer>().material.color = activeColor;
        }

        if (pinchIndexRight.IsPinching && !blockClear && right != null)
        {
            Debug.Log("Clearing LEDS");
            StartCoroutine(clearLEDS());
            blockClear = true;
        }
    }

    IEnumerator clearLEDS() {
        ledHolder.transform.GetChild(1).gameObject.GetComponent<SendCollision>().clearLoop = true;

        for (int i = 0; i < ledHolder.transform.childCount; i++)
        {
            ledHolder.transform.GetChild(i).GetComponent<SendCollision>().ClearSingleLED();
        }
        yield return new WaitForSeconds(3.0f);
        blockClear = false;
    }
}
