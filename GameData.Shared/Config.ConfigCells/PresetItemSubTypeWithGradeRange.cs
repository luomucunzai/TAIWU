using System;

namespace Config.ConfigCells;

[Serializable]
public struct PresetItemSubTypeWithGradeRange
{
	public short SubType;

	public sbyte GradeMin;

	public sbyte GradeMax;

	public PresetItemSubTypeWithGradeRange(short subType, sbyte gradeMin, sbyte gradeMax)
	{
		SubType = subType;
		GradeMin = gradeMin;
		GradeMax = gradeMax;
	}
}
