using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public static Ground instance;

    [SerializeField]
    private string horseHoofTag = "HorseHoof";
    [SerializeField]
    private string animalTag = "Animal";

    public delegate void OnHorseTouchGround();
    public OnHorseTouchGround onHorseTouchGround;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(horseHoofTag) || collision.gameObject.CompareTag("HorseBody"))
        {
            onHorseTouchGround();
        }
        else if (collision.gameObject.CompareTag(animalTag))
        {
            var animal = collision.gameObject.GetComponent<Animal>();
            animal.HitGround();
        }
    }
}