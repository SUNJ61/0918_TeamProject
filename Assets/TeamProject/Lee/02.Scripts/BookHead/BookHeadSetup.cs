using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookHeadSetup : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> BookHead_RBody;
    private Animator BookHead_animator;
    void Awake()
    {
        BookHead_animator = GetComponent<Animator>();
        var rb = transform.GetComponentsInChildren<Rigidbody>();
        if (BookHead_RBody != null)
        {
            BookHead_RBody = new List<Rigidbody>(rb);
        }
        SetRegDoll(false);

        StartCoroutine(TestRegDoll());
    }

    private void SetRegDoll(bool isEnable)
    {
        foreach (Rigidbody rbody in BookHead_RBody)
        {
            rbody.isKinematic = !isEnable;
        }
    }

    IEnumerator TestRegDoll() //�ӽ� �ڵ� 10���� �ִϸ��̼��� ��Ȱ��ȭ �Ͽ� ������ �Ǵ��� üũ
    {
        yield return new WaitForSeconds(10.0f);
        BookHead_animator.enabled = false;
        //SetRegDoll(true);
    }
}
