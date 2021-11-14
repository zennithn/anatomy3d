using Godot;
using System;
using System.Collections.Generic;
using GlmSharp;

using Anatomy = FreedomOfFormFoundation.AnatomyEngine.Anatomy;
using FreedomOfFormFoundation.AnatomyEngine.Geometry;
using FreedomOfFormFoundation.AnatomyEngine.Calculus;
using FreedomOfFormFoundation.AnatomyEngine.Renderable;

namespace FreedomOfFormFoundation.AnatomyRenderer
{
	public class ExampleBone : MeshInstance
	{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			// Example method that creates a character and adds a single joint and bone:
			Anatomy.Skeleton skeleton = new Anatomy.Skeleton();
			
			CreateExampleJoint(skeleton);
			CreateExampleBones(skeleton);
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			
		}
		
		public void CreateExampleBones(Anatomy.Skeleton skeleton)
		{
			// Generate a simple cubic spline that will act as the radius of a long bone:
			SortedList<double, double> radiusPoints = new SortedList<double, double>();
			radiusPoints.Add(-3.5d, 0.7f*1.2d);
			radiusPoints.Add(-1.0d, 0.7f*1.2d);
			radiusPoints.Add(0.02d, 0.7f*1.2d);
			radiusPoints.Add(0.15d, 0.7f*1.0d);
			radiusPoints.Add(0.5d, 0.7f*0.7d);
			radiusPoints.Add(0.8d, 0.7f*0.76d);
			radiusPoints.Add(0.98d, 0.7f*0.8d);
			radiusPoints.Add(4.5d, 0.7f*0.8d);
			
			LinearSpline1D boneRadius = new LinearSpline1D(radiusPoints);

			// Define the center curve of the long bone:
			SortedList<double, dvec3> centerPoints = new SortedList<double, dvec3>();
			centerPoints.Add(0.0d, new dvec3(0.0d, 0.0d, 2.7d));
			centerPoints.Add(0.25d, new dvec3(-0.3d, -0.5d, 1.0d));
			centerPoints.Add(0.5d, new dvec3(0.3d, 1.0d, 0.0d));
			centerPoints.Add(0.75d, new dvec3(0.8d, 1.0d, -1.0d));
			centerPoints.Add(1.0d, new dvec3(0.6d, -0.5d, -0.9d));
			
			SpatialCubicSpline boneCenter = new SpatialCubicSpline(centerPoints);
			
			// Add first bone:
			LineSegment centerLine = new LineSegment(new dvec3(0.0d, 0.0d, 0.5d),
									   new dvec3(0.001d, 10.0d, 0.51d));
			
			var bone1 = new Anatomy.Bones.LongBone(centerLine, boneRadius);
			
			var jointInteraction = new Anatomy.Bone.JointDeformation(skeleton.joints[0], RayCastDirection.Outwards, 3.0f);
			bone1.InteractingJoints.Add(jointInteraction);
			skeleton.bones.Add(bone1);
			
			// Add second bone:
			//LineSegment centerLine2 = new LineSegment(new dvec3(0.0d, -10.0d, 0.5d),
			//						   new dvec3(0.001d, -1.0d, 0.51d));
			
			//var bone2 = new Anatomy.Bones.LongBone(centerLine2, 1.1d);
			//bone2.InteractingJoints.Add((skeleton.joints[0], RayCastDirection.Outwards, 3.0d));
			//skeleton.bones.Add(bone2);
			
			// Generate the geometry vertices of the first bone with resolution U=128 and resolution V=128:
			foreach ( var bone in skeleton.bones )
			{
				UVMesh mesh = bone.GetGeometry().GenerateMesh(256, 256);
				
				// Finally upload the mesh to Godot:
				MeshInstance newMesh = new MeshInstance();
				newMesh.Mesh = new GodotMeshConverter(mesh);
				
				// Give each mesh a random color:
				var boneMaterial = (SpatialMaterial)GD.Load("res://BoneMaterial.tres").Duplicate();
				boneMaterial.AlbedoColor = new Color(GD.Randf(), GD.Randf(), GD.Randf(), GD.Randf());
				newMesh.SetSurfaceMaterial(0, boneMaterial);
				
				AddChild(newMesh);
			}
		}
		
		public void CreateExampleJoint(Anatomy.Skeleton skeleton)
		{
			// Generate a simple cubic spline that will act as the radius of a long bone:
			SortedList<double, double> splinePoints = new SortedList<double, double>();
			double radiusModifier = 0.6f;
			splinePoints.Add(-0.1d, radiusModifier*1.1d);
			splinePoints.Add(0.0d, radiusModifier*1.1d);
			splinePoints.Add(0.15d, radiusModifier*0.95d);
			splinePoints.Add(0.3d, radiusModifier*0.9d);
			splinePoints.Add(0.5d, radiusModifier*1.15d);
			splinePoints.Add(0.7d, radiusModifier*0.95d);
			splinePoints.Add(0.8d, radiusModifier*0.95d);
			splinePoints.Add(1.0d, radiusModifier*1.1d);
			
			QuadraticSpline1D jointSpline = new QuadraticSpline1D(splinePoints);

			// Define the center curve of the long bone:
			LineSegment centerLine = new LineSegment(new dvec3(0.0d, -0.5d, 0.0d),
									   new dvec3(0.0d, 1.5d, 0.5d));
			
			// Add a long bone to the character:
			skeleton.joints.Add(new Anatomy.Joints.HingeJoint(centerLine, jointSpline, 0.0d, 1.0f*(double)Math.PI));
			
			// Generate the geometry vertices of the first bone with resolution U=32 and resolution V=32:
			UVMesh mesh = skeleton.joints[0].GetArticularSurface().GenerateMesh(64, 64);
			
			// Finally upload the mesh to Godot:
			MeshInstance newMesh = new MeshInstance();
			newMesh.Mesh = new GodotMeshConverter(mesh);
			newMesh.SetSurfaceMaterial(0, (Material)GD.Load("res://JointMaterial.tres"));
			
			AddChild(newMesh);
		}
	}
}
