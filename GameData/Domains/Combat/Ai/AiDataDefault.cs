using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x0200071A RID: 1818
	public class AiDataDefault : AiData
	{
		// Token: 0x06006887 RID: 26759 RVA: 0x003B72F8 File Offset: 0x003B54F8
		private static IEnumerable<IAiNode> NewNodes()
		{
			yield return AiNodeFactory.Create(EAiNodeType.Branch, 0, new List<int>
			{
				0,
				1,
				-1
			});
			yield return AiNodeFactory.Create(EAiNodeType.Action, 1, new List<int>
			{
				0
			});
			yield break;
		}

		// Token: 0x06006888 RID: 26760 RVA: 0x003B7301 File Offset: 0x003B5501
		private static IEnumerable<IAiCondition> NewConditions()
		{
			yield return AiConditionFactory.Create(EAiConditionType.Delay, 0, null, new List<int>
			{
				300
			});
			yield break;
		}

		// Token: 0x06006889 RID: 26761 RVA: 0x003B730A File Offset: 0x003B550A
		private static IEnumerable<IAiAction> NewActions()
		{
			yield return AiActionFactory.Create(EAiActionType.NormalAttack, 0, null, null);
			yield break;
		}

		// Token: 0x0600688A RID: 26762 RVA: 0x003B7313 File Offset: 0x003B5513
		public AiDataDefault()
		{
			this.Nodes = new List<IAiNode>(AiDataDefault.NewNodes());
			this.Conditions = new List<IAiCondition>(AiDataDefault.NewConditions());
			this.Actions = new List<IAiAction>(AiDataDefault.NewActions());
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x0600688B RID: 26763 RVA: 0x003B734D File Offset: 0x003B554D
		protected override IReadOnlyList<IAiNode> Nodes { get; }

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x0600688C RID: 26764 RVA: 0x003B7355 File Offset: 0x003B5555
		protected override IReadOnlyList<IAiCondition> Conditions { get; }

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x0600688D RID: 26765 RVA: 0x003B735D File Offset: 0x003B555D
		protected override IReadOnlyList<IAiAction> Actions { get; }
	}
}
