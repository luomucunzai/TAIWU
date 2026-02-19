using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Map;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Leg;

public class YanWangGuiJiao : PowerUpOnCast
{
	private const sbyte AddPowerUnit = 10;

	protected override EDataModifyType ModifyType => (EDataModifyType)1;

	public YanWangGuiJiao()
	{
	}

	public YanWangGuiJiao(CombatSkillKey skillKey)
		: base(skillKey, 15306)
	{
	}

	public override void OnEnable(DataContext context)
	{
		GameData.Domains.Character.Character character = (base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly)).GetCharacter();
		sbyte b = (sbyte)NeiliType.Instance[character.GetNeiliType()].FiveElements;
		PowerUpValue = 0;
		if (b != 5 && b >= 0)
		{
			Location location = CharObj.GetLocation();
			if (!location.IsValid() && base.CharacterId == DomainManager.Taiwu.GetTaiwuCharId())
			{
				location = CharObj.GetValidLocation();
			}
			if (location.IsValid())
			{
				CalcPowerUp(location, b);
			}
		}
		base.OnEnable(context);
	}

	private void CalcPowerUp(Location location, sbyte srcFiveElementsType)
	{
		MapBlockData block = DomainManager.Map.GetBlock(location.AreaId, location.BlockId);
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		sbyte b = (base.IsDirect ? FiveElementsType.Produced[srcFiveElementsType] : FiveElementsType.Countered[srcFiveElementsType]);
		int taiwuViewRange = DomainManager.Map.GetTaiwuViewRange(block);
		DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, list, taiwuViewRange);
		list.Add(block);
		for (int i = 0; i < list.Count; i++)
		{
			HashSet<int> graveSet = list[i].GraveSet;
			if (graveSet == null)
			{
				continue;
			}
			foreach (int item2 in graveSet)
			{
				sbyte item = CharacterDomain.CalcBirthYearAndMonth(DomainManager.Character.GetDeadCharacter(item2).BirthDate).month;
				if (GameData.Domains.Character.SharedMethods.GetInnateFiveElementsType(item) == b)
				{
					PowerUpValue += 10;
				}
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
	}
}
