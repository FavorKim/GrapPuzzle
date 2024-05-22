using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    PlayerController player;
    [SerializeField] CinemachineFreeLook tpsCam;
    //public PlayerController Player { get { return player; } }



    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = FindAnyObjectByType<PlayerController>();
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(GameManager)).GetComponent<GameManager>();
            if (instance == null)
            {
                GameObject obj = new GameObject("GameManager");
                obj.AddComponent<GameManager>();
                instance = obj.GetComponent<GameManager>();
                DontDestroyOnLoad(obj);
            }
        }
    }

    //public void GetDamage(Obstacles obs, GameObject dest)
    //{
    //    if (dest.CompareTag("Player") && obs.Damage != 0)
    //        player.GetDamage(obs.Damage);
    //    else if (dest.GetComponent<ITyped>() != null)
    //        SetTypeAttack(obs, dest.GetComponent<ITyped>());
    //}

    //public void GetDamage(int dmg)
    //{
    //    player.GetDamage(dmg);
    //}

    ///// <summary>
    ///// ���Ͱ� �ƴ� �������� �Ӽ� ������ �޾��� ��
    ///// </summary>
    ///// <param name="from">������</param>
    ///// <param name="to">�ǰ���</param>
    //public void SetTypeAttack(Obstacles from, ITyped to)
    //{
    //    // �����ڰ� ���ݴ�� �Ӽ����� �켼�� ��� ������ ����
    //    to.OnTypeAttacked(from);
    //}

    public void SetCameraFollow(Transform dest)
    {
        tpsCam.Follow = dest;
    }
    public void SetCameraLookAt(Transform dest)
    {
        tpsCam.LookAt = dest;
    }
}
