using UnityEngine;
using System.Collections;

namespace Core.FSM
{
	public class IdleAction
	{
		private readonly FSMState owner;

		public IdleAction (FSMState owner)
		{
			this.owner = owner;
		}

		public FSMState GetOwner ()
		{
			return owner;
		}

		///<summary>
		/// Starts the action.
		///</summary>
		public virtual void OnEnter ()
		{
		}

		///<summary>
		/// Updates the action.
		///</summary>
		public virtual void OnUpdate ()
		{
		}

		///<summary>
		/// Finishes the action.
		///</summary>
		public virtual void OnExit ()
		{
		}
	}
}
