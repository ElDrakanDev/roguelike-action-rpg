using UnityEngine;

[CreateAssetMenu(menuName = "SpriteAnimation")]
public class SpriteAnimationSO : ScriptableObject
{
    public float cycleSeconds = 0.3f;
    public bool looping = true;
    public bool unskippable = false;
    public Sprite[] sprites;
    public SpriteAnimationSO nextAnimation;
}
