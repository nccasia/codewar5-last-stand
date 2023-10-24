using System.Collections.Generic;
using UnityEngine;
using Anthill.AI;
using Anthill.Utils;
using Game.Core;
using Game.Components;
using Game.Map;

namespace Game.AI.BotOne
{
	/// <summary>
	/// Данный класс-состояние является основной практически для всех остальных классов
	/// состояний бота, потому что практически все состояния бота основаны на перемещении
	/// пота по карте.
	///
	/// Таким образом данный класс реализует функцию поиска маршрута и функцию перемещения
	/// по построенному маршруту.
	/// </summary>
	public class StateMove : AntAIState
	{
		protected TankControl _control;
		protected AntAIBlackboard _blackboard;
		protected Magnet _magnet;
		protected List<Vector2> _way;
		protected int _wayIndex;
		protected bool _isWayFinished;

		protected float _targetAngle;
		protected float _updateAngleInterval;
		protected Vector2 _nextPoint;

		public StateMove(GameObject aObject, string aStateName) : base(aStateName)
		{
			_control = aObject.GetComponent<TankControl>();
			_blackboard = aObject.GetComponent<AntAIBlackboard>();
			_magnet = aObject.GetComponent<Magnet>();
		}

		public override void Start()
		{
			_wayIndex = 0;
			_isWayFinished = false;
		}

		public override void Stop()
		{
			// Сброс управления.
			_control.isForward = false;
			_control.isBackward = false;
			_control.isLeft = false;
			_control.isRight = false;
		}

		protected void BuildWay(WayPoint aCurrent, WayPoint aTarget)
		{
			_way = WayMap.Current.FindWay(aCurrent, aTarget);
			NextPoint();
		}

		protected bool OnMove()
		{
			if (Config.Instance.showCurrentWay)
			{
				DrawWay();
			}
			
			// Достигли текущей точки!
			if (AntMath.Distance((Vector2)_control.Position, _nextPoint) < WayMap.Current.approachRadius)
			{
				NextPoint();
			}

			// Рулежка.
			UpdateAngle();
			if (!AntMath.Equal(AntMath.Angle(_control.Angle), AntMath.Angle(_targetAngle), 1.0f))
			{
				float curAng = AntMath.Angle(_control.Angle);
				float tarAng = AntMath.Angle(_targetAngle);
				if (Mathf.Abs(curAng - tarAng) > 180.0f)
				{
					if (curAng > tarAng)
					{
						tarAng += 360.0f;
					}
					else
					{
						tarAng -= 360.0f;
					}
				}

				if (curAng < tarAng)
				{
					_control.isLeft = true;
					_control.isRight = false;
				}
				else if (curAng > tarAng)
				{
					_control.isLeft = false;
					_control.isRight = true;
				}
			}
			else
			{
				_control.isLeft = false;
				_control.isRight = false;

				// Газ.
				if (!_isWayFinished)
				{
					_control.isForward = true;
				}
				else
				{
					_control.isForward = false;
				}
			}

			return _isWayFinished;
		}

		private void NextPoint()
		{
			_wayIndex ++;
			if (_wayIndex >= 0 && _wayIndex < _way.Count)
			{
				_nextPoint = _way[_wayIndex];
				UpdateAngle(true);
			}
			else
			{
				_isWayFinished = true;
			}
		}

		private void UpdateAngle(bool aForce = false)
		{
			_updateAngleInterval -= Time.deltaTime;
			if (_updateAngleInterval < 0.0f || aForce)
			{
				_updateAngleInterval = 0.2f;
				_targetAngle = AntMath.AngleDeg((Vector2) _control.Position, _nextPoint);
			}
		}

		private void DrawWay()
		{
			for (int i = 0, n = _way.Count; i < n; i++)
			{
				if (i > 0)
				{
					AntDrawer.DrawLine(_way[i - 1], _way[i], Color.red);
				}
			}
		}
	}
}