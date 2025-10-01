using UnityEngine;
using TMPro;

public class HUDPlayer : MonoBehaviour
{
    [Header("Referencias")]
    public Animator playerAnimator;      
    public TextMeshProUGUI infoText;      

    [Header("Formato")]
    public string pattern = "Speed: {0:0.00} | Grounded: {1}";

    void Reset()
    {
        infoText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (playerAnimator == null || infoText == null) return;

        float speed = playerAnimator.GetFloat("Speed");
        bool grounded = playerAnimator.GetBool("IsGrounded");

        infoText.text = string.Format(pattern, speed, grounded);
    }
}
