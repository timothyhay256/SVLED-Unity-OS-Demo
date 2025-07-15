using UnityEngine;
using Leap;

public class PaintSystem : MonoBehaviour
{
    public GameObject brushElement;
    public PinchDetector pinchIndex;
    public GameObject ledHolder;

    public PinchDetector pinchIndexRight;

    public Color activeColor;
    private Hand left;

    public float cycleSpeed;

    private bool pinchActive;
    private float hue = 0f;
    void Start()
    {
        left = Hands.Provider.GetHand(Chirality.Left);
    }

    // Update is called once per frame
    void Update()
    {
        left = Hands.Provider.GetHand(Chirality.Left);

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

        if (pinchIndexRight.IsPinching)
        {
            ledHolder.transform.GetChild(0).gameObject.GetComponent<SendCollision>().clearLoop = true;

            for (int i = 0; i < ledHolder.transform.childCount; i++)
            {
                ledHolder.transform.GetChild(i).GetComponent<SendCollision>().ApplyColor(Color.white);
                ledHolder.transform.GetChild(i).GetComponent<SendCollision>().collisionStack.Clear();
                ledHolder.transform.GetChild(i).GetComponent<SendCollision>().activeColliders.Clear();
                ledHolder.transform.GetChild(i).GetComponent<SendCollision>().brushColor = null;
                ledHolder.transform.GetChild(i).GetComponent<SendCollision>().sendClear = true;
            }
        }
    }
}
