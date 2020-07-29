using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class PostProcessController : MonoBehaviour
{
    public VignetteAndChromaticAberration vignetteAndChromatic;
    // Start is called before the first frame update

    public float vignetteIntensity = 0.3f;
    public float chromaticAbberation = 2f;
    public float blur = 0.4f;

    float startvignetteIntensity, startchromaticAbberation, startblur;
    void Start()
    {
        startvignetteIntensity = vignetteAndChromatic.intensity;
        startchromaticAbberation = vignetteAndChromatic.chromaticAberration;
        startblur = vignetteAndChromatic.blur;
    }

    public void OnTouchStart()
    {
        vignetteAndChromatic.intensity = vignetteIntensity;
        vignetteAndChromatic.chromaticAberration = chromaticAbberation;
        vignetteAndChromatic.blur = blur;
    }

    public void OnTouchEnd()
    {
        vignetteAndChromatic.intensity = startvignetteIntensity;
        vignetteAndChromatic.chromaticAberration = startchromaticAbberation;
        vignetteAndChromatic.blur = startblur;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
