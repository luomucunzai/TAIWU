using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Agile
{
	// Token: 0x02000534 RID: 1332
	public class FuLongFenYue : CheckHitEffect
	{
		// Token: 0x06003F9E RID: 16286 RVA: 0x0025AAFF File Offset: 0x00258CFF
		public FuLongFenYue()
		{
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x0025AB09 File Offset: 0x00258D09
		public FuLongFenYue(CombatSkillKey skillKey) : base(skillKey, 14403)
		{
			this.CheckHitType = 0;
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x0025AB20 File Offset: 0x00258D20
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affecting = false;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 69 : 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x0025AB96 File Offset: 0x00258D96
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x0025ABC8 File Offset: 0x00258DC8
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = base.CombatChar != (base.IsDirect ? attacker : defender);
			if (!flag)
			{
				this._affecting = false;
			}
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x0025ABFC File Offset: 0x00258DFC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !(base.IsDirect ? (charId == base.CharacterId) : (isAlly != base.CombatChar.IsAlly)) || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				this._affecting = false;
			}
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x0025AC58 File Offset: 0x00258E58
		protected override bool HitEffect(DataContext context)
		{
			this._affecting = true;
			return true;
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x0025AC74 File Offset: 0x00258E74
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this._affecting;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = (base.IsDirect ? 15 : -30);
			}
			return result;
		}

		// Token: 0x040012BC RID: 4796
		private const sbyte AddDamagePercent = 15;

		// Token: 0x040012BD RID: 4797
		private const sbyte ReduceDamagePercent = -30;

		// Token: 0x040012BE RID: 4798
		private bool _affecting;
	}
}
