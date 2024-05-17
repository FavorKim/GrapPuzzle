using UnityEngine.InputSystem;

namespace ObjectPool
{
    // 투사체를 오브젝트 풀링으로 관리하는 클래스
    public class ProjectilePool : ObjectPoolManager<Projectile>
    {
        protected override Projectile CreatePrefab()
        {
            Projectile obj = Instantiate(poolPrefab).GetComponent<Projectile>();
            obj.GetObjectPool(objectPool);
            return obj;
        }

        // 입력 없이 투사체를 생성(풀에서 가져오기)한다. (AI)
        public void OnShoot()
        {
            objectPool.Get();
        }

        // 입력을 받아 투사체를 생성(풀에서 가져오기)한다. (빙의)
        public void OnShoot(InputAction.CallbackContext context)
        {
            objectPool.Get();
        }
    }
}
