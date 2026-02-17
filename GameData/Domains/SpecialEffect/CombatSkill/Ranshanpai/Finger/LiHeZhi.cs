using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Finger
{
	// Token: 0x0200045E RID: 1118
	public class LiHeZhi : CombatSkillEffectBase
	{
		// Token: 0x06003ADD RID: 15069 RVA: 0x00245919 File Offset: 0x00243B19
		public LiHeZhi()
		{
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x00245923 File Offset: 0x00243B23
		public LiHeZhi(CombatSkillKey skillKey) : base(skillKey, 7104, -1)
		{
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x00245934 File Offset: 0x00243B34
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x00245949 File Offset: 0x00243B49
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x00245960 File Offset: 0x00243B60
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				this.DoAffect(context);
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x002459A8 File Offset: 0x00243BA8
		private void DoAffect(DataContext context)
		{
			int baseOdds = base.IsDirect ? this.CalcDirectOdds(false) : this.CalcReverseOdds(false);
			bool flag = !context.Random.CheckPercentProb(baseOdds);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				DomainManager.Combat.AddGoneMadInjury(context, enemyChar, base.SkillTemplateId, 0);
				base.ShowSpecialEffectTips(0);
				int extraOdds = base.IsDirect ? this.CalcDirectOdds(true) : this.CalcReverseOdds(true);
				bool flag2 = !context.Random.CheckPercentProb(extraOdds);
				if (!flag2)
				{
					List<short> pool = ObjectPool<List<short>>.Instance.Get();
					pool.Clear();
					pool.AddRange(enemyChar.GetBannedSkillIds(false));
					foreach (short skillId in RandomUtils.GetRandomUnrepeated<short>(context.Random, 2, pool, null))
					{
						DomainManager.Combat.AddGoneMadInjury(context, enemyChar, skillId, 0);
					}
					bool flag3 = pool.Count > 0;
					if (flag3)
					{
						base.ShowSpecialEffectTips(1);
					}
					ObjectPool<List<short>>.Instance.Return(pool);
				}
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06003AE3 RID: 15075 RVA: 0x00245AF0 File Offset: 0x00243CF0
		private IEnumerable<CombatCharacter> CurrChars
		{
			get
			{
				yield return DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				yield return base.CombatChar;
				yield break;
			}
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x00245B10 File Offset: 0x00243D10
		private int CalcDirectOdds(bool isExtra = false)
		{
			return (from character in this.CurrChars
			let ratio = isExtra ? 5 : 10
			select (int)character.GetTrickCount(20) * ratio).Sum();
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x00245B70 File Offset: 0x00243D70
		private int CalcReverseOdds(bool isExtra = false)
		{
			return (from character in this.CurrChars
			select new
			{
				character = character,
				ratio = (isExtra ? 5 : 10)
			}).Select(delegate(<>h__TransparentIdentifier0)
			{
				List<bool> mindMarkList = <>h__TransparentIdentifier0.character.GetDefeatMarkCollection().MindMarkList;
				return ((mindMarkList != null) ? mindMarkList.Count : 0) * <>h__TransparentIdentifier0.ratio;
			}).Sum();
		}

		// Token: 0x0400113D RID: 4413
		private const int DirectBaseOdds = 10;

		// Token: 0x0400113E RID: 4414
		private const int DirectExtraOdds = 5;

		// Token: 0x0400113F RID: 4415
		private const int ReverseBaseOdds = 10;

		// Token: 0x04001140 RID: 4416
		private const int ReverseExtraOdds = 5;

		// Token: 0x04001141 RID: 4417
		private const int MaxExtraCount = 2;
	}
}
