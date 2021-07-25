using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board _instance;
    public Transform goldenHolder;
    public Transform greenHolder;    
    public List<Piece> goldenPieces = new List<Piece>();
    public List<Piece> greenPieces = new List<Piece>();
    public Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    void Awake(){
        _instance = this;
        //CreateBoard();
    }

    public async Task LoadAsync(){
        GetTeams();
        await Task.Run(() => CreateBoard());
    }

    private void GetTeams()
    {
        goldenPieces.AddRange(goldenHolder.GetComponentsInChildren<Piece>());
        greenPieces.AddRange(greenHolder.GetComponentsInChildren<Piece>());
    }

    void CreateBoard(){
        
        for (var i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                CreateTile(i, j);
            }
        }

    }

    void CreateTile(int line, int col){

        var tile = new Tile{
            pos = new Vector2Int(line, col)
        };
        tiles.Add(tile.pos, tile);

    }

    public void AddPiece(string team, Piece piece){

        var v2Pos = piece.transform.position;
        var pos = new Vector2Int((int)v2Pos.x, (int)v2Pos.y);
        piece.tile = tiles[pos];
        piece.tile.content = piece;

        // if (team == "GoldenPieces")
        // {
        //     goldenPieces.Add(piece);
        // }else{
        //     greenPieces.Add(piece);
        // }

    }
}
