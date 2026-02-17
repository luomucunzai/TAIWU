using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Map;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Leg
{
	// Token: 0x0200022F RID: 559
	public class YanWangGuiJiao : PowerUpOnCast
	{
		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06002F79 RID: 12153 RVA: 0x002133D2 File Offset: 0x002115D2
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.AddPercent;
			}
		}

		// Token: 0x06002F7A RID: 12154 RVA: 0x002133D5 File Offset: 0x002115D5
		public YanWangGuiJiao()
		{
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x002133DF File Offset: 0x002115DF
		public YanWangGuiJiao(CombatSkillKey skillKey) : base(skillKey, 15306)
		{
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x002133F0 File Offset: 0x002115F0
		public override void OnEnable(DataContext context)
		{
			GameData.Domains.Character.Character srcFiveElementsChar = (base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false)).GetCharacter();
			sbyte srcFiveElementsType = (sbyte)NeiliType.Instance[srcFiveElementsChar.GetNeiliType()].FiveElements;
			this.PowerUpValue = 0;
			bool flag = srcFiveElementsType != 5 && srcFiveElementsType >= 0;
			if (flag)
			{
				Location location = this.CharObj.GetLocation();
				bool flag2 = !location.IsValid() && base.CharacterId == DomainManager.Taiwu.GetTaiwuCharId();
				if (flag2)
				{
					location = this.CharObj.GetValidLocation();
				}
				bool flag3 = location.IsValid();
				if (flag3)
				{
					this.CalcPowerUp(location, srcFiveElementsType);
				}
			}
			base.OnEnable(context);
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x002134BC File Offset: 0x002116BC
		private void CalcPowerUp(Location location, sbyte srcFiveElementsType)
		{
			MapBlockData block = DomainManager.Map.GetBlock(location.AreaId, location.BlockId);
			List<MapBlockData> viewRangeBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			sbyte requireFiveElements = base.IsDirect ? FiveElementsType.Produced[(int)srcFiveElementsType] : FiveElementsType.Countered[(int)srcFiveElementsType];
			int viewRange = DomainManager.Map.GetTaiwuViewRange(block);
			DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, viewRangeBlocks, viewRange);
			viewRangeBlocks.Add(block);
			for (int i = 0; i < viewRangeBlocks.Count; i++)
			{
				HashSet<int> graveSet = viewRangeBlocks[i].GraveSet;
				bool flag = graveSet != null;
				if (flag)
				{
					foreach (int deadCharId in graveSet)
					{
						sbyte birthMonth = CharacterDomain.CalcBirthYearAndMonth(DomainManager.Character.GetDeadCharacter(deadCharId).BirthDate).Item2;
						bool flag2 = GameData.Domains.Character.SharedMethods.GetInnateFiveElementsType(birthMonth) == requireFiveElements;
						if (flag2)
						{
							this.PowerUpValue += 10;
						}
					}
				}
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(viewRangeBlocks);
		}

		// Token: 0x04000E16 RID: 3606
		private const sbyte AddPowerUnit = 10;
	}
}
