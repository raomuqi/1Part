
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using WanzyeeStudio.Extension;

using Object = UnityEngine.Object;

namespace WanzyeeStudio.Editrix.Drawer{

	/// <summary>
	/// <c>UnityEditor.CustomPropertyDrawer</c> for <c>UnityEngine.Events.UnityEvent</c>.
	/// </summary>
	/// 
	/// <remarks>
	/// Unity doesn't allow to reorder the event list by default with unknown reason.
	/// And not support to select between multiple components of the same type.
	/// This overrides the original drawer to make the <c>UnityEditorInternal.ReorderableList</c> draggable.
	/// And modifies the <c>UnityEditor.GenericMenu</c> items to identify each component.
	/// Note, this works by reflection, it might fail if Unity changes the code in the future.
	/// </remarks>
	/// 
	/*
	 * Since the type of UnityEventBase has been used by UnityEditorInternal.UnityEventDrawer.
	 * Declare multiple attributes to override, user may add or remove custom type manually in case.
	 */
	[CustomPropertyDrawer(typeof(UnityEvent), true)]
	[CustomPropertyDrawer(typeof(UnityEvent<bool>), true)]
	[CustomPropertyDrawer(typeof(UnityEvent<float>), true)]
	[CustomPropertyDrawer(typeof(UnityEvent<int>), true)]
	[CustomPropertyDrawer(typeof(UnityEvent<string>), true)]
	[CustomPropertyDrawer(typeof(UnityEvent<Object>), true)]
	[CustomPropertyDrawer(typeof(UnityEvent<Vector2>), true)]
	[CustomPropertyDrawer(typeof(UnityEvent<Vector3>), true)]
	[CustomPropertyDrawer(typeof(UnityEvent<Color>), true)]
	[CustomPropertyDrawer(typeof(UnityEvent<BaseEventData>), true)]
	internal class EventDrawer : UnityEventDrawer{

		#region Static Fields

		/// <summary>
		/// Method to get the state contains the reorderable list.
		/// </summary>
		private static readonly MethodInfo _stateMethod = typeof(UnityEventDrawer).GetMethod(
			"GetState",
			false,
			typeof(State),
			typeof(SerializedProperty)
		);

		/// <summary>
		/// Field to get the reorderable list to setup.
		/// </summary>
		private static readonly FieldInfo _listField = typeof(State).GetField(
			"m_ReorderableList",
			false,
			typeof(ReorderableList)
		);

		/// <summary>
		/// Field to get the event for building menu.
		/// </summary>
		private static readonly FieldInfo _eventField = typeof(UnityEventDrawer).GetField(
			"m_DummyEvent",
			false,
			typeof(UnityEventBase)
		);

		/// <summary>
		/// Method to build the popup menu to select the component.
		/// </summary>
		private static readonly MethodInfo _buildMethod = typeof(UnityEventDrawer).GetMethod(
			"BuildPopupList",
			true,
			typeof(GenericMenu),
			new[]{
				typeof(Object),
				typeof(UnityEventBase),
				typeof(SerializedProperty)
			}
		);

		/// <summary>
		/// Field to get all the menu items to rename.
		/// </summary>
		private static readonly FieldInfo _itemsField = typeof(GenericMenu).GetField(
			"menuItems",
			false,
			typeof(ArrayList)
		);

		/// <summary>
		/// Field to get the content to rename.
		/// </summary>
		private static readonly FieldInfo _contentField = typeof(GenericMenu).GetNestedType("MenuItem", false).GetField(
			"content",
			false,
			typeof(GUIContent)
		);

		#endregion


		#region Fields

		/// <summary>
		/// The stored property paths which has been initialized.
		/// </summary>
		private readonly List<string> _inited = new List<string>();

		/// <summary>
		/// The current drawing list, used to get element <c>UnityEditor.SerializedProperty</c>.
		/// </summary>
		private ReorderableList _current;

		#endregion


		#region Methods

		/// <summary>
		/// Initialize the list of the specified <c>UnityEditor.SerializedProperty</c>.
		/// Called when <c>GetPropertyHeight()</c> be invoked since it's the first GUI entry.
		/// Set it reorderable, and register new element drawing callback.
		/// </summary>
		/// <param name="property">Property.</param>
		private void Initialize(SerializedProperty property){

			try{

				var _list = _listField.GetValue(_stateMethod.Invoke(this, new object[]{property})) as ReorderableList;
				_list.draggable = true;

				var _callback = _list.drawElementCallback;
				_list.drawElementCallback = (rect, index, isActive, isFocused) => _current = _list;
				_list.drawElementCallback += DrawElement;
				_list.drawElementCallback += _callback;

			}finally{
				
				_inited.Add(property.propertyPath);
				
			}

		}

		/// <summary>
		/// Draw the element of events, callback for reorderable list.
		/// Check if need to override the menu button and show the modified menu.
		/// </summary>
		/// <param name="rect">Rect.</param>
		/// <param name="index">Index.</param>
		/// <param name="isActive">If set to <c>true</c> is active.</param>
		/// <param name="isFocused">If set to <c>true</c> is focused.</param>
		private void DrawElement(Rect rect, int index, bool isActive, bool isFocused){

			try{

				var _listener = _current.serializedProperty.GetArrayElementAtIndex(index);

				var _target = _listener.FindPropertyRelative("m_Target").objectReferenceValue;

				if(!(_target is GameObject || _target is Component)) return;

				var _rect = Rect.MinMaxRect(rect.x + (rect.width * 0.3f) + 3f, rect.y, rect.xMax + 2f, rect.y + 21f);

				if(GUI.Button(_rect, GUIContent.none, GUIStyle.none)) GetMenu(_target, _listener).DropDown(_rect);

			}catch{

				_current.drawElementCallback -= DrawElement;

			}

		}

		/// <summary>
		/// Get the original menu with items renamed to identify components.
		/// </summary>
		/// <returns>The menu.</returns>
		/// <param name="target">Target.</param>
		/// <param name="listener">Listener.</param>
		private GenericMenu GetMenu(Object target, SerializedProperty listener){

			var _event = _eventField.GetValue(this);

			var _result = _buildMethod.Invoke(null, new object[]{target, _event, listener}) as GenericMenu;

			RenameMenuItems(_result);

			return _result;

		}

		/// <summary>
		/// Rename the items of the specified menu with target component index.
		/// </summary>
		/// <param name="menu">Menu.</param>
		/*
		 * Skip the first 2 items, which are "No Function" and a separator.
		 */
		private void RenameMenuItems(GenericMenu menu){

			var _items = (_itemsField.GetValue(menu) as ArrayList).Cast<object>();
			var _contents = _items.Skip(2).Select((item) => _contentField.GetValue(item) as GUIContent);

			var _group = _contents.GroupBy((content) => content.text.Remove(content.text.IndexOf('/')));
			var _firsts = _group.Select((contents) => contents.First().text).ToList();

			var _index = -1;
			foreach(var _content in _contents){

				if(_firsts.Contains(_content.text)) _index++;
				_content.text = string.Format("[{0}] {1}", _index, _content.text);

			}

		}

		/// <summary>
		/// Get the height of the property.
		/// </summary>
		/// <returns>The property height.</returns>
		/// <param name="property">Property.</param>
		/// <param name="label">Label.</param>
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label){

			if(!_inited.Contains(property.propertyPath)) Initialize(property);

			return base.GetPropertyHeight(property, label);

		}

		#endregion

	}

}
