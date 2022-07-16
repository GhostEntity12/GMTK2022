using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DicePool : Pool
{
	public List<Die> ActiveDice => activeItems.Cast<Die>().ToList();
}
