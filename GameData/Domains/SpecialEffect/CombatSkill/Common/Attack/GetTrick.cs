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
	// Token: 0x0200059B RID: 1435
	public class GetTrick : CombatSkillEffectBase
	{
		// Token: 0x0600429E RID: 17054 RVA: 0x00267BE6 File Offset: 0x00265DE6
		protected GetTrick()
		{
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x00267BF0 File Offset: 0x00265DF0
		protected GetTrick(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060042A0 RID: 17056 RVA: 0x00267C00 File Offset: 0x00265E00
		public override void OnEnable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 139, -1, -1, -1, -1), EDataModifyType.Custom);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x00267C6A File Offset: 0x00265E6A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060042A2 RID: 17058 RVA: 0x00267C94 File Offset: 0x00265E94
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
						bool isDirect = base.IsDirect;
						if (isDirect)
						{
							this.IsSrcSkillPerformed = true;
							base.AddMaxEffectCount(true);
						}
						else
						{
							DomainManager.Combat.AddTrick(context, base.CombatChar, this.GetTrickType, 2, true, false);
							base.ShowSpecialEffectTips(0);
							base.RemoveSelf(context);
						}
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

		// Token: 0x060042A3 RID: 17059 RVA: 0x00267D54 File Offset: 0x00265F54
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x00267DA4 File Offset: 0x00265FA4
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 139;
				if (flag2)
				{
					DataContext context = DomainManager.Combat.Context;
					sbyte trickType = (sbyte)dataValue;
					bool flag3 = this.DirectCanChangeTrickType.Exist(trickType);
					if (flag3)
					{
						base.ShowSpecialEffectTips(0);
						base.ReduceEffectCount(1);
						return (int)this.GetTrickType;
					}
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x040013B4 RID: 5044
		private const sbyte ReverseGetTrickCount = 2;

		// Token: 0x040013B5 RID: 5045
		protected sbyte GetTrickType;

		// Token: 0x040013B6 RID: 5046
		protected sbyte[] DirectCanChangeTrickType;
	}
}
