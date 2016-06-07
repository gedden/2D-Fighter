using UnityEngine;
using UnityEditor;

namespace Owl
{
	/// <summary>
	/// Simple general and universal extentions
	/// </summary>
	public class OwlExtention
	{
		public OwlExtention ()
		{
		}

		[MenuItem("Assets/Show In Project")]
		/// <summary>
		/// Lets you right click on an asset and do "Show in Project"
		/// BERY USEFUL!
		/// </summary>
		private static void ShowInProject()
		{
			EditorGUIUtility.PingObject( Selection.activeObject );
		}


		[MenuItem("Owl/Anchors to Corners %[")]
		/// <summary>
		/// Anchorses to corners.
		/// 
		/// Hopefully this just gets added to unity here in the near future.
		/// 
		/// This was written by some dude named Senshi
		/// http://answers.unity3d.com/questions/782478/unity-46-beta-anchor-snap-to-button-new-ui-system.html
		/// </summary>
		static void AnchorsToCorners()
		{
			OwlUtil.AnchorsToCorners(Selection.activeTransform as RectTransform);
		}
	}
}

