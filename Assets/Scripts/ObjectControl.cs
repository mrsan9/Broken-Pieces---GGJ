using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{

    public List<GameObject> brokenObjs = new List<GameObject>();
    public List<GameObject> placmeents = new List<GameObject>();

    [HideInInspector]public List<float> startZPos = new List<float>();

    [HideInInspector]public  List<GameObject> oldPositions = new List<GameObject>();
    
    // Update is called once per frame
    RaycastHit hit;
    Ray ray;

    private void Awake()
    {
        StartCoroutine(SetPlacements(0.1f));

        //for (int i = 0; i < placmeents.Count; ++i)
        //{
        //    brokenObjs[i].transform.DOMove(placmeents[i].transform.position, 1);
        //}
    }

    public IEnumerator SetPlacements(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject obj in brokenObjs)
        {
            startZPos.Add(obj.transform.localPosition.z);
            GameObject placer = new GameObject();
            placer.transform.position = obj.transform.position;
            Debug.LogWarning("Position " + placer.transform.position);
            placer.transform.rotation = obj.transform.rotation;
            oldPositions.Add(placer);
            placer.transform.parent = transform;

        }



        foreach (GameObject GO in placmeents)
        {
            GO.transform.parent = null;
        }
        PlayerController.instance.silhouetteGenerator.GenerateSilhouetteObjects();
    }

    private void OnDestroy()
    {
        foreach (GameObject GO in placmeents)
        {
            Destroy(GO);
        }
    }

    private void Start()
    {
        //GameManager.instance.GetCurrentGameObject(false);
    }

    //void Update()
    //{
    //    //float step = 5f* Time.deltaTime;
    //    if (Input.GetMouseButton(0))
    //    {
    //        Vector3 mPos1 = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);
    //        //Debug.Log(mPos1);
    //        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        if (Physics.Raycast(ray, out hit, 10000.0f))
    //        {
    //            for (int i = 0; i < brokenObjs.Count; ++i)
    //            {
    //                if (hit.transform.gameObject == brokenObjs[i])
    //                {

    //                    //float mPosX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x ;
    //                    //float mPosY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y ;
    //                    //float objZ = oldPositions[i].transform.position.z;

    //                    //Vector3 mPos = new Vector3();

    //                    //mPos.x = mPosX * Mathf.Cos(0f * Mathf.Deg2Rad) - mPosY * Mathf.Sin(0f * Mathf.Deg2Rad);
    //                    //mPos.y = mPosX * Mathf.Sin(0f*Mathf.Deg2Rad)+ mPosY * Mathf.Cos(0f * Mathf.Deg2Rad);

    //                    // mPos.z = objZ;

    //                    //hit.transform.position = mPos;

    //                }
    //            }
    //        }
    //    }
    //}


    //IEnumerator Move(Transform onj,Vector3 StartPos,Vector3 TargetPos)
    //{
    //     Vector3 StartPosition  = StartPos;
    //     Vector3 EndPosition = TargetPos;

    //    float t = 0.0f;

    //    while (t < 1.0f)
    //    {
    //        t += Time.deltaTime * 5f;
    //        onj.position = Vector3.Lerp(StartPosition, EndPosition, t);
    //        yield return null;
    //    }
    //    yield break;
    //}
}
