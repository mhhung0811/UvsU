using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform _holder;
    public Transform _all_prefabs;
    public List<Transform> _list_prefab;
    public List<Transform> _list_item;

    // Start is called before the first frame update
    public void Start()
    {
        HideAllPrefabs();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
    public void HideAllPrefabs()
    {
        //Debug.Log($"{_all_prefabs.transform.childCount}");
        foreach (Transform prefab in _all_prefabs)
        {
            _list_prefab.Add(prefab);
            prefab.gameObject.SetActive(false);
        }
    }

    public GameObject Spawn(string prefab_name, Vector3 spawn_pos, Vector3 rotation, float _direct)
    {
        Transform obj = GetObjectFromPool(prefab_name);

        if(obj == null)
        {
            obj = GenerateNewObj(prefab_name);
        }
        if(obj == null)
        {
            Debug.Log("Can not find prefab");
            return null;
        }
        obj.gameObject.SetActive(true);
        ConfigPosAndRotation(obj, spawn_pos, rotation, _direct);
        obj.parent = _holder;

        if (obj == null) return null;
        else return obj.gameObject;
    }
    public Transform GetObjectFromPool(string prefab_name)
    {
        foreach (Transform transform in _list_item)
        {
            if (transform.name == prefab_name + "(Clone)")
            {
                _list_item.Remove(transform);
                return transform;
            }
        }
        return null;
    }
    public Transform GenerateNewObj(string prefab_name)
    {
        foreach(Transform obj in _list_prefab)
        {
            if(obj.name == prefab_name)
            {
                Transform new_obj = Instantiate(obj);
                return new_obj;
            }
        }
        return null;
    }
    public void ConfigPosAndRotation(Transform obj, Vector3 spawnPos, Vector3 rotation, float direct)
    {
        obj.position = spawnPos;

        if (obj.GetComponent<BulletMovement>())
        {
            obj.gameObject.GetComponent<BulletMovement>().SetDirection(direct);
        }
    }
    public void Despawn(Transform obj)
    {
        obj.gameObject.SetActive(false);
        _list_item.Add(obj);
    }

    public void DespawnAll()
    {
        foreach(Transform child in _holder)
        {
            if (child.gameObject.activeSelf)
            {
                Despawn(child);
            }
        }
    }
}
