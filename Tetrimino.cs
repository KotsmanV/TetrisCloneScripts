using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Tetrimino : MonoBehaviour
{
    float fall = 0;
    private float fallSpeed =1;

    public int individualScore = 100;
    private float individualScoreTime;

    private float continuousVerticalSpeed = 0.05f;      //Speed at which the tetrimino will move when DownArrow is held down
    private float continuousHorizontalSpeed = 0.05f;    //Speed at which the tetrimino will move when LeftArrow or DownArrow is held down
    private float buttonDownWaitMax = 0.01f;            //Wait interval before the tetrimino recognizes that a button is being held down


    private float verticalTimer = 0;
    private float horizontalTimer = 0;
    private float buttonDownWaitTimerHorizontal = 0;
    private float buttonDownWaitTimerVertical = 0;

    private bool movedImmediateHorizontal = false;
    private bool movedImmediateVertical = false;

    // Start is called before the first frame update
    void Start()
    {
        fallSpeed = GameObject.Find("GameScript").GetComponent<Game>().fallSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CheckUserInput();
        UpdateIndividualScore();
    }

    private void UpdateIndividualScore()
    {
        if (individualScoreTime < 1)
            individualScoreTime += Time.deltaTime;
        else
        {
            individualScoreTime = 0;
            individualScore = Mathf.Max(individualScore - 10, 0);
        }
    }

    void CheckUserInput()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            movedImmediateHorizontal = false;
            horizontalTimer = 0;
            buttonDownWaitTimerHorizontal = 0;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            movedImmediateVertical = false;
            verticalTimer = 0;
            buttonDownWaitTimerVertical = 0;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKey(KeyCode.DownArrow) || Time.time - fall >= fallSpeed)
        {
            MoveDown();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
        }
    }

    /// <summary>
    /// Left Movement
    /// </summary>
    void MoveLeft()
    {
        if (movedImmediateHorizontal)
        {
            if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
            {
                buttonDownWaitTimerHorizontal += Time.deltaTime;
                return;
            }

            if (horizontalTimer < continuousHorizontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }

        if (!movedImmediateHorizontal)
            movedImmediateHorizontal = true;

        horizontalTimer = 0;

        transform.position += new Vector3(-1, 0, 0);

        if (CheckIsPositionValid())
            FindObjectOfType<Game>().UpdateGrid(this);
        else
            transform.position += new Vector3(1, 0, 0);
    }

    /// <summary>
    /// Right Movement
    /// </summary>
    void MoveRight()
    {
        if (movedImmediateHorizontal)
        {
            if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
            {
                buttonDownWaitTimerHorizontal += Time.deltaTime;
                return;
            }

            if (horizontalTimer < continuousHorizontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }
        if (!movedImmediateHorizontal)
            movedImmediateHorizontal = true;

        horizontalTimer = 0;

        transform.position += new Vector3(1, 0, 0);

        if (CheckIsPositionValid())
            FindObjectOfType<Game>().UpdateGrid(this);
        else
            transform.position += new Vector3(-1, 0, 0);
    }
    /// <summary>
    /// Down Movement
    /// </summary>
    void MoveDown()
    {
        if (movedImmediateVertical)
        {
            if (buttonDownWaitTimerVertical < buttonDownWaitMax)
            {
                buttonDownWaitTimerVertical += Time.deltaTime;
                return;
            }

            if (verticalTimer < continuousVerticalSpeed)
            {
                verticalTimer += Time.deltaTime;
                return;
            }
        }
        if (!movedImmediateVertical)
            movedImmediateVertical = true;

        verticalTimer = 0;

        transform.position += new Vector3(0, -1, 0);

        if (CheckIsPositionValid())
            FindObjectOfType<Game>().UpdateGrid(this);
        else
        {
            transform.position += new Vector3(0, 1, 0);

            FindObjectOfType<Game>().DeleteRow();

            if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
                FindObjectOfType<Game>().GameOver();

            enabled = false;
            FindObjectOfType<Game>().SpawnNextTetrimino();
            Game.currentScore += individualScore;
        }
        fall = math.round(Time.time);
    }

    /// <summary>
    /// Tetrimino Rotation
    /// </summary>
    void Rotate()
    {
        transform.Rotate(0, 0, 90);

        if (CheckIsPositionValid())
            FindObjectOfType<Game>().UpdateGrid(this);
        else
            transform.Rotate(0, 0, -90);
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