using UnityEngine;

public static class SelectionSkill
{
    public static void MoveStraight(this PlayerNode node, int length, Direction dir)
    {
        switch (dir)
        {
            case Direction.윗:
                MoveNode(node, 0, length);
                break;
            case Direction.아랫:
                MoveNode(node, 0, -length);
                break;
            case Direction.왼쪽:
                MoveNode(node, -length, 0);
                break;
            case Direction.오른쪽:
                MoveNode(node, length, 0);
                break;
            default:
                break;
        }
    }

    public static void MoveNode(this PlayerNode node, int xOffset, int yOffset)
    {
        node.SetLocation(node.location.x + xOffset, node.location.y + yOffset);
    }

    public static void DamageToLine(this PlayerNode node, int start, int end, int axis, int dmg, bool isVertical = true, bool damageSelf = false)
    {
        if (end >= GameBoard.nodes.Columns.Count) end = GameBoard.nodes.Columns.Count - 1;
        if (start < 0) start = 0;

        for (int i = start; i <= end; ++i)
        {
            if (isVertical)
                foreach (var n in GameBoard.nodes.Rows[axis].columns[i].Data)
                {
                    if (n != node || damageSelf)
                        n.hp -= dmg;
                }
            else
                foreach (var n in GameBoard.nodes.Columns[axis].rows[i].Data)
                {
                    if (n != node)
                        n.hp -= dmg;
                }
        }
    }

    public static void DamageRect(this PlayerNode node, (int x, int y) start, (int x, int y) end, int dmg, bool damageSelf = false)
    {
        var list = GameBoard.nodes.GetRectangle(start.x, start.y, end.x, end.y);

        foreach (var nodes in list)
        {
            foreach (var n in nodes.Data)
            {
                if (n != node || damageSelf)
                    n.hp -= dmg;
            }
        }
    }
}
