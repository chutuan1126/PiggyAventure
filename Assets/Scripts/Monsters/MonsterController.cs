using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour
{
    private static MonsterController instance;
    public static MonsterController Instance => instance;

    private int attackCount = 0;
    private bool isAttack = false;

    protected void Awake()
    {
        if (MonsterController.instance != null) Debug.LogError("Only 1 GridManagerCtrl allow to exist");
        MonsterController.instance = this;
    }

    public virtual void SetIsAttack(bool value)
    {
        isAttack = value;
    }

    public virtual bool GetIsAttack()
    {
        return this.isAttack;
    }

    public virtual void SetAttackCount(int value)
    {
        if (value > attackCount)
            attackCount = value;
    }

    public virtual int GetAttackCount()
    {
        return this.attackCount;
    }
}
