﻿using Greatbone.Core;

namespace Greatbone.Sample
{
	public class Brand : ISerial
	{
		public string Id;

		public string Name;

		public char[] Credential { get; set; }

		public long ModifiedOn { get; set; }

		public string Key => Id;

		public void ReadFrom(ISerialReader r)
		{
		}

		public void WriteTo(ISerialWriter w)
		{
		}
	}
}