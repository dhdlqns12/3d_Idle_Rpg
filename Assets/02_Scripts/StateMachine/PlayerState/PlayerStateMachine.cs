using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; private set; }

    public float BasePlayerMaxHP { get; private set; }
    public float BaseMovementSpd { get; private set; }
    public float BasePlayerAtk { get; private set; }
    public float BasePlayerAtkSpd { get; private set; }
    public float BaseGetGoldRate { get; private set; }
    public float BaseAttackRange { get; private set; }
    public float BaseDetectionRange { get; private set; }

    public float UpgradePlayerMaxHP { get; set; }
    public float UpgradedMovementSpd { get; set; }
    public float UpgradedAtk { get; set; }
    public float UpgradedAtkSpd { get; set; }

    public float TotalMaxHP => BasePlayerMaxHP + UpgradePlayerMaxHP;
    public float TotalMovementSpd => BaseMovementSpd + UpgradedMovementSpd;
    public float TotalAtk => BasePlayerAtk + UpgradedAtk;
    public float TotalAtkSpd => BasePlayerAtkSpd + UpgradedAtkSpd;

    public PlayerStateMachine(Player _player)
    {
        this.Player = _player;

        InitStats();
    }

    private void InitStats()
    {
        this.BasePlayerMaxHP = Player.Data.MaxHP;
        this.BaseMovementSpd = Player.Data.BaseSpd;
        this.BasePlayerAtk = Player.Data.BaseAtk;
        this.BasePlayerAtkSpd = Player.Data.BaseAtkSpd;
        this.BaseGetGoldRate = Player.Data.BaseGetGoldRate;
        this.BaseAttackRange = Player.Data.AttackRange;
        this.BaseDetectionRange = Player.Data.DetectionRange;

        LoadUpgrade();
    }

    private void LoadUpgrade()
    {
        // TODO: 나중에 SaveManager에서 업그레이드 로드 만약 세이브를 구현 한다면...
    }
}
