using System;
using System.Collections;
using _Project.Scripts.Card;
using _Project.Scripts.TriggerEndGame;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using YG;

namespace _Project.Scripts.Controller
{
    public class InputController : MonoBehaviour, IControllable
    {
        private const float FIXED_Y_POSITION = 3.15f;
        private const float MIN_X = -2.25f;
        private const float MAX_X = 2.25f;
        private const float LERP_SPEED = 25f;
        private const float SPAWN_DELAY = 0.7f;

        private Vector3 _mousePos;
        private Camera _mainCam;
        private Transform _transform;

        public GameObject cardPrefab;
        private SpriteRenderer _spriteRenderer;
        private Card.Card _futureCard;
        private Sprite _futureSprite;

        private bool _canSpawn = true;
        private bool _moveEnded;

        public LayerMask layerMask;
        public LayerMask bottomMask;
        private Sprite _hitSprite;

        public static Action onStepsEnded;
        
        

        private void Start()
        {
            _mainCam = Camera.main;
            _transform = GetComponent<Transform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            // UpdateFutureCardSprite();
            StartCoroutine(CheckMoves());

            if (YandexGame.savesData.gameObjectDataList.Count == 0)
            {
                UpdateFutureCardSprite();
                SpawnFirstCards();
            }
            else
            {
                SetSprite();
            }
        }

        private void Update()
        {
            CheckRaycast();
            if (!TriggerEnd.messageSent)
            {
                Move(_mousePos);
            }


            if (Input.GetMouseButtonUp(0) && _canSpawn)
            {
                if (!EventSystem.current.IsPointerOverGameObject() && !TriggerEnd.messageSent)
                {
                    StartCoroutine(SpawnCardWithDelay());
                }
            }
        }

        private void CheckRaycast()
        {
            // Пускаем луч вниз от позиции объекта
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, layerMask);

            // Проверяем, попал ли луч в объект
            if (hit.collider != null)
            {
                // Получаем компонент SpriteRenderer объекта, в который попал луч
                SpriteRenderer spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();

                if (spriteRenderer != null)
                {
                    // Получаем спрайт объекта
                    _hitSprite = spriteRenderer.sprite;
                    // Debug.Log("Hit object with sprite: " + _hitSprite.name);
                }
            }
        }

        public void Move(Vector3 mousePos)
        {
            // mousePos = Input.mousePosition;
            // mousePos = _mainCam.ScreenToWorldPoint(mousePos);
            // var clampedX = Mathf.Clamp(mousePos.x, MIN_X, MAX_X);
            // var roundedX = Mathf.Round(clampedX / 0.75f) * 0.75f;
            // var targetPosition = new Vector3(roundedX, FIXED_Y_POSITION, transform.position.z);
            // _transform.position = Vector3.Lerp(_transform.position, targetPosition, LERP_SPEED * Time.deltaTime);
            
            // mousePos = Input.mousePosition;
            // mousePos = _mainCam.ScreenToWorldPoint(mousePos);
            // var clampedX = Mathf.Clamp(mousePos.x, MIN_X, MAX_X);
            // var roundedX = Mathf.Round(clampedX / 0.75f) * 0.75f;
            // var targetPosition = new Vector3(roundedX, FIXED_Y_POSITION, transform.position.z);
            // _transform.position = Vector3.Lerp(_transform.position, targetPosition, LERP_SPEED * Time.deltaTime);
            //
            // _moveEnded = Vector3.Distance(_transform.position, targetPosition) < 0.01f;
            
            // no lerp
            mousePos = Input.mousePosition;
            mousePos = _mainCam.ScreenToWorldPoint(mousePos);
            var clampedX = Mathf.Clamp(mousePos.x, MIN_X, MAX_X);
            var roundedX = Mathf.Round(clampedX / 0.75f) * 0.75f;
            var targetPosition = new Vector3(roundedX, FIXED_Y_POSITION, transform.position.z);
            _transform.position = targetPosition; // Прямое присвоение позиции
        }

        private IEnumerator SpawnCardWithDelay()
        {
            _canSpawn = false;
            if (CanSpawnCard())
            {
                SpawnCard();
            }

            yield return new WaitForSeconds(SPAWN_DELAY);
            _canSpawn = true;
            StartCoroutine(CheckMoves());
        }

