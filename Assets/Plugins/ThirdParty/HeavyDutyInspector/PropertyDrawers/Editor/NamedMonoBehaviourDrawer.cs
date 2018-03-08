/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

//----------------------------------------------
//            Heavy-Duty Inspector
//      Copyright © 2013 - 2015  Illogika
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace HeavyDutyInspector
{

	[CustomPropertyDrawer(typeof(NamedMonoBehaviour))]
	public class NamedMonoBehaviourDrawer : IllogikaDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			OnNamedMonoBehaviourGUI(position, property, label, fieldInfo.FieldType, 0);
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) * 2;
		}


	}

}
