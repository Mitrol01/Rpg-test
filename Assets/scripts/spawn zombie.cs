using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomMeteor : MonoBehaviour
{

    public GameObject _zombiePrefab;
    public float _spawningTimer = 2f;

    public Transform _whereToCreate;
    public float _rangeToCreate = 20;

    void Start()
    {
        InvokeRepeating("CreateMeteor", 0, _spawningTimer);

    }

    int _zombieCount;
    void CreateMeteor()
    {

        Vector3 position = _whereToCreate.position + RandomPosition(_rangeToCreate);
        _zombiePrefab = Instantiate(_zombiePrefab, position, Quaternion.identity);
        _zombiePrefab.name = "Created Meteor " + _zombieCount++;

    }

    private Vector3 RandomPosition(float rangeToCreate)
    {
        return new Vector3(UnityEngine.Random.Range(-_rangeToCreate, rangeToCreate), UnityEngine.Random.Range(-_rangeToCreate, rangeToCreate), UnityEngine.Random.Range(-_rangeToCreate, rangeToCreate));
    }
}