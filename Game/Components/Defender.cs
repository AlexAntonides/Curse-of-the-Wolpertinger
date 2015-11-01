using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Offensive))]
public class Defender : MonoBehaviour {

    private float timer;

    private Offensive _offensiveComp;
    private Animator _animatorComp;

    private const string BOOL_SHOOT = "Shoot";

    void Start()
    {
        _offensiveComp = GetComponent<Offensive>();
        _animatorComp = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (Gamesystem.gameState == GameState.IN_WAVE)
        {
            if (timer > _offensiveComp.cooldown)
            {
                if (_offensiveComp.SearchAndAttack())
                    _animatorComp.SetBool(BOOL_SHOOT, true);
                timer = 0;
            }
        }
    }

    public void SetBoolFalse()
    {
        _animatorComp.SetBool(BOOL_SHOOT, false);
    }
}
