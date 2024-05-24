using ObjectPool;
using UnityEngine;

public class SpiderWeb : Projectile
{
    #region Components

    // ������Ʈ(Components)

    // AudioSource
    private AudioSource audioSource;

    // Mesh (Prefab)
    [SerializeField] private GameObject _webPrefab;

    #endregion Components

    #region Fields

    // ȸ�� ������ �ߺ� Ÿ���� ���� ���� ����
    private bool isAlreadyHit = false;

    #endregion Fields

    #region Awake()

    protected override void Awake()
    {
        base.Awake();

        // ������Ʈ�� �ʱ�ȭ�Ѵ�.
        audioSource = GetComponent<AudioSource>();
    }

    #endregion Awake()

    #region OnEnable() / OnDisable()

    private void OnEnable()
    {
        // Ÿ�� ���θ� �ʱ�ȭ�Ѵ�.
        isAlreadyHit = false;
    }

    #endregion Enable() / Disable()

    #region Collision Events

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹�� ���,
        if (other.CompareTag("Player") && !isAlreadyHit)
        {
            // Ÿ���ߴ�.
            isAlreadyHit = true;

            // �������� �����.
            _rigidbody.velocity = Vector3.zero;

            // ���� �ð� ��, �����Ѵ�.
            StartCoroutine(DelayedDestroy());
        }
    }

    #endregion Collision Events
}
