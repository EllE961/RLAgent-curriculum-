using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 100f;

    void Update()
    {
        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        transform.Translate(0, 0, move);
        transform.Rotate(0, turn, 0);
    }
}
