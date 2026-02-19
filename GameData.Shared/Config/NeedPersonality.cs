using System;

namespace Config;

[Serializable]
public struct NeedPersonality
{
	public sbyte PersonalityType;

	public int NeedCount;

	public NeedPersonality(sbyte type, int count)
	{
		PersonalityType = type;
		NeedCount = count;
	}
}
