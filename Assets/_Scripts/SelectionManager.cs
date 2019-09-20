using Libplanet.Action;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager
{
    public SelectionManager()
    {

    }

    public static Selection GetSelection()
    {
        int value = Random.Range(0, 2);
        if (value == 0)
        {
            int r = Random.Range(1, 3);
            var dir = (Direction)Random.Range(0, 4);
            return new Movement(r, dir);
        }
        else
        {
            int r1 = 0;
            while(r1 % 2 == 0)
            {
                r1 = Random.Range(1, 5);
            }
            int r2 = Random.Range(1, 5);
            Range range = (Range)Random.Range(0, 2);
            return new Attack(r1, r2, range);
        }
    }
}


public enum Direction
{
    윗 = 0,
    아랫,
    왼쪽,
    오른쪽 = 3,
    UpRight,
    UpLeft,
    DownRight,
    DownLeft
}

public enum Range
{
    직사각형 = 0,
    직선,
    대각선
}

public class Selection
{
    public int value;
}

public class Movement : Selection
{
    public Direction dir;

    public Movement(int r, Direction dir)
    {
        value = r;
        this.dir = dir;
    }

    public override string ToString()
    {
        string str = $"{value}칸 {dir} 방향으로 이동";
        return str;
    }
}

public class Attack : Selection
{
    public int value2;
    public int damage;
    public Range range;

    public Attack(int r1, int r2, Range range)
    {
        this.value = r1;
        this.value2 = r2;
        this.range = range;
        damage = 50 / (value * value2);
    }

    public override string ToString()
    {
        string str = $"{value}x{value2} 범위에 {damage} 데미지 입히기 (범위에 비례)";
        return str;
    }
}