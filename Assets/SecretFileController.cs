using System.Collections.Generic;
using UnityEngine;

public class SecretFileController : MonoBehaviour {

    public GameObject[] colliders;

    void Start() {
        DisableAll();
    }


    public void DisableAll() {
        for (int i = 0; i < colliders.Length; i++) {
            colliders[i].SetActive(false);
        }
    }

    public void EnableAll() {
        for (int i = 0; i < colliders.Length; i++) {
            if (i == 1) {
                if (PlayerPrefs.GetInt("gizli1", 0) == 0) {
                    continue;
                }
            }
            colliders[i].SetActive(true);
        }
    }

}
