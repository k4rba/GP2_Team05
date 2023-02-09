// DAzBjax (2015), if you have same questions contact me at DAzBjax.Unity@mail.ru 

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Reflection;

[ExecuteInEditMode]
public class DAX_MultiObjectsRenamer : EditorWindow 
{ 
	const string MenuItemSTR = "Tools/Multi objects rename tool";
	const string TabCaptionSTR = "MO Rename";

	Object[] findedObjects;
	bool[] exportedObjects;
		
	bool tOnlyROOT = true;
	bool tShowResult = true;
	string ReplaceFrom = "";
	string ReplaceTo = "";		

	int lastErrorsCount;	

	Vector2 scrollVector;

	byte[] pngBytesEYE = new byte[] {137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 19, 0, 0, 0, 19, 8, 6, 0, 0, 0, 114, 80, 54, 204, 0, 0, 0, 9, 112, 72, 89, 115, 0, 0, 11, 19, 0, 0, 11, 19, 1, 0, 154, 156, 24, 0, 0, 0, 32, 99, 72, 82, 77, 0, 0, 122, 37, 0, 0, 128, 131, 0, 0, 249, 255, 0, 0, 128, 232, 0, 0, 82, 8, 0, 1, 21, 88, 0, 0, 58, 151, 0, 0, 23, 111, 215, 90, 31, 144, 0, 0, 2, 76, 73, 68, 65, 84, 120, 218, 228, 148, 203, 79, 19, 97, 20, 197, 207, 204, 180, 196, 118, 58, 51, 116, 35, 76, 37, 33, 64, 89, 72, 210, 198, 134, 146, 144, 82, 22, 117, 129, 16, 68, 23, 60, 130, 91, 82, 170, 136, 11, 18, 12, 113, 217, 53, 143, 45, 255, 129, 134, 242, 90, 104, 13, 77, 109, 40, 144, 144, 52, 145, 202, 163, 6, 215, 26, 211, 84, 87, 246, 49, 44, 128, 249, 174, 11, 165, 97, 120, 24, 22, 221, 121, 183, 223, 239, 158, 197, 119, 206, 185, 28, 17, 161, 82, 195, 163, 130, 83, 81, 49, 78, 177, 59, 175, 123, 107, 3, 48, 12, 192, 3, 64, 2, 80, 4, 176, 7, 224, 53, 128, 244, 141, 196, 24, 35, 23, 17, 91, 240, 122, 239, 181, 12, 244, 247, 193, 237, 186, 11, 155, 36, 162, 84, 212, 144, 249, 252, 5, 145, 197, 183, 216, 217, 217, 59, 52, 153, 132, 97, 142, 227, 50, 134, 101, 197, 238, 132, 98, 119, 66, 174, 110, 178, 112, 66, 205, 98, 71, 231, 67, 138, 197, 214, 233, 228, 228, 148, 174, 26, 198, 24, 37, 18, 91, 228, 243, 247, 18, 39, 212, 44, 201, 213, 77, 150, 51, 13, 40, 118, 39, 108, 114, 131, 200, 155, 213, 200, 104, 104, 146, 24, 99, 229, 197, 100, 114, 155, 6, 135, 130, 164, 222, 113, 211, 224, 80, 144, 146, 201, 109, 131, 112, 232, 217, 75, 18, 204, 106, 196, 38, 55, 136, 138, 221, 9, 72, 74, 163, 96, 170, 114, 76, 143, 4, 39, 12, 96, 42, 149, 166, 177, 241, 41, 90, 89, 141, 146, 166, 105, 180, 178, 26, 165, 177, 241, 41, 74, 165, 210, 6, 110, 36, 56, 65, 166, 42, 199, 180, 164, 52, 10, 16, 204, 106, 115, 187, 175, 135, 242, 249, 130, 1, 10, 135, 103, 104, 118, 110, 158, 116, 93, 39, 34, 34, 93, 215, 105, 118, 110, 158, 194, 225, 25, 3, 151, 207, 23, 168, 221, 215, 67, 130, 89, 109, 230, 69, 209, 26, 120, 208, 21, 128, 44, 75, 134, 191, 76, 239, 30, 192, 225, 168, 5, 207, 255, 73, 15, 207, 243, 112, 56, 106, 145, 222, 61, 48, 112, 178, 44, 161, 187, 235, 62, 68, 209, 26, 224, 53, 237, 104, 35, 30, 223, 64, 161, 80, 52, 64, 173, 30, 55, 178, 217, 28, 24, 99, 127, 93, 102, 200, 102, 115, 104, 245, 184, 13, 92, 161, 80, 68, 44, 190, 14, 77, 59, 74, 10, 54, 233, 246, 175, 175, 223, 190, 91, 114, 63, 126, 118, 60, 126, 212, 93, 134, 44, 150, 91, 72, 110, 110, 227, 248, 248, 4, 245, 245, 117, 136, 190, 255, 128, 244, 167, 125, 12, 244, 247, 161, 174, 78, 45, 115, 207, 95, 188, 194, 218, 218, 250, 140, 213, 106, 89, 46, 187, 41, 152, 213, 72, 232, 233, 205, 221, 100, 140, 209, 104, 104, 146, 248, 243, 110, 94, 204, 153, 207, 223, 75, 137, 196, 150, 65, 244, 114, 206, 54, 201, 223, 217, 71, 188, 169, 118, 241, 124, 206, 46, 53, 128, 136, 92, 167, 167, 250, 66, 91, 155, 167, 101, 112, 224, 172, 1, 54, 148, 138, 37, 236, 103, 14, 177, 180, 252, 14, 59, 31, 175, 110, 192, 191, 186, 233, 5, 240, 228, 66, 55, 119, 1, 188, 185, 182, 155, 255, 199, 61, 251, 61, 0, 216, 84, 140, 133, 246, 8, 144, 48, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 };
	Texture2D eyeTex;
	
