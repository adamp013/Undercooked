using UnityEngine;

public class GuidePanel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   [SerializeField] Animator animator;
    private bool isMenuOpen = true;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleMenu();
        }
    }
    
    public void Start()
    {
        animator.SetBool("isMenuOpen", isMenuOpen);
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        animator.SetBool("isMenuOpen", isMenuOpen);
    }
}
