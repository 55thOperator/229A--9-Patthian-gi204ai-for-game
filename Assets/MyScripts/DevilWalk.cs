using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class AutoRunnerController : MonoBehaviour
{
    [Header("Movement")]
    public float startSpeed = 5f;
    public float speedIncreaseRate = 0.1f; // increases over time

    private Rigidbody rb;
    private AudioSource footstepAudio;

    private float currentSpeed;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        footstepAudio = GetComponent<AudioSource>();

        // Lock Z for 2.5D
        rb.constraints = RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotation;

        // Audio setup
        footstepAudio.loop = true;
        footstepAudio.playOnAwake = false;
        footstepAudio.spatialBlend = 0.8f;

        currentSpeed = startSpeed;
    }

    void Update()
    {
        // Increase speed over time
        currentSpeed += speedIncreaseRate * Time.deltaTime;

        // Play footsteps if grounded
        if (isGrounded)
        {
            if (!footstepAudio.isPlaying)
                footstepAudio.Play();
        }
        else
        {
            if (footstepAudio.isPlaying)
                footstepAudio.Stop();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(currentSpeed, rb.velocity.y, 0f);
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}