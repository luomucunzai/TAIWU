using System;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Shot
{
	// Token: 0x020005BF RID: 1471
	public class ZhenSiBuZhenHuoFa : CombatSkillEffectBase
	{
		// Token: 0x060043A2 RID: 17314 RVA: 0x0026C12D File Offset: 0x0026A32D
		public ZhenSiBuZhenHuoFa()
		{
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x0026C137 File Offset: 0x0026A337
		public ZhenSiBuZhenHuoFa(CombatSkillKey skillKey) : base(skillKey, 3205, -1)
		{
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x0026C148 File Offset: 0x0026A348
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(89, EDataModifyType.Custom, base.SkillTemplateId);
			Events.RegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x0026C17F File Offset: 0x0026A37F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x0026C1A8 File Offset: 0x0026A3A8
		private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			bool flag = this.SkillKey.IsMatch(attackerId, combatSkillId);
			if (flag)
			{
				this._fatalMarkCount += outerMarkCount + innerMarkCount;
			}
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x0026C1DC File Offset: 0x0026A3DC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = this._fatalMarkCount > 0;
				if (flag2)
				{
					base.CurrEnemyChar.WorsenRepeatableInjury(context, base.IsDirect, this._fatalMarkCount);
					base.ShowSpecialEffectTips(1);
				}
				this._fatalMarkCount = 0;
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x0026C248 File Offset: 0x0026A448
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 89;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool inner = dataKey.CustomParam1 == 1;
				bool flag2 = base.IsDirect == inner || dataValue <= 0L;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					EDamageType damageType = (EDamageType)dataKey.CustomParam0;
					if (!true)
					{
					}
					CombatCharacter combatCharacter;
					if (damageType != EDamageType.Direct)
					{
						if (damageType != EDamageType.Bounce)
						{
							combatCharacter = null;
						}
						else
						{
							combatCharacter = base.CombatChar;
						}
					}
					else
					{
						combatCharacter = base.CurrEnemyChar;
					}
					if (!true)
					{
					}
					CombatCharacter target = combatCharacter;
					bool flag3 = target == null;
					if (flag3)
					{
						short predefinedLogId = 7;
						object arg = this.Id;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
						defaultInterpolatedStringHandler.AppendLiteral("change to fatal by damage type ");
						defaultInterpolatedStringHandler.AppendFormatted<EDamageType>(damageType);
						PredefinedLog.Show(predefinedLogId, arg, defaultInterpolatedStringHandler.ToStringAndClear());
						target = base.CurrEnemyChar;
					}
					DataContext context = base.CombatChar.GetDataContext();
					sbyte bodyPart = (sbyte)dataKey.CustomParam2;
					int count = DomainManager.Combat.AddFatalDamageValue(context, target, (int)Math.Clamp(dataValue, 0L, 2147483647L), inner ? 1 : 0, bodyPart, base.SkillTemplateId, EDamageType.None);
					base.ShowSpecialEffectTipsOnceInFrame(0);
					bool flag4 = target != base.CombatChar;
					if (flag4)
					{
						this._fatalMarkCount += count;
					}
					result = 0L;
				}
			}
			return result;
		}

		// Token: 0x04001413 RID: 5139
		private int _fatalMarkCount;
	}
}
