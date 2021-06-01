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

            if (CheckIsPositionValid())
                FindObjectOfType<Game>().UpdateGrid(this);
            else
                transform.position += Vector3.left;

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;

            if (CheckIsPositionValid())
                FindObjectOfType<Game>().UpdateGrid(this);
            else
                transform.position += Vector3.right;
        }


        if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            transform.position += Vector3.down;

            if (CheckIsPositionValid())
                FindObjectOfType<Game>().UpdateGrid(this);
            else
            {
                transform.position += Vector3.up;

                FindObjectOfType<Game>().DeleteRow();

                enabled = false;

                FindObjectOfType<Game>().SpawnNextTetrimino();
            }


            fall = math.round(Time.time);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, 90);

            if (CheckIsPositionValid())
                FindObjectOfType<Game>().UpdateGrid(this);
            else
                transform.Rotate(0, 0, -90);
        }
    }

    bool CheckIsPositionValid()
    {
        foreach (Transform mino in transform)
        {
            Vector2Int position = Vector2Int.RoundToInt(mino.position);

            if (!FindObjectOfType<Game>().CheckIsInsideGrid(position))
                return false;

            if (FindObjectOfType<Game>().GetTransformAtGridPosition(position) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(position).parent != transform)
                return false;
        }
        return true;
    }

}