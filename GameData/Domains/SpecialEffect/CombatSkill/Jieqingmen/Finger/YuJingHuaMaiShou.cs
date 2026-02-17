using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Finger
{
	// Token: 0x020004F9 RID: 1273
	public class YuJingHuaMaiShou : CombatSkillEffectBase
	{
		// Token: 0x06003E55 RID: 15957 RVA: 0x0025561B File Offset: 0x0025381B
		public YuJingHuaMaiShou()
		{
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x00255625 File Offset: 0x00253825
		public YuJingHuaMaiShou(CombatSkillKey skillKey) : base(skillKey, 13102, -1)
		{
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x00255636 File Offset: 0x00253836
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x0025564B File Offset: 0x0025384B
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x00255660 File Offset: 0x00253860
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					this.DoAffect(context);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x002556AC File Offset: 0x002538AC
		private unsafe void DoAffect(DataContext context)
		{
			byte type = this.RandomNeiliAllocationType(context);
			bool flag = type == byte.MaxValue;
			if (!flag)
			{
				bool flag2 = !base.CombatChar.AbsorbNeiliAllocation(context, base.CurrEnemyChar, type, 3);
				if (!flag2)
				{
					base.ShowSpecialEffectTips(0);
					NeiliAllocation selfNeiliAllocation = base.CombatChar.GetNeiliAllocation();
					NeiliAllocation enemyNeiliAllocation = base.CurrEnemyChar.GetNeiliAllocation();
					NeiliAllocation selfOriginNeiliAllocation = base.CombatChar.GetOriginNeiliAllocation();
					NeiliAllocation enemyOriginNeiliAllocation = base.CurrEnemyChar.GetOriginNeiliAllocation();
					bool selfLessOrigin = *selfNeiliAllocation[(int)type] < *selfOriginNeiliAllocation[(int)type];
					bool enemyMoreOrigin = *enemyNeiliAllocation[(int)type] > *enemyOriginNeiliAllocation[(int)type];
					bool flag3 = base.IsDirect ? (!selfLessOrigin) : (!enemyMoreOrigin);
					if (!flag3)
					{
						base.CombatChar.AbsorbNeiliAllocation(context, base.CurrEnemyChar, type, 3);
						base.CurrEnemyChar.ChangeNeiliAllocation(context, type, -3, true, true);
						base.ShowSpecialEffectTips(1);
					}
				}
			}
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x002557A8 File Offset: 0x002539A8
		private unsafe byte RandomNeiliAllocationType(DataContext context)
		{
			NeiliAllocation selfNeiliAllocation = base.CombatChar.GetNeiliAllocation();
			NeiliAllocation enemyNeiliAllocation = base.CurrEnemyChar.GetNeiliAllocation();
			List<byte> pool = ObjectPool<List<byte>>.Instance.Get();
			short value = base.IsDirect ? short.MaxValue : 0;
			pool.Clear();
			for (byte type = 0; type < 4; type += 1)
			{
				bool flag = base.IsDirect ? (*selfNeiliAllocation[(int)type] < value) : (*enemyNeiliAllocation[(int)type] > value);
				if (flag)
				{
					pool.Clear();
					pool.Add(type);
					value = (base.IsDirect ? (*selfNeiliAllocation[(int)type]) : (*enemyNeiliAllocation[(int)type]));
				}
				else
				{
					bool flag2 = base.IsDirect ? (*selfNeiliAllocation[(int)type] == value) : (*enemyNeiliAllocation[(int)type] == value);
					if (flag2)
					{
						pool.Add(type);
					}
				}
			}
			byte neiliAllocationType = (pool.Count > 0) ? pool[context.Random.Next(0, pool.Count)] : byte.MaxValue;
			ObjectPool<List<byte>>.Instance.Return(pool);
			return neiliAllocationType;
		}

		// Token: 0x04001263 RID: 4707
		private const sbyte BaseNeiliAllocation = 3;

		// Token: 0x04001264 RID: 4708
		private const sbyte ExtraNeiliAllocation = 3;

		// Token: 0x04001265 RID: 4709
		private const sbyte ExtraCostNeiliAllocation = 3;
	}
}
