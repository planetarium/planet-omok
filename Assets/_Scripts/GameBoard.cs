using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleGrid;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    public const int size = 10;

    public Grid<Image> grid;
    public static Grid<List<PlayerNode>> nodes;
    public GameObject cellPrefab;
    public Transform cellParent;
    public Color cellColor1;
    public Color cellColor2;
    public Vector2 pivot = new Vector2(-320f, 32f);

    // Start is called before the first frame update
    void Awake()
    {
        grid = new Grid<Image>(10);
        nodes = new Grid<List<PlayerNode>>(10);
        foreach (var cell in grid)
        {
            var go = Instantiate(cellPrefab, cellParent);
            var image = go.GetComponent<Image>();
            cell.SetData(image);
        }
        foreach (var cell in nodes)
        {
            cell.SetData(new List<PlayerNode>(4));
        }

        for (int x = 0; x < grid.Columns.Count; ++x)
        {
            for(int y = 0; y < grid.Rows.Count; ++y)
            {
                var img = grid[x, y].Data;
                img.rectTransform.anchoredPosition = pivot + new Vector2(x + 1, y + 1) * 64f;
                // 격 자 모 양
                img.color = ((x % 2 == 1) ^ (y % 2 == 1)) ? cellColor1 : cellColor2;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
