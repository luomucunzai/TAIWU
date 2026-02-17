using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Throw
{
	// Token: 0x02000477 RID: 1143
	public class BiAnBaXianZi : CombatSkillEffectBase
	{
		// Token: 0x06003B6B RID: 15211 RVA: 0x00247CF8 File Offset: 0x00245EF8
		public BiAnBaXianZi()
		{
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x00247D02 File Offset: 0x00245F02
		public BiAnBaXianZi(CombatSkillKey skillKey) : base(skillKey, 10401, -1)
		{
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x00247D13 File Offset: 0x00245F13
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x00247D3A File Offset: 0x00245F3A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x00247D64 File Offset: 0x00245F64
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						this._affectCharId = (base.IsDirect ? base.CurrEnemyChar.GetId() : base.CharacterId);
						base.AppendAffectedData(context, this._affectCharId, 163, EDataModifyType.Add, -1);
						base.AppendAffectedData(context, this._affectCharId, 243, EDataModifyType.Add, -1);
						base.AddMaxEffectCount(true);
						base.ShowSpecialEffectTips(0);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x00247E3C File Offset: 0x0024603C
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x00247E8C File Offset: 0x0024608C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this.IsSrcSkillPerformed || dataKey.CharId != this._affectCharId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 163;
				if (flag2)
				{
					base.ReduceEffectCount(1);
					result = 1;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 243;
					if (flag3)
					{
						result = BiAnBaXianZi.PoisonAffectThresholdValues[dataKey.CustomParam0];
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001166 RID: 4454
		private static readonly int[] PoisonAffectThresholdValues = new int[]
		{
			1,
			15,
			25,
			25,
			200,
			200
		};

		// Token: 0x04001167 RID: 4455
		private int _affectCharId;
	}
}
