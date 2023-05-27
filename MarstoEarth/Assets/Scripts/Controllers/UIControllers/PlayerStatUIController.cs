using Character;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatUIController : MonoBehaviour
{
    public TMP_Text coreText;
    public TMP_Text maxHpText;
    public TMP_Text attackText;
    public TMP_Text speedText;
    public TMP_Text defenceText;
    public TMP_Text durationText;
    public TMP_Text rangeText;

    // 유저가 획득한 아이템 갯수
    private int _core;
    public int core
    {
        get => _core;
        set
        {
            _core = value;
            coreText.text = _core.ToString();
        }
    }
    // UI용 변수
    private float _maxHp;
    public float maxHp
    {
        get => _maxHp;
        set
        {
            _maxHp = value;
            maxHpText.text = _maxHp.ToString();
        }
    }
    private float _attack;
    public float attack
    {
        get => _attack;
        set
        {
            _attack = value;
            attackText.text = _attack.ToString();
        }
    }
    private float _speed;
    public float speed
    {
        get => _speed;
        set
        {
            _speed = value;
            speedText.text = _speed.ToString();
        }
    }
    private float _defence;
    public float defence
    {
        get => _defence;
        set
        {
            _defence = value;
            defenceText.text = _defence.ToString();
        }
    }
    private float _duration;
    public float duration
    {
        get => _duration;
        set
        {
            _duration = value;
            durationText.text = _duration.ToString();
        }
    }
    private float _range;
    public float range
    {
        get => _range;
        set
        {
            _range = value;
            rangeText.text = _range.ToString();
        }
    }
    public void InitStatUI(StatInfo statInfo)
    {
        maxHp = statInfo.maxHP;
        attack = statInfo.dmg;
        speed = statInfo.speed;
        defence = statInfo.def;
        duration = statInfo.duration;
        range = statInfo.range;
    }
    public void InitStaticStat()
    {
        maxHp = staticStat.maxHP;
        attack = staticStat.dmg;
        speed = staticStat.speed;
        defence = staticStat.def;
        duration = staticStat.duration;
        range = staticStat.range;
    }
}
