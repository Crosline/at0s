using System.Collections;
using UnityEngine;

public class GlitchController : MonoBehaviour {
    public static GlitchController Instance;
    public Material mat;

    void Awake() {
        Instance = this;
        mat.SetInt("Boolean_fbe84cbd1dfe42ce9ac891cdb88f2915", 1);
    }

    private bool isGlitching = true;

    public void ToggleGlitch() {
        mat.SetInt("Boolean_fbe84cbd1dfe42ce9ac891cdb88f2915", isGlitching ? 0 : 1);
        isGlitching = !isGlitching;
    }

    public void Glitcher(float time = 1.0f) {

        StartCoroutine(GlitchbyTime(time));

    }
    public void GlitcherAfter(float time = 1.0f) {

        StartCoroutine(StartGlitchAfter(time));

    }

    private IEnumerator GlitchbyTime(float time) {
        ToggleGlitch();
        yield return new WaitForSeconds(time);
        ToggleGlitch();
    }

    private IEnumerator StartGlitchAfter(float ti = 1.0f) {
        yield return new WaitForSeconds(ti);
        Glitcher();
    }


}
