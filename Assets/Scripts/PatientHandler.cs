using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PatientHandler : MonoBehaviour
{

    public Transform objectsSpawnPlaceHolder;
    public Patient patient;

    public List<GameObject> objectParts;

    public float xOffset = 2f;
    public float yOffset = 4f;

    public void CreateLevelParts()
    {
        StartCoroutine(CreateLevelPartsCoroutine());
    }
    public IEnumerator CreateLevelPartsCoroutine()
    {
        yield return new WaitForSeconds(2f);

        PlayerController.instance.animator.Play("Spell");
        //yield return new WaitForSeconds(1f);
        
        if (GameManager.instance.levelObjects != null)
        {
            ObjectControl temp = GameManager.instance.levelObjects[GameManager.instance.levelNo - 1].GetComponent<ObjectControl>();
            if (temp != null)
            {
                Vector3 tempScale = Vector3.one;
                for (int i = 0; i < temp.brokenObjs.Count; i++)
                {
                    tempScale = Vector3.one;
                    GameObject part = Instantiate(temp.brokenObjs[i]);
                    switch (temp.brokenObjs[i].transform.parent.tag)
                    {

                       case "shattered_book":
                            tempScale *=0.008f;
                            break;
                        case "shattered_parrot":
                            tempScale *= 0.005f;
                            break;
                        case "shattered_watch":
                            tempScale *= 0.008f;
                            break;
                        case "shattered_perfume":
                            tempScale *= 0.005f;
                            break;

                    }
                    part.transform.localScale = tempScale ;
                    float x = Random.Range(objectsSpawnPlaceHolder.position.x - xOffset, objectsSpawnPlaceHolder.position.x + xOffset);
                    float y = Random.Range(objectsSpawnPlaceHolder.position.y, objectsSpawnPlaceHolder.position.y + yOffset);
                    part.transform.position = transform.position;
                    Vector3 tempPos = part.transform.position;
                    
                    tempPos.z += 0.3f;
                    part.transform.position = tempPos;
                    part.transform.DOMove(new Vector3(x, y, part.transform.position.z - 1.5f), 0.3f);
                    objectParts.Add(part);
                }
            }
            else
            {
                Debug.LogWarning("PARTS NOT FOUND");
            }
        }
        yield return new WaitForSeconds(1.5f);

        for(int i = 0;i<objectParts.Count;i++)
        {
            objectParts[i].transform.DOMove(GameManager.instance.platformObject.transform.position, 0.5f);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);
        DestroyObjects();
        GameManager.instance.OnDialoguesCompleted();
    }

    public void DestroyObjects()
    {
        if(objectParts.Count > 0)
        {
            
            for( int i=0;i<objectParts.Count;i++)
            {
                GameObject temp = objectParts[i];
                Destroy(temp);
            }
            objectParts.Clear();
        }
    }
}
