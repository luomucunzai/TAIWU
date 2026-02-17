using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Throw
{
	// Token: 0x0200021E RID: 542
	public class YeGuiDuSha : CombatSkillEffectBase
	{
		// Token: 0x06002F2C RID: 12076 RVA: 0x002120E9 File Offset: 0x002102E9
		public YeGuiDuSha()
		{
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x002120F3 File Offset: 0x002102F3
		public YeGuiDuSha(CombatSkillKey skillKey) : base(skillKey, 15400, -1)
		{
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x00212104 File Offset: 0x00210304
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x00212119 File Offset: 0x00210319
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x00212130 File Offset: 0x00210330
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					CombatCharacter trickChar = base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					TrickCollection tricks = trickChar.GetTricks();
					IReadOnlyDictionary<int, sbyte> trickDict = tricks.Tricks;
					List<int> indexRandomPool = ObjectPool<List<int>>.Instance.Get();
					indexRandomPool.Clear();
					foreach (KeyValuePair<int, sbyte> trickEntry in trickDict)
					{
						bool flag3 = base.IsDirect == trickChar.IsTrickUseless(trickEntry.Value);
						if (flag3)
						{
							indexRandomPool.Add(trickEntry.Key);
						}
					}
					while (indexRandomPool.Count > 2)
					{
						indexRandomPool.RemoveAt(context.Random.Next(0, indexRandomPool.Count));
					}
					bool flag4 = indexRandomPool.Count > 0;
					if (flag4)
					{
						for (int i = 0; i < indexRandomPool.Count; i++)
						{
							tricks.ReplaceTrick(indexRandomPool[i], 14);
						}
						trickChar.SetTricks(tricks, context);
						base.ShowSpecialEffectTips(0);
					}
					ObjectPool<List<int>>.Instance.Return(indexRandomPool);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000E02 RID: 3586
		private const sbyte ChangeTrickCount = 2;
	}
}
