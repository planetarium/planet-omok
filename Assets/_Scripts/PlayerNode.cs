using UnityEngine;
using UnityEngine.UI;

public class PlayerNode : MonoBehaviour
{
    public int hp = 100;
    public int playerIndex = 0;
    public Sprite[] sprite;
    public (int x, int y) location;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        GameStart();
    }

    // Update is called once per frame
    void GameStart()
    {
        Init();
    }

    public void Init()
    {
        int max = GameBoard.size - 1;
        SetLocation(max * (playerIndex % 2), max * (playerIndex / 2));
        image.sprite = sprite[playerIndex];
        image.rectTransform.sizeDelta = new Vector2(60, 70);
    }

    public void SetLocation(int x, int y)
    {
        if (x < 0) x = 0;
        if (y < 0) y = 0;
        if (x >= GameBoard.nodes.Columns.Count) x = GameBoard.nodes.Columns.Count - 1;
        if (y >= GameBoard.nodes.Rows.Count) y = GameBoard.nodes.Rows.Count - 1;

        image.rectTransform.anchoredPosition = GameBoard.GetWorldPos(x, y);
        GameBoard.nodes[location.x, location.y].Data.Remove(this);
        GameBoard.nodes[x, y].Data.Add(this);
        location = (x, y);
    }
}
