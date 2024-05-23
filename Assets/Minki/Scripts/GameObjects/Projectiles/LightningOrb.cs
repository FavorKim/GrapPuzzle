using ObjectPool;
using UnityEngine;

public class LightningOrb : Projectile
{
    #region Components

    // ������Ʈ(Components)

    // AudioSource
    private AudioSource audioSource;

    // ParticleSystem
    [SerializeField] private ParticleSystem _missile; // ����ü�� ��ü
    [SerializeField] private ParticleSystem _explosion; // ����ü�� ���� �� 1
    [SerializeField] private ParticleSystem _muzzle; // ����ü�� ���� �� 2

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

        _missile = Instantiate(_missile, transform).GetComponent<ParticleSystem>();
        _explosion = Instantiate(_explosion, transform).GetComponent<ParticleSystem>();
        _muzzle = Instantiate(_muzzle, transform).GetComponent<ParticleSystem>();
    }

    #endregion Awake()

    #region OnEnable() / OnDisable()

    private void OnEnable()
    {
        // Ÿ�� ���θ� �ʱ�ȭ�Ѵ�.
        isAlreadyHit = false;

        // ���� �� ����ü �⺻ ȿ���� ����Ѵ�.
        _missile.Play();
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

            //// ������ ȿ���� ����Ѵ�.
            _missile.Stop();
            _explosion.Play();
            _muzzle.Play();

            // ���� �ð� ��, �����Ѵ�.
            StartCoroutine(DelayedDestroy());
        }
    }

    #endregion Collision Events
}
