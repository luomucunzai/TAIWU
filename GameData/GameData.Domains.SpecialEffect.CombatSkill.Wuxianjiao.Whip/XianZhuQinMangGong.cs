using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Whip;

public class XianZhuQinMangGong : CombatSkillEffectBase
{
	private const sbyte AddPowerUnit = 30;

	private const sbyte DirectSilenceCount = 1;

	private const short DirectSilenceFrame = 900;

	private const sbyte ReverseSilenceCount = 2;

	private const short ReverseSilenceFrame = 1800;

	private const short ReverseSilenceSelfFrame = 2700;

	private readonly HashSet<CombatWeaponData> _affectingWeapons = new HashSet<CombatWeaponData>();

	public XianZhuQinMangGong()
	{
	}

	public XianZhuQinMangGong(CombatSkillKey skillKey)
		: base(skillKey, 12406, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType> { 
		{
			new AffectedDataKey(base.CharacterId, 199, -1),
			(EDataModifyType)1
		} };
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_WeaponCdEnd(OnWeaponCdEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_WeaponCdEnd(OnWeaponCdEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId || !PowerMatchAffectRequire(power) || interrupted)
		{
			return;
		}
		CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		ItemKey[] weapons = enemyChar.GetWeapons();
		int usingWeaponIndex = enemyChar.GetUsingWeaponIndex();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		for (int i = 0; i < 3; i++)
		{
			if (i != usingWeaponIndex)
			{
				ItemKey itemKey = weapons[i];
				if (itemKey.IsValid() && enemyChar.GetWeaponData(i).GetDurability() > 0)
				{
					list.Add(i);
				}
			}
		}
		int num = Math.Min(base.IsDirect ? 1 : 2, list.Count);
		if (num > 0)
		{
			if (list.Select((int x) => enemyChar.GetWeaponData(x)).All((CombatWeaponData x) => x.GetFixedCdLeftFrame() == 0))
			{
				CollectionUtils.Shuffle(context.Random, list);
			}
			else
			{
				list.Sort(Comparison);
			}
			for (int num2 = 0; num2 < num; num2++)
			{
				int num3 = list[num2];
				short cdFrame = (short)(base.IsDirect ? 900 : 1800);
				DomainManager.Combat.SilenceWeapon(context, enemyChar, num3, cdFrame);
				CombatWeaponData weaponData = enemyChar.GetWeaponData(num3);
				_affectingWeapons.Add(weaponData);
			}
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			ShowSpecialEffectTips(0);
		}
		ObjectPool<List<int>>.Instance.Return(list);
		if (!base.IsDirect)
		{
			DomainManager.Combat.SilenceSkill(context, base.CombatChar, base.SkillTemplateId, 2700, -1);
		}
		ShowSpecialEffectTips(1);
		int Comparison(int weaponIndexL, int weaponIndexR)
		{
			CombatWeaponData weaponData2 = enemyChar.GetWeaponData(weaponIndexL);
			CombatWeaponData weaponData3 = enemyChar.GetWeaponData(weaponIndexR);
			if (_affectingWeapons.Contains(weaponData2) != _affectingWeapons.Contains(weaponData3))
			{
				return _affectingWeapons.Contains(weaponData2) ? 1 : (-1);
			}
			return (weaponData2.GetFixedCdLeftFrame() != weaponData3.GetFixedCdLeftFrame()) ? weaponData2.GetFixedCdLeftFrame().CompareTo(weaponData3.GetFixedCdLeftFrame()) : weaponIndexL.CompareTo(weaponIndexR);
		}
	}

	private void OnWeaponCdEnd(DataContext context, int charId, bool isAlly, CombatWeaponData weapon)
	{
		if (_affectingWeapons.Contains(weapon))
		{
			_affectingWeapons.Remove(weapon);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || !base.IsDirect)
		{
			return 0;
		}
		if (dataKey.FieldId == 199)
		{
			return _affectingWeapons.Count * 30;
		}
		return 0;
	}
}
