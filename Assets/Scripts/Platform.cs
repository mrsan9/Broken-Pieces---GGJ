using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Platform : MonoBehaviour
{
    public Transform p1, p2, p3;

    private void Start()
    {
        Vector3 p1Pos = p1.position + Vector3.down * 0.5f;
        Vector3 p2Pos = p2.position + Vector3.down * 0.5f;
        Vector3 p3Pos = p3.position + Vector3.down * 0.5f;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(p1.DOMove(p1Pos, 0.5f)).Append(p2.DOMove(p2Pos, 0.5f)).Append(p3.DOMove(p3Pos, 0.5f)).SetLoops(-1,LoopType.Yoyo);

        StartCoroutine(RotatePlatforms(20f * Time.deltaTime));
    }

    IEnumerator RotatePlatforms(float incrementor)
    {
        while(true)
        {
            Vector3 rotator = new Vector3(0, p1.rotation.eulerAngles.y + incrementor, 0);

            p1.rotation = Quaternion.Euler(rotator);
            p2.rotation = Quaternion.Euler(rotator);
            p3.rotation = Quaternion.Euler(rotator);

            yield return null;
        }
    }
}
