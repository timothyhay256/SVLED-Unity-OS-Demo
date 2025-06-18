using UnityEngine;

public class BoundsMarker : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject LEDHolder;
    public bool spawn;

    void Start()
    {

    }

    void Update()
    {
        if (spawn)
        {
            spawn = false;
            Bounds bounds = GetMaxBounds(LEDHolder);

            Vector3 center = bounds.center;
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            Instantiate(wallPrefab, new Vector3(min.x, center.y, center.z), Quaternion.identity, transform);
            Instantiate(wallPrefab, new Vector3(max.x, center.y, center.z), Quaternion.identity, transform);
            Instantiate(wallPrefab, new Vector3(center.x, center.y, min.z), Quaternion.identity, transform);
            Instantiate(wallPrefab, new Vector3(center.x, center.y, max.z), Quaternion.identity, transform);
            Instantiate(wallPrefab, new Vector3(center.x, min.y, center.z), Quaternion.identity, transform);
        }
    }

    Bounds GetMaxBounds(GameObject g)
    {
        var renderers = g.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(g.transform.position, Vector3.zero);
        var b = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            b.Encapsulate(r.bounds);
        }
        return b;
    }
    void OnDrawGizmos()
    {
        if (LEDHolder == null) return;
        Bounds bounds = GetMaxBounds(LEDHolder);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
