using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Misc;

public class AddMaxHealth : SpecialEffectBase
{
	private int _addFinalHealth;

	public AddMaxHealth()
	{
	}

	public AddMaxHealth(int charId, int addFinalHealth)
		: base(charId, 1000001)
	{
		_addFinalHealth = addFinalHealth;
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedData(53, (EDataModifyType)0, -1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 53)
		{
			return 0;
		}
		return _addFinalHealth;
	}

	protected override int GetSubClassSerializedSize()
	{
		return 4;
	}

	protected unsafe override int SerializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = _addFinalHealth;
		ptr += 4;
		return (int)(ptr - pData);
	}

	protected unsafe override int DeserializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		_addFinalHealth = *(int*)ptr;
		ptr += 4;
		return (int)(ptr - pData);
	}
}
