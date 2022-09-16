using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Boss : Enemy
{
    [Header("Boss Settings")]
    [SerializeField] Player player;
    [SerializeField] Pawn pawnPrefab;
    [SerializeField] ParticleSystem _jumpParticles;
    [SerializeField] float _indicatorTime = 1f;
    [SerializeField] float _flashTime = 0.25f;
    [SerializeField] float _movePeriod = 4f;
    [SerializeField] float _jumpPower = 20f;
    [SerializeField] float _torquePower = 300f;

    [Header("Damage Settings")]
    [SerializeField] int _laserDamage;
    [SerializeField] Laser laserPrefab;
    [SerializeField] int _quakeDamage;
    [SerializeField] Quake quakePrefab;
    
    [Header("Sprite Materials")]
    [SerializeField] Material m_pawn;
    [SerializeField] Material m_rook;
    [SerializeField] Material m_knight;
    [SerializeField] Material m_bishop;
    [SerializeField] Material m_queen;
    [SerializeField] Material m_king;

    protected Light _light;
    protected Rigidbody _rb;
    protected BossMoveIndicator _indicator;

    BossHealth _health;
    Health _playerHealth;
    Eye _eye;
    Vector3 _target;
    string moveType;
    float _moveTimer = 0f;
    bool _needsTarget = false;
    bool _inMove = false;
    bool _needsQuake = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _indicator = transform.GetChild(0).GetComponent<BossMoveIndicator>();
        _light = transform.GetChild(1).GetComponent<Light>();
        _health = GetComponent<BossHealth>();
        _playerHealth = player.GetComponent<Health>();
        _eye = transform.GetChild(2).GetComponent<Eye>();
        _eye.SetColorNormal();
        _target = transform.position;

        Random.InitState(System.DateTime.Now.Millisecond);
    }

    void Start()
    {
        _indicator.gameObject.SetActive(false);
        _light.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!_health.isDying)
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
    }

    void ChooseMove()
    {
        // check cardinals
        Vector3 row = new Vector3(transform.localScale.x * 8, transform.localScale.y * 0.5f, transform.localScale.z * 0.25f);
        Vector3 column = new Vector3(transform.localScale.x * 0.25f, transform.localScale.y * 0.5f, transform.localScale.z * 8);
        if (Physics.OverlapBox(transform.position, row, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0 || Physics.OverlapBox(transform.position, column, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= 3)
            {
                if(Random.Range(0, 2) == 0)
                {
                    moveType = "KING";
                }
                else
                {
                    moveType = "ROOK";
                }
            }
            else
            {
                if(Random.Range(0, 2) == 0)
                {
                    moveType = "QUEEN";
                }
                else
                {
                    moveType = "ROOK";
                }
            }
        }
        // check diagonals
        else if (Physics.OverlapBox(transform.position, row, Quaternion.identity * Quaternion.Euler(0, 45, 0), LayerMask.GetMask("Player")).Length > 0 || Physics.OverlapBox(transform.position, column, Quaternion.identity * Quaternion.Euler(0, 45, 0), LayerMask.GetMask("Player")).Length > 0)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= 3)
            {
                if(Random.Range(0, 2) == 0)
                {
                    moveType = "KING";
                }
                else
                {
                    moveType = "BISHOP";
                }
            }
            else
            {
                if(Random.Range(0, 2) == 0)
                {
                    moveType = "QUEEN";
                }
                else
                {
                    moveType = "BISHOP";
                }
            }
        }
        // check knight spaces
        else if (Physics.OverlapBox(transform.position + new Vector3(4, 0, 2), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0 ||
                 Physics.OverlapBox(transform.position + new Vector3(2, 0, 4), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0 ||
                 Physics.OverlapBox(transform.position + new Vector3(4, 0, -2), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0 ||
                 Physics.OverlapBox(transform.position + new Vector3(2, 0, -4), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0 ||
                 Physics.OverlapBox(transform.position + new Vector3(-2, 0, 4), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0 ||
                 Physics.OverlapBox(transform.position + new Vector3(-4, 0, 2), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0 ||
                 Physics.OverlapBox(transform.position + new Vector3(-2, 0, -4), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0 ||
                 Physics.OverlapBox(transform.position + new Vector3(-4, 0, -2), transform.localScale / 2, Quaternion.identity, LayerMask.GetMask("Player")).Length > 0)
        {
            moveType = "KNIGHT";
        }
        else
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    moveType = "BISHOP";
                    break;
                case 1:
                    moveType = "ROOK";
                    break;
                case 2:
                    moveType = "PAWN";
                    break;
                case 3:
                    moveType = "PAWN";
                    break;
            }
        }
    }

    protected override void Move()
    {
        if (!_health.isDying)
        {
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
            if (moveType == "KNIGHT")
            {
                _rb.AddForce(Physics.gravity * -0.75f);
            }
        }
    }

    protected void FindTarget()
    {
        switch (moveType)
        {
            case "PAWN":
                int column = (Random.Range(-4, 4) * 2) + 1;
                int row;
                do
                {
                    row = (Random.Range(-4, 4) * 2) + 1;
                } while (Mathf.Abs(row - transform.position.z) < 1 || (Mathf.Abs(row - player.transform.position.z) < 1));
                StartCoroutine(SpawnPawn(new Vector3(column, 1, row)));
                break;
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
            case "QUEEN":
                StartCoroutine(QueenFire());
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

    IEnumerator FlashLight()
    {
        for (int i = 0; i < 5; i++)
        {
            _light.gameObject.SetActive(!_light.gameObject.activeSelf);
            if (_light.gameObject.activeSelf)
            {
                _eye.SetColorAlert();
            }
            else
            {
                _eye.SetColorNormal();
            }
            yield return new WaitForSeconds(_flashTime);
        }
        _light.gameObject.SetActive(false);
        _eye.SetColorNormal();
    }

    IEnumerator SpawnPawn(Vector3 position)
    {
        StartCoroutine(_indicator.SetSprite(m_pawn, _indicatorTime));
        yield return StartCoroutine(FlashLight());

        _rb.AddForce(new Vector3(0, _jumpPower * 0.35f, 0));
        _needsQuake = true;

        // find valid space
        Pawn pawn = Instantiate(pawnPrefab, position, Quaternion.identity);
        pawn.player = player;

        moveType = null;
    }
    
    IEnumerator RookMove(Vector3 target)
    {
        StartCoroutine(_indicator.SetSprite(m_rook, _indicatorTime));
        yield return StartCoroutine(FlashLight());

        _inMove = true;
        _target = target;
    }

    IEnumerator KnightMove(Vector3 target)
    {
        StartCoroutine(_indicator.SetSprite(m_knight, _indicatorTime));
        yield return StartCoroutine(FlashLight());

        _rb.AddForce(new Vector3(0, _jumpPower, 0));
        
        _inMove = true;
        _target = target;
    }

    IEnumerator BishopMove(Vector3 target)
    {
        StartCoroutine(_indicator.SetSprite(m_bishop, _indicatorTime));
        yield return StartCoroutine(FlashLight());

        _inMove = true;
        _target = target;
    }

    IEnumerator QueenFire()
    {
        StartCoroutine(_indicator.SetSprite(m_queen, _indicatorTime));
        yield return StartCoroutine(FlashLight());

        for (int i = -1; i < 3; i++)
        {
            Laser laser = Instantiate(laserPrefab, transform.position, Quaternion.Euler(new Vector3(0, 45 * i, 0)));
            laser.Damage = _laserDamage;
        }

        moveType = null;
    }
    
    IEnumerator KingMove(Vector3 dir)
    {
        StartCoroutine(_indicator.SetSprite(m_king, _indicatorTime));
        yield return StartCoroutine(FlashLight());

        _rb.AddForceAtPosition(dir * _torquePower, new Vector3(transform.position.x, transform.position.y + 0.98f, transform.position.z));
        moveType = null;
    }

    IEnumerator KingMove(Vector3 dir1, Vector3 dir2)
    {
        StartCoroutine(_indicator.SetSprite(m_king, _indicatorTime));
        yield return StartCoroutine(FlashLight());

        _rb.AddForceAtPosition(dir1 * _torquePower, new Vector3(transform.position.x, transform.position.y + 0.98f, transform.position.z));

        yield return new WaitForSeconds(1.5f);

        _rb.AddForceAtPosition(dir2 * _torquePower, new Vector3(transform.position.x, transform.position.y + 0.98f, transform.position.z));
        moveType = null;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Game Plane")
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            _rb.MovePosition(new Vector3(Mathf.Round(transform.position.x), 1.125f, Mathf.Round(transform.position.z)));
            Instantiate(_jumpParticles, transform.position, Quaternion.identity);

            if (_needsQuake)
            {
                Quake quake = Instantiate(quakePrefab, transform.position, Quaternion.identity);
                quake.Damage = _quakeDamage;
                quake.Target = player;
                _needsQuake = false;
            }
        }
        base.OnCollisionEnter(collision);
    }
}
