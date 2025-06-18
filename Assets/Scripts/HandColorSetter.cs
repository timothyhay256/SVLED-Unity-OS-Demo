using UnityEngine;
using System.Collections;

public class ChildComponentAdder : MonoBehaviour
{
    public GameObject parent;
    public Color32 targetColorLeft;
    public Color32 targetColorRight;

    private void Start()
    {
        StartCoroutine(WaitForChildrenAndAddComponent());
    }

    private IEnumerator WaitForChildrenAndAddComponent()
    {
        yield return new WaitUntil(() => parent.transform.childCount >= 2);

        foreach (Transform child in parent.transform)
        {
            string childName = child.gameObject.name;

            var holder = child.gameObject.AddComponent<HandColorHolder>();

            if (childName.Contains("Left"))
            {
                holder.targetColor = targetColorLeft;
            }
            else if (childName.Contains("Right"))
            {
                holder.targetColor = targetColorRight;
            }
        }
    }
}
