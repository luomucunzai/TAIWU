using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.RanChenZi
{
	// Token: 0x020002EB RID: 747
	public class LiuQiBaJian : CombatSkillEffectBase
	{
		// Token: 0x0600334A RID: 13130 RVA: 0x002243E2 File Offset: 0x002225E2
		public LiuQiBaJian()
		{
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x002243EC File Offset: 0x002225EC
		public LiuQiBaJian(CombatSkillKey skillKey) : base(skillKey, 17134, -1)
		{
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x00224400 File Offset: 0x00222600
		public override void OnEnable(DataContext context)
		{
			this._addPower = 10 * base.CombatChar.GetDefeatMarkCollection().MindMarkList.Count;
			bool flag = this._addPower > 0;
			if (flag)
			{
				base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
				base.ShowSpecialEffectTips(0);
			}
			sbyte[] taskStatus = new sbyte[]
			{
				DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(7).JuniorXiangshuTaskStatus,
				DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(5).JuniorXiangshuTaskStatus,
				DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(4).JuniorXiangshuTaskStatus
			};
			bool goodEnding = !taskStatus.Exist((sbyte status) => status != 6);
			bool badEnding = !taskStatus.Exist((sbyte status) => status != 5);
			bool flag2 = goodEnding || badEnding;
			if (flag2)
			{
				bool flag3 = goodEnding;
				if (flag3)
				{
					DomainManager.Combat.RemoveMindDefeatMark(context, base.CurrEnemyChar, 2, true, 0);
				}
				else
				{
					DomainManager.Combat.AppendMindDefeatMark(context, base.CurrEnemyChar, 2, -1, false);
				}
				base.ShowSpecialEffectTips(goodEnding, 2, 3);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x00224542 File Offset: 0x00222742
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x00224558 File Offset: 0x00222758
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					List<bool> markList = base.CombatChar.GetDefeatMarkCollection().MindMarkList;
					bool flag3 = markList.Count > 0;
					if (flag3)
					{
						CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
						int transferCount = Math.Min(6, markList.Count);
						for (int i = 0; i < transferCount; i++)
						{
							DomainManager.Combat.TransferRandomMindDefeatMark(context, base.CombatChar, enemyChar);
						}
						DomainManager.Combat.AddToCheckFallenSet(enemyChar.GetId());
						base.ShowSpecialEffectTips(1);
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x00224630 File Offset: 0x00222830
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
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
					result = this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000F26 RID: 3878
		private const sbyte AddPowerUnit = 10;

		// Token: 0x04000F27 RID: 3879
		private const sbyte TransferCount = 6;

		// Token: 0x04000F28 RID: 3880
		private int _addPower;
	}
}
