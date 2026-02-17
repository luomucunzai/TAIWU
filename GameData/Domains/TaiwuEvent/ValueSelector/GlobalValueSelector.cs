using System;
using System.Collections.Generic;
using CompDevLib.Interpreter;
using Config;

namespace GameData.Domains.TaiwuEvent.ValueSelector
{
	// Token: 0x02000088 RID: 136
	public class GlobalValueSelector : IValueSelector
	{
		// Token: 0x060018E6 RID: 6374 RVA: 0x00167DBC File Offset: 0x00165FBC
		public GlobalValueSelector()
		{
			this._name2IdMap = new Dictionary<string, int>();
			foreach (EventValueItem valueCfg in ((IEnumerable<EventValueItem>)EventValue.Instance))
			{
				bool flag = valueCfg.Type == EEventValueType.Global;
				if (flag)
				{
					this._name2IdMap.Add(valueCfg.Alias, valueCfg.TemplateId);
				}
			}
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x00167E40 File Offset: 0x00166040
		public ValueInfo SelectValue(Evaluator evaluator, string identifier)
		{
			int id;
			return this._name2IdMap.TryGetValue(identifier, out id) ? this.SelectValue(evaluator, id) : ValueInfo.Void;
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x00167E74 File Offset: 0x00166074
		private ValueInfo SelectValue(Evaluator evaluator, int templateId)
		{
			if (!true)
			{
			}
			ValueInfo result;
			if (templateId <= 21)
			{
				if (templateId >= 7)
				{
					result = evaluator.PushEvaluationResult(DomainManager.Organization.GetSettlementByOrgTemplateId((sbyte)(templateId - 7 + 1)));
					goto IL_113;
				}
				switch (templateId)
				{
				case 0:
					result = evaluator.PushEvaluationResult(DomainManager.Taiwu.GetTaiwu());
					goto IL_113;
				case 1:
					result = evaluator.PushEvaluationResult((int)DomainManager.World.GetXiangshuLevel());
					goto IL_113;
				case 2:
					result = evaluator.PushEvaluationResult((int)DomainManager.World.GetCurrMonthInYear());
					goto IL_113;
				case 3:
					result = evaluator.PushEvaluationResult((int)DomainManager.World.GetCurrYear());
					goto IL_113;
				case 4:
					result = evaluator.PushEvaluationResult(DomainManager.World.GetLeftDaysInCurrMonth());
					goto IL_113;
				case 5:
					result = evaluator.PushEvaluationResult(DomainManager.World.GetLeftDaysInCurrMonth());
					goto IL_113;
				}
			}
			else if (templateId <= 37)
			{
				if (templateId != 22)
				{
					result = evaluator.PushEvaluationResult(DomainManager.Organization.GetSettlementByOrgTemplateId((sbyte)(templateId - 37 + 35)));
					goto IL_113;
				}
				result = evaluator.PushEvaluationResult(DomainManager.Taiwu.TaiwuVillage);
				goto IL_113;
			}
			result = ValueInfo.Void;
			IL_113:
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x04000590 RID: 1424
		private readonly Dictionary<string, int> _name2IdMap;
	}
}
