using UnityEngine;

public class ClearLEDS : MonoBehaviour
{
    public bool clear;
    public GameObject UI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UI.GetComponent<LoadScene>().ClearLEDS();
    }

    // Update is called once per frame
    void Update()
    {
        if (clear) {
            clear = false;

            UI.GetComponent<LoadScene>().ClearLEDS();
        }   
    }
}
