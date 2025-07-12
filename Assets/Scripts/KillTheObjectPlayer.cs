using UnityEngine;
using System.Collections;

public class KillTheObjectPlayer : MonoBehaviour
{
    public float x_interval;
    public float y_interval;
    public float z_interval;

    public int x_lim;
    public int y_lim;
    public int z_lim;

    public float duration = 0.3f;

    private int current_x;
    private int current_y;
    private int current_z;
    private bool isMoving = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isMoving && current_z != 0)
        {
            current_z--;
            Vector3 targetPosition = transform.position + new Vector3(0, 0, -z_interval);
            StartCoroutine(SmoothMove(transform.position, targetPosition, duration));
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && !isMoving && current_z != z_lim)
        {
            current_z++;
            Vector3 targetPosition = transform.position + new Vector3(0, 0, +z_interval);
            StartCoroutine(SmoothMove(transform.position, targetPosition, duration));
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isMoving && current_x != 0)
        {
            current_x--;
            Vector3 targetPosition = transform.position + new Vector3(-x_interval, 0, 0);
            StartCoroutine(SmoothMove(transform.position, targetPosition, duration));
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && !isMoving && current_x != x_lim)
        {
            current_x++;
            Vector3 targetPosition = transform.position + new Vector3(+x_interval, 0, 0);
            StartCoroutine(SmoothMove(transform.position, targetPosition, duration));
        }

        if (Input.GetKeyDown("z") && !isMoving && current_y != y_lim)
        {
            current_y++;
            Vector3 targetPosition = transform.position + new Vector3(0, +y_interval, 0);
            StartCoroutine(SmoothMove(transform.position, targetPosition, duration));
        }

        if (Input.GetKeyDown("x") && !isMoving && current_y != 0)
        {
            current_y--;
            Vector3 targetPosition = transform.position + new Vector3(0, -y_interval, 0);
            StartCoroutine(SmoothMove(transform.position, targetPosition, duration));
        }
    }

    IEnumerator SmoothMove(Vector3 start, Vector3 end, float duration)
    {
        isMoving = true;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Easing: SmoothStep for ramp-up and ramp-down
            float easedT = t * t * (3f - 2f * t); // SmoothStep
            transform.position = Vector3.Lerp(start, end, easedT);

            yield return null;
        }

        transform.position = end; // Ensure it snaps exactly at the end
        isMoving = false;
    }
}
