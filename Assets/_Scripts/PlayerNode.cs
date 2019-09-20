using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleGrid;

public class PlayerNode : MonoBehaviour
{
    public (int x, int y) location;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int playerIndex)
    {
        int max = GameBoard.size - 1;
        SetLocation(max * (playerIndex % 2), max * (playerIndex / 2));
    }

    public void SetLocation(int x, int y)
    {
        GameBoard.nodes[location.x, location.y].Data.Remove(this);
        GameBoard.nodes[x, y].Data.Add(this);
        location = (x, y);
    }
}
