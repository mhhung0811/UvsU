using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private Transform _model;
    [SerializeField] private float _scale_x;
    [SerializeField] private float _scale_y;
    [SerializeField] private float _direct;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {
        this.LoadComponent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LoadComponent()
    {
        this.SetModelScale();
    }
    private void SetModelScale()
    {
        this._scale_x = 0.1f;
        this._scale_y = 0.15f;
        _model.localScale = new Vector3(_scale_x, _scale_y, 1f);
    }
    public void SetDirection(float direct)
    {
        Vector3 new_scale = _model.localScale;
        new_scale.x = direct * _scale_x;
        _model.localScale = new_scale;
        this._direct = direct;
    }
    public float GetDirection()
    {
        return _direct;
    }
    
}
