﻿using UnityEngine;
using System.Collections;
using System;

public class CustomInputEventArgs : EventArgs
{
		public CustomInput Input{ get; private set; }

		public CustomInputEventArgs (CustomInput input)
		{
				this.Input = input;
		}
}
