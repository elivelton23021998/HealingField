using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    public static Equip instance;

    [SerializeField] private int maxEquip;
    [SerializeField] private float _timeReduct;

    public float timeReduct { get { return _timeReduct; } }

    [Header("Equip Upgrade")]
    [SerializeField] private float _equipLv1_ReductTime;
    [SerializeField] private float _equipLv2_ReductTimekit;
    [SerializeField] private float _equipLv3_ReductTimeMedicin;

    public float equipLv1_ReductTime { get { return _equipLv1_ReductTime; } }
    public float equipLv2_ReductTimekit { get { return _equipLv2_ReductTimekit; } }
    public float equipLv3_ReductTimeMedicin { get { return _equipLv3_ReductTimeMedicin; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
