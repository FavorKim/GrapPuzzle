using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    private static MySceneManager instance;
    public static MySceneManager Instance {  get { return instance; } }


    [SerializeField] GameObject loading;
    [SerializeField]CanvasGroup blocker;
    [SerializeField] private float fadeDuration;
    float percentage;


    private void Awake()
    {
        blocker = GetComponentInChildren<CanvasGroup>();

    }
    private void Start()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        Debug.Log("start");
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += FadeIn;
    }

    private void Update()
    {
        Debug.Log(percentage);
    }

    public void ChangeScene(string sceneName)
    {
        FadeOut(sceneName);
    }

    void LoadScene(string sceneName)
    {
        StartCoroutine(CorLoadScene(sceneName));
    }



    void FadeOut(string sceneName)
    {
        blocker.DOFade(1, fadeDuration).OnStart(() => blocker.blocksRaycasts = true).OnComplete(() => { LoadScene(sceneName); });
    }
    void FadeIn(Scene scene, LoadSceneMode mode)
    {
        blocker.DOFade(0, fadeDuration).OnStart(() => blocker.blocksRaycasts = false) ;
    }


    IEnumerator CorLoadScene(string sceneName)
    {
        //loading.SetActive(true);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            yield return null;
            percentage = async.progress;
            if (percentage >= 0.9f)
                async.allowSceneActivation = true;
        }
    }

    // fadeout -> �Ϸ� �� �� �ε� -> �Ϸ� �� FadeIn
    /*
    ���� - �̻��� ������ ���� (����, �ڽ� ��)
    ���� - �̻��� ������ (���� ����), ���� ������(�ڽ� ���� �� ���� �ʱ�ȭ)

    -����-
    �� ���� ������ �����ϴ°� objectPool�� projectile�� ����.
    �ڽ� - �� �������� Projectile �ȿ� missle, muzzle, explosion�� ������.
    -����-
    Projectile �ȿ� �ڽĵ��� �־���� ����,
    �ڽ��������� Projectile�� ����ȭ �� ��, Projectile ���ο��� �ڽĵ��� Instantiate
    Instantiate �� ���� �ʱ�ȭ.


    class Projectile
    {
        [serializefield] GameObject misslePrefab;
        [serializefield] GameObject muzzlePrefab;
        [serializefield] GameObject explosionPrefab;
        ParticleSystem misslefx;
        ParticleSystem muzzlefx;
        ParticleSystem explosionfx;
        
        void Awake()
        {
            misslefx = Instantiate(missle, transform).GetComponent<ParticleSystem>();
            misslefx = Instantiate(missle, transform).GetComponent<ParticleSystem>();
            misslefx = Instantiate(missle, transform).GetComponent<ParticleSystem>();
        }

        void OnTriggerEnter(Collision other)
        {
            if(other.CompareTag("Player)
            {   
                misslefx.Stop();
                explosionfx.Play();
            }
        }
    }


    
    ����ȭ�� PS������ Stop�ϸ�
    clone�� �ƴ϶� ������ ������ PS�� Stop.
    �츮�� Clone�� �����ؾ��Ѵ�.








    Awake
    ParticleSystem[] fxs = GetComponentsInChildren<GameObject>().GetComponent<ParticleSystem>();
    missle = fxs[0];
    explosion = fxs[1];
    muzzle = fxs[2];
    */

}
