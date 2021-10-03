using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndPoint : MonoBehaviour
{
    [SerializeField]
    private bool isFinalLevel = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HorseHoof") || collision.gameObject.CompareTag("HorseBody"))
        {
            if (isFinalLevel)
            {
                GameManager.instance.Win();
            }
            else
            {
                GameManager.instance.BeatLevel();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("HorseHoof") || other.gameObject.CompareTag("HorseBody"))
        {
            if (isFinalLevel)
            {
                GameManager.instance.Win();
            }
            else
            {
                GameManager.instance.BeatLevel();
            }
        }
    }
}