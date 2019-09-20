using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleGrid;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    public Grid<GameObject> grid;
    public GameObject cellPrefab;
    public Transform cellParent;
    public Color cellColor1 = new Color(0.295f, 0.31f, 0.196f);
    public Color cellColor2 = new Color((float)0x5C/0xFF, (float)0xD7 /0xFF, (float)0x3D /0xFF);

    // Start is called before the first frame update
    void Awake()
    {
        grid = new Grid<GameObject>(10);
        int cnt = 0;
        foreach (var cell in grid)
        {
            var go = Instantiate(cellPrefab, cellParent);
            var image = go.GetComponent<Image>();
            image.color = ((cnt & 1) == 1) ? cellColor1 : cellColor2;
            ++cnt;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
