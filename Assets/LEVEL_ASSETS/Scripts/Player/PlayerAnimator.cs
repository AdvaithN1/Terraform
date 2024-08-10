using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerMovement mov;
    private SpriteRenderer spriteRend;
    public Rigidbody2D RB { get; private set; }


    [Header("Movement Tilt")]
    [SerializeField] private float maxTilt;
    [SerializeField] [Range(0, 1)] private float tiltSpeed;

    [Header("Particle FX")]
    [SerializeField] private GameObject jumpFX;
    [SerializeField] private GameObject landFX;
    [SerializeField] private GameObject dashFX;
    [SerializeField] private GameObject trailFX;
    [SerializeField] private GameObject colFX;

    [Header("References")]
    [SerializeField] public Text playerText;
    [SerializeField] private Canvas _canvas;
    private ParticleSystem _jumpParticle;
    private ParticleSystem _landParticle;


    public bool startedJumping {  private get; set; }
    public bool startedDashing {  private get; set; }
    public bool isDashing {  private get; set; }
    public bool justLanded { private get; set; }
    public bool doCommand { private get; set; }
    public float speed { private get; set; }

    public float currentVelY;
    public Vector3 animaLoc;
    private bool _animateJump;

    private void Awake() {
		RB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        mov = GetComponent<PlayerMovement>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();

        _jumpParticle = jumpFX.GetComponent<ParticleSystem>();
        _landParticle = landFX.GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        float avgx = 0;
        float avgy = 0;
        foreach (ContactPoint2D contact in collision.contacts) {
            avgx += contact.point.x;
            avgy += contact.point.y;
        }
        GameObject obj = Instantiate(colFX, new Vector3(avgx / collision.contacts.Length, avgy / collision.contacts.Length, transform.position.z - 2), Quaternion.Euler(0, 0, -90));
        obj.GetComponent<ParticleSystem>().startColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
        obj.GetComponent<ParticleSystem>().maxParticles = Mathf.Min((int) (0.3f * Mathf.Sqrt(speed)) + 1, 10);
        obj.layer = 7;
        Destroy(obj, 1);

        if (justLanded) {
            obj = Instantiate(landFX, transform.position - (Vector3.up * transform.localScale.y / 1.5f), Quaternion.Euler(-90, 0, 0));
            obj.GetComponent<ParticleSystem>().startColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
            Destroy(obj, 1);
            justLanded = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (_animateJump) {
            GameObject obj = Instantiate(jumpFX, animaLoc, Quaternion.Euler(-90, 0, 0));
            obj.GetComponent<ParticleSystem>().startColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
            Destroy(obj, 1);
            _animateJump = false;
            // Debug.Log("I should be animating jump!");
            return;
        }
    }

    private void FixedUpdate() {
        speed = RB.velocity.magnitude;
        if (startedJumping) {
            animaLoc = transform.position - (Vector3.up * transform.localScale.y / 2);
            _animateJump = true;
            startedJumping = false;
        }
    }

    private void LateUpdate()
    {
        #region Tilt
        float tiltProgress;

        int mult = -1;

        if (mov.IsSliding)
        {
            tiltProgress = 0.25f;
        }
        else
        {
            tiltProgress = Mathf.InverseLerp(-mov.Data.runMaxSpeed, mov.Data.runMaxSpeed, mov.RB.velocity.x);
            mult = (mov.IsFacingRight) ? -1 : 1;
        }
            
        float newRot = ((tiltProgress * maxTilt * 2) - maxTilt);
        float rot = Mathf.LerpAngle(spriteRend.transform.localRotation.eulerAngles.z * mult, newRot, tiltSpeed);
        spriteRend.transform.localRotation = Quaternion.Euler(0, 0, rot * mult);
        #endregion

        CheckAnimationState();

        ParticleSystem.MainModule jumpPSettings = _jumpParticle.main;
        // jumpPSettings.startColor = new ParticleSystem.MinMaxGradient(demoManager.SceneData.foregroundColor);
        ParticleSystem.MainModule landPSettings = _landParticle.main;
        // landPSettings.startColor = new ParticleSystem.MinMaxGradient(demoManager.SceneData.foregroundColor);
    }

    private void CheckAnimationState()
    {
        if (doCommand) {
            playerText.color = new Color(0f, 0.6262352f, 1.0f, 1f);
            StopCoroutine(commandFade());
            StartCoroutine(commandFade());
            doCommand = false;
        }
        if (startedDashing) {
            GameObject obj = Instantiate(dashFX, transform.position - (Vector3.up * transform.localScale.y / 2), Quaternion.Euler(0, 0, mov.dashDir));
            playerText.text = "$ force P 99 --impulse";
            playerText.color = new Color(0f, 0.6262352f, 1.0f, 1f);
            StopCoroutine(commandFade());
            StartCoroutine(commandFade());
            Destroy(obj, 1);
            startedDashing = false;
            // Debug.Log("I should be animating dash!");
            return;
        } else if (isDashing) {
            var vel = RB.velocity;
            float speed = vel.magnitude;
            if (Random.Range(-20,500) < speed) {
                GameObject obj = Instantiate(trailFX, transform.position - (Vector3.up * transform.localScale.y / 2), Quaternion.Euler(0, 0,mov.dashDir));
                Destroy(obj, 1);
                // Debug.Log("I should be animating dash!");
            }
            return;
        } 
        // else if (justLanded) {
        //     GameObject obj = Instantiate(landFX, transform.position - (Vector3.up * transform.localScale.y / 1.5f), Quaternion.Euler(-90, 0, 0));
        //     Destroy(obj, 1);
        //     justLanded = false;
        //     // Debug.Log("I should be animating land!");
        //     return;
        // }

    }

    IEnumerator commandFade() {
        yield return new WaitForSeconds(0.2f);
        for (int i = 20; i > 0; i -= 1) {
            playerText.color = new Color(0f, 0.6262352f, 1.0f, Mathf.Min(i/20f, 1.0f));
            yield return new WaitForSeconds(0.007f);
        }
        playerText.color = new Color(0f, 0.6262352f, 1.0f, 0f);
        // playerText.text = "$";
    }
}
