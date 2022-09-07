using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Boss : Enemy
{
    [Header("Boss Settings")]
    [SerializeField] Player player;
    [SerializeField] float _indicatorTime = 1f;
    [SerializeField] float _flashTime = 0.25f;
    [SerializeField] float _movePeriod = 4f;
    [SerializeField] float _jumpPower = 20f;
    
    [Header("Sprite Materials")]
    [SerializeField] Material m_pawn;
    [SerializeField] Material m_rook;
    [SerializeField] Material m_knight;
    [SerializeField] Material m_bishop;
    [SerializeField] Material m_queen;
    [SerializeField] Material m_king;

    protected Rigidbody _rb;
    protected GameObject _indicator;
    protected Light _light;

    public Vector3 _target;
    string moveType;
    float _moveTimer = 0f;
    bool _needsTarget = false;
    bool _inMove = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _indicator = transform.GetChild(0).gameObject;
        _light = transform.GetChild(1).GetComponent<Light>();
        _target = transform.position;

        Random.InitState(System.DateTime.Now.Millisecond);
    }

    void Start()
    {
        _indicator.SetActive(false);
        _light.gameObject.SetActive(false);
    }

    void Update()
    {
        _moveTimer += Time.deltaTime;
        if (_moveTimer >= _movePeriod)
        {
            ChooseMove();
            _moveTimer = 0f;
            _inMove = false;
            _needsTarget = true;
        }
    }

    void ChooseMove()
    {
        if (Input.GetKey(KeyCode.P))
        {
            StartCoroutine(SetSprite(m_pawn));
        }
        else if (Input.GetKey(KeyCode.R))
        {
            moveType = "ROOK";
        }
        else if (Input.GetKey(KeyCode.H))
        {
            moveType = "KNIGHT";
        }
        else if (Input.GetKey(KeyCode.B))
        {
            moveType = "BISHOP";
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            StartCoroutine(SetSprite(m_queen));
        }
        else if (Input.GetKey(KeyCode.K))
        {
            moveType = "KING";
        }
    }

    protected override void Move()
    {
        _rb.AddForce(Physics.gravity * 0.5f);
        
        if (_needsTarget)
        {
            FindTarget();
        }
        if (_inMove && (moveType == "ROOK" || moveType == "KNIGHT" || moveType == "BISHOP"))
        {
            Vector3 moveOffset = _target - transform.position;
            _rb.MovePosition(transform.position + (moveOffset * Time.deltaTime * _moveSpeed / Vector3.Distance(transform.position, _target)));
            if (Mathf.Round(transform.position.x) == _target.x && Mathf.Round(transform.position.z) == _target.z)
            {
                _rb.MovePosition(_target);
                moveType = null;
            }
        }
    }

    protected void FindTarget()
    {
        switch (moveType)
        {
            case "ROOK":
                bool XisNegligible = Mathf.Abs(player.transform.position.x - transform.position.x) <= 1f;
                bool ZisNegligible = Mathf.Abs(player.transform.position.z - transform.position.z) <= 1f;
                if (player.transform.position.x >= transform.position.x && ZisNegligible)
                {
                    StartCoroutine(RookMove(new Vector3(7, 1.125f, Mathf.Round(transform.position.z))));
                }
                else if (player.transform.position.x <= transform.position.x && ZisNegligible)
                {
                    StartCoroutine(RookMove(new Vector3(-7, 1.125f, Mathf.Round(transform.position.z))));
                }
                else if (player.transform.position.z >= transform.position.z && XisNegligible)
                {
                    StartCoroutine(RookMove(new Vector3(Mathf.Round(transform.position.x), 1.125f, 7)));
                }
                else if (player.transform.position.z <= transform.position.z && XisNegligible)
                {
                    StartCoroutine(RookMove(new Vector3(Mathf.Round(transform.position.x), 1.125f, -7)));
                }
                break;
            case "KNIGHT":
                if (player.transform.position.x >= transform.position.x && player.transform.position.z >= transform.position.z)
                {
                    Vector3 target = new Vector3(Mathf.Round(transform.position.x) + 4, 1.125f, Mathf.Round(transform.position.z) + 4);
                    if (Mathf.Abs(player.transform.position.x - transform.position.x) > Mathf.Abs(player.transform.position.z - transform.position.z))
                    {
                        target -= new Vector3(0, 0, 2);
                    }
                    else
                    {
                        target -= new Vector3(2, 0, 0);
                    }
                    StartCoroutine(KnightMove(target));
                }
                else if (player.transform.position.x <= transform.position.x && player.transform.position.z >= transform.position.z)
                {
                    Vector3 target = new Vector3(Mathf.Round(transform.position.x) - 4, 1.125f, Mathf.Round(transform.position.z) + 4);
                    if (Mathf.Abs(player.transform.position.x - transform.position.x) > Mathf.Abs(player.transform.position.z - transform.position.z))
                    {
                        target -= new Vector3(0, 0, 2);
                    }
                    else
                    {
                        target += new Vector3(2, 0, 0);
                    }
                    StartCoroutine(KnightMove(target));
                }
                else if (player.transform.position.x <= transform.position.x && player.transform.position.z <= transform.position.z)
                {
                    Vector3 target = new Vector3(Mathf.Round(transform.position.x) - 4, 1.125f, Mathf.Round(transform.position.z) - 4);
                    if (Mathf.Abs(player.transform.position.x - transform.position.x) > Mathf.Abs(player.transform.position.z - transform.position.z))
                    {
                        target += new Vector3(0, 0, 2);
                    }
                    else
                    {
                        target += new Vector3(2, 0, 0);
                    }
                    StartCoroutine(KnightMove(target));
                }
                else if (player.transform.position.x >= transform.position.x && player.transform.position.z <= transform.position.z)
                {
                    Vector3 target = new Vector3(Mathf.Round(transform.position.x) + 4, 1.125f, Mathf.Round(transform.position.z) - 4);
                    if (Mathf.Abs(player.transform.position.x - transform.position.x) > Mathf.Abs(player.transform.position.z - transform.position.z))
                    {
                        target += new Vector3(0, 0, 2);
                    }
                    else
                    {
                        target -= new Vector3(2, 0, 0);
                    }
                    StartCoroutine(KnightMove(target));
                }
                break;
            case "BISHOP":
                if (player.transform.position.x >= transform.position.x && player.transform.position.z >= transform.position.z)
                {
                    Vector3 target = new Vector3(Mathf.Round(transform.position.x), 1.125f, Mathf.Round(transform.position.z));
                    while (target.x < 7 && target.z < 7)
                    {
                        target.x += 2;
                        target.z += 2;
                    }
                    StartCoroutine(BishopMove(target));
                }
                else if (player.transform.position.x <= transform.position.x && player.transform.position.z >= transform.position.z)
                {
                    Vector3 target = new Vector3(Mathf.Round(transform.position.x), 1.125f, Mathf.Round(transform.position.z));
                    while (target.x > -7 && target.z < 7)
                    {
                        target.x -= 2;
                        target.z += 2;
                    }
                    StartCoroutine(BishopMove(target));
                }
                else if (player.transform.position.x <= transform.position.x && player.transform.position.z <= transform.position.z)
                {
                    Vector3 target = new Vector3(Mathf.Round(transform.position.x), 1.125f, Mathf.Round(transform.position.z));
                    while (target.x > -7 && target.z > -7)
                    {
                        target.x -= 2;
                        target.z -= 2;
                    }
                    StartCoroutine(BishopMove(target));
                }
                else if (player.transform.position.x >= transform.position.x && player.transform.position.z <= transform.position.z)
                {
                    Vector3 target = new Vector3(Mathf.Round(transform.position.x), 1.125f, Mathf.Round(transform.position.z));
                    while (target.x < 7 && target.z > -7)
                    {
                        target.x += 2;
                        target.z -= 2;
                    }
                    StartCoroutine(BishopMove(target));
                }
                break;
            case "KING":
                Vector3 _dir1 = Vector3.zero;
                Vector3 _dir2 = Vector3.zero;
                
                // check which adjacent box player is in
                // first check cardinals
                if (Physics.OverlapBox(new Vector3(transform.position.x + 2, transform.position.y, transform.position.z), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0)
                {
                    _dir1 = Vector3.right;
                }
                else if (Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y, transform.position.z + 2), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0)
                {
                    _dir1 = Vector3.forward;
                }
                else if (Physics.OverlapBox(new Vector3(transform.position.x - 2, transform.position.y, transform.position.z), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0)
                {
                    _dir1 = Vector3.left;
                }
                else if (Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y, transform.position.z - 2), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0)
                {
                    _dir1 = Vector3.back;
                }
                // then check diagonals
                else if (Physics.OverlapBox(new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0)
                {
                    _dir1 = Vector3.forward;
                    _dir2 = Vector3.right;
                }
                else if (Physics.OverlapBox(new Vector3(transform.position.x - 2, transform.position.y, transform.position.z + 2), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0)
                {
                    _dir1 = Vector3.forward;
                    _dir2 = Vector3.left;
                }
                else if (Physics.OverlapBox(new Vector3(transform.position.x - 2, transform.position.y, transform.position.z - 2), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0)
                {
                    _dir1 = Vector3.back;
                    _dir2 = Vector3.left;
                }
                else if (Physics.OverlapBox(new Vector3(transform.position.x + 2, transform.position.y, transform.position.z - 2), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0)
                {
                    _dir1 = Vector3.back;
                    _dir2 = Vector3.right;
                }

                // now move
                // if only one dir has been set, only move on one axis
                if (_dir2 == Vector3.zero)
                {
                    StartCoroutine(KingMove(_dir1));
                }
                // if both have been set, move on both axes
                else
                {
                    StartCoroutine(KingMove(_dir1, _dir2));
                }
                break;
        }
        _needsTarget = false;
    }

    IEnumerator SetSprite(Material sprite)
    {
        _indicator.SetActive(true);
        _indicator.GetComponent<MeshRenderer>().material = sprite;
        yield return new WaitForSeconds(_indicatorTime);
        _indicator.SetActive(false);
    }

    IEnumerator FlashLight()
    {
        for (int i = 0; i < 5; i++)
        {
            _light.gameObject.SetActive(!_light.gameObject.activeSelf);
            yield return new WaitForSeconds(_flashTime);
        }
        _light.gameObject.SetActive(false);
    }

    IEnumerator RookMove(Vector3 target)
    {
        StartCoroutine(SetSprite(m_rook));
        yield return StartCoroutine(FlashLight());

        _inMove = true;
        _target = target;
    }

    IEnumerator KnightMove(Vector3 target)
    {
        StartCoroutine(SetSprite(m_knight));
        yield return StartCoroutine(FlashLight());

        _rb.AddForce(new Vector3(0, _jumpPower, 0));
        
        _inMove = true;
        _target = target;
    }

    IEnumerator BishopMove(Vector3 target)
    {
        StartCoroutine(SetSprite(m_bishop));
        yield return StartCoroutine(FlashLight());

        _inMove = true;
        _target = target;
    }
    
    IEnumerator KingMove(Vector3 dir)
    {
        StartCoroutine(SetSprite(m_king));
        yield return StartCoroutine(FlashLight());

        _rb.AddForceAtPosition(dir * 150, new Vector3(transform.position.x, transform.position.y + 0.98f, transform.position.z));
        moveType = null;
    }

    IEnumerator KingMove(Vector3 dir1, Vector3 dir2)
    {
        StartCoroutine(SetSprite(m_king));
        yield return StartCoroutine(FlashLight());

        _rb.AddForceAtPosition(dir1 * 150, new Vector3(transform.position.x, transform.position.y + 0.98f, transform.position.z));

        yield return new WaitForSeconds(1.8f);

        _rb.AddForceAtPosition(dir2 * 150, new Vector3(transform.position.x, transform.position.y + 0.98f, transform.position.z));
        moveType = null;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Game Plane"))
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.MovePosition(new Vector3(Mathf.Round(transform.position.x), 1.125f, Mathf.Round(transform.position.z)));
        }
    }
}
