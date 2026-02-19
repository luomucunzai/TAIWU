using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using HarmonyLib.Internal.Patching;
using Mono.Cecil.Cil;

namespace HarmonyLib;

[Serializable]
public class HarmonyException : Exception
{
	private Dictionary<int, CodeInstruction> instructions = new Dictionary<int, CodeInstruction>();

	private int errorOffset = -1;

	internal HarmonyException()
	{
	}

	internal HarmonyException(string message)
		: base(message)
	{
	}

	internal HarmonyException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected HarmonyException(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
		throw new NotImplementedException();
	}

	internal HarmonyException(Exception innerException, Dictionary<int, CodeInstruction> instructions, int errorOffset)
		: base("IL Compile Error", innerException)
	{
		this.instructions = instructions;
		this.errorOffset = errorOffset;
	}

	internal static Exception Create(Exception ex, MethodBody body)
	{
		if (ex is HarmonyException { instructions: { Count: >0 }, errorOffset: >=0 })
		{
			return ex;
		}
		Match match = Regex.Match(ex.Message.TrimEnd(Array.Empty<char>()), "(?:Reason: )?Invalid IL code in.+: IL_(\\d{4}): (.+)$");
		if (!match.Success)
		{
			return new HarmonyException("IL Compile Error (unknown location)", ex);
		}
		Dictionary<int, CodeInstruction> dictionary2 = ILManipulator.GetInstructions(body) ?? new Dictionary<int, CodeInstruction>();
		int num = int.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
		Regex.Replace(match.Groups[2].Value, " {2,}", " ");
		if (ex is HarmonyException ex3)
		{
			if (dictionary2.Count != 0)
			{
				ex3.instructions = dictionary2;
				ex3.errorOffset = num;
			}
			return ex3;
		}
		return new HarmonyException(ex, dictionary2, num);
	}

	public List<KeyValuePair<int, CodeInstruction>> GetInstructionsWithOffsets()
	{
		return instructions.OrderBy((KeyValuePair<int, CodeInstruction> ins) => ins.Key).ToList();
	}

	public List<CodeInstruction> GetInstructions()
	{
		return (from ins in instructions
			orderby ins.Key
			select ins.Value).ToList();
	}

	public int GetErrorOffset()
	{
		return errorOffset;
	}

	public int GetErrorIndex()
	{
		if (instructions.TryGetValue(errorOffset, out var value))
		{
			return GetInstructions().IndexOf(value);
		}
		return -1;
	}
}
