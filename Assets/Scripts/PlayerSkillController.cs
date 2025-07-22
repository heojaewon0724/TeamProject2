using UnityEngine;
using UnityEngine.Playables;
public class PlayerSkillController : MonoBehaviour
{
    public PlayableDirector skillCutsceneDirector;
    public PlayerInput playerInput;

    public Animator playerAnimator;

    public SkillBase[] skills;
    private float[] skillCooldownTimers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skillCooldownTimers = new float[skills.Length];
    }

    // Update is called once per frame
    void Update()
    {
       // if(GameManager.instance != null && GameManager.instance.)
    }
}
