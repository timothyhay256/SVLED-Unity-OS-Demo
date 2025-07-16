using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BulletCollision : MonoBehaviour
{
    public TMP_Text score;
    public TMP_Text deaths;

    public SpawnObjects spawner;
    public GameObject outwardAnim;
    public GameObject outwardAnimDeath;

    void Start()
    {
        score = GameObject.Find("Score").GetComponent<TMP_Text>();
        deaths = GameObject.Find("Deaths").GetComponent<TMP_Text>();
        outwardAnim = GameObject.Find("Exploding Sphere");
        outwardAnimDeath = GameObject.Find("Exploding Evil");

        spawner = GameObject.Find("Object Spawner").GetComponent<SpawnObjects>();
    }

    void OnCollisionEnter(Collision col)
    {
        var name = col.gameObject.name;

        if (name == "Object Eater")
        {
            spawner.deathCount++;
            deaths.text = "Deaths: " + spawner.deathCount;

            spawner.scoreCount = Math.Max(0, spawner.scoreCount - 1);
            score.text = "Score: " + spawner.scoreCount;

            GameObject effect = Instantiate(outwardAnimDeath, transform.position, transform.rotation);
            effect.GetComponent<ExpandingSphereEffectSingleColor>().enabled = true;
            Destroy(gameObject.transform.parent.gameObject);
        }
        else if (name.Contains("Left") || name.Contains("Right") || name.Contains("Thumb") || name.Contains("Index") || name.Contains("Middle") || name.Contains("Ring") || name.Contains("Pinky"))
        {
            spawner.scoreCount++;
            score.text = "Score: " + spawner.scoreCount;

            GameObject effect = Instantiate(outwardAnim, transform.position, transform.rotation);
            effect.GetComponent<ExpandingSphereEffect>().enabled = true;
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
