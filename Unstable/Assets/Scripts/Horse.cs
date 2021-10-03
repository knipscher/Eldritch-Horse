using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Horse : MonoBehaviour
{
    [SerializeField]
    private List<BodyPart> extraBodyParts = new List<BodyPart>();

    [SerializeField]
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<Color> spriteRendererOriginalColors = new List<Color>();

    public int maxAttachedAnimals { get; private set; } = 7;

    // Making everything public because the theme is Unstable
    public float speed = 20;
    public float jumpSpeed = 80;

    private float originalJumpSpeed;

    private Rigidbody2D rb2d;

    private bool hasTouchedGroundSinceLastJump = true;
    public int flightPower = 0;

    public float mutation { get; private set; }
    public float maxMutation { get; private set; } = 10000;
    [SerializeField]
    private float mutationDecreasePerFrame = 1.5f;

    [SerializeField]
    private float mutationChance = 0.05f;

    [SerializeField]
    private float maxVelocity = 10f;

    private float originalDrag;

    [SerializeField]
    private HingeJoint2D hingeJoint2D;
    [SerializeField]
    private HingeJoint2D[] allHingeJoints;

    [SerializeField]
    private Slider mutationSlider;

    public int animalCount { get; private set; } = 0;

    [SerializeField]
    private GameObject[] wings;
    private float wingCooldown = 0;
    private bool isInWingCooldown = false;

    public int traps = 0;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        originalDrag = rb2d.drag;

        foreach (BodyPart bodyPart in extraBodyParts)
        {
            bodyPart.gameObject.SetActive(false);
        }

        Ground.instance.onHorseTouchGround += OnTouchGround;

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRendererOriginalColors.Add(spriteRenderer.color);
        }

        var difficultyAdjustedMaxMutation = PlayerPrefs.GetInt("DifficultyAdjustedMaxMutation");
        if (difficultyAdjustedMaxMutation > 0)
        {
            maxMutation = difficultyAdjustedMaxMutation;
        }
        mutationSlider.maxValue = maxMutation;
        mutationSlider.SetValueWithoutNotify(0);

        foreach (GameObject wing in wings)
        {
            wing.SetActive(false);
        }

        originalJumpSpeed = jumpSpeed;
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isGameOver)
        {
            Run();
            Jump();
            DecreaseMutation();
            Brake();
        }
    }

    public void SetJumpSpeed(float value)
    {
        value = Mathf.Clamp(value, originalJumpSpeed, value);
    }

    private void OnTouchGround()
    {
        hasTouchedGroundSinceLastJump = true;
    }

    private void Run()
    {
        var runInput = (Input.GetAxis("Horizontal"));
        if (runInput != 0)
        {
            var force = new Vector2(runInput * speed, 0);
            if (traps > 0)
            {
                force /= (traps + 1);
            }
            rb2d.AddForce(force);
            rb2d.MovePosition((Vector2)rb2d.transform.position + force * Time.deltaTime);
        }
    }

    private void Jump()
    {
        if (traps == 0)
        {
            var jumpInput = Input.GetAxis("Jump") / 2f;
            if (jumpInput > 0 && hasTouchedGroundSinceLastJump)
            {
                hasTouchedGroundSinceLastJump = false;
                var jumpForce = new Vector2(0, jumpInput * jumpSpeed);
                rb2d.AddForce(jumpForce, ForceMode2D.Impulse);
                rb2d.MovePosition((Vector2)rb2d.transform.position + jumpForce * 10 * Time.deltaTime);
            }
            else if (jumpInput > 0 && flightPower > 0)
            {
                var runInput = (Input.GetAxis("Horizontal"));
                var force = new Vector2(runInput * speed, 0);
                if (runInput != 0)
                {
                    if (traps > 0)
                    {
                        force /= (traps + 1);
                    }
                }
                var jumpForce = new Vector2(0, jumpInput * jumpSpeed);
                rb2d.AddForce((jumpForce + force) * 2);
                rb2d.MovePosition((Vector2)rb2d.transform.position + (jumpForce + force) * Time.deltaTime);
            }
        }
    }

    private void Brake()
    {
        if (Vector3.Magnitude(rb2d.velocity) > maxVelocity)
        {
            rb2d.drag += 0.5f;
        }
        else
        {
            rb2d.drag = Mathf.Lerp(rb2d.drag, originalDrag, 0.5f);
        }
    }

    public void AddAnimal(int mutationAmount, bool canFly, SpriteRenderer hitSprite, Color animalColor)
    {
        if (!GameManager.instance.isGameOver)
        {
            if (!hitSprite)
            {
                foreach (SpriteRenderer sprite in spriteRenderers)
                {
                    var random = Random.value;
                    {
                        if (random < mutationChance)
                        {
                            sprite.color = Color.Lerp(sprite.color, animalColor, 0.8f);
                        }
                    }
                }
            }
            else
            {
                hitSprite.color = Color.Lerp(hitSprite.color, animalColor, 0.8f);
            }

            animalCount++;
            if (animalCount > 2 && !GameManager.instance.isGameOver)
            {
                hingeJoint2D.useLimits = false;
            }
            AddMutation(mutationAmount, canFly);
        }
    }

    private void AddMutation(int mutationAmount, bool canFly)
    {
        mutation += mutationAmount;
        GameManager.instance.AddToScore(mutationAmount);
        mutationSlider.SetValueWithoutNotify(mutation);
        RandomMutation();
        if (canFly)
        {
            AddWing();
        }
        if (mutation > maxMutation)
        {
            GameManager.instance.Lose();
            Horseplode();
        }
    }

    private void AddWing()
    {
        if (!isInWingCooldown)
        {
            StartCoroutine(WingCooldown());
            flightPower += 1;
            SetJumpSpeed(jumpSpeed * 2);
            foreach (GameObject wing in wings)
            {
                if (!wing.activeSelf)
                {
                    wing.SetActive(true);
                    return;
                }
            }
        }
    }

    private IEnumerator WingCooldown()
    {
        isInWingCooldown = true;
        yield return new WaitForSeconds(wingCooldown);
        isInWingCooldown = false;
    }

    public void RemoveAnimal()
    {
        if (!GameManager.instance.isGameOver)
        {
            animalCount--;
            if (animalCount <= 2)
            {
                hingeJoint2D.useLimits = true;
            }
        }
    }

    private void RandomMutation()
    {
        foreach (BodyPart bodyPart in extraBodyParts)
        {
            var random = Random.value;
            if (random < mutationChance)
            {
                bodyPart.gameObject.SetActive(true);
            }
        }
    }

    private void DecreaseMutation()
    {
        mutation = Mathf.Clamp(mutation - (maxMutation / mutationDecreasePerFrame), 0, maxMutation);
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            spriteRenderers[i].color = Color.Lerp(spriteRenderers[i].color, spriteRendererOriginalColors[i], Time.deltaTime / 10);
        }
        foreach (BodyPart bodyPart in extraBodyParts)
        {
            if (mutation < 10)
            {
                bodyPart.gameObject.SetActive(false);
            }

            var random = Random.value;
            if (random < mutationChance * Time.deltaTime)
            {
                bodyPart.gameObject.SetActive(false);
            }
        }
        mutationSlider.SetValueWithoutNotify(mutation);
    }

    public void RemoveWing()
    {
        if (flightPower > 0)
        {
            flightPower -= 1;
            SetJumpSpeed(jumpSpeed / 2);
            foreach (GameObject wing in wings)
            {
                if (wing.activeSelf)
                {
                    wing.SetActive(false);
                    return;
                }
            }
        }
    }

    public void Horseplode()
    {
        foreach (HingeJoint2D hingeJoint in allHingeJoints)
        {
            if (hingeJoint)
            {
                hingeJoint.useMotor = true;
                hingeJoint.breakForce = 0.1f;
            }
        }
    }
}