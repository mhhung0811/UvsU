using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnstableFlatform : MonoBehaviour
{
    [SerializeField] private Vector2 _anchorA;
    [SerializeField] private Vector2 _anchorB;
    [SerializeField, Range(1f, 20f)] private float _speed;

    [SerializeField] private Tilemap _tile_map;
    
    private Vector2 _target;
    private void Start()
    {
        
    }

    private void Update()
    {
        PlatformMoving();
    }

    void PlatformMoving()
    {
        if (Vector2.Distance(_tile_map.tileAnchor, _anchorA) < 0.1f)
        {
            _target = _anchorB;
        }
        if (Vector2.Distance(_tile_map.tileAnchor, _anchorB) < 0.1f)
        {
            _target = _anchorA;
        }
        _tile_map.tileAnchor = Vector2.MoveTowards(_tile_map.tileAnchor, _target, _speed * Time.deltaTime);
        
    } 
}
