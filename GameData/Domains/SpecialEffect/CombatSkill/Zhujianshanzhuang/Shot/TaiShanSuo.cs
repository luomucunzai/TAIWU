using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Shot
{
	// Token: 0x020001BF RID: 447
	public class TaiShanSuo : CombatSkillEffectBase
	{
		// Token: 0x06002CAD RID: 11437 RVA: 0x002087EF File Offset: 0x002069EF
		public TaiShanSuo()
		{
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x00208821 File Offset: 0x00206A21
		public TaiShanSuo(CombatSkillKey skillKey) : base(skillKey, 9405, -1)
		{
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x0020885A File Offset: 0x00206A5A
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x0020886F File Offset: 0x00206A6F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x00208884 File Offset: 0x00206A84
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				byte trickCount = base.CombatChar.GetTrickCount(12);
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && trickCount > 0;
				if (flag2)
				{
					CombatCharacter enemyChar = base.CurrEnemyChar;
					List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
					sbyte[] affectParts = base.IsDirect ? this._directBodyParts : this._reverseBodyParts;
					byte[] acupointCounts = enemyChar.GetAcupointCount();
					int maxCount = enemyChar.GetMaxAcupointCount();
					bodyPartRandomPool.Clear();
					foreach (sbyte bodyPart in affectParts)
					{
						int canAddCount = maxCount - (int)acupointCounts[(int)bodyPart];
						for (int j = 0; j < canAddCount; j++)
						{
							bodyPartRandomPool.Add(bodyPart);
						}
					}
					bool flag3 = bodyPartRandomPool.Count > 0;
					if (flag3)
					{
						int addCount = Math.Min((int)trickCount, bodyPartRandomPool.Count);
						int keepFrames = GlobalConfig.Instance.FlawBaseKeepTime[1];
						for (int k = 0; k < addCount; k++)
						{
							sbyte bodyPart2 = bodyPartRandomPool[context.Random.Next(0, bodyPartRandomPool.Count)];
							bodyPartRandomPool.Remove(bodyPart2);
							enemyChar.AddOrUpdateFlawOrAcupoint(context, bodyPart2, false, 1, true, keepFrames, keepFrames);
						}
						DomainManager.Combat.AddToCheckFallenSet(enemyChar.GetId());
					}
					DomainManager.Combat.RemoveTrick(context, base.CombatChar, 12, trickCount, true, -1);
					base.ShowSpecialEffectTips(0);
					ObjectPool<List<sbyte>>.Instance.Return(bodyPartRandomPool);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000D78 RID: 3448
		private const sbyte AddAcupointLevel = 1;

		// Token: 0x04000D79 RID: 3449
		private readonly sbyte[] _directBodyParts = new sbyte[]
		{
			5,
			6
		};

		// Token: 0x04000D7A RID: 3450
		private readonly sbyte[] _reverseBodyParts = new sbyte[]
		{
			3,
			4
		};
	}
}
