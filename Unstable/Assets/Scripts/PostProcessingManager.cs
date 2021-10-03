using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{
    [SerializeField]
    private PostProcessVolume postProcessVolume = null;

    [SerializeField]
    private Horse horse;

    public void Update()
    {
        var ratio = horse.maxMutation * 0.20f;
        if (horse.mutation > ratio)
        {
            ControlWeirdness(horse.mutation / horse.maxMutation);
        }
    }

    private void ControlWeirdness(float value)
    {
        if (postProcessVolume != null)
        {
            ChromaticAberration chromaticAberration;
            if (postProcessVolume.profile.TryGetSettings(out chromaticAberration))
            {
                chromaticAberration.intensity.value = value * 0.66f;
            }

            ColorGrading colorGrading;
            if (postProcessVolume.profile.TryGetSettings(out colorGrading))
            {
                colorGrading.hueShift.value = value * 80;
                colorGrading.contrast.value = value * 80;
                colorGrading.brightness.value = - value * 40;
                colorGrading.saturation.value = value * 80;
            }
        }
    }
}