using UnityEngine;

public class UpdateState : MonoBehaviour
{
    public GameObject ledHolder;

    public bool updateState;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (updateState) {
            updateState = false;
            for (int i = 0; i < ledHolder.transform.childCount; i++)
        {
            ledHolder.transform.GetChild(i).GetComponent<SendCollision>().UpdateState();
        }
        }
        
    }
}
