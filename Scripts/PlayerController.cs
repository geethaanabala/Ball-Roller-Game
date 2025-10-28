using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    public float speed = 0;
    public float jumpForce = 5f;      // NEW: jump force
    private bool isGrounded = true;   // NEW: check if on ground
    private int count;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    void Start()
    {
        count = 0;
        rb = GetComponent<Rigidbody>();

        setCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementVal)
    {
        Vector2 movementVector = movementVal.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        // existing movement
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void Update()
    {
        // NEW: jump input
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce + rb.velocity.normalized * speed, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // NEW: detect ground, ramps, bumps
        if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("Ramp") ||
            collision.gameObject.CompareTag("Bump"))
        {
            isGrounded = true;
        }

        // NEW: detect pitfalls
        if (collision.gameObject.CompareTag("Pitfall"))
        {
            Debug.Log("Fell into a pit!");
            // TODO: reset level or deduct life
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            setCountText();
        }
    }

    void setCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 12)
        {
            if (winTextObject != null) winTextObject.SetActive(true);

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.OnLevelComplete();
            }
            else
            {
                Debug.Log("LevelManager not found - win registered.");
            }
        }
    }
}
