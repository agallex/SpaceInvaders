using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public int speedSheep = 10;
    private readonly float _maxX = 3.5f;
    void Start() 
    {
        transform.position = new Vector3(0, -4, 0);
    }

    // Update is called once per frame
    void Update()
    {
        {
            float horizontal = Input.GetAxis("Horizontal") * speedSheep * Time.deltaTime;
            float posX = transform.position.x;
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
