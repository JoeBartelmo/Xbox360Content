/*  Copyright (C) 2012 Joseph Bartelmo

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

namespace Xbox360Content.STFS.Enums
{
    /// <summary>
    /// STFS Signature ("CON ", Live, Pirs)
    /// </summary>
    public enum Signature : uint
    {
        /// <summary>
        /// Signed by a console.
        /// </summary>
        Con = 1279874629u,
        /// <summary>
        /// Signed by Microsoft [offline].
        /// </summary>
        Pirs = 1346982483u,
        /// <summary>
        /// Signed by the Marketplace.
        /// </summary>
        Live = 1279874629u
    }
    /// <summary>
    /// Console type identification in the signature.
    /// </summary>
    public enum ConsoleType : byte
    {
        /// <summary>
        /// Constant dev kit.
        /// </summary>
        DevKit = 0x01,
        /// <summary>
        /// Constant retail.
        /// </summary>
        Retail = 0x02
    }
    /// <summary>
    /// For use with DiscInfo
    /// </summary>
    public enum Disc : uint
    {
        /// <summary>
        /// Identifies Xbox or PC
        /// </summary>
        Platform = 0xff000000,
        /// <summary>
        /// Executable type
        /// </summary>
        Executable = 0x00ff0000,
        /// <summary>
        /// Disc number
        /// </summary>
        Number = 0x0000ff00,
        /// <summary>
        /// Disc number in included set (series)
        /// </summary>
        ID = 0x000000ff
    }
}
