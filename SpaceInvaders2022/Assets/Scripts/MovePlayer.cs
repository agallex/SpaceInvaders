using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public int speedSheep = 10;
    private readonly float _maxX = 2f;
    private bool left = false;
    private bool right = false;
    void Start() 
    {
        transform.position = new Vector3(0, -3, 0);
    }

    public void LeftButtonUp()
    {
        left = false;
    }

    public void LeftButtonDown()
    {
        left = true;
    }
    public void RightButtonUp()
    {
        right = false;
    }
    
    public void RightButtonDown()
    {
        right = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        {
            float horizontal = Input.GetAxis("Horizontal") * speedSheep * Time.deltaTime;
            float posX = transform.position.x;
            
            if (right)
            {
                float dirX = speedSheep * Time.deltaTime * 0.5f;
                if (posX + dirX < _maxX)
                {
                    transform.Translate(dirX, 0, 0);
                }
                else
                {
                    transform.Translate(_maxX - posX, 0, 0);
                }
            }
            if (left)
            {
                float dirX = -speedSheep * Time.deltaTime * 0.5f;
                if (posX + dirX > -_maxX)
                {
                    transform.Translate(dirX, 0, 0);
                }
                else
                {
                    transform.Translate(-_maxX - posX, 0, 0);
                }
            }
            if (Input.GetButton("Horizontal"))
            {
                if (Mathf.Abs(posX + horizontal) < _maxX)
                {
                    transform.Translate(horizontal, 0, 0);
                }
                else
                {
                    if (posX >= 0)
                    {
                        transform.Translate(_maxX - posX, 0, 0);
                    }
                    else
                    {
                        transform.Translate(-_maxX - posX, 0, 0);
                    }
                }
            }
        }
    }
}
