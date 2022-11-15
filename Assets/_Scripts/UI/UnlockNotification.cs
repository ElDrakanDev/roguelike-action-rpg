using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using System;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Game.UI
{
    public class UnlockNotification : MonoBehaviour
    {
        static UnlockNotification Instance { get; set; }
        [SerializeField] float _readTime = 5f;
        [SerializeField] float _scaleDuration = 0.25f;
        [SerializeField] GameObject _notificationUi;
        [SerializeField] UITextName _notificationName;
        [SerializeField] UITextDescription _notificationDescription;
        [SerializeField] Image _notificationImage;
        Queue<Tuple<string, string, Sprite>> _notificationsQueue = new Queue<Tuple<string, string, Sprite>>();
        Vector3 _initialUiScale;
        bool _ShowingNotification { get => _notificationUi.activeInHierarchy; set => _notificationUi.SetActive(value); }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
            _initialUiScale = _notificationUi.transform.localScale;
        }
        private void OnEnable()
        {
            EventManager.onUnlockShow += QueueUnlock;
        }
        private void OnDisable()
        {
            EventManager.onUnlockShow -= QueueUnlock;
        }

        void QueueUnlock(string unlockName, string description, Sprite icon)
        {
            _notificationsQueue.Enqueue(Tuple.Create(unlockName, description, icon));
            TryShowUnlock();
        }
        async void TryShowUnlock()
        {
            if(_ShowingNotification is false && _notificationsQueue.TryDequeue(out var result))
            {
                _ShowingNotification = true;
                _notificationUi.transform.DOScaleX(_initialUiScale.x, _scaleDuration).SetEase(Ease.OutBounce);
                result.Deconstruct(out string unlockName, out string unlockDesc, out Sprite icon);
                _notificationName.Text = unlockName;
                _notificationDescription.Text = unlockDesc;
                var imglayout =_notificationImage.GetComponent<LayoutElement>();
                imglayout.preferredWidth = icon.texture.width;
                imglayout.preferredHeight = icon.texture.height;
                var imgRect = _notificationImage.GetComponent<RectTransform>();
                imgRect.sizeDelta = new Vector2(icon.texture.width, icon.texture.height);
                _notificationImage.sprite = icon;
                await Task.Delay((int)(_readTime * 1000));
                await _notificationUi.transform.DOScaleX(0, _scaleDuration).AsyncWaitForCompletion();
                _ShowingNotification = false;
                TryShowUnlock();
            }
        }
    }
}