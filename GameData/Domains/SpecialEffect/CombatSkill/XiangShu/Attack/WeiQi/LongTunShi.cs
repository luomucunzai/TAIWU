using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.WeiQi
{
	// Token: 0x020002DC RID: 732
	public class LongTunShi : CombatSkillEffectBase
	{
		// Token: 0x060032E3 RID: 13027 RVA: 0x0022144A File Offset: 0x0021F64A
		public LongTunShi()
		{
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x00221454 File Offset: 0x0021F654
		public LongTunShi(CombatSkillKey skillKey) : base(skillKey, 17050, -1)
		{
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x00221465 File Offset: 0x0021F665
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x0022148C File Offset: 0x0021F68C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			bool isSrcSkillPerformed = this.IsSrcSkillPerformed;
			if (isSrcSkillPerformed)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._injuriesUid, base.DataHandlerKey);
			}
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x002214DC File Offset: 0x0021F6DC
		private unsafe void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				NeiliAllocation neiliAllocation = enemyChar.GetNeiliAllocation();
				List<byte> typeRandomPool = ObjectPool<List<byte>>.Instance.Get();
				typeRandomPool.Clear();
				for (byte type = 0; type < 4; type += 1)
				{
					bool flag2 = *(ref neiliAllocation.Items.FixedElementField + (IntPtr)type * 2) >= 18;
					if (flag2)
					{
						typeRandomPool.Add(type);
					}
				}
				bool flag3 = !this.IsSrcSkillPerformed;
				if (flag3)
				{
					bool flag4 = typeRandomPool.Count > 0 && base.PowerMatchAffectRequire((int)power, 0);
					if (flag4)
					{
						byte type2 = typeRandomPool[context.Random.Next(typeRandomPool.Count)];
						enemyChar.ChangeNeiliAllocation(context, type2, -18, true, true);
						bool flag5 = this.CheckAndHealInjury(context);
						if (flag5)
						{
							base.RemoveSelf(context);
						}
						else
						{
							this.IsSrcSkillPerformed = true;
							base.AddMaxEffectCount(true);
							this._injuriesUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 29U);
							GameDataBridge.AddPostDataModificationHandler(this._injuriesUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnInjuriesChanged));
						}
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag6 = typeRandomPool.Count > 0 && base.PowerMatchAffectRequire((int)power, 0);
					if (flag6)
					{
						base.RemoveSelf(context);
					}
				}
				ObjectPool<List<byte>>.Instance.Return(typeRandomPool);
			}
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x0022167C File Offset: 0x0021F87C
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x002216CC File Offset: 0x0021F8CC
		private void OnInjuriesChanged(DataContext context, DataUid dataUid)
		{
			bool flag = this.CheckAndHealInjury(context);
			if (flag)
			{
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x002216F0 File Offset: 0x0021F8F0
		private bool CheckAndHealInjury(DataContext context)
		{
			Injuries injuries = base.CombatChar.GetInjuries();
			List<sbyte> partRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
			partRandomPool.Clear();
			for (sbyte part = 0; part < 7; part += 1)
			{
				ValueTuple<sbyte, sbyte> injury = injuries.Get(part);
				bool flag = injury.Item1 >= 6 || injury.Item2 >= 6;
				if (flag)
				{
					partRandomPool.Add(part);
				}
			}
			bool affected = partRandomPool.Count > 0;
			bool flag2 = affected;
			if (flag2)
			{
				sbyte part2 = partRandomPool[context.Random.Next(partRandomPool.Count)];
				ValueTuple<sbyte, sbyte> injury2 = injuries.Get(part2);
				bool flag3 = injury2.Item1 > 0;
				if (flag3)
				{
					DomainManager.Combat.RemoveInjury(context, base.CombatChar, part2, false, injury2.Item1, false, false);
				}
				bool flag4 = injury2.Item2 > 0;
				if (flag4)
				{
					DomainManager.Combat.RemoveInjury(context, base.CombatChar, part2, true, injury2.Item2, true, false);
				}
				base.ShowSpecialEffectTips(0);
			}
			ObjectPool<List<sbyte>>.Instance.Return(partRandomPool);
			return affected;
		}

		// Token: 0x04000F0B RID: 3851
		private const sbyte CostNeiliAllocation = 18;

		// Token: 0x04000F0C RID: 3852
		private DataUid _injuriesUid;
	}
}
