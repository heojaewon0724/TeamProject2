using UnityEngine;
using UnityEngine.Playables;

public abstract class SkillBase : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public string cutsceneObjectName; // PlayableDirector가 붙은 오브젝트 이름
    public bool allowMoveWhileCasting = false; // 시전 중 이동 허용 여부

    public abstract float cooldown { get; } // 스킬의 쿨타임 (초 단위)


    // 스킬 발동 시 호출
    public abstract void Activate(GameObject user);
}