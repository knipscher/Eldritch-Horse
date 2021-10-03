using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;

    [SerializeField]
    private GameObject[] storyParagraphs;
    private int currentStoryID = 0;

    [SerializeField]
    private GameObject[] horseMutations;
    private int horseMutationID = 0;
    [SerializeField]
    private float timeBetweenMutations = 1;
    [SerializeField]
    private Rigidbody2D[] rigidbodies;

    [SerializeField]
    private HorseTracker horseTracker;
    [SerializeField]
    private TransformTracker transformTracker;

    [SerializeField]
    private GameObject texMutation;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private GameObject nextButton;

    [SerializeField]
    private PostProcessVolume postProcessVolume = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (GameObject horseMutation in horseMutations)
        {
            horseMutation.SetActive(false);
        }

        for (int i = 0; i < storyParagraphs.Length; i++)
        {
            if (i == 0)
            {
                storyParagraphs[i].SetActive(true);
            }
            else
            {
                storyParagraphs[i].SetActive(false);
            }
        }

        StartCoroutine(NextButtonCooldown());
    }

    public void MoveToNextParagraph()
    {
        currentStoryID++;
        if (currentStoryID < storyParagraphs.Length)
        {
            StartCoroutine(NextButtonCooldown());
            for (int i = 0; i < storyParagraphs.Length; i++)
            {
                if (i == currentStoryID)
                {
                    storyParagraphs[i].SetActive(true);
                    if (i == 1)
                    {
                        StartCoroutine(MutateHorses());
                        ControlWeirdness(0.25f);
                    }
                    else if (i == 2)
                    {
                        transformTracker.enabled = true;
                        horseTracker.focusOnHorse = true;
                        StartCoroutine(MutateTex());
                    }
                }
                else
                {
                    storyParagraphs[i].SetActive(false);
                }
            }
        }
        else
        {
            SceneChangeManager.instance.LoadNextScene();
        }
    }

    private IEnumerator MutateHorses()
    {
        while (horseMutationID < horseMutations.Length - 1)
        {
            yield return new WaitForSeconds(Random.Range(timeBetweenMutations, timeBetweenMutations * 2));
            audioSource.Play();
            horseMutationID++;
            horseMutations[horseMutationID].SetActive(true);
            rigidbodies[horseMutationID].constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }

    private IEnumerator MutateTex()
    {
        yield return new WaitForSeconds(timeBetweenMutations);
        audioSource.Play();
        texMutation.SetActive(true);
    }

    private void ControlWeirdness(float value)
    {
        if (postProcessVolume != null)
        {
            ChromaticAberration chromaticAberration;
            if (postProcessVolume.profile.TryGetSettings(out chromaticAberration))
            {
                chromaticAberration.intensity.value = value;
            }

            ColorGrading colorGrading;
            if (postProcessVolume.profile.TryGetSettings(out colorGrading))
            {
                colorGrading.hueShift.value = value * 20;
            }
        }
    }

    private IEnumerator NextButtonCooldown()
    {
        nextButton.SetActive(false);
        yield return new WaitForSeconds(2);
        nextButton.SetActive(true);
    }
}