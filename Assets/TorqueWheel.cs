using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueWheel : MonoBehaviour
{
    private CapsuleCollider _collider;
    private ICollection<Rigidbody> _collidedRigidbodies;

    // Use this for initialization
    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _collidedRigidbodies = new List<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (var rigidbody in _collidedRigidbodies)
            {
                var transform = rigidbody.gameObject.transform;

                if (transform.rotation.eulerAngles.z >= 180 && transform.rotation.eulerAngles.z <= 270)
                {
                    var joint = gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = rigidbody;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!_collidedRigidbodies.Contains(collision.rigidbody))
        {
            _collidedRigidbodies.Add(collision.rigidbody);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        _collidedRigidbodies.Remove(collision.rigidbody);
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }
}
