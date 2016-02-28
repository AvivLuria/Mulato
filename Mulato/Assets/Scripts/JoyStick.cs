using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : SceneSingleton<JoyStick>, IDragHandler, IPointerUpHandler, IPointerDownHandler {

	private Image bgImag;
	private Image joystickImg;
	public Vector3 inputVector;

	private void Start () {

		bgImag = GetComponent<Image> ();
		joystickImg = transform.GetChild (0).GetComponent<Image> ();

	}


	public virtual void OnDrag(PointerEventData ped) {
		
		Vector2 pos;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (bgImag.rectTransform
			, ped.position
			, ped.pressEventCamera
			, out pos)) 
		{
			pos.x = (pos.x / bgImag.rectTransform.sizeDelta.x);
			pos.y = (pos.y / bgImag.rectTransform.sizeDelta.y);

			inputVector = new Vector3 (pos.x * 2, 0, pos.y * 2);
			inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

			// MoveJoystick image
			joystickImg.rectTransform.anchoredPosition =
				new Vector3 (inputVector.x * (bgImag.rectTransform.sizeDelta.x / 3),
					inputVector.z * (bgImag.rectTransform.sizeDelta.y / 3));
		}
	}

	public virtual void OnPointerDown(PointerEventData ped) {
		OnDrag (ped);
	}

	public virtual void OnPointerUp(PointerEventData ped) {
		inputVector = Vector3.zero;
		joystickImg.rectTransform.anchoredPosition = Vector3.zero;
	}

}
