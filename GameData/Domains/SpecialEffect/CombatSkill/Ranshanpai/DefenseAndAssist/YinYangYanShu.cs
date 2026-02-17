using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.DefenseAndAssist
{
	// Token: 0x02000469 RID: 1129
	public class YinYangYanShu : AssistSkillBase
	{
		// Token: 0x06003B1C RID: 15132 RVA: 0x00246A84 File Offset: 0x00244C84
		public YinYangYanShu()
		{
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x00246A8E File Offset: 0x00244C8E
		public YinYangYanShu(CombatSkillKey skillKey) : base(skillKey, 7602)
		{
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x00246A9E File Offset: 0x00244C9E
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x00246ACD File Offset: 0x00244CCD
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x00246AFC File Offset: 0x00244CFC
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || !base.CanAffect;
			if (!flag)
			{
				Character enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetCharacter();
				short selfSpeed = this.CharObj.GetCastSpeed();
				short enemySpeed = enemyChar.GetCastSpeed();
				bool flag2 = !(base.IsDirect ? (selfSpeed > enemySpeed) : (selfSpeed < enemySpeed));
				if (!flag2)
				{
					this._reduceSpeed = -2 * Math.Abs((int)(selfSpeed - enemySpeed));
					bool flag3 = this.AffectDatas == null || this.AffectDatas.Count == 0;
					if (flag3)
					{
						base.AppendAffectedData(context, enemyChar.GetId(), 11, EDataModifyType.Add, -1);
					}
					base.ShowEffectTips(context);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x00246BCC File Offset: 0x00244DCC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				base.ClearAffectedData(context);
			}
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x00246BF4 File Offset: 0x00244DF4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 11;
			int result;
			if (flag)
			{
				result = this._reduceSpeed;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04001151 RID: 4433
		private const sbyte ReduceSpeedUnit = -2;

		// Token: 0x04001152 RID: 4434
		private int _reduceSpeed;
	}
}
