using System.Collections.Generic;
using CompDevLib.Interpreter;
using Config;

namespace GameData.Domains.TaiwuEvent.ValueSelector;

public class GlobalValueSelector : IValueSelector
{
	private readonly Dictionary<string, int> _name2IdMap;

	public GlobalValueSelector()
	{
		_name2IdMap = new Dictionary<string, int>();
		foreach (EventValueItem item in (IEnumerable<EventValueItem>)EventValue.Instance)
		{
			if (item.Type == EEventValueType.Global)
			{
				_name2IdMap.Add(item.Alias, item.TemplateId);
			}
		}
	}

	public ValueInfo SelectValue(Evaluator evaluator, string identifier)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		int value;
		return _name2IdMap.TryGetValue(identifier, out value) ? SelectValue(evaluator, value) : ValueInfo.Void;
	}

	private ValueInfo SelectValue(Evaluator evaluator, int templateId)
	{
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		if (1 == 0)
		{
		}
		ValueInfo result;
		switch (templateId)
		{
		case 0:
			result = evaluator.PushEvaluationResult((object)DomainManager.Taiwu.GetTaiwu());
			break;
		case 1:
			result = evaluator.PushEvaluationResult((int)DomainManager.World.GetXiangshuLevel());
			break;
		case 2:
			result = evaluator.PushEvaluationResult((int)DomainManager.World.GetCurrMonthInYear());
			break;
		case 3:
			result = evaluator.PushEvaluationResult((int)DomainManager.World.GetCurrYear());
			break;
		case 4:
			result = evaluator.PushEvaluationResult(DomainManager.World.GetLeftDaysInCurrMonth());
			break;
		case 5:
			result = evaluator.PushEvaluationResult(DomainManager.World.GetLeftDaysInCurrMonth());
			break;
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
			result = evaluator.PushEvaluationResult((object)DomainManager.Organization.GetSettlementByOrgTemplateId((sbyte)(templateId - 7 + 1)));
			break;
		case 22:
			result = evaluator.PushEvaluationResult((object)DomainManager.Taiwu.TaiwuVillage);
			break;
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
		case 30:
		case 31:
		case 32:
		case 33:
		case 34:
		case 35:
		case 36:
		case 37:
			result = evaluator.PushEvaluationResult((object)DomainManager.Organization.GetSettlementByOrgTemplateId((sbyte)(templateId - 37 + 35)));
			break;
		default:
			result = ValueInfo.Void;
			break;
		}
		if (1 == 0)
		{
		}
		return result;
	}
}
