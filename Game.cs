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
    //public Text lines = FindObjectOfType<Text>();

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    public int scoreOneLine = 40;
    public int scoreTwoLines = 100;
    public int scoreThreeLines = 300;
    public int scoreFourLines = 500;

    private int numberOfRowsThisTurn = 0;

    public Text hud_score;
    //public Text lines = FindObjectOfType<Text>().;
    private int currentScore = 0 ;

    void Start()
    {
        SpawnNextTetrimino();
    }

    private void Update()
    {
        UpdateScore();
        UpdateUI();
    }

    public void UpdateScore()
    {
        if (numberOfRowsThisTurn != 0)
        {
            switch (numberOfRowsThisTurn)
            {
                case 1:
                    currentScore += scoreOneLine; break;
                case 2:
                    currentScore += scoreTwoLines; break;
                case 3:
                    currentScore += scoreThreeLines; break;
                case 4:
                    currentScore += scoreFourLines; break;         
            }
        }
        numberOfRowsThisTurn = 0;
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

                //lines.text = $"Lines\n{rowsDeleted}";
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
        GameObject nextTetrimino = (GameObject)Instantiate(Resources.Load(GetRandomTetrimino()), new Vector2(4.0f, 20.0f), Quaternion.identity);
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