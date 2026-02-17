using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.FistAndPalm
{
	// Token: 0x0200042A RID: 1066
	public class XiangMoGuangMingQuan : CombatSkillEffectBase
	{
		// Token: 0x0600398E RID: 14734 RVA: 0x0023F10F File Offset: 0x0023D30F
		public XiangMoGuangMingQuan()
		{
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x0023F119 File Offset: 0x0023D319
		public XiangMoGuangMingQuan(CombatSkillKey skillKey) : base(skillKey, 1107, -1)
		{
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x0023F12C File Offset: 0x0023D32C
		public override void OnEnable(DataContext context)
		{
			sbyte targetType = base.IsDirect ? 1 : 3;
			sbyte enemyType = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetCharacter().GetBehaviorType();
			this._addPower = 40 * Math.Abs((int)(targetType - enemyType));
			this._addRange = 10 * Math.Abs((int)(targetType - enemyType));
			bool flag = this._addPower > 0;
			if (flag)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x0023F236 File Offset: 0x0023D436
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x0023F24C File Offset: 0x0023D44C
		private unsafe void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				sbyte targetType = base.IsDirect ? 1 : 3;
				CombatCharacter enemyCombatChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				Character enemyChar = enemyCombatChar.GetCharacter();
				HitOrAvoidInts selfHits = this.CharObj.GetHitValues();
				HitOrAvoidInts enemyAvoids = enemyChar.GetAvoidValues();
				sbyte enemyType = enemyChar.GetBehaviorType();
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && *(ref selfHits.Items.FixedElementField + (IntPtr)3 * 4) > *(ref enemyAvoids.Items.FixedElementField + (IntPtr)3 * 4) && targetType != enemyType;
				if (flag2)
				{
					enemyChar.ChangeBaseMorality(context, (enemyType > targetType) ? 125 : -125);
					base.ShowSpecialEffectTips(1);
					bool flag3 = enemyChar.GetBehaviorType() == targetType && enemyCombatChar.AiController.CanFlee();
					if (flag3)
					{
						enemyCombatChar.SetNeedUseOtherAction(context, 2);
						base.AppendAffectedData(context, enemyCombatChar.GetId(), 124, EDataModifyType.AddPercent, -1);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x0023F37C File Offset: 0x0023D57C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 124;
			int result;
			if (flag)
			{
				base.RemoveSelf(DomainManager.Combat.Context);
				result = -67;
			}
			else
			{
				bool flag2 = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 199;
					if (flag3)
					{
						result = this._addPower;
					}
					else
					{
						bool flag4 = dataKey.FieldId == 145 || dataKey.FieldId == 146;
						if (flag4)
						{
							result = this._addRange;
						}
						else
						{
							result = 0;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x040010D1 RID: 4305
		private const sbyte AddPowerUnit = 40;

		// Token: 0x040010D2 RID: 4306
		private const sbyte AddRangeUnit = 10;

		// Token: 0x040010D3 RID: 4307
		private const sbyte ChangeMorality = 125;

		// Token: 0x040010D4 RID: 4308
		private int _addPower;

		// Token: 0x040010D5 RID: 4309
		private int _addRange;
	}
}