	[MenuItem(MenuItemSTR)]
	static void Init () 
	{
		// Get existing open window or if none, make a new one:
		DAX_MultiObjectsRenamer window = (DAX_MultiObjectsRenamer)EditorWindow.GetWindow (typeof (DAX_MultiObjectsRenamer));
		window.title = TabCaptionSTR;
		window.Show();
	}

	void updateInfo( bool loadSelection = false )
	{
		try{
		findedObjects = new Object[0];
		if (loadSelection) 
		{
			findedObjects = Selection.objects;
		}
		else
		{
			findedObjects = GameObject.FindObjectsOfType (typeof(GameObject));
		}
		
		if ( findedObjects != null )
		{
			Object[] tfObj;
			int maxSize = 0;
			for (int i = 0; i < findedObjects.Length; i++) 
			{
				if (!tOnlyROOT)
				{
					maxSize++;
				}
				else if ((findedObjects[i] as GameObject).transform.parent == null)
				{
					maxSize++;
				}
			}
	
			tfObj = new Object[maxSize];
			int curItem = 0;
			for (int i = 0; i < findedObjects.Length; i++) 
			{
				if (!tOnlyROOT | ((findedObjects[i] as GameObject).transform.parent == null) )
				{
					tfObj[curItem] = findedObjects[i];
					curItem++;
				}
			}
			findedObjects = tfObj;	
	
			exportedObjects = new bool[findedObjects.Length];
			for(int i = 0; i < findedObjects.Length; i++) 
			{
				exportedObjects[i] = true;
			}
		}
		}catch{};
	}

