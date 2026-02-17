using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.DefenseAndAssist
{
	// Token: 0x020003AA RID: 938
	public class TianCanShiGu : AssistSkillBase
	{
		// Token: 0x060036C0 RID: 14016 RVA: 0x00231D36 File Offset: 0x0022FF36
		public TianCanShiGu()
		{
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x00231D4B File Offset: 0x0022FF4B
		public TianCanShiGu(CombatSkillKey skillKey) : base(skillKey, 12806)
		{
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x00231D66 File Offset: 0x0022FF66
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x00231D98 File Offset: 0x0022FF98
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._eatingItemsUid, base.DataHandlerKey);
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			bool isCurrCombatChar = this._isCurrCombatChar;
			if (isCurrCombatChar)
			{
				Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x00231DF0 File Offset: 0x0022FFF0
		private void OnCombatBegin(DataContext context)
		{
			this._isCurrCombatChar = DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar);
			bool isCurrCombatChar = this._isCurrCombatChar;
			if (isCurrCombatChar)
			{
				Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
			this._eatingItemsUid = new DataUid(4, 0, (ulong)((long)(base.IsDirect ? base.CurrEnemyChar.GetId() : base.CharacterId)), 59U);
			GameDataBridge.AddPostDataModificationHandler(this._eatingItemsUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnUpdateEatingItems));
			this.OnUpdateEatingItems(context, default(DataUid));
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x00231E9C File Offset: 0x0023009C
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (flag)
			{
				bool isCurrChar = DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar);
				bool flag2 = this._isCurrCombatChar == isCurrChar;
				if (!flag2)
				{
					this._isCurrCombatChar = isCurrChar;
					bool flag3 = isCurrChar;
					if (flag3)
					{
						Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
					}
					else
					{
						Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
					}
				}
			}
			else
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					this.UpdateEnemyUid(context);
				}
			}
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x00231F28 File Offset: 0x00230128
		private unsafe void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = base.CombatChar != combatChar || DomainManager.Combat.Pause;
			if (!flag)
			{
				bool affected = false;
				foreach (short key in this._wugFrameDict.Keys)
				{
					int frame = this._wugFrameDict[key] + 1;
					bool flag2 = frame >= 300;
					if (flag2)
					{
						frame = 0;
						bool canAffect = base.CanAffect;
						if (canAffect)
						{
							CombatCharacter affectChar = base.IsDirect ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false) : base.CombatChar;
							bool isDirect = base.IsDirect;
							if (isDirect)
							{
								NeiliAllocation neiliAllocation = affectChar.GetNeiliAllocation();
								List<byte> typeRandomPool = ObjectPool<List<byte>>.Instance.Get();
								typeRandomPool.Clear();
								for (byte type = 0; type < 4; type += 1)
								{
									bool flag3 = *(ref neiliAllocation.Items.FixedElementField + (IntPtr)type * 2) > 0;
									if (flag3)
									{
										typeRandomPool.Add(type);
									}
								}
								bool flag4 = typeRandomPool.Count > 0;
								if (flag4)
								{
									affectChar.ChangeNeiliAllocation(context, typeRandomPool[context.Random.Next(0, typeRandomPool.Count)], -3, true, true);
									affected = true;
								}
								ObjectPool<List<byte>>.Instance.Return(typeRandomPool);
							}
							else
							{
								affectChar.ChangeNeiliAllocation(context, (byte)context.Random.Next(0, 4), 3, true, true);
								affected = true;
							}
						}
					}
					this._wugFrameDict[key] = frame;
				}
				bool flag5 = affected;
				if (flag5)
				{
					base.ShowEffectTips(context);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x00232114 File Offset: 0x00230314
		private unsafe void OnUpdateEatingItems(DataContext context, DataUid dataUid)
		{
			Character affectChar = base.IsDirect ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetCharacter() : this.CharObj;
			EatingItems eatingItems = *affectChar.GetEatingItems();
			List<short> wugList = ObjectPool<List<short>>.Instance.Get();
			List<short> removeList = ObjectPool<List<short>>.Instance.Get();
			wugList.Clear();
			for (int i = 0; i < 9; i++)
			{
				ItemKey itemKey = (ItemKey)(*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)i * 8));
				bool flag = EatingItems.IsWug(itemKey);
				if (flag)
				{
					wugList.Add(itemKey.TemplateId);
				}
			}
			removeList.Clear();
			removeList.AddRange(this._wugFrameDict.Keys);
			removeList.RemoveAll((short id) => wugList.Contains(id));
			for (int j = 0; j < removeList.Count; j++)
			{
				this._wugFrameDict.Remove(removeList[j]);
			}
			for (int k = 0; k < wugList.Count; k++)
			{
				short wugId = wugList[k];
				bool flag2 = !this._wugFrameDict.ContainsKey(wugId);
				if (flag2)
				{
					this._wugFrameDict.Add(wugId, 0);
				}
			}
			ObjectPool<List<short>>.Instance.Return(wugList);
			ObjectPool<List<short>>.Instance.Return(removeList);
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x002322AC File Offset: 0x002304AC
		private void UpdateEnemyUid(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._eatingItemsUid, base.DataHandlerKey);
			this._eatingItemsUid = new DataUid(4, 0, (ulong)((long)DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetId()), 59U);
			GameDataBridge.AddPostDataModificationHandler(this._eatingItemsUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnUpdateEatingItems));
			this._wugFrameDict.Clear();
			this.OnUpdateEatingItems(context, default(DataUid));
		}

		// Token: 0x04000FF4 RID: 4084
		private const short EffectRequireFrame = 300;

		// Token: 0x04000FF5 RID: 4085
		private const short ChangeNeiliAllocationValue = 3;

		// Token: 0x04000FF6 RID: 4086
		private bool _isCurrCombatChar;

		// Token: 0x04000FF7 RID: 4087
		private DataUid _eatingItemsUid;

		// Token: 0x04000FF8 RID: 4088
		private readonly Dictionary<short, int> _wugFrameDict = new Dictionary<short, int>();
	}
}
