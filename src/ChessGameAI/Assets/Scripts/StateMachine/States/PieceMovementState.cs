using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovementState : State
{
    public static List<AffectedPiece> AffectedPieces = new List<AffectedPiece>();
    public static AvailableMove EnPassantFlag;
    public override async void EnterAsync()
    {
        Debug.Log("Piece Movement State.");
        var tcs = new TaskCompletionSource<bool>();
        var moveType = Board.Instance.SelectedMove.MoveType;
        //ClearEnPassant();
        MovePiece(tcs, false, moveType);
        await tcs.Task;
        //await Task.Delay(100);
        Machine.ChangeTo<TurnEndState>();
    }
    public static void MovePiece(TaskCompletionSource<bool> tcs, bool skipMovement, MoveType moveType)
    {
        AffectedPieces = new List<AffectedPiece>();        
        EnPassantFlag = new AvailableMove();

        switch (moveType)
        {

            case MoveType.Normal:
                NormalMove(tcs, skipMovement);
                break;
            case MoveType.Castling:
                Castling(tcs, skipMovement);
                break;
            case MoveType.PawnDoubleMove:
                PawnDoubleMove(tcs, skipMovement);
                break;
            case MoveType.EnPassant:
                EnPassant(tcs, skipMovement);
                break;
            case MoveType.Promotion:
                Promotion(tcs, skipMovement);
                break;
        }
    }
    private static void NormalMove(TaskCompletionSource<bool> tcs, bool skipMovement)
    {
        var piece = Board.Instance.SelectedPiece as Piece;
        //Debug.Log("WasMoved:" + piece.WasMoved);
        var pieceMoving = piece.CreateAffected();
        pieceMoving.Piece = piece;
        pieceMoving.From = piece.tile;
        pieceMoving.To = Board.Instance.Tiles[Board.Instance.SelectedMove.Pos];
        //pieceMoving.WasMoved = piece.WasMoved;
        AffectedPieces.Insert(0, pieceMoving);        
        piece.tile.content = null;
        piece.tile = pieceMoving.To;
        
        if (piece.tile.content != null)
        {
            var deadPiece = piece.tile.content as Piece;
            var pieceKilled = new AffectedPiece();
            pieceKilled.Piece = deadPiece;
            pieceKilled.From = piece.tile;
            pieceKilled.To = piece.tile;
            AffectedPieces.Add(pieceKilled);
            Debug.LogFormat("The Piece {0} has been captured", deadPiece.transform.name);
            deadPiece.gameObject.SetActive(false);
        }
        piece.tile.content = piece;        
        
        if (skipMovement)
        {
            //piece.WasMoved = true;
            //piece.transform.position = Board.Instance.SelectedHighlight.transform.position;
            var Vector3Pos = new Vector3(Board.Instance.SelectedMove.Pos.x, Board.Instance.SelectedMove.Pos.y, 0);
            piece.transform.position = Vector3Pos;
            tcs.SetResult(true);
        }
        else
        {
            piece.WasMoved = true;
            var vector3Pos = new Vector3(Board.Instance.SelectedMove.Pos.x, Board.Instance.SelectedMove.Pos.y, 0);
            var timing = Vector3.Distance(piece.transform.position,vector3Pos) * 0.5f;
            LeanTween.move(piece.gameObject, vector3Pos, timing)
                     .setOnComplete(() => 
                     { 
                         tcs.SetResult(true); 
                     });           
            
        }
    }   
    private static void EnPassant(TaskCompletionSource<bool> tcs, bool skipMovement)
    {
        Debug.Log("EnPassant:");
        var pawn = Board.Instance.SelectedPiece as Pawn;
        //Debug.Log("Pawn: " + pawn.tile.pos);
        var direction = pawn.MaxKingdom ? new Vector2Int(0, -1) : new Vector2Int(0, 1);
        //Debug.Log("Direction: " + direction);
        //Debug.Log("SelectedMove: " + Board.Instance.SelectedMove.Pos + direction);
        //Debug.Log("En Passant:" + Board.Instance.SelectedHighlight.Tile.pos + direction);
        var enemy = Board.Instance.Tiles[Board.Instance.SelectedMove.Pos + direction];       
        Debug.Log("Enemy: " + enemy.content);
        var affectedEnemy = new AffectedPiece();
        affectedEnemy.From = enemy;
        affectedEnemy.To = enemy;
        affectedEnemy.Piece = enemy.content;
        //affectedEnemy.WasMoved = enemy.content.WasMoved;
        AffectedPieces.Add(affectedEnemy);
        enemy.content.gameObject.SetActive(false);
        enemy.content = null;        
        NormalMove(tcs, skipMovement);
    }
    private static void PawnDoubleMove(TaskCompletionSource<bool> tcs, bool skipMovement)
    {
        Debug.Log("PawnDoubleMove:");
        var pawn = Board.Instance.SelectedPiece as Pawn;
        var direction = pawn.MaxKingdom ? new Vector2Int(0, 1) : new Vector2Int(0, -1);
        //Board.Instance.Tiles[pawn.tile.pos + direction].MoveType = MoveType.EnPassant;
        EnPassantFlag = new AvailableMove(pawn.tile.pos + direction, MoveType.EnPassant);
        NormalMove(tcs, skipMovement);
    }
    private static void Castling(TaskCompletionSource<bool> tcs, bool skipMovement)
    {
        var king = Board.Instance.SelectedPiece;
        var affectedKing = new AffectedKingRook();
        affectedKing.From = king.tile;
        king.tile.content = null;
        //affectedKing.WasMoved = king.WasMoved;
        affectedKing.Piece = king;

        var rook = Board.Instance.Tiles[Board.Instance.SelectedMove.Pos].content;
        var affectedRook = new AffectedKingRook();
        affectedRook.From = rook.tile;        
        rook.tile.content = null;
        //affectedRook.WasMoved = rook.WasMoved;
        affectedRook.Piece = rook;

        var direction = rook.tile.pos - king.tile.pos;
        if (direction.x > 0)
        {
            king.tile = Board.Instance.Tiles[new Vector2Int(king.tile.pos.x + 2, king.tile.pos.y)];
            rook.tile = Board.Instance.Tiles[new Vector2Int(king.tile.pos.x - 1, rook.tile.pos.y)];
        }
        else
        {
            king.tile = Board.Instance.Tiles[new Vector2Int(king.tile.pos.x - 2, king.tile.pos.y)];
            rook.tile = Board.Instance.Tiles[new Vector2Int(king.tile.pos.x + 1, rook.tile.pos.y)];
        }

        king.tile.content = king;
        affectedKing.To = king.tile;
        AffectedPieces.Add(affectedKing);
        rook.tile.content = rook;
        affectedRook.To = rook.tile;
        AffectedPieces.Add(affectedRook);
        king.WasMoved = true;
        rook.WasMoved = true;

        // king.transform.position = new Vector3(king.tile.pos.x, king.tile.pos.y, 0);
        // rook.transform.position = new Vector3(rook.tile.pos.x, rook.tile.pos.y, 0);
        // tcs.SetResult(true);

        if (skipMovement)
        {
            tcs.SetResult(true);
        }
        else
        {
            LeanTween.move(king.gameObject, new Vector3(king.tile.pos.x, king.tile.pos.y, 0), 1.5f)
                     .setOnComplete(() => 
                     { 
                         tcs.SetResult(true); 
                     });
            LeanTween.move(rook.gameObject, new Vector3(rook.tile.pos.x, rook.tile.pos.y, 0), 1.4f);
        }
        
    }   
    private static async void Promotion(TaskCompletionSource<bool> tcs, bool skipMovement)
    {
        Debug.Log("Pawn Promotion");
        var movementTCS = new TaskCompletionSource<bool>();
        NormalMove(movementTCS, skipMovement);
        await movementTCS.Task;
        Pawn pawn = Board.Instance.SelectedPiece as Pawn; 
        
        if (!skipMovement)
        {
            StateMachineController.Instance.TaskHold = new TaskCompletionSource<object>();
            StateMachineController.Instance.PromotionPanel.SetActive(true);
            await StateMachineController.Instance.TaskHold.Task;
            var result = StateMachineController.Instance.TaskHold.Task.Result as string;
            
            if (result == "Knight")
            {                
                Board.Instance.SelectedPiece.Movement = pawn.KnightMovement;
            }
            else
            {
                Board.Instance.SelectedPiece.Movement = pawn.QueenMovement;
            }
            StateMachineController.Instance.PromotionPanel.SetActive(false);            
        }
        else
        {
            var affectedPawn = new AffectedPawn
            {
                WasMoved = true,
                ResetMovement = true,
                From = AffectedPieces[0].From,
                To = AffectedPieces[0].To,
                Piece = pawn
            };
            
            AffectedPieces[0] = affectedPawn;
            //Debug.Log(pawn.QueenMovement);
            pawn.Movement = pawn.QueenMovement;
        }
        tcs.SetResult(true);
    }
    private static void ClearEnPassant()
    {
        ClearEnPassant(5);
        ClearEnPassant(2);
    }
    private static void ClearEnPassant(int height)
    {
        var position = new Vector2Int(0, height);
        for (int i = 0; i < 7; i++)
        {
            position.x = position.x + 1;
            Board.Instance.Tiles[position].MoveType = MoveType.Normal;
        }
    }
}
