// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using System;
using UnityEngine;

namespace AmplifyShaderEditor
{
	[Serializable]
	public class TemplateModuleParent
	{
		[SerializeField]
		protected bool m_validData = false;

		public virtual void Draw( UndoParentNode owner ) { }
		public virtual void ReadFromString( ref uint index, ref string[] nodeParams ) { }
		public virtual void WriteToString( ref string nodeInfo ) { }
		public virtual string GenerateShaderData() { return string.Empty; }
		public virtual void Destroy() { }
		public bool ValidData { get { return m_validData; } }
	}
}
