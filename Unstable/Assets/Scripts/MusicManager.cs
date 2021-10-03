using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] audioSources = null;

    [SerializeField]
    private Horse horse;

    private void Update()
    {
        if (horse.mutation > horse.maxMutation * 0.66f)
        {
            audioSources[0].volume = 0;
            audioSources[1].volume = 0.5f;
            audioSources[2].volume = 0.5f;
        }
        else if (horse.mutation < horse.maxMutation * 0.66f && horse.mutation > horse.maxMutation * 0.2f)
        {
            audioSources[0].volume = horse.mutation / horse.maxMutation;
            audioSources[1].volume = (1 - (horse.mutation / horse.maxMutation)) / 2;

            if (horse.mutation > horse.maxMutation * 0.45f)
            {
                audioSources[2].volume = (1 - (horse.mutation / horse.maxMutation)) / 2;
            }
            else
            {
                audioSources[2].volume = 0;
            }
        }
        else
        {
            audioSources[0].volume = 1;
            audioSources[1].volume = 0;
            audioSources[2].volume = 0;
        }
    }
}