        private bool CanSpawnCard()
        {
            if (_hitSprite == null || _futureSprite == null)
            {
                return false;
            }

            // Сравниваем масти спрайтов
            string hitSuit = _hitSprite.name.Substring(_hitSprite.name.Length - 1);
            string futureSuit = _futureSprite.name.Substring(_futureSprite.name.Length - 1);

            // Сравниваем ранги спрайтов
            int hitRank = int.Parse(_hitSprite.name.Substring(0, _hitSprite.name.Length - 1));
            int futureRank = int.Parse(_futureSprite.name.Substring(0, _futureSprite.name.Length - 1));

            return hitSuit == futureSuit && futureRank <= hitRank;
        }

        private void SpawnCard()
        {
            if (_futureSprite != null)
            {
                CardDealer.Instance.SpawnCardWithSprite(cardPrefab, transform.position, _futureCard, _futureSprite);
                UpdateFutureCardSprite();
            }
        }

        private void SpawnFirstCards()
        {
            const float y = -4f;
            const float startX = -2.25f;
            const float stepX = 0.75f;
            const int numberOfObjects = 7;

            Vector3[] positions = new Vector3[numberOfObjects];

            for (var i = 0; i < numberOfObjects; i++)
            {
                var x = startX + i * stepX;
                positions[i] = new Vector3(x, y, 0);
            }

            CardDealer.Instance.SpawnSpecificCardsAtPositions(cardPrefab, positions);
        }

        private void UpdateFutureCardSprite()
        {
            var (futureCard, futureSprite) = CardDealer.Instance.GetFutureCardSprite();
            if (futureCard != null && futureSprite != null)
            {
                _futureCard = futureCard;
                var spritePath = $"{_futureCard.Rank}{_futureCard.Suit[0].ToString().ToLower()}";
                _futureSprite = Resources.Load<Sprite>($"Sprites/{spritePath}");
                _spriteRenderer.sprite = _futureSprite;
                transform.localScale = Vector3.zero;
                transform.DOScale(new Vector3(0.6f, 0.6f, 1f), 0.2f);
            }
            else
            {
                Debug.LogWarning("Future card sprite not loaded, skipping update.");
            }
        }

        private void SetSprite()
        {
            _futureCard = YandexGame.savesData.FutureCard;
            var spritePath = $"{_futureCard.Rank}{_futureCard.Suit[0].ToString().ToLower()}";
            _futureSprite = Resources.Load<Sprite>($"Sprites/{spritePath}");
            _spriteRenderer.sprite = _futureSprite;
            transform.localScale = Vector3.zero;
            transform.DOScale(new Vector3(0.6f, 0.6f, 1f), 0.2f);
        }
        private IEnumerator CheckMoves()
        {
            var movePossible = false;
        
            for (var x = MIN_X; x <= MAX_X; x += 0.75f)
            {
                var position = new Vector2(x, FIXED_Y_POSITION);
                var hit = Physics2D.Raycast(position, Vector2.down, Mathf.Infinity, layerMask);
                
                if (hit.collider != null)
                {
                    var hitSpriteRenderer = hit.collider.GetComponent<SpriteRenderer>();
        
                    if (hitSpriteRenderer != null)
                    {
                        var hitSprite = hitSpriteRenderer.sprite;
        
                        if (_futureSprite != null && hitSprite != null)
                        {
                            var hitSuit = hitSprite.name.Substring(hitSprite.name.Length - 1);
                            var futureSuit = _futureSprite.name.Substring(_futureSprite.name.Length - 1);
        
                            var hitRank = int.Parse(hitSprite.name.Substring(0, hitSprite.name.Length - 1));
                            var futureRank = int.Parse(_futureSprite.name.Substring(0, _futureSprite.name.Length - 1));
        
                            if (hitSuit == futureSuit && futureRank <= hitRank)
                            {
                                movePossible = true;
                                break; 
                            }
                        }
                    }
                }
        
                yield return null;
            }
        
            if (!movePossible)
            {
                onStepsEnded?.Invoke();
                // print($"<color=#e34234>==========================Ходов нет===================</color>");
            }
        }
    }
}