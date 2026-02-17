using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character.Relation;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Music
{
	// Token: 0x02000270 RID: 624
	public class XiangNvQiCangWu : CombatSkillEffectBase
	{
		// Token: 0x06003092 RID: 12434 RVA: 0x00217B5F File Offset: 0x00215D5F
		public XiangNvQiCangWu()
		{
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x00217B71 File Offset: 0x00215D71
		public XiangNvQiCangWu(CombatSkillKey skillKey) : base(skillKey, 8304, -1)
		{
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x00217B8C File Offset: 0x00215D8C
		public override void OnEnable(DataContext context)
		{
			int marryCharId = base.IsDirect ? base.CurrEnemyChar.GetId() : base.CharacterId;
			HashSet<int> marryChars = DomainManager.Character.GetRelatedCharIds(marryCharId, 1024);
			short minFavor = short.MaxValue;
			foreach (int charId in marryChars)
			{
				bool flag = !DomainManager.Character.IsCharacterAlive(charId);
				if (flag)
				{
					short favor = DomainManager.Character.GetFavorability(marryCharId, charId);
					bool flag2 = favor < minFavor;
					if (flag2)
					{
						minFavor = favor;
					}
				}
			}
			bool flag3 = minFavor < short.MaxValue;
			if (flag3)
			{
				sbyte favorType = FavorabilityType.GetFavorabilityType(minFavor);
				this._powerChangeValue = (int)((favorType <= 0) ? this.LowFavorReducePower : XiangNvQiCangWu.PowerChangeValue[(int)(favorType - 1)]);
				bool flag4 = this._powerChangeValue != 0;
				if (flag4)
				{
					this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
					this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
					base.ShowSpecialEffectTips(this._powerChangeValue > 0, 0, 1);
				}
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x00217CDC File Offset: 0x00215EDC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x00217CF4 File Offset: 0x00215EF4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x00217D2C File Offset: 0x00215F2C
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
					result = this._powerChangeValue;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E6F RID: 3695
		private static readonly sbyte[] PowerChangeValue = new sbyte[]
		{
			-20,
			0,
			20,
			40,
			60,
			80
		};

		// Token: 0x04000E70 RID: 3696
		private sbyte LowFavorReducePower = -40;

		// Token: 0x04000E71 RID: 3697
		private int _powerChangeValue;
	}
}
