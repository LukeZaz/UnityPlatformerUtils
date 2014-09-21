using UnityEngine;
using System.Collections;

/*
 * This namespace contains various methods used across all UnityPlatformerUtils scripts.
 * It does nothing by itself.
 */

namespace UnityPlatformerUtils
{
	public static class Compare
	{
		/// <summary>
		/// Tests if a float is within the provided range. Also returns true if the number
		/// was equal to the lower or upper limit.
		/// </summary>
		/// <returns><c>true</c>, if number was within range, <c>false</c> otherwise.</returns>
		/// <param name="num">Number to check.</param>
		/// <param name="first">First part of range. Can be lower or higher than second.</param>
		/// <param name="second">Second part of range. Can be lower or higher than first.</param>
		
		public static bool inRange (float num, float first, float second)
		{
			if (first > second)
			{
				if (num >= second && num <= first)
					return true;
			}
			else
			{
				if (num >= first && num <= second)
					return true;
			}
			return false;
		}
		
		/// <summary>
		/// Tests if a integer is within the provided range. Also returns true if the number
		/// was equal to the lower or upper limit.
		/// </summary>
		/// <returns><c>true</c>, if number was within range, <c>false</c> otherwise.</returns>
		/// <param name="num">Number to check.</param>
		/// <param name="first">First part of range. Can be lower or higher than second.</param>
		/// <param name="second">Second part of range. Can be lower or higher than first.</param>
		
		public static bool inRange (int num, int first, int second)
		{
			if (first > second)
			{
				if (num >= second && num <= first)
					return true;
			}
			else
			{
				if (num >= first && num <= second)
					return true;
			}
			return false;
		}
		
		/// <summary>
		/// Tests if a double is within the provided range. Also returns true if the number
		/// was equal to the lower or upper limit.
		/// </summary>
		/// <returns><c>true</c>, if number was within range, <c>false</c> otherwise.</returns>
		/// <param name="num">Number to check.</param>
		/// <param name="first">First part of range. Can be lower or higher than second.</param>
		/// <param name="second">Second part of range. Can be lower or higher than first.</param>
		
		public static bool inRange (double num, double first, double second)
		{
			if (first > second)
			{
				if (num >= second && num <= first)
					return true;
			}
			else
			{
				if (num >= first && num <= second)
					return true;
			}
			return false;
		}
	}
}
