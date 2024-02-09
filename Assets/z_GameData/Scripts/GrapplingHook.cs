using UnityEngine;
using UnityEngine.UI;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private float _hookSpeed = 10f; // Speed of the hook
    [SerializeField] private float _retractSpeed = 20f; // Speed at which the hook retracts
    [SerializeField] private float _maxDistance = 3f; // Maximum distance the hook can travel
    [SerializeField] private float _coolDownTime = 1f; // cool down time for checking hook and player distance in update

    [SerializeField] private float _manaDecreaseAmount = 0.3f;
    [SerializeField] private float _manaIncreaseAmount = 0.01f;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Image _playerManaFillBar;

    [SerializeField] private bool _isGrappling = false;
    private Vector3 _targetPosition;
    private Transform _hookTransform;
    private float _tempTime;

    void Start()
    {
        _hookTransform = transform;
        _targetPosition = _playerTransform.position;

        _tempTime = _coolDownTime;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !_isGrappling && _playerManaFillBar.fillAmount > _manaDecreaseAmount)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _targetPosition = mousePosition;
            _isGrappling = true;
            _playerManaFillBar.fillAmount -= _manaDecreaseAmount;
            Debug.Log("Hook");
        }

        if (_isGrappling)
        {
            //make line btw player pos and hook current pos
            _lineRenderer.SetPosition(0, _playerTransform.position);
            _lineRenderer.SetPosition(1, _hookTransform.position);

            //move hook towards target pos
            _hookTransform.position = Vector3.MoveTowards(_hookTransform.position, _targetPosition, _hookSpeed * Time.deltaTime);

            //set distance to maxDistance if its greater than it
            float distance = Vector3.Distance(_playerTransform.position, _targetPosition);
            if (distance > _maxDistance)
                distance = _maxDistance;


            if (Vector3.Distance(_hookTransform.position, _playerTransform.position) >= distance)
            {
                // Retract the hook if its greater than distance
                _targetPosition = _playerTransform.position;
            }
            _tempTime -= Time.deltaTime;
            if (Vector3.Distance(_hookTransform.position, _playerTransform.position) < 0.1f && _tempTime <= 0)
            {
                _isGrappling = false;
                _tempTime = _coolDownTime;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).transform.localPosition = Vector3.zero;
                }
            }
            return;
        }
        if (!Input.GetMouseButtonDown(1) && !_isGrappling)
        {
            _playerManaFillBar.fillAmount += _manaIncreaseAmount * Time.deltaTime;

            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);

            // Retract the hook if isGrappling is false
            _targetPosition = _playerTransform.position;
            _hookTransform.position =
                Vector3.MoveTowards(_hookTransform.position, _targetPosition, _retractSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isGrappling && other.CompareTag("Enemy"))
        {
            other.transform.parent = transform;
            other.transform.localPosition = Vector3.zero;
            other.GetComponent<Enemy>().GetStuckByHook();

            //// Stop grappling
            //_isGrappling = false;

            //// Retract the hook
            //_targetPosition = _playerTransform.position;
        }
    }
}
