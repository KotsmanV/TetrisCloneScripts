using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Tetrimino : MonoBehaviour
{
    float fall = 0;
    public float fallSpeed = 1;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckUserInput();
    }

    void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;

            if (!CheckIsPositionValid())
                transform.position += Vector3.left;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;

            if (!CheckIsPositionValid())
                transform.position += Vector3.right;
        }


        if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            transform.position += Vector3.down;

            if (!CheckIsPositionValid())
                transform.position += Vector3.up;

            fall = math.round(Time.time);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, 90);

            if (!CheckIsPositionValid())
                transform.Rotate(0, 0, -90);
        }


    }

    bool CheckIsPositionValid()
    {
        foreach (Transform mino in transform)
        {
            Vector2 position = Vector2Int.RoundToInt(mino.position);

            if (!FindObjectOfType<Game>().CheckIsInsideGrid(position))
                return false;
        }
        return true;
    }

}
