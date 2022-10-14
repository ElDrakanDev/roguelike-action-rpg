using UnityEngine;
using DG.Tweening;

namespace Game.Entities
{
    public class EntityInstantiate : MonoBehaviour
    {
        public EntityDataSO data;
        public float effectDuration = 0.5f;

        void Start()
        {
            InstantiateEntity(transform.position);
        }

        void InstantiateEntity(Vector3 position)
        {
            transform.position = position;
            transform.localScale = Vector3.zero;
            if(!TryGetComponent(out SpriteRenderer renderer))
                renderer = gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = data.Prefab.GetComponent<SpriteRenderer>().sprite;
            var targetScale = data.Prefab.transform.localScale;
            transform.DOScale(targetScale, effectDuration).OnComplete(
                () => {
                    Entity.Create(data, position);
                    Destroy(gameObject);
                }
            );
        }
    }
}
