using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 20;
    public static int rowsDeleted = 0;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    public int scoreOneLine = 40;
    public int scoreTwoLines = 100;
    public int scoreThreeLines = 300;
    public int scoreFourLines = 500;

    public int currentLevel = 0;
    private int linesCleared = 0;

    public float fallSpeed = 1.0f;

    private int numberOfRowsThisTurn = 0;

    public Text hud_score;
    public static int currentScore = 0 ;

    private GameObject previewTetrimino;
    private GameObject nextTetrimino;

    private bool gameStarted = false;

    private Vector2 previewTetriminoPosition = new Vector2(-6.5f, 15);

    void Start()
    {
        SpawnNextTetrimino();
    }

    private void Update()
    {
        UpdateScore();
        UpdateUI();

        UpdateLevel();
        UpdateSpeed();
    }

    void UpdateLevel()
    {
        currentLevel = linesCleared / 10;
        Debug.Log("Current level: " + currentLevel);
    }
    
    void UpdateSpeed()
    {
        fallSpeed = 1.0f - currentLevel * 0.1f;
        Debug.Log("Current fall speed: " + fallSpeed);
    }

    public void UpdateScore()
    {
        if (numberOfRowsThisTurn > 0)
        {
            if (numberOfRowsThisTurn == 1)
            {
                OneLine();
                //currentScore += scoreOneLine;
                //linesCleared++;
            }
            else if (numberOfRowsThisTurn == 2)
            {
                currentScore += scoreTwoLines;
                linesCleared += 2;
            }
            else if (numberOfRowsThisTurn == 3)
            {
                currentScore += scoreThreeLines;
                linesCleared += 3;
            }
            else if (numberOfRowsThisTurn == 4)
            {
                currentScore += scoreFourLines;
                linesCleared += 4;
            }
            numberOfRowsThisTurn = 0;
            //switch (numberOfRowsThisTurn)
            //{
            //    case 1: currentScore += scoreOneLine;
            //            linesCleared += 1;
            //            break;

            //    case 2: currentScore += scoreTwoLines;
            //            linesCleared += 2;
            //            break;

            //    case 3: currentScore += scoreThreeLines;
            //            linesCleared += 3;
            //            break;

            //    case 4: currentScore += scoreFourLines;
            //            linesCleared += 4;
            //            break;         
            //}

        }

    }

    public void OneLine()
    {
        currentScore += scoreOneLine;
        linesCleared++;
    }

    public void UpdateUI()
    {
        hud_score.text = currentScore.ToString();
    }

    public bool CheckIsAboveGrid(Tetrimino tetrimino)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            foreach (Transform mino in tetrimino.transform)
            {
                Vector2Int position = Vector2Int.RoundToInt(mino.position);

                if (position.y > gridHeight - 1)
                    return true;
            }
        }
        return false;
    }

    public bool IsRowFullAt(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] == null)
                return false;
        }
        numberOfRowsThisTurn++;
        return true;
    }

    public void DeleteMinoAt(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void MoveRowDown(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x,y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += Vector3.down;
            }
        }
    }

    public void MoveAllRowsDown(int y)
    {
        for (int i = y; i < gridHeight; ++i)
        {
            MoveRowDown(i);
        }
    }

    public void DeleteRow()
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            if (IsRowFullAt(y))
            {
                DeleteMinoAt(y);
                MoveAllRowsDown(y + 1);
                --y;
                rowsDeleted++;                              
            }
        }
    }

    public void UpdateGrid(Tetrimino tetrimino)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (grid[x, y] != null)
                    if (grid[x, y].parent == tetrimino.transform)
                            grid[x, y] = null;
            }
        }

        foreach (Transform mino in tetrimino.transform)
        {
            Vector2Int position = Vector2Int.RoundToInt(mino.position);

            if (position.y < gridHeight)
                grid[position.x, position.y] = mino;
        }
    }

    public Transform GetTransformAtGridPosition(Vector2Int position)
    {
        return position.y > (gridHeight - 1) ? null : grid[(int)position.x, (int)position.y];
    }

    public void SpawnNextTetrimino()
    {
        if (!gameStarted)
        {
            gameStarted = true;

            nextTetrimino = (GameObject)Instantiate(Resources.Load(GetRandomTetrimino()), new Vector2(4.0f, 20.0f), Quaternion.identity);
            previewTetrimino = (GameObject)Instantiate(Resources.Load(GetRandomTetrimino()), previewTetriminoPosition, Quaternion.identity);
            previewTetrimino.GetComponent<Tetrimino>().enabled = false;
        }
        else
        {
            previewTetrimino.transform.localPosition = new Vector2(4.0f, 20.0f);
            nextTetrimino = previewTetrimino;
            nextTetrimino.GetComponent<Tetrimino>().enabled = true;

            previewTetrimino = (GameObject)Instantiate(Resources.Load(GetRandomTetrimino()), previewTetriminoPosition, Quaternion.identity);
            previewTetrimino.GetComponent<Tetrimino>().enabled = false;
        }

    }

    private string GetRandomTetrimino()
    {
        int randomTetrimino = Random.Range(1, 8);

        string randomTetriminoName = "";

        switch (randomTetrimino)
        {
            case 1: randomTetriminoName = "Prefabs/Tetrimino_J"; break;
            case 2: randomTetriminoName = "Prefabs/Tetrimino_L"; break;
            case 3: randomTetriminoName = "Prefabs/Tetrimino_Long"; break;
            case 4: randomTetriminoName = "Prefabs/Tetrimino_S"; break;
            case 5: randomTetriminoName = "Prefabs/Tetrimino_Square"; break;
            case 6: randomTetriminoName = "Prefabs/Tetrimino_T"; break;
            case 7: randomTetriminoName = "Prefabs/Tetrimino_Z"; break;
        }
        return randomTetriminoName;
    }

    public bool CheckIsInsideGrid(Vector2Int position)
    {
        return (position.x >= 0 && position.x < gridWidth && position.y >= 0);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

}