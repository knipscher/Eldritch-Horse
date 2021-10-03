using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField]
    private Color mainColor;

    [SerializeField]
    private bool canFly = false;

    [SerializeField]
    private List<BodyPart> bodyParts = new List<BodyPart>();
    [SerializeField]
    private Transform[] allTransformsInAnimal = null;
    [SerializeField]
    private Collider2D[] allCollidersInAnimal = null;
    [SerializeField]
    private Rigidbody2D[] allRigidbodies = null;
    [SerializeField]
    private SpriteRenderer[] spriteRenderersToTurnOffWhenAttaching;

    [SerializeField]
    private string[] horseTags = null;

    [SerializeField]
    private int mutationAmount = 1000;
    [SerializeField]
    private float speed = 20;
    [SerializeField]
    private float bounceSpeed = 50;
    [SerializeField]
    private float jumpSpeed = 80;

    private Rigidbody2D rb2d;

    [SerializeField]
    private float dissolveTime = 15f;

    private Horse attachedHorse = null;

    [SerializeField]
    private Vector3 moveForce;

    [SerializeField]
    private Vector3 jumpForce;
    [SerializeField]
    private float jumpTime = 1;

    [SerializeField]
    private float lifetimeInSeconds = 120;

    [SerializeField]
    private bool trapsHorse = false;

    [SerializeField]
    private AudioSource soundEffect;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(Jump());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (string tag in horseTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                var horse = collision.transform.root.GetComponent<Horse>(); // please note, this means we can't parent the horse to anything else
                AttachToHorse(horse, collision.transform, tag);
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        if (attachedHorse == null)
        {
            rb2d.AddForce(moveForce * speed);
        }
    }

    private IEnumerator Jump()
    {
        while (true)
        {
            rb2d.AddForce(jumpForce * jumpSpeed, ForceMode2D.Impulse);
            yield return new WaitForSeconds(Random.Range(jumpTime / 2, jumpTime));
        }
    }

    public virtual void HitGround()
    {
        rb2d.AddForce(new Vector2(0, bounceSpeed), ForceMode2D.Impulse);
        foreach (BodyPart bodyPart in bodyParts)
        {
            bodyPart.Bounce();
        }
    }

    private void AttachToHorse(Horse horse, Transform hitTransform, string tag)
    {
        if (horse.animalCount < horse.maxAttachedAnimals)
        {
            attachedHorse = horse;
            rb2d.isKinematic = true;

            var hitSprite = hitTransform.GetComponent<SpriteRenderer>();

            transform.SetParent(hitTransform);

            foreach (Transform animalTransform in allTransformsInAnimal)
            {
                animalTransform.tag = tag;
            }
            foreach (Collider2D collider2D in allCollidersInAnimal)
            {
                collider2D.enabled = false;
            }
            foreach (Rigidbody2D rigidbody2D in allRigidbodies)
            {
                rb2d.drag = 0;
                rb2d.angularDrag = 0;
            }
            foreach (SpriteRenderer spriteRenderer in spriteRenderersToTurnOffWhenAttaching)
            {
                spriteRenderer.enabled = false;
            }
            attachedHorse.speed += speed;
            // attachedHorse.bounceSpeed += bounceSpeed;
            attachedHorse.SetJumpSpeed(attachedHorse.jumpSpeed += jumpSpeed);

            if (trapsHorse)
            {
                attachedHorse.traps++;
            }
            attachedHorse.AddAnimal(mutationAmount, canFly, hitSprite, mainColor);

            soundEffect.Play();

            StartCoroutine(DissolveAfterDelay());
        }
    }

    private IEnumerator DissolveAfterDelay()
    {
        yield return new WaitForSeconds(dissolveTime);
        if (!GameManager.instance.isGameOver)
        {
            attachedHorse.speed -= speed;
            attachedHorse.jumpSpeed -= jumpSpeed;
            if (canFly)
            {
                attachedHorse.RemoveWing();
            }
            if (trapsHorse)
            {
                attachedHorse.traps--;
            }
            attachedHorse.RemoveAnimal();
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(lifetimeInSeconds);
        Destroy(gameObject);
    }
}