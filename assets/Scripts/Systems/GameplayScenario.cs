using Anthill.Core;

namespace Game.Systems
{
	/// <summary>
	/// This class is a set of systems that solve a specific problem.
	/// For example, in this case, the script implements the gameplay. Past the gameplay scenario
	/// can be any other scripts, for example, for processing game menus, loading levels, etc.
	///
	/// An arbitrary number of different scenarios can work in the game at the same time, as well as
	/// if necessary, you can pause some scripts and activate others.
	/// </summary>
	public class GameplayScenario : AntScenario
	{
		public GameplayScenario() : base("GameplayScenario")
		{
			// ..
		}

		public override void AddedToEngine(AntEngine aEngine)
		{
			base.AddedToEngine(aEngine);
			Add<AIControlSystem>();
			Add<DropperSystem>();
			Add<HealthSystem>();
			Add<MagnetSystem>();
			Add<MovementSystem>();
			Add<PlayerControlSystem>();
			Add<SpawnSystem>();
		}

		public override void RemovedFromEngine(AntEngine aEngine)
		{
			Remove<AIControlSystem>();
			Remove<DropperSystem>();
			Remove<HealthSystem>();
			Remove<MagnetSystem>();
			Remove<MovementSystem>();
			Remove<PlayerControlSystem>();
			Remove<SpawnSystem>();
			base.RemovedFromEngine(aEngine);
		}
	}
}