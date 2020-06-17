using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public VirtualJoystick joyStick = null;

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float angleSpeed = 150f;
    

    Rigidbody2D rb;
    float input, divider, coinCount = 0;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GameManager.Instance.ScoreCalulator(coinCount = 0);
        divider = Screen.width / 2;
    }

    void Update()
    {
        if (GameManager.Instance.GameControl == GameControllerOption.SIDE_TOUCH)
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.mousePosition.x > divider)
                {
                    input = 1;
                }
                else
                {
                    input = -1;
                }
            }
            else
            {
                input = 0;
            }
        }

    }
    private void FixedUpdate()
    {
        rb.velocity = transform.up * moveSpeed;

        if (!GameManager.Instance.IsGameOver)
            PlaneMovement(input, joyStick.Direction , GameManager.Instance.GameControl);
    }

    public void PlaneMovement(float? input,Vector2? direction, GameControllerOption controllerOption)
    {

        switch (controllerOption)
        {
            case GameControllerOption.SIDE_TOUCH:
                SideTouchMove((float)input);
                break;
            case GameControllerOption.VIRTUAL_PAD:
                VirtualPadMove((Vector2)direction);
                break;
            default:
                break;
        }
    }

    private void SideTouchMove(float num)
    {
        rb.angularVelocity = (num == 0) ? 0 : (num < 0) ? angleSpeed : -angleSpeed;
    }
    public void VirtualPadMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -rotateAmount * angleSpeed;
        }
        else
        {
            rb.angularVelocity = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            GameManager.Instance.SpawnStar();
            GameManager.Instance.ScoreCalulator(coinCount);
        }
    }
}