	void OnGUI () 
	{
		try {
		GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ();
		
			EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Find:", GUILayout.MaxWidth(75.0f));
				ReplaceFrom = EditorGUILayout.TextField( ReplaceFrom );		
			EditorGUILayout.EndHorizontal();
		
			EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Replace to:", GUILayout.MaxWidth(75.0f));
				ReplaceTo = EditorGUILayout.TextField( ReplaceTo );		
			EditorGUILayout.EndHorizontal();
		

		EditorGUILayout.EndVertical ();
		
		EditorGUILayout.Separator();
		
		EditorGUILayout.BeginHorizontal ();
		
			EditorGUILayout.BeginVertical();
			#if UNITY_5_0
				tOnlyROOT = EditorGUILayout.ToggleLeft( "Root Objects", tOnlyROOT, GUILayout.MaxWidth(115.0f) );
				tShowResult = EditorGUILayout.ToggleLeft( "Show result", tShowResult, GUILayout.MaxWidth(115.0f) );
			#else
				tOnlyROOT = EditorGUILayout.Toggle( "Root Objects", tOnlyROOT );
				tShowResult = EditorGUILayout.Toggle( "Show result", tShowResult );
			#endif
			
			EditorGUILayout.EndVertical();

			
				if(GUILayout.RepeatButton("Load ALL", GUILayout.MaxWidth(70.0f)))
				{
					updateInfo( false );
				}
				if(GUILayout.RepeatButton("Load Selection", GUILayout.MaxWidth(95.0f)))
				{
					updateInfo( true );
				}

		EditorGUILayout.EndHorizontal ();
		
		GUILayout.BeginHorizontal(GUI.skin.box, GUILayout.MaxHeight(400.0f));
		
		if (eyeTex==null) //no eye lodaded
		{
			eyeTex = new Texture2D(2,2);
			eyeTex.LoadImage( pngBytesEYE ); //load eye
		}	
		
			scrollVector = GUILayout.BeginScrollView(scrollVector);
			lastErrorsCount = 0;
				if (findedObjects != null) 
				{
					for (int i = 0; i < findedObjects.Length; i++) 
					{
						if (findedObjects [i] != null)
						{		
							try
							{			
								EditorGUILayout.BeginHorizontal ();	
																	
									if (GUILayout.Button( eyeTex, GUI.skin.label, GUILayout.MaxHeight(18.0f), GUILayout.MaxWidth( 20.0f )   )) //draw asterix
									{
										EditorGUIUtility.PingObject( findedObjects [i] );
									}	
									
									try 
									{		
										if ((ReplaceFrom!="") & (tShowResult) & (exportedObjects [i]))
										{
											exportedObjects [i] = EditorGUILayout.BeginToggleGroup (findedObjects [i].name.Replace( ReplaceFrom, ReplaceTo ), exportedObjects [i]); //checkboxes left
											EditorGUILayout.EndToggleGroup ();
										}else
										{
											exportedObjects [i] = EditorGUILayout.BeginToggleGroup (findedObjects [i].name, exportedObjects [i]); //checkboxes left
											EditorGUILayout.EndToggleGroup ();
										}
									}catch{};
								EditorGUILayout.EndHorizontal ();
							}catch{};
						}		
					}
				}
			GUILayout.EndScrollView();
		GUILayout.EndHorizontal();


		EditorGUILayout.BeginHorizontal (GUI.skin.box);

			if (GUILayout.Button ("Rename Objects")) 
			{
				if (findedObjects != null) 
				{
					for (int i = 0; i < findedObjects.Length; i++) 
					{
						if (findedObjects [i] != null)
						{		
							try
							{	
								if ((ReplaceFrom!="") & (exportedObjects [i]))
								{
									findedObjects [i].name = findedObjects [i].name.Replace( ReplaceFrom, ReplaceTo );
								}
							}	
							catch{};
						}
					}
				}
			}
			
			GUILayout.FlexibleSpace();
			
			if (GUILayout.Button ("Cancel")) 
			{
				DAX_MultiObjectsRenamer window = (DAX_MultiObjectsRenamer)EditorWindow.GetWindow (typeof (DAX_MultiObjectsRenamer));
				window.Close();
			}

		EditorGUILayout.EndHorizontal ();
		}catch{};
	
	}
}