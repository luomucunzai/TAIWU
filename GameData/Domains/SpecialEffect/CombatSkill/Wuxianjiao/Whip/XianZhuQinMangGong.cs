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

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Whip
{
	// Token: 0x02000384 RID: 900
	public class XianZhuQinMangGong : CombatSkillEffectBase
	{
		// Token: 0x06003607 RID: 13831 RVA: 0x0022EF1F File Offset: 0x0022D11F
		public XianZhuQinMangGong()
		{
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x0022EF34 File Offset: 0x0022D134
		public XianZhuQinMangGong(CombatSkillKey skillKey) : base(skillKey, 12406, -1)
		{
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x0022EF50 File Offset: 0x0022D150
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>
			{
				{
					new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1),
					EDataModifyType.AddPercent
				}
			};
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_WeaponCdEnd(new Events.OnWeaponCdEnd(this.OnWeaponCdEnd));
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x0022EFA9 File Offset: 0x0022D1A9
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_WeaponCdEnd(new Events.OnWeaponCdEnd(this.OnWeaponCdEnd));
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x0022EFD0 File Offset: 0x0022D1D0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			XianZhuQinMangGong.<>c__DisplayClass11_0 CS$<>8__locals1 = new XianZhuQinMangGong.<>c__DisplayClass11_0();
			CS$<>8__locals1.<>4__this = this;
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.PowerMatchAffectRequire((int)power, 0) || interrupted;
			if (!flag)
			{
				CS$<>8__locals1.enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				ItemKey[] weapons = CS$<>8__locals1.enemyChar.GetWeapons();
				int currWeaponIndex = CS$<>8__locals1.enemyChar.GetUsingWeaponIndex();
				List<int> weaponIndexRandomPool = ObjectPool<List<int>>.Instance.Get();
				weaponIndexRandomPool.Clear();
				for (int i = 0; i < 3; i++)
				{
					bool flag2 = i == currWeaponIndex;
					if (!flag2)
					{
						ItemKey weaponKey = weapons[i];
						bool flag3 = !weaponKey.IsValid();
						if (!flag3)
						{
							bool flag4 = CS$<>8__locals1.enemyChar.GetWeaponData(i).GetDurability() <= 0;
							if (!flag4)
							{
								weaponIndexRandomPool.Add(i);
							}
						}
					}
				}
				int silenceCount = Math.Min(base.IsDirect ? 1 : 2, weaponIndexRandomPool.Count);
				bool flag5 = silenceCount > 0;
				if (flag5)
				{
					bool flag6 = (from x in weaponIndexRandomPool
					select CS$<>8__locals1.enemyChar.GetWeaponData(x)).All((CombatWeaponData x) => x.GetFixedCdLeftFrame() == 0);
					if (flag6)
					{
						CollectionUtils.Shuffle<int>(context.Random, weaponIndexRandomPool);
					}
					else
					{
						weaponIndexRandomPool.Sort(new Comparison<int>(CS$<>8__locals1.<OnCastSkillEnd>g__Comparison|0));
					}
					for (int j = 0; j < silenceCount; j++)
					{
						int weaponIndex = weaponIndexRandomPool[j];
						short cdFrame = base.IsDirect ? 900 : 1800;
						DomainManager.Combat.SilenceWeapon(context, CS$<>8__locals1.enemyChar, weaponIndex, (int)cdFrame);
						CombatWeaponData weaponData = CS$<>8__locals1.enemyChar.GetWeaponData(weaponIndex);
						this._affectingWeapons.Add(weaponData);
					}
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					base.ShowSpecialEffectTips(0);
				}
				ObjectPool<List<int>>.Instance.Return(weaponIndexRandomPool);
				bool flag7 = !base.IsDirect;
				if (flag7)
				{
					DomainManager.Combat.SilenceSkill(context, base.CombatChar, base.SkillTemplateId, 2700, -1);
				}
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x0022F224 File Offset: 0x0022D424
		private void OnWeaponCdEnd(DataContext context, int charId, bool isAlly, CombatWeaponData weapon)
		{
			bool flag = !this._affectingWeapons.Contains(weapon);
			if (!flag)
			{
				this._affectingWeapons.Remove(weapon);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x0022F270 File Offset: 0x0022D470
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || !base.IsDirect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = this._affectingWeapons.Count * 30;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000FBC RID: 4028
		private const sbyte AddPowerUnit = 30;

		// Token: 0x04000FBD RID: 4029
		private const sbyte DirectSilenceCount = 1;

		// Token: 0x04000FBE RID: 4030
		private const short DirectSilenceFrame = 900;

		// Token: 0x04000FBF RID: 4031
		private const sbyte ReverseSilenceCount = 2;

		// Token: 0x04000FC0 RID: 4032
		private const short ReverseSilenceFrame = 1800;

		// Token: 0x04000FC1 RID: 4033
		private const short ReverseSilenceSelfFrame = 2700;

		// Token: 0x04000FC2 RID: 4034
		private readonly HashSet<CombatWeaponData> _affectingWeapons = new HashSet<CombatWeaponData>();
	}
}
