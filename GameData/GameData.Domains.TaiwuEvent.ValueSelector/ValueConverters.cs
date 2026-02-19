using CompDevLib.Interpreter;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.ValueSelector;

public class ValueConverters
{
	[ValueConverter(typeof(int), typeof(GameData.Domains.Character.Character))]
	public static ValueInfo CharIdToCharacter(Evaluator evaluator, ValueInfo valueInfo)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		int objectId = evaluator.EvaluationStack.PopUnmanaged<int>();
		DomainManager.Character.TryGetElement_Objects(objectId, out var element);
		return evaluator.PushEvaluationResult((object)element);
	}

	[ValueConverter(typeof(int), typeof(Settlement))]
	public static ValueInfo SettlementIdToSettlement(Evaluator evaluator, ValueInfo valueInfo)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		int num = evaluator.EvaluationStack.PopUnmanaged<int>();
		Settlement settlement = DomainManager.Organization.GetSettlement((short)num);
		return evaluator.PushEvaluationResult((object)settlement);
	}

	[ValueConverter(typeof(int), typeof(MapAreaData))]
	public static ValueInfo AreaTemplateIdToMapAreaData(Evaluator evaluator, ValueInfo valueInfo)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		int num = evaluator.EvaluationStack.PopUnmanaged<int>();
		short areaIdByAreaTemplateId = DomainManager.Map.GetAreaIdByAreaTemplateId((short)num);
		MapAreaData areaByAreaId = DomainManager.Map.GetAreaByAreaId(areaIdByAreaTemplateId);
		return evaluator.PushEvaluationResult((object)areaByAreaId);
	}

	[ValueConverter(typeof(int), typeof(UnmanagedVariant<TemplateKey>))]
	public static ValueInfo UIntToTemplateKey(Evaluator evaluator, ValueInfo valueInfo)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		int num = evaluator.EvaluationStack.PopUnmanaged<int>();
		TemplateKey templateKey = (TemplateKey)(uint)num;
		return evaluator.PushEvaluationResult((object)new UnmanagedVariant<TemplateKey>(templateKey));
	}

	[ValueConverter(typeof(Location), typeof(MapBlockData))]
	public static ValueInfo LocationToMapBlock(Evaluator evaluator, ValueInfo valueInfo)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Location key = (Location)(object)evaluator.EvaluationStack.PopObject<ISerializableGameData>();
		MapBlockData block = DomainManager.Map.GetBlock(key);
		return evaluator.PushEvaluationResult((object)block);
	}
}
