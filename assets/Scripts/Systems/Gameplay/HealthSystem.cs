using System.Collections.Generic;
using UnityEngine;
using Anthill.Core;
using Game.Core;
using Game.Nodes;
using Game.Components;

namespace Game.Systems
{
	/// <summary>
	/// Данная система следит за состоянием здоровья игровых объектов и удаляет их
	/// если здоровье закончилось. Кроме этого, данная система обрабатывает панели здоровья
	/// для игровых объектов.
	/// </summary>
	public class HealthSystem : ISystem, IExecuteSystem
	{
		private AntNodeList<HealthNode> _healthNodes;
		private List<KeyValuePair<HealthNode, HealthBar>> _healthBars;
		private GameCore _gameCore;
		private AntEngine _engine;

		#region ISystem Implementation
		
		public void AddedToEngine(AntEngine aEngine)
		{
			_healthNodes = aEngine.GetNodes<HealthNode>();

			// Подписываемся на события добавления/удаления игровых объектов обладающих
			// компонентом здоровья чтобы создавать/удалять панели здоровья для них.
			_healthNodes.EventNodeAdded += OnHealthNodeAdded;
			_healthNodes.EventNodeRemoved += OnHealthNodeRemoved;

			// Ищим игровой объект, т.к. в нем указан префаб для панелей здоровья.
			_gameCore = GameObject.Find("Game").GetComponent<GameCore>();
			_healthBars = new List<KeyValuePair<HealthNode, HealthBar>>();
			_engine = aEngine;
		}

		public void RemovedFromEngine(AntEngine aEngine)
		{
			_healthNodes = null;
			_engine = null;
		}
		
		#endregion
		#region IExecuteSystem Implementation
		
		public void Execute()
		{
			// Следим за здоровьем игровых объектов.
			HealthNode node;
			for (int i = _healthNodes.Count - 1; i >= 0; i--)
			{
				node = _healthNodes[i];
				if (node.Health.HP <= 0.0f)
				{
					// Удаляем игровые объекты если они не обладают достаточнм здоровьем.
					_engine.RemoveEntity(node.entity);
					GameObject.DestroyObject(node.entity.gameObject);
				}
			}

			// Обновляем состояние для панелей здоровья.
			KeyValuePair<HealthNode, HealthBar> pair;
			for (int i = 0, n = _healthBars.Count; i < n; i++)
			{
				pair = _healthBars[i];
				pair.Value.HP = pair.Key.Health.HP;
				pair.Value.Position = pair.Key.entity.Position;
			}
		}
		
		#endregion
		#region Event Handlers
		
		private void OnHealthNodeAdded(HealthNode aNode)
		{
			// Если объект обладает здоровьем более 1hp, 
			// то для него следует создать панель здоровья.
			if (aNode.Health.maxHP > 1.0f)
			{
				HealthBar healthBar = GameObject.Instantiate((GameObject) _gameCore.healthBarPrefab).GetComponent<HealthBar>();
				healthBar.HP = aNode.Health.HP;
				healthBar.MaxHP = aNode.Health.maxHP;
				_healthBars.Add(new KeyValuePair<HealthNode, HealthBar>(aNode, healthBar));
			}
		}

		private void OnHealthNodeRemoved(HealthNode aNode)
		{
			// Подсчет статистики.
			Vision vision = aNode.entity.GetComponent<Vision>();
			if (vision != null)
			{
				if (vision.group == GroupEnum.GreenTeam)
				{
					_gameCore.GreenKills++;
				}
				else if (vision.group == GroupEnum.RedTeam)
				{
					_gameCore.RedKills++;
				}
			}

			int index = _healthBars.FindIndex(x => System.Object.ReferenceEquals(x.Key, aNode));
			if (index >= 0 && index < _healthBars.Count)
			{
				_healthBars[index].Value.Destroy();
				_healthBars.RemoveAt(index);
			}
		}
		
		#endregion
	}
}