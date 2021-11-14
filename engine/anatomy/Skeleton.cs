using System.Collections.Generic;

namespace FreedomOfFormFoundation.AnatomyEngine.Anatomy
{
	//Suggestion: deprecate this separate class
	public class Skeleton
	{
		public List<Joint> joints;
		public List<Bone> bones;
		
		public Skeleton()
		{
			joints = new List<Joint>();
			bones = new List<Bone>();
		}
	}
}
