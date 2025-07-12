using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class RotateAndMoveLeft : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 100f, 0f); // degrees per second
    public float moveSpeed = 1f;

    void Update()
    {
        // Rotate the GameObject
        Quaternion deltaRotation = Quaternion.Euler(rotationSpeed * Time.deltaTime);
        transform.rotation *= deltaRotation;

        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }
}
