using System.Collections.Generic;
using System.Numerics;
using FreedomOfFormFoundation.AnatomyEngine.Anatomy;

namespace FreedomOfFormFoundation.AnatomyEngine
{
	public class Character
	{
		
		public SkeletonHumanoid skeleton;
		
		public Character()
		{
			skeleton = new SkeletonHumanoid();
		}


		
	}

	public class SkeletonHumanoid 
	{

		//iterable lists

		public List<Anatomy.Joint> jointsList;
		public List<Anatomy.Bone> bonesList;



		//***Individually accessible objects in some way.***
		//We DO need to have defaults for important bones,
		//however I'm not sure if the actual variable names should be hard-set.
		//could we have a data structure where search keys are like "Humerus_R", "Radius_R", etc?
		public LongBone Humerus_R = new LongBone();
		public LongBone Radius_R = new LongBone();
		public LongBone Ulna_R = new LongBone();

		public Joint Elbow_Joint = new Joint();
		public Joint Radius_Ulna_Interaction = new Joint();


		//default constructor should set up a lot of the basics.
		public SkeletonHumanoid()
		{
			//add the objects to the iterable lists.
			bonesList.Add(Humerus_R);
			bonesList.Add(Radius_R);
			bonesList.Add(Ulna_R);

			jointsList.Add(Elbow_Joint);
			jointsList.Add(Radius_Ulna_Interaction);


			//relate objects to each other
			Humerus_R.AddInteraction(Elbow_Joint);
			Radius_R.AddInteraction(Elbow_Joint);
			Ulna_R.AddInteraction(Elbow_Joint);

			Radius_R.AddInteraction(Radius_Ulna_Interaction);
			Ulna_R.AddInteraction(Radius_Ulna_Interaction);
		}
	}

	
}
