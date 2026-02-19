using System;

namespace Config;

[Serializable]
public class EnhancedRichTextTagsItem
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly bool HasCloseTag;

	public readonly bool HasHandler;

	public readonly string OpenTagReplacement;

	public readonly string CloseTagReplacement;

	public readonly byte OpenTagLineBreakCount;

	public readonly byte CloseTagLineBreakCount;

	public EnhancedRichTextTagsItem(short arg0, string arg1, bool arg2, bool arg3, string arg4, string arg5, byte arg6, byte arg7)
	{
		TemplateId = arg0;
		Name = arg1;
		HasCloseTag = arg2;
		HasHandler = arg3;
		OpenTagReplacement = arg4;
		CloseTagReplacement = arg5;
		OpenTagLineBreakCount = arg6;
		CloseTagLineBreakCount = arg7;
	}

	public EnhancedRichTextTagsItem()
	{
		TemplateId = 0;
		Name = null;
		HasCloseTag = true;
		HasHandler = false;
		OpenTagReplacement = null;
		CloseTagReplacement = null;
		OpenTagLineBreakCount = 0;
		CloseTagLineBreakCount = 0;
	}
}
