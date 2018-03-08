﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrbbecGestures
{
	public class PlayerState_SegState : PlayerStateBase
	{
		public override IntPtr GetGestureInfoPtr()
		{
			return GestureInfoData.GetItemHeadPtr();
		}

		public override void ResetGestureInfoSize(int Size)
		{
			GestureInfoData.Resize(Size);
		}

		public OGList<GestureInfo_Seg_State> GestureInfoData = new OGList<GestureInfo_Seg_State>();
	}
}
