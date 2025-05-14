using UnityEngine;

public class ConveyerBeltVisual : MonoBehaviour
{
   public float scrollSpeed = 1f;
    private Renderer rend;
    private Vector2 offset;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        offset = new Vector2(0, Time.time * scrollSpeed); // or change to X if needed
        rend.material.mainTextureOffset = offset;
    }
}
