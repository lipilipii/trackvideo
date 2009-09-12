//*******************************************************************
/*

	Solution:	TrackVideo
	Project:	TrackVideoApp
	File:		RMainModule.cs

	Copyright 2009, Raphael MOLL.

	This file is part of TrackVideo.

	TrackVideo is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	TrackVideo is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with TrackVideo; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

*/
//*******************************************************************



using System;
using System.Windows.Forms;

using Alfray.LibUtils2.Misc;

//*************************************
namespace Alfray.TrackVideo.TrackVideoApp
{
	//***************************************************
	/// <summary>
	/// Summary description for RMainModule.
	/// </summary>
	public class MainModule
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//**********************
		public static Pref Pref
		{
			get
			{
				return mMainMod.mPref;
			}
		}


		//******************************
		public static MainForm MainForm
		{
			get
			{
				return MainModule.mMainForm;
			}
		}

		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//****************
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		//****************
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();

			mMainMod = new MainModule();
			mMainForm = new MainForm();

			Application.Run(mMainForm);
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private static MainForm mMainForm;
		private static MainModule mMainMod;
		private Pref mPref = new Pref();

	} // class RMainModule
} // namespace Alfray.TrackVideo.TrackVideoApp
