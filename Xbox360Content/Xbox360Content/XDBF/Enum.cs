/*  Copyright (C) 2013 Joseph Bartelmo

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360Content.XDBF.Enums
{
    public enum GPDID : ushort
    {
        Achievement = 1, 
        Image = 2,
        Setting = 3,
        Title = 4,
        String = 5,
        AvatarAward = 6
    }
    public enum SPAID : ushort
    {
        Metadata = 1,
        Images = 2,
        StringTable = 3
    }
    public enum GPDData : uint
    {
        SyncList = 0x10000000,
        SyncData = 0x20000000,
        OptionControllerVibration = 0x10040003,
        TitleSpecific1 = 0x63E83FFF,
        TitleSpecific2 = 0x63E83FFE,
        TitleSpecific3 = 0x63E83FFD,
        GamerYAxisInversion = 0x10040002,
        GamercardZone = 0x10040004,
        GamercardRegion = 0x10040005,
        GamercardCred = 0x10040006,
        GamercardRep = 0x50040011,
        OptionVoiceMuted = 0x10040012,
        OptionVoiceThruSpeakers = 0x10040013,
        OptionVoiceThruSpeakersRaw = 0x10040063,
        OptionVoiceVolume = 0x10040014,
        GamercardTitlesPlayed = 0x10040018,
        GamercardAchievementsEarned = 0x10040019,
        GamerDifficulty = 0x10040021,
        GamerControlSensitivity = 0x10040024,
        GamerPreferredColorFirst = 0x10040029,
        GamerPreferredColorSecond = 0x10040030,
        GamerActionAutoAim = 0x10040034,
        GamerActionAutoCenter = 0x10040035,
        GamerActionMovementControl = 0x10040036,
        GamerRaceActionTransmission = 0x10040038,
        GamerRaceCameraLocation = 0x10040039,
        GamerRaceBrakeControl = 0x10040040,
        GamerRaceAcceleratorcontrol = 0x10040041,
        GamercardTitleCredEarned = 0x10040056,
        GamercardTitleAchievementsearned = 0x10040057,
        AvatarMetadata = 0x63E80068,
        GamercardPictureKey = 0x4064000F,
        GamercardMotto = 0x402C0011,
        TittleInformation = 0x8000,
        GamerName = 0x41040040,
        GamerLocation = 0x40520041,
        AvatarInformation = 0x63e80044,
        AvatarImage = 0x8007
    }
}
