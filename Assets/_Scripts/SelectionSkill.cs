using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSkill : MonoBehaviour
{
    PlayerNode node;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public void Awake()
    {
        node = GetComponent<PlayerNode>();
    }

    public void MoveNode(int xOffset, int yOffset)
    {
        node.SetLocation(node.location.x + xOffset, node.location.y + yOffset);
    }

    public void DamageToLine(int start, int end, int axis, int dmg, bool isVertical = true, bool damageSelf = false)
    {
        if (end >= GameBoard.nodes.Columns.Count) end = GameBoard.nodes.Columns.Count - 1;
        if (start < 0) start = 0;

        for (int i = start; i <= end; ++i)
        {
            if (isVertical)
                foreach (var node in GameBoard.nodes.Rows[axis].columns[i].Data)
                {
                    if (node != this || damageSelf)
                        node.hp -= dmg;
                }
            else
                foreach (var node in GameBoard.nodes.Columns[axis].rows[i].Data)
                {
                    if (node != this)
                        node.hp -= dmg;
                }
        }
    }

    public void DamageRect((int x, int y) start, (int x, int y) end, int dmg, bool damageSelf = false)
    {
        var list = GameBoard.nodes.GetRectangle(start.x, start.y, end.x, end.y);

        foreach (var nodes in list)
        {
            foreach (var n in nodes.Data)
            {
                if (n != this || damageSelf)
                    n.hp -= dmg;
            }
        }
    }
}
