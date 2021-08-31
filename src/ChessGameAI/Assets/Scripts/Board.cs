using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

//public delegate void TileClickedEvent(object sender, object args);
public class Board : MonoBehaviour
{
    public static Board Instance;
    public Transform GoldenHolder { 
        get { 
                return StateMachineController.Instance.Player1.transform; 
            } 
    }
    public Transform GreenHolder { 
        get { 
                return StateMachineController.Instance.Player2.transform;
            } 
    }    
    public List<Piece> GoldenPieces = new List<Piece>();
    public List<Piece> GreenPieces = new List<Piece>();
    public Dictionary<Vector2Int, Tile> Tiles = new Dictionary<Vector2Int, Tile>();    
    public Piece SelectedPiece;
    public HighlightClick SelectedHighlight;
    public AvailableMove SelectedMove;

    void Awake(){
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else if (Instance != this)
        {
            Destroy(Instance);
        }        
        
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

    private void CreateBoard(){
        
        for (var i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                CreateTile(i, j);
            }
        }

    }

    private void CreateTile(int line, int col){

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

    }

    [ContextMenu("Reset Board")]
    public void ResetBoard(){
        foreach (var tile in Tiles.Values)
        {
            tile.content = null;
        }
        foreach (var piece in GoldenPieces)
        {
            ResetPiece(piece);
        }
        foreach (var piece in GreenPieces)
        {
            ResetPiece(piece);
        }
    }

    private void ResetPiece(Piece piece)
    {
        if (!piece.gameObject.activeSelf)
        {
            return;
        }
        var pos = new Vector2Int((int) piece.transform.position.x, (int) piece.transform.position.y);
        Tiles.TryGetValue(pos, out piece.tile);
        piece.tile.content = piece;
    }
}
