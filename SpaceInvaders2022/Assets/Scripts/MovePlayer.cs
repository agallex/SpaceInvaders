using System.Collections;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public int speedSheep = 6;
    private readonly float _maxX = 2f;
    public bool left = false;
    public bool right = false;
    public Coroutine speedMove;
    
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
            
            if (SpeedBonus.GetSpeedBonus)
            {
                SpeedBonus.GetSpeedBonus = false;
                if (speedMove != null)
                {
                    StopCoroutine(speedMove);
                }
                speedMove = StartCoroutine(UpSpeedMove());
            }
            
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
    IEnumerator UpSpeedMove()
    {
        speedSheep = 12;
        yield return new WaitForSeconds(5f);
        speedSheep = 6;
    }
    
}
