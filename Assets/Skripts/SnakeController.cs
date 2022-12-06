using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
    public List<Transform> Tails;

    [Range(0f, 3f)]
    public float BonesDistance;
    public GameObject BonePrafab;
    private Transform _transform;
    [Range(0f, 4f)]
    public float Speed;

    public UnityEvent OnEat;
    public UnityEvent OnBlock;

    int count = 0;
    public Text countText;


    private void Start()
    {
        _transform = GetComponent<Transform>();

        count = 0;
        countText.text = "Score  " + count.ToString();
    }

    private void Update()
    {
        MoveSnake(_transform.position + transform.forward * Speed);

        float angel = Input.GetAxis("Horizontal") *4;
        _transform.Rotate(0, angel, 0);
    }
    private void MoveSnake(Vector3 newPosition)
    {
        float sqrDistance = BonesDistance * BonesDistance;
        Vector3 previosPosition = _transform.position ;

        foreach(var bone in Tails)
        {
         if ((bone.position - previosPosition).sqrMagnitude > sqrDistance)
            {
                var temp = bone.position;
                bone.position = previosPosition;
                previosPosition = temp;
     }
         else
            {
                break;
            }
        }
        _transform.position = newPosition;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Food")
        {
            Destroy(collision.gameObject);

            var bone = Instantiate(BonePrafab);
            Tails.Add(bone.transform);

            count++;
            countText.text = "Score" + count.ToString();


            if (OnEat != null)
            {
                OnEat.Invoke();
            }


            if (count++ > 12)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); ;
            }
        }

        if (collision.gameObject.tag == "Block")
        {

            if (OnBlock != null)
            {
                OnBlock.Invoke();
            }
        }
    }

}
