using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SpawnMonster : MonoBehaviour
{
    public Monsters[] monsters;
    NavMeshAgent agent;
    private void Awake()
    {
        Instantiate(monsters[0], gameObject.transform);
    }

    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        //�ش� ��ġ�� �������� �ϳ� ��ȯ
        //int randNumber = Random.Range(0, monsters.Length);
        //Instantiate(monsters[0], gameObject.transform);
        /*monsters[randNumber].transform.Translate(collision.transform.position);
        monsters[randNumber].gameObject.SetActive(true);*/
        Destroy(gameObject);
    }
}
