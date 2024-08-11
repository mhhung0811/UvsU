using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
                
    }
    public void EnableArrow()
    {
        _arrow.SetActive(true);
    }
    public void DisableArrow()
    {
        _arrow.SetActive(false);
    }
}
