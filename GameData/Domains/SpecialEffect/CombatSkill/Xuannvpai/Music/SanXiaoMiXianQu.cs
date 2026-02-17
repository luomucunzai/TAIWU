using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Music
{
	// Token: 0x0200026E RID: 622
	public class SanXiaoMiXianQu : CombatSkillEffectBase
	{
		// Token: 0x06003086 RID: 12422 RVA: 0x00217784 File Offset: 0x00215984
		public SanXiaoMiXianQu()
		{
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x0021778E File Offset: 0x0021598E
		public SanXiaoMiXianQu(CombatSkillKey skillKey) : base(skillKey, 8305, -1)
		{
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x002177A0 File Offset: 0x002159A0
		public override void OnEnable(DataContext context)
		{
			this._addHitMind = (int)(this.CharObj.GetAttraction() * 20 / 100);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 35, -1, -1, -1, -1), EDataModifyType.Add);
			base.ShowSpecialEffectTips(0);
			Character enemyChar = base.CurrEnemyChar.GetCharacter();
			bool flag = enemyChar.CheckGenderMeetsRequirement(base.IsDirect ? 1 : 0);
			if (flag)
			{
				this._addPower += 20;
				bool flag2 = !enemyChar.HasVirginity();
				if (flag2)
				{
					this._addPower += 20;
				}
			}
			bool flag3 = this._addPower > 0;
			if (flag3)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				base.ShowSpecialEffectTips(1);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x00217894 File Offset: 0x00215A94
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x002178AC File Offset: 0x00215AAC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x002178E4 File Offset: 0x00215AE4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 35;
				if (flag2)
				{
					result = this._addHitMind;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 199 && dataKey.CombatSkillId == base.SkillTemplateId;
					if (flag3)
					{
						result = this._addPower;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000E67 RID: 3687
		private const sbyte CharmAddHitPercent = 20;

		// Token: 0x04000E68 RID: 3688
		private const sbyte GenderAddPower = 20;

		// Token: 0x04000E69 RID: 3689
		private const sbyte VirginAddPower = 20;

		// Token: 0x04000E6A RID: 3690
		private int _addHitMind;

		// Token: 0x04000E6B RID: 3691
		private int _addPower;
	}
}
