using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x0200058E RID: 1422
	public class AttackBodyPartLegacy : CombatSkillEffectBase
	{
		// Token: 0x0600421D RID: 16925 RVA: 0x00265550 File Offset: 0x00263750
		protected AttackBodyPartLegacy()
		{
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x0026555A File Offset: 0x0026375A
		protected AttackBodyPartLegacy(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x00265568 File Offset: 0x00263768
		public override void OnEnable(DataContext context)
		{
			this._affected = false;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 77, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 140, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			}
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x00265610 File Offset: 0x00263810
		public override void OnDisable(DataContext context)
		{
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x00265650 File Offset: 0x00263850
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.AppendAffectedData(context, base.CharacterId, 69, EDataModifyType.AddPercent, base.SkillTemplateId);
			}
		}

		// Token: 0x06004222 RID: 16930 RVA: 0x0026569C File Offset: 0x0026389C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool affected = this._affected;
				if (affected)
				{
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x002656E4 File Offset: 0x002638E4
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 77;
				if (flag2)
				{
					this._affected = true;
					result = true;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x0026573C File Offset: 0x0026393C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result2;
			if (flag)
			{
				result2 = 0;
			}
			else
			{
				DataContext context = DomainManager.Combat.Context;
				bool flag2 = dataKey.FieldId == 140 && dataKey.CombatSkillId == base.SkillTemplateId && this.BodyParts.Exist((sbyte)dataValue) && context.Random.CheckPercentProb(50);
				if (flag2)
				{
					List<sbyte> partRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
					partRandomPool.Clear();
					partRandomPool.AddRange(AttackBodyPartLegacy.ReverseSecondRandOdds);
					for (int i = 0; i < this.BodyParts.Length; i++)
					{
						partRandomPool[(int)this.BodyParts[i]] = 0;
					}
					int sum = partRandomPool.ToArray().Sum();
					int result = context.Random.Next(sum);
					int accumulator = 0;
					for (sbyte part = 0; part < 7; part += 1)
					{
						accumulator += (int)partRandomPool[(int)part];
						bool flag3 = accumulator > result;
						if (flag3)
						{
							dataValue = (int)part;
							break;
						}
					}
					ObjectPool<List<sbyte>>.Instance.Return(partRandomPool);
				}
				result2 = dataValue;
			}
			return result2;
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x00265870 File Offset: 0x00263A70
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 69 && dataKey.CombatSkillId == base.SkillTemplateId && this.BodyParts.Exist((sbyte)dataKey.CustomParam1);
			int result;
			if (flag)
			{
				this._affected = true;
				result = (int)this.ReverseAddDamagePercent;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04001380 RID: 4992
		private const sbyte ReverseMissOdds = 50;

		// Token: 0x04001381 RID: 4993
		private static readonly sbyte[] ReverseSecondRandOdds = new sbyte[]
		{
			20,
			20,
			1,
			20,
			20,
			20,
			20
		};

		// Token: 0x04001382 RID: 4994
		protected sbyte[] BodyParts;

		// Token: 0x04001383 RID: 4995
		protected sbyte ReverseAddDamagePercent;

		// Token: 0x04001384 RID: 4996
		private bool _affected;
	}
}
