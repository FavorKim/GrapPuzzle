using BehaviourTree;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyBT : MonoBehaviour
    {
        // Enemy ��ü
        public Enemy _enemy { get; set; }

        Node rootNode;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            rootNode = SetBT();
            
        }

        private void Update()
        {
            rootNode.Evaluate();
            Debug.Log("Enemy's Name = " + _enemy._name);
        }

        public Node SetBT()
        {
            Node node = new Selector(new List<Node>
        {
            new Sequence(new List<Node>()
            {
                new CheckNearToAttack(_enemy), new Attack(_enemy)
            }),

            new Sequence(new List<Node>()
            {
                new CheckNearToChase(_enemy), new Chase(_enemy),
            }),

            new Patrol(_enemy),
        });

            return node;
        }
    }
}
