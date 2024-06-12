using UnityEngine;

public class JumpButton : MonoBehaviour
{
    public void JumpPressed()
    {
        Player.instance.wantsJump = true;
    }
}