using System;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000865 RID: 2149
	[SerializableGameData(NotForDisplayModule = true)]
	public class SectStoryBaihuaToCureManic : BasePrioritizedAction
	{
		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06007728 RID: 30504 RVA: 0x0045B93A File Offset: 0x00459B3A
		public override short ActionType
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x06007729 RID: 30505 RVA: 0x0045B93E File Offset: 0x00459B3E
		public override void OnStart(DataContext context, Character selfChar)
		{
			DomainManager.World.BaihuaAddCharIdToCureSpecialDebuffIntList(context, this.Target.TargetCharId);
		}

		// Token: 0x0600772A RID: 30506 RVA: 0x0045B958 File Offset: 0x00459B58
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			DomainManager.World.BaihuaRemoveCharIdToCureSpecialDebuffIntList(context, this.Target.TargetCharId);
		}

		// Token: 0x0600772B RID: 30507 RVA: 0x0045B974 File Offset: 0x00459B74
		public override bool Execute(DataContext context, Character selfChar)
		{
			Character targetChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(this.Target.TargetCharId, out targetChar);
			bool result;
			if (flag)
			{
				result = !this.TryChangeTarget(context);
			}
			else
			{
				bool flag2 = !targetChar.IsInteractableAsIntelligentCharacter();
				if (flag2)
				{
					result = !this.TryChangeTarget(context);
				}
				else
				{
					bool flag3 = !targetChar.RemoveFeatureGroup(context, 541);
					if (flag3)
					{
						result = !this.TryChangeTarget(context);
					}
					else
					{
						int currDate = DomainManager.World.GetCurrDate();
						LifeRecordCollection lifeRecord = DomainManager.LifeRecord.GetLifeRecordCollection();
						lifeRecord.AddSectMainStoryBaihuaManiaCure(selfChar.GetId(), currDate, targetChar.GetId(), selfChar.GetLocation());
						result = !this.TryChangeTarget(context);
					}
				}
			}
			return result;
		}

		// Token: 0x0600772C RID: 30508 RVA: 0x0045BA30 File Offset: 0x00459C30
		private bool TryChangeTarget(DataContext context)
		{
			int newTargetCharId;
			bool flag = !DomainManager.Character.BaihuaManicCharIds.TryTake(out newTargetCharId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DomainManager.World.BaihuaRemoveCharIdToCureSpecialDebuffIntList(context, this.Target.TargetCharId);
				this.Target.TargetCharId = newTargetCharId;
				DomainManager.World.BaihuaAddCharIdToCureSpecialDebuffIntList(context, this.Target.TargetCharId);
				result = true;
			}
			return result;
		}
	}
}
