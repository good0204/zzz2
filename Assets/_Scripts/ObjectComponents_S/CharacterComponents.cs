using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CharacterComponents : MonoBehaviour
{
    public Rigidbody bombrigidbody;
    [SerializeField] Rigidbody headrigidbody;
    [SerializeField] Animator objectAnimator;
    [SerializeField] ObjectAnimation objectAnimation;

    [SerializeField] bool IsTarget;
    [Header("관통")]
    [SerializeField] bool Penetrate;
    [Space]
    [Header("자전값들")]
    [SerializeField] bool IsRotation;
    [SerializeField] float RotationSpeed;
    [SerializeField] int RotationLeft;
    [Space]
    [Header("공전값들")]
    [SerializeField] bool IsRevolution;
    [SerializeField] int RevolutionLeft;
    [SerializeField] float radius;
    [SerializeField] float RevolutioSpeed;
    [Space]
    [Header("패트롤값들")]
    [SerializeField] bool IsPatrol;
    [SerializeField] LoopType loopType;
    [SerializeField] Vector3[] wayPoints;
    [SerializeField] float[] movingTime;
    [SerializeField] float[] Interval;
    PathType pathType = PathType.Linear;
    GameObject RevolutionPoint;
    Sequence PatrolSequence;
    bool IsWall;
    private void Start()
    {
        if (GetComponent<TargetGimmick>() != null || GetComponent<BonusObject>() != null)
        {
            IsTarget = true;
        }
        if (GetComponent<WallComponent>() != null)
        {
            IsWall = true;
        }

        if (IsRevolution)
        {
            if (RevolutionLeft == 1)
            {
                transform.DOLocalRotate(new Vector3(0, 180, 0), 0);
            }
            PlayRevolution();
        }
        if (IsRotation)
        {
            // RotationSequence = DOTween.Sequence();
            // RotationSequence.SetAutoKill(false);
            // PlayRotation();
        }
        if (IsPatrol)
        {
            PatrolSequence = DOTween.Sequence();
            PatrolSequence.SetAutoKill(false);
            PlayPatrol();
        }
    }
    private void Update()
    {
        if (IsRevolution)
        {
            RevolutionPoint.transform.RotateAround(RevolutionPoint.transform.position, Vector3.up * RevolutionLeft, RevolutioSpeed * Time.deltaTime);

        }
        if (IsRotation)
        {
            transform.Rotate(Vector3.up * RotationLeft * Time.deltaTime * RotationSpeed);
        }
    }
    void PlayRevolution()
    {
        RevolutionPoint = new GameObject("RevolutionPoint");
        RevolutionPoint.transform.SetParent(transform.parent);
        RevolutionPoint.transform.localPosition = this.transform.localPosition;
        RevolutionPoint.transform.localEulerAngles = transform.localEulerAngles;
        transform.SetParent(RevolutionPoint.transform);
        transform.localPosition = new Vector3(radius, 0, 0);
    }
    void PlayPatrol()
    {
        if (!IsRevolution)
        {
            //wayPoints[0] = transform.localPosition;
            Vector3 lookPos = transform.position - transform.localPosition;
            for (int i = 1; i < wayPoints.Length; i++)
            {
                wayPoints[i].y = transform.position.y;
            }
            for (int i = 0; i < wayPoints.Length; i++)
            {
                PatrolSequence.Append(transform.DOLookAt(wayPoints[i] + lookPos, 0));
                if (Interval[i] != 0)
                {
                    PatrolSequence.AppendCallback(objectAnimation.Pause);
                    PatrolSequence.AppendInterval(Interval[i]);
                    PatrolSequence.AppendCallback(objectAnimation.ReturnState);
                }
                PatrolSequence.Append(transform.DOLookAt(wayPoints[i] + lookPos, 0));
                PatrolSequence.Append(transform.DOLocalMove(wayPoints[i], movingTime[i]));
            }
        }
        else
        {
            for (int i = 1; i < wayPoints.Length; i++)
            {
                wayPoints[i].y = transform.parent.position.y;
            }
            for (int i = 0; i < wayPoints.Length; i++)
            {
                PatrolSequence.Append(transform.parent.DOLookAt(wayPoints[i], 0));
                //PatrolSequence.AppendCallback(objectAnimation.Pause);
                PatrolSequence.AppendInterval(Interval[i]);
                PatrolSequence.AppendCallback(objectAnimation.ReturnState);
                PatrolSequence.Append(transform.parent.DOLookAt(wayPoints[i], 0));
                PatrolSequence.Append(transform.parent.DOLocalMove(wayPoints[i], movingTime[i]));
            }
        }

        PatrolSequence.SetLoops(-1, loopType);

    }
    public void BombAction(float explosionForce, Vector3 position, float explosionRadius, float upwardsModifier)
    {
        if (!IsTarget)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            objectAnimator.enabled = false;
            bombrigidbody.AddExplosionForce(explosionForce, position, explosionRadius, upwardsModifier, ForceMode.Impulse);
        }
        AllStopAnimation();
    }
    void AllStopAnimation()
    {
        IsRotation = false;
        IsRevolution = false;
        PatrolSequence.Kill();
        if (!IsTarget)
            CoinManager.Instance.StageExternalCoinUp(10);

        CountBonsObject.Instance.CountUp();
    }
    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.tag == "Bullet")
        // {
        //     Bullet bullet = other.gameObject.GetComponent<Bullet>();
        //     bullet.TweenKill();
        //     Vector3 incomingVec = other.transform.localPosition - bullet.StartPosition;
        //     Vector3 normalVec = other.contacts[0].normal;
        //     Vector3 reflctVec = Vector3.Reflect(incomingVec, normalVec);

        //     bullet.StartPosition = other.transform.localPosition;
        //     bullet.Reflection(reflctVec.normalized);

        // }
        if (other.gameObject.tag == "Bullet")
        {
            if (IsWall)
                return;

            objectAnimator.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            AllStopAnimation();


            if (!IsTarget)
            {
                SoundManager.Instance.Play("gruntLow");

                EffectManager.Instance.EffectPlay("CharacterHit", transform.position + new Vector3(0, 1, 0));
            }
        }
    }
}

