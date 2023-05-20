using Godot;
using HonedGodot.Extensions;
using System;

namespace HonedGodot
{
	public class IdTag<T> : Node
	{
		public T Data { get; set; }
		private Node parent;
		private Guid guid = Guid.NewGuid();

		public override void _Ready()
		{
			parent = GetParent();

			this.InlineConnect<Node>(parent, Constants.Signal_Node_ChildEnteredTree, child => 
			{
				Propagate(child);
			});

			Logging.Log($"Tagged {parent.Name} with data {Data}", HonedGodotLogTag.Tagging);
		}

		public void Propagate(params Node[] otherNodes)
		{
			foreach(var node in otherNodes)
			{
				var clone = new IdTag<T>();
				clone.Data = Data;

				node.AddChild(clone);
			}
		}

		public bool SameTag(IdTag<T> otherTag)
		{
			return guid == otherTag.guid;
		}
	}
}