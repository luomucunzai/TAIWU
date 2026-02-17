using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Whip
{
	// Token: 0x020003B6 RID: 950
	public class LaoJunFuChenGong : CombatSkillEffectBase
	{
		// Token: 0x06003709 RID: 14089 RVA: 0x00233584 File Offset: 0x00231784
		public LaoJunFuChenGong()
		{
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x00233599 File Offset: 0x00231799
		public LaoJunFuChenGong(CombatSkillKey skillKey) : base(skillKey, 4303, -1)
		{
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x002335B8 File Offset: 0x002317B8
		public override void OnEnable(DataContext context)
		{
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 6, base.MaxEffectCount, false);
			this._defeatMarkUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 50U);
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x00233638 File Offset: 0x00231838
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey);
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x00233660 File Offset: 0x00231860
		private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
		{
			bool flag = base.EffectCount <= 0 || !DomainManager.Combat.IsCharacterHalfFallen(base.CombatChar);
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					Injuries injuries = base.CombatChar.GetInjuries();
					Injuries newInjuries = injuries.Subtract(base.CombatChar.GetOldInjuries());
					List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
					bodyPartRandomPool.Clear();
					for (sbyte part = 0; part < 7; part += 1)
					{
						ValueTuple<sbyte, sbyte> markCount = newInjuries.Get(part);
						for (int i = 0; i < (int)(markCount.Item1 + markCount.Item2); i++)
						{
							bodyPartRandomPool.Add(part);
						}
					}
					bool flag2 = bodyPartRandomPool.Count > 0;
					if (flag2)
					{
						int removeCount = Math.Min(base.EffectCount, bodyPartRandomPool.Count);
						for (int j = 0; j < removeCount; j++)
						{
							sbyte part2 = bodyPartRandomPool[context.Random.Next(0, bodyPartRandomPool.Count)];
							bool isInner = newInjuries.Get(part2, true) > 0 && (newInjuries.Get(part2, false) <= 0 || context.Random.CheckPercentProb(50));
							injuries.Change(part2, isInner, -1);
							newInjuries.Change(part2, isInner, -1);
						}
						DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries, true, true);
						DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), (short)(-(short)removeCount), true, false);
						base.ShowSpecialEffectTips(0);
					}
				}
				else
				{
					DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
					this._markRandomPool.Clear();
					for (sbyte part3 = 0; part3 < 7; part3 += 1)
					{
						for (int k = 0; k < markCollection.FlawMarkList[(int)part3].Count; k++)
						{
							this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(0, part3));
						}
						for (int l = 0; l < markCollection.AcupointMarkList[(int)part3].Count; l++)
						{
							this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(1, part3));
						}
					}
					for (int m = 0; m < markCollection.MindMarkList.Count; m++)
					{
						this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(2, -1));
					}
					bool flag3 = this._markRandomPool.Count > 0;
					if (flag3)
					{
						int removeCount2 = Math.Min(base.EffectCount, this._markRandomPool.Count);
						for (int n = 0; n < removeCount2; n++)
						{
							int index = context.Random.Next(0, this._markRandomPool.Count);
							ValueTuple<sbyte, sbyte> mark = this._markRandomPool[index];
							bool flag4 = mark.Item1 == 0;
							if (flag4)
							{
								DomainManager.Combat.RemoveFlaw(context, base.CombatChar, mark.Item2, context.Random.Next(0, (int)base.CombatChar.GetFlawCount()[(int)mark.Item2]), true, true);
							}
							else
							{
								bool flag5 = mark.Item1 == 1;
								if (flag5)
								{
									DomainManager.Combat.RemoveAcupoint(context, base.CombatChar, mark.Item2, context.Random.Next(0, (int)base.CombatChar.GetAcupointCount()[(int)mark.Item2]), true, true);
								}
								else
								{
									DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, 1, true, 0);
								}
							}
							this._markRandomPool.RemoveAt(index);
						}
						DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), (short)(-(short)removeCount2), true, false);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x00233A5C File Offset: 0x00231C5C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.PowerMatchAffectRequire((int)power, 0) || base.EffectCount >= (int)base.MaxEffectCount;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 1, true, false);
			}
		}

		// Token: 0x0400100E RID: 4110
		private const sbyte InitEffectCount = 6;

		// Token: 0x0400100F RID: 4111
		private DataUid _defeatMarkUid;

		// Token: 0x04001010 RID: 4112
		private readonly List<ValueTuple<sbyte, sbyte>> _markRandomPool = new List<ValueTuple<sbyte, sbyte>>();
	}
}
