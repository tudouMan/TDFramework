using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
namespace TDFramework
{
	/// <summary>
	/// Input检测 注册对应事件即可 
	/// Update监听 鼠标事件MouseInput 手机端TouchInput
	/// 初始化请注册相机
	/// </summary>
	public class InputHelper 
	{
		public TouchOperation OnTouchDown;
		public TouchOperation OnTouchMove;
		public TouchOperation OnTouchUp;
		public TouchOperation OnClick;
		public long TouchDownTime;
		public long TouchUpTime;
		[HideInInspector]
		public float Pressure = 1;

		private bool m_FixMouseBegan = false;
		private Camera m_InputCamera;

		public InputHelper(Camera camera)
        {
			if (camera == null)
				GameEntry.Debug.LogError("input camera set is null,please register!");
			m_InputCamera = camera;
		}
		 

		public void MouseInput()
		{
			UnityEngine.EventSystems.EventSystem eventSystem = UnityEngine.EventSystems.EventSystem.current;
			if (eventSystem != null && eventSystem.IsPointerOverGameObject())
			{
				if(!m_FixMouseBegan) return;
			}
			if (Input.GetMouseButton(0) && m_FixMouseBegan)
			{
				Vector3 mousePositon = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
				OnUserTouchMove(Input.mousePosition);
			}
			if (Input.GetMouseButtonDown(0))
			{
				Vector3 mousePositon = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
				OnUserTouchDown(Input.mousePosition);
				m_FixMouseBegan = true;
			}
			if (Input.GetMouseButtonUp(0) && m_FixMouseBegan)
			{
				Vector3 mousePositon = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
				OnUserTouchUp(Input.mousePosition);
				m_FixMouseBegan = false;
			}
		}

		private TouchPhase preTouchPhase;
		private bool fixTouchBegan = false;
		public void TouchInput()
		{
			if (Input.touchCount > 0)
			{
				Pressure = Input.GetTouch(0).pressure;
				bool isClickOverUI;
				isClickOverUI =UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
				if (isClickOverUI)
				{
					if(!fixTouchBegan) return;
				}
				Touch firstTouch = Input.GetTouch(0);
				if (firstTouch.phase == TouchPhase.Began)
				{
					fixTouchBegan = true;
					Vector3 touchPositon = Camera.main.ScreenToWorldPoint(new Vector3(firstTouch.position.x, firstTouch.position.y, -Camera.main.transform.position.z));
					OnUserTouchDown(firstTouch.position);
				}
				if (firstTouch.phase == TouchPhase.Moved)
				{
					Vector3 touchPositon = Camera.main.ScreenToWorldPoint(new Vector3(firstTouch.position.x, firstTouch.position.y, -Camera.main.transform.position.z));
					OnUserTouchMove(firstTouch.position);
				}
				if (firstTouch.phase == TouchPhase.Ended && preTouchPhase != TouchPhase.Ended)
				{
					Vector3 touchPositon = Camera.main.ScreenToWorldPoint(new Vector3(firstTouch.position.x, firstTouch.position.y, -Camera.main.transform.position.z));
					OnUserTouchUp(firstTouch.position);
					fixTouchBegan = false;
				}
				if (firstTouch.phase == TouchPhase.Stationary && preTouchPhase != TouchPhase.Ended)
				{
					Vector3 touchPositon = Camera.main.ScreenToWorldPoint(new Vector3(firstTouch.position.x, firstTouch.position.y, -Camera.main.transform.position.z));
					OnUserTouchMove(firstTouch.position);
				}
				preTouchPhase = firstTouch.phase;
			}
		}

		private void OnUserTouchDown(Vector3 position)
		{
			TouchDownTime = GetTimestamp();
			if(OnTouchDown != null){
				OnTouchDown(position);
			}
		}

		private void OnUserTouchMove(Vector3 position){
			if(OnTouchMove != null){
				OnTouchMove(position);
			}
		}

		private void OnUserTouchUp(Vector3 position){
			TouchUpTime = GetTimestamp();
			if(OnTouchUp != null){
				OnTouchUp(position);
			}
			if(TouchUpTime - TouchDownTime <= 100){
				OnUserClick(position);
			}
		}

		private void OnUserClick(Vector3 position){
			if(OnClick != null){
				OnClick(position);
			}
		}

		public static long GetTimestamp(){
			long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
			return unixTimestamp;
		}

		public delegate void TouchOperation(Vector3 position);

	}
}