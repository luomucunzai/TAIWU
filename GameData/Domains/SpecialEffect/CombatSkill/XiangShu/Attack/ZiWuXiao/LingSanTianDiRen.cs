using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ZiWuXiao
{
	// Token: 0x020002B9 RID: 697
	public class LingSanTianDiRen : CombatSkillEffectBase
	{
		// Token: 0x06003234 RID: 12852 RVA: 0x0021E77B File Offset: 0x0021C97B
		public LingSanTianDiRen()
		{
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x0021E785 File Offset: 0x0021C985
		public LingSanTianDiRen(CombatSkillKey skillKey) : base(skillKey, 17113, -1)
		{
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x0021E798 File Offset: 0x0021C998
		public override void OnEnable(DataContext context)
		{
			SkillEffectKey effectKey = new SkillEffectKey(874, true);
			Dictionary<SkillEffectKey, short> effectDict = base.CombatChar.GetSkillEffectCollection().EffectDict;
			short value;
			this._addPower = (int)((effectDict != null && effectDict.TryGetValue(effectKey, out value)) ? (60 * value) : 0);
			bool flag = this._addPower > 0;
			if (flag)
			{
				base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
				DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, effectKey, -effectDict[effectKey], true, false);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x0021E839 File Offset: 0x0021CA39
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x0021E850 File Offset: 0x0021CA50
		private unsafe void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					SkillEffectKey effectKey = new SkillEffectKey(875, true);
					Dictionary<SkillEffectKey, short> effectDict = base.CombatChar.GetSkillEffectCollection().EffectDict;
					bool flag3 = effectDict != null && effectDict.ContainsKey(effectKey);
					if (flag3)
					{
						DomainManager.Combat.CastSkillFree(context, base.CombatChar, 878, ECombatCastFreePriority.Normal);
						base.ShowSpecialEffectTips(1);
					}
					else
					{
						CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
						NeiliAllocation neiliAllocation = enemyChar.GetNeiliAllocation();
						for (byte type = 0; type < 4; type += 1)
						{
							enemyChar.ChangeNeiliAllocation(context, type, (int)(-(*(ref neiliAllocation.Items.FixedElementField + (IntPtr)type * 2)) / 2), true, true);
						}
						base.ShowSpecialEffectTips(2);
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x0021E958 File Offset: 0x0021CB58
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

		// Token: 0x04000EDF RID: 3807
		private const sbyte AddPowerUnit = 60;

		// Token: 0x04000EE0 RID: 3808
		private int _addPower;
	}
}
