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


    public GameObject gizliDosya;
    public GameObject snek;

    private List<GameObject> spawnedFoods = new List<GameObject>();

    // Use this for initialization
    void Start () {
        Spawn();
        snek.SetActive(false);
    }

    public void Eat(GameObject g) {

        if (spawnedFoods.Contains(g)) {
            spawnedFoods.Remove(g);
        }
        Destroy(g);



        score++;

        if (score == 5) {

            GlitchController.Instance.Glitcher(0.1f);

            PlayerPrefs.SetInt("gizli1", 1);

            transform.parent.gameObject.GetComponent<SecretFileController>().EnableAll();
            gizliDosya.SetActive(true);
            transform.parent.gameObject.SetActive(false);

            return;
        }

        scoreText.text = score.ToString();
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

        // y position between top & bottom border
        float y = Random.Range(borderBottom.position.y,
                                  borderTop.position.y);


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