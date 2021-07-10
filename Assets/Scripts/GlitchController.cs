using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{
    public static GlitchController Instance;
    public Material mat;

    void Awake()
    {
        Instance = this;
        mat.SetInt("Boolean_fbe84cbd1dfe42ce9ac891cdb88f2915", 1);
    }

    private bool isGlitching = true;    

    public void ToggleGlitch()
    {
        mat.SetInt("Boolean_fbe84cbd1dfe42ce9ac891cdb88f2915", isGlitching ? 0 : 1);
        isGlitching = !isGlitching;
    }

}
