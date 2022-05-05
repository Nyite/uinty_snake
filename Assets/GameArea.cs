using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea
{
    public Bounds Bounds { get; }
    public int Width { get; }
    public int Height { get; }
    public float Center { get; }
    public BitArray[] Grid { get; set; }
    public GameArea(BoxCollider2D gameBox)
    {
        Bounds = gameBox.bounds;
        Width = Mathf.RoundToInt(Bounds.max.x - Bounds.min.x) + 2;
        Height = Mathf.RoundToInt(Bounds.max.y - Bounds.min.y) + 2;
        Center = Mathf.Round(Width / 2.0f);

        Grid = new BitArray[Width];
        for (int i = 0; i < Width; i++)
            Grid[i] = new BitArray(Height, false);
    }
}