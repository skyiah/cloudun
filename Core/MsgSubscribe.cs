﻿namespace Greatbone.Core
{
	public delegate void MsgDoer(MsgContext ec);

	public class MsgSubscribe : IMember
	{
		public string Topic { get; }

		private readonly MsgDoer doer;

		internal MsgSubscribe(string topic, MsgDoer doer)
		{
			// NOTE: strict method nzame as key here to avoid the default base url trap
			Topic = topic;
			this.doer = doer;
		}

		internal void Do(MsgContext ec)
		{
			doer(ec);
		}

		public string Key => Topic;

		public override string ToString()
		{
			return Topic;
		}
	}
}