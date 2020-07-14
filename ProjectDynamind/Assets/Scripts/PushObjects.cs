using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;

public class PushObjects : MonoBehaviour
{
    float pushPower = 2.0f;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        Rigidbody body = hit.collider.attachedRigidbody;
     
        // no rigidbody or body == weapontype
        if (body == null || body.isKinematic || hit.transform.name.Contains("Dynamite") || hit.transform.name.Contains("Bazooka") || hit.transform.name.Contains("Grenade"))
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        int objectMult = 1;
        if (hit.gameObject.name == "Sphere")
            objectMult = 25;
        body.velocity = pushDir * pushPower * objectMult * 1f/body.mass;
    }
}
