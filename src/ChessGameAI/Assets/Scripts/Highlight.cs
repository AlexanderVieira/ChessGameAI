using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    public static Highlight Instance;
    public SpriteRenderer HighlightPrefab;
    private Queue<SpriteRenderer> _activeHighlights = new Queue<SpriteRenderer>();
    private Queue<SpriteRenderer> _onReserve = new Queue<SpriteRenderer>();
    private void Awake(){

        Instance = this;

    }    

    public void SelectTiles(List<Tile> tiles){

        foreach (var tile in tiles)
        {
            if (_onReserve.Count == 0)
            {
                CreateHighlight();
            }

            var spriteRenderer = _onReserve.Dequeue();
            spriteRenderer.gameObject.SetActive(true);
            spriteRenderer.color = StateMachineController.Instance.CurrentlyPlaying.color;
            spriteRenderer.transform.position = new Vector3(tile.pos.x, tile.pos.y, 0);
            spriteRenderer.GetComponent<HighlightClick>().Tile = tile;
            _activeHighlights.Enqueue(spriteRenderer);
        }
    }

    [ContextMenu("DeSelect")]
    public void DeSelectTiles(){
        while (_activeHighlights.Count != 0)
        {
            var spriteRenderer = _activeHighlights.Dequeue();
            spriteRenderer.gameObject.SetActive(false);
            _onReserve.Enqueue(spriteRenderer);
        }
    }

    private void CreateHighlight()
    {
        var spriteRenderer = Instantiate(HighlightPrefab, Vector3.zero, Quaternion.identity, transform);
        _onReserve.Enqueue(spriteRenderer);
    }
    
}
