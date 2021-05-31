using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridheight = 20;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckIsInsideGrid(Vector2 position)
    {
        return (position.x >= 0 && position.x < gridWidth && position.y >= 0);
    }

}
