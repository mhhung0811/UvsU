using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{


    [SerializeField] private Transform _model;
    [SerializeField] private float _scale_x;
    [SerializeField] private float _scale_y;
    [SerializeField] private float _direct;

    public GameObject _current_one_way_platfrom { get; set; }

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
    public void LoadComponent()
    {
        this.SetModelScale();
    }
    private void SetModelScale()
    {
        this._scale_x = 0.1f;
        this._scale_y = 0.15f;
        _model.localScale = new Vector3(_scale_x, _scale_y, 1f);
        GetComponent<SpriteRenderer>().color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlatformEffector2D>() != null)
        {
            _current_one_way_platfrom = collision.gameObject;
        }
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlatformEffector2D>() != null)
        {
            _current_one_way_platfrom = null;
        }
    }

    public void ChangeState()
    {
        if(gameObject.name != "IterBlack(Clone)")
        {
            return;
        }
        GetComponent<SpriteRenderer>().color = new Color32(0x3D, 0x3D, 0x3D, 0xFF);
    }
}
