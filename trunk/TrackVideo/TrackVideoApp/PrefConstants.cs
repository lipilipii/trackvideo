//*******************************************************************
/*

	Solution:	TrackVideo
	Project:	TrackVideoApp
	File:		RPrefConstants.cs

	Copyright 2003, 2004, Raphael MOLL.

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

//*************************************
namespace Alfray.TrackVideo.TrackVideoApp
{
	//***************************************************
	/// <summary>
	/// Summary description for RPrefConstants.
	/// </summary>
	public class PrefConstants
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------

		// UI

		public const string kMainForm = "forms-main";
		public const string kPrefForm = "forms-pref";
        public const string kDebugForm = "forms-debug";
        public const string kPreviewForm = "forms-preview";

        public const string kLastKmxPath  = "last-kmx-path";
        public const string kLastDestPath = "last-dest-path";
        public const string kLastFps      = "last-fps";
        public const string kLastMovieSx  = "last-movie-sx";
        public const string kLastMovieSy  = "last-movie-sy";
        public const string kLastTrackSx  = "last-track-sx";
        public const string kLastTrackSy  = "last-track-sy";


	} // class RPrefConstants
} // namespace Alfray.TrackVideo.TrackVideoApp

