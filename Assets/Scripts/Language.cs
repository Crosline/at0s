using TMPro;
using UnityEngine;

#pragma warning disable 0649

public class Language : MonoBehaviour {
    // Start is called before the first frame update

    [SerializeField] private string turkishText;
    [SerializeField] private string englishText;
    [SerializeField] private TMP_Text text;
    void Awake() {
        text = GetComponent<TMP_Text>();
        UpdateLanguage(GameSettings.Instance.language);
    }

    // Update is called once per frame
    public void UpdateLanguage(bool lang) {
        if (lang) {
            text.text = englishText;
        } else {
            text.text = turkishText;
        }
    }
}
