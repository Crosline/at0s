using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnFood : MonoBehaviour {
    // Food Prefab
    public GameObject foodPrefab;

    private int score = 0;

    // Borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    public TextMesh scoreText;

    private List<GameObject> spawnedFoods = new List<GameObject>();

    // Use this for initialization
    void Start () {
        Spawn();
    }

    public void Eat(GameObject g) {

        if (spawnedFoods.Contains(g)) {
            spawnedFoods.Remove(g);
        }
        Destroy(g);


        scoreText.text = score.ToString();

        score++;

        if (score == 11) {



            // WİN



            return;
        }


        Spawn();
    }

    public void ResetFood() {
        foreach (GameObject g in spawnedFoods)
            Destroy(g);

        spawnedFoods.Clear();

        score = 0;

        scoreText.text = score.ToString();

        Spawn();

    }


    // Spawn one piece of food
    public void Spawn() {


        // x position between left & right border
        float x = Random.Range(borderLeft.position.x,
                                  borderRight.position.x);

        Debug.Log("---");
        Debug.Log(x);
        // y position between top & bottom border
        float y = Random.Range(borderBottom.position.y,
                                  borderTop.position.y);

        Debug.Log(y);

        Debug.Log("---");

        GameObject temp = Instantiate(foodPrefab, new Vector3(x, y, 0f),
                    Quaternion.identity, transform.parent);

        if (spawnedFoods.Contains(temp)) {
            Destroy(temp);
            Spawn();
        } else {
            // Instantiate the food at (x, y)
            spawnedFoods.Add(temp); // default rotation
        }
    }


}