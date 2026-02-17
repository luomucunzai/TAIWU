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
	// Token: 0x020002E0 RID: 736
	public class SheShenShi : CombatSkillEffectBase
	{
		// Token: 0x06003303 RID: 13059 RVA: 0x002227A9 File Offset: 0x002209A9
		public SheShenShi()
		{
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x002227B3 File Offset: 0x002209B3
		public SheShenShi(CombatSkillKey skillKey) : base(skillKey, 17053, -1)
		{
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x002227C4 File Offset: 0x002209C4
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x00222800 File Offset: 0x00220A00
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			bool isSrcSkillPerformed = this.IsSrcSkillPerformed;
			if (isSrcSkillPerformed)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._enemyInjuriesUid, base.DataHandlerKey);
			}
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x00222860 File Offset: 0x00220A60
		private unsafe void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || interrupted;
			if (!flag)
			{
				NeiliAllocation neiliAllocation = base.CombatChar.GetNeiliAllocation();
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
					bool flag4 = typeRandomPool.Count > 0;
					if (flag4)
					{
						byte type2 = typeRandomPool[context.Random.Next(typeRandomPool.Count)];
						base.CombatChar.ChangeNeiliAllocation(context, type2, -18, true, true);
						bool flag5 = this.CheckAndAddNeiliAllocation(context);
						if (flag5)
						{
							base.RemoveSelf(context);
						}
						else
						{
							this.IsSrcSkillPerformed = true;
							base.AddMaxEffectCount(true);
							this.UpdateEnemyUid(true);
						}
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag6 = typeRandomPool.Count > 0;
					if (flag6)
					{
						base.RemoveSelf(context);
					}
				}
				ObjectPool<List<byte>>.Instance.Return(typeRandomPool);
			}
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x002229A0 File Offset: 0x00220BA0
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = !this.IsSrcSkillPerformed || isAlly == base.CombatChar.IsAlly;
			if (!flag)
			{
				this.UpdateEnemyUid(false);
			}
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x002229D8 File Offset: 0x00220BD8
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x00222A28 File Offset: 0x00220C28
		private void OnEnemyInjuriesChanged(DataContext context, DataUid dataUid)
		{
			bool flag = this.CheckAndAddNeiliAllocation(context);
			if (flag)
			{
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x00222A4C File Offset: 0x00220C4C
		private void UpdateEnemyUid(bool init)
		{
			CombatCharacter currEnemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			bool flag = !init;
			if (flag)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._enemyInjuriesUid, base.DataHandlerKey);
			}
			this._enemyInjuriesUid = new DataUid(8, 10, (ulong)((long)currEnemy.GetId()), 29U);
			GameDataBridge.AddPostDataModificationHandler(this._enemyInjuriesUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnEnemyInjuriesChanged));
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x00222AC4 File Offset: 0x00220CC4
		private bool CheckAndAddNeiliAllocation(DataContext context)
		{
			Injuries enemyInjuries = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetInjuries();
			bool affected = false;
			for (sbyte part = 0; part < 7; part += 1)
			{
				ValueTuple<sbyte, sbyte> injury = enemyInjuries.Get(part);
				bool flag = injury.Item1 >= 6 || injury.Item2 >= 6;
				if (flag)
				{
					affected = true;
					break;
				}
			}
			bool flag2 = affected;
			if (flag2)
			{
				for (byte type = 0; type < 4; type += 1)
				{
					base.CombatChar.ChangeNeiliAllocation(context, type, 18, true, true);
				}
				base.ShowSpecialEffectTips(0);
			}
			return affected;
		}

		// Token: 0x04000F16 RID: 3862
		private const sbyte NeiliAllocationValue = 18;

		// Token: 0x04000F17 RID: 3863
		private DataUid _enemyInjuriesUid;
	}
}
