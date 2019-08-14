using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SwipeHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public const float Threshold = 600;

    public float LeftAmount = 0, RightAmount = 0;

    public Vector2 startPosCard;
    public Text Question, TextRight, TextLeft;

    public new RectTransform transform => base.transform as RectTransform;

    private void Start()
    {
        startPosCard = transform.position;

        //example text
        Question.text = "Waddup!";
        TextRight.text = "'sup!";
        TextLeft.text = "dafuq...";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData data)
    {
        SetDraggedPosition(data);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform, data.position, data.pressEventCamera, out var _))
        {
            //card moves from mouse movement, but not with mouse position
            Vector2 cardPos = transform.position;
            cardPos.x += data.delta.x;
            transform.position = cardPos;

            //answers only become visible if swiped partly
            var offset = cardPos.x - startPosCard.x;
            var alphaText = offset / 200;
            TextRight.color = new Color(0, 0, 0, alphaText);
            TextLeft.color = new Color(0, 0, 0, -alphaText);

            // card rotates when swiped
            var rotation = -offset / 30;
            transform.eulerAngles = new Vector3(0, 0, rotation);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var imageColor = transform.GetComponent<Image>();

        //swipe check through colour change
        if (transform.localPosition.x >= Threshold)
        {
            RightAmount++;
            imageColor.color = Color.yellow;
        }
        else if (transform.localPosition.x <= -Threshold)
        {
            LeftAmount++;
            imageColor.color = Color.green;
        }
        else
        {
            imageColor.color = Color.grey;
            Debug.Log("Did not choose, back to begin position");
        }

        //example texts
        if (LeftAmount == 0 && RightAmount == 1)
        {
            Question.text = "So how has your day been?";
            TextLeft.text = "It was horrible";
            TextRight.text = "Alright.";
        }
        else if (LeftAmount == 1 && RightAmount == 0)
        {
            Question.text = "That bad a day, huh?";
            TextLeft.text = "Who are you?";
            TextRight.text = "It was horrible";
        }
        else if (LeftAmount == 1 && RightAmount == 1)
        {
            Question.text = "Tell me";
            TextLeft.text = "Well, it's like this...";
            TextRight.text = "I'm not gonna tell you!";
        }
        else if (LeftAmount == 0 && RightAmount == 0)
        {
            Question.text = Question.text;
            TextRight.text = TextRight.text;
            TextLeft.text = TextLeft.text;
        }
        else
        {
            Question.text = "Something something";
            TextLeft.text = "ikr";
            TextRight.text = "I don't think so, no";
        }

        //text alpha reset
        TextRight.color = TextLeft.color = Color.clear;
        //card position and rotation reset
        transform.position = startPosCard;
        transform.eulerAngles = Vector3.zero;
    }
}