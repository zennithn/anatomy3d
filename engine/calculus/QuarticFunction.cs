﻿/*
 * Copyright (C) 2021 Freedom of Form Foundation, Inc.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License, version 2 (GPLv2) as published by the Free Software Foundation.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License, version 2 (GPLv2) for more details.
 * 
 * You should have received a copy of the GNU General Public License, version 2 (GPLv2)
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System;
using FreedomOfFormFoundation.AnatomyEngine.Geometry;

namespace FreedomOfFormFoundation.AnatomyEngine.Calculus
{
	public class QuarticFunction : ContinuousMap<float, float>
	{
		public float a0 { get; }
		public float a1 { get; }
		public float a2 { get; }
		public float a3 { get; }
		public float a4 { get; }
		
		/// <summary>
		///     A quartic function defined by \f$q(x) = a_0 + a_1 x + a_2 x^2 + a_3 x^3 + a_4 x^4\f$.
		///		See https://en.wikipedia.org/wiki/Quartic_function for more information.
		/// </summary>
		public QuarticFunction(float a0, float a1, float a2, float a3, float a4)
		{
			this.a0 = a0;
			this.a1 = a1;
			this.a2 = a2;
			this.a3 = a3;
			this.a4 = a4;
		}
		
		public float GetAt(float x, uint derivative)
		{
			// Return a different function depending on the derivative level:
			float output = 0.0f;
			switch (derivative)
			{
				case 0:
					output = a0 + a1*x + a2*x*x + a3*x*x*x + a4*x*x*x*x;
					break;
				case 1:
					output = a1 + 2.0f*a2*x + 3.0f*a3*x*x + 4.0f*a4*x*x*x;
					break;
				case 2:
					output = 2.0f*a2 + 6.0f*a3*x + 12.0f*a4*x*x;
					break;
				case 3:
					output = 6.0f*a3 + 24.0f*a4*x;
					break;
				case 4:
					output = 24.0f*a4;
					break;
				default:
					output = 0.0f;
					break;
			}
			
			return output;
		}
		
		/// <inheritdoc />
		public override float GetValueAt(float x)
		{
			return GetAt(x, 0);
		}
		
		public float GetDerivativeAt(float x)
		{
			return GetAt(x, 1);
		}
		
		public List<float> Roots()
		{
			return QuarticFunction.Solve(a0, a1, a2, a3, a4);
		}
		
		/// <summary>
		///     Solves the equation \f$e + d x + c x^2 + b x^3 + a x^4 = 0\f$, returning all real values of
		///		\f$x\f$ for which the equation is true. See https://en.wikipedia.org/wiki/Quartic_function for the
		///		algorithm used.
		/// </summary>
		public static List<float> Solve(float e, float d, float c, float b, float a)
		{
			if(Math.Abs(a) <= 0.005f)
			{
				//return new List<float>{2.0f};
				return CubicFunction.Solve(e, d, c, b);
			}
			
			//if(Math.Min(Math.Min(Math.Abs(b), Math.Abs(c)), Math.Min(Math.Abs(d), Math.Abs(e))) <= 0.015f)
			//{
			//	return new List<float>{2.0f};
			//}
			
			float ba = b/a;
			float ca = c/a;
			float da = d/a;
			float ea = e/a;
			
			List<float> output = new List<float>(4);
			
			Complex p1 = 2.0*c*c*c - 9.0*b*c*d + 27.0*a*d*d + 27.0*b*b*e - 72.0*a*c*e;
			Complex q = c*c - 3.0*b*d + 12.0*a*e;
			Complex p2 = p1 + Complex.Sqrt(-4.0*q*q*q + p1*p1);
			
			Complex pow = Complex.Pow(p2/2.0, (1.0/3.0));
			
			Complex p3 = q/(3.0*a*pow) + pow/(3.0*a);
			
			Complex p4 = Complex.Sqrt(ba*ba/4.0 - 2.0*ca/(3.0) + p3);
			
			Complex p5 = b*b/(2.0*a*a) - 4.0*c/(3.0*a) - p3;
			
			Complex p6 = (-ba*ba*ba + 4.0*ba*ca - 8.0*da)/(4.0*p4);
			
			List<Complex> roots = new List<Complex>
			{
				-ba/(4.0) - p4/2.0 - 0.5*Complex.Sqrt(p5 - p6),
				-ba/(4.0) - p4/2.0 + 0.5*Complex.Sqrt(p5 - p6),
				-ba/(4.0) + p4/2.0 - 0.5*Complex.Sqrt(p5 + p6),
				-ba/(4.0) + p4/2.0 + 0.5*Complex.Sqrt(p5 + p6),
			};
			
			foreach (Complex root in roots)
			{
				if(Math.Abs(root.Imaginary) <= 0.005f)
				{
					output.Add((float)root.Real);
				}
			}
			
			return output;
		}
	}
}
