using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CharacterControllerMar : MonoBehaviour
{
    public float acceleration = 3f;
    public float maxSpeed = 10f;
    public float jumpImpulse = 8f;
    Rigidbody rb;
    public float jumpBoost = 8f;
    public GameObject resultText;

    public delegate void blockInteractions(int points, int coins);
    public static event blockInteractions OnBlockInteractions;

    [Header("Debug Stuff")] public bool isGrounded;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalAmount = Input.GetAxis("Horizontal");
        rb.linearVelocity += Vector3.right * (horizontalAmount * Time.deltaTime * acceleration);
        
        float horizontalSpeed = rb.linearVelocity.x;
       horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);
       
       Vector3 newVelocity = rb.linearVelocity;
       newVelocity.x = horizontalSpeed;
       rb.linearVelocity = newVelocity;
       
       //should also clamp vertical velocity
       float verticalSpeed = rb.linearVelocity.y;
       verticalSpeed = Mathf.Clamp(verticalSpeed, -maxSpeed, maxSpeed);
       
       Collider c = GetComponent<Collider>();
       Vector3 startPoint = transform.position;
       float castDistance = c.bounds.extents.y + 0.01f;
       Color color = (isGrounded) ? Color.green : Color.red;
       Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.down, color, 0f, false);
       
       isGrounded = Physics.Raycast(startPoint, Vector3.down, castDistance);
       
       
       

       if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
       {
           if (isGrounded)
           {
               //apply an impulse force upward. ForceMode.VelocityChange ignores mass, which is what we want.
               rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
           }
       }
       
       else if (Input.GetKey(KeyCode.Space) && !isGrounded)
       {
           //applies additional height to the jump
           if (rb.linearVelocity.y > 0)
           {
               rb.AddForce(Vector3.up * jumpBoost, ForceMode.Acceleration);
           }
       }

       if (horizontalAmount == 0)
       {
           Vector3 decayedVelocity = rb.linearVelocity;
           decayedVelocity.x *= 1f - Time.deltaTime * 4f;
           rb.linearVelocity = decayedVelocity;
       }
       else
       {
           float yawRotation = (horizontalAmount > 0f) ? 90f : -90f;
           Quaternion rotation = Quaternion.Euler(0f, yawRotation, 0f);
           transform.rotation = rotation;
       }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
            resultText.gameObject.SetActive(true);
            resultText.GetComponent<TextMeshProUGUI>().text = "Game Over";
        }
        
        if (collision.gameObject.CompareTag("goal"))
        {
            resultText.gameObject.SetActive(true);
            resultText.GetComponent<TextMeshProUGUI>().text = "Congratulations!";
        }
        
        if (collision.gameObject.CompareTag("Brick"))
        {
            Destroy(collision.gameObject);
            OnBlockInteractions?.Invoke(100, 0);
        }
        
        if (collision.gameObject.CompareTag("QBlock"))
        {
            OnBlockInteractions?.Invoke(100, 1);
        }
    }
            
    }

