using UnityEngine;

public class PlayerDataSetting: MonoBehaviour
{
    [Header("캐릭터 스탯 설정")]
    [SerializeField] private int speed;
    public int Speed => speed;
    [SerializeField] private int dashSpeed;
    public int DashSpeed => dashSpeed;
    [SerializeField] private float dashDuration;
    public float DashDuration => dashDuration;
    [SerializeField] private float dashEffectCount;
    public float DashEffectCount => dashEffectCount;
    [SerializeField] private float playerDashFadeTime;
    public float PlayerDashFadeTime => playerDashFadeTime;
    
    [SerializeField] private float dashCoolTime;
    public float DashCoolTime => dashCoolTime;
    
    [SerializeField] private float invincibleTime;
    public float InvincibleTime => invincibleTime;
}