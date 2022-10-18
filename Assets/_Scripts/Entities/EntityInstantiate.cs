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
            Run.Run.instance.navigator.enemiesLeft = true;
            transform.position = position;
            transform.localScale = Vector3.zero;
            if(!TryGetComponent(out SpriteRenderer renderer))
                renderer = gameObject.AddComponent<SpriteRenderer>();
            var prefabRenderer = data.Prefab.GetComponent<SpriteRenderer>();
            renderer.sprite = prefabRenderer.sprite;
            renderer.color = prefabRenderer.color;
            var targetScale = data.Prefab.transform.localScale;
            transform.DOScale(targetScale, effectDuration).OnComplete(
                () => {

                    Entity.Create(data, position, parent: transform.parent);
                    Entity.UpdateEntitiesLeft();
                    Destroy(gameObject);
                }
            );
        }
    }
}
