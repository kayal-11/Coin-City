using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class cubecontroller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float scaleSpeed;
    private int score;
    private int health;
    public TextMeshProUGUI scoreTextObject, healthTextObject;
    void Start()
    {
        score = 0;
        health = 3;
        // Debug.Log(transform.position);
        //transform.position += (speed*new Vector3 1, 0, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update successfully");
        if (Keyboard.current.rightArrowKey.isPressed)
            //Keyboard.current.upArrowKey.wasPressedThisFrame
            //Keyboard.current.upArrowKey.wasReleasedThisFrame
            transform.Translate(moveSpeed * Time.deltaTime* Vector3.right);
        if (Keyboard.current.leftArrowKey.isPressed)
            transform.Translate(moveSpeed * Time.deltaTime*Vector3.left);
        if (Keyboard.current.upArrowKey.isPressed)
            transform.Translate(moveSpeed * Time.deltaTime*Vector3.up);
        if (Keyboard.current.downArrowKey.isPressed)
            transform.Translate(moveSpeed * Time.deltaTime*Vector3.down);
        if (Keyboard.current.spaceKey.isPressed)
            transform.Translate(moveSpeed * Time.deltaTime*Vector3.forward);
        if (Keyboard.current.rightShiftKey.isPressed)
            transform.Translate(moveSpeed * Time.deltaTime*Vector3.back);
        //transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);
        //transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.left);
        //transform.localScale += scaleSpeed * Time.deltaTime* new Vector3(1, 0, 0);
    }
    private void OnCollisionEnter(Collision collision) //this is for enemies 
    {
        /* if(collision.gameObject.tag=="Coins")
         {
             score++;
             Debug.Log(score);
             Destroy(collision.gameObject);
             //collision.gameObject.SetActive(false);
         }*/
        if (collision.gameObject.tag == "healths")
        {
            health--;
            Debug.Log(health);
            if (health <= 0) 
            {
                Debug.Log("Game Over");
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) //this is for collectors
    {

        if (other.gameObject.tag == "Coins")
        {
            score++;
            Debug.Log(score);
            Destroy(other.gameObject);
            //collision.gameObject.SetActive(false);
        }
    }
}
