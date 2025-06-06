using UnityEngine;

public class BeltScript : MonoBehaviour
{
   public float speed = 5f;
    public Vector3 direction = Vector3.forward;

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            Vector3 movement = direction.normalized * speed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }
}
