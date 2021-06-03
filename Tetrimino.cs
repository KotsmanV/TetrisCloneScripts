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
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * 5f * Time.time;

            if (CheckIsPositionValid())
                FindObjectOfType<Game>().UpdateGrid(this);
            else
                transform.position += Vector3.left;

        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * 5f * Time.deltaTime;

            if (CheckIsPositionValid())
                FindObjectOfType<Game>().UpdateGrid(this);
            else
                transform.position += Vector3.right;
        }


        if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {

            //if (Game.rowsDeleted % 10 == 0)
            //    fallSpeed-=0.1f;

            transform.position += Vector3.down;



            if (CheckIsPositionValid())
                FindObjectOfType<Game>().UpdateGrid(this);
            else
            {
                transform.position += Vector3.up;

                FindObjectOfType<Game>().DeleteRow();

                if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
                    FindObjectOfType<Game>().GameOver();


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