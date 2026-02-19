using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation;

[SerializableGameData(NotForDisplayModule = true)]
public abstract class OperationGridBase : OperationBase
{
	public int GridIndex { get; private set; }

	public bool IsActive { get; private set; }

	public Coordinate2D<sbyte> Coordinate => Grid.ToCenterAnchoredCoordinate(GridIndex);

	public OperationGridBase()
	{
	}

	public override string Inspect()
	{
		return $"Grid[{GridIndex % 7 - 3}, {GridIndex / 7 - 3}]";
	}

	public override int GetSerializedSize()
	{
		int num = 0;
		num += base.GetSerializedSize();
		num += 4;
		return num + 1;
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Serialize(ptr);
		*(int*)ptr = GridIndex;
		ptr += 4;
		*ptr = (IsActive ? ((byte)1) : ((byte)0));
		ptr++;
		return (int)(ptr - pData);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Deserialize(ptr);
		GridIndex = *(int*)ptr;
		ptr += 4;
		IsActive = *ptr != 0;
		ptr++;
		return (int)(ptr - pData);
	}

	public void MakeActive()
	{
		Tester.Assert(!IsActive);
		IsActive = true;
	}

	public void MakeNonActive()
	{
		Tester.Assert(IsActive);
		IsActive = false;
	}

	protected OperationGridBase(sbyte playerId, int stamp, int gridIndex)
		: base(playerId, stamp)
	{
		GridIndex = gridIndex;
	}
}
