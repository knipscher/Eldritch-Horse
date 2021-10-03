using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyNet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HorseHoof") || collision.gameObject.CompareTag("HorseBody"))
        {
            var horse = collision.gameObject.GetComponent<Horse>();
            if (horse)
            {
                horse.Horseplode();
            }
            GameManager.instance.Lose();
        }
        Destroy(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("HorseHoof") || other.gameObject.CompareTag("HorseBody"))
        {
            var horse = other.gameObject.GetComponent<Horse>();
            if (horse)
            {
                horse.Horseplode();
            }
            GameManager.instance.Lose();
        }
        Destroy(other.gameObject);
    }
}