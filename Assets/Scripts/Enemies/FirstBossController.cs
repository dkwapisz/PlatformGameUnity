using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossController : Enemy
{
    [SerializeField] GameObject ownPrefab;
    public bool isChild = false;
    int childrenHealthPoints;

    protected override void Start()
    {
        base.Start();
        childrenHealthPoints = healthPoints/2;
    }
    protected override void destroyObject()
    {
        gameObject.SetActive(false);
        Vector2 parentPosition = transform.position;
        
        if (!isChild) {
            createChildren(parentPosition);
        }
        base.destroyObject();
    }

    void createChildren(Vector2 parentPosition) {
        GameObject child1 = Instantiate(ownPrefab, parentPosition, Quaternion.identity);
        GameObject child2;
        child1.transform.localScale = child1.transform.localScale/2;
        child1.GetComponent<FirstBossController>().healthPoints = childrenHealthPoints;
        child1.GetComponent<FirstBossController>().isChild = true;
        child1.SetActive(true);
        child2 = Instantiate(child1, parentPosition, Quaternion.identity);
        child1.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector2(3, 5));
        child2.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector2(-3, 5));
    }
}
