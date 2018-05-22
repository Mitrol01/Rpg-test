using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MooveBullet : MonoBehaviour
{
        [SerializeField]
        Rigidbody body;

        [SerializeField]
        float moveForce = 10000f;

        [SerializeField]
        float LifeTime = 2f;
        float buffer = 0f;

        void Awake()
        {
            buffer = LifeTime;
            if (!body) body = GetComponent<Rigidbody>();
        }

        // Use this for initialization
        void Start()
        {
            body.AddForce(transform.forward * moveForce);
        }

        // Update is called once per frame
        void Update()
        {
            if (buffer <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                buffer -= Time.deltaTime;
            }
        }

        void OnCollisionEnter(Collision other)
        {
        Debug.Log(other.transform.name);
            if (other.transform.tag == "Enemi")
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
}
