using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameBehaviour : MonoBehaviour {

	protected virtual void Start() {

		foreach(FieldInfo field in ReflectionService.GetTypeMemberFields(GetType())) {
			if(typeof(MonoBehaviour).IsAssignableFrom(field.FieldType)) {
				if (ReflectionService.GetFieldAttribute<InitFromParent>(field) != null) {
					field.SetValue(this, GetComponentInParent(field.FieldType));
				}
				if (ReflectionService.GetFieldAttribute<InitFromSelf>(field) != null) {
					field.SetValue(this, GetComponent(field.FieldType));
				}
				if (ReflectionService.GetFieldAttribute<InitFromChildren>(field) != null) {
					field.SetValue(this, GetComponentInChildren(field.FieldType));
				}
			}
		}


	}


	[AttributeUsage(AttributeTargets.Field)]
	protected class InitFromParent : Attribute { }

	[AttributeUsage(AttributeTargets.Field)]
	protected class InitFromSelf : Attribute { }

	[AttributeUsage(AttributeTargets.Field)]
	protected class InitFromChildren : Attribute { }

}
