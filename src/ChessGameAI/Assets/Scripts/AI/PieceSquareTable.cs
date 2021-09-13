using System.Collections.Generic;
using UnityEngine;

public class PieceSquareTable
{
    public Dictionary<Vector2Int, int> PawnGolden = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> PawnGreen = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> KnightGolden = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> KnightGreen = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> BishopGolden = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> BishopGreen = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> RookGolden = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> RookGreen = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> QueenGolden = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> QueenGreen = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> KingGolden = new Dictionary<Vector2Int, int>();
    public Dictionary<Vector2Int, int> KingGreen = new Dictionary<Vector2Int, int>(); 
    
    public void SetDictionaries(){

        var pawnValues = new List<int>(){
            0,  0,  0,  0,  0,  0,  0,  0,
           50, 50, 50, 50, 50, 50, 50, 50,
           10, 10, 20, 30, 30, 20, 10, 10,
            5,  5, 10, 25, 25, 10,  5,  5,
            0,  0,  0, 20, 20,  0,  0,  0,
            5, -5,-10,  0,  0,-10, -5,  5,
            5, 10, 10,-20,-20, 10, 10,  5,
            0,  0,  0,  0,  0,  0,  0,  0
        };
        SetDictionary(PawnGolden, PawnGreen, pawnValues);
         
        var knightValues = new List<int>(){
           -50,-40,-30,-30,-30,-30,-40,-50,
           -40,-20,  0,  0,  0,  0,-20,-40,
           -30,  0, 10, 15, 15, 10,  0,-30,
           -30,  5, 15, 20, 20, 15,  5,-30,
           -30,  0, 15, 20, 20, 15,  0,-30,
           -30,  5, 10, 15, 15, 10,  5,-30,
           -40,-20,  0,  5,  5,  0,-20,-40,
           -50,-40,-30,-30,-30,-30,-40,-50,
        };
        SetDictionary(KnightGolden, KnightGreen, knightValues);
        
        var bishopValues = new List<int>(){
           -20,-10,-10,-10,-10,-10,-10,-20,
           -10,  0,  0,  0,  0,  0,  0,-10,
           -10,  0,  5, 10, 10,  5,  0,-10,
           -10,  5,  5, 10, 10,  5,  5,-10,
           -10,  0, 10, 10, 10, 10,  0,-10,
           -10, 10, 10, 10, 10, 10, 10,-10,
           -10,  5,  0,  0,  0,  0,  5,-10,
           -20,-10,-10,-10,-10,-10,-10,-20,
        };
        SetDictionary(BishopGolden, BishopGreen, bishopValues);

        var rookValues = new List<int>(){
             0,  0,  0,  0,  0,  0,  0,  0,
             5, 10, 10, 10, 10, 10, 10,  5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
             0,  0,  0,  5,  5,  0,  0,  0
        };
        SetDictionary(RookGolden, RookGreen, rookValues);
        
        var queenValues = new List<int>(){
           -20,-10,-10, -5, -5,-10,-10,-20,
           -10,  0,  0,  0,  0,  0,  0,-10,
           -10,  0,  5,  5,  5,  5,  0,-10,
            -5,  0,  5,  5,  5,  5,  0, -5,
             0,  0,  5,  5,  5,  5,  0, -5,
           -10,  5,  5,  5,  5,  5,  0,-10,
           -10,  0,  5,  0,  0,  0,  0,-10,
           -20,-10,-10, -5, -5,-10,-10,-20
        };
        SetDictionary(QueenGolden, QueenGreen, queenValues);

        var kingValues = new List<int>(){
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -20,-30,-30,-40,-40,-30,-30,-20,
            -10,-20,-20,-20,-20,-20,-20,-10,
             20, 20,  0,  0,  0,  0, 20, 20,
             20, 30, 10,  0,  0, 10, 30, 20
        };

        // var kingValuesFinal = new List<int>(){
        //     -50,-40,-30,-20,-20,-30,-40,-50,
        //     -30,-20,-10,  0,  0,-10,-20,-30,
        //     -30,-10, 20, 30, 30, 20,-10,-30,
        //     -30,-10, 30, 40, 40, 30,-10,-30,
        //     -30,-10, 30, 40, 40, 30,-10,-30,
        //     -30,-10, 20, 30, 30, 20,-10,-30,
        //     -30,-30,  0,  0,  0,  0,-30,-30,
        //     -50,-30,-30,-30,-30,-30,-30,-50
        // };       

        SetDictionary(KingGolden, KingGreen, kingValues);

    }

    private void SetDictionary(Dictionary<Vector2Int, int> goldens, 
                               Dictionary<Vector2Int, int> greens, List<int> values)
    {
       
        int i = 0;       
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                //var posGolden = new Vector2Int(x, y);
                goldens.Add(new Vector2Int(x, y), values[i]);               
                //var posGreen = new Vector2Int(x, y);
                greens.Add(new Vector2Int(7 - x, 7 - y), values[i++]);                
            }
        }
                    
    }
}
