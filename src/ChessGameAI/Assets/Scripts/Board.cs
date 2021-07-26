using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board _instance;
    public Transform GoldenHolder { 
        get { 
                return StateMachineController._instance.Player1.transform; 
            } 
    }
    public Transform GreenHolder { 
        get { 
                return StateMachineController._instance.Player2.transform;
            } 
    }    
    public List<Piece> GoldenPieces = new List<Piece>();
    public List<Piece> GreenPieces = new List<Piece>();
    public Dictionary<Vector2Int, Tile> Tiles = new Dictionary<Vector2Int, Tile>();

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
        GoldenPieces.AddRange(GoldenHolder.GetComponentsInChildren<Piece>());
        GreenPieces.AddRange(GreenHolder.GetComponentsInChildren<Piece>());
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
        Tiles.Add(tile.pos, tile);

    }

    public void AddPiece(string team, Piece piece){

        var v2Pos = piece.transform.position;
        var pos = new Vector2Int((int)v2Pos.x, (int)v2Pos.y);
        piece.tile = Tiles[pos];
        piece.tile.content = piece;

        // if (team == "GoldenPieces")
        // {
        //     goldenPieces.Add(piece);
        // }else{
        //     greenPieces.Add(piece);
        // }

    }
}
