using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : MonoBehaviour {

    private Vector3 Target;
    private Vector3 PlayerTarget = Vector3.one;
    public bool Abducting;
    public Vector2 SpeedMinAndMax;
    private float _speed;
    public float SpeedAbductingModifier = 1.5f;
    private bool _doesMove;

    public float Magnitude = 1.0f;
    public Vector3 CustomMovementVector;

    private Transform _transform;
    private Rigidbody _myRigidbody;

    public Vector2 ChangeDirectionInterval = Vector2.one;

    public Vector2 XRange = Vector2.one;
    public Vector2 YRange = Vector2.one;
    public Vector2 ZRange = Vector2.one;

    public Color GizmoColor = Color.white;

    private Vector3 _smjer;

    private void Awake()
    {
        // ovo ispod je zapravo transform = GetComponent<Transform>
        _transform = transform; //ovo je stvar dobre prakse, mogli smo dolje i direktno mijenjati vlasiti transform.position
        _myRigidbody = GetComponent<Rigidbody>();

        _speed = Vector2RandomExtension.V2Random(SpeedMinAndMax);        

        Target = Vector3.one;

        _doesMove = true;

    }

    public void Start()
    {
        Target = new Vector3(XRange.RandomValue(), YRange.RandomValue(), ZRange.RandomValue());

        StartCoroutine(DefineNewPosition());
    }

    private void FixedUpdate()
    {
        if (_doesMove && !Abducting)
        {
            _myRigidbody.velocity = (Target - _transform.position).normalized * _speed;
        }

        if (!_doesMove)
        {
            _myRigidbody.velocity = Vector3.zero;
        //Abducting = false;
        }

        if(_doesMove && Abducting)
        {
            _myRigidbody.velocity = (PlayerTarget - _transform.position).normalized * _speed * SpeedAbductingModifier;
        }
    }

    public IEnumerator DefineNewPosition()
    {
        while (GameManager.GM.CurrentState == GameManager.GameState.Playing)
        {
            Target = new Vector3(XRange.RandomValue(), YRange.RandomValue(), ZRange.RandomValue());
            _speed = Vector2RandomExtension.V2Random(SpeedMinAndMax);

            yield return new WaitForSeconds(ChangeDirectionInterval.RandomValue());
        }
    }

    private void OnDrawGizmos() //nacrtati prostor u kojem se spawnaju neprijatelji
    {
        Gizmos.color = Color.red;
        float _sizeX = XRange.y - XRange.x;
        float _sizeY = YRange.y - YRange.x;
        float _sizeZ = ZRange.y - ZRange.x;

        float _middleX = XRange.x + (XRange.y - XRange.x) / 2;
        float _middleY = YRange.x + (YRange.y - YRange.x) / 2;
        float _middleZ = ZRange.x + (ZRange.y - ZRange.x) / 2;

        Vector3 _middle = new Vector3(_middleX, _middleY, _middleZ);
        Vector3 _size = new Vector3(_sizeX, _sizeY, _sizeZ);
        Gizmos.DrawWireCube(_middle, _size);
    }

    public void CanMove(bool canMove)
    {
        _doesMove = canMove;
    }

    public void SetPlayerTarget(Vector3 playerTarget)
    {
        PlayerTarget = playerTarget;
        StopCoroutine(DefineNewPosition());
        Abducting = true;
    }

    public void RestartMovement()
    {
        Debug.Log("Zaustavljam Korutinu");
        StopCoroutine(DefineNewPosition());
        Abducting = false;
        Debug.Log("Pozivam Korutinu");
        StartCoroutine(DefineNewPosition());
    }
}