using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class ZhujianCombatSkillToWeapon : ConfigData<ZhujianCombatSkillToWeaponItem, int>
{
	public static ZhujianCombatSkillToWeapon Instance = new ZhujianCombatSkillToWeapon();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "CombatSkillId", "WeaponId", "EffectId", "TemplateId" };

	internal override int ToInt(int value)
	{
		return value;
	}

	internal override int ToTemplateId(int value)
	{
		return value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(0, 558, 467, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(1, 559, 440, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(2, 560, 477, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(3, 561, 450, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(4, 562, 487, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(5, 563, 460, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(6, 564, 470, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(7, 565, 443, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(8, 598, 538, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(9, 599, 565, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(10, 600, 548, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(11, 601, 575, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(12, 602, 531, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(13, 603, 558, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(14, 604, 532, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(15, 605, 559, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(16, 640, 619, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(17, 641, 673, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(18, 642, 638, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(19, 643, 692, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(20, 644, 656, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(21, 645, 666, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(22, 646, 640, 55));
		_dataArray.Add(new ZhujianCombatSkillToWeaponItem(23, 647, 623, 55));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<ZhujianCombatSkillToWeaponItem>(24);
		CreateItems0();
	}
}
